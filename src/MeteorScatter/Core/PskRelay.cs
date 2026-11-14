// YA-MS - rimando a PSKReporter delle decodifiche MS ricevute via UDP (regola dell'autore, IC8TEM, 14/11/2026).
// Metodo nascosto, acceso di partenza: se il programma digitale (WSJT-X, JTDX, MSHV) non sta mandando i propri spot a
// PSKReporter, le stazioni che decodifico in MSK144, FSK441, PSK2K o JTMS si mandano da YA-MS, col mio nominativo come
// ricevitore, cosi' la percezione del traffico MS sulla mappa e nel flusso MQTT si ricostruisce.
// Come si capisce che il programma non manda: la richiesta dei monitor attivi (PskActive, ogni 5 minuti, 28-440 MHz)
// elenca chi sta inviando rapporti. Se il mio nominativo c'e' con un programma che non e' YA-MS, l'invio funziona e
// YA-MS tace; se manca da piu' di 20 minuti (e la richiesta e' andata a buon fine), YA-MS rimanda.
// Protocollo: IPFIX su UDP verso report.pskreporter.info:4739 (pskreporter.info/pskdev.html), enterprise 30351.
// In piu' (richiesta dell'autore, 14/11/2026) si legge l'impostazione del programma digitale: in WSJT-X.ini (o JTDX.ini),
// sezione [Configuration], PSKReporter=false vuol dire che l'invio e' spento e YA-MS rimanda subito, senza aspettare i
// 20 minuti; con PSKReporter=true decide la lista di PSKReporter (l'impostazione accesa non dice che gli spot arrivano).
// Invio ogni 5 minuti, una stazione per banda al massimo ogni 5 minuti. Spegnimento: psk_rimando=0 in data/config.ini.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace YAMS
{
    public static class PskRelay
    {
        public const string Host = "report.pskreporter.info";
        public const int Port = 4739;
        public static bool Enabled = true;
        public static bool Active { get; private set; }   // in questo momento YA-MS sta rimandando
        public static int Sent, Packets;
        public static DateTime LastSendUtc;

        static readonly object lk = new object();
        static readonly List<UdpDecode> queue = new List<UdpDecode>();
        static readonly Dictionary<string, DateTime> lastByCall = new Dictionary<string, DateTime>();
        static Timer timer;
        static uint seq;
        static readonly uint domain = (uint)new Random().Next(1, int.MaxValue);
        static DateTime started = DateTime.UtcNow;

        public static void Start()
        {
            if (timer != null) return;
            started = DateTime.UtcNow;
            WsjtxUdp.Decoded += OnDecode;
            timer = new Timer(Tick, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        public static bool IsMsMode(string mode)
        {
            string m = (mode ?? "").ToUpperInvariant();
            foreach (string x in PskMqtt.MsModes) if (m.Contains(x)) return true;
            return false;
        }

        static void OnDecode(UdpDecode d)
        {
            if (!Enabled || d == null || d.Call.Length == 0 || !IsMsMode(d.Mode)) return;
            if (d.FreqKHz < 28000 || d.FreqKHz > 440000) return;   // stessa gamma della richiesta dei monitor
            if (d.Call == WsjtxUdp.DeCall) return;
            lock (lk)
            {
                string k = d.Call + "|" + MsBands.Name(d.FreqKHz);
                DateTime t;
                if (lastByCall.TryGetValue(k, out t) && (d.Time - t).TotalMinutes < 5) return;
                lastByCall[k] = d.Time;
                queue.Add(d);
                if (queue.Count > 500) queue.RemoveRange(0, queue.Count - 500);
            }
        }

        // impostazione di invio a PSKReporter del programma digitale: true, false o null (file non trovato)
        public static bool? IniPskReporter;
        public static string IniPath = "";
        static bool? ReadIni()
        {
            try
            {
                List<string> names = new List<string>();
                if (WsjtxUdp.ProgramId.Length > 0) names.Add(WsjtxUdp.ProgramId);
                names.Add("WSJT-X"); names.Add("JTDX");
                string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                foreach (string n in names)
                {
                    string b = n; int k = b.IndexOf(" - ", StringComparison.Ordinal); if (k > 0) b = b.Substring(0, k);
                    foreach (string f in new[] { Path.Combine(local, b, n + ".ini"), Path.Combine(home, ".config", n + ".ini"), Path.Combine(home, "Library", "Preferences", n + ".ini") })
                    {
                        if (!File.Exists(f)) continue;
                        bool inConf = false;
                        foreach (string raw in File.ReadLines(f))
                        {
                            string l = raw.Trim();
                            if (l.StartsWith("[", StringComparison.Ordinal)) { inConf = string.Equals(l, "[Configuration]", StringComparison.OrdinalIgnoreCase); continue; }
                            if (!inConf || !l.StartsWith("PSKReporter=", StringComparison.OrdinalIgnoreCase)) continue;
                            IniPath = f;
                            string v = l.Substring(12).Trim();
                            return string.Equals(v, "true", StringComparison.OrdinalIgnoreCase) || v == "1";
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        // il programma digitale sta gia' mandando i miei spot?
        static bool OwnReportsSeen()
        {
            IniPskReporter = ReadIni();
            if (IniPskReporter == false) return false;                                // invio spento nel programma digitale
            if (PskActive.LastOkUtc == DateTime.MinValue) return true;              // nessuna risposta ancora: prudenza, non si manda
            if (PskActive.SelfSeenUtc > DateTime.UtcNow.AddMinutes(-20)) return true;
            return (DateTime.UtcNow - started).TotalMinutes < 20;                   // dopo l'avvio si aspetta 20 minuti
        }

        static void Tick(object st)
        {
            try
            {
                List<UdpDecode> batch;
                lock (lk)
                {
                    batch = new List<UdpDecode>(queue); queue.Clear();
                    DateTime old = DateTime.UtcNow.AddMinutes(-30);
                    List<string> ks = new List<string>();
                    foreach (KeyValuePair<string, DateTime> kv in lastByCall) if (kv.Value < old) ks.Add(kv.Key);
                    foreach (string k in ks) lastByCall.Remove(k);
                }
                bool go = Enabled && WsjtxUdp.DeCall.Length > 0 && WsjtxUdp.DeGrid.Length >= 4 && !OwnReportsSeen();
                if (go != Active)
                {
                    Active = go;
                    WsjtxUdp.Note(go ? "PSKReporter: il programma digitale non manda i miei spot, YA-MS rimanda le decodifiche MS"
                                     : "PSKReporter: rimando delle decodifiche MS fermo");
                }
                if (!go || batch.Count == 0) return;
                using (UdpClient u = new UdpClient())
                {
                    int i = 0;
                    while (i < batch.Count)
                    {
                        List<UdpDecode> part = batch.GetRange(i, Math.Min(25, batch.Count - i));
                        byte[] pkt = Build(part);
                        u.Send(pkt, pkt.Length, Host, Port);
                        Packets++; Sent += part.Count; i += part.Count;
                    }
                }
                LastSendUtc = DateTime.UtcNow;
                WsjtxUdp.Note("PSKReporter: rimandate " + batch.Count.ToString(CultureInfo.InvariantCulture) + " decodifiche MS");
            }
            catch (Exception ex) { WsjtxUdp.Note("PSKReporter: invio non riuscito: " + ex.Message); }
        }

        // ---- pacchetto IPFIX ----
        static readonly byte[] RxTemplate =
        {
            0x00, 0x03, 0x00, 0x24, 0x99, 0x92, 0x00, 0x03, 0x00, 0x00,
            0x80, 0x02, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,   // receiverCallsign
            0x80, 0x04, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,   // receiverLocator
            0x80, 0x08, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,   // decodingSoftware
            0x00, 0x00
        };
        static readonly byte[] TxTemplate =
        {
            0x00, 0x02, 0x00, 0x3C, 0x99, 0x93, 0x00, 0x07,
            0x80, 0x01, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,   // senderCallsign
            0x80, 0x05, 0x00, 0x04, 0x00, 0x00, 0x76, 0x8F,   // frequency (Hz)
            0x80, 0x06, 0x00, 0x01, 0x00, 0x00, 0x76, 0x8F,   // sNR
            0x80, 0x0A, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,   // mode
            0x80, 0x03, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,   // senderLocator
            0x80, 0x0B, 0x00, 0x01, 0x00, 0x00, 0x76, 0x8F,   // informationSource
            0x00, 0x96, 0x00, 0x04                            // flowStartSeconds
        };

        static void Str(List<byte> b, string s)
        {
            byte[] x = Encoding.UTF8.GetBytes(s ?? "");
            int n = Math.Min(x.Length, 254);
            b.Add((byte)n);
            for (int i = 0; i < n; i++) b.Add(x[i]);
        }
        static void U16(List<byte> b, int v) { b.Add((byte)(v >> 8)); b.Add((byte)v); }
        static void U32(List<byte> b, uint v) { b.Add((byte)(v >> 24)); b.Add((byte)(v >> 16)); b.Add((byte)(v >> 8)); b.Add((byte)v); }
        static void Pad(List<byte> set) { while (set.Count % 4 != 0) set.Add(0); }

        static string Mode(string m)
        {
            string u = (m ?? "").ToUpperInvariant();
            foreach (string x in PskMqtt.MsModes) if (u.Contains(x)) return x;
            return u;
        }

        public static byte[] Build(List<UdpDecode> list)
        {
            // record del ricevitore (io)
            List<byte> rx = new List<byte>();
            U16(rx, 0x9992); U16(rx, 0);
            Str(rx, WsjtxUdp.DeCall); Str(rx, WsjtxUdp.DeGrid);
            Str(rx, (WsjtxUdp.ProgramId.Length > 0 ? WsjtxUdp.ProgramId + " via " : "") + "YA-MS " + AppPaths.Version);
            Pad(rx);
            rx[2] = (byte)(rx.Count >> 8); rx[3] = (byte)rx.Count;
            // record delle stazioni decodificate
            List<byte> tx = new List<byte>();
            U16(tx, 0x9993); U16(tx, 0);
            foreach (UdpDecode d in list)
            {
                Str(tx, d.Call);
                U32(tx, (uint)Math.Round(d.FreqKHz * 1000));
                tx.Add((byte)(sbyte)Math.Max(-128, Math.Min(127, d.Snr)));
                Str(tx, Mode(d.Mode));
                Str(tx, d.Grid);
                tx.Add(1);   // estratto automaticamente dalla decodifica
                U32(tx, (uint)new DateTimeOffset(d.Time).ToUnixTimeSeconds());
            }
            Pad(tx);
            tx[2] = (byte)(tx.Count >> 8); tx[3] = (byte)tx.Count;
            List<byte> p = new List<byte>();
            U16(p, 10); U16(p, 0);
            U32(p, (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            U32(p, seq);
            seq += (uint)(list.Count + 1);
            U32(p, domain);
            p.AddRange(RxTemplate); p.AddRange(TxTemplate); p.AddRange(rx); p.AddRange(tx);
            p[2] = (byte)(p.Count >> 8); p[3] = (byte)p.Count;
            return p.ToArray();
        }
    }
}
