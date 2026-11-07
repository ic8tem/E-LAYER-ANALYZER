#!/usr/bin/env python3
# YA-MS - rigenera data/lingue/chiavi.txt: elenco di tutti i testi italiani da tradurre.
#
# Cerca in src/**/*.cs le chiamate Lang.T("...") e Lang.F("...") con una stringa letterale (anche
# piu' letterali unite con +), toglie gli escape del C# (\" \\ \n \t \uXXXX ...) e aggiunge i testi
# che arrivano dai dati (nomi degli sciami di data/sciami.csv e le voci di EXTRA).
# Nel file gli a capo sono scritti come \n (due caratteri), una chiave per riga, in ordine, senza doppioni.
#
# Uso:  python3 tools/estrai_chiavi.py              rigenera data/lingue/chiavi.txt
#       python3 tools/estrai_chiavi.py --controlla  elenca anche, per ogni lingua, le chiavi non ancora tradotte
import glob
import os
import re
import sys

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
OUT = os.path.join(ROOT, "data", "lingue", "chiavi.txt")
LANGS = ["en", "ru", "ja", "de", "nl"]

# testi passati a Lang.T come variabili (valori che arrivano dai dati o dal modello)
EXTRA = [
    "Sorgente Antielio",                          # Catalog.cs: nome dello sciame ANT
    "Hot A", "Hot B", "Limite A", "Limite B",     # MsModel.cs: HotPoint.Label
]

# argomenti non letterali ammessi (i loro valori stanno nei nomi degli sciami o in EXTRA)
KNOWN_DYNAMIC = {"s.Name", "h.Label", "rh.Label"}

HEADER = """# YA-MS - chiavi da tradurre (testi italiani dell'interfaccia). File generato da tools/estrai_chiavi.py: non modificare a mano.
# Ogni file di traduzione data/lingue/<codice>.txt (UTF-8) ha una voce per riga:
#     <chiave italiana><TAB><traduzione>
# Le righe che iniziano con # sono commenti. In entrambe le colonne la sequenza \\n (due caratteri) indica un a capo.
# I segnaposto {0}, {1}, ... (anche con formato, es. {0:F1}, {3:+0;-0}) vanno mantenuti nella traduzione, nell'ordine che serve alla lingua.
# Non tradurre: sigle degli sciami, nominativi, locator, unita' di misura (km, dB, MHz, λ), nomi propri, nomi di file.
# Voce mancante o traduzione vuota: il programma mostra il testo italiano.
"""

ESC = {'"': '"', "\\": "\\", "'": "'", "n": "\n", "t": "\t", "r": "\r", "0": "\0", "a": "\a", "b": "\b", "f": "\f", "v": "\v"}


def read_literal(s, i):
    """Legge una stringa letterale C# che inizia in s[i]. Restituisce (testo, indice dopo la stringa) o (None, i)."""
    if s.startswith('@"', i):
        j = i + 2
        out = []
        while j < len(s):
            if s[j] == '"':
                if j + 1 < len(s) and s[j + 1] == '"':
                    out.append('"'); j += 2; continue
                return "".join(out), j + 1
            out.append(s[j]); j += 1
        return None, i
    if i >= len(s) or s[i] != '"':
        return None, i
    j = i + 1
    out = []
    while j < len(s):
        c = s[j]
        if c == "\\":
            n = s[j + 1]
            if n == "u":
                out.append(chr(int(s[j + 2:j + 6], 16))); j += 6; continue
            if n == "x":
                m = re.match(r"[0-9a-fA-F]{1,4}", s[j + 2:])
                out.append(chr(int(m.group(0), 16))); j += 2 + len(m.group(0)); continue
            out.append(ESC.get(n, n)); j += 2; continue
        if c == '"':
            return "".join(out), j + 1
        if c == "\n":
            return None, i
        out.append(c); j += 1
    return None, i


