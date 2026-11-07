// YA-MS - catalogo degli sciami (data\sciami.csv, data\ant_radiante.csv)
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace YAMS
{
    public class Shower
    {
        public string Code, Name, Type;
        public int StartMonth, StartDay, EndMonth, EndDay;
        public double LambdaMax, Ra, Dec, DRa, DDec, V, PopIndex, Zhr, BUp, BDown;
        public bool IsAntihelion;

        // Giorno dell'anno (1..366 circa) usato per i confronti delle date di attivita'
        static int DayKey(int m, int d) { return m * 32 + d; }

        public bool IsActive(DateTime utc)
        {
            int k = DayKey(utc.Month, utc.Day);
            int s = DayKey(StartMonth, StartDay), e = DayKey(EndMonth, EndDay);
            if (s <= e) return k >= s && k <= e;
            return k >= s || k <= e;   // attivita' a cavallo di fine anno
        }

        // ZHR stimato alla longitudine solare lambda (profilo esponenziale, 0 fuori periodo)
        public double ZhrAt(double lambda, DateTime utc)
        {
            if (!IsActive(utc)) return 0;
            double z = Zhr > 0 ? Zhr : 5.0;          // "Var" -> valore di lavoro 5
            if (IsAntihelion) return z;
            double dl = Astro.Norm180(lambda - LambdaMax);
            double b = dl < 0 ? BUp : BDown;
            return z * Math.Pow(10.0, -b * Math.Abs(dl));
        }

        public void RadiantAt(double lambda, DateTime utc, out double ra, out double dec)
        {
            if (IsAntihelion) { Catalog.AntRadiant(utc, out ra, out dec); return; }
            double dl = Astro.Norm180(lambda - LambdaMax);
            ra = Astro.Norm360(Ra + DRa * dl);
            dec = Dec + DDec * dl;
        }

        public DateTime PeakUtc(int year)
        {
            return Astro.FromJulianDay(Astro.JdOfSolarLongitude(LambdaMax, year));
        }

        // Prossimo massimo a partire da adesso: se quello dell'anno in corso e' passato da piu' di un giorno si passa all'anno dopo.
        // Se il calendario IMO di quell'anno e' in data\massimi_imo.csv si usano data e ora IMO, altrimenti il calcolo da lambda.
        public DateTime NextPeak(DateTime nowUtc, out string text, out string source)
        {
            int y = nowUtc.Year;
            DateTime calc = PeakUtc(y);
            if (calc.AddDays(1) < nowUtc) { y++; calc = PeakUtc(y); }
            string[] imo;
            if (Catalog.ImoPeaks.TryGetValue(y + ";" + Code, out imo))
            try
            {
                string[] md = imo[0].Split('-');
                DateTime d = new DateTime(y, int.Parse(md[0]), int.Parse(md[1]), 0, 0, 0, DateTimeKind.Utc);
                string tm = imo[1].Trim();
                if (tm.Length >= 5)
                {
                    int hh = int.Parse(tm.Substring(0, 2)), mm = int.Parse(tm.Substring(3, 2));
                    text = d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " + tm;
                    source = "IMO " + y;
                    return d.AddHours(hh).AddMinutes(mm);
                }
                text = Lang.F("{0} (~{1} calc.)", d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), calc.ToString("HH:mm", CultureInfo.InvariantCulture));
                source = Lang.F("IMO {0} (giorno)", y);
                return calc.Date == d ? calc : d.AddHours(12);
            }
            catch { }   // riga di massimi_imo.csv scritta male: si usa il calcolo da lambda
            text = calc.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            source = Lang.T("calcolato da λ");
            return calc;
        }

        public override string ToString() { return Code + " - " + Name; }
    }

    public static class Catalog
    {
        public static List<Shower> Showers = new List<Shower>();
        static List<double[]> antTable = new List<double[]>();   // giornoAnno, ra, dec
        public static Dictionary<string, string[]> ImoPeaks = new Dictionary<string, string[]>();   // "anno;codice" -> data, ora
        public static string LoadMessage = "";

        static double P(string s)
        {
            return double.Parse(s.Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public static void Load(string dataDir)
        {
            Showers.Clear(); antTable.Clear();
            LoadMessage = "";   // la finestra puo' essere ricostruita (cambio di lingua): niente messaggi doppi
            string f = Path.Combine(dataDir, "sciami.csv");
            if (!File.Exists(f)) { LoadMessage = Lang.F("File mancante: {0}", f); return; }
            int line = 0;
            foreach (string raw in File.ReadAllLines(f))
            {
                line++;
                string l = raw.Trim();
                if (l.Length == 0 || l.StartsWith("#") || l.StartsWith("codice")) continue;
                string[] c = l.Split(';');
                if (c.Length < 15) { LoadMessage += Lang.F("Riga {0} ignorata (colonne insufficienti)", line) + "\r\n"; continue; }
                try
                {
                    Shower s = new Shower();
                    s.Code = c[0].Trim(); s.Name = c[1].Trim();
                    string[] a = c[2].Split('-'); s.StartMonth = int.Parse(a[0]); s.StartDay = int.Parse(a[1]);
                    string[] b = c[3].Split('-'); s.EndMonth = int.Parse(b[0]); s.EndDay = int.Parse(b[1]);
                    s.LambdaMax = P(c[4]); s.Ra = P(c[5]); s.Dec = P(c[6]); s.DRa = P(c[7]); s.DDec = P(c[8]);
                    s.V = P(c[9]); s.PopIndex = P(c[10]); s.Zhr = P(c[11]); s.BUp = P(c[12]); s.BDown = P(c[13]);
                    s.Type = c[14].Trim();
                    Showers.Add(s);
                }
                catch (Exception ex) { LoadMessage += Lang.F("Riga {0}: {1}", line, ex.Message) + "\r\n"; }
            }
            // Massimi pubblicati dall'IMO
            ImoPeaks.Clear();
            string fm = Path.Combine(dataDir, "massimi_imo.csv");
            if (File.Exists(fm))
                foreach (string raw in File.ReadAllLines(fm))
                {
                    string l = raw.Trim();
                    if (l.Length == 0 || l.StartsWith("#") || l.StartsWith("anno")) continue;
                    string[] c = l.Split(';');
                    if (c.Length < 3 || c[2].Split('-').Length != 2) continue;
                    ImoPeaks[c[0].Trim() + ";" + c[1].Trim()] = new string[] { c[2].Trim(), c.Length > 3 ? c[3] : "" };
                }

            // Sorgente antielio (ANT)
            string fa = Path.Combine(dataDir, "ant_radiante.csv");
            if (File.Exists(fa))
            {
                foreach (string raw in File.ReadAllLines(fa))
                {
                    string l = raw.Trim();
                    if (l.Length == 0 || l.StartsWith("#")) continue;
                    string[] c = l.Split(';');
                    if (c.Length < 3) continue;
                    try
                    {
                        string[] md = c[0].Split('-');
                        double doy = MonthStartDoy(int.Parse(md[0])) + int.Parse(md[1]);
                        antTable.Add(new double[] { doy, P(c[1]), P(c[2]) });
                    }
                    catch { LoadMessage += Lang.F("ant_radiante.csv: riga ignorata: {0}", l) + "\r\n"; }
                }
                antTable.Sort(delegate(double[] x, double[] y) { return x[0].CompareTo(y[0]); });
                Shower ant = new Shower();
                ant.Code = "ANT"; ant.Name = "Sorgente Antielio"; ant.Type = "V";
                ant.StartMonth = 12; ant.StartDay = 10; ant.EndMonth = 9; ant.EndDay = 20;
                ant.Zhr = 4; ant.V = 30; ant.PopIndex = 3.0; ant.IsAntihelion = true;
                Showers.Add(ant);
            }
        }

        static double MonthStartDoy(int m)
        {
            int[] cum = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            return cum[Math.Max(1, Math.Min(12, m)) - 1];
        }

        public static void AntRadiant(DateTime utc, out double ra, out double dec)
        {
            ra = 0; dec = 0;
            if (antTable.Count < 2) return;
            double doy = MonthStartDoy(utc.Month) + utc.Day + utc.TimeOfDay.TotalDays;
            double[] a = null, b = null;
            for (int i = 0; i < antTable.Count; i++)
            {
                if (antTable[i][0] <= doy) a = antTable[i];
                if (antTable[i][0] > doy) { b = antTable[i]; break; }
            }
            if (a == null) { a = antTable[antTable.Count - 1]; a = new double[] { a[0] - 365, a[1], a[2] }; }
            if (b == null) { b = antTable[0]; b = new double[] { b[0] + 365, b[1], b[2] }; }
            double t = (doy - a[0]) / Math.Max(1e-6, b[0] - a[0]);
            double dra = Astro.Norm180(b[1] - a[1]);
            ra = Astro.Norm360(a[1] + dra * t);
            dec = a[2] + (b[2] - a[2]) * t;
        }
    }
}
