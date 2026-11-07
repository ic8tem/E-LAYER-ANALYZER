// YA-MS - mappa d'Europa da Natural Earth (dominio pubblico, www.naturalearthdata.com)
// Al primo avvio scarica le coste e i confini in scala 1:50 milioni in data\mappa\ e poi li usa senza rete.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;

namespace YAMS
{
    public static class MapData
    {
        public const string CoastUrl = "https://raw.githubusercontent.com/nvkelso/natural-earth-vector/master/geojson/ne_50m_coastline.geojson";
        public const string BorderUrl = "https://raw.githubusercontent.com/nvkelso/natural-earth-vector/master/geojson/ne_50m_admin_0_boundary_lines_land.geojson";

        public static string CoastFile(string dataDir) { return Path.Combine(Path.Combine(dataDir, "mappa"), "ne_50m_coastline.geojson"); }
        public static string BorderFile(string dataDir) { return Path.Combine(Path.Combine(dataDir, "mappa"), "ne_50m_admin_0_boundary_lines_land.geojson"); }

        public static bool EnsureFiles(string dataDir, out string msg)
        {
            lock (fileLock) return EnsureFilesLocked(dataDir, out msg);
        }

        static readonly object fileLock = new object();

        static bool EnsureFilesLocked(string dataDir, out string msg)
        {
            msg = "";
            Directory.CreateDirectory(Path.Combine(dataDir, "mappa"));
            bool ok = true;
            string[] urls = { CoastUrl, BorderUrl };
            string[] files = { CoastFile(dataDir), BorderFile(dataDir) };
            for (int i = 0; i < 2; i++)
            {
                if (File.Exists(files[i])) continue;
                try
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | (SecurityProtocolType)768 | SecurityProtocolType.Tls;
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers[HttpRequestHeader.UserAgent] = RadarFlux.UserAgent;
                        wc.DownloadFile(urls[i], files[i] + ".tmp");
                    }
                    File.Move(files[i] + ".tmp", files[i]);
                }
                catch (Exception ex) { ok = false; msg += Path.GetFileName(files[i]) + ": " + ex.Message + " "; }
            }
            return ok;
        }

        // Lettura minima di GeoJSON (LineString / MultiLineString / Polygon): restituisce polilinee (lon, lat)
        // limitate alla zona euro-mediterranea e dintorni.
        // Mondo intero (E-LAYER ANALYZER: mappe di sporadico E, F.A.I. e aurora)
        public static List<PointF[]> LoadAll(string file)
        {
            world = true;
            try { return Load(file); }
            finally { world = false; }
        }

        [ThreadStatic] static bool world;

        public static List<PointF[]> Load(string file) { return Load(file, -50, 75); }

        // come Load, con i limiti di longitudine scelti (la mappa delle fonti arriva fino al Canada)
        public static List<PointF[]> Load(string file, double lonMin, double lonMax)
        {
            List<PointF[]> res = new List<PointF[]>();
            if (!File.Exists(file)) return res;
            string s = File.ReadAllText(file);
            int pos = 0;
            while (true)
            {
                int k = s.IndexOf("\"coordinates\"", pos, StringComparison.Ordinal);
                if (k < 0) break;
                int b = s.IndexOf('[', k);
                if (b < 0) break;
                pos = b;
                ParseArray(s, ref pos, res, lonMin, lonMax);
            }
            return res;
        }

        // Analizza un array annidato; ogni array di coppie [lon, lat] diventa una polilinea
        static void ParseArray(string s, ref int pos, List<PointF[]> res, double lonMin, double lonMax)
        {
            // pos punta a '['
            int depth = 0;
            List<PointF> cur = null;
            List<double> nums = new List<double>();
            while (pos < s.Length)
            {
                char c = s[pos];
                if (c == '[')
                {
                    depth++; nums.Clear(); pos++;
                    continue;
                }
                if (c == ']')
                {
                    if (nums.Count >= 2)
                    {
                        if (cur == null) cur = new List<PointF>();
                        cur.Add(new PointF((float)nums[0], (float)nums[1]));
                        nums.Clear();
                    }
                    else if (cur != null)
                    {
                        Flush(cur, res, lonMin, lonMax); cur = null;
                    }
                    depth--; pos++;
                    if (depth == 0) { if (cur != null) Flush(cur, res, lonMin, lonMax); return; }
                    continue;
                }
                if (c == '-' || c == '.' || char.IsDigit(c))
                {
                    int st = pos;
                    while (pos < s.Length && (char.IsDigit(s[pos]) || s[pos] == '-' || s[pos] == '.' || s[pos] == 'e' || s[pos] == 'E' || s[pos] == '+')) pos++;
                    double v;
                    if (double.TryParse(s.Substring(st, pos - st), NumberStyles.Float, CultureInfo.InvariantCulture, out v)) nums.Add(v);
                    continue;
                }
                pos++;
            }
        }

        static void Flush(List<PointF> cur, List<PointF[]> res, double lonMin, double lonMax)
        {
            // tiene solo i tratti nella zona d'interesse (lat 15..82, lon lonMin..lonMax)
            List<PointF> seg = new List<PointF>();
            foreach (PointF p in cur)
            {
                bool inside = world || (p.Y >= 15 && p.Y <= 82 && p.X >= lonMin && p.X <= lonMax);
                if (inside) seg.Add(p);
                else { if (seg.Count > 1) res.Add(seg.ToArray()); seg.Clear(); }
            }
            if (seg.Count > 1) res.Add(seg.ToArray());
        }
    }
}
