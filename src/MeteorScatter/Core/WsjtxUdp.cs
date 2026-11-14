// YA-MS - messaggi UDP di WSJT-X / JTDX / MSHV, come in LIVETROPOVHF (IC8TEM): si ascolta la porta (2237 di partenza,
// anche multicast), ogni pacchetto si ripete identico sulla porta di inoltro (2238 di partenza) per gli altri programmi
// (GridTracker, JTAlert, log...), e dalle decodifiche si ricavano:
//  - la stazione che trasmette, sentita dalla propria stazione (nominativo e locatore dello Status di WSJT-X);
//  - i QSO chiusi tra due altre stazioni ("IK3FHP SV8PEX RR73", "... 73", "... RRR").
// Un solo ricevitore per tutto il programma (e per tutto E-LAYER ANALYZER, che ospita YA-MS): chi vuole gli spot si
// iscrive a SpotDecoded; la Raccolta dati di YA-MS li fa passare dagli stessi filtri degli altri spot (fonte 4).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YAMS
{
    // una stazione decodificata dal programma digitale
    public sealed class UdpDecode
    {
        public DateTime Time;
        public double FreqKHz;
        public string Mode = "";      // modo dello Status (FT8, MSK144, ...)
        public string TxMode = "";    // carattere di modo della decodifica (~ FT8, + FT4, & MSK144 ...)
        public int Snr;
        public double Dt;
        public uint Df;
        public string Message = "";
        public string Call = "";
        public string Grid = "";
    }

    public static class WsjtxUdp
    {
        public static string ListenAddress = "127.0.0.1";
        public static int ListenPort = 2237;
        public static string ForwardAddress = "127.0.0.1";
        public static int ForwardPort = 2238;
        public static bool Forward = true;
        public static bool AutoStart = true;

        public static bool Running { get; private set; }
        public static string LastError = "";
        public static string ProgramId = "", ProgramVersion = "";
        public static double DialHz;
        public static string DialMode = "";
        public static string DeCall = "", DeGrid = "";   // la propria stazione, dallo Status
        public static string DxCall = "";
        public static bool TxEnabled;
        public static DateTime StatusTime;
        public static long Packets, Forwarded, Decodes, Qsos;
        public static readonly Queue<string> Log = new Queue<string>();   // ultime 300 righe (per la console)

        public static event Action StateChanged;
        public static event Action<UdpDecode> Decoded;
        // uno spot pronto per le raccolte (YA-MS e, dentro E-LAYER ANALYZER, le sue linguette): fonte 4
        public static event Action<MsSpot> SpotDecoded;

        static UdpClient rx, tx;
        static CancellationTokenSource cts;
        static readonly object logLk = new object();

        public static void Load(Config cfg)
        {
            ListenAddress = cfg.Get("udp_in_addr", "127.0.0.1");
            int p; ListenPort = int.TryParse(cfg.Get("udp_in_port", "2237"), out p) && p > 0 && p < 65536 ? p : 2237;
            ForwardAddress = cfg.Get("udp_out_addr", "127.0.0.1");
            ForwardPort = int.TryParse(cfg.Get("udp_out_port", "2238"), out p) && p > 0 && p < 65536 ? p : 2238;
            Forward = cfg.GetB("udp_forward", true);
            AutoStart = cfg.GetB("udp_auto", true);
        }

        public static void Save(Config cfg)
        {
            cfg.Set("udp_in_addr", ListenAddress); cfg.Set("udp_in_port", ListenPort.ToString(CultureInfo.InvariantCulture));
            cfg.Set("udp_out_addr", ForwardAddress); cfg.Set("udp_out_port", ForwardPort.ToString(CultureInfo.InvariantCulture));
            cfg.Set("udp_forward", Forward); cfg.Set("udp_auto", AutoStart);
        }

        static void Say(string s)
        {
            lock (logLk) { Log.Enqueue(DateTime.UtcNow.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + "  " + s); while (Log.Count > 300) Log.Dequeue(); }
        }

        public static string LogText() { lock (logLk) return string.Join("\n", Log); }

        public static void Start()
        {
            Stop();
            try
            {
                IPAddress listenIp = IPAddress.Parse(ListenAddress.Trim());
                bool multicast = listenIp.AddressFamily == AddressFamily.InterNetwork && (listenIp.GetAddressBytes()[0] & 0xF0) == 0xE0;
                rx = new UdpClient(AddressFamily.InterNetwork);
                rx.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                rx.Client.Bind(new IPEndPoint(multicast ? IPAddress.Any : listenIp, ListenPort));
                if (multicast) rx.JoinMulticastGroup(listenIp);
                tx = new UdpClient(AddressFamily.InterNetwork);
                cts = new CancellationTokenSource();
                CancellationToken ct = cts.Token;
                Running = true; LastError = "";
                Say("in ascolto su " + ListenAddress + ":" + ListenPort.ToString(CultureInfo.InvariantCulture) + (Forward ? ", inoltro a " + ForwardAddress + ":" + ForwardPort.ToString(CultureInfo.InvariantCulture) : ""));
                Changed();
                Task.Run(() => Loop(ct));
            }
            catch (Exception ex)
            {
                LastError = ex.Message; Running = false;
                Say("errore: " + ex.Message);
                Changed();
            }
        }

        public static void Stop()
        {
            try { if (cts != null) cts.Cancel(); } catch { }
            try { if (rx != null) rx.Close(); } catch { }
            try { if (tx != null) tx.Close(); } catch { }
            rx = null; tx = null;
            if (Running) { Running = false; Say("fermato"); Changed(); }
        }

        static void Changed() { Action a = StateChanged; if (a != null) a(); }

        static async Task Loop(CancellationToken ct)
        {
            UdpClient r = rx, t = tx;
            while (!ct.IsCancellationRequested && r != null)
            {
                UdpReceiveResult res;
                try { res = await r.ReceiveAsync(ct); }
                catch { break; }
                Interlocked.Increment(ref Packets);
                byte[] data = res.Buffer;
                if (Forward && t != null)
                {
                    try { await t.SendAsync(data, data.Length, ForwardAddress.Trim(), ForwardPort); Interlocked.Increment(ref Forwarded); }
                    catch { }
                }
                try { Parse(data); }
                catch (Exception ex) { Say("pacchetto non leggibile: " + ex.Message); }
            }
            Running = false;
            Changed();
        }

        // ---- protocollo (QDataStream, big-endian)
        sealed class Reader
        {
            readonly byte[] b; int p;
            public Reader(byte[] b) { this.b = b; }
            public bool Eof { get { return p >= b.Length; } }
            public uint U32() { uint v = (uint)(b[p] << 24 | b[p + 1] << 16 | b[p + 2] << 8 | b[p + 3]); p += 4; return v; }
            public int I32() { return (int)U32(); }
            public ulong U64() { ulong hi = U32(); ulong lo = U32(); return hi << 32 | lo; }
            public bool Bool() { return b[p++] != 0; }
            public double F64()
            {
                byte[] t = new byte[8];
                Array.Copy(b, p, t, 0, 8); p += 8;
                if (BitConverter.IsLittleEndian) Array.Reverse(t);
                return BitConverter.ToDouble(t, 0);
            }
            public string Utf8()
            {
                uint n = U32();
                if (n == 0xFFFFFFFF) return "";
                string s = Encoding.UTF8.GetString(b, p, (int)n); p += (int)n; return s;
            }
        }

        static void Parse(byte[] data)
        {
            Reader r = new Reader(data);
            if (data.Length < 12 || r.U32() != 0xADBCCBDA) { Say("pacchetto sconosciuto (" + data.Length.ToString(CultureInfo.InvariantCulture) + " byte)"); return; }
            uint schema = r.U32();
            uint type = r.U32();
            string id = r.Utf8();
            switch (type)
            {
                case 0:   // Heartbeat
                {
                    r.U32();
                    string ver = r.Eof ? "" : r.Utf8();
                    ProgramId = id; ProgramVersion = ver;
                    Say("Heartbeat  " + id + "  " + ver);
                    Changed();
                    break;
                }
                case 1:   // Status
                {
                    ulong dial = r.U64();
                    string mode = r.Utf8();
                    string dxCall = r.Utf8();
                    r.Utf8(); r.Utf8();   // rapporto, modo TX
                    bool txEn = r.Bool(); r.Bool(); r.Bool();
                    r.U32(); r.U32();
                    string deCall = r.Utf8(); string deGrid = r.Utf8();
                    DialHz = dial; DialMode = mode; ProgramId = id;
                    DxCall = (dxCall ?? "").Trim().ToUpperInvariant(); TxEnabled = txEn; StatusTime = DateTime.UtcNow;
                    if (!string.IsNullOrWhiteSpace(deCall)) DeCall = deCall.Trim().ToUpperInvariant();
                    if (!string.IsNullOrWhiteSpace(deGrid)) DeGrid = deGrid.Trim().ToUpperInvariant();
                    Changed();
                    break;
                }
                case 2:   // Decode
                {
                    r.Bool();
                    r.U32();
                    int snr = r.I32();
                    double dt = r.F64();
                    uint df = r.U32();
                    string txMode = r.Utf8();
                    string msg = r.Utf8();
                    UdpDecode d = new UdpDecode
                    {
                        Time = DateTime.UtcNow, Mode = DialMode, TxMode = txMode, Snr = snr, Dt = dt, Df = df, Message = msg,
                        FreqKHz = DialHz > 0 ? (DialHz + df) / 1000.0 : 0
                    };
                    ExtractStation(msg, d);
                    Interlocked.Increment(ref Decodes);
                    Say(string.Format(CultureInfo.InvariantCulture, "{0,3} {1,4:0.0} {2,5} {3,-7} {4}", snr, dt, df, DialMode, msg));
                    if (d.Call.Length > 0) { Action<UdpDecode> a = Decoded; if (a != null) a(d); }
                    MakeSpots(d);
                    break;
                }
                case 5: Say("QSO registrato  " + id); break;
                case 6: Say("chiuso  " + id); break;
            }
        }

        // decodifica -> spot per le raccolte
        static void MakeSpots(UdpDecode d)
        {
            Action<MsSpot> a = SpotDecoded;
            if (a == null || d.FreqKHz <= 0) return;
            string me = DeCall;
            string mode = (d.Mode ?? "").Trim().ToUpperInvariant();
            string tail = mode + " " + d.Snr.ToString("+0;-0;0", CultureInfo.InvariantCulture) + " dB  " + d.Message + "  (WSJT-X UDP)";
            string time = d.Time.ToString("HHmm", CultureInfo.InvariantCulture) + "Z";
            // 1) la stazione che trasmette, sentita dalla propria
            if (d.Call.Length > 0 && me.Length > 0 && d.Call != me)
                a(new MsSpot
                {
                    Utc = d.Time, FreqKHz = d.FreqKHz, DxCall = d.Call, Spotter = me, LocDx = d.Grid, LocSpotter = DeGrid,
                    Mode = mode, Comment = tail, Source = 4,
                    Line = "UDP de " + me + ": " + d.FreqKHz.ToString("F1", CultureInfo.InvariantCulture) + "  " + d.Call + "  " + tail + "  " + time
                });
            // 2) QSO chiuso tra due altre stazioni: la tratta vale per tutte e due
            string qa, qb;
            if (TryQso(d.Message, out qa, out qb) && qa != me && qb != me)
            {
                Interlocked.Increment(ref Qsos);
                a(new MsSpot
                {
                    Utc = d.Time, FreqKHz = d.FreqKHz, DxCall = qb, Spotter = qa, Mode = mode, Comment = "QSO " + tail, Source = 4,
                    Line = "UDP QSO: " + d.FreqKHz.ToString("F1", CultureInfo.InvariantCulture) + "  " + qa + " <> " + qb + "  " + tail + "  " + time
                });
            }
        }

        static readonly HashSet<string> CqWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "DX", "EU", "TEST", "NA", "SA", "AS", "AF", "OC", "QRP", "POTA", "SOTA", "FD", "RU", "WW" };

        static bool IsCall(string c) { return c.Length >= 3 && c.Any(char.IsDigit) && c.Any(char.IsLetter) && c != "RR73" && !c.StartsWith("CQ", StringComparison.Ordinal); }

        // messaggio di chiusura di un QSO: "IC8TEM IK3FHP RR73", "IK3FKP SV8PEX 73" o "... RRR"
        public static bool TryQso(string msg, out string a, out string b)
        {
            a = b = "";
            string[] t = (msg ?? "").Trim().ToUpperInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (t.Length != 3 || (t[2] != "RR73" && t[2] != "73" && t[2] != "RRR")) return false;
            string x = t[0].Trim('<', '>'), y = t[1].Trim('<', '>');
            if (!IsCall(x) || !IsCall(y) || x == y) return false;
            a = x; b = y;
            return true;
        }

        // stazione che trasmette e suo locatore dal testo del messaggio
        public static void ExtractStation(string msg, UdpDecode d)
        {
            string[] t = (msg ?? "").Trim().ToUpperInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (t.Length == 0) return;
            int i;
            if (t[0] == "CQ" || t[0] == "QRZ" || t[0].StartsWith("CQ_", StringComparison.Ordinal))
            {
                i = 1;
                if (i < t.Length && (CqWords.Contains(t[i]) || (t[i].All(char.IsDigit) && t[i].Length <= 3))) i++;
            }
            else i = 1;
            if (i >= t.Length) return;
            string call = t[i].Trim('<', '>');
            if (call.Length < 3 || !call.Any(char.IsDigit) || !call.Any(char.IsLetter) || call == "RR73" || call == "73") return;
            d.Call = call;
            if (i + 1 < t.Length)
            {
                string g = t[i + 1];
                if (g.Length == 4 && g != "RR73" && CallDb.ValidLoc(g)) d.Grid = g;
            }
        }
    }
}
