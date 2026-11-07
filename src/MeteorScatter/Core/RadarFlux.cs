// YA-MS - prelievo e lettura del flusso meteorico dai radar
// Collm (Univ. Lipsia / IAP Kuehlungsborn), grafico standard SKiYMET 720x720:
//   pannello flusso: x 101..676 = 48 ore (ieri + oggi, UTC), y 216 = 0, y 72 = 1.2 x 10^3 meteore/ora
//   pannello quote : y 633 = 70 km, y 374 = 110 km; x 532 = 0, x 676 = 2.0 x 10^3 meteore
// Calibrazione fatta sul grafico del 5/11/2026. Se le etichette delle scale cambiano, il programma lo segnala.
using System;
using System.Collections.Generic;
using SkiaSharp;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace YAMS
{
    public class RadarSource
    {
        public string Name, Type, Url, Note;
        public bool Active;
        public double FreqMHz;
        public string LocalFile;
        public DateTime LastDownloadUtc = DateTime.MinValue;
        public DateTime LastModifiedUtc = DateTime.MinValue;
        public string Status = Lang.T("mai scaricato");
        public bool Offline;
    }

    public class CollmData
    {
        public DateTime DayToday;                 // data UTC del pannello "Today"
        public double[] Valid = new double[48];   // -1 = dato mancante
        public double[] Rejected = new double[48];
        public double[] HeightCounts = new double[40];  // 70..109 km, bin di 1 km
        public double HeightMode, HeightMean, HeightMedian, HeightSd, HeightTotal;
        public bool ScaleOk = true, HeightScaleOk = true;
        public double FluxTop = 1200;             // meteore/ora alla tacca piu' alta della scala (letta dall'immagine)
        public string ScaleNote = "";             // come e' stata ricavata la scala
        public string Problem = "";
    }

    public static class RadarFlux
    {
        public const ulong FluxScaleSignature = 0x18da2b3dfded3c74UL;
        public const ulong HeightScaleSignature = 0x666cf5a33658b1ccUL;
        public static string UserAgent = "YA-MS/1.0 (programma per radioamatori)";

        public static List<RadarSource> LoadSources(string dataDir)
        {
            List<RadarSource> list = new List<RadarSource>();
            string f = Path.Combine(dataDir, "fonti_radar.csv");
            if (!File.Exists(f)) return list;
            foreach (string raw in File.ReadAllLines(f))
            {
                string l = raw.Trim();
                if (l.Length == 0 || l.StartsWith("#") || l.StartsWith("nome;")) continue;
                string[] c = l.Split(';');
                if (c.Length < 5) continue;
                RadarSource s = new RadarSource();
                s.Name = c[0].Trim(); s.Active = c[1].Trim() == "1"; s.Type = c[2].Trim();
                double fr; double.TryParse(c[3].Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out fr);
                s.FreqMHz = fr; s.Url = c[4].Trim(); s.Note = c.Length > 5 ? c[5].Trim() : "";
                string safe = s.Name.Split(' ')[0].ToLowerInvariant().Replace(".", "_");
                s.LocalFile = Path.Combine(Path.Combine(dataDir, "radar"), safe + ".png");
                list.Add(s);
            }
            return list;
        }

        // Scarica l'immagine della fonte. Restituisce true se riuscito.
        // Scarica la fonte solo se e' cambiata (richiesta condizionale If-Modified-Since).
        // Restituisce 1 = nuovo dato scaricato, 0 = invariato, -1 = errore (s.Offline = errore di rete).
        public static int Download(RadarSource s)
        {
            bool offline; string err;
            DateTime lm = s.LastModifiedUtc;
            int r = Fetch(s.Url, s.LocalFile, ref lm, out offline, out err);
            s.Offline = offline;
            if (r >= 0) { s.LastDownloadUtc = DateTime.UtcNow; s.LastModifiedUtc = lm; }
            if (r == 1) s.Status = Lang.F("nuovo dato alle {0} UTC", DateTime.UtcNow.ToString("HH:mm", CultureInfo.InvariantCulture));
            else if (r == 0) s.Status = Lang.F("invariato (controllato alle {0} UTC)", DateTime.UtcNow.ToString("HH:mm", CultureInfo.InvariantCulture));
            else s.Status = offline ? Lang.F("rete assente: {0}", err) : Lang.F("errore: {0}", err);
            return r;
        }

        public static int Fetch(string url, string file, ref DateTime lastModifiedUtc, out bool offline, out string err)
        {
            offline = false; err = "";
            try
            {
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) { offline = true; err = Lang.T("nessuna rete attiva"); return -1; }
                Directory.CreateDirectory(Path.GetDirectoryName(file));
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.UserAgent = UserAgent;
                req.Timeout = 30000;
                req.ReadWriteTimeout = 30000;
                if (File.Exists(file) && lastModifiedUtc > DateTime.MinValue) req.IfModifiedSince = lastModifiedUtc.ToLocalTime();
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                using (Stream rs = resp.GetResponseStream())
                {
                    string tmp = file + ".tmp";
                    using (FileStream fs = File.Create(tmp)) rs.CopyTo(fs);
                    if (File.Exists(file)) File.Delete(file);
                    File.Move(tmp, file);
                    DateTime lm = resp.LastModified.ToUniversalTime();
                    lastModifiedUtc = lm.Year > 2000 ? lm : DateTime.UtcNow;
                    try { File.SetLastWriteTimeUtc(file, lastModifiedUtc); } catch { }
                }
                return 1;
            }
            catch (WebException wex)
            {
                HttpWebResponse r = wex.Response as HttpWebResponse;
                if (r != null && r.StatusCode == HttpStatusCode.NotModified) { r.Close(); return 0; }
                if (r != null) r.Close();
                offline = wex.Status == WebExceptionStatus.NameResolutionFailure || wex.Status == WebExceptionStatus.ConnectFailure
                       || wex.Status == WebExceptionStatus.Timeout || wex.Status == WebExceptionStatus.ProxyNameResolutionFailure
                       || wex.Status == WebExceptionStatus.SendFailure;
                err = wex.Message;
                return -1;
            }
            catch (Exception ex) { err = ex.Message; return -1; }
        }

        // ---- lettura dei pixel ----
        static int[] ReadPixels(SKBitmap bmp, out int w, out int h)
        {
            w = bmp.Width; h = bmp.Height;
            SKColor[] c = bmp.Pixels;
            int[] px = new int[w * h];
            for (int i = 0; i < px.Length && i < c.Length; i++) px[i] = (c[i].Red << 16) | (c[i].Green << 8) | c[i].Blue;
            return px;
        }

        static bool Is(int[] px, int w, int x, int y, int rgb) { return (px[y * w + x] & 0xFFFFFF) == rgb; }

        const int White = 0xFFFFFF, Cyan = 0x00FFFF, Red = 0xFF0000, Yellow = 0xFFFF00;

        static ulong Signature(int[] px, int w, int x0, int x1, int y0, int y1)
        {
            ulong hsh = 0xcbf29ce484222325UL;
            for (int y = y0; y < y1; y++)
                for (int x = x0; x < x1; x++)
                {
                    ulong b = Is(px, w, x, y, White) ? 1UL : 0UL;
                    hsh ^= b; hsh = unchecked(hsh * 0x100000001b3UL);
                }
            return hsh;
        }

        // ---- lettura delle etichette della scala (carattere a matrice del grafico SKiYMET, 7 x 11 pixel) ----
        // Modelli ricavati dai grafici di Collm del 5/11 e 6/11/2026 (cifre 0 1 2 4 6 7 8 9).
        static readonly string[][] DigitTemplates =
        {
            new[] { "0", "..###../.#...#./#.....#/#.....#/#.....#/#.....#/#.....#/#.....#/#.....#/.#...#./..###.." },
            new[] { "1", "..#../.##../#.#../..#../..#../..#../..#../..#../..#../..#../#####" },
            new[] { "2", ".####../#....##/#.....#/......#/.....##/...##../.##..../#....../#....../#....../#######" },
            new[] { "4", "....#../...##../...##../..#.#../..#.#../.#..#../.#..#../#######/....#../....#../...###." },
            new[] { "6", ".####../#....##/#.....#/#....../#####../#....##/#.....#/#.....#/#.....#/#....##/.####.." },
            new[] { "7", "#######/#.....#/......#/.....#./.....#./....#../....#../...#.../...#.../...#.../...#..." },
            new[] { "8", ".####../#....##/#.....#/#.....#/#....##/.####../#....##/#.....#/#.....#/#....##/.####.." },
            new[] { "9", ".####../#....##/#.....#/#.....#/#.....#/#....##/.####.#/......#/#.....#/#....##/.####.." },
            // varianti alte 12 pixel (il grafico ridimensiona il carattere secondo la posizione)
            new[] { "0", "..###../.#...#./#.....#/#.....#/#.....#/#.....#/#.....#/#.....#/#.....#/.#...#./.#...#./..###.." },
            new[] { "0", "..###../.#...#./.#...#./#.....#/#.....#/#.....#/#.....#/#.....#/#.....#/#.....#/.#...#./..###.." },
            new[] { "6", ".####../#....##/#.....#/#....../#####../#....##/#.....#/#.....#/#.....#/#.....#/#....##/.####.." },
            new[] { "8", "..###../.#...#./#.....#/#.....#/#.....#/#....##/.####../#....##/#.....#/#.....#/#....##/.####.." },
        };

        // tacche lunghe dell'asse del flusso (7 pixel bianchi a x 93..99), dall'alto in basso
        static List<int> MajorTicks(int[] px, int w)
        {
            List<int> t = new List<int>();
            for (int y = 60; y <= 230; y++)
            {
                bool all = true;
                for (int x = 93; x <= 99; x++) if (!Is(px, w, x, y, White)) { all = false; break; }
                if (all) t.Add(y);
            }
            return t;
        }

        // legge l'etichetta accanto alla tacca ymid; null se una cifra non e' riconosciuta
        static string ReadLabel(int[] px, int w, int x0, int x1, int ymid)
        {
            int top = -1, bot = -1;
            for (int y = ymid - 10; y <= ymid + 10; y++)
                for (int x = x0; x < x1; x++)
                    if (Is(px, w, x, y, White)) { if (top < 0) top = y; bot = y; break; }
            if (top < 0) return null;
            string res = "";
            int gx = -1;
            for (int x = x0; x <= x1; x++)
            {
                bool col = false;
                if (x < x1) for (int y = top; y <= bot; y++) if (Is(px, w, x, y, White)) { col = true; break; }
                if (col && gx < 0) gx = x;
                if (!col && gx >= 0)
                {
                    char c = MatchGlyph(px, w, gx, x - 1, top, bot);
                    if (c == '?') return null;
                    res += c;
                    gx = -1;
                }
            }
            return res.Length > 0 ? res : null;
        }

        static char MatchGlyph(int[] px, int w, int gx0, int gx1, int y0, int y1)
        {
            // riquadro stretto del carattere
            int t = -1, b = -1;
            for (int y = y0; y <= y1; y++)
                for (int x = gx0; x <= gx1; x++)
                    if (Is(px, w, x, y, White)) { if (t < 0) t = y; b = y; break; }
            int gw = gx1 - gx0 + 1, gh = b - t + 1;
            if (gw <= 2 && gh <= 2) return '.';
            char best = '?'; int bestD = int.MaxValue;
            foreach (string[] tp in DigitTemplates)
            {
                string[] rows = tp[1].Split('/');
                int tw = rows[0].Length, th = rows.Length;
                if (Math.Abs(tw - gw) > 1 || Math.Abs(th - gh) > 1) continue;
                int d = 0, W = Math.Max(tw, gw), H = Math.Max(th, gh);
                for (int y = 0; y < H; y++)
                    for (int x = 0; x < W; x++)
                    {
                        bool a = y < th && x < tw && rows[y][x] == '#';
                        bool g = y < gh && x < gw && Is(px, w, gx0 + x, t + y, White);
                        if (a != g) d++;
                    }
                if (d < bestD) { bestD = d; best = tp[0][0]; }
            }
            return bestD <= 8 ? best : '?';
        }

        public static CollmData ParseCollm(string file, DateTime imageUtc)
        {
            CollmData d = new CollmData();
            d.DayToday = imageUtc.Date;
            using (SKBitmap bmp = SKBitmap.Decode(file))
            {
                int w, h;
                int[] px = ReadPixels(bmp, out w, out h);
                if (w != 720 || h != 720) { d.Problem = Lang.F("Formato del grafico cambiato ({0}x{1}): lettura non possibile.", w, h); d.ScaleOk = false; return d; }
                if (!Is(px, w, 101, 150, White) || !Is(px, w, 300, 216, White)) d.Problem += Lang.T("Assi del flusso non trovati nella posizione attesa.") + " ";
                d.HeightScaleOk = Signature(px, w, 515, 705, 640, 672) == HeightScaleSignature;
                if (!d.HeightScaleOk) d.Problem += Lang.T("La scala del grafico delle quote e' cambiata: le statistiche delle quote (moda, media, mediana) restano valide, i conteggi assoluti no.") + " ";

                // Scala del flusso: il radar la adatta ai valori del giorno (es. 0-1,0 o 0-1,2 x 10^3).
                // Si cercano le tacche lunghe dell'asse e si legge l'etichetta della tacca piu' alta.
                const int X0 = 101, X1 = 676;
                int Y0 = 216, Ytop = 72;
                double top;
                List<int> ticks = MajorTicks(px, w);
                string lab = ticks.Count >= 2 ? ReadLabel(px, w, 50, 92, ticks[0]) : null;
                string lab0 = ticks.Count >= 2 ? ReadLabel(px, w, 50, 92, ticks[ticks.Count - 1]) : null;
                if (lab != null && lab0 == "0.0" && double.TryParse(lab, NumberStyles.Float, CultureInfo.InvariantCulture, out top) && top > 0)
                {
                    Ytop = ticks[0]; Y0 = ticks[ticks.Count - 1];
                    d.FluxTop = top * 1000; d.ScaleOk = true;
                    d.ScaleNote = Lang.F("scala letta dal grafico: 0-{0} meteore/ora", d.FluxTop.ToString("F0", CultureInfo.InvariantCulture));
                }
                else
                {
                    d.ScaleOk = Signature(px, w, 40, 96, 60, 226) == FluxScaleSignature;
                    d.FluxTop = 1200;
                    d.ScaleNote = d.ScaleOk ? Lang.T("scala tarata 0-1200 meteore/ora") : Lang.T("scala non letta");
                    if (!d.ScaleOk) d.Problem += Lang.F("Non riesco a leggere la scala verticale del flusso (etichetta in cima: {0}): valori da verificare.", lab ?? "?") + " ";
                }
                for (int hr = 0; hr < 48; hr++)
                {
                    int xc = (int)Math.Round(X0 + (hr + 0.5) * (X1 - X0) / 48.0);
                    int yc = -1, yr = -1;
                    for (int y = 60; y <= Y0; y++)
                    {
                        if (yc < 0 && Is(px, w, xc, y, Cyan)) yc = y;
                        if (yr < 0 && Is(px, w, xc, y, Red)) yr = y;
                    }
                    d.Valid[hr] = yc < 0 ? -1 : Math.Max(0, (Y0 - yc) * d.FluxTop / (Y0 - Ytop));
                    d.Rejected[hr] = yr < 0 ? -1 : Math.Max(0, (Y0 - yr) * d.FluxTop / (Y0 - Ytop));
                }
                // istogramma delle quote
                double tot = 0, sum = 0; int best = 0;
                for (int k = 0; k < 40; k++)
                {
                    int y = (int)Math.Round(633 - (k + 0.5) * (633 - 374) / 40.0);
                    int n = 0;
                    for (int x = 530; x < 720; x++) if (Is(px, w, x, y, Yellow)) n++;
                    d.HeightCounts[k] = n * 2000.0 / (676 - 532);
                    tot += d.HeightCounts[k]; sum += d.HeightCounts[k] * (70.5 + k);
                    if (d.HeightCounts[k] > d.HeightCounts[best]) best = k;
                }
                d.HeightTotal = tot;
                if (tot > 0)
                {
                    d.HeightMean = sum / tot; d.HeightMode = 70.5 + best;
                    double cum = 0, var = 0;
                    d.HeightMedian = 0;
                    for (int k = 0; k < 40; k++)
                    {
                        cum += d.HeightCounts[k];
                        if (d.HeightMedian == 0 && cum >= tot / 2) d.HeightMedian = 70.5 + k;
                        var += d.HeightCounts[k] * Math.Pow(70.5 + k - d.HeightMean, 2);
                    }
                    d.HeightSd = Math.Sqrt(var / tot);
                }
            }
            return d;
        }

        // Archivio orario: data\flusso_collm.csv (aggiorna le ore gia' presenti)
        public static void Archive(string dataDir, CollmData d, double freqMHz)
        {
            string f = Path.Combine(dataDir, "flusso_collm.csv");
            SortedDictionary<string, string> rows = new SortedDictionary<string, string>();
            if (File.Exists(f))
                foreach (string l in File.ReadAllLines(f))
                {
                    if (l.StartsWith("#") || l.StartsWith("ora_utc")) continue;
                    int i = l.IndexOf(';'); if (i > 0) rows[l.Substring(0, i)] = l;
                }
            DateTime start = d.DayToday.AddDays(-1);
            string now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            for (int hr = 0; hr < 48; hr++)
            {
                if (d.Valid[hr] < 0) continue;
                DateTime t = start.AddHours(hr);
                if (t >= DateTime.UtcNow.AddHours(-1)) continue;   // ora non ancora conclusa
                string key = t.ToString("yyyy-MM-dd HH:00", CultureInfo.InvariantCulture);
                rows[key] = key + ";" + d.Valid[hr].ToString("F0", CultureInfo.InvariantCulture) + ";"
                          + (d.Rejected[hr] < 0 ? "" : d.Rejected[hr].ToString("F0", CultureInfo.InvariantCulture)) + ";"
                          + freqMHz.ToString("F2", CultureInfo.InvariantCulture) + ";" + now + ";" + (d.ScaleOk ? "ok" : "scala?");
            }
            List<string> outl = new List<string>();
            outl.Add("# Flusso orario radar Collm letto dal grafico (meteore/ora, precisione circa 8/ora)");
            outl.Add("ora_utc;validi;scartati;freq_MHz;letto_il;controllo_scala");
            outl.AddRange(rows.Values);
            File.WriteAllLines(f, outl.ToArray());

            string fq = Path.Combine(dataDir, "quote_collm.csv");
            bool exists = File.Exists(fq);
            using (StreamWriter sw = new StreamWriter(fq, true))
            {
                if (!exists) sw.WriteLine("letto_il_utc;moda_km;media_km;mediana_km;dev_std_km;meteore_totali;controllo_scala");
                sw.WriteLine(now + ";" + d.HeightMode.ToString("F1", CultureInfo.InvariantCulture) + ";" + d.HeightMean.ToString("F1", CultureInfo.InvariantCulture)
                    + ";" + d.HeightMedian.ToString("F1", CultureInfo.InvariantCulture) + ";" + d.HeightSd.ToString("F1", CultureInfo.InvariantCulture)
                    + ";" + d.HeightTotal.ToString("F0", CultureInfo.InvariantCulture) + ";" + (d.HeightScaleOk ? "ok" : "scala?"));
            }
        }
    }
}
