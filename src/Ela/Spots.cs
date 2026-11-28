// E-LAYER ANALYZER - spot dei DX cluster (riga "DX de ...") e archivio in memoria / su disco
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ELA
{
    public class Spot
    {
        public DateTime Utc;          // ora dello spot (dal cluster) o di ricezione
        public double FreqKHz;
        public string DxCall = "", Spotter = "", Comment = "";
        public int Source;            // 0 = cluster a scelta, 1 = RBN, 2 = cluster con PSKReporter, 3 = WSJT-X UDP, 4 = APRS-IS (2 m), 9 = prova
        public string Prop = "";      // modo di propagazione dichiarato: ES, FAI, AU, MS, TR, EME, F2, TEP
        public string LocSpotter = "", LocDx = "";
        public bool LocFromComment;   // due locator letti nel commento nella forma LOC<modo>LOC (si imparano per CALL3)
        public double LatS = double.NaN, LonS = double.NaN, LatD = double.NaN, LonD = double.NaN;
        public double Snr = double.NaN;
        public string DigiMode = "";  // CW, FT8, FT4, ... (RBN e PSKReporter)

        public double FreqMHz { get { return FreqKHz / 1000.0; } }

        // ---- memorizzazione dei calcoli dello spot (ottimizzazione del 28/11/2026) ----
        // Le ricerche nel testo (commento e modo digitale), la distanza, la chiave e la banda si calcolano una volta e si
        // riusano finche' non cambia il dato da cui dipendono (stesso testo, stesse coordinate, stesse regole dei filtri):
        // i risultati sono identici a prima, cambia solo quante volte si calcolano.
        // (ogni memoria e' un oggetto che non cambia, sostituito per intero: sicuro anche fra thread diversi)
        sealed class TextMemo { public string C, D; public int Gen, Flags; }
        sealed class DistMemo { public double LatS, LonS, LatD, LonD, Km; }
        sealed class KeyMemo { public string Dx, Sp, Key; public double F; public DateTime U; }
        sealed class BandMemo { public double F; public string Name; }
        TextMemo tm; DistMemo dm; KeyMemo km; BandMemo bm;
        const int TF_FT = 1, TF_MSMODE = 2, TF_EME = 4, TF_ESTAG = 8, TF_AUE = 16, TF_NOTES = 32, TF_AUTAG = 64, TF_FAITAG = 128;
        bool TF(int bit)
        {
            string c = Comment, d = DigiMode;
            int gen = Filters.Gen;
            TextMemo m = tm;
            if (m == null || !ReferenceEquals(c, m.C) || !ReferenceEquals(d, m.D) || gen != m.Gen)
            {
                string cc = c ?? "", dd = d ?? "";
                int f = 0;
                if (rxFt.IsMatch(dd + " " + cc)) f |= TF_FT;
                if (rxMsMode.IsMatch(cc + " " + dd)) f |= TF_MSMODE;
                if (rxEmeMode.IsMatch(dd + " " + cc)) f |= TF_EME;
                if (rxEsTag.IsMatch(cc)) f |= TF_ESTAG;
                if (rxAuE.IsMatch(cc)) f |= TF_AUE;
                if (Filters.NotEsTag(c)) f |= TF_NOTES;
                if (Filters.AuTag(c)) f |= TF_AUTAG;
                if (Filters.FaiTag(c)) f |= TF_FAITAG;
                m = new TextMemo { C = c, D = d, Gen = gen, Flags = f };
                tm = m;
            }
            return (m.Flags & bit) != 0;
        }

        // Regola dell'autore (IC8TEM): se nel testo dello spot compare <MS>, <AU> o <TR> lo spot non riguarda l'E-sporadico
        // (serve invece al meteor scatter, all'aurora o al tropo) e la linguetta SPORADIC-E lo scarta.
        static readonly System.Text.RegularExpressions.Regex rxNotEs = new System.Text.RegularExpressions.Regex(@"<\s*(MS|AURORA|AUR|AUE|AU|TR|TROPO|AS|ACS)\s*>",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        // Regola di YA-MS (IC8TEM), valida anche qui: spot di meteor scatter = modi univocamente MS (MSK144, FSK441/FSK41, PSK2K,
        // JTMS, JT6M) oppure <MS> nel testo, senza distinzione tra maiuscole e minuscole. Non sono spot di E-sporadico.
        static readonly System.Text.RegularExpressions.Regex rxMsMode = new System.Text.RegularExpressions.Regex(@"MSK144|FSK441|FSK41|PSK2K|JTMS|JT6M",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        // Primo criterio di YA-MS (regola dell'autore): frequenza in una finestra tipica del MS (50190-50300, 70100-70400,
        // 144290-144399 kHz, impostabili nella Raccolta dati di YA-MS) senza scritte di altra propagazione (<TR> <AS> <ACS>
        // <AU> <ES> <FAI> <F2>); secondo criterio: modi MS o <MS>.
        public bool IsMeteorScatter
        {
            get
            {
                if (Prop == "MS" || TF(TF_MSMODE)) return true;
                // FT8 e FT4 in 50 e 70 MHz non sono meteor scatter per la sola finestra del bandplan (regola dell'autore,
                // 14/11/2026): in 70 MHz l'FT8 sta a 70,154 MHz, dentro la finestra MS 70100-70400 kHz
                if (IsFt8Ft4 && (Band == "6m" || Band == "4m")) return false;
                return YAMS.MsCollector.UseWindows && YAMS.MsCollector.OtherTag(Comment) == null && YAMS.MsCollector.WindowOf(FreqKHz) != null;
            }
        }
        // FT8 o FT4 (modo dato da RBN, PSKReporter o WSJT-X, oppure scritto nel commento)
        static readonly System.Text.RegularExpressions.Regex rxFt = new System.Text.RegularExpressions.Regex(@"(?<![A-Z0-9])(FT8|FT4)(?![A-Z0-9])",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        public bool IsFt8Ft4 { get { return TF(TF_FT); } }
        // Q65 e JT65 in 144 MHz sono EME (regola dell'autore, 14/11/2026), salvo <ES> nel testo
        static readonly System.Text.RegularExpressions.Regex rxEmeMode = new System.Text.RegularExpressions.Regex(@"(?<![A-Z0-9])(Q65[A-E]?|JT65[A-C]?)(?![A-Z0-9])",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        static readonly System.Text.RegularExpressions.Regex rxEsTag = new System.Text.RegularExpressions.Regex(@"<\s*ES\s*>",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        public bool IsEme144 { get { return Band == "2m" && Prop != "ES" && !TF(TF_ESTAG) && TF(TF_EME); } }
        // Spot di aurora (regola dell'autore): <AU> o <AUR> (anche AUE, AURORA) nel testo, oppure modo dichiarato AU
        static readonly System.Text.RegularExpressions.Regex rxAu = new System.Text.RegularExpressions.Regex(@"<\s*(AURORA|AUR|AUE|AU)\s*>",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        public bool IsAurora { get { return (Prop == "AU" || TF(TF_AUTAG)) && !IsAuroraE; } }
        // aurora-E (Au-E): E sporadico di origine aurorale, senza Doppler, sul percorso diretto, dopo l'evento aurorale (G0ISW [33]);
        // riconosciuto da AUE, AU-E o AURORAL E nel testo, oppure dal modo dichiarato AUE
        static readonly System.Text.RegularExpressions.Regex rxAuE = new System.Text.RegularExpressions.Regex(@"(?<![A-Z0-9])(AUE|AU-E|AURORAL[- ]?E)(?![A-Z0-9])",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        public bool IsAuroraE { get { return Prop == "AUE" || TF(TF_AUE); } }
        // MS sicuro: modo dichiarato MS, modo digitale MS o <MS> (la sola finestra di frequenza del bandplan e' un indizio)
        public bool IsMeteorScatterSure { get { return Prop == "MS" || TF(TF_MSMODE); } }
        // In 50 MHz il bandplan non e' un criterio esclusivo (regola dell'autore): la sola finestra MS non toglie lo spot all'E-sporadico
        public bool TaggedNotEs { get { return TF(TF_NOTES) || (Band == "6m" && Filters.Es50WindowNotExclusive ? IsMeteorScatterSure : IsMeteorScatter); } }
        // In 50 MHz entro EsModel.MinKm50 (450 km) un collegamento non e' E-sporadico (regola dell'autore);
        // in 28 MHz la distanza minima si scala da quella dei 50 MHz seguendo IK3XTV (EsModel.MinKm28)
        // in 70 MHz entro EsModel.MinKm70 (450 km, regola dell'autore del 14/11/2026); in 40 MHz la distanza minima si scala da
        // quella dei 50 MHz come per i 28 MHz e vale solo il primo salto della geometria dello strato E (EsModel.MaxKm40)
        public bool TooShortForEs { get { return HasBoth && ((Band == "6m" && DistanceKm < EsModel.MinKm50) || (Band == "4m" && DistanceKm < EsModel.MinKm70) || (Band == "10m" && DistanceKm < EsModel.MinKm28) || (Band == "12m" && DistanceKm < EsModel.MinKm24) || (Band == "8m" && DistanceKm < EsModel.MinKm40)); } }
        public bool TooLongForEs { get { return HasBoth && Band == "8m" && DistanceKm > EsModel.MaxKm40; } }
        // utile per l'E-sporadico: modo dichiarato compatibile e nessuna etichetta <MS>/<AU>/<TR> nel testo
        public bool EsCandidate { get { return Prop != "TR" && Prop != "MS" && Prop != "AU" && Prop != "FAI" && Prop != "EME" && Prop != "TEP" && !TaggedNotEs && !TooShortForEs && !TooLongForEs && !IsEme144 && !TF(TF_FAITAG) && !IsAurora && !IsAuroraE && Prop != "AUE"; } }   // <FAI> o fai (maiuscolo, minuscolo o misto) = F.A.I., non Es
        // transequatoriale per la regola dell'autore (EsModel.IsTransEquatorial), salvo che lo spot dichiari ES
        // e per la regola generale di tutto il mondo (EsModel.IsTransEquatorialWorld, Kennewell e Wilkinson)
        // Es che aggancia il TEP (ombra del TEP): resta tra gli spot Es, si disegna in blu elettrico con la scritta TEP
        public bool IsEsTep { get { return Prop != "TEP" && HasBoth && EsModel.IsEsTepCached(LatS, LonS, LatD, LonD); } }
        public bool IsTep { get { return Prop == "TEP" || (Prop != "ES" && HasBoth && (EsModel.IsTransEquatorial(LatS, LonS, LatD, LonD) || EsModel.IsTransEquatorialWorld(LatS, LonS, LatD, LonD))); } }
        public bool HasBoth { get { return !double.IsNaN(LatS) && !double.IsNaN(LatD); } }
        public double DistanceKm
        {
            get
            {
                if (!HasBoth) return double.NaN;
                double a = LatS, b = LonS, c = LatD, d = LonD;
                DistMemo m = dm;
                if (m == null || a != m.LatS || b != m.LonS || c != m.LatD || d != m.LonD) { m = new DistMemo { LatS = a, LonS = b, LatD = c, LonD = d, Km = Geo.DistanceKm(a, b, c, d) }; dm = m; }
                return m.Km;
            }
        }
        public string Band
        {
            get { double f = FreqKHz; BandMemo m = bm; if (m == null || f != m.F) { m = new BandMemo { F = f, Name = Bands.Name(f) }; bm = m; } return m.Name; }
        }
        public string Key
        {
            get
            {
                string dx = DxCall, sp = Spotter; double f = FreqKHz; DateTime u = Utc;
                KeyMemo m = km;
                if (m == null || !ReferenceEquals(dx, m.Dx) || !ReferenceEquals(sp, m.Sp) || f != m.F || u != m.U)
                {
                    m = new KeyMemo { Dx = dx, Sp = sp, F = f, U = u, Key = dx + "|" + sp + "|" + Math.Round(f) + "|" + u.ToString("yyyyMMddHHmm") };
                    km = m;
                }
                return m.Key;
            }
        }
    }

    public static class Bands
    {
        // nome della banda dalla frequenza (kHz)
        public static string Name(double kHz)
        {
            if (kHz >= 24890 && kHz <= 24990) return "12m";   // solo per i prodromi dei 50 MHz (Prodromes.cs)
            if (kHz >= 28000 && kHz <= 29700) return "10m";
            if (kHz >= 40000 && kHz <= 41000) return "8m";
            if (kHz >= 50000 && kHz <= 54000) return "6m";
            if (kHz >= 69900 && kHz <= 71000) return "4m";
            if (kHz >= 144000 && kHz <= 148000) return "2m";
            if (kHz >= 220000 && kHz <= 225000) return "1.25m";
            if (kHz >= 430000 && kHz <= 440000) return "70cm";
            if (kHz >= 1800 && kHz < 28000) return "HF";
            return "";
        }

        public static bool IsVhf(double kHz) { return kHz >= 28000 && kHz <= 440000; }
    }

    public static class SpotParser
    {
        // DX de SPOTTER:   FREQ  DXCALL   commento   HHMMZ [LOC]
        static readonly Regex rxDx = new Regex(@"^DX de\s+([A-Z0-9/\-#_]+)[:>]?\s+(\d+(?:\.\d+)?)\s+([A-Z0-9/]+)\s+(.*?)\s*(\d{4})Z(?:\s+([A-R]{2}\d{2}(?:[A-X]{2})?))?\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        // locator <modo> locator, es. "JN70CN<ES>JO62QM", "JN61 <FAI> JO40", "IO81(TR)JN18"
        static readonly Regex rxLocProp = new Regex(@"\b([A-R]{2}\d{2}(?:[A-X]{2})?)\s*[<\(\[]?\s*(ES|FAI|AURORA|AUR|AUE|AU|MS|TR|TROPO|EME|F2|TEP|SS|RS)?\s*[>\)\]]\s*([A-R]{2}\d{2}(?:[A-X]{2})?)\b|\b([A-R]{2}\d{2}(?:[A-X]{2})?)\s*<\s*(ES|FAI|AURORA|AUR|AUE|AU|MS|TR|TROPO|EME|F2|TEP|SS|RS)\s*>\s*([A-R]{2}\d{2}(?:[A-X]{2})?)\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static readonly Regex rxLoc = new Regex(@"\b([A-R]{2}\d{2}(?:[A-X]{2})?)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static readonly Regex rxProp = new Regex(@"(?:^|[\s<\(\[/])(ES|E-?SKIP|SPORADIC|FAI|AURORA|AUR|AUE|AU|MS|METEOR|TR|TROPO|EME|F2|TEP)(?:$|[\s>\)\]/!.,])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static readonly Regex rxSnr = new Regex(@"(-?\d+)\s*dB", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static readonly Regex rxMode = new Regex(@"\b(CW|FT8|FT4|RTTY|PSK31|JT65|MSK144|Q65|FSK441|SSB|USB|BCN|NCDXF|JS8)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static Spot Parse(string line, int source, DateTime nowUtc)
        {
            if (line == null) return null;
            line = line.TrimEnd();
            int k = line.IndexOf("DX de ", StringComparison.OrdinalIgnoreCase);
            if (k < 0) return null;
            line = line.Substring(k);
            Match m = rxDx.Match(line);
            if (!m.Success) return null;
            Spot s = new Spot();
            s.Source = source;
            s.Spotter = m.Groups[1].Value.ToUpperInvariant();
            double f;
            if (!double.TryParse(m.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out f)) return null;
            s.FreqKHz = f;
            s.DxCall = m.Groups[3].Value.ToUpperInvariant();
            s.Comment = m.Groups[4].Value.Trim();
            string hhmm = m.Groups[5].Value;
            int hh = int.Parse(hhmm.Substring(0, 2), CultureInfo.InvariantCulture), mm = int.Parse(hhmm.Substring(2, 2), CultureInfo.InvariantCulture);
            DateTime t = new DateTime(nowUtc.Year, nowUtc.Month, nowUtc.Day, Math.Min(23, hh), Math.Min(59, mm), 0, DateTimeKind.Utc);
            if (t > nowUtc.AddMinutes(30)) t = t.AddDays(-1);   // spot di ieri sera ricevuto dopo mezzanotte
            s.Utc = t;
            Analyze(s);
            if (m.Groups[6].Success && m.Groups[6].Value.Length > 0 && string.IsNullOrEmpty(s.LocSpotter))
                s.LocSpotter = m.Groups[6].Value.ToUpperInvariant();
            Locate(s);
            return s;
        }

        // modo di propagazione, locator, SNR e modo di emissione dal commento
        public static void Analyze(Spot s)
        {
            string c = s.Comment ?? "";
            Match lp = rxLocProp.Match(c);
            if (lp.Success)
            {
                bool second = !lp.Groups[1].Success || lp.Groups[1].Value.Length == 0;
                string l1 = second ? lp.Groups[4].Value : lp.Groups[1].Value;
                string pr = second ? lp.Groups[5].Value : lp.Groups[2].Value;
                string l2 = second ? lp.Groups[6].Value : lp.Groups[3].Value;
                s.LocSpotter = l1.ToUpperInvariant(); s.LocDx = l2.ToUpperInvariant();
                s.LocFromComment = true;
                if (!string.IsNullOrEmpty(pr)) s.Prop = NormProp(pr);
            }
            else
            {
                // un solo locator nel commento: di solito e' quello del DX
                Match l = rxLoc.Match(c);
                if (l.Success && Geo.IsLocator(l.Groups[1].Value)) { s.LocDx = l.Groups[1].Value.ToUpperInvariant(); }   // un solo locator: non si impara (potrebbe essere dello spotter)
            }
            if (s.Prop.Length == 0)
            {
                Match p = rxProp.Match(" " + c + " ");
                if (p.Success) s.Prop = NormProp(p.Groups[1].Value);
            }
            Match sn = rxSnr.Match(c);
            if (sn.Success) { double v; if (double.TryParse(sn.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out v)) s.Snr = v; }
            Match md = rxMode.Match(c);
            if (md.Success) s.DigiMode = md.Groups[1].Value.ToUpperInvariant();
        }

        static string NormProp(string p)
        {
            p = p.ToUpperInvariant();
            if (p.StartsWith("E-") || p == "ESKIP" || p == "SPORADIC") return "ES";
            if (p == "AURORA" || p == "AUR") return "AU";   // AUE (aurora-E) resta un modo a se' (G0ISW)
            if (p == "METEOR") return "MS";
            if (p == "TROPO") return "TR";
            return p;
        }

        // coordinate delle due stazioni: locator del commento, altrimenti CALL3
        public static void Locate(Spot s)
        {
            if (!Geo.IsLocator(s.LocSpotter)) { string l = CallDb.Lookup(s.Spotter); s.LocSpotter = l ?? ""; }
            else { if (s.LocFromComment) CallDb.Learn(s.Spotter, s.LocSpotter); s.LocSpotter = CallDb.Refine(s.Spotter, s.LocSpotter); }   // locator non preciso: da CALL3
            if (!Geo.IsLocator(s.LocDx)) { string l = CallDb.Lookup(s.DxCall); s.LocDx = l ?? ""; }
            else { if (s.LocFromComment) CallDb.Learn(s.DxCall, s.LocDx); s.LocDx = CallDb.Refine(s.DxCall, s.LocDx); }
            double la, lo;
            if (Geo.LocToLatLon(s.LocSpotter, out la, out lo)) { s.LatS = la; s.LonS = lo; } else { s.LatS = s.LonS = double.NaN; }
            if (Geo.LocToLatLon(s.LocDx, out la, out lo)) { s.LatD = la; s.LonD = lo; } else { s.LatD = s.LonD = double.NaN; }
        }
    }

    // Archivio degli spot: in memoria (ultimi 3 giorni, al massimo 50000) e su disco, un file CSV al giorno in data/spot/
    public static class SpotStore
    {
        public static readonly List<Spot> All = new List<Spot>();
        static readonly HashSet<string> keys = new HashSet<string>();
        public static string Dir;
        public static event Action<Spot> Added;
        public static bool SaveToDisk = true;
        public static readonly string[] SourceNames = { "Cluster", "RBN", "PSKReporter", "WSJT-X UDP", "APRS-IS" };

        public static void Init(string dataDir)
        {
            Dir = Path.Combine(dataDir, "spot");
            try { Directory.CreateDirectory(Dir); } catch { }
        }

        // aggiunge uno spot (chiamare dal thread dell'interfaccia); false se doppio
        public static bool Add(Spot s)
        {
            if (s == null || !keys.Add(s.Key)) return false;
            All.Add(s);
            if (All.Count > 50000)
            {
                int n = All.Count - 45000;
                for (int i = 0; i < n; i++) keys.Remove(All[i].Key);
                All.RemoveRange(0, n);
            }
            if (SaveToDisk && s.Source != 9) Append(s);
            Action<Spot> a = Added;
            if (a != null) a(s);
            return true;
        }

        // Scrittura su disco a blocchi (ottimizzazione del 28/11/2026): le righe si accumulano e si scrivono ogni 5 secondi
        // (o ogni 500 spot) fuori dal thread dell'interfaccia, invece di aprire e chiudere il file a ogni spot. Il contenuto
        // dei file e' lo stesso; prima di leggere o archiviare i file si scrive tutto quello che e' in attesa (Flush).
        static readonly object pendLock = new object(), writeLock = new object();
        static Dictionary<string, StringBuilder> pend = new Dictionary<string, StringBuilder>();
        static int pendCount;
        static System.Threading.Timer flushTimer;

        static void Append(Spot s)
        {
            try
            {
                string f = Path.Combine(Dir, s.Utc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".csv");
                string line = CsvLine(s);
                bool now;
                lock (pendLock)
                {
                    StringBuilder sb;
                    if (!pend.TryGetValue(f, out sb)) { sb = new StringBuilder(); pend[f] = sb; }
                    sb.AppendLine(line);
                    now = ++pendCount >= 500;
                    if (flushTimer == null) flushTimer = new System.Threading.Timer(_ => Flush(), null, 5000, 5000);
                }
                if (now) System.Threading.ThreadPool.QueueUserWorkItem(_ => Flush());
            }
            catch { }
        }

        // scrive su disco le righe in attesa (chiamabile da qualunque thread)
        public static void Flush()
        {
            lock (writeLock)
            {
                Dictionary<string, StringBuilder> todo;
                lock (pendLock)
                {
                    if (pendCount == 0) return;
                    todo = pend; pend = new Dictionary<string, StringBuilder>(); pendCount = 0;
                }
                foreach (KeyValuePair<string, StringBuilder> kv in todo)
                {
                    try
                    {
                        bool head = !File.Exists(kv.Key);
                        File.AppendAllText(kv.Key, (head ? CsvHeader + Environment.NewLine : "") + kv.Value.ToString());
                    }
                    catch { }
                }
            }
        }

        // formato dei file di data/spot (anche per salvare e ricaricare gli spot di una sessione, linguetta NUBI Es NEL TEMPO)
        public const string CsvHeader = "utc;freq_khz;dx;spotter;loc_dx;loc_spotter;prop;modo;snr;fonte;commento";
        public static string CsvLine(Spot s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(s.Utc.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)).Append(';')
              .Append(s.FreqKHz.ToString("F1", CultureInfo.InvariantCulture)).Append(';')
              .Append(s.DxCall).Append(';').Append(s.Spotter).Append(';')
              .Append(s.LocDx).Append(';').Append(s.LocSpotter).Append(';')
              .Append(s.Prop).Append(';').Append(s.DigiMode).Append(';')
              .Append(double.IsNaN(s.Snr) ? "" : s.Snr.ToString("F0", CultureInfo.InvariantCulture)).Append(';')
              .Append(SourceNames[Math.Max(0, Math.Min(4, s.Source))]).Append(';')
              .Append((s.Comment ?? "").Replace(';', ',').Replace('\n', ' ').Replace('\r', ' '));
            return sb.ToString();
        }

        // una riga di un file di spot; null se non valida
        public static Spot ParseCsvLine(string line)
        {
            string[] p = (line ?? "").Split(';');
            if (p.Length < 11 || p[0] == "utc") return null;
            Spot s = new Spot();
            DateTime t;
            if (!DateTime.TryParseExact(p[0], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out t)) return null;
            s.Utc = DateTime.SpecifyKind(t, DateTimeKind.Utc);
            double fr; double.TryParse(p[1], NumberStyles.Float, CultureInfo.InvariantCulture, out fr); s.FreqKHz = fr;
            s.DxCall = p[2]; s.Spotter = p[3]; s.LocDx = p[4]; s.LocSpotter = p[5]; s.Prop = p[6]; s.DigiMode = p[7];
            double sn; s.Snr = double.TryParse(p[8], NumberStyles.Float, CultureInfo.InvariantCulture, out sn) ? sn : double.NaN;
            s.Source = Array.IndexOf(SourceNames, p[9]); if (s.Source < 0) s.Source = 0;
            s.Comment = p[10];
            double la, lo;
            if (Geo.LocToLatLon(s.LocSpotter, out la, out lo)) { s.LatS = la; s.LonS = lo; }
            if (Geo.LocToLatLon(s.LocDx, out la, out lo)) { s.LatD = la; s.LonD = lo; }
            return s;
        }

        // ARCHIVIO STORICO (dal 22/11/2026, richiesta dell'autore: «adesso che il programma è più stabile, l'archivio spot ha
        // senso»): alla chiusura, prima di svuotare data/spot, ogni giorno si aggiunge compresso in data/archivio_spot/AAAA-MM-GG.csv.gz
        // (stesso formato CSV, si carica in NUBI Es NEL TEMPO e nel BENCHMARK 50 MHz). Si puo' spegnere in IMPOSTAZIONI.
        public static bool Archive = true;
        public static string ArchiveDir { get { return Dir == null ? null : Path.Combine(Path.GetDirectoryName(Dir), "archivio_spot"); } }
        static void ArchiveDay(string csv)
        {
            string ad = ArchiveDir;
            Directory.CreateDirectory(ad);
            string gz = Path.Combine(ad, Path.GetFileNameWithoutExtension(csv) + ".csv.gz");
            List<string> lines = new List<string>();
            HashSet<string> seen = new HashSet<string>();
            if (File.Exists(gz)) foreach (string l in ReadLines(gz)) if (l.Length > 0 && seen.Add(l)) lines.Add(l);
            foreach (string l in File.ReadLines(csv)) if (l.Length > 0 && seen.Add(l)) lines.Add(l);
            if (lines.Count == 0 || lines[0] != CsvHeader) { lines.Remove(CsvHeader); lines.Insert(0, CsvHeader); }
            string tmp = gz + ".tmp";
            using (FileStream fs = File.Create(tmp))
            using (System.IO.Compression.GZipStream z = new System.IO.Compression.GZipStream(fs, System.IO.Compression.CompressionLevel.Optimal))
            using (StreamWriter w = new StreamWriter(z, new UTF8Encoding(false)))
                foreach (string l in lines) w.WriteLine(l);
            if (File.Exists(gz)) File.Delete(gz);
            File.Move(tmp, gz);
        }

        // righe di un file di spot, anche compresso (.gz)
        public static IEnumerable<string> ReadLines(string path)
        {
            Flush();   // righe ancora in attesa di scrittura
            if (!path.EndsWith(".gz", StringComparison.OrdinalIgnoreCase)) { foreach (string l in File.ReadLines(path)) yield return l; yield break; }
            using (FileStream fs = File.OpenRead(path))
            using (System.IO.Compression.GZipStream z = new System.IO.Compression.GZipStream(fs, System.IO.Compression.CompressionMode.Decompress))
            using (StreamReader r = new StreamReader(z, Encoding.UTF8))
            {
                string l;
                while ((l = r.ReadLine()) != null) yield return l;
            }
        }
        public static bool IsSpotFile(string path) { return path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".csv.gz", StringComparison.OrdinalIgnoreCase); }

        // svuota il database degli spot: memoria e file CSV di data/spot (alla chiusura del programma, regola dell'autore)
        public static void ClearAll()
        {
            Flush();
            if (Archive && Dir != null && Directory.Exists(Dir))
                foreach (string f in Directory.GetFiles(Dir, "*.csv")) { try { ArchiveDay(f); } catch { } }
            All.Clear(); keys.Clear();
            try
            {
                if (Dir != null && Directory.Exists(Dir))
                    foreach (string f in Directory.GetFiles(Dir, "*.csv")) { try { File.Delete(f); } catch { } }
            }
            catch { }
        }

        // ricarica gli spot di oggi e di ieri dall'archivio (all'avvio)
        public static void LoadRecent()
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
                        Spot s = ParseCsvLine(line);
                        if (s == null) continue;
                        if (keys.Add(s.Key)) All.Add(s);
                    }
                }
                catch { }
            }
        }

        // spot con ora tra 'from' e 'to'; l'archivio e' in ordine di arrivo, quindi si scorre dalla fine e ci si ferma
        // quando gli spot sono piu' vecchi di 'from' di oltre 15 minuti (margine per gli spot arrivati in ritardo)
        public static List<Spot> Window(DateTime from, DateTime to)
        {
            List<Spot> r = new List<Spot>();
            DateTime stop = from.AddMinutes(-15);
            for (int i = All.Count - 1; i >= 0; i--)
            {
                Spot s = All[i];
                if (s.Utc < stop) break;
                if (s.Utc >= from && s.Utc <= to) r.Add(s);
            }
            return r;
        }

        // spot recenti (ultimi 'minutes' minuti) di una banda, con entrambe le stazioni localizzate
        public static List<Spot> Recent(double minutes, Func<Spot, bool> filter)
        {
            DateTime lim = DateTime.UtcNow.AddMinutes(-minutes);
            List<Spot> r = new List<Spot>();
            for (int i = All.Count - 1; i >= 0; i--)
            {
                Spot s = All[i];
                if (s.Utc < lim) continue;
                if (filter == null || filter(s)) r.Add(s);
            }
            return r;
        }
    }
}
