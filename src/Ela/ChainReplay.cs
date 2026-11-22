// E-LAYER ANALYZER - verifica delle catene a salti corti (Chains.cs) nel BENCHMARK 50 MHz: la regola si rigioca sui dati del
// passato. Nubi: gli spot di uno o piu' file CSV del programma (DATA COLLECTION / NUBI Es NEL TEMPO, "Esporta gli spot della
// sessione") e i QSO dei log caricati. Per ogni apertura multi-salto della stazione 1 (primo QSO oltre 3100 km del giorno in
// un settore di 30 gradi) si guarda se, nelle due ore prima, la regola avrebbe dato una catena dal QTH nella stessa direzione
// (entro 20 gradi): ALLERTA, PRECURSORE oppure solo il primo salto corto (CERCA LA NUBE), con l'anticipo.
// Poi si contano le allerte e i precursori senza QSO multi-salto nelle due ore dopo (la stazione poteva anche essere spenta).
// Il log da solo non basta: durante le aperture si lavora il DX, non i collegamenti corti, e le nubi lontane si vedono solo
// negli spot di altre stazioni (verificato sul log 50 MHz di IC8TEM, 2017-2026, il 20/11/2026).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using YAMS;

namespace ELA
{
    public static class ChainReplay
    {
        public const double LookBackMin = 120, StepMin = 10, AzTolDeg = 20, AfterMin = 120;
        public const double BenchAfterMin = 60;   // benchmark delle allerte (0.6.5): riuscita entro un'ora (autore, 22/11/2026)

        public class Opening
        {
            public BenchQso Qso;
            public int Level = -1;              // -1 nulla, 0 solo primo salto corto, 1 PRECURSORE, 2 ALLERTA
            public double LeadMin = double.NaN; // anticipo della prima catena (livello >= 1, altrimenti >= 0)
            public string Zone = "";
        }

        public class Result
        {
            public List<Opening> Openings = new List<Opening>();
            public int Spots, CsvSpots, Episodes, EpisodesMissed;
            // benchmark delle allerte (dal 22/11/2026): per livello, quante sono seguite da un QSO o una decodifica multi-salto
            // della stazione 1 nella stessa direzione entro due ore; riferimento = stessi istanti, settori di 30° senza catene
            public int AlN, AlHit, PrN, PrHit;
            public readonly int[] RefN = new int[12], RefHit = new int[12], AlSec = new int[12], PrSec = new int[12];
            public DateTime From, To;
        }

        static double AzDiff(double a, double b) { return Math.Abs(((a - b + 540) % 360) - 180); }

        // QSO del log come spot (la regola degli FT8 isolati non si applica ai QSO dei log, come in BENCHMARK Es)
        public static Spot FromQso(BenchQso q)
        {
            return new Spot { Utc = q.Utc, FreqKHz = q.FreqMHz * 1000, DxCall = q.Call, Spotter = q.MyCall, LocSpotter = q.MyGrid, LocDx = q.Grid, LatS = q.LatTx, LonS = q.LonTx, LatD = q.LatRx, LonD = q.LonRx, Prop = q.Prop, Comment = "ADIF" };
        }

