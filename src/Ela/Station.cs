// E-LAYER ANALYZER - stazione dell'utente (nominativo, locator, quota) e impostazioni (data/ela.ini)
using System;
using System.IO;
using YAMS;

namespace ELA
{
    public static class Station
    {
        public static string Call = "", Loc = "";
        public static double AltM = 0, Lat = double.NaN, Lon = double.NaN;
        public static Config Cfg;
        public static event Action Changed;

        public static bool Valid { get { return !double.IsNaN(Lat); } }
        public static double AltKm { get { return AltM / 1000.0; } }

        public static void Load(string dataDir)
        {
            Cfg = new Config(Path.Combine(dataDir, "ela.ini"));
            Call = Cfg.Get("nominativo", "");
            string ymsLoc = "";
            try { ymsLoc = new Config(Path.Combine(dataDir, "config.ini")).Get("mio_locator", ""); } catch { }
            Set(Call, Cfg.Get("locator", ymsLoc), Cfg.GetD("quota_m", 0), false);
        }

        public static void Set(string call, string loc, double altM, bool notify = true)
        {
            Call = (call ?? "").Trim().ToUpperInvariant();
            Loc = (loc ?? "").Trim().ToUpperInvariant();
            AltM = altM;
            double la, lo;
            if (Geo.LocToLatLon(Loc, out la, out lo)) { Lat = la; Lon = lo; } else { Lat = Lon = double.NaN; }
            if (Cfg != null)
            {
                Cfg.Set("nominativo", Call); Cfg.Set("locator", Loc); Cfg.Set("quota_m", AltM);
                try { Cfg.Save(); } catch { }
            }
            if (notify) { Action a = Changed; if (a != null) a(); }
        }

        public static void Save() { try { if (Cfg != null) Cfg.Save(); } catch { } }
    }
}
