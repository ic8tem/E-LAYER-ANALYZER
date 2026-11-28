// Misura delle prestazioni (solo con --screenshot, alla fine): tempi dei calcoli principali e memoria, con spot sintetici
// riproducibili, e un'impronta dei risultati per verificare che le ottimizzazioni non cambino i numeri (dal 28/11/2026).
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using YAMS;

namespace ELA
{
    public partial class ElaWindow
    {
        static string F(double v) { return v.ToString("R", CultureInfo.InvariantCulture); }

        static double Median(List<double> v) { v.Sort(); return v.Count == 0 ? double.NaN : v[v.Count / 2]; }

        async Task RunPerf(string dir)
        {
            StringBuilder t = new StringBuilder(), r = new StringBuilder();
            CultureInfo ic = CultureInfo.InvariantCulture;
            Process pr = Process.GetCurrentProcess();
            t.AppendLine("MISURA DELLE PRESTAZIONI " + Version + " " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm", ic) + " UTC, " + Environment.ProcessorCount + " processori");
            Func<string> mem = () => { GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); pr.Refresh();
                return string.Format(ic, "gestita {0:F1} MB, processo {1:F1} MB", GC.GetTotalMemory(true) / 1048576.0, pr.WorkingSet64 / 1048576.0); };
            t.AppendLine("Memoria prima degli spot: " + mem());

            // 1) spot sintetici: 20000 spot negli ultimi 60 minuti (Europa e Mediterraneo; 10, 6, 2 m; FT8, CW, SSB)
            Random rnd = new Random(12345);
            string[] pre = { "I", "IK", "IZ", "DL", "F", "G", "EA", "OH", "SM", "SV", "9A", "OK", "SP", "LZ", "YO", "HA", "OE", "HB9", "PA", "ON", "CT", "EI", "LA", "OZ", "S5", "YU", "UR", "ES", "YL", "LY" };
            List<Spot> syn = new List<Spot>();
            DateTime now = DateTime.UtcNow;
            for (int i = 0; i < 20000; i++)
            {
                double la1 = 35 + rnd.NextDouble() * 28, lo1 = -10 + rnd.NextDouble() * 45;
                double la2 = 33 + rnd.NextDouble() * 32, lo2 = -15 + rnd.NextDouble() * 55;
                int b = rnd.Next(10);
                double f = b < 6 ? 50313 + rnd.Next(3) : b < 8 ? 28074 : b < 9 ? 50090 + rnd.Next(60) : 144174 + rnd.Next(200);
                string mode = f % 1 == 0 && (f == 50313 || f == 50314 || f == 50315 || f == 28074 || f >= 144174) ? "FT8" : "CW";
                Spot s = new Spot
                {
                    Utc = now.AddSeconds(-rnd.Next(3600)), FreqKHz = f,
                    DxCall = pre[rnd.Next(pre.Length)] + rnd.Next(10) + (char)('A' + rnd.Next(26)) + (char)('A' + rnd.Next(26)) + (char)('A' + rnd.Next(26)),
                    Spotter = pre[rnd.Next(pre.Length)] + rnd.Next(10) + (char)('A' + rnd.Next(26)) + (char)('A' + rnd.Next(26)),
                    LocSpotter = Geo.LatLonToLoc(la1, lo1, 6), LocDx = Geo.LatLonToLoc(la2, lo2, 6),
                    Source = mode == "FT8" ? 2 : 1, DigiMode = mode, Snr = -20 + rnd.Next(30),
                    Comment = mode == "FT8" ? "FT8 " + (-20 + rnd.Next(30)) + " dB" : (rnd.Next(4) == 0 ? Geo.LatLonToLoc(la1, lo1, 4) + "<ES>" + Geo.LatLonToLoc(la2, lo2, 4) : "CW " + (5 + rnd.Next(25)) + " dB 22 WPM CQ")
                };
                double a, o;
                if (Geo.LocToLatLon(s.LocSpotter, out a, out o)) { s.LatS = a; s.LonS = o; }
                if (Geo.LocToLatLon(s.LocDx, out a, out o)) { s.LatD = a; s.LonD = o; }
                syn.Add(s);
            }
            bool save = SpotStore.SaveToDisk;
            Stopwatch sw = Stopwatch.StartNew();
            foreach (Spot s in syn) SpotStore.Add(s);
            sw.Stop();
            t.AppendLine(string.Format(ic, "A. Arrivo di 20000 spot con archivio su disco: {0:F0} ms ({1:F3} ms per spot)", sw.Elapsed.TotalMilliseconds, sw.Elapsed.TotalMilliseconds / 20000));
            t.AppendLine("Memoria con gli spot: " + mem());

            // 2) linguetta SPORADIC-E: ricalcolo completo (come il timer di 3 secondi)
            tabs.Select(0);
            await Task.Delay(1500);
            List<double> ms = new List<double>();
            for (int k = 0; k < 5; k++) { sw.Restart(); es.Refresh(); sw.Stop(); ms.Add(sw.Elapsed.TotalMilliseconds); }
            t.AppendLine(string.Format(ic, "B. SPORADIC-E, ricalcolo completo: mediana {0:F0} ms su 5", Median(ms)));
            r.AppendLine(es.PerfSignature());

            // 3) precursori e catene (timer di 1 minuto)
            ms.Clear(); List<PrecursorAlert> al = null;
            for (int k = 0; k < 5; k++) { sw.Restart(); al = Precursors.Compute(es.H); sw.Stop(); ms.Add(sw.Elapsed.TotalMilliseconds); }
            t.AppendLine(string.Format(ic, "C. Precursori e catene: mediana {0:F0} ms su 5", Median(ms)));
            r.AppendLine("PRECURSORI " + al.Count);
            foreach (PrecursorAlert p in al.OrderBy(x => x.Key, StringComparer.Ordinal)) r.AppendLine(p.Key + " " + p.Level + " " + F(p.DistKm) + " " + F(p.Az) + " " + p.Covered);

            // 4) classificazione degli spot (proprieta' lette nei filtri)
            sw.Restart(); int n1 = 0;
            for (int k = 0; k < 5; k++) foreach (Spot s in syn) { if (s.EsCandidate) n1++; if (s.IsTep) n1++; if (s.DistanceKm > 1000) n1++; if (s.Key.Length > 0) n1++; }
            sw.Stop();
            t.AppendLine(string.Format(ic, "D. Classificazione di 100000 spot (E-sporadico, TEP, distanza, chiave): {0:F0} ms", sw.Elapsed.TotalMilliseconds));
            r.AppendLine("CLASSIFICAZIONE " + n1);

            // 5) campo magnetico IGRF
            sw.Restart(); double acc = 0;
            for (int i = 0; i < 100000; i++)
            {
                double bn, be, bd;
                Igrf.Field(-60 + (i % 120), -180 + (i * 7 % 360), 6481.2, out bn, out be, out bd);
                acc += bn + be + bd;
            }
            sw.Stop();
            t.AppendLine(string.Format(ic, "E. IGRF, 100000 punti: {0:F0} ms", sw.Elapsed.TotalMilliseconds));
            r.AppendLine("IGRF " + F(acc));
            sw.Restart();
            List<PerpPoint> loc = FieldAligned.PairLocus(40.56, 14.21, 50.13, -2.62, 110, 2.5, 0, 2500, 0.25, 4.0 / 3);
            sw.Stop();
            t.AppendLine(string.Format(ic, "F. Luogo F.A.I. IC8FAX-G4LOH (passo 0,25°): {0:F0} ms, {1} punti", sw.Elapsed.TotalMilliseconds, loc.Count));
            double so = 0; foreach (PerpPoint p in loc) so += p.Off + p.Elev + p.Az;
            r.AppendLine("LUOGO " + loc.Count + " " + F(so));

            // 6) disegno della finestra con la mappa SPORADIC-E (azimutale, reticolo dei locator)
            ms.Clear();
            PixelSize ps = new PixelSize(Math.Max(100, (int)Bounds.Width), Math.Max(100, (int)Bounds.Height));
            for (int k = 0; k < 6; k++)
            {
                sw.Restart();
                using (RenderTargetBitmap rtb = new RenderTargetBitmap(ps, new Vector(96, 96))) rtb.Render(this);
                sw.Stop(); if (k > 0) ms.Add(sw.Elapsed.TotalMilliseconds);
            }
            t.AppendLine(string.Format(ic, "G. Disegno della finestra (SPORADIC-E): mediana {0:F0} ms su 5", Median(ms)));
            t.AppendLine("Memoria alla fine: " + mem());
            SpotStore.SaveToDisk = save;
            try { File.WriteAllText(Path.Combine(dir, "prestazioni.txt"), t.ToString()); } catch { }
            try { File.WriteAllText(Path.Combine(dir, "prestazioni_risultati.txt"), r.ToString()); } catch { }
        }
    }
}