        public static Result Run(List<BenchQso> qsos, List<Spot> csv, double Hkm, Func<BenchQso, bool> isMulti)
        {
            Result r = new Result();
            List<Spot> spots = new List<Spot>(csv);
            spots.AddRange(qsos.Select(FromQso));
            spots.Sort((a, b) => a.Utc.CompareTo(b.Utc));
            r.Spots = spots.Count; r.CsvSpots = csv.Count;
            if (spots.Count == 0) return r;
            r.From = spots[0].Utc; r.To = spots[spots.Count - 1].Utc;
            long[] ticks = spots.Select(s => s.Utc.Ticks).ToArray();
            Func<Spot, bool> isolated = s => s.Comment != "ADIF" && Ft8Isolated(s, spots, ticks);

            // aperture: primo QSO multi-salto della stazione 1 per giorno e settore di 30 gradi
            Dictionary<string, BenchQso> first = new Dictionary<string, BenchQso>();
            foreach (BenchQso q in qsos.Where(x => x.Set == 0 && isMulti(x)).OrderBy(x => x.Utc))
            {
                double az = Geo.Bearing(q.LatTx, q.LonTx, q.LatRx, q.LonRx);
                string k = q.Day + "|" + (int)(az / 30);
                if (!first.ContainsKey(k)) first[k] = q;
            }
            foreach (BenchQso q in first.Values.OrderBy(x => x.Utc))
            {
                Opening o = new Opening { Qso = q };
                double az = Geo.Bearing(q.LatTx, q.LonTx, q.LatRx, q.LonRx);
                using (Igrf.At(Igrf.YearOf(q.Utc)))
                    for (double m = LookBackMin; m >= 0; m -= StepMin)
                    {
                        DateTime t = q.Utc.AddMinutes(-m);
                        int lev; string zone;
                        Best(spots, ticks, t, Hkm, q.LatTx, q.LonTx, az, isolated, out lev, out zone);
                        if (lev > o.Level) { o.Level = lev; o.Zone = zone; }
                        if (lev >= 1 && double.IsNaN(o.LeadMin)) o.LeadMin = m;
                    }
                if (double.IsNaN(o.LeadMin) && o.Level == 0) o.LeadMin = double.NaN;
                r.Openings.Add(o);
            }

            // allerte e precursori senza QSO multi-salto nelle due ore dopo: giorni con QSO della stazione 1, ogni 15 minuti
            List<BenchQso> multi = qsos.Where(x => x.Set == 0 && isMulti(x)).OrderBy(x => x.Utc).ToList();
            BenchQso home = qsos.FirstOrDefault(x => x.Set == 0);
            if (home != null)
            {
                HashSet<string> seen = new HashSet<string>();
                foreach (var day in qsos.Where(x => x.Set == 0).GroupBy(x => x.Utc.Date))
                {
                    DateTime t0 = day.Min(x => x.Utc).AddMinutes(-LookBackMin), t1 = day.Max(x => x.Utc);
                    using (Igrf.At(Igrf.YearOf(t0)))
                        for (DateTime t = t0; t <= t1; t = t.AddMinutes(15))
                        {
                            List<Spot> win = Window(spots, ticks, t, Precursors.WindowMin), hour = Window(spots, ticks, t, 60);
                            if (win.Count == 0) continue;
                            List<StrongCloud> st;
                            List<ChainAlert> ch = Chains.Core(Hkm, win, hour, t, true, home.LatTx, home.LonTx, false, false, false, isolated, out st);
                            List<ChainAlert> mine = ch.Where(x => (x.Kind == ChainAlert.KindQth || x.Kind == ChainAlert.KindEqual) && x.Level >= 1).ToList();
                            Func<double, double, bool> hitIn = (az, min) => multi.Any(m => m.Utc > t && m.Utc <= t.AddMinutes(min) && AzDiff(Geo.Bearing(m.LatTx, m.LonTx, m.LatRx, m.LonRx), az) <= AzTolDeg);
                            Func<double, bool> hitAz = az => hitIn(az, AfterMin), hit1 = az => hitIn(az, BenchAfterMin);
                            foreach (ChainAlert c in mine)
                            {
                                string k = t.ToString("yyyyMMddHH", CultureInfo.InvariantCulture) + "|" + (int)(c.Az / 30);
                                if (!seen.Add(k)) continue;
                                r.Episodes++;
                                if (!hitAz(c.Az)) r.EpisodesMissed++;
                                bool hit = hit1(c.Az);
                                int sec = (int)(Geo.Norm360(c.Az) / 30) % 12;
                                if (c.Level == 2) { r.AlN++; if (hit) r.AlHit++; r.AlSec[sec]++; }
                                else { r.PrN++; if (hit) r.PrHit++; r.PrSec[sec]++; }
                            }
                            // riferimento: i settori di 30° senza catene in questo istante (solo se la stazione era in aria nell'ultima ora)
                            if (multi.Count > 0 && qsos.Any(x => x.Set == 0 && x.Utc <= t && x.Utc > t.AddMinutes(-60)))
                                for (int sec = 0; sec < 12; sec++)
                                {
                                    double az = sec * 30 + 15;
                                    if (mine.Any(c => AzDiff(c.Az, az) <= AzTolDeg + 15)) continue;
                                    r.RefN[sec]++; if (hit1(az)) r.RefHit[sec]++;
                                }
                        }
                }
            }
            return r;
        }

