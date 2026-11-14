// E-LAYER ANALYZER - archivio nominativo -> locator: un solo database per tutto il programma, quello del nucleo di YA-MS
// (YAMS.CallDb, regole di LIVETROPOVHF: aggiornamento da MMMonVHF, voci a mano, file aggiunti, correzioni dal cluster,
// locatori a 6 caratteri imparati, G7VRD ogni 30 minuti, correzioni scritte dentro data/call3.txt). Questa classe tiene solo
// i nomi usati dalle linguette di E-LAYER e passa tutto a YAMS.CallDb (richiesta dell'autore, 14/11/2026).
// I locatori imparati dalla versione precedente (data/call3_nuovi.txt) si passano una volta a LOC6 di YA-MS.
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ELA
{
    public static class CallDb
    {
        public static bool Loading { get { return YAMS.CallDb.Loading; } }
        public static int Count { get { return YAMS.CallDb.Count; } }
        public static int LearnedCount { get { return YAMS.CallDb.LearnedCount; } }
        public static string FilePath { get { return YAMS.CallDb.FilePath; } }
        public static DateTime FileDate { get { return YAMS.CallDb.FileDate; } }
        public static string LastError { get { return YAMS.CallDb.LastError; } }

        public static event Action Loaded
        {
            add { YAMS.CallDb.Loaded += value; }
            remove { YAMS.CallDb.Loaded -= value; }
        }

        // righe di commento in testa a call3.txt (provenienza del file)
        static string header; static DateTime headerDate;
        public static string Header
        {
            get
            {
                try
                {
                    string f = FilePath;
                    if (string.IsNullOrEmpty(f) || !File.Exists(f)) return "";
                    DateTime d = File.GetLastWriteTime(f);
                    if (header != null && d == headerDate) return header;
                    StringBuilder sb = new StringBuilder();
                    using (StreamReader sr = new StreamReader(f, Encoding.Latin1))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null && line.StartsWith("//", StringComparison.Ordinal) && sb.Length < 2000) sb.AppendLine(line.Substring(2).Trim());
                    }
                    header = sb.ToString(); headerDate = d;
                    return header;
                }
                catch { return ""; }
            }
        }

        public static void StartLoad(string dataDir)
        {
            if (YAMS.CallDb.Loading) return;
            YAMS.CallDb.StartLoad(dataDir);
            ImportOldLearned(dataDir);
        }

        static int imported;
        static void ImportOldLearned(string dataDir)
        {
            if (Interlocked.Exchange(ref imported, 1) == 1) return;
            string f = Path.Combine(dataDir, "call3_nuovi.txt");
            if (!File.Exists(f)) return;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    while (YAMS.CallDb.Loading) Thread.Sleep(500);
                    foreach (string line in File.ReadAllLines(f))
                    {
                        string[] p = line.Split(',');
                        if (p.Length >= 2) YAMS.CallDb.Learn(p[0], p[1]);
                    }
                    YAMS.CallDb.SaveLayers();
                    File.Move(f, f + ".importato", true);
                }
                catch { }
            });
        }

        public static string Lookup(string call) { return YAMS.CallDb.Lookup(call); }

        // stessa regola di YA-MS: locatore non preciso (meno di 6 caratteri o centro LL/MM) -> quello a 6 del database
        public static string Refine(string call, string loc)
        {
            if (loc == null) return null;
            string r = YAMS.CallDb.Refine(loc, call);
            return string.IsNullOrEmpty(r) ? loc : r;
        }

        public static void Learn(string call, string loc) { YAMS.CallDb.Learn(call, loc); }

        public static void Replace(string newFile, string dataDir) { YAMS.CallDb.Replace(newFile, dataDir); }
    }
}