def skip_ws_comments(s, i):
    while i < len(s):
        if s[i].isspace():
            i += 1
        elif s.startswith("//", i):
            k = s.find("\n", i)
            i = len(s) if k < 0 else k + 1
        elif s.startswith("/*", i):
            k = s.find("*/", i)
            i = len(s) if k < 0 else k + 2
        else:
            break
    return i


def scan_file(path, keys, warnings):
    s = open(path, encoding="utf-8-sig").read()
    for m in re.finditer(r"\bLang\.(T|F)\s*\(", s):
        i = skip_ws_comments(s, m.end())
        parts = []
        while True:
            lit, j = read_literal(s, i)
            if lit is None:
                break
            parts.append(lit)
            i = skip_ws_comments(s, j)
            if i < len(s) and s[i] == "+":
                k = skip_ws_comments(s, i + 1)
                if k < len(s) and (s[k] == '"' or s.startswith('@"', k)):
                    i = k
                    continue
                parts = None   # letterale + variabile: non e' una chiave fissa
            break
        line = s.count("\n", 0, m.start()) + 1
        rel = os.path.relpath(path, ROOT)
        if not parts:
            if parts is not None:
                k = s.find(")", i)
                if s[i:k].strip() in KNOWN_DYNAMIC:
                    continue
            warnings.append("%s:%d: Lang.%s con testo non letterale (va elencato in EXTRA se arriva dai dati)" % (rel, line, m.group(1)))
            continue
        if i < len(s) and s[i] not in ",)":
            warnings.append("%s:%d: Lang.%s: dopo la stringa c'e' '%s' (chiave forse incompleta)" % (rel, line, m.group(1), s[i]))
        keys.add("".join(parts))


def shower_names():
    names = []
    f = os.path.join(ROOT, "data", "sciami.csv")
    if not os.path.exists(f):
        return names
    for raw in open(f, encoding="utf-8-sig"):
        l = raw.strip()
        if not l or l.startswith("#") or l.startswith("codice"):
            continue
        c = l.split(";")
        if len(c) >= 15 and c[1].strip():
            names.append(c[1].strip())
    return names


def escape(k):
    return k.replace("\r\n", "\n").replace("\n", "\\n")


def unescape(k):
    return k.replace("\\n", "\n")


def main():
    keys = set()
    warnings = []
    for path in sorted(glob.glob(os.path.join(ROOT, "src", "**", "*.cs"), recursive=True)):
        scan_file(path, keys, warnings)
    n_code = len(keys)
    for n in shower_names():
        keys.add(n)
    for e in EXTRA:
        keys.add(e)
    keys.discard("")
    for k in keys:
        if "\t" in k:
            warnings.append("chiave con TAB (non ammessa): %r" % k)
    lines = sorted(escape(k) for k in keys)
    os.makedirs(os.path.dirname(OUT), exist_ok=True)
    with open(OUT, "w", encoding="utf-8", newline="\n") as f:
        f.write(HEADER)
        for l in lines:
            f.write(l + "\n")
    print("%s: %d chiavi (%d dal codice, piu' nomi degli sciami ed EXTRA)" % (os.path.relpath(OUT, ROOT), len(lines), n_code))
    for w in warnings:
        print("ATTENZIONE " + w)

    if "--controlla" in sys.argv:
        for code in LANGS:
            f = os.path.join(ROOT, "data", "lingue", code + ".txt")
            done = {}
            if os.path.exists(f):
                for raw in open(f, encoding="utf-8-sig"):
                    raw = raw.rstrip("\r\n")
                    if not raw or raw.startswith("#") or "\t" not in raw:
                        continue
                    k, v = raw.split("\t", 1)
                    if v.strip():
                        done[unescape(k)] = v
            missing = [escape(k) for k in sorted(keys) if k not in done]
            extra = [escape(k) for k in sorted(done) if k not in keys]
            print("%s: %d tradotte, %d mancanti, %d chiavi non piu' usate" % (code, len(keys) - len(missing), len(missing), len(extra)))
            for k in extra:
                print("   non usata: " + k)


if __name__ == "__main__":
    main()
