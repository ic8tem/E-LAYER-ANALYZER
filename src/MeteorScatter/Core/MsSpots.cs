// YA-MS - raccolta degli spot meteor scatter dai DX cluster, da RBN e da PSKReporter (via telnet), come in
// E-LAYER ANALYZER: tre sessioni telnet, lettura delle righe "DX de ...", filtro e archivio.
//
// Filtro (regola dell'autore, IC8TEM): si tengono solo gli spot che contengono una di queste parole, in maiuscolo,
// minuscolo o misto:
//   MSK144, FSK441, PSK2K, JTMS, JT6M = modi digitali usati solo per il meteor scatter (lo spot e' MS di sicuro);
//   <MS>                             = indicazione generica che il collegamento e' avvenuto via meteor scatter.
// L'elenco si puo' ampliare dalla linguetta; ogni parola si puo' spegnere. Le parole alfanumeriche valgono solo come
// parola intera (MSK144 non scatta dentro "XMSK1440"); <MS> ammette spazi dentro le parentesi (< ms >).
//
// Le sessioni sono statiche: sopravvivono al cambio di lingua o di schema di colori (la finestra viene ricreata).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace YAMS
{
    public class MsSpot
    {
        public DateTime Utc;
        public double FreqKHz;
        public string DxCall = "", Spotter = "", Comment = "", Line = "";
        public int Source;                 // 0 = cluster, 1 = RBN, 2 = PSKReporter (cluster DM4X), 3 = PSKReporter MQTT, 4 = WSJT-X UDP
        public string Match = "";          // parola del filtro trovata (MSK144, <MS>, ...)
        public string Mode = "";           // modo dichiarato dalla fonte (PSKReporter MQTT: MSK144, FSK441...)
        public string LocSpotter = "", LocDx = "";
        public double DistKm = double.NaN;
        public bool Shared;                // ricevuto da un altro utente di YA-MS (YamsShare): messaggio rivolto a me
        public DateTime Received = DateTime.MinValue;   // ora (UTC del computer) in cui lo spot e' arrivato: lampeggio nel traffico in diretta
        public string Band { get { return MsBands.Name(FreqKHz); } }
        public string Key { get { return DxCall + "|" + Spotter + "|" + Math.Round(FreqKHz) + "|" + Utc.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture); } }
    }

    public static class MsBands
    {
        public static readonly string[] Names = { "28", "50", "70", "144", "222", "432" };

        public static string Name(double kHz)
        {
            if (kHz >= 28000 && kHz <= 29700) return "28";
            if (kHz >= 50000 && kHz <= 54000) return "50";
            if (kHz >= 69900 && kHz <= 71000) return "70";
            if (kHz >= 144000 && kHz <= 148000) return "144";
            if (kHz >= 220000 && kHz <= 225000) return "222";
            if (kHz >= 430000 && kHz <= 440000) return "432";
            return "";
        }
    }

    // stazione in ascolto nei modi MS secondo PSKReporter (chi riceve negli spot MQTT)
    public class MsListener
    {
        public string Call = "", Loc = "", Band = "", Modes = "", Soft = "";
        public DateTime LastUtc;          // ultimo spot ricevuto (MQTT)
        public DateTime SeenUtc;          // ultima volta tra i monitor attivi della mappa di PSKReporter
        public int Count;                 // spot ricevuti; 0 = solo monitor attivo, nessuno spot MS ancora
        public DateTime Latest { get { return LastUtc > SeenUtc ? LastUtc : SeenUtc; } }
    }

    public class MsFilterWord
    {
        public string Text;
        public bool Enabled = true;
        public bool BuiltIn;
        public int Count;                  // spot accettati grazie a questa parola (da quando il programma e' aperto)
        Regex rx;

        public MsFilterWord(string text, bool enabled, bool builtIn)
        {
            Text = text.Trim(); Enabled = enabled; BuiltIn = builtIn;
            string t = Text;
            string pat;
            if (t.StartsWith("<") && t.EndsWith(">") && t.Length > 2)
                pat = "<\\s*" + Regex.Escape(t.Substring(1, t.Length - 2).Trim()) + "\\s*>";
            else
                pat = "(?<![A-Za-z0-9])" + Regex.Escape(t) + "(?![A-Za-z0-9])";
            rx = new Regex(pat, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        public bool IsMatch(string s) { return rx.IsMatch(s ?? ""); }
    }

    public static class MsCollector
    {
        public static readonly string[] SourceNames = { "Cluster", "RBN", "PSKReporter", "PSKR MQTT", "WSJT-X UDP" };
        public static readonly string[] BuiltInWords = { "MSK144", "FSK441", "FSK41", "PSK2K", "JTMS", "JT6M", "<MS>" };   // FSK41: grafia abbreviata di FSK441 indicata dall'autore

        // valori di partenza delle tre sessioni: nome, host, porta, comandi iniziali, nota
        public static readonly string[][] Defaults =
        {
            new[] { "DX cluster a scelta", "on0dxk.dyndns.org", "8000", "set/filter dxbm/pass 10;set/filter dxbm/pass 6;set/filter dxbm/pass 4;set/filter dxbm/pass 2", "Cluster scelto dall'utente. Su ON0DXK le bande VHF sono filtrate: si abilitano con set/filter dxbm/pass 6 (poi 4, 2...). Altro esempio: hamqth.com 7300." },
            new[] { "Reverse Beacon Network", "telnet.reversebeacon.net", "7001", "", "RBN: porta 7000 = CW e RTTY, porta 7001 = FT8. Login con il solo nominativo. Gli skimmer RBN raramente decodificano i modi MS: serve soprattutto per gli spot con <MS> nel commento." },
            new[] { "Cluster con PSKReporter", "dm4x.ddns.net", "8500", "", "Cluster di DM4X con gli spot di PSKReporter (dm4x.ddns.net, porta 8500): qui arrivano le decodifiche MSK144 di WSJT-X e MSHV. In alternativa HamAlert (hamalert.org 7300, serve un account con i trigger)." }
        };

        public const string OldClusterCommands = "set/filter dxbm/pass 6;set/filter dxbm/pass 2";   // fino alla 1.8.3 (senza 28 e 70 MHz)

        public static readonly TelnetSession[] Sessions = { new TelnetSession(), new TelnetSession(), new TelnetSession() };
        public static readonly List<MsSpot> Spots = new List<MsSpot>();          // accettati (ultimi 5000)
        public static readonly List<MsFilterWord> Words = new List<MsFilterWord>();
        public static readonly HashSet<string> Bands = new HashSet<string>(MsBands.Names);
        public static readonly Queue<string>[] ConsoleLines = { new Queue<string>(), new Queue<string>(), new Queue<string>(), new Queue<string>(), new Queue<string>() };
        public static int[] Lines = new int[5], Parsed = new int[5], Accepted = new int[5];
        public static int RejectedNoWord, RejectedBand, DroppedLow, DroppedCw, DroppedNear;
        // regola dell'autore (IC8TEM, 13/11/2026): gli spot CW si scartano subito, come quelli sotto 27990 kHz (non vanno nella
        // console e non si memorizzano). CW = parola CW o WPM nel commento (formato RBN "CW 12 dB 25 WPM"), oppure
        // frequenza nel segmento solo CW dei 10 m (28000-28070 kHz). Restano gli spot dichiarati meteor scatter con <MS>.
        public static bool DropCw = true;
        // regola dell'autore (IC8TEM, 13/11/2026): gli spot tra stazioni piu' vicine di NearKm (500 km) si scartano subito:
        // a quella distanza il collegamento e' diretto o troposferico, non meteor scatter. Senza i due locator lo spot resta.
        public static bool DropNear = true;
        public static int NearKm = 500;
        public static bool IsNear(MsSpot s)
        {
            if (s == null) return false;
            if (double.IsNaN(s.DistKm)) Locate(s);
            return !double.IsNaN(s.DistKm) && s.DistKm < NearKm;
        }
        // spot da non mostrare con i filtri attuali (anche quelli gia' in memoria o letti dall'archivio di una versione
        // precedente): CW e stazioni piu' vicine della distanza minima
        public static bool Hidden(MsSpot s) { return (DropCw && IsCw(s)) || (DropNear && IsNear(s)); }
        // spot MS delle bande non scelte nella Raccolta dati: non si memorizzano su disco, ma si mostrano comunque nel
        // Traffico in diretta (richiesta dell'autore, 13/11/2026); ultimi 2000
        public static readonly List<MsSpot> OtherBandSpots = new List<MsSpot>();
        static readonly HashSet<string> otherKeys = new HashSet<string>();
        // ---- spot WSJT-X UDP che riguardano la mia stazione (regola dell'autore, 14/11/2026): la decodifica contiene il
        // mio nominativo (chi mi chiama o mi risponde). Si disegnano nel Traffico in diretta con un tratteggio che
        // lampeggia sempre, in qualsiasi modo e a qualsiasi distanza. Ultimi 500, non si memorizzano.
        public static readonly List<MsSpot> MyUdp = new List<MsSpot>();
        public static bool IsMine(MsSpot s)
        {
            if (s.Source != 4) return false;
            string me = WsjtxUdp.DeCall;
            if (string.IsNullOrEmpty(me)) return false;
            return Regex.IsMatch(s.Comment ?? "", @"(?<![A-Z0-9/])" + Regex.Escape(me) + @"(?![A-Z0-9])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
        static bool NoteMine(MsSpot s, DateTime now)
        {
            if (!IsMine(s)) return false;
            Locate(s);
            s.Received = now;
            MyUdp.Add(s);
            if (MyUdp.Count > 500) MyUdp.RemoveRange(0, 100);
            return true;
        }

        // ---- clausola degli FT8 isolati (regola dell'autore, IC8TEM, 14/11/2026) ----
        // Uno spot FT8 oltre i 1100 km che nei 3 minuti intorno non ha tratte parallele (stessa banda, punto medio entro
        // 300 km e direzione entro 15 gradi, coppia di stazioni diversa) non ha l'aspetto dell'E-sporadico o della tropo,
        // che danno sempre molte tratte vicine e parallele: puo' essere una scia meteorica. Si mostra nel Traffico in
        // diretta in verde chiaro, con la riga della tabella che lampeggia sempre. Non entra nell'archivio MS.
        public class Ft8Path
        {
            public MsSpot S;
            public LatLon Mid;
            public double Axis;        // direzione della tratta, 0-180 gradi
            public string Pair = "";
            public bool Parallel;      // trovata almeno una tratta parallela entro 3 minuti (prima o dopo), o ricevente che sente altri
            public bool NeedsMqtt;     // spot di un cluster o di RBN: vale solo se la stessa coppia arriva anche da PSKReporter MQTT
            public bool Confirmed;     // stessa coppia vista anche in PSKReporter MQTT entro 3 minuti
            public string Rx = "", Tx = "";
            public bool Show { get { return !Parallel && (!NeedsMqtt || Confirmed); } }
        }
        // nominativo senza suffissi degli skimmer ("EI4HQ-#" -> "EI4HQ")
        static string BaseCall(string c)
        {
            c = (c ?? "").Trim().ToUpperInvariant();
            int i = c.IndexOf('-'); return i > 0 ? c.Substring(0, i) : c;
        }
        // ricezioni FT8 per ricevente e banda viste in PSKReporter MQTT (ultimi 10 minuti): chi sente molte stazioni ha la
        // banda aperta, il suo spot non e' isolato (regola dell'autore, 14/11/2026)
        static readonly Dictionary<string, List<KeyValuePair<DateTime, string>>> rxHeard = new Dictionary<string, List<KeyValuePair<DateTime, string>>>();
        public static bool Ft8Enabled = true;
        public const double Ft8MinKm = 1100, Ft8Minutes = 3, Ft8MidKm = 300, Ft8Deg = 15;
        public static int Ft8Seen;
        static readonly Dictionary<string, List<Ft8Path>> ft8Recent = new Dictionary<string, List<Ft8Path>>();
        public static readonly List<Ft8Path> Ft8Long = new List<Ft8Path>();   // FT8 oltre i 1100 km (ultimi 2000)
        static readonly HashSet<string> ft8Keys = new HashSet<string>();
        static readonly Regex rxFt8 = new Regex(@"(?<![A-Za-z0-9])FT8(?![A-Za-z0-9])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public static bool IsFt8(MsSpot s)
        {
            return string.Equals(s.Mode, "FT8", StringComparison.OrdinalIgnoreCase) || rxFt8.IsMatch(s.Comment ?? "");
        }

        static double AxisDiff(double a, double b) { double d = Math.Abs(a - b) % 180; return Math.Min(d, 180 - d); }

        // true se e' arrivato un FT8 lungo e isolato da mostrare
        static bool NoteFt8(MsSpot s, DateTime now)
        {
            if (!Ft8Enabled || !IsFt8(s)) return false;
            string band = s.Band;
            if (band.Length == 0 || band == "28") return false;   // FT8 sui 28 MHz: tolto in ogni caso (regola dell'autore, 14/11/2026)
            Ft8Seen++;
            Locate(s);
            LatLon a, b;
            if (!Astro.TryLocator(s.LocSpotter, out a) || !Astro.TryLocator(s.LocDx, out b)) return false;
            double km = Astro.DistanceKm(a, b);
            if (km < 50) return false;
            string rx = BaseCall(s.Spotter), tx = BaseCall(s.DxCall);
            bool mqtt = s.Source == 3;
            Ft8Path p = new Ft8Path
            {
                S = s, Mid = Astro.Destination(a, Astro.Bearing(a, b), km / 2), Axis = Astro.Bearing(a, b) % 180,
                Pair = string.CompareOrdinal(rx, tx) < 0 ? rx + "|" + tx : tx + "|" + rx, Rx = rx, Tx = tx,
                NeedsMqtt = s.Source <= 2   // cluster, RBN, cluster con PSKReporter: serve la conferma da MQTT
            };
            List<Ft8Path> l;
            if (!ft8Recent.TryGetValue(band, out l)) { l = new List<Ft8Path>(); ft8Recent[band] = l; }
            // le tratte piu' vecchie di 10 minuti non servono piu'
            DateTime old = now.AddMinutes(-10);
            int cut = 0; while (cut < l.Count && l[cut].S.Received < old) cut++;
            if (cut > 0) l.RemoveRange(0, cut);
            foreach (Ft8Path q in l)
            {
                if (Math.Abs((q.S.Utc - s.Utc).TotalMinutes) > Ft8Minutes) continue;
                if (q.Pair == p.Pair)
                {
                    // stessa coppia: conferma da PSKReporter MQTT per gli spot dei cluster e di RBN
                    if (mqtt) q.Confirmed = true;
                    if (q.S.Source == 3) p.Confirmed = true;
                    continue;
                }
                if (AxisDiff(q.Axis, p.Axis) <= Ft8Deg && Astro.DistanceKm(q.Mid, p.Mid) <= Ft8MidKm) { q.Parallel = true; p.Parallel = true; }
            }
            // ricevente che in MQTT sente altre stazioni lontane (oltre 1100 km) nella stessa banda entro 3 minuti: banda aperta
            string rk = rx + "|" + band;
            List<KeyValuePair<DateTime, string>> h;
            if (!rxHeard.TryGetValue(rk, out h)) { h = new List<KeyValuePair<DateTime, string>>(); rxHeard[rk] = h; }
            h.RemoveAll(x => x.Key < s.Utc.AddMinutes(-10));
            foreach (KeyValuePair<DateTime, string> x in h)
                if (x.Value != tx && Math.Abs((x.Key - s.Utc).TotalMinutes) <= Ft8Minutes) { p.Parallel = true; break; }
            if (mqtt && km > Ft8MinKm)   // solo le ricezioni lunghe: le stazioni vicine si sentono sempre
            {
                h.Add(new KeyValuePair<DateTime, string>(s.Utc, tx));
                if (h.Count > 500) h.RemoveRange(0, 100);
                foreach (Ft8Path q in Ft8Long)
                    if (q.Rx == rx && q.Tx != tx && q.S.Band == band && Math.Abs((q.S.Utc - s.Utc).TotalMinutes) <= Ft8Minutes) q.Parallel = true;
                if (rxHeard.Count > 20000) rxHeard.Clear();
            }
            s.Received = now;
            l.Add(p);
            if (l.Count > 30000) l.RemoveRange(0, 5000);
            if (km <= Ft8MinKm) return false;
            if ((s.Spotter ?? "").Contains("-#")) return false;   // skimmer RBN: mai nella clausola degli FT8 isolati
            s.DistKm = km;
            if (!ft8Keys.Add(s.Key + "|" + band)) return false;
            Ft8Long.Add(p);
            if (Ft8Long.Count > 2000) { for (int k = 0; k < 200; k++) ft8Keys.Remove(Ft8Long[k].S.Key + "|" + Ft8Long[k].S.Band); Ft8Long.RemoveRange(0, 200); }
            return p.Show;
        }

        // tutti gli spot da disegnare nel traffico in diretta, in ordine di arrivo
        public static List<MsSpot> MapSpots()
        {
            List<MsSpot> r = new List<MsSpot>(Spots.Count + OtherBandSpots.Count);
            foreach (MsSpot s in Spots) if (!Hidden(s)) r.Add(s);
            foreach (MsSpot s in OtherBandSpots) if (!Hidden(s)) r.Add(s);
            r.Sort((x, y) => x.Utc.CompareTo(y.Utc));
            return r;
        }
        static readonly Regex rxCw = new Regex(@"(?<![A-Za-z0-9])(CW|WPM)(?![A-Za-z0-9])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        static readonly Regex rxMsTag = new Regex(@"<\s*MS\s*>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        public static bool IsCw(MsSpot s)
        {
            if (s == null || rxMsTag.IsMatch(s.Comment ?? "")) return false;
            if (string.Equals(s.Mode, "CW", StringComparison.OrdinalIgnoreCase)) return true;
            if (rxCw.IsMatch(s.Comment ?? "")) return true;
            return s.FreqKHz >= 28000 && s.FreqKHz < 28070;
        }
        // stazioni in ascolto nei modi MS (PSKReporter MQTT), per nominativo e banda
        public static readonly Dictionary<string, MsListener> Listeners = new Dictionary<string, MsListener>();
        public static readonly string[] ListenerBands = { "28", "50", "70", "144", "432" };
        static void NoteListener(MsSpot s)
        {
            if (Array.IndexOf(PskMqtt.MsModes, s.Mode) < 0) return;
            string b = s.Band;
            if (Array.IndexOf(ListenerBands, b) < 0 || s.Spotter.Length == 0) return;
            string k = s.Spotter + "|" + b;
            MsListener l;
            if (!Listeners.TryGetValue(k, out l)) { l = new MsListener { Call = s.Spotter, Band = b }; Listeners[k] = l; }
            string loc = Refine(s.LocSpotter, s.Spotter);
            if (loc.Length > 0) l.Loc = loc;
            if (s.Utc > l.LastUtc) l.LastUtc = s.Utc;
            l.Count++;
            if (l.Modes.IndexOf(s.Mode, StringComparison.Ordinal) < 0) l.Modes = l.Modes.Length == 0 ? s.Mode : l.Modes + " " + s.Mode;
            PruneListeners();
        }
        static void PruneListeners()
        {
            if (Listeners.Count <= 20000) return;
            DateTime lim = DateTime.UtcNow.AddHours(-24);
            List<string> old = new List<string>();
            foreach (KeyValuePair<string, MsListener> kv in Listeners) if (kv.Value.Latest < lim) old.Add(kv.Key);
            foreach (string o in old) Listeners.Remove(o);
        }
        // monitor attivi della mappa di PSKReporter (anche senza spot ricevuti): stessa chiave nominativo|banda
        static void NoteMonitor(PskMonitor m)
        {
            if (Array.IndexOf(ListenerBands, m.Band) < 0) return;
            string k = m.Call + "|" + m.Band;
            MsListener l;
            if (!Listeners.TryGetValue(k, out l)) { l = new MsListener { Call = m.Call, Band = m.Band }; Listeners[k] = l; }
            string loc = Refine(m.Loc, m.Call);
            if (loc.Length > 0) l.Loc = loc; else if (l.Loc.Length == 0) l.Loc = m.Loc;
            if (m.SeenUtc > l.SeenUtc) l.SeenUtc = m.SeenUtc;
            if (m.Soft.Length > 0) l.Soft = m.Soft;
            if (l.Modes.IndexOf(m.Mode, StringComparison.Ordinal) < 0) l.Modes = l.Modes.Length == 0 ? m.Mode : l.Modes + " " + m.Mode;
            PruneListeners();
        }
        // regola dell'autore (IC8TEM): le righe "DX de" con frequenza sotto 27990,0 kHz si scartano subito, in locale:
        // non vanno nella console, non si contano come spot e non si memorizzano
        // (opzionale: casella nella linguetta, accesa di partenza)
        public const double MinKHz = 27990.0;
        public static bool DropLow = true;
        // PRIMO criterio (regola dell'autore, IC8TEM): le frequenze tipiche degli spot MS. Uno spot che cade in una di
        // queste finestre si accetta anche senza parole del modo, a meno che il commento dichiari un'altra propagazione
        // (<TR> tropo, <AS>/<ACS> aircraft scatter, <AU> aurora, <ES> sporadico E, <FAI>, <F2>): in quel caso, e per
        // tutto cio' che sta fuori dalle finestre, decide il secondo criterio (modi digitali MS e <MS>).
        public const string DefaultWindows = "50190-50300; 70100-70400; 144290-144399";
        public const string DefaultOtherTags = "<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>";
        public static bool UseWindows = true;
        public static string WindowsText = DefaultWindows, OtherTagsText = DefaultOtherTags;
        public static int AcceptedWindow, RejectedOther;
        static List<double[]> windows = ParseWindows(DefaultWindows);
        static Regex rxOther = BuildOther(DefaultOtherTags);

        public static List<double[]> ParseWindows(string t)
        {
            List<double[]> l = new List<double[]>();
            foreach (Match m in Regex.Matches(t ?? "", @"(\d+(?:[.,]\d+)?)\s*-\s*(\d+(?:[.,]\d+)?)"))
            {
                double a = double.Parse(m.Groups[1].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
                double b = double.Parse(m.Groups[2].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
                if (b < a) { double x = a; a = b; b = x; }
                l.Add(new[] { a, b });
            }
            return l;
        }

        static Regex BuildOther(string t)
        {
            List<string> tags = new List<string>();
            foreach (Match m in Regex.Matches(t ?? "", @"<\s*([A-Za-z0-9]+)\s*>")) tags.Add(Regex.Escape(m.Groups[1].Value));
            if (tags.Count == 0) return null;
            return new Regex(@"<\s*(" + string.Join("|", tags) + @")\s*>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        public static void SetWindows(string t) { WindowsText = t ?? ""; windows = ParseWindows(WindowsText); }
        public static void SetOtherTags(string t) { OtherTagsText = t ?? ""; rxOther = BuildOther(OtherTagsText); }

        // finestra MS che contiene la frequenza ("144290-144399"), null se nessuna
        public static string WindowOf(double kHz)
        {
            foreach (double[] w in windows)
                if (kHz >= w[0] && kHz <= w[1] + 0.999) return w[0].ToString("0.###", CultureInfo.InvariantCulture) + "-" + w[1].ToString("0.###", CultureInfo.InvariantCulture);
            return null;
        }

        // scritta di altra propagazione nel commento (<ES>, <TR>...), null se nessuna
        public static string OtherTag(string comment)
        {
            if (rxOther == null) return null;
            Match m = rxOther.Match(comment ?? "");
            return m.Success ? "<" + m.Groups[1].Value.ToUpperInvariant() + ">" : null;
        }

        // decisione sullo spot: etichetta del criterio che lo accetta (finestra o parola), null se scartato.
        // word = parola del secondo criterio (null se accettato per finestra); other = scritta di altra propagazione trovata
        public static string Decide(MsSpot s, out MsFilterWord word, out string other)
        {
            word = FindWord(s.Comment + " " + s.DxCall);
            other = OtherTag(s.Comment);
            if (UseWindows && other == null)
            {
                string win = WindowOf(s.FreqKHz);
                if (win != null) { word = null; return win; }
            }
            return word != null ? word.Text : null;
        }

        public static string Dir;
        public static bool Started;        // connessione automatica gia' eseguita (una sola volta per avvio del programma)
        public static string[] StateText = { "Non connesso", "Non connesso", "Non connesso" };
        static readonly HashSet<string> keys = new HashSet<string>();
        static readonly Queue<KeyValuePair<int, string>> pending = new Queue<KeyValuePair<int, string>>();
        static readonly Queue<MsSpot> pendingSpots = new Queue<MsSpot>();   // spot gia' letti (PSKReporter MQTT)
        static readonly object lk = new object();
        static bool hooked;

        public static void Init(string dataDir, Config cfg)
        {
            Dir = Path.Combine(dataDir, "spot_ms");
            try { Directory.CreateDirectory(Dir); } catch { }
            if (!hooked)
            {
                hooked = true;
                for (int i = 0; i < 3; i++)
                {
                    int k = i;
                    Sessions[i].Name = Defaults[i][0];
                    Sessions[i].LineReceived += l => { lock (lk) pending.Enqueue(new KeyValuePair<int, string>(k, l)); };
                    Sessions[i].StateChanged += st => { StateText[k] = st; };
                }
                PskMqtt.SpotReceived += (sp, line) => { lock (lk) { pendingSpots.Enqueue(sp); while (pendingSpots.Count > 20000) pendingSpots.Dequeue(); } };
                // decodifiche di WSJT-X / JTDX / MSHV via UDP (fonte 4): un solo ricevitore per tutto il programma
                WsjtxUdp.SpotDecoded += sp => { lock (lk) { pendingSpots.Enqueue(sp); while (pendingSpots.Count > 20000) pendingSpots.Dequeue(); } };
                // messaggi rivolti a me sentiti da altri utenti di YA-MS (scambio cifrato via MQTT)
                YamsShare.SpotReceived += sp => { lock (lk) { pendingSpots.Enqueue(sp); while (pendingSpots.Count > 20000) pendingSpots.Dequeue(); } };
                LoadWords(cfg);
                DropLow = cfg.GetB("rd_scarta_sotto_28", true);
                DropCw = cfg.GetB("rd_scarta_cw", true);
                DropNear = cfg.GetB("rd_scarta_vicini", true);
                int nk; NearKm = int.TryParse(cfg.Get("rd_scarta_vicini_km", "500"), out nk) && nk >= 0 && nk <= 5000 ? nk : 500;
                UseWindows = cfg.GetB("rd_finestre_attive", true);
                SetWindows(cfg.Get("rd_finestre", DefaultWindows));
                SetOtherTags(cfg.Get("rd_altre_propagazioni", DefaultOtherTags));
                string b = cfg.Get("rd_bande", "");
                if (b.Length > 0) { Bands.Clear(); foreach (string x in b.Split(',')) if (x.Trim().Length > 0) Bands.Add(x.Trim()); }
                LoadRecent();
                WsjtxUdp.Load(cfg);
                if (WsjtxUdp.AutoStart && !WsjtxUdp.Running) WsjtxUdp.Start();
                PskActive.Enabled = cfg.GetB("psk_monitor_attivi", true);
                Ft8Enabled = cfg.GetB("ft8_isolati", true);
                // metodo nascosto: rimando a PSKReporter delle decodifiche MS via UDP se il programma digitale non le manda
                PskRelay.Enabled = cfg.GetB("psk_rimando", true);
                cfg.Set("psk_rimando", PskRelay.Enabled);
                PskRelay.Start();
                YamsShare.Enabled = cfg.GetB("yams_scambio", true);
                cfg.Set("yams_scambio", YamsShare.Enabled);
                YamsShare.Start(dataDir);
                PskActive.Start();
            }
        }

        static void LoadWords(Config cfg)
        {
            Words.Clear();
            string s = cfg.Get("rd_filtri", "");
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (s.Length > 0)
                foreach (string it in s.Split('\u0001'))
                {
                    int p = it.IndexOf('|');
                    if (p < 0) continue;
                    string w = it.Substring(p + 1).Trim();
                    if (w.Length == 0 || !seen.Add(w)) continue;
                    Words.Add(new MsFilterWord(w, it.Substring(0, p) == "1", Array.IndexOf(BuiltInWords, w.ToUpperInvariant()) >= 0));
                }
            foreach (string w in BuiltInWords)   // le parole dell'autore ci sono sempre (si possono solo spegnere)
                if (!seen.Contains(w)) { Words.Insert(Math.Min(Words.Count, Array.IndexOf(BuiltInWords, w)), new MsFilterWord(w, true, true)); seen.Add(w); }
        }

        public static void Save(Config cfg)
        {
            StringBuilder sb = new StringBuilder();
            foreach (MsFilterWord w in Words) { if (sb.Length > 0) sb.Append('\u0001'); sb.Append(w.Enabled ? "1|" : "0|").Append(w.Text); }
            cfg.Set("rd_filtri", sb.ToString());
            cfg.Set("rd_bande", string.Join(",", Bands));
            cfg.Set("rd_scarta_sotto_28", DropLow);
            cfg.Set("rd_scarta_cw", DropCw);
            cfg.Set("rd_scarta_vicini", DropNear);
            cfg.Set("rd_scarta_vicini_km", NearKm.ToString(CultureInfo.InvariantCulture));
            cfg.Set("rd_finestre_attive", UseWindows);
            cfg.Set("rd_finestre", WindowsText);
            cfg.Set("rd_altre_propagazioni", OtherTagsText);
            WsjtxUdp.Save(cfg);
        }

        public static void Shutdown() { foreach (TelnetSession s in Sessions) s.Disconnect(); PskMqtt.Disconnect(); WsjtxUdp.Stop(); PskActive.Stop(); YamsShare.Stop(); }

        // E-LAYER ANALYZER: riga ricevuta da una sessione di DATA COLLECTION (0 = cluster, 1 = RBN, 2 = PSKReporter)
        public static void Feed(int source, string line) { lock (lk) pending.Enqueue(new KeyValuePair<int, string>(source, line)); }

        // svuota la tabella degli spot e l'elenco dei doppioni (uno spot uguale arrivato dopo torna a comparire)
        public static void ClearSpots() { Spots.Clear(); keys.Clear(); OtherBandSpots.Clear(); otherKeys.Clear(); }

        // solo per le fotografie di controllo (--screenshot): spot di esempio, non salvati su disco
        public static void AddDemo(MsSpot s) { if (keys.Add(s.Key)) Spots.Add(s); }

        // parola del filtro trovata nella riga (null = nessuna)
        public static MsFilterWord FindWord(string text)
        {
            foreach (MsFilterWord w in Words) if (w.Enabled && w.IsMatch(text)) return w;
            return null;
        }

        // elabora le righe arrivate (thread dell'interfaccia); true se ci sono spot nuovi
        public static bool Process()
        {
            List<KeyValuePair<int, string>> got = new List<KeyValuePair<int, string>>();
            List<MsSpot> gotSpots = new List<MsSpot>();
            lock (lk) { while (pending.Count > 0) got.Add(pending.Dequeue()); while (pendingSpots.Count > 0) gotSpots.Add(pendingSpots.Dequeue()); }
            bool added = false;
            DateTime now = DateTime.UtcNow;
            foreach (PskMonitor m in PskActive.Take()) NoteMonitor(m);
            foreach (KeyValuePair<int, string> kv in got)
            {
                int i = kv.Key;
                Lines[i]++;
                MsSpot s = Parse(kv.Value, i, now);
                if (DropLow && s != null && s.FreqKHz < MinKHz) { DroppedLow++; continue; }
                if (DropCw && s != null && IsCw(s)) { DroppedCw++; continue; }
                if (s != null && NoteFt8(s, now)) added = true;   // FT8 lunghi e isolati (prima dello scarto dei vicini)
                if (DropNear && s != null && IsNear(s)) { DroppedNear++; continue; }
                ConsoleLines[i].Enqueue(kv.Value);
                while (ConsoleLines[i].Count > 200) ConsoleLines[i].Dequeue();
                if (s == null) continue;
                Parsed[i]++;
                if (Accept(s, i)) added = true;
            }
            // spot di PSKReporter via MQTT (fonte 3) e di WSJT-X via UDP (fonte 4): arrivano gia' letti, passano dallo stesso filtro
            foreach (MsSpot s in gotSpots)
            {
                if (s.Shared)
                {
                    // da un altro utente di YA-MS: FT8 solo se oltre i 1100 km e senza tratte parallele (regola dell'autore)
                    if (IsFt8(s) && !NoteFt8(s, now)) continue;
                    if (NoteMine(s, now)) added = true;
                    continue;
                }
                int k = s.Source == 4 ? 4 : 3;
                Lines[k]++;
                if (DropLow && s.FreqKHz < MinKHz) { DroppedLow++; continue; }
                if (DropCw && IsCw(s)) { DroppedCw++; continue; }
                NoteListener(s);   // chi ascolta resta sulla mappa anche se lo spot e' vicino
                if (NoteMine(s, now)) added = true;
                if (NoteFt8(s, now)) added = true;
                if (s.Source == 3 && IsFt8(s)) continue;   // FT8 da MQTT: servono solo alla clausola degli FT8 isolati
                if (DropNear && IsNear(s)) { DroppedNear++; continue; }
                ConsoleLines[k].Enqueue(s.Line);
                while (ConsoleLines[k].Count > 200) ConsoleLines[k].Dequeue();
                Parsed[k]++;
                if (Accept(s, k)) added = true;
            }
            return added;
        }

        // filtro dello spot (criteri dell'autore e bande) e memorizzazione; true se accettato
        static bool Accept(MsSpot s, int i)
        {
            MsFilterWord w; string other;
            string why = Decide(s, out w, out other);
            if (why == null) { if (other != null) RejectedOther++; else RejectedNoWord++; return false; }
            if (s.Band.Length == 0) { RejectedBand++; return false; }
            if (!Bands.Contains(s.Band))
            {
                RejectedBand++;
                s.Match = why;
                if (!otherKeys.Add(s.Key)) return false;
                Locate(s);
                s.Received = DateTime.UtcNow;
                OtherBandSpots.Add(s);
                if (OtherBandSpots.Count > 2000) { for (int k = 0; k < 200; k++) otherKeys.Remove(OtherBandSpots[k].Key); OtherBandSpots.RemoveRange(0, 200); }
                return true;
            }
            s.Match = why;
            if (!keys.Add(s.Key)) return false;
            if (w != null) w.Count++; else AcceptedWindow++;
            Accepted[i]++;
            Locate(s);
            s.Received = DateTime.UtcNow;
            Spots.Add(s);
            if (Spots.Count > 5000) { for (int k = 0; k < 500; k++) keys.Remove(Spots[k].Key); Spots.RemoveRange(0, 500); }
            Append(s);
            return true;
        }

        // ---- lettura della riga "DX de SPOTTER:  FREQ  DXCALL  commento  HHMMZ [LOC]"
        static readonly Regex rxDx = new Regex(@"^DX de\s+([A-Z0-9/\-#_]+)[:>]?\s+(\d+(?:\.\d+)?)\s+([A-Z0-9/]+)\s+(.*?)\s*(\d{4})Z(?:\s+([A-R]{2}\d{2}(?:[A-X]{2})?))?\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static readonly Regex rxTwoLoc = new Regex(@"\b([A-R]{2}\d{2}(?:[A-X]{2})?)\s*(?:<\s*MS\s*>|\(\s*MS\s*\)|<-?>|[-/>\s])\s*([A-R]{2}\d{2}(?:[A-X]{2})?)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static readonly Regex rxLoc = new Regex(@"\b([A-R]{2}\d{2}(?:[A-X]{2})?)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static MsSpot Parse(string line, int source, DateTime nowUtc)
        {
            if (line == null) return null;
            int k = line.IndexOf("DX de ", StringComparison.OrdinalIgnoreCase);
            if (k < 0) return null;
            Match m = rxDx.Match(line.Substring(k).TrimEnd());
            if (!m.Success) return null;
            double f;
            if (!double.TryParse(m.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out f)) return null;
            MsSpot s = new MsSpot
            {
                Source = source, Line = line, FreqKHz = f,
                Spotter = m.Groups[1].Value.ToUpperInvariant(), DxCall = m.Groups[3].Value.ToUpperInvariant(), Comment = m.Groups[4].Value.Trim()
            };
            string hhmm = m.Groups[5].Value;
            int hh = int.Parse(hhmm.Substring(0, 2), CultureInfo.InvariantCulture), mm = int.Parse(hhmm.Substring(2, 2), CultureInfo.InvariantCulture);
            DateTime t = new DateTime(nowUtc.Year, nowUtc.Month, nowUtc.Day, Math.Min(23, hh), Math.Min(59, mm), 0, DateTimeKind.Utc);
            if (t > nowUtc.AddMinutes(30)) t = t.AddDays(-1);
            s.Utc = t;
            Match tl = rxTwoLoc.Match(s.Comment);
            if (tl.Success && IsLoc(tl.Groups[1].Value) && IsLoc(tl.Groups[2].Value))
            {
                s.LocSpotter = tl.Groups[1].Value.ToUpperInvariant(); s.LocDx = tl.Groups[2].Value.ToUpperInvariant();
                // il primo locatore e' quello che la stazione che spotta dichiara di se': corregge CALL3 (come LIVETROPOVHF)
                CallDb.SetCluster(s.Spotter, s.LocSpotter);
            }
            else
            {
                Match l = rxLoc.Match(s.Comment);
                if (l.Success && IsLoc(l.Groups[1].Value)) s.LocDx = l.Groups[1].Value.ToUpperInvariant();
            }
            if (s.LocSpotter.Length == 0 && m.Groups[6].Success) s.LocSpotter = m.Groups[6].Value.ToUpperInvariant();
            return s;
        }

        static bool IsLoc(string l) { LatLon p; return l != null && (l.Length == 4 || l.Length == 6) && Astro.TryLocator(l.ToUpperInvariant(), out p); }

        // Regola dell'autore (IC8TEM): i locator non precisi si prendono da CALL3.
        // - locator mancante: quello di CALL3;
        // - locator a 4 caratteri (solo il quadrato, ~100 x 200 km): se CALL3 ha un locator a 6 caratteri si usa sempre
        //   quello (~5 x 2,5 km), anche se indica un altro quadrato (scelta dell'autore, 7/11/2026);
        // - locator a 6 caratteri: si tiene quello dello spot.
        public static string Refine(string loc, string call) { return CallDb.Refine(loc, call); }

        // locator mancanti o imprecisi dall'archivio CALL3 e distanza
        static void Locate(MsSpot s)
        {
            s.LocSpotter = Refine(s.LocSpotter, s.Spotter);
            s.LocDx = Refine(s.LocDx, s.DxCall);
            LatLon a, b;
            if (Astro.TryLocator(s.LocSpotter, out a) && Astro.TryLocator(s.LocDx, out b)) s.DistKm = Astro.DistanceKm(a, b);
        }

        static void Append(MsSpot s)
        {
            if (Dir == null) return;
            try
            {
                string f = Path.Combine(Dir, s.Utc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".csv");
                bool head = !File.Exists(f);
                StringBuilder sb = new StringBuilder();
                if (head) sb.AppendLine("utc;freq_khz;banda;dx;spotter;loc_dx;loc_spotter;km;filtro;fonte;commento");
                sb.Append(s.Utc.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)).Append(';')
                  .Append(s.FreqKHz.ToString("F1", CultureInfo.InvariantCulture)).Append(';').Append(s.Band).Append(';')
                  .Append(s.DxCall).Append(';').Append(s.Spotter).Append(';').Append(s.LocDx).Append(';').Append(s.LocSpotter).Append(';')
                  .Append(double.IsNaN(s.DistKm) ? "" : s.DistKm.ToString("F0", CultureInfo.InvariantCulture)).Append(';')
                  .Append(s.Match).Append(';').Append(SourceNames[s.Source]).Append(';')
                  .Append((s.Comment ?? "").Replace(';', ',')).AppendLine();
                File.AppendAllText(f, sb.ToString());
            }
            catch { }
        }

        // all'avvio: spot MS di ieri e di oggi dall'archivio
        static void LoadRecent()
        {
            if (Dir == null) return;
            DateTime now = DateTime.UtcNow;
            for (int d = 1; d >= 0; d--)
            {
                string f = Path.Combine(Dir, now.AddDays(-d).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".csv");
                if (!File.Exists(f)) continue;
                try
                {
                    foreach (string line in File.ReadAllLines(f))
                    {
                        string[] p = line.Split(';');
                        if (p.Length < 11 || p[0] == "utc") continue;
                        DateTime t;
                        if (!DateTime.TryParseExact(p[0], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out t)) continue;
                        MsSpot s = new MsSpot { Utc = DateTime.SpecifyKind(t, DateTimeKind.Utc) };
                        double fr; double.TryParse(p[1], NumberStyles.Float, CultureInfo.InvariantCulture, out fr); s.FreqKHz = fr;
                        s.DxCall = p[3]; s.Spotter = p[4]; s.LocDx = p[5]; s.LocSpotter = p[6];
                        double km; s.DistKm = double.TryParse(p[7], NumberStyles.Float, CultureInfo.InvariantCulture, out km) ? km : double.NaN;
                        s.Match = p[8]; s.Source = Math.Max(0, Array.IndexOf(SourceNames, p[9])); s.Comment = p[10];
                        if (Hidden(s)) continue;   // archivio scritto con filtri diversi (versione precedente)
                        if (keys.Add(s.Key)) Spots.Add(s);
                    }
                }
                catch { }
            }
        }
    }
}
