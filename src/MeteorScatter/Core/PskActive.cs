// YA-MS - stazioni in ascolto che compaiono sulla mappa di PSKReporter ("active monitors"), anche quelle che non
// hanno ancora ricevuto nulla. Lo stream MQTT manda solo gli spot: chi ascolta senza ricevere non c'e'. La mappa del
// sito usa l'interfaccia di consultazione retrieve.pskreporter.info/query, che oltre ai rapporti restituisce gli
// elementi <activeReceiver callsign=".." locator=".." frequency=".." mode=".." decoderSoftware=".." .../>.
// Regole del servizio (pskreporter.info/pskdev.html): non piu' di una richiesta ogni 5 minuti, appcontact con il nome
// del programma (mai l'indirizzo dell'utente). Qui: una sola richiesta ogni 5 minuti per tutti i modi (aggiornamento a
// 5 minuti voluto dall'autore, 14/11/2026), solo da 28 a 440 MHz (frange), ultimi 15 minuti; rptlimit=1 perche' i
// rapporti arrivano gia' da MQTT. Dei monitor si tengono quelli in MSK144, FSK441, PSK2K e JTMS.
// I monitor trovati entrano in MsCollector.Listeners (triangoli del Traffico in diretta) con Count = 0.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Xml;

namespace YAMS
{
    public class PskMonitor
    {
        public string Call = "", Loc = "", Band = "", Mode = "", Soft = "";
        public DateTime SeenUtc;
    }

    public static class PskActive
    {
        public const string Url = "https://retrieve.pskreporter.info/query";
        public static bool Enabled = true;
        public static int Monitors, Requests, Errors;
        public static DateTime LastOkUtc;
        public static DateTime SelfSeenUtc;   // il mio nominativo tra chi manda rapporti, con un programma che non e' YA-MS (PskRelay)
        public static string LastError = "";

        static readonly object lk = new object();
        static readonly List<PskMonitor> pending = new List<PskMonitor>();
        static Timer timer;
        static int busy;

        public static void Start()
        {
            if (timer != null) return;
            timer = new Timer(Tick, null, TimeSpan.FromSeconds(20), TimeSpan.FromMinutes(5));
        }

        public static void Stop()
        {
            Timer t = timer; timer = null;
            if (t != null) t.Dispose();
        }

        // monitor arrivati dall'ultima chiamata (thread dell'interfaccia, da MsCollector.Process)
        public static List<PskMonitor> Take()
        {
            lock (lk)
            {
                List<PskMonitor> r = new List<PskMonitor>(pending);
                pending.Clear();
                return r;
            }
        }

        static void Tick(object st)
        {
            if (!(Enabled || PskRelay.Enabled) || Interlocked.Exchange(ref busy, 1) == 1) return;
            try { Fetch(); }
            finally { busy = 0; }
        }

        static void Fetch()
        {
            try
            {
                string url = Url + "?flowStartSeconds=-900&rptlimit=1&frange=28000000-440000000&appcontact=" +
                             Uri.EscapeDataString("YA-MS " + AppPaths.Version + " (IC8TEM)");
                string xml;
                using (HttpClient h = new HttpClient { Timeout = TimeSpan.FromSeconds(90) })
                {
                    h.DefaultRequestHeaders.UserAgent.ParseAdd("YA-MS/" + AppPaths.Version);
                    xml = h.GetStringAsync(url).GetAwaiter().GetResult();
                }
                Requests++;
                List<PskMonitor> got = Parse(xml, "", DateTime.UtcNow);
                lock (lk)
                {
                    pending.AddRange(got);
                    if (pending.Count > 20000) pending.RemoveRange(0, pending.Count - 20000);
                }
                LastOkUtc = DateTime.UtcNow; LastError = "";
            }
            catch (Exception ex) { Errors++; LastError = ex.Message; }
        }

        public static List<PskMonitor> Parse(string xml, string mode, DateTime now)
        {
            List<PskMonitor> r = new List<PskMonitor>();
            XmlReaderSettings xs = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = null };
            using (XmlReader x = XmlReader.Create(new StringReader(xml), xs))
            {
                while (x.Read())
                {
                    if (x.NodeType != XmlNodeType.Element || !string.Equals(x.LocalName, "activeReceiver", StringComparison.OrdinalIgnoreCase)) continue;
                    string call = (x.GetAttribute("callsign") ?? "").Trim().ToUpperInvariant();
                    string loc = (x.GetAttribute("locator") ?? "").Trim();
                    string md = (x.GetAttribute("mode") ?? mode ?? "").Trim().ToUpperInvariant();
                    double hz;
                    if (call.Length > 0 && call == WsjtxUdp.DeCall && (x.GetAttribute("decoderSoftware") ?? "").IndexOf("YA-MS", StringComparison.OrdinalIgnoreCase) < 0)
                        SelfSeenUtc = now;
                    if (call.Length == 0 || !double.TryParse(x.GetAttribute("frequency") ?? "", NumberStyles.Float, CultureInfo.InvariantCulture, out hz)) continue;
                    if (Array.IndexOf(PskMqtt.MsModes, md) < 0) continue;   // solo i modi MS
                    string band = MsBands.Name(hz / 1000.0);
                    if (band.Length == 0) continue;
                    r.Add(new PskMonitor { Call = call, Loc = loc, Band = band, Mode = md, Soft = (x.GetAttribute("decoderSoftware") ?? "").Trim(), SeenUtc = now });
                }
            }
            Monitors = r.Count;
            return r;
        }
    }
}
