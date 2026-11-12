// E-LAYER ANALYZER - dati di propagazione per la linguetta DATA COLLECTION (sottolinguetta "Dati di propagazione")
// e sintesi in sovraimpressione sulle mappe di ogni modo di propagazione.
//  - N0NBH (hamqsl.com): dati solari e condizioni VHF in XML (E-skip Europa / Nord America e aurora da DXrobot) e il banner
//    VHF "Solar101 VHF"; l'autore chiede di non scaricarli piu' di una volta l'ora.
//  - Make More Miles on VHF (mmmonvhf.de): grafico dell'attivita' meteorica del giorno (dati RMOB), aggiornato ogni ora.
//  - Bz e vento solare: NOAA SWPC, Real-Time Solar Wind (DSCOVR/ACE), gia' usati dalla linguetta AURORA (SpaceWeather).
// Le letture si fanno sui dati XML (non sull'immagine), che contengono gli stessi valori del banner.
// I file si salvano in data/propagazione/ e si riusano senza rete.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using YAMS;

namespace ELA
{
    public static class Continents
    {
        // nomi (chiavi italiane), viste delle mappe (lonW, lonE, latS, latN)
        // per l'estrazione delle chiavi: Lang.T("Europa") Lang.T("Nord America") Lang.T("Sud America") Lang.T("Africa") Lang.T("Asia") Lang.T("Oceania")
        public static readonly string[] Names = { "Europa", "Nord America", "Sud America", "Africa", "Asia", "Oceania" };
        static readonly double[][] views =
        {
            new double[] { -30, 60, 20, 72 }, new double[] { -170, -50, 8, 75 }, new double[] { -95, -25, -58, 15 },
            new double[] { -25, 60, -38, 40 }, new double[] { 25, 150, -10, 75 }, new double[] { 100, 185, -50, 5 }
        };
        public static int Index;
        public static event Action Changed;
        public static double[] View { get { return views[Math.Max(0, Math.Min(views.Length - 1, Index))]; } }
        public static void Set(int i)
        {
            Index = Math.Max(0, Math.Min(Names.Length - 1, i));
            if (Station.Cfg != null) Station.Cfg.Set("continente", Index);
            Action a = Changed; if (a != null) a();
        }
        public static void Load() { Index = Station.Cfg != null ? (int)Station.Cfg.GetD("continente", 0) : 0; }
        public static void Apply(MapView m) { double[] v = View; m.SetView(v[0], v[1], v[2], v[3]); }
    }

    public static class PropData
    {
        public const string XmlUrl = "https://www.hamqsl.com/solarxml.php";
        public const string VhfImageUrl = "https://www.hamqsl.com/solar101vhf.php";
        public const string MsImageUrl = "https://mmmonvhf.de/ms/ms.png";
        public const int MinMinutes = 60;   // N0NBH: non piu' di una volta l'ora

        public static readonly Dictionary<string, string> Solar = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> Vhf = new Dictionary<string, string>();   // "nome|luogo" -> valore
        public static DateTime LastFetch = DateTime.MinValue;
        public static string LastError = "";
        public static event Action Updated;
        static bool fetching;
        static readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(40) };

        static PropData()
        {
            try { http.DefaultRequestHeaders.UserAgent.ParseAdd("E-LAYER-ANALYZER (programma per radioamatori)"); } catch { }
        }

        public static string Dir(string dataDir) { string d = Path.Combine(dataDir, "propagazione"); Directory.CreateDirectory(d); return d; }
        public static string VhfImageFile(string dataDir) { return Path.Combine(Dir(dataDir), "n0nbh_vhf.gif"); }
        public static string MsImageFile(string dataDir) { return Path.Combine(Dir(dataDir), "mmmonvhf_ms.png"); }

