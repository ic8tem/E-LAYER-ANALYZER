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
        public int Source;            // 0 = cluster a scelta, 1 = RBN, 2 = cluster con PSKReporter, 9 = prova
        public string Prop = "";      // modo di propagazione dichiarato: ES, FAI, AU, MS, TR, EME, F2, TEP
        public string LocSpotter = "", LocDx = "";
        public bool LocFromComment;   // due locator letti nel commento nella forma LOC<modo>LOC (si imparano per CALL3)
        public double LatS = double.NaN, LonS = double.NaN, LatD = double.NaN, LonD = double.NaN;
        public double Snr = double.NaN;
        public string DigiMode = "";  // CW, FT8, FT4, ... (RBN e PSKReporter)

        public double FreqMHz { get { return FreqKHz / 1000.0; } }

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
                if (Prop == "MS" || rxMsMode.IsMatch((Comment ?? "") + " " + (DigiMode ?? ""))) return true;
                return YAMS.MsCollector.UseWindows && YAMS.MsCollector.OtherTag(Comment) == null && YAMS.MsCollector.WindowOf(FreqKHz) != null;
            }
        }
        // Spot di aurora (regola dell'autore): <AU> o <AUR> (anche AUE, AURORA) nel testo, oppure modo dichiarato AU
        static readonly System.Text.RegularExpressions.Regex rxAu = new System.Text.RegularExpressions.Regex(@"<\s*(AURORA|AUR|AUE|AU)\s*>",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        public bool IsAurora { get { return (Prop == "AU" || Filters.AuTag(Comment)) && !IsAuroraE; } }
        // aurora-E (Au-E): E sporadico di origine aurorale, senza Doppler, sul percorso diretto, dopo l'evento aurorale (G0ISW [33]);
        // riconosciuto da AUE, AU-E o AURORAL E nel testo, oppure dal modo dichiarato AUE
        static readonly System.Text.RegularExpressions.Regex rxAuE = new System.Text.RegularExpressions.Regex(@"(?<![A-Z0-9])(AUE|AU-E|AURORAL[- ]?E)(?![A-Z0-9])",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        public bool IsAuroraE { get { return Prop == "AUE" || rxAuE.IsMatch(Comment ?? ""); } }
        // MS sicuro: modo dichiarato MS, modo digitale MS o <MS> (la sola finestra di frequenza del bandplan e' un indizio)
        public bool IsMeteorScatterSure { get { return Prop == "MS" || rxMsMode.IsMatch((Comment ?? "") + " " + (DigiMode ?? "")); } }
        // In 50 MHz il bandplan non e' un criterio esclusivo (regola dell'autore): la sola finestra MS non toglie lo spot all'E-sporadico
        public bool TaggedNotEs { get { return Filters.NotEsTag(Comment) || (Band == "6m" && Filters.Es50WindowNotExclusive ? IsMeteorScatterSure : IsMeteorScatter); } }
        // In 50 MHz entro EsModel.MinKm50 (450 km) un collegamento non e' E-sporadico (regola dell'autore);
        // in 28 MHz la distanza minima si scala da quella dei 50 MHz seguendo IK3XTV (EsModel.MinKm28)
        public bool TooShortForEs { get { return HasBoth && ((Band == "6m" && DistanceKm < EsModel.MinKm50) || (Band == "10m" && DistanceKm < EsModel.MinKm28)); } }
        // utile per l'E-sporadico: modo dichiarato compatibile e nessuna etichetta <MS>/<AU>/<TR> nel testo
        public bool EsCandidate { get { return Prop != "TR" && Prop != "MS" && Prop != "AU" && Prop != "FAI" && Prop != "EME" && Prop != "TEP" && !TaggedNotEs && !TooShortForEs && !Filters.FaiTag(Comment) && !IsAurora && !IsAuroraE && Prop != "AUE"; } }   // <FAI> o fai (maiuscolo, minuscolo o misto) = F.A.I., non Es
        // transequatoriale per la regola dell'autore (EsModel.IsTransEquatorial), salvo che lo spot dichiari ES
        public bool IsTep { get { return Prop == "TEP" || (Prop != "ES" && HasBoth && EsModel.IsTransEquatorial(LatS, LonS, LatD, LonD)); } }
        public bool HasBoth { get { return !double.IsNaN(LatS) && !double.IsNaN(LatD); } }
        public double DistanceKm { get { return HasBoth ? Geo.DistanceKm(LatS, LonS, LatD, LonD) : double.NaN; } }
        public string Band { get { return Bands.Name(FreqKHz); } }
        public string Key { get { return DxCall + "|" + Spotter + "|" + Math.Round(FreqKHz) + "|" + Utc.ToString("yyyyMMddHHmm"); } }
    }

    public static class Bands
    {
        // nome della banda dalla frequenza (kHz)
        public static string Name(double kHz)
        {
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
        public static readonly string[] SourceNames = { "Cluster", "RBN", "PSKReporter" };

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

        static void Append(Spot s)
        {
            try
            {
                string f = Path.Combine(Dir, s.Utc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".csv");
                bool head = !File.Exists(f);
                StringBuilder sb = new StringBuilder();
                if (head) sb.AppendLine("utc;freq_khz;dx;spotter;loc_dx;loc_spotter;prop;modo;snr;fonte;commento");
                sb.Append(s.Utc.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)).Append(';')
                  .Append(s.FreqKHz.ToString("F1", CultureInfo.InvariantCulture)).Append(';')
                  .Append(s.DxCall).Append(';').Append(s.Spotter).Append(';')
                  .Append(s.LocDx).Append(';').Append(s.LocSpotter).Append(';')
                  .Append(s.Prop).Append(';').Append(s.DigiMode).Append(';')
                  .Append(double.IsNaN(s.Snr) ? "" : s.Snr.ToString("F0", CultureInfo.InvariantCulture)).Append(';')
                  .Append(SourceNames[Math.Max(0, Math.Min(2, s.Source))]).Append(';')
                  .Append((s.Comment ?? "").Replace(';', ',')).AppendLine();
                File.AppendAllText(f, sb.ToString());
            }
            catch { }
        }

        // svuota il database degli spot: memoria e file CSV di data/spot (alla chiusura del programma, regola dell'autore)
        public static void ClearAll()
        {
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
                        string[] p = line.Split(';');
                        if (p.Length < 11 || p[0] == "utc") continue;
                        Spot s = new Spot();
                        DateTime t;
                        if (!DateTime.TryParseExact(p[0], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out t)) continue;
                        s.Utc = DateTime.SpecifyKind(t, DateTimeKind.Utc);
                        double fr; double.TryParse(p[1], NumberStyles.Float, CultureInfo.InvariantCulture, out fr); s.FreqKHz = fr;
                        s.DxCall = p[2]; s.Spotter = p[3]; s.LocDx = p[4]; s.LocSpotter = p[5]; s.Prop = p[6]; s.DigiMode = p[7];
                        double sn; s.Snr = double.TryParse(p[8], NumberStyles.Float, CultureInfo.InvariantCulture, out sn) ? sn : double.NaN;
                        s.Source = Array.IndexOf(SourceNames, p[9]); if (s.Source < 0) s.Source = 0;
                        s.Comment = p[10];
                        double la, lo;
                        if (Geo.LocToLatLon(s.LocSpotter, out la, out lo)) { s.LatS = la; s.LonS = lo; }
                        if (Geo.LocToLatLon(s.LocDx, out la, out lo)) { s.LatD = la; s.LonD = lo; }
                        if (keys.Add(s.Key)) All.Add(s);
                    }
                }
                catch { }
            }
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
