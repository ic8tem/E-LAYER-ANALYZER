// YA-MS - raccolta degli spot meteor scatter dai DX cluster, da RBN e da PSKReporter (via telnet), come in
// E-LAYER ANALYZER: tre sessioni telnet, lettura delle righe "DX de ...", filtro e archivio.
//
// Filtro (regola dell'autore, IC8TEM): si tengono solo gli spot che contengono una di queste parole, in maiuscolo,
// minuscolo o misto:
//   MSK144, FSK441, PSK2K, JTMS, JT6M = modi digitali usati solo per il meteor scatter (lo spot e' MS di sicuro);
//   <MS>                             = indicazione generica che il collegamento e' avvenuto via meteor scatter.
// L'elenco si puo' ampliare dalla linguetta; ogni parola si puo' spegnere. Le parole alfanumeriche valgono solo come
// parola intera (MSK144 non scatta dentro "XMSK1440"); <MS> ammette spazi dentro le parentesi (< ms >).
//
// Le sessioni sono statiche: sopravvivono al cambio di lingua o di schema di colori (la finestra viene ricreata).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace YAMS
{
    public class MsSpot
    {
        public DateTime Utc;
        public double FreqKHz;
        public string DxCall = "", Spotter = "", Comment = "", Line = "";
        public int Source;                 // 0 = cluster, 1 = RBN, 2 = PSKReporter
        public string Match = "";          // parola del filtro trovata (MSK144, <MS>, ...)
        public string LocSpotter = "", LocDx = "";
        public double DistKm = double.NaN;
        public DateTime Received = DateTime.MinValue;   // ora (UTC del computer) in cui lo spot e' arrivato: lampeggio nel traffico in diretta
        public string Band { get { return MsBands.Name(FreqKHz); } }
        public string Key { get { return DxCall + "|" + Spotter + "|" + Math.Round(FreqKHz) + "|" + Utc.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture); } }
    }

    public static class MsBands
    {
        public static readonly string[] Names = { "28", "50", "70", "144", "222", "432" };

        public static string Name(double kHz)
        {
            if (kHz >= 28000 && kHz <= 29700) return "28";
            if (kHz >= 50000 && kHz <= 54000) return "50";
            if (kHz >= 69900 && kHz <= 71000) return "70";
            if (kHz >= 144000 && kHz <= 148000) return "144";
            if (kHz >= 220000 && kHz <= 225000) return "222";
            if (kHz >= 430000 && kHz <= 440000) return "432";
            return "";
        }
    }

    public class MsFilterWord
    {
        public string Text;
        public bool Enabled = true;
        public bool BuiltIn;
        public int Count;                  // spot accettati grazie a questa parola (da quando il programma e' aperto)
        Regex rx;

        public MsFilterWord(string text, bool enabled, bool builtIn)
        {
            Text = text.Trim(); Enabled = enabled; BuiltIn = builtIn;
            string t = Text;
            string pat;
            if (t.StartsWith("<") && t.EndsWith(">") && t.Length > 2)
                pat = "<\\s*" + Regex.Escape(t.Substring(1, t.Length - 2).Trim()) + "\\s*>";
            else
                pat = "(?<![A-Za-z0-9])" + Regex.Escape(t) + "(?![A-Za-z0-9])";
            rx = new Regex(pat, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        public bool IsMatch(string s) { return rx.IsMatch(s ?? ""); }
    }

    public static class MsCollector
    {
        public static readonly string[] SourceNames = { "Cluster", "RBN", "PSKReporter" };
        public static readonly string[] BuiltInWords = { "MSK144", "FSK441", "FSK41", "PSK2K", "JTMS", "JT6M", "<MS>" };   // FSK41: grafia abbreviata di FSK441 indicata dall'autore

        // valori di partenza delle tre sessioni: nome, host, porta, comandi iniziali, nota
        public static readonly string[][] Defaults =
        {
            new[] { "DX cluster a scelta", "on0dxk.dyndns.org", "8000", "set/filter dxbm/pass 6;set/filter dxbm/pass 2", "Cluster scelto dall'utente. Su ON0DXK le bande VHF sono filtrate: si abilitano con set/filter dxbm/pass 6 (poi 4, 2...). Altro esempio: hamqth.com 7300." },
            new[] { "Reverse Beacon Network", "telnet.reversebeacon.net", "7001", "", "RBN: porta 7000 = CW e RTTY, porta 7001 = FT8. Login con il solo nominativo. Gli skimmer RBN raramente decodificano i modi MS: serve soprattutto per gli spot con <MS> nel commento." },
            new[] { "Cluster con PSKReporter", "dm4x.ddns.net", "8500", "", "Cluster di DM4X con gli spot di PSKReporter (dm4x.ddns.net, porta 8500): qui arrivano le decodifiche MSK144 di WSJT-X e MSHV. In alternativa HamAlert (hamalert.org 7300, serve un account con i trigger)." }
        };

        public static readonly TelnetSession[] Sessions = { new TelnetSession(), new TelnetSession(), new TelnetSession() };
        public static readonly List<MsSpot> Spots = new List<MsSpot>();          // accettati (ultimi 5000)
        public static readonly List<MsFilterWord> Words = new List<MsFilterWord>();
        public static readonly HashSet<string> Bands = new HashSet<string>(MsBands.Names);
        public static readonly Queue<string>[] ConsoleLines = { new Queue<string>(), new Queue<string>(), new Queue<string>() };
        public static int[] Lines = new int[3], Parsed = new int[3], Accepted = new int[3];
        public static int RejectedNoWord, RejectedBand, DroppedLow;
        // regola dell'autore (IC8TEM): le righe "DX de" con frequenza sotto 27990,0 kHz si scartano subito, in locale:
        // non vanno nella console, non si contano come spot e non si memorizzano
        // (opzionale: casella nella linguetta, accesa di partenza)
        public const double MinKHz = 27990.0;
        public static bool DropLow = true;
        // PRIMO criterio (regola dell'autore, IC8TEM): le frequenze tipiche degli spot MS. Uno spot che cade in una di
        // queste finestre si accetta anche senza parole del modo, a meno che il commento dichiari un'altra propagazione
        // (<TR> tropo, <AS>/<ACS> aircraft scatter, <AU> aurora, <ES> sporadico E, <FAI>, <F2>): in quel caso, e per
        // tutto cio' che sta fuori dalle finestre, decide il secondo criterio (modi digitali MS e <MS>).
        public const string DefaultWindows = "50190-50300; 70100-70400; 144290-144399";
        public const string DefaultOtherTags = "<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>";
        public static bool UseWindows = true;
        public static string WindowsText = DefaultWindows, OtherTagsText = DefaultOtherTags;
        public static int AcceptedWindow, RejectedOther;
        static List<double[]> windows = ParseWindows(DefaultWindows);
        static Regex rxOther = BuildOther(DefaultOtherTags);

        public static List<double[]> ParseWindows(string t)
        {
            List<double[]> l = new List<double[]>();
            foreach (Match m in Regex.Matches(t ?? "", @"(\d+(?:[.,]\d+)?)\s*-\s*(\d+(?:[.,]\d+)?)"))
            {
                double a = double.Parse(m.Groups[1].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
                double b = double.Parse(m.Groups[2].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
                if (b < a) { double x = a; a = b; b = x; }
                l.Add(new[] { a, b });
            }
            return l;
        }

        static Regex BuildOther(string t)
        {
            List<string> tags = new List<string>();
            foreach (Match m in Regex.Matches(t ?? "", @"<\s*([A-Za-z0-9]+)\s*>")) tags.Add(Regex.Escape(m.Groups[1].Value));
            if (tags.Count == 0) return null;
            return new Regex(@"<\s*(" + string.Join("|", tags) + @")\s*>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        public static void SetWindows(string t) { WindowsText = t ?? ""; windows = ParseWindows(WindowsText); }
        public static void SetOtherTags(string t) { OtherTagsText = t ?? ""; rxOther = BuildOther(OtherTagsText); }

        // finestra MS che contiene la frequenza ("144290-144399"), null se nessuna
        public static string WindowOf(double kHz)
        {
            foreach (double[] w in windows)
                if (kHz >= w[0] && kHz <= w[1] + 0.999) return w[0].ToString("0.###", CultureInfo.InvariantCulture) + "-" + w[1].ToString("0.###", CultureInfo.InvariantCulture);
            return null;
        }

        // scritta di altra propagazione nel commento (<ES>, <TR>...), null se nessuna
        public static string OtherTag(string comment)
        {
            if (rxOther == null) return null;
            Match m = rxOther.Match(comment ?? "");
            return m.Success ? "<" + m.Groups[1].Value.ToUpperInvariant() + ">" : null;
        }

        // decisione sullo spot: etichetta del criterio che lo accetta (finestra o parola), null se scartato.
        // word = parola del secondo criterio (null se accettato per finestra); other = scritta di altra propagazione trovata
        public static string Decide(MsSpot s, out MsFilterWord word, out string other)
        {
            word = FindWord(s.Comment + " " + s.DxCall);
            other = OtherTag(s.Comment);
            if (UseWindows && other == null)
            {
                string win = WindowOf(s.FreqKHz);
                if (win != null) { word = null; return win; }
            }
            return word != null ? word.Text : null;
        }

        public static string Dir;
        public static bool Started;        // connessione automatica gia' eseguita (una sola volta per avvio del programma)
        public static string[] StateText = { "Non connesso", "Non connesso", "Non connesso" };
        static readonly HashSet<string> keys = new HashSet<string>();
        static readonly Queue<KeyValuePair<int, string>> pending = new Queue<KeyValuePair<int, string>>();
        static readonly object lk = new object();
        static bool hooked;

        public static void Init(string dataDir, Config cfg)
        {
            Dir = Path.Combine(dataDir, "spot_ms");
            try { Directory.CreateDirectory(Dir); } catch { }
            if (!hooked)
            {
                hooked = true;
                for (int i = 0; i < 3; i++)
                {
                    int k = i;
                    Sessions[i].Name = Defaults[i][0];
                    Sessions[i].LineReceived += l => { lock (lk) pending.Enqueue(new KeyValuePair<int, string>(k, l)); };
                    Sessions[i].StateChanged += st => { StateText[k] = st; };
                }
                LoadWords(cfg);
                DropLow = cfg.GetB("rd_scarta_sotto_28", true);
                UseWindows = cfg.GetB("rd_finestre_attive", true);
                SetWindows(cfg.Get("rd_finestre", DefaultWindows));
                SetOtherTags(cfg.Get("rd_altre_propagazioni", DefaultOtherTags));
                string b = cfg.Get("rd_bande", "");
                if (b.Length > 0) { Bands.Clear(); foreach (string x in b.Split(',')) if (x.Trim().Length > 0) Bands.Add(x.Trim()); }
                LoadRecent();
            }
        }

        static void LoadWords(Config cfg)
        {
            Words.Clear();
            string s = cfg.Get("rd_filtri", "");
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (s.Length > 0)
                foreach (string it in s.Split('\u0001'))
                {
                    int p = it.IndexOf('|');
                    if (p < 0) continue;
                    string w = it.Substring(p + 1).Trim();
                    if (w.Length == 0 || !seen.Add(w)) continue;
                    Words.Add(new MsFilterWord(w, it.Substring(0, p) == "1", Array.IndexOf(BuiltInWords, w.ToUpperInvariant()) >= 0));
                }
            foreach (string w in BuiltInWords)   // le parole dell'autore ci sono sempre (si possono solo spegnere)
                if (!seen.Contains(w)) { Words.Insert(Math.Min(Words.Count, Array.IndexOf(BuiltInWords, w)), new MsFilterWord(w, true, true)); seen.Add(w); }
        }

        public static void Save(Config cfg)
        {
            StringBuilder sb = new StringBuilder();
            foreach (MsFilterWord w in Words) { if (sb.Length > 0) sb.Append('\u0001'); sb.Append(w.Enabled ? "1|" : "0|").Append(w.Text); }
            cfg.Set("rd_filtri", sb.ToString());
            cfg.Set("rd_bande", string.Join(",", Bands));
            cfg.Set("rd_scarta_sotto_28", DropLow);
            cfg.Set("rd_finestre_attive", UseWindows);
            cfg.Set("rd_finestre", WindowsText);
            cfg.Set("rd_altre_propagazioni", OtherTagsText);
        }

        public static void Shutdown() { foreach (TelnetSession s in Sessions) s.Disconnect(); }

        // E-LAYER ANALYZER: riga ricevuta da una sessione di DATA COLLECTION (0 = cluster, 1 = RBN, 2 = PSKReporter)
        public static void Feed(int source, string line) { lock (lk) pending.Enqueue(new KeyValuePair<int, string>(source, line)); }

        // svuota la tabella degli spot e l'elenco dei doppioni (uno spot uguale arrivato dopo torna a comparire)
        public static void ClearSpots() { Spots.Clear(); keys.Clear(); }

        // solo per le fotografie di controllo (--screenshot): spot di esempio, non salvati su disco
        public static void AddDemo(MsSpot s) { if (keys.Add(s.Key)) Spots.Add(s); }

        // parola del filtro trovata nella riga (null = nessuna)
        public static MsFilterWord FindWord(string text)
        {
            foreach (MsFilterWord w in Words) if (w.Enabled && w.IsMatch(text)) return w;
            return null;
        }

        // elabora le righe arrivate (thread dell'interfaccia); true se ci sono spot nuovi
        public static bool Process()
        {
            List<KeyValuePair<int, string>> got = new List<KeyValuePair<int, string>>();
            lock (lk) { while (pending.Count > 0) got.Add(pending.Dequeue()); }
            bool added = false;
            DateTime now = DateTime.UtcNow;
            foreach (KeyValuePair<int, string> kv in got)
            {
                int i = kv.Key;
                Lines[i]++;
                MsSpot s = Parse(kv.Value, i, now);
                if (DropLow && s != null && s.FreqKHz < MinKHz) { DroppedLow++; continue; }
                ConsoleLines[i].Enqueue(kv.Value);
                while (ConsoleLines[i].Count > 200) ConsoleLines[i].Dequeue();
                if (s == null) continue;
                Parsed[i]++;
                MsFilterWord w; string other;
                string why = Decide(s, out w, out other);
                if (why == null) { if (other != null) RejectedOther++; else RejectedNoWord++; continue; }
                if (s.Band.Length == 0 || !Bands.Contains(s.Band)) { RejectedBand++; continue; }
                s.Match = why;
                if (!keys.Add(s.Key)) continue;
                if (w != null) w.Count++; else AcceptedWindow++;
                Accepted[i]++;
                Locate(s);
                s.Received = DateTime.UtcNow;
                Spots.Add(s);
                if (Spots.Count > 5000) { for (int k = 0; k < 500; k++) keys.Remove(Spots[k].Key); Spots.RemoveRange(0, 500); }
                Append(s);
                added = true;
            }
            return added;
        }

        // ---- lettura della riga "DX de SPOTTER:  FREQ  DXCALL  commento  HHMMZ [LOC]"
        static readonly Regex rxDx = new Regex(@"^DX de\s+([A-Z0-9/\-#_]+)[:>]?\s+(\d+(?:\.\d+)?)\s+([A-Z0-9/]+)\s+(.*?)\s*(\d{4})Z(?:\s+([A-R]{2}\d{2}(?:[A-X]{2})?))?\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static readonly Regex rxTwoLoc = new Regex(@"\b([A-R]{2}\d{2}(?:[A-X]{2})?)\s*(?:<\s*MS\s*>|\(\s*MS\s*\)|<-?>|[-/>\s])\s*([A-R]{2}\d{2}(?:[A-X]{2})?)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static readonly Regex rxLoc = new Regex(@"\b([A-R]{2}\d{2}(?:[A-X]{2})?)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static MsSpot Parse(string line, int source, DateTime nowUtc)
        {
            if (line == null) return null;
            int k = line.IndexOf("DX de ", StringComparison.OrdinalIgnoreCase);
            if (k < 0) return null;
            Match m = rxDx.Match(line.Substring(k).TrimEnd());
            if (!m.Success) return null;
            double f;
            if (!double.TryParse(m.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out f)) return null;
            MsSpot s = new MsSpot
            {
                Source = source, Line = line, FreqKHz = f,
                Spotter = m.Groups[1].Value.ToUpperInvariant(), DxCall = m.Groups[3].Value.ToUpperInvariant(), Comment = m.Groups[4].Value.Trim()
            };
            string hhmm = m.Groups[5].Value;
            int hh = int.Parse(hhmm.Substring(0, 2), CultureInfo.InvariantCulture), mm = int.Parse(hhmm.Substring(2, 2), CultureInfo.InvariantCulture);
            DateTime t = new DateTime(nowUtc.Year, nowUtc.Month, nowUtc.Day, Math.Min(23, hh), Math.Min(59, mm), 0, DateTimeKind.Utc);
            if (t > nowUtc.AddMinutes(30)) t = t.AddDays(-1);
            s.Utc = t;
            Match tl = rxTwoLoc.Match(s.Comment);
            if (tl.Success && IsLoc(tl.Groups[1].Value) && IsLoc(tl.Groups[2].Value))
            {
                s.LocSpotter = tl.Groups[1].Value.ToUpperInvariant(); s.LocDx = tl.Groups[2].Value.ToUpperInvariant();
            }
            else
            {
                Match l = rxLoc.Match(s.Comment);
                if (l.Success && IsLoc(l.Groups[1].Value)) s.LocDx = l.Groups[1].Value.ToUpperInvariant();
            }
            if (s.LocSpotter.Length == 0 && m.Groups[6].Success) s.LocSpotter = m.Groups[6].Value.ToUpperInvariant();
            return s;
        }

        static bool IsLoc(string l) { LatLon p; return l != null && (l.Length == 4 || l.Length == 6) && Astro.TryLocator(l.ToUpperInvariant(), out p); }

        // Regola dell'autore (IC8TEM): i locator non precisi si prendono da CALL3.
        // - locator mancante: quello di CALL3;
        // - locator a 4 caratteri (solo il quadrato, ~100 x 200 km): se CALL3 ha un locator a 6 caratteri si usa sempre
        //   quello (~5 x 2,5 km), anche se indica un altro quadrato (scelta dell'autore, 7/11/2026);
        // - locator a 6 caratteri: si tiene quello dello spot.
        public static string Refine(string loc, string call)
        {
            loc = (loc ?? "").Trim().ToUpperInvariant();
            string c3 = CallDb.Lookup(call);
            if (loc.Length == 0) return c3 ?? "";
            if (loc.Length < 6 && c3 != null && c3.Length >= 6) return c3;
            return loc;
        }

        // locator mancanti o imprecisi dall'archivio CALL3 e distanza
        static void Locate(MsSpot s)
        {
            s.LocSpotter = Refine(s.LocSpotter, s.Spotter);
            s.LocDx = Refine(s.LocDx, s.DxCall);
            LatLon a, b;
            if (Astro.TryLocator(s.LocSpotter, out a) && Astro.TryLocator(s.LocDx, out b)) s.DistKm = Astro.DistanceKm(a, b);
        }

        static void Append(MsSpot s)
        {
            if (Dir == null) return;
            try
            {
                string f = Path.Combine(Dir, s.Utc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".csv");
                bool head = !File.Exists(f);
                StringBuilder sb = new StringBuilder();
                if (head) sb.AppendLine("utc;freq_khz;banda;dx;spotter;loc_dx;loc_spotter;km;filtro;fonte;commento");
                sb.Append(s.Utc.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)).Append(';')
                  .Append(s.FreqKHz.ToString("F1", CultureInfo.InvariantCulture)).Append(';').Append(s.Band).Append(';')
                  .Append(s.DxCall).Append(';').Append(s.Spotter).Append(';').Append(s.LocDx).Append(';').Append(s.LocSpotter).Append(';')
                  .Append(double.IsNaN(s.DistKm) ? "" : s.DistKm.ToString("F0", CultureInfo.InvariantCulture)).Append(';')
                  .Append(s.Match).Append(';').Append(SourceNames[s.Source]).Append(';')
                  .Append((s.Comment ?? "").Replace(';', ',')).AppendLine();
                File.AppendAllText(f, sb.ToString());
            }
            catch { }
        }

        // all'avvio: spot MS di ieri e di oggi dall'archivio
        static void LoadRecent()
        {
            if (Dir == null) return;
            DateTime now = DateTime.UtcNow;
            for (int d = 1; d >= 0; d--)
            {
                string f = Path.Combine(Dir, now.AddDays(-d).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".csv");
                if (!File.Exists(f)) continue;
                try
                {
                    foreach (string line in File.ReadAllLines(f))
                    {
                        string[] p = line.Split(';');
                        if (p.Length < 11 || p[0] == "utc") continue;
                        DateTime t;
                        if (!DateTime.TryParseExact(p[0], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out t)) continue;
                        MsSpot s = new MsSpot { Utc = DateTime.SpecifyKind(t, DateTimeKind.Utc) };
                        double fr; double.TryParse(p[1], NumberStyles.Float, CultureInfo.InvariantCulture, out fr); s.FreqKHz = fr;
                        s.DxCall = p[3]; s.Spotter = p[4]; s.LocDx = p[5]; s.LocSpotter = p[6];
                        double km; s.DistKm = double.TryParse(p[7], NumberStyles.Float, CultureInfo.InvariantCulture, out km) ? km : double.NaN;
                        s.Match = p[8]; s.Source = Math.Max(0, Array.IndexOf(SourceNames, p[9])); s.Comment = p[10];
                        if (keys.Add(s.Key)) Spots.Add(s);
                    }
                }
                catch { }
            }
        }
    }

    // Archivio nominativo -> locator (file CALL3.TXT di WSJT-X/JTDX/MSHV, formato "CALL,LOCATOR,..."), in data/call3.txt
    public static class CallDb
    {
        static string[] calls = new string[0], locs = new string[0];
        public static volatile bool Loading;
        public static int Count { get { return calls.Length; } }
        public static string FilePath = "";
        public static DateTime FileDate;
        public static string LastError = "";
        public static event Action Loaded;

        public static void StartLoad(string dataDir)
        {
            FilePath = Path.Combine(dataDir, "call3.txt");
            if (Loading) return;
            Loading = true;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try { Load(); LastError = ""; } catch (Exception ex) { LastError = ex.Message; }
                Loading = false;
                Action a = Loaded; if (a != null) a();
            });
        }

        static void Load()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            Dictionary<string, string> pool = new Dictionary<string, string>(StringComparer.Ordinal);
            if (File.Exists(FilePath))
            {
                FileDate = File.GetLastWriteTime(FilePath);
                using (StreamReader sr = new StreamReader(FilePath, Encoding.ASCII))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("//", StringComparison.Ordinal)) continue;
                        int c1 = line.IndexOf(',');
                        if (c1 <= 0) continue;
                        int c2 = line.IndexOf(',', c1 + 1);
                        string call = line.Substring(0, c1).Trim().ToUpperInvariant();
                        string loc = (c2 > c1 ? line.Substring(c1 + 1, c2 - c1 - 1) : line.Substring(c1 + 1)).Trim().ToUpperInvariant();
                        if (call.Length == 0 || (loc.Length != 4 && loc.Length != 6)) continue;
                        string pl; if (!pool.TryGetValue(loc, out pl)) { pool[loc] = loc; pl = loc; }
                        list.Add(new KeyValuePair<string, string>(call, pl));
                    }
                }
            }
            list.Sort((a, b) => string.CompareOrdinal(a.Key, b.Key));
            string[] c = new string[list.Count], l = new string[list.Count];
            for (int i = 0; i < list.Count; i++) { c[i] = list[i].Key; l[i] = list[i].Value; }
            calls = c; locs = l;
        }

        static string Exact(string call)
        {
            string[] c = calls, l = locs;
            int lo = 0, hi = c.Length - 1;
            while (lo <= hi)
            {
                int mid = (lo + hi) >> 1;
                int k = string.CompareOrdinal(c[mid], call);
                if (k == 0) return l[mid];
                if (k < 0) lo = mid + 1; else hi = mid - 1;
            }
            return null;
        }

        // esatto, poi senza suffisso da skimmer (-#, -1), poi la parte piu' lunga tra le barre
        public static string Lookup(string call)
        {
            if (string.IsNullOrEmpty(call)) return null;
            call = call.Trim().ToUpperInvariant();
            string r = Exact(call);
            if (r != null) return r;
            int dash = call.IndexOf('-');
            if (dash > 0) { call = call.Substring(0, dash); r = Exact(call); if (r != null) return r; }
            if (call.IndexOf('/') >= 0)
            {
                string best = "";
                foreach (string p in call.Split('/')) if (p.Length > best.Length) best = p;
                if (best.Length >= 3) return Exact(best);
            }
            return null;
        }

        public static void Replace(string newFile, string dataDir)
        {
            string dest = Path.Combine(dataDir, "call3.txt");
            if (File.Exists(dest)) File.Copy(dest, dest + ".bak", true);
            File.Copy(newFile, dest, true);
            StartLoad(dataDir);
        }
    }
}
