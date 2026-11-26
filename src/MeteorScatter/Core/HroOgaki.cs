// YA-MS - conteggi HRO in diretta dal Giappone (stazione ricevente di Ogaki, hro-ogaki.org)
// Ogni 10 minuti HROFFT salva un'immagine dello spettro (629x400) con il conteggio automatico degli echi
// scritto nell'intestazione ("meteor NN", cifre gialle, righe 41-49, colonne 128-176). Il programma legge quel
// numero confrontando le cifre con un font fisso (come per le etichette del grafico di Collm) e somma i 6 valori di
// ogni ora. Fari: JA2SDA Toyokawa 53,100 MHz e JE3AKE Otsu 53,372 MHz. Nomi dei file: AAMMGGhhmm.png in ora JST,
// archivio di 30 giorni. Il conteggio automatico conta un po' piu' di quello a mano (echi lunghi, disturbi):
// e' un valore relativo, da tarare con i file mensili RMOJ. Si usa solo entro 3500 km dal Giappone
// (richiesta dell'autore, IC8TEM, 22/11/2026).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SkiaSharp;

namespace YAMS
{
    public class HroStation
    {
        public string Key, Beacon, Place, ImgBase;
        public double FreqMHz, Lat, Lon;
        public Dictionary<DateTime, int> Slots = new Dictionary<DateTime, int>();   // inizio dei 10 minuti (UTC) -> echi; -1 = immagine mancante
        public DateTime LastOkUtc = DateTime.MinValue;
        public string Status = "";
    }

    public static class HroOgaki
    {
        public const double RxLat = 35.35, RxLon = 136.60;     // Ogaki, Gifu
        public const double MaxKmFromJapan = 3500;
        public static readonly HroStation[] Stations =
        {
            new HroStation { Key = "toyokawa", Beacon = "JA2SDA", Place = "Toyokawa", FreqMHz = 53.100, Lat = 34.838, Lon = 137.291, ImgBase = "https://hro-ogaki.org/hro_data_toyokawa/img/" },
            new HroStation { Key = "otsu", Beacon = "JE3AKE", Place = "Otsu", FreqMHz = 53.372, Lat = 34.950, Lon = 135.900, ImgBase = "https://hro-ogaki.org/hro_data_otsu/img/" },
        };

        // cifre del font di HROFFT: righe 41-49 (9 righe), larghezza in pixel dopo la barra
        static readonly Dictionary<string, char> Glyphs = new Dictionary<string, char>
        {
            { "011010011001100110011001100110010110/4", '0' },
            { "011101010101010101/2", '1' },
            { "011010011001000100100100010010001111/4", '2' },
            { "011010011001000101100001100110010110/4", '3' },
            { "000100011000110010100101010010111110001000010/5", '4' },
            { "111110001000111010010001100110010110/4", '5' },
            { "011010011001100011101001100110010110/4", '6' },
            { "111100010001001000100010010001000100/4", '7' },
            { "011010011001100101101001100110010110/4", '8' },
            { "011010011001100101110001100110010110/4", '9' },
        };

        // il QTH e' entro 3500 km dal Giappone (da uno qualsiasi dei fari o dal ricevitore)?
        public static bool NearJapan(LatLon qth)
        {
            if (Astro.DistanceKm(qth, new LatLon(RxLat, RxLon)) <= MaxKmFromJapan) return true;
            foreach (HroStation s in Stations) if (Astro.DistanceKm(qth, new LatLon(s.Lat, s.Lon)) <= MaxKmFromJapan) return true;
            return false;
        }

        static bool Yellow(SKColor c) { return c.Red > 150 && c.Green > 150 && c.Blue < 100; }

        // legge il numero "meteor" dall'immagine; -1 se non riconosciuto
        public static int ReadCount(string png)
        {
            using (SKBitmap bmp = SKBitmap.Decode(png))
            {
                if (bmp == null || bmp.Width < 200 || bmp.Height < 60) return -1;
                const int x0 = 128, x1 = 176, y0 = 41, y1 = 50;
                string digits = "";
                List<int> cur = new List<int>();
                for (int x = x0; x <= x1; x++)
                {
                    bool on = false;
                    if (x < x1) for (int y = y0; y < y1; y++) if (Yellow(bmp.GetPixel(x, y))) { on = true; break; }
                    if (on) { cur.Add(x); continue; }
                    if (cur.Count == 0) continue;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int y = y0; y < y1; y++) foreach (int cx in cur) sb.Append(Yellow(bmp.GetPixel(cx, y)) ? '1' : '0');
                    sb.Append('/').Append(cur.Count);
                    char d;
                    if (!Glyphs.TryGetValue(sb.ToString(), out d)) return -1;
                    digits += d; cur.Clear();
                }
                int n;
                return digits.Length > 0 && int.TryParse(digits, out n) ? n : -1;
            }
        }