        static List<Spot> Window(List<Spot> spots, long[] ticks, DateTime t, double minutes)
        {
            int i = Array.BinarySearch(ticks, t.AddMinutes(-minutes).Ticks); if (i < 0) i = ~i;
            while (i > 0 && ticks[i - 1] >= t.AddMinutes(-minutes).Ticks) i--;
            List<Spot> l = new List<Spot>();
            for (; i < spots.Count && ticks[i] <= t.Ticks; i++) l.Add(spots[i]);
            return l;
        }

        static void Best(List<Spot> spots, long[] ticks, DateTime t, double Hkm, double lat, double lon, double az, Func<Spot, bool> isolated, out int level, out string zone)
        {
            level = -1; zone = "";
            List<Spot> win = Window(spots, ticks, t, Precursors.WindowMin);
            if (win.Count == 0) return;
            List<StrongCloud> st;
            List<ChainAlert> ch = Chains.Core(Hkm, win, Window(spots, ticks, t, 60), t, true, lat, lon, false, false, true, isolated, out st);
            foreach (ChainAlert c in ch)
            {
                if (AzDiff(c.Az, az) > AzTolDeg) continue;
                if (c.Level > level) { level = c.Level; zone = c.Region; }
            }
        }

        // FT8/FT4 in 50 MHz isolati fra gli spot dei file (stessa regola della linguetta NUBI Es NEL TEMPO)
        static bool Ft8Isolated(Spot s, List<Spot> all, long[] ticks)
        {
            if (!EsModel.UseFt8CloudRule || s.Band != "6m" || !s.IsFt8Ft4) return false;
            double mla, mlo; Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out mla, out mlo);
            long w = TimeSpan.FromMinutes(EsModel.Ft8CloudMin).Ticks;
            int i = Array.BinarySearch(ticks, s.Utc.Ticks - w); if (i < 0) i = ~i;
            while (i > 0 && ticks[i - 1] >= s.Utc.Ticks - w) i--;
            for (; i < all.Count && ticks[i] <= s.Utc.Ticks + w; i++)
            {
                Spot q = all[i];
                if (q == s || q.Band != "6m" || !q.HasBoth) continue;
                if ((q.DxCall == s.DxCall && q.Spotter == s.Spotter) || (q.DxCall == s.Spotter && q.Spotter == s.DxCall)) continue;
                double la, lo; Geo.Intermediate(q.LatS, q.LonS, q.LatD, q.LonD, 0.5, out la, out lo);
                if (Geo.DistanceKm(mla, mlo, la, lo) <= EsModel.Ft8CloudKm) return false;
            }
            return true;
        }

