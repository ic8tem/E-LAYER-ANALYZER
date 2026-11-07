// E-LAYER ANALYZER - archivio nominativo -> locator (file CALL3.TXT di JTDX/WSJT, formato "CALL,LOCATOR,,,")
// Il file sta in data/call3.txt; si carica su un thread secondario (circa un milione di nominativi).
// I locator letti nei commenti degli spot (es. "JN70CN<ES>JO62") si aggiungono in data/call3_nuovi.txt.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ELA
{
    public static class CallDb
    {
        sealed class Table { public string[] Calls = new string[0], Locs = new string[0]; }
        static volatile Table table = new Table();
        static readonly Dictionary<string, string> learned = new Dictionary<string, string>(StringComparer.Ordinal);
        static readonly object lk = new object();

        public static volatile bool Loading;
        public static int Count { get { return table.Calls.Length; } }
        public static int LearnedCount { get { lock (lk) return learned.Count; } }
        public static string FilePath, LearnedPath, Header = "";
        public static DateTime FileDate;
        public static string LastError = "";
        public static event Action Loaded;

        public static void StartLoad(string dataDir)
        {
            FilePath = Path.Combine(dataDir, "call3.txt");
            LearnedPath = Path.Combine(dataDir, "call3_nuovi.txt");
            if (Loading) return;
            Loading = true;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try { Load(); LastError = ""; }
                catch (Exception ex) { LastError = ex.Message; }
                Loading = false;
                Action a = Loaded;
                if (a != null) a();
            });
        }

        static void Load()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(1100000);
            Dictionary<string, string> pool = new Dictionary<string, string>(StringComparer.Ordinal);   // locator condivisi
            StringBuilder head = new StringBuilder();
            if (File.Exists(FilePath))
            {
                FileDate = File.GetLastWriteTime(FilePath);
                using (StreamReader sr = new StreamReader(FilePath, Encoding.ASCII))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("//", StringComparison.Ordinal))
                        {
                            if (head.Length < 2000) head.AppendLine(line.Substring(2).Trim());
                            continue;
                        }
                        int c1 = line.IndexOf(',');
                        if (c1 <= 0) continue;
                        int c2 = line.IndexOf(',', c1 + 1);
                        string call = line.Substring(0, c1).Trim().ToUpperInvariant();
                        string loc = (c2 > c1 ? line.Substring(c1 + 1, c2 - c1 - 1) : line.Substring(c1 + 1)).Trim().ToUpperInvariant();
                        if (call.Length == 0 || !Geo.IsLocator(loc)) continue;
                        string pl;
                        if (!pool.TryGetValue(loc, out pl)) { pool[loc] = loc; pl = loc; }
                        list.Add(new KeyValuePair<string, string>(call, pl));
                    }
                }
            }
            list.Sort((a, b) => string.CompareOrdinal(a.Key, b.Key));
            string[] c = new string[list.Count], l = new string[list.Count];
            for (int i = 0; i < list.Count; i++) { c[i] = list[i].Key; l[i] = list[i].Value; }
            table = new Table { Calls = c, Locs = l };   // una sola assegnazione: chi legge vede sempre una coppia coerente
            Header = head.ToString();
            lock (lk)
            {
                learned.Clear();
                if (File.Exists(LearnedPath))
                    foreach (string line in File.ReadAllLines(LearnedPath))
                    {
                        string[] p = line.Split(',');
                        if (p.Length >= 2 && Geo.IsLocator(p[1].Trim())) learned[p[0].Trim().ToUpperInvariant()] = p[1].Trim().ToUpperInvariant();
                    }
            }
        }

        static string Exact(string call)
        {
            lock (lk) { string l; if (learned.TryGetValue(call, out l)) return l; }
            Table t = table;
            string[] c = t.Calls;
            int lo = 0, hi = c.Length - 1;
            while (lo <= hi)
            {
                int mid = (lo + hi) >> 1;
                int k = string.CompareOrdinal(c[mid], call);
                if (k == 0) return t.Locs[mid];
                if (k < 0) lo = mid + 1; else hi = mid - 1;
            }
            return null;
        }

        // Locator di un nominativo: esatto, poi senza suffissi di skimmer (-#, -1...), poi la parte piu' lunga tra le barre
        public static string Lookup(string call)
        {
            if (string.IsNullOrEmpty(call)) return null;
            call = call.Trim().ToUpperInvariant();
            string r = Exact(call);
            if (r != null) return r;
            int dash = call.IndexOf('-');
            if (dash > 0) { call = call.Substring(0, dash); r = Exact(call); if (r != null) return r; }
            if (call.IndexOf('/') >= 0)
            {
                string best = "";
                foreach (string p in call.Split('/')) if (p.Length > best.Length) best = p;
                if (best.Length >= 3) return Exact(best);
            }
            return null;
        }

        // Locator imparato da uno spot (solo 6 caratteri, per non peggiorare un dato piu' preciso)
        // Regola dell'autore (come in YA-MS): i locator non precisi (meno di 6 caratteri) si prendono da CALL3: se CALL3 ha
        // per quel nominativo un locator a 6 o piu' caratteri, prevale sempre quello.
        public static string Refine(string call, string loc)
        {
            if (loc == null) return null;
            loc = loc.Trim().ToUpperInvariant();
            if (!Geo.IsLocator(loc) || loc.Length >= 6) return loc;
            string c = Lookup(call);
            if (c == null || c.Length < 6) return loc;
            c = c.ToUpperInvariant();
            return c;
        }

        public static void Learn(string call, string loc)
        {
            if (string.IsNullOrEmpty(call) || !Geo.IsLocator(loc) || loc.Length < 6) return;
            call = call.Trim().ToUpperInvariant(); loc = loc.Trim().ToUpperInvariant();
            lock (lk)
            {
                string old;
                if (learned.TryGetValue(call, out old) && old == loc) return;
                learned[call] = loc;
                try { if (LearnedPath != null) File.AppendAllText(LearnedPath, call + "," + loc + "," + DateTime.UtcNow.ToString("yyyy-MM-dd") + Environment.NewLine); } catch { }
            }
        }

        // Sostituisce data/call3.txt con un nuovo file scelto dall'utente e ricarica
        public static void Replace(string newFile, string dataDir)
        {
            string dest = Path.Combine(dataDir, "call3.txt");
            if (File.Exists(dest)) File.Copy(dest, dest + ".bak", true);
            File.Copy(newFile, dest, true);
            StartLoad(dataDir);
        }
    }
}
