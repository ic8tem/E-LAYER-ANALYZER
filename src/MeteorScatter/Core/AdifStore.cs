// YA-MS - quadrati Maidenhead lavorati, letti da un file ADIF (nessun collegamento a LoTW)
// Il caricamento di un nuovo ADIF sostituisce i dati precedenti; i dati restano in data/adif_quadrati.csv.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace YAMS
{
    public static class AdifStore
    {
        public static readonly string[] Bands = { "6m", "4m", "2m", "1.25m", "70cm" };
        public static readonly string[] BandNames = { "50 MHz", "70 MHz", "144 MHz", "222 MHz", "432 MHz" };
        public static List<GridQso> Qsos = new List<GridQso>();
        public static string SourceName = "";

        static AdifStore()
        {
            // in .NET 8 la codifica Windows-1252 richiede il provider delle code page
            try { Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); } catch { }
        }

        static Encoding Ansi()
        {
            try { return Encoding.GetEncoding(1252); }
            catch { return Encoding.Latin1; }
        }

        static string StoreFile(string dataDir) { return Path.Combine(dataDir, "adif_quadrati.csv"); }

        // Banda ADIF dal campo BAND o, se manca, dalla frequenza in MHz
        static string BandOf(string band, string freq)
        {
            string b = (band ?? "").Trim().ToLowerInvariant();
            foreach (string x in Bands) if (b == x) return x;
            double f;
            if (double.TryParse((freq ?? "").Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out f))
            {
                if (f >= 50 && f <= 54) return "6m";
                if (f >= 69.9 && f <= 71.5) return "4m";
                if (f >= 144 && f <= 148) return "2m";
                if (f >= 219 && f <= 225) return "1.25m";
                if (f >= 420 && f <= 450) return "70cm";
            }
            return null;
        }

        static bool ValidGrid(string g)
        {
            return g.Length >= 4 && g[0] >= 'A' && g[0] <= 'R' && g[1] >= 'A' && g[1] <= 'R' && char.IsDigit(g[2]) && char.IsDigit(g[3]);
        }

        // Lettura ADIF: <CAMPO:lunghezza[:tipo]>valore ... <EOR>
        public static int LoadAdif(string file, out string msg)
        {
            msg = "";
            string text = File.ReadAllText(file, Ansi());
            List<GridQso> list = new List<GridQso>();
            Dictionary<string, string> rec = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            int pos = 0, eoh = text.IndexOf("<eoh>", StringComparison.OrdinalIgnoreCase);
            if (eoh >= 0) pos = eoh + 5;
            int records = 0, skipped = 0;
            while (pos < text.Length)
            {
                int lt = text.IndexOf('<', pos);
                if (lt < 0) break;
                int gt = text.IndexOf('>', lt);
                if (gt < 0) break;
                string tag = text.Substring(lt + 1, gt - lt - 1);
                pos = gt + 1;
                if (tag.Equals("eor", StringComparison.OrdinalIgnoreCase))
                {
                    records++;
                    if (!AddRecord(rec, list)) skipped++;
                    rec.Clear();
                    continue;
                }
                string[] parts = tag.Split(':');
                int len;
                if (parts.Length < 2 || !int.TryParse(parts[1], out len) || len < 0) continue;
                if (pos + len > text.Length) len = text.Length - pos;
                rec[parts[0].Trim()] = text.Substring(pos, len);
                pos += len;
            }
            Qsos = list;
            SourceName = Path.GetFileName(file);
            msg = Lang.F("{0} QSO letti, {1} quadrati-QSO nelle bande 50-432 MHz, {2} QSO scartati (altre bande o senza quadrato).", records, list.Count, skipped);
            return list.Count;
        }

        static bool AddRecord(Dictionary<string, string> r, List<GridQso> list)
        {
            string v;
            string band = BandOf(r.TryGetValue("BAND", out v) ? v : null, r.TryGetValue("FREQ", out v) ? v : null);
            if (band == null) return false;
            List<string> grids = new List<string>();
            if (r.TryGetValue("GRIDSQUARE", out v)) grids.Add(v);
            if (r.TryGetValue("VUCC_GRIDS", out v)) grids.AddRange(v.Split(','));
            bool conf = (r.TryGetValue("QSL_RCVD", out v) && v.Trim().ToUpperInvariant() == "Y")
                     || (r.TryGetValue("LOTW_QSL_RCVD", out v) && v.Trim().ToUpperInvariant() == "Y");
            string call = r.TryGetValue("CALL", out v) ? v.Trim().ToUpperInvariant() : "";
            string date = r.TryGetValue("QSO_DATE", out v) ? v.Trim() : "";
            bool any = false;
            foreach (string g0 in grids)
            {
                string g = g0.Trim().ToUpperInvariant();
                if (!ValidGrid(g)) continue;
                GridQso q = new GridQso();
                q.Band = band; q.Grid = g.Substring(0, 4); q.Call = call; q.Date = date; q.Confirmed = conf;
                list.Add(q); any = true;
            }
            return any;
        }

        public static void Save(string dataDir)
        {
            List<string> l = new List<string>();
            l.Add("# Quadrati lavorati letti dal file ADIF " + SourceName);
            l.Add("banda;quadrato;nominativo;data;confermato");
            foreach (GridQso q in Qsos) l.Add(q.Band + ";" + q.Grid + ";" + q.Call + ";" + q.Date + ";" + (q.Confirmed ? "1" : "0"));
            Directory.CreateDirectory(dataDir);
            File.WriteAllLines(StoreFile(dataDir), l.ToArray());
        }

        public static void Load(string dataDir)
        {
            Qsos = new List<GridQso>(); SourceName = "";
            string f = StoreFile(dataDir);
            if (!File.Exists(f)) return;
            foreach (string raw in File.ReadAllLines(f))
            {
                if (raw.StartsWith("# Quadrati lavorati letti dal file ADIF ")) { SourceName = raw.Substring(40).Trim(); continue; }
                if (raw.StartsWith("#") || raw.StartsWith("banda;")) continue;
                string[] c = raw.Split(';');
                if (c.Length < 5) continue;
                GridQso q = new GridQso();
                q.Band = c[0]; q.Grid = c[1]; q.Call = c[2]; q.Date = c[3]; q.Confirmed = c[4] == "1";
                Qsos.Add(q);
            }
        }

        public static void Clear(string dataDir)
        {
            Qsos = new List<GridQso>(); SourceName = "";
            string f = StoreFile(dataDir);
            if (File.Exists(f)) File.Delete(f);
        }
    }
}
