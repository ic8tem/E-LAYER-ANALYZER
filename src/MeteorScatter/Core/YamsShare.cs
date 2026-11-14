// YA-MS - scambio tra gli utenti di YA-MS dei messaggi indirizzati a noi (richiesta dell'autore, IC8TEM, 14/11/2026).
// In meteor scatter capita che un altro operatore senta il ping del mio corrispondente che mi chiama o mi risponde e io
// no; PSKReporter riceve solo chi trasmette, non il testo. Qui ogni YA-MS che decodifica via UDP un messaggio MS tra due
// stazioni (es. "IC8TEM 9A2AE R+03") lo manda, cifrato, al destinatario (IC8TEM); ogni YA-MS riceve solo i messaggi
// rivolti al proprio nominativo e li disegna nel Traffico in diretta (tratteggio viola che lampeggia sempre).
//  - Canale: MQTT 3.1.1 sui tre broker pubblici usati da IP-DESK-CTRL (broker.hivemq.com, test.mosquitto.org,
//    broker.emqx.io, porta 1883): si pubblica e si riceve su tutti e tre, i doppioni si scartano.
//  - Cifratura (richiesta dell'autore): ogni YA-MS ha una coppia di chiavi ECDH P-256 (data/yams_scambio.key) e pubblica
//    la chiave pubblica, conservata dal broker (retain), su yams/v1/k/<hash del nominativo>. Chi manda un messaggio
//    crea una chiave temporanea, calcola il segreto comune con la chiave pubblica del destinatario e cifra con
//    AES-256-CBC + HMAC-SHA256: solo il destinatario puo' leggere. Gli argomenti usano l'hash del nominativo, mai il
//    nominativo in chiaro.
//  - Limiti: chiunque potrebbe pubblicare una chiave falsa per un nominativo (senza un'autorita' che le certifichi);
//    i broker pubblici non garantiscono il servizio. I messaggi non si conservano sul server.
//  - Modi MS (MSK144, FSK441, PSK2K, JTMS) e FT8 solo oltre i 1100 km; chi riceve un FT8 lo mostra solo se non ha tratte
//    parallele nei 3 minuti (stessa clausola degli FT8 isolati). Lo stesso messaggio parte al massimo una volta al minuto.
//  - Acceso di partenza; spegnimento: yams_scambio=0 in data/config.ini.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace YAMS
{
    // client MQTT 3.1.1 minimo con pubblicazione (QoS 0, retain) e abbonamenti, riconnessione automatica
    public sealed class MqttLink
    {
        public readonly string Host; public readonly int Port;
        public bool Connected { get; private set; }
        public event Action<string, byte[]> Message;
        public event Action Up;
        readonly HashSet<string> subs = new HashSet<string>(StringComparer.Ordinal);
        readonly object wlk = new object();
        NetworkStream ns; TcpClient tc;
        volatile bool want;
        int pid = 1;

        public MqttLink(string host, int port) { Host = host; Port = port; }

        public void Start() { if (want) return; want = true; Task.Run(Loop); }
        public void Stop() { want = false; try { tc?.Close(); } catch { } }

        public void Subscribe(string topic)
        {
            lock (subs) if (!subs.Add(topic)) return;
            if (Connected) Send(SubPacket(topic));
        }

        public void Publish(string topic, byte[] payload, bool retain)
        {
            if (!Connected) return;
            List<byte> b = new List<byte>(); b.AddRange(Str(topic)); b.AddRange(payload);
            Send(Packet((byte)(0x30 | (retain ? 1 : 0)), b));
        }

        void Send(byte[] p)
        {
            try { lock (wlk) { if (ns != null) { ns.Write(p, 0, p.Length); ns.Flush(); } } } catch { }
        }

        byte[] SubPacket(string topic)
        {
            int id = Interlocked.Increment(ref pid) & 0x7FFF; if (id == 0) id = 1;
            List<byte> b = new List<byte> { (byte)(id >> 8), (byte)id };
            b.AddRange(Str(topic)); b.Add(0);
            return Packet(0x82, b);
        }

        async Task Loop()
        {
            int wait = 5;
            while (want)
            {
                try
                {
                    tc = new TcpClient();
                    using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(20)))
                        await tc.ConnectAsync(Host, Port, cts.Token);
                    ns = tc.GetStream();
                    List<byte> c = new List<byte>();
                    c.AddRange(Str("MQTT")); c.Add(4); c.Add(0x02); c.Add(0); c.Add(60);
                    c.AddRange(Str("YA-MS-" + Guid.NewGuid().ToString("N").Substring(0, 12)));
                    Send(Packet(0x10, c));
                    int type; byte[] body = Read(out type);
                    if (type != 2 || body.Length < 2 || body[1] != 0) throw new IOException("refused");
                    Connected = true; wait = 5;
                    List<string> all; lock (subs) all = new List<string>(subs);
                    foreach (string s in all) Send(SubPacket(s));
                    Up?.Invoke();
                    DateTime ping = DateTime.UtcNow;
                    while (want)
                    {
                        if ((DateTime.UtcNow - ping).TotalSeconds > 40) { Send(new byte[] { 0xC0, 0 }); ping = DateTime.UtcNow; }
                        if (!ns.DataAvailable) { await Task.Delay(100); continue; }
                        body = Read(out type);
                        if (type == 3 && body.Length >= 2)
                        {
                            int tl = body[0] << 8 | body[1];
                            if (2 + tl > body.Length) continue;
                            string topic = Encoding.UTF8.GetString(body, 2, tl);
                            byte[] pl = new byte[body.Length - 2 - tl];
                            Buffer.BlockCopy(body, 2 + tl, pl, 0, pl.Length);
                            try { Message?.Invoke(topic, pl); } catch { }
                        }
                    }
                }
                catch { }
                finally { Connected = false; try { tc?.Close(); } catch { } lock (wlk) ns = null; }
                if (!want) break;
                try { await Task.Delay(wait * 1000); } catch { }
                wait = Math.Min(300, wait * 2);
            }
        }

        static byte[] Str(string s) { byte[] b = Encoding.UTF8.GetBytes(s); byte[] r = new byte[b.Length + 2]; r[0] = (byte)(b.Length >> 8); r[1] = (byte)b.Length; Buffer.BlockCopy(b, 0, r, 2, b.Length); return r; }

        static byte[] Packet(byte head, List<byte> body)
        {
            List<byte> p = new List<byte> { head };
            int len = body.Count;
            do { byte d = (byte)(len % 128); len /= 128; if (len > 0) d |= 0x80; p.Add(d); } while (len > 0);
            p.AddRange(body);
            return p.ToArray();
        }

        void ReadExact(byte[] buf, int n)
        {
            int got = 0;
            while (got < n) { int r = ns.Read(buf, got, n - got); if (r <= 0) throw new IOException("closed"); got += r; }
        }

        byte[] Read(out int type)
        {
            byte[] one = new byte[1];
            ReadExact(one, 1);
            type = one[0] >> 4;
            int len = 0, mul = 1;
            for (int i = 0; i < 4; i++) { ReadExact(one, 1); len += (one[0] & 0x7F) * mul; mul *= 128; if ((one[0] & 0x80) == 0) break; }
            if (len > 1 << 20) throw new IOException("packet too large");
            byte[] body = new byte[len];
            if (len > 0) ReadExact(body, len);
            return body;
        }
    }

    public static class YamsShare
    {
        public static readonly string[] Brokers = { "broker.hivemq.com", "test.mosquitto.org", "broker.emqx.io" };
        public const int BrokerPort = 1883;
        public static bool Enabled = true;
        public static int Sent, Received, Keys;
        public static event Action<MsSpot> SpotReceived;   // messaggio rivolto a me, sentito da un altro utente di YA-MS

        static readonly List<MqttLink> links = new List<MqttLink>();
        static ECDiffieHellman me;
        static byte[] myPub;
        static string myCall = "", dataDir = "";
        static readonly ConcurrentDictionary<string, byte[]> peerKeys = new ConcurrentDictionary<string, byte[]>(StringComparer.Ordinal);
        static readonly ConcurrentDictionary<string, DateTime> sentRecently = new ConcurrentDictionary<string, DateTime>(StringComparer.Ordinal);
        static readonly ConcurrentDictionary<string, DateTime> seen = new ConcurrentDictionary<string, DateTime>(StringComparer.Ordinal);
        static readonly ConcurrentQueue<KeyValuePair<string, byte[]>> waiting = new ConcurrentQueue<KeyValuePair<string, byte[]>>();
        static Timer timer;

        public static int ConnectedBrokers { get { int n = 0; lock (links) foreach (MqttLink l in links) if (l.Connected) n++; return n; } }

        public static string Hash(string call)
        {
            byte[] h = SHA256.HashData(Encoding.UTF8.GetBytes("yams-v1|" + (call ?? "").Trim().ToUpperInvariant()));
            return Convert.ToHexString(h, 0, 10).ToLowerInvariant();
        }

        public static void Start(string dir)
        {
            if (!Enabled || timer != null) return;
            dataDir = dir;
            LoadKey();
            foreach (string b in Brokers)
            {
                MqttLink l = new MqttLink(b, BrokerPort);
                l.Message += OnMessage;
                l.Up += () => PublishMyKey(l);
                lock (links) links.Add(l);
                l.Start();
            }
            WsjtxUdp.Decoded += OnDecode;
            timer = new Timer(_ => Tick(), null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }

        public static void Stop()
        {
            Timer t = timer; timer = null; t?.Dispose();
            WsjtxUdp.Decoded -= OnDecode;
            lock (links) { foreach (MqttLink l in links) l.Stop(); links.Clear(); }
        }

        static void LoadKey()
        {
            string f = Path.Combine(dataDir, "yams_scambio.key");
            me = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
            try
            {
                if (File.Exists(f)) me.ImportPkcs8PrivateKey(File.ReadAllBytes(f), out _);
                else File.WriteAllBytes(f, me.ExportPkcs8PrivateKey());
            }
            catch { }
            myPub = me.ExportSubjectPublicKeyInfo();
        }

        // ogni 10 s: il mio nominativo (da WSJT-X) puo' cambiare; messaggi in attesa della chiave del destinatario
        static void Tick()
        {
            string c = WsjtxUdp.DeCall ?? "";
            if (c.Length > 0 && c != myCall)
            {
                myCall = c;
                lock (links) foreach (MqttLink l in links) { l.Subscribe("yams/v1/a/" + Hash(myCall)); PublishMyKey(l); }
            }
            int n = waiting.Count;
            for (int i = 0; i < n && waiting.TryDequeue(out KeyValuePair<string, byte[]> w); i++)
            {
                byte[] key; DateTime t0 = DateTime.FromBinary(BitConverter.ToInt64(w.Value, 0));
                if (peerKeys.TryGetValue(w.Key, out key)) { byte[] plain = new byte[w.Value.Length - 8]; Buffer.BlockCopy(w.Value, 8, plain, 0, plain.Length); SendTo(w.Key, key, plain); }
                else if ((DateTime.UtcNow - t0).TotalMinutes < 3) waiting.Enqueue(w);
            }
            DateTime old = DateTime.UtcNow.AddMinutes(-10);
            foreach (var kv in sentRecently) if (kv.Value < old) sentRecently.TryRemove(kv.Key, out _);
            foreach (var kv in seen) if (kv.Value < old) seen.TryRemove(kv.Key, out _);
        }

        static void PublishMyKey(MqttLink l)
        {
            if (myCall.Length == 0 || myPub == null) return;
            l.Subscribe("yams/v1/a/" + Hash(myCall));
            l.Publish("yams/v1/k/" + Hash(myCall), Encoding.ASCII.GetBytes(Convert.ToBase64String(myPub)), true);
        }

        static readonly Regex rxCall = new Regex(@"^<?([A-Z0-9]{1,4}/)?[A-Z0-9]{1,3}[0-9][A-Z0-9]{0,4}[A-Z](/[A-Z0-9]{1,4})?>?$", RegexOptions.Compiled);

        // decodifica UDP: se e' un messaggio MS tra due stazioni, lo mando cifrato al destinatario
        static void OnDecode(UdpDecode d)
        {
            if (!Enabled || d == null || d.Call.Length == 0 || myCall.Length == 0) return;
            bool ft8 = string.Equals((d.Mode ?? "").Trim(), "FT8", StringComparison.OrdinalIgnoreCase);
            if (!PskRelay.IsMsMode(d.Mode) && !ft8) return;
            if (ft8 && !LongFt8(d)) return;   // FT8 solo oltre i 1100 km (le tratte parallele le scarta chi riceve)
            string[] w = (d.Message ?? "").Trim().ToUpperInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (w.Length < 2 || w[0] == "CQ" || w[0] == "QRZ" || w[0] == "DE") return;
            string to = w[0].Trim('<', '>');
            if (!rxCall.IsMatch(w[0]) || to == myCall || to == d.Call) return;
            string dedupe = to + "|" + d.Message;
            if (sentRecently.TryGetValue(dedupe, out DateTime last) && (DateTime.UtcNow - last).TotalSeconds < 60) return;
            sentRecently[dedupe] = DateTime.UtcNow;
            string json = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                { "rx", myCall }, { "rl", WsjtxUdp.DeGrid ?? "" }, { "tx", d.Call }, { "tl", d.Grid ?? "" },
                { "f", Math.Round(d.FreqKHz * 1000) }, { "md", d.Mode ?? "" }, { "s", d.Snr },
                { "t", new DateTimeOffset(d.Time).ToUnixTimeSeconds() }, { "msg", d.Message ?? "" }, { "sw", "YA-MS " + AppPaths.Version }
            });
            byte[] plain = Encoding.UTF8.GetBytes(json);
            byte[] key;
            if (peerKeys.TryGetValue(to, out key)) { SendTo(to, key, plain); return; }
            // chiave del destinatario non ancora nota: la chiedo (messaggio conservato dal broker) e il messaggio aspetta
            lock (links) foreach (MqttLink l in links) l.Subscribe("yams/v1/k/" + Hash(to));
            keyTopicToCall[Hash(to)] = to;
            byte[] withTime = new byte[plain.Length + 8];
            Buffer.BlockCopy(BitConverter.GetBytes(DateTime.UtcNow.ToBinary()), 0, withTime, 0, 8);
            Buffer.BlockCopy(plain, 0, withTime, 8, plain.Length);
            waiting.Enqueue(new KeyValuePair<string, byte[]>(to, withTime));
        }

        // FT8: distanza tra chi trasmette e me oltre i 1100 km (locator dal messaggio o dal database nominativi)
        static bool LongFt8(UdpDecode d)
        {
            try
            {
                string tl = CallDb.Refine(d.Grid ?? "", d.Call);
                LatLon a, b;
                if (!Astro.TryLocator(tl, out a) || !Astro.TryLocator(WsjtxUdp.DeGrid ?? "", out b)) return false;
                return Astro.DistanceKm(a, b) > MsCollector.Ft8MinKm;
            }
            catch { return false; }
        }

        static readonly ConcurrentDictionary<string, string> keyTopicToCall = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

        static void SendTo(string to, byte[] peerPub, byte[] plain)
        {
            try
            {
                byte[] payload = Encrypt(peerPub, plain);
                lock (links) foreach (MqttLink l in links) l.Publish("yams/v1/a/" + Hash(to), payload, false);
                Interlocked.Increment(ref Sent);
            }
            catch { }
        }

        static void OnMessage(string topic, byte[] payload)
        {
            try
            {
                if (topic.StartsWith("yams/v1/k/", StringComparison.Ordinal))
                {
                    string h = topic.Substring(10), call;
                    if (keyTopicToCall.TryGetValue(h, out call) && payload.Length > 0)
                    {
                        byte[] k = Convert.FromBase64String(Encoding.ASCII.GetString(payload).Trim());
                        using (ECDiffieHellman t = ECDiffieHellman.Create()) t.ImportSubjectPublicKeyInfo(k, out _);   // chiave valida?
                        if (peerKeys.TryAdd(call, k)) Interlocked.Increment(ref Keys); else peerKeys[call] = k;
                    }
                    return;
                }
                if (myCall.Length == 0 || topic != "yams/v1/a/" + Hash(myCall)) return;
                string id = Convert.ToHexString(SHA256.HashData(payload), 0, 12);
                if (!seen.TryAdd(id, DateTime.UtcNow)) return;   // stesso messaggio da un altro broker
                byte[] plain = Decrypt(payload);
                if (plain == null) return;
                using (JsonDocument doc = JsonDocument.Parse(plain))
                {
                    JsonElement r = doc.RootElement;
                    string S(string k) { JsonElement v; return r.TryGetProperty(k, out v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : ""; }
                    double N(string k) { JsonElement v; return r.TryGetProperty(k, out v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : double.NaN; }
                    string rx = S("rx").ToUpperInvariant(), tx = S("tx").ToUpperInvariant(), msg = S("msg"), md = S("md").ToUpperInvariant();
                    double f = N("f"), t = N("t"), snr = N("s");
                    if (rx.Length == 0 || tx.Length == 0 || !(f > 0)) return;
                    DateTime utc = !double.IsNaN(t) ? DateTimeOffset.FromUnixTimeSeconds((long)t).UtcDateTime : DateTime.UtcNow;
                    if (utc > DateTime.UtcNow.AddMinutes(5) || utc < DateTime.UtcNow.AddHours(-1)) return;
                    string snrTxt = double.IsNaN(snr) ? "" : " " + snr.ToString("+0;-0;0", CultureInfo.InvariantCulture) + " dB";
                    MsSpot s = new MsSpot
                    {
                        Utc = utc, FreqKHz = f / 1000.0, Spotter = rx, LocSpotter = S("rl"), DxCall = tx, LocDx = S("tl"), Mode = md,
                        Source = 4, Shared = true,
                        Comment = md + snrTxt + "  " + msg + "  (YA-MS: " + rx + ")",
                        Line = "YA-MS " + rx + ": " + (f / 1000.0).ToString("F1", CultureInfo.InvariantCulture) + "  " + tx + "  " + msg
                    };
                    Interlocked.Increment(ref Received);
                    WsjtxUdp.Note("YA-MS: " + rx + " ha sentito " + tx + " (" + msg + ")" + snrTxt);
                    SpotReceived?.Invoke(s);
                }
            }
            catch { }
        }

        // ---- cifratura: ECDH P-256 effimero + AES-256-CBC + HMAC-SHA256
        static void Keys2(byte[] secret, out byte[] enc, out byte[] mac)
        {
            using (HMACSHA256 h = new HMACSHA256(secret))
            {
                enc = h.ComputeHash(Encoding.ASCII.GetBytes("yams-v1-enc"));
                mac = h.ComputeHash(Encoding.ASCII.GetBytes("yams-v1-mac"));
            }
        }

        static byte[] Encrypt(byte[] peerPub, byte[] plain)
        {
            using (ECDiffieHellman peer = ECDiffieHellman.Create())
            using (ECDiffieHellman eph = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256))
            {
                peer.ImportSubjectPublicKeyInfo(peerPub, out _);
                byte[] secret = eph.DeriveKeyFromHash(peer.PublicKey, HashAlgorithmName.SHA256);
                Keys2(secret, out byte[] ek, out byte[] mk);
                using (Aes a = Aes.Create())
                {
                    a.Key = ek; a.GenerateIV();
                    byte[] ct = a.EncryptCbc(plain, a.IV, PaddingMode.PKCS7);
                    byte[] macData = new byte[a.IV.Length + ct.Length];
                    Buffer.BlockCopy(a.IV, 0, macData, 0, a.IV.Length); Buffer.BlockCopy(ct, 0, macData, a.IV.Length, ct.Length);
                    byte[] tag; using (HMACSHA256 h = new HMACSHA256(mk)) tag = h.ComputeHash(macData);
                    string json = JsonSerializer.Serialize(new Dictionary<string, object>
                    {
                        { "v", 1 }, { "e", Convert.ToBase64String(eph.ExportSubjectPublicKeyInfo()) },
                        { "i", Convert.ToBase64String(a.IV) }, { "c", Convert.ToBase64String(ct) }, { "m", Convert.ToBase64String(tag) }
                    });
                    return Encoding.UTF8.GetBytes(json);
                }
            }
        }

        static byte[] Decrypt(byte[] payload)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(payload))
                {
                    JsonElement r = doc.RootElement;
                    byte[] e = Convert.FromBase64String(r.GetProperty("e").GetString());
                    byte[] iv = Convert.FromBase64String(r.GetProperty("i").GetString());
                    byte[] ct = Convert.FromBase64String(r.GetProperty("c").GetString());
                    byte[] tag = Convert.FromBase64String(r.GetProperty("m").GetString());
                    using (ECDiffieHellman eph = ECDiffieHellman.Create())
                    {
                        eph.ImportSubjectPublicKeyInfo(e, out _);
                        byte[] secret = me.DeriveKeyFromHash(eph.PublicKey, HashAlgorithmName.SHA256);
                        Keys2(secret, out byte[] ek, out byte[] mk);
                        byte[] macData = new byte[iv.Length + ct.Length];
                        Buffer.BlockCopy(iv, 0, macData, 0, iv.Length); Buffer.BlockCopy(ct, 0, macData, iv.Length, ct.Length);
                        byte[] want; using (HMACSHA256 h = new HMACSHA256(mk)) want = h.ComputeHash(macData);
                        if (!CryptographicOperations.FixedTimeEquals(want, tag)) return null;
                        using (Aes a = Aes.Create()) { a.Key = ek; return a.DecryptCbc(ct, iv, PaddingMode.PKCS7); }
                    }
                }
            }
            catch { return null; }
        }
    }
}
