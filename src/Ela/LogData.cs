// E-LAYER ANALYZER - log ADIF (porting di LoTW Grid Map v2 di IC8TEM): lettura ADIF, bande dai 28 MHz in su
// (ognuna per conto suo, senza aggregazioni), unione del report LoTW con i file ADIF del log.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ELA
{
    public class QsoRecord
    {
        public string Call = "", Band = "", Mode = "", PropMode = "", Date = "", Time = "", MyGrid = "";
        public List<string> Grids = new List<string>();   // quadrati a 4 caratteri
        public bool Lotw, Qsl;
        public string Key { get { string t = Time.Length >= 4 ? Time.Substring(0, 4) : Time; return Call + "|" + Band + "|" + Date + "|" + t; } }
        public string DateText { get { return Date.Length == 8 ? Date.Substring(0, 4) + "-" + Date.Substring(4, 2) + "-" + Date.Substring(6, 2) : Date; } }
    }

    public static class Adif
    {
        // Legge un file ADIF: record come dizionari campo -> valore
        public static List<Dictionary<string, string>> Parse(byte[] data)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            Dictionary<string, string> cur = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            int n = data.Length, i = 0;
            bool inHeader = n > 0 && data[0] != (byte)'<';
            while (i < n)
            {
                if (data[i] != (byte)'<') { i++; continue; }
                int close = Array.IndexOf(data, (byte)'>', i + 1);
                if (close < 0) break;
                if (close - i - 1 > 200) { i++; continue; }
                string tag = Encoding.ASCII.GetString(data, i + 1, close - i - 1).Trim();
                i = close + 1;
                string lower = tag.ToLowerInvariant();
                if (lower == "eoh") { cur = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); inHeader = false; continue; }
                if (lower == "eor")
                {
                    if (!inHeader && cur.Count > 0) list.Add(cur);
                    cur = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    continue;
                }
                string[] parts = tag.Split(':');
                if (parts.Length < 2) continue;
                int len;
                if (!int.TryParse(parts[1].Trim(), out len) || len < 0) continue;
                if (i + len > n) len = n - i;
                string val = Encoding.UTF8.GetString(data, i, len);
                i += len;
                cur[parts[0].Trim()] = val.Trim();
            }
            return list;
        }

        static string Get(Dictionary<string, string> f, string k) { string v; return f.TryGetValue(k, out v) && v != null ? v.Trim() : ""; }
        static bool Yes(string v) { v = v.Trim().ToUpperInvariant(); return v == "Y" || v == "V"; }

        public static QsoRecord ToQso(Dictionary<string, string> f, bool fromLotw)
        {
            QsoRecord q = new QsoRecord();
            q.Call = Get(f, "CALL").ToUpperInvariant();
            q.Band = LogBands.Normalize(Get(f, "BAND"), Get(f, "FREQ"));
            q.Mode = Get(f, "MODE").ToUpperInvariant();
            string sub = Get(f, "SUBMODE").ToUpperInvariant();
            if (sub.Length > 0) q.Mode = sub;
            q.PropMode = Get(f, "PROP_MODE").ToUpperInvariant();
            q.Date = Get(f, "QSO_DATE");
            q.Time = Get(f, "TIME_ON");
            q.MyGrid = Get(f, "MY_GRIDSQUARE").ToUpperInvariant();
            AddGrid(q, Get(f, "GRIDSQUARE"));
            foreach (string g in Get(f, "VUCC_GRIDS").Split(',', ' ', ';')) AddGrid(q, g);
            if (fromLotw) q.Lotw = Yes(Get(f, "QSL_RCVD"));
            else { q.Lotw = Yes(Get(f, "LOTW_QSL_RCVD")); q.Qsl = Yes(Get(f, "QSL_RCVD")); }
            return q;
        }

        public static void AddGrid(QsoRecord q, string g)
        {
            if (g == null) return;
            g = g.Trim().ToUpperInvariant();
            if (g.Length < 4) return;
            string s = g.Substring(0, 4);
            if (Geo.IsLocator(s) && !q.Grids.Contains(s)) q.Grids.Add(s);
        }
    }

    public static class LogBands
    {
        // nome ADIF, limiti in MHz, etichetta: solo dai 28 MHz in su, ognuna come banda singola
        public static readonly string[] Names = { "10m", "8m", "6m", "5m", "4m", "2m", "1.25m", "70cm", "33cm", "23cm", "13cm", "9cm", "6cm", "3cm", "1.25cm" };
        static readonly double[] Lo = { 28.0, 40.0, 50.0, 54.000001, 70.0, 144.0, 222.0, 420.0, 902.0, 1240.0, 2300.0, 3300.0, 5650.0, 10000.0, 24000.0 };
        static readonly double[] Hi = { 29.7, 45.0, 54.0, 69.9, 71.0, 148.0, 225.0, 450.0, 928.0, 1300.0, 2450.0, 3500.0, 5925.0, 10500.0, 24250.0 };
        public static readonly string[] Labels = { "28 MHz (10 m)", "40 MHz (8 m)", "50 MHz (6 m)", "5 m", "70 MHz (4 m)", "144 MHz (2 m)", "222 MHz (1,25 m)", "432 MHz (70 cm)",
            "902 MHz (33 cm)", "1296 MHz (23 cm)", "2,3 GHz (13 cm)", "3,4 GHz (9 cm)", "5,7 GHz (6 cm)", "10 GHz (3 cm)", "24 GHz (1,25 cm)" };

        public static string Normalize(string band, string freq)
        {
            string b = (band ?? "").Trim().ToLowerInvariant().Replace(',', '.').Replace(" ", "");
            if (Array.IndexOf(Names, b) >= 0) return b;
            double f;
            if (double.TryParse((freq ?? "").Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out f) && f > 0)
                for (int i = 0; i < Names.Length; i++) if (f >= Lo[i] && f <= Hi[i]) return Names[i];
            return b.Length > 0 ? b : "?";
        }

        public static int Index(string b) { return Array.IndexOf(Names, b); }
    }

    public class SquareInfo
    {
        public string Grid;
        public bool Lotw, Qsl;
        public List<QsoRecord> Qsos = new List<QsoRecord>();
        public int Status { get { return Lotw ? 2 : Qsl ? 1 : 0; } }   // 0 lavorato, 1 QSL, 2 LoTW
    }

    // Archivio del log: report LoTW (data/log/lotw_report.adi) + file ADIF aperti dall'utente
    public static class LogStore
    {
        public static Dictionary<string, QsoRecord> Qsos = new Dictionary<string, QsoRecord>();   // sostituito per intero a ogni ricarica
        public static readonly List<string> Files = new List<string>();
        public static string Dir, LotwFile;
        public static string LastError = "";
        public static event Action Changed;

        public static void Init(string dataDir)
        {
            Dir = Path.Combine(dataDir, "log");
            try { Directory.CreateDirectory(Dir); } catch { }
            LotwFile = Path.Combine(Dir, "lotw_report.adi");
        }

        static void Merge(Dictionary<string, QsoRecord> qs, QsoRecord q)
        {
            if (q.Call.Length == 0 || LogBands.Index(q.Band) < 0) return;   // HF sotto i 28 MHz: non serve
            QsoRecord old;
            if (qs.TryGetValue(q.Key, out old))
            {
                old.Lotw |= q.Lotw; old.Qsl |= q.Qsl;
                foreach (string g in q.Grids) if (!old.Grids.Contains(g)) old.Grids.Add(g);
                if (old.PropMode.Length == 0) old.PropMode = q.PropMode;
                if (old.Mode.Length == 0) old.Mode = q.Mode;
            }
            else qs[q.Key] = q;
        }

        static int LoadFile(Dictionary<string, QsoRecord> qs, string path, bool lotw)
        {
            byte[] data = File.ReadAllBytes(path);
            int n = 0;
            foreach (Dictionary<string, string> f in Adif.Parse(data)) { Merge(qs, Adif.ToQso(f, lotw)); n++; }
            return n;
        }

        // ricarica tutto (thread secondario)
        public static void Reload()
        {
            Dictionary<string, QsoRecord> qs = new Dictionary<string, QsoRecord>();
            string err = "";
            try { if (File.Exists(LotwFile)) LoadFile(qs, LotwFile, true); } catch (Exception ex) { err += "LoTW: " + ex.Message + " "; }
            string[] files; lock (Files) files = Files.ToArray();
            foreach (string f in files)
                try { if (File.Exists(f)) LoadFile(qs, f, false); } catch (Exception ex) { err += Path.GetFileName(f) + ": " + ex.Message + " "; }
            Qsos = qs; LastError = err;
        }

        public static void RaiseChanged() { Action a = Changed; if (a != null) a(); }

        // quadrati di una banda, con filtro facoltativo sul modo di propagazione
        public static Dictionary<string, SquareInfo> Squares(string band, Func<QsoRecord, bool> filter)
        {
            Dictionary<string, SquareInfo> d = new Dictionary<string, SquareInfo>();
            Dictionary<string, QsoRecord> all = Qsos;
            foreach (QsoRecord q in all.Values)
            {
                if (q.Band != band) continue;
                if (filter != null && !filter(q)) continue;
                foreach (string g in q.Grids)
                {
                    SquareInfo s;
                    if (!d.TryGetValue(g, out s)) { s = new SquareInfo { Grid = g }; d[g] = s; }
                    s.Qsos.Add(q);
                    s.Lotw |= q.Lotw; s.Qsl |= q.Qsl;
                }
            }
            return d;
        }
    }
}