        public static string Report(Result r)
        {
            StringBuilder sb = new StringBuilder();
            int n = r.Openings.Count;
            if (n == 0) { sb.AppendLine(Lang.T("Nessuna apertura multi-salto della stazione 1 nel log.")); return sb.ToString(); }
            int a = r.Openings.Count(o => o.Level == 2), p = r.Openings.Count(o => o.Level == 1), s = r.Openings.Count(o => o.Level == 0), z = r.Openings.Count(o => o.Level < 0);
            List<double> leads = r.Openings.Where(o => o.Level >= 1 && !double.IsNaN(o.LeadMin)).Select(o => o.LeadMin).OrderBy(x => x).ToList();
            sb.AppendLine(Lang.F("Verifica delle catene a salti corti: {0} aperture multi-salto della stazione 1 (primo QSO oltre 3100 km per giorno e settore di 30°), dal {1:yyyy-MM-dd} al {2:yyyy-MM-dd}.", n, r.From, r.To));
            sb.AppendLine(Lang.F("Nubi da {0} spot, QSO e decodifiche ({1} dalle sorgenti caricate).", r.Spots, r.CsvSpots));
            sb.AppendLine(Lang.F("Nelle due ore prima, nella stessa direzione (±20°): ALLERTA {0} ({1:F0}%), PRECURSORE {2} ({3:F0}%), solo il primo salto corto {4} ({5:F0}%), nulla {6} ({7:F0}%).",
                a, 100.0 * a / n, p, 100.0 * p / n, s, 100.0 * s / n, z, 100.0 * z / n));
            if (leads.Count > 0) sb.AppendLine(Lang.F("Anticipo mediano di ALLERTA e PRECURSORE: {0:F0} minuti.", leads[leads.Count / 2]));
            if (r.Episodes > 0) sb.AppendLine(Lang.F("ALLERTE e PRECURSORI dal QTH nei giorni del log: {0}; senza QSO multi-salto nelle due ore dopo: {1} ({2:F0}%, ma la stazione poteva anche non essere in aria).", r.Episodes, r.EpisodesMissed, 100.0 * r.EpisodesMissed / r.Episodes));
            // benchmark delle allerte: riuscite contro il riferimento senza catene, pesato sugli stessi settori
            Func<int[], double> refPct = w =>
            {
                double num = 0, den = 0;
                for (int i = 0; i < 12; i++) if (r.RefN[i] > 0 && w[i] > 0) { num += w[i] * (double)r.RefHit[i] / r.RefN[i]; den += w[i]; }
                return den > 0 ? 100 * num / den : double.NaN;
            };
            if (r.AlN > 0)
            {
                double rf = refPct(r.AlSec);
                sb.AppendLine(Lang.F("Benchmark: {0} ALLERTE, seguite da un QSO o una decodifica multi-salto nella stessa direzione entro un'ora: {1:F0}% (falsi allarmi {2:F0}%).", r.AlN, 100.0 * r.AlHit / r.AlN, 100.0 - 100.0 * r.AlHit / r.AlN)
                    + (double.IsNaN(rf) ? "" : " " + Lang.F("Riferimento negli stessi settori senza catene: {0:F0}%.", rf)));
            }
            if (r.PrN > 0)
            {
                double rf = refPct(r.PrSec);
                sb.AppendLine(Lang.F("Benchmark: {0} PRECURSORI, seguiti da un QSO o una decodifica multi-salto nella stessa direzione entro un'ora: {1:F0}%.", r.PrN, 100.0 * r.PrHit / r.PrN)
                    + (double.IsNaN(rf) ? "" : " " + Lang.F("Riferimento negli stessi settori senza catene: {0:F0}%.", rf)));
            }
            if (r.CsvSpots == 0) sb.AppendLine(Lang.T("Senza sorgenti caricate le nubi vengono solo dai QSO del log: l'ultimo salto corto, lontano, quasi non si vede. Per una verifica vera carica gli spot esportati da NUBI Es NEL TEMPO, i log ADIF o le decodifiche ALL.TXT di altre stazioni, meglio se lontane."));
            sb.AppendLine();
            foreach (Opening o in r.Openings.Take(60))
            {
                string lv = o.Level == 2 ? Lang.T("ALLERTA") : o.Level == 1 ? Lang.T("PRECURSORE") : o.Level == 0 ? Lang.T("CERCA LA NUBE") : "-";
                sb.AppendLine(o.Qso.Utc.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) + "Z  " + o.Qso.Call + " (" + o.Qso.Grid + ")  " + o.Qso.Path.DistKm.ToString("F0", CultureInfo.CurrentCulture) + " km  "
                    + lv + (double.IsNaN(o.LeadMin) ? "" : "  " + Lang.F("{0:F0} min prima", o.LeadMin)) + (o.Zone.Length > 0 ? "  " + o.Zone : ""));
            }
            if (r.Openings.Count > 60) sb.AppendLine(Lang.F("... e altre {0}.", r.Openings.Count - 60));
            return sb.ToString();
        }
    }
}
