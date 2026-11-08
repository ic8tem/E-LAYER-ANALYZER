// E-LAYER ANALYZER - allineatore dell'orologio (SNTP, RFC 4330), elemento nascosto.
// Ogni 5 minuti si interrogano 2 server di un elenco di 10 sparsi nel mondo; se uno non risponde si passa al successivo
// dell'elenco (fino a provarli tutti). Con almeno una risposta lo stato e' "NTP:OK", altrimenti "NTP:FAILED".
// Lo scarto rispetto al PC si applica all'orologio del programma (Ntp.UtcNow); se lo scarto supera 1 s e il programma
// ha i diritti per farlo (Windows, amministratore), si corregge anche l'orologio del PC.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ELA
{
    public static class Ntp
    {
        // 10 server pubblici in continenti diversi
        public static readonly string[] Servers =
        {
            "ntp1.inrim.it",        // Italia, INRIM (tempo legale italiano)
            "ptbtime1.ptb.de",      // Germania, PTB
            "time.nist.gov",        // Stati Uniti, NIST
            "time.nrc.ca",          // Canada, NRC
            "ntp.nict.jp",          // Giappone, NICT
            "a.st1.ntp.br",         // Brasile, NIC.br
            "au.pool.ntp.org",      // Australia, pool NTP
            "za.pool.ntp.org",      // Sudafrica, pool NTP
            "time.google.com",      // Google (anycast mondiale)
            "time.cloudflare.com",  // Cloudflare (anycast mondiale)
        };

        public static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);
        public static bool? Ok;                       // null = nessuna prova ancora
        public static double OffsetSec;               // ora del server meno ora del PC
        public static string LastServers = "";
        public static DateTime LastCheckUtc = DateTime.MinValue;
        public static bool SystemClockSet;
        public static event Action Changed;
        static int next;                              // primo server da interrogare al prossimo giro
        static bool busy;

        public static DateTime UtcNow { get { return DateTime.UtcNow.AddSeconds(Ok == true ? OffsetSec : 0); } }

        public static async Task CheckAsync()
        {
            if (busy) return;
            busy = true;
            try
            {
                List<double> offs = new List<double>();
                List<string> used = new List<string>();
                int tried = 0;
                // due server per giro; se uno manca la risposta si prova il successivo, fino a tutti e dieci
                while (offs.Count < 2 && tried < Servers.Length)
                {
                    string host = Servers[next % Servers.Length];
                    tried++;
                    double? o = await Query(host);
                    if (o.HasValue) { offs.Add(o.Value); used.Add(host); }
                    next = (next + 1) % Servers.Length;
                }
                LastCheckUtc = DateTime.UtcNow;
                if (offs.Count > 0)
                {
                    OffsetSec = offs.Average();
                    LastServers = string.Join(", ", used);
                    Ok = true;
                    if (Math.Abs(OffsetSec) > 1.0) SystemClockSet = TrySetSystemClock(DateTime.UtcNow.AddSeconds(OffsetSec));
                    if (SystemClockSet && Math.Abs(OffsetSec) > 1.0) OffsetSec = 0;
                }
                else { Ok = false; LastServers = ""; }
            }
            catch { Ok = false; }
            finally { busy = false; }
            Action a = Changed; if (a != null) a();
        }

        // una richiesta SNTP: restituisce lo scarto in secondi o null
        static async Task<double?> Query(string host)
        {
            try
            {
                IPAddress[] addrs = await Dns.GetHostAddressesAsync(host);
                IPAddress ip = addrs.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork) ?? addrs.FirstOrDefault();
                if (ip == null) return null;
                using (UdpClient udp = new UdpClient(ip.AddressFamily))
                {
                    byte[] req = new byte[48];
                    req[0] = 0x1B;   // LI 0, versione 3, modo client
                    udp.Connect(ip, 123);
                    DateTime t1 = DateTime.UtcNow;
                    await udp.SendAsync(req, req.Length);
                    Task<UdpReceiveResult> rx = udp.ReceiveAsync();
                    if (await Task.WhenAny(rx, Task.Delay(3000)) != rx) return null;
                    DateTime t4 = DateTime.UtcNow;
                    byte[] r = rx.Result.Buffer;
                    if (r.Length < 48 || (r[0] & 0x07) != 4 || r[1] == 0) return null;   // modo server, stratum valido
                    DateTime t2 = Stamp(r, 32), t3 = Stamp(r, 40);
                    return ((t2 - t1).TotalSeconds + (t3 - t4).TotalSeconds) / 2.0;
                }
            }
            catch { return null; }
        }

        static DateTime Stamp(byte[] b, int i)
        {
            ulong sec = ((ulong)b[i] << 24) | ((ulong)b[i + 1] << 16) | ((ulong)b[i + 2] << 8) | b[i + 3];
            ulong frac = ((ulong)b[i + 4] << 24) | ((ulong)b[i + 5] << 16) | ((ulong)b[i + 6] << 8) | b[i + 7];
            return new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(sec + frac / 4294967296.0);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SYSTEMTIME { public ushort Year, Month, DayOfWeek, Day, Hour, Minute, Second, Milliseconds; }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetSystemTime(ref SYSTEMTIME st);

        // corregge l'orologio del PC solo su Windows e solo con i diritti di amministratore; altrimenti si usa lo scarto
        static bool TrySetSystemClock(DateTime utc)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return false;
            try
            {
                SYSTEMTIME st = new SYSTEMTIME
                {
                    Year = (ushort)utc.Year, Month = (ushort)utc.Month, DayOfWeek = (ushort)utc.DayOfWeek, Day = (ushort)utc.Day,
                    Hour = (ushort)utc.Hour, Minute = (ushort)utc.Minute, Second = (ushort)utc.Second, Milliseconds = (ushort)utc.Millisecond
                };
                return SetSystemTime(ref st);
            }
            catch { return false; }
        }
    }
}