        public static async Task FetchAsync(string dataDir, bool force)
        {
            if (fetching) return;
            if (!force && (DateTime.UtcNow - LastFetch).TotalMinutes < MinMinutes) return;
            fetching = true;
            LastError = "";
            try
            {
                string d = Dir(dataDir);
                string[] urls = { XmlUrl, VhfImageUrl, MsImageUrl };
                string[] files = { Path.Combine(d, "n0nbh.xml"), VhfImageFile(dataDir), MsImageFile(dataDir) };
                for (int i = 0; i < urls.Length; i++)
                {
                    try { byte[] b = await http.GetByteArrayAsync(urls[i]); File.WriteAllBytes(files[i], b); }
                    catch (Exception ex) { LastError += Path.GetFileName(files[i]) + ": " + ex.Message + " "; }
                }
                LastFetch = DateTime.UtcNow;
                await FetchAuroraAsync(dataDir);
                LoadCached(dataDir);
            }
            finally { fetching = false; }
        }

        public static void LoadCached(string dataDir)
        {
            string f = Path.Combine(Dir(dataDir), "n0nbh.xml");
            try
            {
                if (!File.Exists(f)) return;
                XDocument x = XDocument.Load(f);
                XElement sd = x.Descendants("solardata").FirstOrDefault();
                if (sd == null) return;
                lock (Solar)
                {
                    Solar.Clear(); Vhf.Clear();
                    foreach (XElement e in sd.Elements())
                        if (!e.HasElements) Solar[e.Name.LocalName] = e.Value.Trim();
                    foreach (XElement p in sd.Descendants("phenomenon"))
                        Vhf[(string)p.Attribute("name") + "|" + (string)p.Attribute("location")] = p.Value.Trim();
                }
            }
            catch (Exception ex) { LastError += "xml: " + ex.Message + " "; }
            Action a = Updated; if (a != null) a();
        }

        public static string S(string k) { string v; lock (Solar) return Solar.TryGetValue(k, out v) && v.Length > 0 ? v : "-"; }
        static string V(string name, string loc) { string v; lock (Solar) return Vhf.TryGetValue(name + "|" + loc, out v) && v.Length > 0 ? v : null; }

        // E-skip del continente scelto (N0NBH copre Europa e Nord America)
        public static string EsSummary()
        {
            if (Solar.Count == 0) return null;
            if (Continents.Index == 0)
            {
                string eu = V("E-Skip", "europe"), e6 = V("E-Skip", "europe_6m"), e4 = V("E-Skip", "europe_4m");
                return Lang.F("E-skip Europa: {0}   6 m: {1}   4 m: {2}", Tr(eu), Tr(e6), Tr(e4));
            }
            if (Continents.Index == 1) return Lang.F("E-skip Nord America: {0}", Tr(V("E-Skip", "north_america")));
            return Lang.T("E-skip: N0NBH pubblica solo Europa e Nord America");
        }

        public static string AuroraSummary()
        {
            if (Solar.Count == 0) return null;
            string vhfAu = V("vhf-aurora", "northern_hemi");
            return Lang.F("Aurora VHF: {0}   aurora {1} (lat {2}°)", Tr(vhfAu), S("aurora"), S("latdegree"));
        }

        // valori di N0NBH tradotti: Lang.T("Band Closed") Lang.T("50MHz ES") Lang.T("70MHz ES") Lang.T("144MHz ES") Lang.T("High MUF")
        // Lang.T("High LAT AUR") Lang.T("Mid LAT AUR")
        // ---------------------------------------------------------------- dati per l'aurora (fonti approvate dall'autore)
        // Kyoto WDC: indice Dst orario del mese in corso (testo, solo uso non commerciale) [46]
        public const string DstUrl = "https://wdc.kugi.kyoto-u.ac.jp/dst_realtime/presentmonth/index.html";
        // Kyoto WDC: indice AE in tempo reale (pagina; i file digitali sono fermi da ottobre 2026) [45]
        public const string AeUrl = "https://wdc.kugi.kyoto-u.ac.jp/ae_realtime/today/today.html";
        // SuperDARN Canada: convezione in tempo reale e bordo dell'ovale (solo collegamento) [47]
        public const string SuperDarnUrl = "https://superdarn.ca/realtime-convection";
        // Bureau of Meteorology (Australia): API con chiave gratuita, POST JSON [44]
        public const string BomApi = "https://sws-data.sws.bom.gov.au/api/v1/";
        public static double Dst = double.NaN; public static DateTime DstTime;
        public static string BomKey = "";
        public static int BomKAus = -1; public static string BomAlert = "", BomWatch = "", BomOutlook = "", BomError = "";

