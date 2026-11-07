// YA-MS - traduzione dell'interfaccia. L'italiano e' la lingua di partenza: le chiavi sono i testi italiani.
// File di traduzione: data/lingue/<codice>.txt (UTF-8), una voce per riga: <chiave italiana><TAB><traduzione>.
// Le righe che iniziano con # sono commenti; in entrambe le colonne la sequenza \n indica un a capo.
// Voce o file mancante: si mostra il testo italiano.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace YAMS
{
    public static class Lang
    {
        public static string Code = "it";

        // codice, nome nella lingua stessa
        public static readonly string[][] Languages =
        {
            new[] { "it", "Italiano" },
            new[] { "en", "English" },
            new[] { "ru", "Русский" },
            new[] { "ja", "日本語" },
            new[] { "de", "Deutsch" },
            new[] { "nl", "Nederlands" }
        };

        // cultura per numeri e date di ogni lingua
        static readonly string[][] Cultures =
        {
            new[] { "it", "it-IT" }, new[] { "en", "en-GB" }, new[] { "ru", "ru-RU" },
            new[] { "ja", "ja-JP" }, new[] { "de", "de-DE" }, new[] { "nl", "nl-NL" }
        };

        // il dizionario viene sostituito per intero (lettura sicura anche dai thread secondari)
        static Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.Ordinal);

        public static int IndexOf(string code)
        {
            for (int i = 0; i < Languages.Length; i++) if (Languages[i][0] == code) return i;
            return -1;
        }

        // Lingua all'avvio: quella salvata in config.ini (chiave "lingua"); al primo avvio quella del sistema, se prevista, altrimenti l'italiano
        public static void Init(string saved)
        {
            string code = (saved ?? "").Trim().ToLowerInvariant();
            if (IndexOf(code) < 0)
            {
                code = "it";
                try
                {
                    string os = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
                    if (IndexOf(os) >= 0) code = os;
                }
                catch { }
            }
            Load(code);
            ApplyCulture();
        }

        // Carica la lingua (per "it" nessun file: si usano le chiavi)
        public static void Load(string code)
        {
            if (IndexOf(code) < 0) code = "it";
            Dictionary<string, string> m = new Dictionary<string, string>(StringComparer.Ordinal);
            if (code != "it")
            {
                string file = FindFile(code);
                if (file != null)
                {
                    try
                    {
                        foreach (string raw in File.ReadAllLines(file, Encoding.UTF8))
                        {
                            string l = raw.TrimEnd('\r');
                            if (l.Length > 0 && l[0] == '﻿') l = l.Substring(1);
                            if (l.Length == 0 || l.StartsWith("#")) continue;
                            int tab = l.IndexOf('\t');
                            if (tab <= 0) continue;
                            string key = Unescape(l.Substring(0, tab));
                            string val = Unescape(l.Substring(tab + 1));
                            if (val.Trim().Length == 0) continue;   // traduzione vuota: resta l'italiano
                            m[key] = val;
                        }
                    }
                    catch { }
                }
            }
            map = m;
            Code = code;
        }

        static string FindFile(string code)
        {
            string[] dirs =
            {
                Path.Combine(AppPaths.DataDir, "lingue"),
                Path.Combine(AppContext.BaseDirectory, "data", "lingue"),
                Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "Resources", "data", "lingue"))
            };
            foreach (string d in dirs)
            {
                try
                {
                    string f = Path.Combine(d, code + ".txt");
                    if (File.Exists(f)) return f;
                }
                catch { }
            }
            return null;
        }

        // la sequenza di due caratteri \n diventa un a capo
        public static string Unescape(string s) { return s.Replace("\\n", "\n"); }
        public static string Escape(string s) { return s.Replace("\r\n", "\n").Replace("\n", "\\n"); }

        // Testo tradotto (o l'italiano se manca la traduzione)
        public static string T(string it)
        {
            if (string.IsNullOrEmpty(it)) return it;
            string t;
            Dictionary<string, string> m = map;
            return m.TryGetValue(it, out t) ? t : it;
        }

        // Testo tradotto con segnaposto {0}, {1}, ... (numeri e date secondo la cultura della lingua scelta)
        public static string F(string it, params object[] args)
        {
            string t = T(it);
            try { return string.Format(CultureInfo.CurrentCulture, t, args); }
            catch (FormatException)
            {
                // traduzione con segnaposto sbagliati: si usa il testo italiano
                try { return string.Format(CultureInfo.CurrentCulture, it, args); } catch (FormatException) { return it; }
            }
        }

        // Cultura per numeri e date della lingua scelta (anche per i thread secondari).
        // La lettura dei file di dati usa sempre InvariantCulture.
        public static void ApplyCulture()
        {
            string name = "it-IT";
            foreach (string[] c in Cultures) if (c[0] == Code) name = c[1];
            try
            {
                CultureInfo ci = CultureInfo.GetCultureInfo(name);
                CultureInfo.CurrentCulture = ci;
                CultureInfo.CurrentUICulture = ci;
                CultureInfo.DefaultThreadCurrentCulture = ci;
                CultureInfo.DefaultThreadCurrentUICulture = ci;
            }
            catch { }
        }
    }
}
