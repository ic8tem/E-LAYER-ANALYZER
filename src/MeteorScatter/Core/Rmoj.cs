// YA-MS - conteggi radio mensili del Giappone (RMOJ, "Radio Meteor Observation in Japan", IPRMO)
// File di testo mensili www.iprmo.org/library/rmoj/AAAA/rmojAAAAMM.txt (codifica Shift-JIS):
// per ogni osservatore un blocco con Observer, Location (coordinate), Frequency, Transmitter e la tabella
// "Total number of echoes" (righe = giorni, colonne = ore 00-23 in JST o UT). Celle: numero, "%n" = stima su meno
// di 60 minuti, "--" / "???" = dato perso, "NS" / "IF" = rumore o interferenza, "ES"/"Es" = sporadico E, "AI".
// Due impaginazioni: a colonne fisse ("May| 00 01 ...") oppure con le barre ("May| 00h| 01h| ...").
// Le tabelle in UT cominciano spesso con l'ultimo giorno del mese prima.
// Uso: taratura e storico per il Giappone (fari HAM a 53 MHz), non previsione in diretta (escono con settimane di ritardo).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace YAMS
{
    public class RmojObserver
    {
        public string Name = "", Location = "", Beacon = "", BeaconPlace = "", Tz = "JST";
        public double Lat = double.NaN, Lon = double.NaN, BeaconLat = double.NaN, BeaconLon = double.NaN, FreqMHz;
        public Dictionary<DateTime, double> Counts = new Dictionary<DateTime, double>();   // inizio dell'ora UTC -> echi
        public int Estimated, Missing, Sporadic;
        public bool HasPath { get { return !double.IsNaN(Lat) && !double.IsNaN(BeaconLat); } }
        public override string ToString() { return Name + " - " + FreqMHz.ToString("0.000", CultureInfo.InvariantCulture) + " MHz"; }
    }

    public static class Rmoj
    {
        public const string UrlTemplate = "https://www.iprmo.org/library/rmoj/{AAAA}/rmoj{AAAAMM}.txt";

        // fari HAM per l'osservazione radio delle meteore (IPRMO, hro-ogaki.org, sito di H. Sugimoto; 2024-2026)
        static readonly (double f, string call, string place, double lat, double lon)[] Beacons =
        {
            (53.100, "JA2SDA", "Toyokawa, Aichi", 34.838, 137.291),
            (53.372, "JE3AKE", "Otsu, Shiga", 34.950, 135.900),
            (52.905, "JJ1RLQ", "Higashimurayama / Tachikawa, Tokyo", 35.767, 139.467),
            (53.750, "JA9YDB", "Sabae, Fukui", 35.95, 136.18),
            (53.755, "JH9YYA", "Fukui", 36.06, 136.22),
            (50.017, "JA6YBR", "Miyazaki", 31.83, 131.42),
        };

        public static string Url(int year, int month)
        {
            string ym = year.ToString("0000", CultureInfo.InvariantCulture) + month.ToString("00", CultureInfo.InvariantCulture);
            return UrlTemplate.Replace("{AAAAMM}", ym).Replace("{AAAA}", year.ToString("0000", CultureInfo.InvariantCulture));
        }

        public static string FileFor(string dataDir, int year, int month)
        {
            return Path.Combine(dataDir, "radar", "rmoj", "rmoj" + year.ToString("0000", CultureInfo.InvariantCulture) + month.ToString("00", CultureInfo.InvariantCulture) + ".txt");
        }

        static Encoding Enc()
        {
            try { Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); } catch { }
            try { return Encoding.GetEncoding(932); } catch { return Encoding.UTF8; }
        }

        // il file scaricato e' un RMOJ valido (non la pagina 404 del sito)?
        public static bool IsValid(string file)
        {
            try
            {
                if (!File.Exists(file) || new FileInfo(file).Length < 500) return false;
                using (StreamReader r = new StreamReader(file, Enc())) return (r.ReadLine() ?? "").StartsWith("Radio Meteor Observation", StringComparison.Ordinal);
            }
            catch { return false; }
        }

        // Scarica i mesi mancanti, dal mese corrente indietro per 'months' mesi. I mesi chiusi da piu' di 3 mesi
        // gia' presenti non si riscaricano. Restituisce i mesi disponibili sul disco (piu' recente per primo).
        public static List<DateTime> Update(string dataDir, int months, out string msg)
        {
            List<DateTime> have = new List<DateTime>();
            int got = 0, fail = 0; string lastErr = "";
            DateTime now = DateTime.UtcNow, m0 = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            for (int k = 0; k < months; k++)
            {
                DateTime m = m0.AddMonths(-k);
                string file = FileFor(dataDir, m.Year, m.Month);
                bool old = k >= 3 && IsValid(file);
                if (!old)
                {
                    string tmp = file + ".dl";
                    DateTime lm = DateTime.MinValue; bool off; string err;
                    int r = RadarFlux.Fetch(Url(m.Year, m.Month), tmp, ref lm, out off, out err);
                    if (r == 1 && IsValid(tmp)) { if (File.Exists(file)) File.Delete(file); File.Move(tmp, file); got++; }
                    else
                    {
                        try { if (File.Exists(tmp)) File.Delete(tmp); } catch { }
                        if (r < 0 && err.Length > 0 && !err.Contains("404")) { fail++; lastErr = err; }
                    }
                }
                if (IsValid(file)) have.Add(m);
            }
            msg = Lang.F("{0} mesi disponibili, {1} scaricati ora", have.Count, got) + (fail > 0 ? " - " + Lang.F("errore: {0}", lastErr) : "");
            return have;
        }

        public static List<DateTime> OnDisk(string dataDir)
        {
            List<DateTime> res = new List<DateTime>();
            string dir = Path.Combine(dataDir, "radar", "rmoj");
            if (!Directory.Exists(dir)) return res;
            foreach (string f in Directory.GetFiles(dir, "rmoj??????.txt"))
            {
                string n = Path.GetFileNameWithoutExtension(f).Substring(4);
                int y, mo;
                if (int.TryParse(n.Substring(0, 4), out y) && int.TryParse(n.Substring(4, 2), out mo) && mo >= 1 && mo <= 12 && IsValid(f))
                    res.Add(new DateTime(y, mo, 1, 0, 0, 0, DateTimeKind.Utc));
            }
            res.Sort((a, b) => b.CompareTo(a));
            return res;
        }

        static double Num(string s)
        {
            double v; return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v) ? v : double.NaN;
        }

        // coordinate "(34.96 N, 136.61 E)", "(140.1 E, 39.4 N)", "JAPAN(139.71E, 35.82N)"
        static void Coords(string loc, out double lat, out double lon)
        {
            lat = double.NaN; lon = double.NaN;
            foreach (Match m in Regex.Matches(loc, @"(-?\d+(?:\.\d+)?)\s*([NSEW])\b"))
            {
                double v = Num(m.Groups[1].Value); string h = m.Groups[2].Value;
                if (h == "N") lat = v; else if (h == "S") lat = -v; else if (h == "E") lon = v; else lon = -v;
            }
        }

        public static List<RmojObserver> Parse(string file, int year, int month)
        {
            List<RmojObserver> res = new List<RmojObserver>();
            string[] lines = File.ReadAllText(file, Enc()).Replace("\r", "").Split('\n');
            List<List<string>> blocks = new List<List<string>>(); List<string> cur = new List<string>();
            foreach (string ln in lines)
            {
                if (Regex.IsMatch(ln, @"^-{20,}\s*$")) { blocks.Add(cur); cur = new List<string>(); }
                else cur.Add(ln);
            }
            blocks.Add(cur);
            DateTime m0 = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            foreach (List<string> b in blocks)
            {
                int iTot = b.FindIndex(x => x.Contains("Total number of echoes"));
                if (iTot < 0) continue;
                RmojObserver o = new RmojObserver();
                foreach (string x in b.GetRange(0, iTot))
                {
                    Match m = Regex.Match(x, @"^\s*Observer\s*:\s*(.+)$"); if (m.Success && o.Name == "") o.Name = m.Groups[1].Value.Trim();
                    m = Regex.Match(x, @"^\s*Location\s*:\s*(.+)$"); if (m.Success && o.Location == "") { o.Location = m.Groups[1].Value.Trim(); Coords(o.Location, out o.Lat, out o.Lon); }
                    m = Regex.Match(x, @"^\s*Frequency\s*:\s*(\d+(?:\.\d+)?)"); if (m.Success && o.FreqMHz == 0) o.FreqMHz = Num(m.Groups[1].Value);
                }
                if (o.FreqMHz <= 0 || o.FreqMHz > 100) continue;          // VOR aeronautici e blocchi senza frequenza: esclusi
                if (o.Name == "") o.Name = o.Location;
                foreach (var bc in Beacons)
                    if (Math.Abs(bc.f - o.FreqMHz) < 0.002) { o.Beacon = bc.call; o.BeaconPlace = bc.place; o.BeaconLat = bc.lat; o.BeaconLon = bc.lon; }
                string hdr = null; bool seenLow = false;
                for (int i = iTot + 1; i < b.Count; i++)
                {
                    string x = b[i];
                    if (x.Contains("longer than")) break;
                    if (hdr == null && Regex.IsMatch(x, @"^\s*[A-Za-z]{3}\s*\|") && x.Contains("23")) { hdr = x; o.Tz = Regex.IsMatch(x, @"UT\s*$") ? "UT" : "JST"; continue; }
                    Match m = Regex.Match(x, @"^\s*(\d{1,2})\s*\|(.*)$");
                    if (hdr == null || !m.Success) continue;
                    int d = int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
                    List<string> cells = new List<string>();
                    if (hdr.Contains("h|"))
                        foreach (string c in m.Groups[2].Value.Split('|')) cells.Add(c.Trim());
                    else
                    {
                        int off = hdr.IndexOf('|');
                        foreach (Match p in Regex.Matches(hdr.Substring(off), @"\d\d"))
                        {
                            int s = off + p.Index - 1;
                            cells.Add(s < x.Length ? x.Substring(s, Math.Min(3, x.Length - s)).Trim() : "");
                        }
                    }
                    bool prev = d > 20 && !seenLow;
                    if (d <= 10) seenLow = true;
                    DateTime day;
                    try { day = prev ? new DateTime(m0.AddMonths(-1).Year, m0.AddMonths(-1).Month, d, 0, 0, 0, DateTimeKind.Utc) : new DateTime(year, month, d, 0, 0, 0, DateTimeKind.Utc); }
                    catch { continue; }
                    for (int h = 0; h < 24 && h < cells.Count; h++)
                    {
                        string c = cells[h];
                        if (c.Length == 0 || c == "--" || c.Contains("?") ) { o.Missing++; continue; }
                        if (c.StartsWith("E", StringComparison.OrdinalIgnoreCase)) { o.Sporadic++; continue; }
                        bool est = c.StartsWith("%");
                        double v = Num(c.TrimStart('%'));
                        if (double.IsNaN(v)) { o.Missing++; continue; }
                        if (est) o.Estimated++;
                        DateTime utc = day.AddHours(h - (o.Tz == "JST" ? 9 : 0));
                        o.Counts[utc] = v;
                    }
                }
                if (o.Counts.Count > 0) res.Add(o);
            }
            return res;
        }

        // media per ora UTC (0-23) sui giorni del mese con dato; -1 se nessun dato
        public static double[] HourlyMean(RmojObserver o, int year, int month)
        {
            double[] s = new double[24]; int[] n = new int[24];
            foreach (KeyValuePair<DateTime, double> kv in o.Counts)
                if (kv.Key.Year == year && kv.Key.Month == month) { s[kv.Key.Hour] += kv.Value; n[kv.Key.Hour]++; }
            double[] r = new double[24];
            for (int h = 0; h < 24; h++) r[h] = n[h] > 0 ? s[h] / n[h] : -1;
            return r;
        }

        // totale giornaliero UTC (solo giorni con almeno 20 ore valide); -1 altrimenti
        public static double[] DailyTotals(RmojObserver o, int year, int month)
        {
            int nd = DateTime.DaysInMonth(year, month);
            double[] s = new double[nd]; int[] n = new int[nd];
            foreach (KeyValuePair<DateTime, double> kv in o.Counts)
                if (kv.Key.Year == year && kv.Key.Month == month) { s[kv.Key.Day - 1] += kv.Value; n[kv.Key.Day - 1]++; }
            double[] r = new double[nd];
            for (int d = 0; d < nd; d++) r[d] = n[d] >= 20 ? s[d] * 24.0 / n[d] : -1;
            return r;
        }
    }
}