        static void ParseDst(string html)
        {
            // righe "giorno  v1 ... v24" con valori in nT; 9999 = mancante
            DateTime now = DateTime.UtcNow;
            double last = double.NaN; DateTime lt = DateTime.MinValue;
            foreach (string raw in html.Replace("\r", "").Split('\n'))
            {
                string line = System.Text.RegularExpressions.Regex.Replace(raw, "<[^>]+>", "");
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(line, @"^\s*(\d{1,2})\s+(-?\d.*)$");
                if (!m.Success) continue;
                int day = int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
                if (day < 1 || day > 31) continue;
                // valori di 4 caratteri: possono essere attaccati (es. -100-120); le ore mancanti sono gruppi di 9999 attaccati
                List<double> vs = new List<double>();
                foreach (string tok in m.Groups[2].Value.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(tok, "^9{4,}$")) { for (int q = 0; q < tok.Length / 4; q++) vs.Add(double.NaN); continue; }
                    foreach (System.Text.RegularExpressions.Match mv in System.Text.RegularExpressions.Regex.Matches(tok, @"-?\d+"))
                        vs.Add(double.Parse(mv.Value, CultureInfo.InvariantCulture));
                }
                if (vs.Count != 24) continue;
                for (int h = 0; h < 24; h++)
                {
                    double v = vs[h];
                    if (double.IsNaN(v) || Math.Abs(v) >= 999) continue;
                    try { DateTime t = new DateTime(now.Year, now.Month, day, h, 0, 0, DateTimeKind.Utc); if (t > lt) { lt = t; last = v; } } catch { }
                }
            }
            if (!double.IsNaN(last)) { Dst = last; DstTime = lt; }
        }

        static async Task<string> BomPost(string method, string options)
        {
            string body = "{\"api_key\":\"" + BomKey.Replace("\"", "") + "\"" + (options.Length > 0 ? ",\"options\":{" + options + "}" : "") + "}";
            using (StringContent sc = new StringContent(body, System.Text.Encoding.UTF8, "application/json"))
            using (HttpResponseMessage r = await http.PostAsync(BomApi + method, sc))
                return await r.Content.ReadAsStringAsync();
        }

        static string Field(string json, string name)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(json, "\"" + name + "\"\\s*:\\s*\"?([^\",}\\]]*)");
            return m.Success ? m.Groups[1].Value.Trim() : "";
        }

        public static async Task FetchAuroraAsync(string dataDir)
        {
            string d = Dir(dataDir);
            try
            {
                string html = await http.GetStringAsync(DstUrl);
                File.WriteAllText(Path.Combine(d, "kyoto_dst.html"), html);
                ParseDst(html);
            }
            catch (Exception ex) { LastError += "Dst: " + ex.Message + " "; }
            BomError = "";
            if (BomKey.Length > 0)
            {
                try
                {
                    string k = await BomPost("get-k-index", "\"location\":\"Australian region\"");
                    string ks = Field(k, "index"); int ki; BomKAus = int.TryParse(ks, out ki) ? ki : -1;
                    string a = await BomPost("get-aurora-alert", "");
                    BomAlert = Field(a, "k_aus").Length > 0 ? Lang.F("allerta aurora: K aus {0}, latitudini {1}, fino a {2} UTC", Field(a, "k_aus"), Field(a, "lat_band"), Field(a, "valid_until")) : "";
                    string w = await BomPost("get-aurora-watch", "");
                    BomWatch = Field(w, "k_aus").Length > 0 ? Lang.F("sorveglianza aurora dal {0}: K aus {1} ({2})", Field(w, "start_date"), Field(w, "k_aus"), Field(w, "cause")) : "";
                    string o = await BomPost("get-aurora-outlook", "");
                    BomOutlook = Field(o, "k_aus").Length > 0 ? Lang.F("previsione aurora: K aus {0}, latitudini {1} ({2})", Field(o, "k_aus"), Field(o, "lat_band"), Field(o, "cause")) : "";
                    if (k.Contains("\"errors\"")) BomError = Field(k, "message");
                }
                catch (Exception ex) { BomError = ex.Message; }
            }
            Action u = Updated; if (u != null) u();
        }

        // sintesi per la linguetta AURORA e per il calcolo della probabilita'
        public static string DstSummary()
        {
            if (double.IsNaN(Dst)) return null;
            string lvl = Dst <= -100 ? Lang.T("tempesta intensa") : Dst <= -50 ? Lang.T("tempesta moderata") : Dst <= -30 ? Lang.T("debole disturbo") : Lang.T("quiete");
            return Lang.F("Dst {0:F0} nT ({1}, {2:HH} UTC, Kyoto)", Dst, lvl, DstTime);
        }

        public static string BomSummary()
        {
            if (BomKey.Length == 0) return null;
            if (BomError.Length > 0) return Lang.F("BoM: {0}", BomError);
            List<string> p = new List<string>();
            if (BomKAus >= 0) p.Add(Lang.F("K Australia {0}", BomKAus));
            if (BomAlert.Length > 0) p.Add(BomAlert);
            if (BomWatch.Length > 0) p.Add(BomWatch);
            if (BomOutlook.Length > 0) p.Add(BomOutlook);
            return p.Count == 0 ? Lang.T("BoM: nessuna allerta aurora") : "BoM: " + string.Join("; ", p);
        }

        static string Tr(string v) { return v == null ? "-" : Lang.T(v); }

        // righe in sovraimpressione sulle mappe
        public static List<string> NoteEs()
        {
            List<string> l = new List<string>();
            string es = EsSummary();
            if (es != null) l.Add(Lang.F("N0NBH ({0})", S("updated")));
            if (es != null) l.Add(es);
            if (Solar.Count > 0) l.Add(Lang.F("SFI {0}   K {1}   A {2}", S("solarflux"), S("kindex"), S("aindex")));
            // IK3XTV-IZ3ESV [51]: oltre il 90% delle aperture Es con campo quieto (Kp <= 3). Verifica sul log in 50 MHz di IC8TEM
            // (proposta 9 del confronto delle fonti; maggio-agosto 2020-2025, 702 intervalli di 3 ore con QSO a 500-2300 km, Kp GFZ):
            // l'86% degli intervalli con Es ha Kp <= 3, contro l'88% di tutti gli intervalli della stagione; la frequenza dell'Es
            // non cala con Kp > 3 (17% degli intervalli diurni con Kp 0-2, 22-24% con Kp > 2). Letteratura discorde (Tang e altri [64]).
            // Resta solo un avviso, senza effetto sui calcoli.
            double kp = SpaceWeather.LatestKp;
            if (double.IsNaN(kp)) double.TryParse(S("kindex"), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out kp);
            if (!double.IsNaN(kp) && kp > 3) l.Add(Lang.F("Kp {0}: campo perturbato (solo un avviso: nel log in 50 MHz di IC8TEM l'E-sporadico non cala con Kp > 3; IK3XTV-IZ3ESV [51], Tang e altri [64])", Geo.Fmt(kp, 1)));
            return l;
        }

        public static List<string> NoteAurora()
        {
            List<string> l = new List<string>();
            string au = AuroraSummary();
            if (au != null) { l.Add(Lang.F("N0NBH ({0})", S("updated"))); l.Add(au); }
            string dst = DstSummary(); if (dst != null) l.Add(dst);
            if (Continents.Index == 5 || Continents.Index == 2) { string b = BomSummary(); if (b != null) l.Add(b); }
            if (!double.IsNaN(SpaceWeather.Bz))
                l.Add(Lang.F("NOAA RTSW: Bz {0} nT   Bt {1} nT   vento {2} km/s", Geo.Fmt(SpaceWeather.Bz, 1), Geo.Fmt(SpaceWeather.Bt, 1), Geo.Fmt(SpaceWeather.Speed, 0)));
            return l;
        }

        public static List<string> NoteFai()
        {
            List<string> l = new List<string>();
            string es = EsSummary();
            if (es != null) { l.Add(Lang.F("N0NBH ({0})", S("updated"))); l.Add(es); }
            return l;
        }
    }
}
