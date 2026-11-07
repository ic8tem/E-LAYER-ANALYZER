// YA-MS - impostazioni in data\config.ini (nessun dato in AppData o in C:\Users)
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace YAMS
{
    public class Config
    {
        public string File;
        Dictionary<string, string> v = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Config(string file)
        {
            File = file;
            if (System.IO.File.Exists(file))
                foreach (string raw in System.IO.File.ReadAllLines(file))
                {
                    string l = raw.Trim();
                    if (l.Length == 0 || l.StartsWith("#") || l.StartsWith(";")) continue;
                    int i = l.IndexOf('=');
                    if (i > 0) v[l.Substring(0, i).Trim()] = l.Substring(i + 1).Trim();
                }
        }

        public string Get(string key, string def) { string s; return v.TryGetValue(key, out s) ? s : def; }

        public double GetD(string key, double def)
        {
            string s; double d;
            if (v.TryGetValue(key, out s) && double.TryParse(s.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out d)) return d;
            return def;
        }

        public bool GetB(string key, bool def) { string s; return v.TryGetValue(key, out s) ? (s == "1" || s.ToLowerInvariant() == "true") : def; }

        public void Set(string key, string val) { v[key] = val; }
        public void Set(string key, double val) { v[key] = val.ToString("R", CultureInfo.InvariantCulture); }
        public void Set(string key, bool val) { v[key] = val ? "1" : "0"; }

        public void Save()
        {
            List<string> l = new List<string>();
            l.Add("# YA-MS - impostazioni (modificabile a mano)");
            foreach (KeyValuePair<string, string> kv in v) l.Add(kv.Key + "=" + kv.Value);
            try { System.IO.File.WriteAllLines(File, l.ToArray()); } catch { }
        }
    }
}
