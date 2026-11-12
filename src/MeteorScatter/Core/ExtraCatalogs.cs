// YA-MS - cataloghi e conteggi esterni per il confronto
//  - IAU Meteor Data Center (MDC), elenco degli sciami stabiliti (streamestablisheddata<anno>.txt, www.ta3.sk/IAUC22DB/MDC2022):
//    tutte le soluzioni pubblicate per ogni sciame (longitudine solare, radiante, velocita', riferimento). Si scarica il file
//    originale e lo si legge qui; il MDC chiede di essere citato (Jenniskens et al. 2020; Jopek & Kanuchova 2017, 2014;
//    Jopek & Jenniskens 2011). Righe di commento con ':', campi separati da '|' tra virgolette.
//  - Global Meteor Network (GMN), riassunto giornaliero delle traiettorie (traj_summary_latest_daily.txt): una riga per
//    traiettoria confermata, con il codice IAU dello sciame ("..." = sporadica). Righe di commento con '#', campi separati da ';'.
//    E' un CONTEGGIO di quanto visto dalle telecamere, non un flusso. Dati CC BY 4.0.
//  - GMN, pagina del flusso: tabella degli sciami con l'indice di popolazione r usato per passare dal flusso allo ZHR.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace YAMS
{
    public class MdcSolution
    {
        public string Code, AdNo, Activity, Reference;
        public double LoS, Ra, De, Vg;
    }

    public static class IauMdc
    {
        public const string UrlYear = "https://www.ta3.sk/IAUC22DB/MDC2022/Etc/streamestablisheddata{0}.txt";
        public const string UrlLatest = "https://iaumdc.ta3.sk/downloads/lists_shw_data/streamestablisheddata.txt";
        public static Dictionary<string, List<MdcSolution>> ByCode = new Dictionary<string, List<MdcSolution>>();
        public static string Edition = "";

        public static string LocalFile(string dataDir) { return Path.Combine(dataDir, "cataloghi", "iau_mdc_stabiliti.txt"); }

        static double P(string s)
        {
            double d;
            return double.TryParse(s.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out d) ? d : double.NaN;
        }

        public static void Load(string dataDir)
        {
            Dictionary<string, List<MdcSolution>> d = new Dictionary<string, List<MdcSolution>>();
            string f = LocalFile(dataDir);
            Edition = "";
            if (File.Exists(f))
            {
                try
                {
                    foreach (string line in File.ReadAllLines(f))
                    {
                        if (line.StartsWith(":"))
                        {
                            // es. ": Last update: Nov 10, 20:00:00 UTC, 2026  Modified by ..."
                            Match m = Regex.Match(line, @"Last update:\s*(.*?\b\d{4})\b", RegexOptions.IgnoreCase);
                            if (m.Success && Edition.Length == 0) Edition = m.Groups[1].Value.Trim();
                            continue;
                        }
                        if (line.IndexOf('|') < 0) continue;
                        string[] c = line.Split('|');
                        if (c.Length < 36) continue;
                        for (int i = 0; i < c.Length; i++) c[i] = c[i].Trim().Trim('"').Trim();
                        // colonne (da 1): 2 IAUNo, 3 AdNo, 4 Code, 8 activity, 11 LoS, 12 Ra, 13 De, 16 Vg, 36 References
                        MdcSolution s = new MdcSolution
                        {
                            Code = c[3].ToUpperInvariant(), AdNo = c[2], Activity = c[7],
                            LoS = P(c[10]), Ra = P(c[11]), De = P(c[12]), Vg = P(c[15]), Reference = c[35]
                        };
                        if (s.Code.Length != 3 || double.IsNaN(s.Ra) || double.IsNaN(s.De)) continue;
                        List<MdcSolution> l;
                        if (!d.TryGetValue(s.Code, out l)) { l = new List<MdcSolution>(); d[s.Code] = l; }
                        l.Add(s);
                    }
                }
                catch { }
                if (Edition.Length == 0) Edition = File.GetLastWriteTimeUtc(f).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            ByCode = d;
        }

        public static double Median(List<double> v)
        {
            if (v.Count == 0) return double.NaN;
            v.Sort();
            return v.Count % 2 == 1 ? v[v.Count / 2] : (v[v.Count / 2 - 1] + v[v.Count / 2]) / 2;
        }

        // distanza angolare tra due radianti (gradi)
        public static double SepDeg(double ra1, double de1, double ra2, double de2)
        {
            double d2r = Math.PI / 180;
            double c = Math.Sin(de1 * d2r) * Math.Sin(de2 * d2r) + Math.Cos(de1 * d2r) * Math.Cos(de2 * d2r) * Math.Cos((ra1 - ra2) * d2r);
            return Math.Acos(Math.Max(-1, Math.Min(1, c))) / d2r;
        }

        // confronto con il catalogo del programma: radiante di YA-MS portato alla longitudine solare di ciascuna soluzione
        public class Comparison
        {
            public int N;
            public double LoSMed, RaMed, DeMed, VgMed, SepMed, SepMax, DLambda, DV;
        }

        public static Comparison Compare(Shower s)
        {
            List<MdcSolution> l;
            if (s.IsAntihelion || !ByCode.TryGetValue(s.Code, out l) || l.Count == 0) return null;
            List<double> los = new List<double>(), ra = new List<double>(), de = new List<double>(), vg = new List<double>(), sep = new List<double>();
            foreach (MdcSolution x in l)
            {
                if (!double.IsNaN(x.LoS)) los.Add(x.LoS);
                ra.Add(x.Ra); de.Add(x.De);
                if (!double.IsNaN(x.Vg)) vg.Add(x.Vg);
                double lam = double.IsNaN(x.LoS) ? s.LambdaMax : x.LoS;
                double dl = Astro.Norm180(lam - s.LambdaMax);
                double r = Astro.Norm360(s.Ra + s.DRa * dl), dd = s.Dec + s.DDec * dl;
                sep.Add(SepDeg(r, dd, x.Ra, x.De));
            }
            Comparison c = new Comparison { N = l.Count };
            c.LoSMed = Median(los); c.RaMed = Median(ra); c.DeMed = Median(de); c.VgMed = Median(vg);
            List<double> sp = new List<double>(sep);
            c.SepMed = Median(sp); c.SepMax = 0; foreach (double x in sep) c.SepMax = Math.Max(c.SepMax, x);
            c.DLambda = double.IsNaN(c.LoSMed) ? double.NaN : Astro.Norm180(s.LambdaMax - c.LoSMed);
            c.DV = double.IsNaN(c.VgMed) ? double.NaN : s.V - c.VgMed;
            return c;
        }
    }

    public static class GmnDaily
    {
        public const string Url = "https://globalmeteornetwork.org/data/traj_summary_data/daily/traj_summary_latest_daily.txt";
        public static Dictionary<string, int> Counts = new Dictionary<string, int>();
        public static int Total, Sporadic;
        public static DateTime FirstUtc = DateTime.MinValue, LastUtc = DateTime.MinValue;
        public static string Generated = "";

        public static string LocalFile(string dataDir) { return Path.Combine(dataDir, "radar", "diretta", "gmn_traiettorie_giorno.txt"); }

        public static void Load(string dataDir)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            int tot = 0, spo = 0; DateTime a = DateTime.MaxValue, b = DateTime.MinValue; string gen = "";
            string f = LocalFile(dataDir);
            if (File.Exists(f))
            {
                try
                {
                    foreach (string line in File.ReadLines(f))
                    {
                        if (line.StartsWith("#"))
                        {
                            int k = line.IndexOf("generated on", StringComparison.OrdinalIgnoreCase);
                            if (k >= 0 && gen.Length == 0) gen = line.Substring(k + 12).Trim();
                            continue;
                        }
                        string[] c = line.Split(';');
                        if (c.Length < 5) continue;
                        // campi (da 0): 0 identificatore, 1 giorno giuliano, 2 ora UTC di inizio, 3 numero IAU, 4 codice IAU ("..." = sporadica)
                        string code = c[4].Trim();
                        tot++;
                        if (code.Length == 0 || code.Trim('.').Length == 0) spo++;
                        else { int n; d.TryGetValue(code, out n); d[code] = n + 1; }
                        DateTime t;
                        if (DateTime.TryParse(c[2].Trim(), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out t))
                        { if (t < a) a = t; if (t > b) b = t; }
                    }
                }
                catch { }
            }
            Counts = d; Total = tot; Sporadic = spo; Generated = gen;
            FirstUtc = tot > 0 && a != DateTime.MaxValue ? a : DateTime.MinValue; LastUtc = tot > 0 ? b : DateTime.MinValue;
        }
    }

    // indice di popolazione r degli sciami dalla tabella della pagina del flusso GMN (gmn_flux.html salvata)
    public static class GmnPopIndex
    {
        public static Dictionary<string, double> R = new Dictionary<string, double>();

        public static void Load(string htmlFile)
        {
            Dictionary<string, double> d = new Dictionary<string, double>();
            try
            {
                if (File.Exists(htmlFile))
                {
                    string html = File.ReadAllText(htmlFile);
                    foreach (Match row in Regex.Matches(html, @"<tr[^>]*>(.*?)</tr>", RegexOptions.Singleline | RegexOptions.IgnoreCase))
                    {
                        List<string> cells = new List<string>();
                        foreach (Match td in Regex.Matches(row.Groups[1].Value, @"<t[dh][^>]*>(.*?)</t[dh]>", RegexOptions.Singleline | RegexOptions.IgnoreCase))
                            cells.Add(Regex.Replace(td.Groups[1].Value, "<[^>]+>", "").Trim());
                        if (cells.Count < 2) continue;
                        string code = null;
                        foreach (string c in cells) if (Regex.IsMatch(c, "^[A-Z]{3}$")) { code = c; break; }
                        if (code == null) continue;
                        // l'indice di popolazione e' l'ultima cella numerica tra 1,5 e 4
                        double r = double.NaN;
                        for (int i = cells.Count - 1; i >= 0; i--)
                        {
                            double x;
                            if (double.TryParse(cells[i], NumberStyles.Float, CultureInfo.InvariantCulture, out x) && x >= 1.5 && x <= 4) { r = x; break; }
                        }
                        if (!double.IsNaN(r) && !d.ContainsKey(code)) d[code] = r;
                    }
                }
            }
            catch { }
            R = d;
        }
    }
}
