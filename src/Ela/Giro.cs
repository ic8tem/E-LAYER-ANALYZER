// E-LAYER ANALYZER - ionosonde GIRO (Global Ionosphere Radio Observatory, UMass Lowell): foEs, foF2 e MUF(D) in diretta.
// Servizio pubblico lgdc.uml.edu/fastchar/getbest (lo stesso di MUF Muncher di DM6AX, licenza MIT): una riga per ionogramma,
// "ora CS valore QD valore QD valore QD" nell'ordine delle caratteristiche chieste (qui foF2, MUF(D), foEs); "---" = mancante.
// Dati GIRO: licenza CC BY-NC-SA 4.0 (solo uso non commerciale), con la citazione di Reinisch e Galkin 2011 (fonte [68]).
// Elenco delle stazioni: ULCAR, "Digisonde station list" (ulcar.uml.edu/stationlist.html), stazioni non dismesse; Gibilmanna
// e Sopron dall'elenco controllato di MUF Muncher. Le stazioni che non rispondono restano in grigio.
// Ogni stazione ha una cronologia di 48 ore in data/giro/<codice>.csv: si chiedono solo i dati nuovi.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ELA
{
    public class GiroRec { public DateTime Utc; public double Cs = double.NaN, FoF2 = double.NaN, Muf = double.NaN, FoEs = double.NaN; }

    public class GiroStation
    {
        public string Code, Name;
        public double Lat, Lon;
        public readonly List<GiroRec> Recs = new List<GiroRec>();   // in ordine di tempo, ultime 48 ore
        public string Error = "";
        public DateTime LastAsk = DateTime.MinValue;
        // ultimo valore disponibile di una caratteristica (NaN se manca nelle ultime 'maxAgeMin' minuti)
        public GiroRec Last
        {
            get { lock (Recs) return Recs.Count > 0 ? Recs[Recs.Count - 1] : null; }
        }
        public double LastValue(Func<GiroRec, double> f, double maxAgeMin, out DateTime when)
        {
            when = DateTime.MinValue;
            DateTime lim = DateTime.UtcNow.AddMinutes(-maxAgeMin);
            lock (Recs)
                for (int i = Recs.Count - 1; i >= 0; i--)
                {
                    if (Recs[i].Utc < lim) break;
                    double v = f(Recs[i]);
                    if (!double.IsNaN(v)) { when = Recs[i].Utc; return v; }
                }
            return double.NaN;
        }
        public bool Fresh { get { GiroRec r = Last; return r != null && (DateTime.UtcNow - r.Utc).TotalMinutes <= Giro.FreshMin; } }
    }

    public static class Giro
    {
        public const string Url = "https://lgdc.uml.edu/fastchar/getbest";
        public const double FreshMin = 90;         // oltre 90 minuti dall'ultimo ionogramma la stazione e' "vecchia" (grigia)
        public const double HistoryHours = 48;
        public static readonly List<GiroStation> Stations = new List<GiroStation>();
        public static DateTime LastFetch = DateTime.MinValue;
        public static bool Busy;
        public static string Status = "";
        public static event Action Updated;
        public static bool Ready { get { return dir != null; } }   // archivio caricato (Init fatto)
        static string dir;
        static readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };

        // codice URSI, nome, latitudine, longitudine (est positiva)
        static readonly object[][] List0 =
        {
            new object[] { "THJ77", "Qaanaaq", 77.5, -69.4 }, new object[] { "TR169", "Tromsø", 69.6, 19.2 }, new object[] { "NO369", "Norilsk", 69.4, 88.1 },
            new object[] { "SMJ67", "Sondrestrom", 67.0, -50.7 }, new object[] { "ZH466", "Zhigansk", 66.8, 123.4 }, new object[] { "CO764", "College (Alaska)", 64.9, -147.8 },
            new object[] { "GA762", "Gakona (Alaska)", 62.24, -145.09 }, new object[] { "YA462", "Yakutsk", 62.0, 129.6 }, new object[] { "NQJ61", "Narsarsuaq", 61.2, -45.4 },
            new object[] { "LE061", "Lerwick", 60.1, -1.2 }, new object[] { "KS759", "King Salmon (Alaska)", 58.7, -156.6 }, new object[] { "JR055", "Juliusruh", 54.6, 13.4 },
            new object[] { "MH453", "Mohe", 53.5, 112.3 }, new object[] { "IR352", "Irkutsk", 52.4, 104.3 }, new object[] { "FF051", "Fairford", 51.7, -1.5 },
            new object[] { "RL052", "Chilton", 51.5, -0.6 }, new object[] { "DB049", "Dourbes", 50.1, 4.6 }, new object[] { "PQ052", "Průhonice", 50.0, 14.6 },
            new object[] { "SO148", "Sopron", 47.63, 16.72 }, new object[] { "AFJ49", "Argentia", 47.3, -54.0 }, new object[] { "MHJ45", "Millstone Hill", 42.6, -71.5 },
            new object[] { "RO041", "Roma", 41.9, 12.5 }, new object[] { "EB040", "Roquetes (Ebro)", 40.8, 0.5 }, new object[] { "VT139", "San Vito dei Normanni", 40.6, 17.8 },
            new object[] { "BP440", "Beijing", 40.3, 116.2 }, new object[] { "BC840", "Boulder", 40.0, -105.3 }, new object[] { "AT138", "Atene", 38.0, 23.5 },
            new object[] { "GM037", "Gibilmanna", 37.9, 14.0 }, new object[] { "WP937", "Wallops Island", 37.9, -75.5 }, new object[] { "AN438", "Anyang", 37.39, 126.95 },
            new object[] { "EA036", "El Arenosillo", 37.1, -6.7 }, new object[] { "SN437", "Osan", 37.1, 127.0 }, new object[] { "TO535", "Kokubunji", 35.7, 139.5 },
            new object[] { "PA836", "Point Arguello", 34.7, -120.6 }, new object[] { "IS233", "Islamabad", 33.8, 72.9 }, new object[] { "DS932", "Dyess", 32.5, -99.7 },
            new object[] { "BJJ32", "Bermuda", 32.4, -64.7 }, new object[] { "EG931", "Eglin", 30.4, -86.8 }, new object[] { "OK426", "Okinawa", 26.3, 127.8 },
            new object[] { "CL424", "Chung-Li", 25.0, 121.2 }, new object[] { "KA225", "Karachi", 25.0, 67.1 }, new object[] { "HA419", "Hainan", 19.4, 109.0 },
            new object[] { "PRJ18", "Ramey (Porto Rico)", 18.5, -67.1 }, new object[] { "SA418", "Sanya", 18.3, 109.6 }, new object[] { "KJ609", "Kwajalein", 9.4, 167.4 },
            new object[] { "BVJ03", "Boa Vista", 2.8, -60.7 }, new object[] { "SAA0K", "São Luís", -2.5, -44.2 }, new object[] { "FZA0M", "Fortaleza", -3.8, -38.0 },
            new object[] { "AS00Q", "Ascensione", -8.0, -14.0 }, new object[] { "JI91J", "Jicamarca", -12.0, -76.8 }, new object[] { "DW41K", "Darwin", -12.5, 130.9 },
            new object[] { "CGK21", "Campo Grande", -20.5, -54.7 }, new object[] { "LM42B", "Learmonth", -21.8, 114.0 }, new object[] { "MU12K", "Madimbo", -22.38, 30.88 },
            new object[] { "CAJ2M", "Cachoeira Paulista", -22.7, -45.0 }, new object[] { "TUJ20", "Tucumán", -26.9, -65.4 }, new object[] { "LV12P", "Louisvale", -28.51, 21.24 },
            new object[] { "GR13L", "Grahamstown", -33.3, 26.5 }, new object[] { "BV53Q", "Bundoora", -37.72, 145.05 }, new object[] { "PMJ4K", "Puerto Madryn", -42.7, -65.3 },
            new object[] { "PSJ5J", "Port Stanley", -51.6, -57.9 }, new object[] { "ZS36R", "Zhongshan (Antartide)", -69.4, 76.4 }
        };

        static Giro()
        {
            try { http.DefaultRequestHeaders.UserAgent.ParseAdd("E-LAYER-ANALYZER (programma gratuito per radioamatori, github.com/ic8tem/E-LAYER-ANALYZER)"); } catch { }
            foreach (object[] o in List0)
                Stations.Add(new GiroStation { Code = (string)o[0], Name = (string)o[1], Lat = Convert.ToDouble(o[2], CultureInfo.InvariantCulture), Lon = Convert.ToDouble(o[3], CultureInfo.InvariantCulture) });
        }

        public static GiroStation Find(string code) { foreach (GiroStation s in Stations) if (s.Code == code) return s; return null; }

        // ---------------------------------------------------------------- archivio su disco

        public static void Init(string dataDir)
        {
            dir = Path.Combine(dataDir, "giro");
            try { Directory.CreateDirectory(dir); } catch { }
            foreach (GiroStation s in Stations) LoadCache(s);
        }

        static void LoadCache(GiroStation s)
        {
            try
            {
                string f = Path.Combine(dir, s.Code + ".csv");
                if (!File.Exists(f)) return;
                DateTime lim = DateTime.UtcNow.AddHours(-HistoryHours);
                List<GiroRec> l = new List<GiroRec>();
                foreach (string line in File.ReadAllLines(f))
                {
                    string[] p = line.Split(';');
                    if (p.Length < 5) continue;
                    DateTime t;
                    if (!DateTime.TryParse(p[0], CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out t) || t < lim) continue;
                    l.Add(new GiroRec { Utc = DateTime.SpecifyKind(t, DateTimeKind.Utc), Cs = D(p[1]), FoF2 = D(p[2]), Muf = D(p[3]), FoEs = D(p[4]) });
                }
                lock (s.Recs) { s.Recs.Clear(); s.Recs.AddRange(l); }
            }
            catch { }
        }

        static void SaveCache(GiroStation s)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                lock (s.Recs)
                    foreach (GiroRec r in s.Recs)
                        sb.Append(r.Utc.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)).Append(';').Append(S(r.Cs)).Append(';')
                          .Append(S(r.FoF2)).Append(';').Append(S(r.Muf)).Append(';').Append(S(r.FoEs)).Append('\n');
                File.WriteAllText(Path.Combine(dir, s.Code + ".csv"), sb.ToString());
            }
            catch { }
        }

        static double D(string s) { double v; return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v) ? v : double.NaN; }
        static string S(double v) { return double.IsNaN(v) ? "" : v.ToString("0.###", CultureInfo.InvariantCulture); }

        // ---------------------------------------------------------------- scaricamento (thread secondario, silenzioso)

        // tutte le stazioni, quattro richieste alla volta; per ognuna solo i dati dopo l'ultimo ionogramma in archivio
        public static async Task FetchAllAsync()
        {
            if (Busy) return;
            Busy = true;
            int ok = 0, fail = 0;
            try
            {
                using (SemaphoreSlim sem = new SemaphoreSlim(4))
                {
                    List<Task> tasks = new List<Task>();
                    foreach (GiroStation s in Stations)
                    {
                        await sem.WaitAsync();
                        GiroStation st = s;
                        tasks.Add(Task.Run(async () =>
                        {
                            try { if (await FetchAsync(st)) Interlocked.Increment(ref ok); else Interlocked.Increment(ref fail); }
                            finally { sem.Release(); }
                        }));
                    }
                    await Task.WhenAll(tasks);
                }
                LastFetch = DateTime.UtcNow;
                int fresh = 0; foreach (GiroStation s in Stations) if (s.Fresh) fresh++;
                Status = string.Format(CultureInfo.InvariantCulture, "{0:HH:mm} UTC|{1}|{2}|{3}", LastFetch, fresh, Stations.Count, fail);
            }
            finally { Busy = false; }
            Action a = Updated; if (a != null) a();
        }

        static async Task<bool> FetchAsync(GiroStation s)
        {
            DateTime now = DateTime.UtcNow;
            DateTime from = now.AddHours(-24);
            GiroRec last = s.Last;
            if (last != null && last.Utc > from) from = last.Utc.AddSeconds(1);
            string q = "ursiCode=" + Uri.EscapeDataString(s.Code) + "&charName=" + Uri.EscapeDataString("foF2,MUF(D),foEs") +
                       "&fromDate=" + Uri.EscapeDataString(from.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)) +
                       "&toDate=" + Uri.EscapeDataString(now.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));
            s.LastAsk = now;
            string txt;
            try { txt = await http.GetStringAsync(Url + "?" + q); }
            catch (Exception ex) { s.Error = ex.Message; return false; }
            List<GiroRec> add = Parse(txt);
            s.Error = "";
            DateTime lim = now.AddHours(-HistoryHours);
            lock (s.Recs)
            {
                DateTime lastT = s.Recs.Count > 0 ? s.Recs[s.Recs.Count - 1].Utc : DateTime.MinValue;
                foreach (GiroRec r in add) if (r.Utc > lastT) { s.Recs.Add(r); lastT = r.Utc; }
                int old = 0; while (old < s.Recs.Count && s.Recs[old].Utc < lim) old++;
                if (old > 0) s.Recs.RemoveRange(0, old);
            }
            if (add.Count > 0) SaveCache(s);
            return true;
        }

        // righe "2026-11-15T13:45:00.000Z 85 6.250 // 18.40 // 4.10 //": ora, CS, poi valore e QD per ogni caratteristica
        public static List<GiroRec> Parse(string txt)
        {
            List<GiroRec> l = new List<GiroRec>();
            if (string.IsNullOrEmpty(txt)) return l;
            foreach (string line0 in txt.Split('\n'))
            {
                string line = line0.Trim();
                if (line.Length == 0 || line.StartsWith("#")) continue;
                string[] p = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (p.Length < 7) continue;
                DateTime t;
                if (!DateTime.TryParse(p[0], CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out t)) continue;
                l.Add(new GiroRec { Utc = DateTime.SpecifyKind(t, DateTimeKind.Utc), Cs = D(p[1]), FoF2 = D(p[2]), Muf = D(p[4]), FoEs = D(p[6]) });
            }
            l.Sort((a, b) => a.Utc.CompareTo(b.Utc));
            return l;
        }
    }
}
