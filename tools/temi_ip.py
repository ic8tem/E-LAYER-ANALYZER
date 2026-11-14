# E-LAYER ANALYZER - aggiunge agli schemi di colori di YA-MS (src/MeteorScatter/UI/Theme.cs) i sei temi di IP-TRX-CTRL
# (src/IpTrxCtrl/UI/Themes.cs: Chiaro, Notte, Nero, Blu, Viola, Verde; Blu, Viola e Verde sono anche i colori in piu' di
# YAEC). Richiesta dell'autore, 14/11/2026. Si richiama da tools/sync_yams.py dopo ogni allineamento a YA-MS; se il file
# ha gia' i temi non fa nulla. Ogni modifica controlla di aver trovato il suo testo.
import os, sys

EL = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
P = os.path.join(EL, 'src', 'MeteorScatter', 'UI', 'Theme.cs')

def rep(s, a, b, what):
    if s.count(a) != 1: raise SystemExit('temi_ip.py: NON TROVATO (' + what + '): ' + a[:80])
    return s.replace(a, b)

def main():
    s = open(P, encoding='utf-8-sig').read()
    if 'IpChiave' in s:
        print('temi IP-TRX-CTRL: gia\' presenti'); return

    s = rep(s, '''            Color.FromRgb(230, 110, 160), Color.FromRgb(160, 120, 210), Color.FromRgb(80, 90, 190), Color.FromRgb(150, 105, 70) };
''', '''            Color.FromRgb(230, 110, 160), Color.FromRgb(160, 120, 210), Color.FromRgb(80, 90, 190), Color.FromRgb(150, 105, 70) };

        // E-LAYER ANALYZER (tools/temi_ip.py): i sei temi di IP-TRX-CTRL (UI/Themes.cs), dopo la Scia meteorica.
        // Blu, Viola e Verde sono anche i colori in piu' di YAEC (Tema.IpColori), con gli stessi valori.
        public static readonly string[] IpChiave = { "chiaro", "notte", "nero", "blu", "viola", "verde" };
        public static readonly string[] IpNome = { "Chiaro (IP-TRX-CTRL)", "Notte (IP-TRX-CTRL)", "Nero (IP-TRX-CTRL)", "Blu", "Viola", "Verde" };
        public static readonly bool[] IpScuro = { false, true, true, true, true, true };
        // per ogni tema: Bg, Panel, Inset, Border, Field, Text, Muted, Btn1, Btn2, BtnText, BtnBorder, Accent, AccentText,
        // QrgBg, Qrg, Ok, Bar, Track, Tx, TxDim, LogBg, LogText, LogSend, LogRecv, LogErr, LogMuted (come in IP-TRX-CTRL)
        public static readonly string[][] IpColori = {
            new[] { "#F5F7FA", "#FFFFFF", "#F0F2F5", "#D5DAE1", "#FFFFFF", "#1B1F24", "#555D68", "#FDFEFF", "#E3E7ED", "#1B1F24", "#B9C0CA", "#1F6FEB", "#FFFFFF",
                    "#141A22", "#F5B942", "#1E7A3C", "#2E9E55", "#DDE1E7", "#D92D20", "#F3C2BD", "#141A22", "#D7DEE8", "#7FB3FF", "#8EE0A6", "#FF8A80", "#8C97A6" },
            new[] { "#0F1A2B", "#15233A", "#0B1526", "#2A3D5C", "#0B1526", "#E6ECF5", "#A9B6C9", "#2E4870", "#1F3354", "#FFFFFF", "#3B5682", "#5AA2FF", "#0B1526",
                    "#070E19", "#F5B942", "#8EE0A6", "#4CC774", "#26364F", "#FF4D4F", "#5A2A33", "#070E19", "#D7DEE8", "#7FB3FF", "#8EE0A6", "#FF8A80", "#7E8CA3" },
            new[] { "#0B0B0B", "#151515", "#050505", "#303030", "#050505", "#F2F2F2", "#A6A6A6", "#323232", "#1D1D1D", "#FFFFFF", "#444444", "#FF9F1C", "#000000",
                    "#000000", "#FFB000", "#7CE29A", "#3FBF6A", "#262626", "#FF3B30", "#4A1A17", "#000000", "#E0E0E0", "#FFC266", "#7CE29A", "#FF8A80", "#8A8A8A" },
            new[] { "#12417B", "#16498A", "#0C3162", "#2F64A8", "#0C3162", "#FFFFFF", "#C9D8EC", "#4384D3", "#2B63AD", "#FFFFFF", "#5A93DA", "#FFD24A", "#0C3162",
                    "#0A2A55", "#FFD24A", "#9BF0B5", "#7FE0A0", "#0E3A70", "#FF5A52", "#6B2E4A", "#0A2A55", "#E6EEF8", "#FFD24A", "#9BF0B5", "#FFA39A", "#9DB4D3" },
            new[] { "#2A1A41", "#33204F", "#22143A", "#573A85", "#3C245D", "#F4EEFB", "#CDBFDF", "#7650AF", "#583789", "#FFFFFF", "#8A66C2", "#E2C6FF", "#2A1A41",
                    "#1E1131", "#FFD98A", "#B9F2A0", "#9FE08A", "#45306A", "#FF5C7A", "#5E2A4E", "#1E1131", "#EDE4F8", "#E2C6FF", "#B9F2A0", "#FFA3B5", "#A897C2" },
            new[] { "#12301F", "#173B27", "#0B2215", "#2E6C49", "#0B2215", "#E6F6EC", "#BAD4C5", "#3E8A5D", "#2B6644", "#FFFFFF", "#4F9B6E", "#A6E35F", "#0B2215",
                    "#08180F", "#E8F5A0", "#A6E35F", "#A6E35F", "#1F4A32", "#FF5A4E", "#4A2E25", "#08180F", "#E1F2E7", "#E8F5A0", "#A6E35F", "#FF9E94", "#86A893" } };
        public static bool IsIp { get { return Schema > SchemaScia; } }
''', 'IpColori')

    s = rep(s, 'public static int Count { get { return SchemaScia + 1; } }',
            'public static int Count { get { return SchemaScia + 1 + IpChiave.Length; } }', 'Count')
    s = rep(s, '                n[SchemaScia] = "Scia meteorica";\n',
            '                n[SchemaScia] = "Scia meteorica";\n                for (int i = 0; i < IpNome.Length; i++) n[SchemaScia + 1 + i] = IpNome[i];\n', 'Names')
    s = rep(s, '                if (Schema == SchemaScia) return "scia";\n',
            '                if (Schema > SchemaScia && Schema < Count) return "ip-" + IpChiave[Schema - SchemaScia - 1];\n                if (Schema == SchemaScia) return "scia";\n', 'Chiave')
    s = rep(s, '            if (k == "scia") return SchemaScia;\n',
            '            if (k == "scia") return SchemaScia;\n            if (k.StartsWith("ip-")) { int i = Array.IndexOf(IpChiave, k.Substring(3)); if (i >= 0) return SchemaScia + 1 + i; }\n', 'DaChiave')
    s = rep(s, '            Schema = Math.Max(0, Math.Min(SchemaScia, schema));\n            Scuro = Schema == 1 || Schema == 2 || Schema == SchemaScia;',
            '            Schema = Math.Max(0, Math.Min(Count - 1, schema));\n            Scuro = Schema == 1 || Schema == 2 || Schema == SchemaScia || (Schema > SchemaScia && IpScuro[Schema - SchemaScia - 1]);', 'Imposta')

    s = rep(s, '''            Silver = Rgb(192, 192, 192);
            if (schema == Tema.SchemaScia)''', '''            Silver = Rgb(192, 192, 192);
            if (schema > Tema.SchemaScia && schema < Tema.Count) { LoadIp(schema - Tema.SchemaScia - 1); return; }
            if (schema == Tema.SchemaScia)''', 'Load')

    s = rep(s, '''        // Riempie la tavolozza con lo schema indicato''', '''        static Color Hex(string h) { return Color.Parse(h); }
        static Color Fra(Color a, Color b, double k)
        {
            return Color.FromRgb((byte)(a.R + (b.R - a.R) * k), (byte)(a.G + (b.G - a.G) * k), (byte)(a.B + (b.B - a.B) * k));
        }

        // E-LAYER ANALYZER (tools/temi_ip.py): i temi di IP-TRX-CTRL nella tavolozza del programma. La barra delle
        // linguette e quella di stato hanno il fondo del display della frequenza (QrgBg), scuro anche nel tema chiaro.
        static void LoadIp(int i)
        {
            string[] c = Tema.IpColori[i];
            bool dark = Tema.IpScuro[i];
            Color bg = Hex(c[0]), pan = Hex(c[1]), ins = Hex(c[2]), bor = Hex(c[3]), fld = Hex(c[4]), txt = Hex(c[5]), mut = Hex(c[6]);
            Color btn1 = Hex(c[7]), btn2 = Hex(c[8]), btnTxt = Hex(c[9]), btnBor = Hex(c[10]), acc = Hex(c[11]), accTxt = Hex(c[12]);
            Color qrgBg = Hex(c[13]), qrg = Hex(c[14]), ok = Hex(c[15]), tx = Hex(c[18]);
            Color logBg = Hex(c[20]), logTxt = Hex(c[21]), logSend = Hex(c[22]), logRecv = Hex(c[23]), logMut = Hex(c[25]);
            Color white = Rgb(255, 255, 255);
            Back = bg; Panel = pan; Panel2 = ins; Panel3 = btn2;
            Track = qrgBg; Border = bor; Fore = txt; Muted = mut;
            Azure = acc; AzureDark = Fra(bg, acc, dark ? 0.18 : 0.12);
            Violet = dark ? Rgb(196, 170, 255) : Rgb(124, 58, 237);
            Green = ok; Yellow = dark ? qrg : Rgb(181, 131, 0);
            Orange = dark ? Rgb(255, 170, 64) : Rgb(217, 123, 0);
            OrangeDark = dark ? Fra(bg, Orange, 0.22) : Rgb(255, 245, 220);
            OrangeText = dark ? Rgb(254, 215, 170) : Rgb(180, 83, 9);
            Pink = dark ? Rgb(240, 171, 252) : Rgb(190, 50, 150); Red = tx;
            Card = pan; Field = fld; FieldFore = txt;
            ChartBg = ins; ChartGrid = bor; ChartText = mut;
            OkBg = Fra(bg, ok, dark ? 0.28 : 0.16); WarnBg = OrangeDark;
            BarFore = dark ? txt : Rgb(226, 232, 240); BarMuted = dark ? mut : Rgb(148, 163, 184);
            BarHover = Fra(qrgBg, white, 0.12);
            AccentFore = accTxt; AccentHover = Fra(acc, white, 0.25);
            Pill = btn2; PillFore = btnTxt; PillBorder = btnBor; PillHover = btn1;
            Select = Fra(pan, acc, dark ? 0.28 : 0.15);
            TableHead = ins; TableHeadFore = txt; TableHeadLine = bor;
            CodeBack = logBg; CodeHead = qrg; CodeLabel = logSend; CodeText = logTxt;
            Desk = dark ? qrgBg : ins;
            if (!dark) { Fore = txt; FieldFore = txt; }
        }

        // Riempie la tavolozza con lo schema indicato''', 'LoadIp')
    open(P, 'w', encoding='utf-8').write(s)
    print('temi IP-TRX-CTRL aggiunti in', P)

main()
