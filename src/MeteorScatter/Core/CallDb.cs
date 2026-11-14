// YA-MS - archivio nominativo -> locator (CALL3.TXT) con le stesse regole di LIVETROPOVHF (IC8TEM):
//  - base: data/call3.txt (formato CALL3 di WSJT-X/JTDX/MSHV: "CALL,LOCATOR,...");
//  - aggiornamento da MMMonVHF (www.mmmonvhf.de): "Aggiorna adesso" o da solo all'avvio ogni 7 giorni; il file
//    scaricato si accetta solo con almeno 100.000 righe valide, il precedente resta in call3.bak;
//  - strati sopra il file, tutti nella cartella data e mai toccati dall'aggiornamento:
//      voci a mano (CALL3-MANUALE.TXT)  >  correzioni dal cluster (CALL3-CLUSTER.TXT)  >
//      file aggiunti (CALL3-AGGIUNTE/*.txt|csv, in ordine alfabetico: l'ultimo vince)  >  call3.txt  >
//      locatori a 6 caratteri imparati dal vivo (LOC6.TXT)  >  locatore ricevuto;
//  - correzioni dal cluster: negli spot con due locatori ("KN09TP<>KN09") il primo e' quello che la stazione che
//    spotta dichiara di se': se ha 6 caratteri (non LL/MM) e differisce dal database, lo corregge; non confermate
//    per due anni si lasciano cadere;
//  - LOC6: ogni locatore a 6 caratteri ricevuto (cluster, PSKReporter) per un nominativo che il database non ha o ha
//    solo a 4 caratteri si ricorda e vale per gli spot dello stesso nominativo nello stesso quadrato; due anni;
//  - ultima fonte G7VRD (api.g7vrd.co.uk, "locator by callsign"): solo per i nominativi senza locatore a 6 caratteri
//    in tutte le fonti precedenti; si accetta solo un 6 caratteri valido (non LL/MM) nello stesso quadrato di quello
//    ricevuto; le ricerche si fanno in automatico ogni 6 ore (richiesta dell'autore, 14/11/2026), in gruppi di 40
//    nominativi, ogni nominativo al massimo una volta al giorno; risultati in data/CALL3-G7VRD.TXT (tenuti un anno);
//    si spegne con la casella (call3_g7vrd=0);
//  - le correzioni (voci a mano, cluster, file aggiunti, G7VRD, LOC6) si scrivono anche dentro data/call3.txt
//    (richiesta dell'autore, 14/11/2026), cosi' le vedono anche gli altri programmi che usano lo stesso file; gli
//    strati restano come fonte, perche' l'aggiornamento da MMMonVHF riscrive call3.txt e dopo le si riapplica;
//    nessuna correzione e' scritta nel codice del programma: quelle di LIVETROPOVHF sono gia' in data/call3.txt;
//  - regola unica: un locatore ricevuto di meno di 6 caratteri, o a 6 che finisce con LL/MM (centro del quadrato),
//    si sostituisce con quello a 6 caratteri del database; per i portatili (con la barra: IC8TEM/P, F/IC8TEM)
//    vale solo una voce esatta.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace YAMS
{
    public static class CallDb
    {
        static string[] calls = new string[0], locs = new string[0];
        static readonly object lk = new object();
        static readonly Dictionary<string, string> manual = new Dictionary<string, string>(StringComparer.Ordinal);
        static readonly Dictionary<string, string> overlay = new Dictionary<string, string>(StringComparer.Ordinal);
        static readonly Dictionary<string, KeyValuePair<string, DateTime>> cluster = new Dictionary<string, KeyValuePair<string, DateTime>>(StringComparer.Ordinal);
        static readonly Dictionary<string, KeyValuePair<string, DateTime>> learned = new Dictionary<string, KeyValuePair<string, DateTime>>(StringComparer.Ordinal);
        static int clusterDirty, learnedDirty;

        public static volatile bool Loading;
        public static int Count { get { return calls.Length; } }
        public static int ManualCount { get { lock (lk) return manual.Count; } }
        public static int OverlayCount { get { lock (lk) return overlay.Count; } }
        public static int ClusterCount { get { lock (lk) return cluster.Count; } }
        public static int LearnedCount { get { lock (lk) return learned.Count; } }
        public static List<string> OverlayFiles = new List<string>();
        public static string DataDir = "";
        public static string FilePath = "";
        public static DateTime FileDate;
        public static string LastError = "";
        public static event Action Loaded;

        public const string MmmonvhfUrl = "https://www.mmmonvhf.de/dl.php?dl=call3.txt";
        public const int AutoDays = 7;
        public static string ManualPath { get { return Path.Combine(DataDir, "CALL3-MANUALE.TXT"); } }
        public static string OverlayDir { get { return Path.Combine(DataDir, "CALL3-AGGIUNTE"); } }
        public static string ClusterPath { get { return Path.Combine(DataDir, "CALL3-CLUSTER.TXT"); } }
        public static string LearnedPath { get { return Path.Combine(DataDir, "LOC6.TXT"); } }
        public static string G7Path { get { return Path.Combine(DataDir, "CALL3-G7VRD.TXT"); } }

        // ---- G7VRD
        public const string G7Api = "https://api.g7vrd.co.uk/v1/locators/by-callsign/";
        public static volatile bool G7Enabled = true;
        public static string G7Status = "";
        static readonly ConcurrentDictionary<string, KeyValuePair<string, DateTime>> g7 = new ConcurrentDictionary<string, KeyValuePair<string, DateTime>>(StringComparer.Ordinal);
        static readonly ConcurrentDictionary<string, DateTime> g7Asked = new ConcurrentDictionary<string, DateTime>(StringComparer.Ordinal);
        static readonly ConcurrentQueue<string> g7Queue = new ConcurrentQueue<string>();
        static int g7Dirty, g7Busy;
        static Timer g7Timer;
        public static int G7Count { get { return g7.Count; } }

        static string G7Get(string call) { KeyValuePair<string, DateTime> v; return call != null && g7.TryGetValue(call, out v) ? v.Key : ""; }

        static void G7Request(string call)
        {
            if (!G7Enabled || string.IsNullOrEmpty(call) || call.Length < 3 || call.IndexOf('/') >= 0 || call.IndexOf('-') >= 0) return;
            DateTime now = DateTime.UtcNow, t;
            if (g7Asked.TryGetValue(call, out t) && now - t < TimeSpan.FromHours(24)) return;
            g7Asked[call] = now;
            g7Queue.Enqueue(call);
        }

        static void G7Load()
        {
            try
            {
                if (!File.Exists(G7Path)) return;
                foreach (string line in File.ReadLines(G7Path))
                {
                    if (line.StartsWith("//", StringComparison.Ordinal)) continue;
                    string[] p = line.Split(',');
                    if (p.Length < 2 || !Precise(p[1].Trim().ToUpperInvariant())) continue;
                    DateTime w;
                    if (p.Length < 3 || !DateTime.TryParse(p[2], CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out w)) w = DateTime.UtcNow;
                    if (DateTime.UtcNow - w > TimeSpan.FromDays(365)) continue;
                    g7[p[0].Trim().ToUpperInvariant()] = new KeyValuePair<string, DateTime>(p[1].Trim().ToUpperInvariant(), w);
                }
            }
            catch { }
        }

        static void G7Save()
        {
            if (Interlocked.Exchange(ref g7Dirty, 0) == 0 || DataDir.Length == 0) return;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("// YA-MS - locatori a 6 caratteri da api.g7vrd.co.uk (dati raccolti in automatico, ultima fonte). CALL,LOCATORE,data");
                List<string> k = new List<string>(g7.Keys); k.Sort(StringComparer.Ordinal);
                foreach (string c in k) sb.Append(c).Append(',').Append(g7[c].Key).Append(',').Append(g7[c].Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).AppendLine();
                File.WriteAllText(G7Path + ".tmp", sb.ToString());
                File.Move(G7Path + ".tmp", G7Path, true);
            }
            catch { }
        }

        // ogni 6 ore, in sottofondo: tutti i nominativi in attesa, in gruppi di 40 con una pausa tra un gruppo e l'altro
        static async void G7Tick(object state)
        {
            if (Interlocked.Exchange(ref g7Busy, 1) == 1) return;
            int found = 0, asked = 0;
            try
            {
                while (G7Enabled && !g7Queue.IsEmpty)
                {
                    int n = await G7Batch();
                    if (n < 0) break;
                    found += n; asked++;
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
                if (found > 0) G7Save();
                WriteCorrectionsToCall3();   // ogni 6 ore: correzioni nuove (cluster, LOC6, G7VRD) dentro call3.txt
            }
            catch (Exception ex) { G7Status = "G7VRD: " + ex.Message; }
            finally { Interlocked.Exchange(ref g7Busy, 0); }
        }

        // un gruppo di al massimo 40 nominativi; restituisce quanti locatori ha trovato, -1 se la rete non risponde
        static async Task<int> G7Batch()
        {
            try
            {
                List<string> batch = new List<string>();
                string c;
                while (batch.Count < 40 && g7Queue.TryDequeue(out c)) batch.Add(c);
                if (batch.Count == 0) return 0;
                List<string> esc = new List<string>(); foreach (string b in batch) esc.Add(Uri.EscapeDataString(b));
                using (HttpClient h = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
                {
                    h.DefaultRequestHeaders.UserAgent.ParseAdd("YA-MS/" + AppPaths.Version);
                    using (HttpResponseMessage r = await h.GetAsync(G7Api + string.Join(",", esc)))
                    {
                        if (!r.IsSuccessStatusCode) { G7Status = "G7VRD: HTTP " + ((int)r.StatusCode).ToString(CultureInfo.InvariantCulture); return -1; }
                        using (JsonDocument doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync()))
                        {
                            int n = 0;
                            if (doc.RootElement.ValueKind == JsonValueKind.Object)
                                foreach (JsonProperty prop in doc.RootElement.EnumerateObject())
                                {
                                    string loc = G7Pick(prop.Value);
                                    if (loc.Length == 0) continue;
                                    g7[prop.Name.ToUpperInvariant()] = new KeyValuePair<string, DateTime>(loc, DateTime.UtcNow);
                                    n++;
                                }
                            if (n > 0) { Interlocked.Exchange(ref g7Dirty, 1); call3Pending = 1; }
                            G7Status = "G7VRD: " + batch.Count.ToString(CultureInfo.InvariantCulture) + " / " + n.ToString(CultureInfo.InvariantCulture);
                            return n;
                        }
                    }
                }
            }
            catch (Exception ex) { G7Status = "G7VRD: " + ex.Message; return -1; }
        }

        // il valore puo' essere una stringa, un elenco o un oggetto: il primo locatore a 6 caratteri utile
        static string G7Pick(JsonElement v)
        {
            switch (v.ValueKind)
            {
                case JsonValueKind.String:
                    string s = (v.GetString() ?? "").Trim().ToUpperInvariant();
                    return Precise(s) ? s : "";
                case JsonValueKind.Array:
                    foreach (JsonElement e in v.EnumerateArray()) { string r = G7Pick(e); if (r.Length > 0) return r; }
                    return "";
                case JsonValueKind.Object:
                    foreach (string name in new[] { "locator", "grid", "maidenhead", "value" })
                    {
                        JsonElement x;
                        if (v.TryGetProperty(name, out x)) { string r = G7Pick(x); if (r.Length > 0) return r; }
                    }
                    foreach (JsonProperty p in v.EnumerateObject()) { string r = G7Pick(p.Value); if (r.Length > 0) return r; }
                    return "";
                default: return "";
            }
        }


        public static bool ValidLoc(string l)
        {
            if (l == null || (l.Length != 4 && l.Length != 6)) return false;
            l = l.ToUpperInvariant();
            if (l[0] < 'A' || l[0] > 'R' || l[1] < 'A' || l[1] > 'R' || !char.IsDigit(l[2]) || !char.IsDigit(l[3])) return false;
            return l.Length == 4 || (l[4] >= 'A' && l[4] <= 'X' && l[5] >= 'A' && l[5] <= 'X');
        }

        // locatore a 6 caratteri "vero" (non il centro del quadrato LL/MM che molti sistemi aggiungono da soli)
        static bool Precise(string l) { return l != null && l.Length == 6 && ValidLoc(l) && !l.EndsWith("LL", StringComparison.Ordinal) && !l.EndsWith("MM", StringComparison.Ordinal); }

        public static void StartLoad(string dataDir)
        {
            DataDir = dataDir;
            FilePath = Path.Combine(dataDir, "call3.txt");
            if (g7Timer == null) { G7Load(); g7Timer = new Timer(G7Tick, null, TimeSpan.FromMinutes(2), TimeSpan.FromHours(6)); }
            if (Loading) return;
            Loading = true;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try { Load(); LoadLayers(); LastError = ""; } catch (Exception ex) { LastError = ex.Message; }
                Loading = false;
                Action a = Loaded; if (a != null) a();
            });
        }

        static void Load()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            Dictionary<string, string> pool = new Dictionary<string, string>(StringComparer.Ordinal);
            if (File.Exists(FilePath))
            {
                FileDate = File.GetLastWriteTime(FilePath);
                using (StreamReader sr = new StreamReader(FilePath, Encoding.Latin1))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("//", StringComparison.Ordinal)) continue;
                        int c1 = line.IndexOf(',');
                        if (c1 <= 0) continue;
                        int c2 = line.IndexOf(',', c1 + 1);
                        string call = line.Substring(0, c1).Trim().ToUpperInvariant();
                        string loc = (c2 > c1 ? line.Substring(c1 + 1, c2 - c1 - 1) : line.Substring(c1 + 1)).Trim().ToUpperInvariant();
                        if (call.Length == 0 || !ValidLoc(loc)) continue;
                        string pl; if (!pool.TryGetValue(loc, out pl)) { pool[loc] = loc; pl = loc; }
                        list.Add(new KeyValuePair<string, string>(call, pl));
                    }
                }
            }
            list.Sort((a, b) => string.CompareOrdinal(a.Key, b.Key));
            string[] c = new string[list.Count], l = new string[list.Count];
            for (int i = 0; i < list.Count; i++) { c[i] = list[i].Key; l[i] = list[i].Value; }
            calls = c; locs = l;
        }

        static IEnumerable<KeyValuePair<string, string>> ReadFile(string path)
        {
            foreach (string raw in File.ReadLines(path, Encoding.Latin1))
            {
                if (raw.Length == 0 || raw.StartsWith("//", StringComparison.Ordinal)) continue;
                string[] p = raw.Split(new[] { ',', ';', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (p.Length < 2) continue;
                string call = p[0].Trim().ToUpperInvariant(), loc = p[1].Trim().ToUpperInvariant();
                if (call.Length >= 3 && ValidLoc(loc)) yield return new KeyValuePair<string, string>(call, loc);
            }
        }

        static Dictionary<string, KeyValuePair<string, DateTime>> ReadDated(string path)
        {
            Dictionary<string, KeyValuePair<string, DateTime>> d = new Dictionary<string, KeyValuePair<string, DateTime>>(StringComparer.Ordinal);
            if (!File.Exists(path)) return d;
            foreach (string line in File.ReadLines(path))
            {
                if (line.StartsWith("//", StringComparison.Ordinal)) continue;
                string[] p = line.Split(',');
                if (p.Length < 2 || !Precise(p[1].Trim().ToUpperInvariant())) continue;
                DateTime w;
                if (p.Length < 3 || !DateTime.TryParse(p[2], CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out w)) w = DateTime.UtcNow;
                if (DateTime.UtcNow - w > TimeSpan.FromDays(730)) continue;   // non confermato da due anni: si lascia cadere
                d[p[0].Trim().ToUpperInvariant()] = new KeyValuePair<string, DateTime>(p[1].Trim().ToUpperInvariant(), w);
            }
            return d;
        }

        // voci a mano, file aggiunti, correzioni dal cluster e locatori imparati (si puo' richiamare per ricaricare)
        public static void LoadLayers()
        {
            Dictionary<string, string> ov = new Dictionary<string, string>(StringComparer.Ordinal);
            List<string> files = new List<string>();
            try
            {
                if (Directory.Exists(OverlayDir))
                {
                    List<string> fl = new List<string>();
                    foreach (string f in Directory.GetFiles(OverlayDir))
                        if (f.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)) fl.Add(f);
                    fl.Sort(StringComparer.OrdinalIgnoreCase);
                    foreach (string f in fl)
                    {
                        int n = 0;
                        foreach (KeyValuePair<string, string> kv in ReadFile(f)) { ov[kv.Key] = kv.Value; n++; }
                        files.Add(Path.GetFileName(f) + " (" + n.ToString(CultureInfo.InvariantCulture) + ")");
                    }
                }
            }
            catch { }
            Dictionary<string, string> man = new Dictionary<string, string>(StringComparer.Ordinal);
            try { if (File.Exists(ManualPath)) foreach (KeyValuePair<string, string> kv in ReadFile(ManualPath)) man[kv.Key] = kv.Value; }
            catch { }
            Dictionary<string, KeyValuePair<string, DateTime>> cl = new Dictionary<string, KeyValuePair<string, DateTime>>(), le = new Dictionary<string, KeyValuePair<string, DateTime>>();
            try { cl = ReadDated(ClusterPath); } catch { }
            try { le = ReadDated(LearnedPath); } catch { }
            lock (lk)
            {
                overlay.Clear(); foreach (KeyValuePair<string, string> kv in ov) overlay[kv.Key] = kv.Value;
                manual.Clear(); foreach (KeyValuePair<string, string> kv in man) manual[kv.Key] = kv.Value;
                cluster.Clear(); foreach (var kv in cl) cluster[kv.Key] = kv.Value;
                learned.Clear(); foreach (var kv in le) learned[kv.Key] = kv.Value;
                OverlayFiles = files;
            }
        }

        static void WriteManual(Dictionary<string, string> man)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// YA-MS - voci a mano sopra call3.txt (nominativo,locatore): valgono sopra tutto");
            List<string> k = new List<string>(man.Keys); k.Sort(StringComparer.Ordinal);
            foreach (string c in k) sb.Append(c).Append(',').Append(man[c]).AppendLine();
            Directory.CreateDirectory(DataDir);
            File.WriteAllText(ManualPath, sb.ToString());
        }

        // ---- voci a mano (sottolinguetta della Raccolta dati)
        public static List<KeyValuePair<string, string>> ManualEntries()
        {
            lock (lk)
            {
                List<KeyValuePair<string, string>> r = new List<KeyValuePair<string, string>>(manual);
                r.Sort((a, b) => string.CompareOrdinal(a.Key, b.Key));
                return r;
            }
        }

        public static bool SetManual(string call, string loc)
        {
            call = (call ?? "").Trim().ToUpperInvariant(); loc = (loc ?? "").Trim().ToUpperInvariant();
            if (call.Length < 3 || !ValidLoc(loc)) return false;
            lock (lk) { manual[call] = loc; try { WriteManual(manual); } catch { } }
            Interlocked.Exchange(ref call3Pending, 1);
            return true;
        }

        // tutte le voci a mano insieme (finestra "Voci a mano")
        public static void ReplaceManual(IEnumerable<KeyValuePair<string, string>> entries)
        {
            lock (lk)
            {
                manual.Clear();
                foreach (KeyValuePair<string, string> kv in entries)
                {
                    string c = (kv.Key ?? "").Trim().ToUpperInvariant(), l = (kv.Value ?? "").Trim().ToUpperInvariant();
                    if (c.Length >= 3 && ValidLoc(l)) manual[c] = l;
                }
                try { WriteManual(manual); } catch { }
            }
            Interlocked.Exchange(ref call3Pending, 1);
            ThreadPool.QueueUserWorkItem(_ => WriteCorrectionsToCall3());
        }

        public static void RemoveManual(string call)
        {
            call = (call ?? "").Trim().ToUpperInvariant();
            lock (lk) { if (manual.Remove(call)) try { WriteManual(manual); } catch { } }
        }

        // ---- file aggiunti: copiati in data/CALL3-AGGIUNTE, valgono sopra call3.txt
        public static void AddOverlayFile(string path)
        {
            Directory.CreateDirectory(OverlayDir);
            File.Copy(path, Path.Combine(OverlayDir, Path.GetFileName(path)), true);
            LoadLayers();
            call3Pending = 1;
            ThreadPool.QueueUserWorkItem(_ => WriteCorrectionsToCall3());
        }

        // ---- correzioni dal cluster: locatore a 6 caratteri dichiarato dalla stazione stessa che spotta
        public static void SetCluster(string call, string loc)
        {
            call = (call ?? "").Trim().ToUpperInvariant(); loc = (loc ?? "").Trim().ToUpperInvariant();
            if (call.Length < 3 || !Precise(loc)) return;
            lock (lk)
            {
                if (manual.ContainsKey(call)) return;   // la voce a mano vince sempre
                KeyValuePair<string, DateTime> c;
                if (cluster.TryGetValue(call, out c) && c.Key == loc)
                {
                    if (DateTime.UtcNow - c.Value > TimeSpan.FromDays(1)) { cluster[call] = new KeyValuePair<string, DateTime>(loc, DateTime.UtcNow); clusterDirty++; }
                    return;
                }
                string o, before = cluster.TryGetValue(call, out c) ? c.Key : overlay.TryGetValue(call, out o) ? o : (Exact(call) ?? "");
                if (before == loc) return;
                cluster[call] = new KeyValuePair<string, DateTime>(loc, DateTime.UtcNow);
                clusterDirty++;
                call3Pending = 1;
            }
            if (clusterDirty >= 20) SaveLayers();
        }

        // ---- LOC6: locatore a 6 caratteri visto dal vivo per un nominativo che il database non ha a 6 caratteri
        public static void Learn(string call, string loc)
        {
            call = (call ?? "").Trim().ToUpperInvariant(); loc = (loc ?? "").Trim().ToUpperInvariant();
            if (call.Length < 3 || !Precise(loc)) return;
            string db = LookupDb(call);
            if (db != null && db.Length == 6) return;   // il database lo sa gia'
            lock (lk)
            {
                KeyValuePair<string, DateTime> c;
                if (learned.TryGetValue(call, out c) && c.Key == loc && DateTime.UtcNow - c.Value < TimeSpan.FromDays(1)) return;
                if (!learned.TryGetValue(call, out c) || c.Key != loc) call3Pending = 1;
                learned[call] = new KeyValuePair<string, DateTime>(loc, DateTime.UtcNow);
                learnedDirty++;
            }
            if (learnedDirty >= 20) SaveLayers();
        }

        public static void SaveLayers() { SaveLayers(false); }

        public static void SaveLayers(bool writeCall3)
        {
            if (DataDir.Length == 0) return;
            try
            {
                lock (lk)
                {
                    if (clusterDirty > 0) { WriteDated(ClusterPath, cluster, "// YA-MS - correzioni di call3.txt dal cluster (nominativo,locatore,ultima conferma UTC)"); clusterDirty = 0; }
                    if (learnedDirty > 0) { WriteDated(LearnedPath, learned, "// YA-MS - locatori a 6 caratteri imparati dal vivo (nominativo,locatore,ultima volta UTC)"); learnedDirty = 0; }
                }
                G7Save();
                if (writeCall3) WriteCorrectionsToCall3();   // alla chiusura; altrimenti ogni 6 ore (G7Tick)
            }
            catch { }
        }

        static void WriteDated(string path, Dictionary<string, KeyValuePair<string, DateTime>> d, string head)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(head);
            List<string> k = new List<string>(d.Keys); k.Sort(StringComparer.Ordinal);
            foreach (string c in k) sb.Append(c).Append(',').Append(d[c].Key).Append(',').Append(d[c].Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)).AppendLine();
            Directory.CreateDirectory(DataDir);
            File.WriteAllText(path, sb.ToString());
        }

        static string Exact(string call)
        {
            string[] c = calls, l = locs;
            int lo = 0, hi = c.Length - 1;
            while (lo <= hi)
            {
                int mid = (lo + hi) >> 1;
                int k = string.CompareOrdinal(c[mid], call);
                if (k == 0) return l[mid];
                if (k < 0) lo = mid + 1; else hi = mid - 1;
            }
            return null;
        }

        // voce esatta negli strati e nel file: a mano > cluster > file aggiunti > call3.txt
        static string ExactAll(string call)
        {
            lock (lk)
            {
                string r; KeyValuePair<string, DateTime> c;
                if (manual.TryGetValue(call, out r)) return r;
                if (cluster.TryGetValue(call, out c)) return c.Key;
                if (overlay.TryGetValue(call, out r)) return r;
            }
            return Exact(call);
        }

        // database senza LOC6: esatto, poi senza suffisso da ricevitore (-#, -1, -SDR, /SDR); i portatili (con la
        // barra) solo con la voce esatta, perche' potrebbero trovarsi altrove
        static string LookupDb(string call)
        {
            if (string.IsNullOrEmpty(call)) return null;
            call = call.Trim().ToUpperInvariant();
            string r = ExactAll(call);
            if (r != null) return r;
            if (call.EndsWith("/SDR", StringComparison.Ordinal)) call = call.Substring(0, call.Length - 4);
            int dash = call.IndexOf('-');
            if (dash > 0) call = call.Substring(0, dash);
            return call.IndexOf('/') >= 0 ? null : ExactAll(call);
        }

        // locatore dal database, poi LOC6, poi G7VRD (stesso quadrato del locatore ricevuto o di quello a 4 caratteri
        // del database); G7VRD si chiede in sottofondo e la risposta vale dalla volta successiva
        public static string Lookup(string call) { return Lookup(call, null); }

        public static string Lookup(string call, string grid)
        {
            string r = LookupDb(call);
            if (r != null && r.Length == 6) return r;
            string key = (call ?? "").Trim().ToUpperInvariant();
            int dash = key.IndexOf('-'); if (dash > 0) key = key.Substring(0, dash);
            string sq = grid != null && grid.Length >= 4 ? grid.Substring(0, 4).ToUpperInvariant() : r != null && r.Length >= 4 ? r.Substring(0, 4) : "";
            KeyValuePair<string, DateTime> c;
            lock (lk) if (learned.TryGetValue(key, out c) && (sq.Length == 0 || c.Key.StartsWith(sq, StringComparison.Ordinal))) return c.Key;
            string g = G7Get(key);
            if (g.Length == 6 && (sq.Length == 0 || g.StartsWith(sq, StringComparison.Ordinal))) return g;
            if (g.Length == 0) G7Request(key);
            return r;
        }

        // regola unica per i locatori ricevuti: meno di 6 caratteri, o centro del quadrato (LL/MM) -> quello a 6 del
        // database; la voce a mano vale sopra anche al locatore che la stazione manda
        public static string Refine(string loc, string call)
        {
            loc = (loc ?? "").Trim().ToUpperInvariant();
            string key = (call ?? "").Trim().ToUpperInvariant();
            string man; lock (lk) if (manual.TryGetValue(key, out man)) return man;
            if (Precise(loc)) Learn(key, loc);
            string db = Lookup(call, loc);
            if (loc.Length == 0) return db ?? "";
            if (!Precise(loc) && db != null && db.Length == 6) return db;
            return loc;
        }

        // ---- correzioni scritte dentro data/call3.txt
        static int call3Pending, call3Writing;

        // voci a mano, cluster e file aggiunti cambiano sempre la riga del file; G7VRD e LOC6 solo se il file non ha gia'
        // un locatore a 6 caratteri vero per quel nominativo
        public static void WriteCorrectionsToCall3()
        {
            if (DataDir.Length == 0 || Interlocked.Exchange(ref call3Pending, 0) == 0) return;
            if (Interlocked.Exchange(ref call3Writing, 1) == 1) { call3Pending = 1; return; }
            try
            {
                Dictionary<string, string> strong = new Dictionary<string, string>(StringComparer.Ordinal), weak = new Dictionary<string, string>(StringComparer.Ordinal);
                lock (lk)
                {
                    foreach (var kv in learned) weak[kv.Key] = kv.Value.Key;
                    foreach (var kv in g7) weak[kv.Key] = kv.Value.Key;
                    foreach (var kv in overlay) strong[kv.Key] = kv.Value;
                    foreach (var kv in cluster) strong[kv.Key] = kv.Value.Key;
                    foreach (var kv in manual) strong[kv.Key] = kv.Value;
                }
                if (strong.Count == 0 && weak.Count == 0) return;
                string path = Path.Combine(DataDir, "call3.txt");
                List<string> lines = File.Exists(path) ? new List<string>(File.ReadAllLines(path, Encoding.Latin1)) : new List<string>();
                HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
                int changed = 0;
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i];
                    if (line.StartsWith("//", StringComparison.Ordinal)) continue;
                    int c1 = line.IndexOf(',');
                    if (c1 <= 0) continue;
                    string call = line.Substring(0, c1).Trim().ToUpperInvariant();
                    int c2 = line.IndexOf(',', c1 + 1);
                    string loc = (c2 > c1 ? line.Substring(c1 + 1, c2 - c1 - 1) : line.Substring(c1 + 1)).Trim().ToUpperInvariant();
                    string want;
                    if (!strong.TryGetValue(call, out want) && !(weak.TryGetValue(call, out want) && !Precise(loc))) { seen.Add(call); continue; }
                    seen.Add(call);
                    if (want == loc) continue;
                    lines[i] = line.Substring(0, c1 + 1) + want + (c2 > c1 ? line.Substring(c2) : ",,,");
                    changed++;
                }
                List<string> add = new List<string>();
                foreach (var kv in strong) if (!seen.Contains(kv.Key)) add.Add(kv.Key + "," + kv.Value + ",,,");
                foreach (var kv in weak) if (!seen.Contains(kv.Key) && !strong.ContainsKey(kv.Key)) add.Add(kv.Key + "," + kv.Value + ",,,");
                if (changed == 0 && add.Count == 0) return;
                add.Sort(StringComparer.Ordinal);
                lines.AddRange(add);
                string tmp = path + ".tmp";
                File.WriteAllText(tmp, string.Join("\r\n", lines) + "\r\n", Encoding.Latin1);
                if (File.Exists(path)) File.SetAttributes(path, FileAttributes.Normal);
                File.Move(tmp, path, true);
                FileDate = File.GetLastWriteTime(path);
            }
            catch (Exception ex) { LastError = ex.Message; call3Pending = 1; }
            finally { Interlocked.Exchange(ref call3Writing, 0); }
        }

        // ---- importazione di un CALL3.TXT scelto a mano
        public static void Replace(string newFile, string dataDir)
        {
            string dest = Path.Combine(dataDir, "call3.txt");
            if (File.Exists(dest)) File.Copy(dest, Path.Combine(dataDir, "call3.bak"), true);
            File.Copy(newFile, dest, true);
            call3Pending = 1;
            WriteCorrectionsToCall3();
            StartLoad(dataDir);
        }

        // ---- aggiornamento da MMMonVHF: almeno 100.000 righe valide, il precedente in call3.bak; null se riuscito
        static int busy;
        public static async Task<string> UpdateFromMmmonvhf(string dataDir, Action<string> say)
        {
            if (Interlocked.Exchange(ref busy, 1) == 1) return Lang.T("Aggiornamento di CALL3 già in corso.");
            try
            {
                if (say != null) say(Lang.T("Scarico CALL3.TXT da mmmonvhf.de..."));
                byte[] bytes;
                using (HttpClient h = new HttpClient { Timeout = TimeSpan.FromMinutes(5) })
                {
                    h.DefaultRequestHeaders.UserAgent.ParseAdd("YA-MS/" + AppPaths.Version);
                    bytes = await h.GetByteArrayAsync(MmmonvhfUrl);
                }
                string text = Encoding.Latin1.GetString(bytes);
                int ok = 0;
                foreach (string line in text.Split('\n'))
                {
                    if (line.StartsWith("//", StringComparison.Ordinal)) continue;
                    string[] p = line.Split(',');
                    if (p.Length >= 2 && p[0].Trim().Length >= 3 && ValidLoc(p[1].Trim())) ok++;
                }
                if (ok < 100000) return Lang.F("File scaricato non valido ({0} righe utili): CALL3 non cambiato.", ok);
                string dest = Path.Combine(dataDir, "call3.txt"), tmp = dest + ".nuovo";
                File.WriteAllBytes(tmp, bytes);
                if (File.Exists(dest))
                {
                    File.SetAttributes(dest, FileAttributes.Normal);
                    File.Copy(dest, Path.Combine(dataDir, "call3.bak"), true);
                }
                File.Move(tmp, dest, true);
                call3Pending = 1;
                WriteCorrectionsToCall3();   // le correzioni tornano dentro il file nuovo
                StartLoad(dataDir);
                return null;
            }
            catch (Exception ex) { return Lang.F("Aggiornamento di CALL3 non riuscito: {0}", ex.Message); }
            finally { Interlocked.Exchange(ref busy, 0); }
        }
    }
}