        static string CacheFile(string dataDir, HroStation s) { return Path.Combine(dataDir, "radar", "hro_ogaki_" + s.Key + ".csv"); }

        public static void Load(string dataDir)
        {
            foreach (HroStation s in Stations)
            {
                s.Slots.Clear();
                string f = CacheFile(dataDir, s);
                if (!File.Exists(f)) continue;
                foreach (string ln in File.ReadAllLines(f))
                {
                    string[] p = ln.Split(';');
                    DateTime t; int n;
                    if (p.Length == 2 && DateTime.TryParseExact(p[0], "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out t)
                        && int.TryParse(p[1], out n)) s.Slots[DateTime.SpecifyKind(t, DateTimeKind.Utc)] = n;
                }
            }
        }

        static void Save(string dataDir, HroStation s)
        {
            DateTime lim = DateTime.UtcNow.AddDays(-3);
            List<string> lines = new List<string>();
            List<DateTime> keys = new List<DateTime>(s.Slots.Keys); keys.Sort();
            foreach (DateTime t in keys) if (t >= lim) lines.Add(t.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture) + ";" + s.Slots[t]);
            Directory.CreateDirectory(Path.GetDirectoryName(CacheFile(dataDir, s)));
            File.WriteAllLines(CacheFile(dataDir, s), lines);
        }

        // scarica le immagini mancanti delle ultime 48 ore (dalla piu' recente, al massimo maxDl per stazione)
        public static void Update(string dataDir, int maxDl)
        {
            DateTime now = DateTime.UtcNow;
            DateTime last = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute / 10 * 10, 0, DateTimeKind.Utc).AddMinutes(-10);
            string tmp = Path.Combine(dataDir, "radar", "hro_ogaki.tmp.png");
            foreach (HroStation s in Stations)
            {
                int dl = 0, ok = 0; string err = "";
                for (DateTime t = last; t > now.AddHours(-48) && dl < maxDl; t = t.AddMinutes(-10))
                {
                    if (s.Slots.ContainsKey(t)) continue;
                    DateTime jst = t.AddHours(9);
                    string name = jst.ToString("yyMMddHHmm", CultureInfo.InvariantCulture) + ".png";
                    DateTime lm = DateTime.MinValue; bool off;
                    try { if (File.Exists(tmp)) File.Delete(tmp); } catch { }
                    int r = RadarFlux.Fetch(s.ImgBase + name, tmp, ref lm, out off, out err);
                    dl++;
                    if (off) break;
                    if (r == 1)
                    {
                        int n = -1;
                        try { n = ReadCount(tmp); } catch { }
                        s.Slots[t] = n;
                        if (n >= 0) { ok++; if (t > s.LastOkUtc) s.LastOkUtc = t; }
                    }
                    else if (t < now.AddMinutes(-40)) s.Slots[t] = -1;   // immagine che non arrivera' piu'
                }
                foreach (KeyValuePair<DateTime, int> kv in s.Slots) if (kv.Value >= 0 && kv.Key > s.LastOkUtc) s.LastOkUtc = kv.Key;
                s.Status = Lang.F("{0} immagini lette ora, ultima delle {1} UTC", ok,
                    s.LastOkUtc > DateTime.MinValue ? s.LastOkUtc.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture) : "-") + (err.Length > 0 && ok == 0 ? " - " + err : "");
                try { Save(dataDir, s); } catch { }
            }
            try { if (File.Exists(tmp)) File.Delete(tmp); } catch { }
        }

        // conteggi orari di ieri e oggi (UTC) nel formato dei dati di Collm: somma dei 6 valori dell'ora,
        // riportata a 60 minuti se ne mancano al massimo 2; -1 altrimenti
        public static CollmData AsHourly(HroStation s)
        {
            DateTime now = DateTime.UtcNow;
            CollmData d = new CollmData { DayToday = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc) };
            DateTime start = d.DayToday.AddDays(-1);
            for (int i = 0; i < 48; i++)
            {
                double sum = 0; int n = 0;
                for (int k = 0; k < 6; k++)
                {
                    int c;
                    if (s.Slots.TryGetValue(start.AddHours(i).AddMinutes(10 * k), out c) && c >= 0) { sum += c; n++; }
                }
                d.Valid[i] = n >= 4 ? sum * 6.0 / n : -1;
                d.Rejected[i] = -1;
            }
            d.ScaleNote = Lang.F("conteggio automatico di HROFFT a Ogaki, faro {0} {1}", s.Beacon, s.Place);
            return d;
        }
    }
}
