// E-LAYER ANALYZER - APRS-IS in sola lettura per l'E-sporadico in 144 MHz (dal 22/11/2026, richiesta dell'autore: «un altro
// motivo di alert in 144 è la ricorrenza a breve tempo di spot in APRS: già li tracci per LIVETROPOVHF»). Il lettore dei
// pacchetti e' quello di LIVETROPOVHF (Core/Aprs.cs, stesso autore), con le stesse cautele: solo salti radio fra stazioni di
// posizione nota (sorgente -> digipeater -> IGate), niente terze parti, niente digipeater sconosciuti o ritrasmissioni nascoste,
// niente palloni e aerei. Qui contano solo i salti in 2 m da 900 km in su (regola dell'autore per l'Es in 144 MHz).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ELA
{
    public struct AprsPos
    {
        public readonly double Lat, Lon;
        public AprsPos(double lat, double lon) { Lat = lat; Lon = lon; }
    }

    public sealed class AprsIs
    {
        public string Host = "rotate.aprs2.net";
        public int Port = 14580;
        public string Call = "N0CALL";
        public string Filter = "a/72/-25/30/45";     // in E-LAYER ANALYZER: r/lat/lon/3000 attorno alla stazione
        public double MinKm = 300, MaxKm = 1500;
        public bool Connected { get; private set; }
        public long Packets, Hops;
        public string LastError = "";
        public event Action<string> Log;
        /// <summary>Salto radio: (ora, banda «144», «432» o «1296» per gli 868 MHz, chiamata A, posizione A, chiamata B, posizione B).</summary>
        public event Action<DateTime, string, string, AprsPos, string, AprsPos> Hop;

        /// <summary>Salti scartati per possibile ritrasmissione nascosta (percorso consumato, digipeater a metà strada, terze parti).</summary>
        public long Hidden;
        sealed class Sta
        {
            public AprsPos Pos; public DateTime T; public string Band = "144"; public bool Explicit; public string Why = "";
            public Dictionary<string, int> MaxN; public int Seen; public DateTime DigiT = DateTime.MinValue;
        }
        readonly Dictionary<string, Sta> _sta = new(StringComparer.OrdinalIgnoreCase);
        CancellationTokenSource _cts;

        static readonly string[] LoraTocalls = { "APEP", "APJMF2", "APLAB0", "APLDAG", "APLDG", "APLDH", "APLDI", "APLDL1", "APLDM", "APLER", "APLEST", "APLESW", "APLFG", "APLFL", "APLG", "APLHB9", "APLHI", "APLHM", "APLHT1", "APLHT2", "APLIF", "APLJRA", "APLJRT", "APLLT", "APLO", "APLP1", "APLPIK", "APLPS", "APLRF", "APLRG", "APLRT", "APLT", "APLU0", "APLU1", "APLULD", "APLULT", "APLVGL", "APLXY1", "APLZA", "APLZX", "APRFGB", "APRFGI", "APRFGL", "APRFGT", "APRFGW", "APSFLG", "APSFTL" };
        static readonly string[] Aliases = { "WIDE", "RELAY", "TRACE", "TEMP", "ECHO", "GATE", "NOGATE", "RFONLY", "TCPIP", "TCPXX", "CQ", "SGATE", "LOCAL" };

        public void Start()
        {
            Stop();
            _cts = new CancellationTokenSource();
            var ct = _cts.Token;
            Task.Run(() => Loop(ct));
        }

        public void Stop() { try { _cts?.Cancel(); } catch { } _cts = null; Connected = false; }

        async Task Loop(CancellationToken ct)
        {
            int wait = 10;
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    using var tcp = new TcpClient();
                    await tcp.ConnectAsync(Host, Port);
                    using var ns = tcp.GetStream();
                    var rd = new StreamReader(ns, Encoding.GetEncoding("ISO-8859-1"));
                    var login = Encoding.ASCII.GetBytes($"user {(string.IsNullOrWhiteSpace(Call) ? "N0CALL" : Call.ToUpperInvariant())} pass -1 vers E-LAYER-ANALYZER 0.6 filter {Filter}\r\n");
                    await ns.WriteAsync(login, 0, login.Length);
                    Connected = true; wait = 10;
                    Log?.Invoke($"APRS-IS: collegato a {Host}:{Port} in sola lettura, filtro {Filter}");
                    while (!ct.IsCancellationRequested)
                    {
                        var line = await rd.ReadLineAsync();
                        if (line == null) break;
                        if (line.Length == 0 || line[0] == '#') continue;
                        Packets++;
                        try { Handle(line, DateTime.UtcNow); } catch { }
                    }
                }
                catch (Exception ex) { LastError = ex.Message; Log?.Invoke("APRS-IS: " + ex.Message); }
                Connected = false;
                if (ct.IsCancellationRequested) break;
                try { await Task.Delay(TimeSpan.FromSeconds(wait), ct); } catch { break; }
                wait = Math.Min(300, wait * 2);
            }
        }

        /// <summary>Una riga APRS-IS: SRC&gt;DEST,PATH:INFO. Pubblico per le prove.</summary>
        public void Handle(string line, DateTime now)
        {
            int gt = line.IndexOf('>'), colon = line.IndexOf(':');
            if (gt <= 0 || colon <= gt) return;
            string src = line.Substring(0, gt).Trim();
            var hdr = line.Substring(gt + 1, colon - gt - 1).Split(',');
            string dest = hdr[0];
            string info = line.Substring(colon + 1);
            // terze parti: pacchetto rigirato da internet in radio da un IGate, non è un salto radio della sorgente
            if (info.StartsWith("}", StringComparison.Ordinal)) return;
            // posizione e caratteristiche della sorgente
            Update(src, dest, info, now);
            if (hdr.Length < 2) return;
            // costrutto q: solo pacchetti ricevuti via radio da un IGate
            int qi = Array.FindIndex(hdr, h => h.StartsWith("qA", StringComparison.Ordinal));
            if (qi < 0 || qi + 1 >= hdr.Length) return;
            string q = hdr[qi];
            if (q != "qAR" && q != "qAo" && q != "qAO") return;
            string igate = hdr[qi + 1];
            // ultimo elemento usato del percorso (asterisco): tutti quelli prima sono usati
            int lastUsed = -1;
            for (int i = 1; i < qi; i++) if (hdr[i].EndsWith("*")) lastUsed = i;
            // ritrasmissione nascosta: confronta il percorso non ancora usato con quello che la sorgente manda di solito
            bool known, silent = false;
            lock (_sta)
            {
                known = _sta.TryGetValue(src, out var ss) && ss.Seen >= 2;
                if (ss != null)
                {
                    ss.MaxN ??= new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 1; i < qi; i++)
                    {
                        if (hdr[i].EndsWith("*") || i <= lastUsed) continue;
                        if (!PathReq(hdr[i], out var bas, out int n)) continue;
                        if (ss.MaxN.TryGetValue(bas, out int mx) && n < mx) silent = true;
                        if (!ss.MaxN.TryGetValue(bas, out mx) || n > mx) ss.MaxN[bas] = n;
                    }
                }
                // digipeater che tracciano: servono per il controllo «a metà strada»
                for (int i = 1; i <= lastUsed; i++)
                {
                    var nm = hdr[i].TrimEnd('*');
                    if (!IsAlias(nm) && _sta.TryGetValue(nm, out var dg)) dg.DigiT = now;
                }
            }
            if (silent || !known) { if (silent) Hidden++; return; }
            var chain = new List<string> { src };
            bool broken = false;
            for (int i = 1; i <= lastUsed; i++)
            {
                var name = hdr[i].TrimEnd('*');
                if (IsAlias(name)) { broken = true; continue; }   // digipeater sconosciuto: il salto non si può attribuire
                if (broken) { chain.Clear(); broken = false; }    // la catena riparte dal primo nome noto dopo il buco
                chain.Add(name);
            }
            if (!broken) chain.Add(igate);
            for (int i = 1; i < chain.Count; i++) TryHop(chain[i - 1], chain[i], now);
        }

        static bool IsAlias(string n)
        {
            var u = n.ToUpperInvariant();
            foreach (var a in Aliases) if (u.StartsWith(a, StringComparison.Ordinal)) return true;
            // alias numerici come IT2-2, I1-1 (alias regionali): lettere + una cifra + "-n"
            return u.Length <= 6 && u.Contains("-") && char.IsDigit(u[u.IndexOf('-') - 1]) && u.IndexOf('-') <= 3;
        }

        /// <summary>Richiesta di percorso tipo WIDE2-1, IT2-2: base «WIDE2» e salti rimasti n (WIDE2 senza SSID = 0).</summary>
        static bool PathReq(string e, out string bas, out int n)
        {
            bas = ""; n = 0;
            int d = e.IndexOf('-');
            string b = d < 0 ? e : e.Substring(0, d);
            if (b.Length < 2 || !char.IsDigit(b[b.Length - 1]) || b[b.Length - 1] == '0') return false;
            if (!IsAlias(e) && !IsAlias(b + "-1")) return false;
            if (d >= 0 && !int.TryParse(e.Substring(d + 1), NumberStyles.Integer, CultureInfo.InvariantCulture, out n)) return false;
            bas = b.ToUpperInvariant(); return true;
        }

        void TryHop(string a, string b, DateTime now)
        {
            Sta sa, sb; string band; bool mid = false;
            lock (_sta)
            {
                if (!_sta.TryGetValue(a, out sa) || !_sta.TryGetValue(b, out sb)) return;
                if (sa.Band == "x" || sb.Band == "x") return;
                if (sa.Explicit && sb.Explicit && sa.Band != sb.Band) return;
                band = sa.Explicit ? sa.Band : sb.Explicit ? sb.Band : "144";
                if ((now - sa.T).TotalHours > 24 || (now - sb.T).TotalHours > 24) return;
                double km0 = Geo.DistanceKm(sa.Pos.Lat, sa.Pos.Lon, sb.Pos.Lat, sb.Pos.Lon);
                if (km0 < MinKm || km0 > MaxKm) return;
                // un digipeater attivo (ultime 3 ore) della stessa banda a metà strada può aver ritrasmesso senza tracciare
                foreach (var kv in _sta)
                {
                    var d = kv.Value;
                    if (ReferenceEquals(d, sa) || ReferenceEquals(d, sb) || (now - d.DigiT).TotalHours > 3) continue;
                    if (d.Explicit && d.Band != band) continue;
                    double x = Geo.DistanceKm(sa.Pos.Lat, sa.Pos.Lon, d.Pos.Lat, d.Pos.Lon), y = Geo.DistanceKm(d.Pos.Lat, d.Pos.Lon, sb.Pos.Lat, sb.Pos.Lon);
                    if (x <= 0.8 * km0 && y <= 0.8 * km0 && x + y <= 1.25 * km0) { mid = true; break; }
                }
            }
            if (mid) { Hidden++; return; }
            Hops++;
            Hop?.Invoke(now, band == "868" ? "1296" : band, a.ToUpperInvariant(), sa.Pos, b.ToUpperInvariant(), sb.Pos);
        }

        void Update(string call, string dest, string info, DateTime now)
        {
            if (info.Length < 2) return;
            char ty = info[0];
            AprsPos? pos = null; string comment = "";
            try
            {
                if (ty == '`' || ty == '\'') { pos = MicE(dest, info); comment = info.Length > 9 ? info.Substring(9) : ""; }
                else if (ty == '!' || ty == '=' || ty == '/' || ty == '@')
                {
                    int o = (ty == '/' || ty == '@') ? 8 : 1;
                    if (info.Length > o + 12)
                    {
                        char c0 = info[o];
                        if (c0 == '/' || c0 == '\\' || (c0 >= 'A' && c0 <= 'Z') || (c0 >= 'a' && c0 <= 'j'))
                        {
                            pos = Compressed(info.Substring(o + 1, 8)); comment = info.Length > o + 13 ? info.Substring(o + 13) : "";
                        }
                        else { pos = Plain(info.Substring(o)); comment = info.Length > o + 19 ? info.Substring(o + 19) : ""; }
                    }
                }
            }
            catch { pos = null; }
            lock (_sta)
            {
                if (!_sta.TryGetValue(call, out var s)) { if (pos == null) return; s = new Sta(); _sta[call] = s; }
                if (pos != null) { s.Pos = pos.Value; s.T = now; }
                s.Seen++;
                var (band, expl, why) = Classify(dest, comment);
                if (expl || !s.Explicit) { s.Band = band; s.Explicit = expl; s.Why = why; }
                // aeroplani e palloni: quota oltre 1500 m (5000 piedi)
                int ai = comment.IndexOf("/A=", StringComparison.Ordinal);
                if (ai >= 0 && comment.Length >= ai + 9 && int.TryParse(comment.Substring(ai + 3, 6), NumberStyles.Integer, CultureInfo.InvariantCulture, out int ft) && ft > 5000)
                    s.T = DateTime.MinValue;   // posizione non usabile per i salti
                if (_sta.Count > 60000) { var old = _sta.Where(kv => (now - kv.Value.T).TotalHours > 24).Select(kv => kv.Key).ToList(); foreach (var k in old) _sta.Remove(k); }
            }
        }

        /// <summary>Banda della stazione: «144» (di default, non dichiarata), «432» (LoRa 433 MHz), «868» (LoRa 868 MHz)
        /// o «x» (HF o frequenze che non servono). explicit = dichiarata dal tocall o dal commento.</summary>
        public static (string band, bool expl, string why) Classify(string tocall, string comment)
        {
            var tc = (tocall ?? "").ToUpperInvariant();
            var c = (comment ?? "").ToUpperInvariant();
            bool lora = c.Contains("LORA");
            foreach (var p in LoraTocalls) if (tc.StartsWith(p, StringComparison.Ordinal)) { lora = true; break; }
            if (c.Contains("300BD") || c.Contains("300 BD") || c.Contains("300 BAUD") || c.Contains(" HF ") || c.StartsWith("HF ")) return ("x", true, "HF");
            // frequenze scritte nel commento (433.775, 868.000, 144.800…)
            foreach (System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(c, @"\b(\d{2,4})[.,](\d{1,4})\s?(MHZ)?\b"))
            {
                if (!double.TryParse(m.Groups[1].Value + "." + m.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double f)) continue;
                if (f >= 863 && f <= 870) return ("868", true, $"{f:0.###} MHz");
                if (f >= 430 && f <= 440) return ("432", true, $"{f:0.###} MHz");
                if (f >= 144 && f <= 146) return ("144", true, $"{f:0.###} MHz");
                if (m.Groups[3].Success) return ("x", true, $"{f:0.###} MHz");
            }
            if (c.Contains("868") || c.Contains("869")) return ("868", true, "868 MHz");
            if (lora) return ("432", true, tc.StartsWith("AP", StringComparison.Ordinal) && !c.Contains("LORA") ? "LoRa (tocall " + tc + ")" : "LoRa");
            return ("144", false, "");
        }

        static AprsPos? Plain(string s)
        {
            // DDMM.mmN/DDDMM.mmE (il carattere fra le due è la tabella dei simboli)
            if (s.Length < 19) return null;
            double lat = double.Parse(s.Substring(0, 2), CultureInfo.InvariantCulture) + double.Parse(s.Substring(2, 5).Replace(' ', '0'), CultureInfo.InvariantCulture) / 60;
            if (s[7] == 'S') lat = -lat; else if (s[7] != 'N') return null;
            double lon = double.Parse(s.Substring(9, 3), CultureInfo.InvariantCulture) + double.Parse(s.Substring(12, 5).Replace(' ', '0'), CultureInfo.InvariantCulture) / 60;
            if (s[17] == 'W') lon = -lon; else if (s[17] != 'E') return null;
            if (Math.Abs(lat) > 90 || Math.Abs(lon) > 180 || (lat == 0 && lon == 0)) return null;
            return new AprsPos(lat, lon);
        }

        static AprsPos? Compressed(string s)
        {
            if (s.Length < 8) return null;
            double B(string x) { double v = 0; foreach (var ch in x) { if (ch < 33 || ch > 124) throw new FormatException(); v = v * 91 + (ch - 33); } return v; }
            double lat = 90 - B(s.Substring(0, 4)) / 380926.0, lon = -180 + B(s.Substring(4, 4)) / 190463.0;
            if (Math.Abs(lat) > 90 || Math.Abs(lon) > 180) return null;
            return new AprsPos(lat, lon);
        }

        static AprsPos? MicE(string dest, string info)
        {
            var d = dest.Split('-')[0];
            if (d.Length < 6 || info.Length < 4) return null;
            int Dig(char c) => c >= '0' && c <= '9' ? c - '0' : c >= 'A' && c <= 'J' ? c - 'A' : c >= 'P' && c <= 'Y' ? c - 'P' : 0;
            double lat = Dig(d[0]) * 10 + Dig(d[1]) + (Dig(d[2]) * 10 + Dig(d[3]) + (Dig(d[4]) * 10 + Dig(d[5])) / 100.0) / 60;
            bool north = d[3] >= 'P' && d[3] <= 'Z', off = d[4] >= 'P' && d[4] <= 'Z', west = d[5] >= 'P' && d[5] <= 'Z';
            if (!north) lat = -lat;
            int deg = info[1] - 28; if (off) deg += 100;
            if (deg >= 180 && deg <= 189) deg -= 80; else if (deg >= 190 && deg <= 199) deg -= 190;
            int min = info[2] - 28; if (min >= 60) min -= 60;
            int hun = info[3] - 28;
            double lon = deg + (min + hun / 100.0) / 60;
            if (west) lon = -lon;
            if (Math.Abs(lat) > 90 || Math.Abs(lon) > 180) return null;
            return new AprsPos(lat, lon);
        }
    }
}
