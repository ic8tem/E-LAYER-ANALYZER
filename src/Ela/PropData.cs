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

        static string Tr(string v) { return v == null ? "-" : Lang.T(v); }

        // righe in sovraimpressione sulle mappe
        public static List<string> NoteEs()
        {
            List<string> l = new List<string>();
            string es = EsSummary();
            if (es != null) l.Add(Lang.F("N0NBH ({0})", S("updated")));
            if (es != null) l.Add(es);
            if (Solar.Count > 0) l.Add(Lang.F("SFI {0}   K {1}   A {2}", S("solarflux"), S("kindex"), S("aindex")));
            return l;
        }

        public static List<string> NoteAurora()
        {
            List<string> l = new List<string>();
            string au = AuroraSummary();
            if (au != null) { l.Add(Lang.F("N0NBH ({0})", S("updated"))); l.Add(au); }
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
