// E-LAYER ANALYZER - PREVISIONE DEL TEP (propagazione transequatoriale) dalla propria stazione (dal 22/11/2026, richiesta
// dell'autore: «servono indicatori, tipo SFI, che valutino la ionizzazione delle sacche, come modello previsionale»; decisione
// dell'autore del 22/11/2026: l'SFI pesa ma non azzera la previsione, perche' al minimo solare il TEP diventa raro ma non sparisce).
// Indice 0-100 = stagione x SFI x Kp x ora locale x foF2 delle ionosonde equatoriali, per ogni zona oltre l'equatore magnetico che
// la regola del TEP (EsModel.IsTransEquatorialWorld, Kennewell e Wilkinson [49]) collega alla propria stazione:
//  - stagione: massimo agli equinozi (marzo-aprile, settembre-ottobre), 0,35 ai solstizi (ZS6BTE [79]; VK2ZRH [81]: in VHF bassa
//    le aperture continuano anche nei mesi dei solstizi);
//  - SFI: da 0,2 (SFI 65 o meno) a 1 (SFI 150 o piu'): le aperture crescono con il massimo solare ma non spariscono al minimo [81];
//  - Kp, solo per il TEP serale: 1 con Kp fino a 3, poi scende (BOM/IPS [49]: la quiete geomagnetica favorisce l'eTEP);
//  - ora solare locale sull'equatore magnetico: TEP pomeridiano (aTEP) con il massimo verso le 17, serale (eTEP) verso le 21 [49];
//  - foF2 delle ionosonde GIRO entro 2500 km dal punto medio (cresta dell'anomalia equatoriale): da 0,3 (8 MHz o meno) a 1
//    (14 MHz o piu'); senza ionosonde il fattore vale 1 e la riga lo dice. Lo spread F (Cueva e altri [78]) non e' tra i dati GIRO letti.
// TEP IN CORSO: spot TEP dell'ultima ora con un'estremita' entro 1500 km dalla propria stazione o dalla zona.
// I pesi sono scelte del programma, da verificare sui dati: non sono tarati su un archivio.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ELA
{
    public class TepAlert : PrecursorAlert
    {
        public double EndLat, EndLon, MidLat, MidLon, Index, LocalHour;
        public double FSeason, FSfi, FKp, FTime, FGiro, Sfi = double.NaN, Kp = double.NaN, FoF2 = double.NaN;
        public string Kind = "aTEP", GiroNames = "";
        public int Spots;
        public override string LevelText { get { return Spots > 0 ? YAMS.Lang.T("TEP IN CORSO") : Level == 2 ? YAMS.Lang.T("TEP PROBABILE") : YAMS.Lang.T("TEP POSSIBILE"); } }
        public override string Key { get { return "T|" + Region; } }
    }

    public static class TepForecast
    {
        public static bool Enabled = true;
        public const double ProbableIdx = 40, PossibleIdx = 15, SpotKm = 1500;

        static double Gauss(double x, double mu, double sd) { return Math.Exp(-0.5 * (x - mu) * (x - mu) / (sd * sd)); }

        public static double SeasonFactor(DateTime utc)
        {
            double doy = utc.DayOfYear;
            double d = Math.Min(Math.Min(Math.Abs(doy - 79), Math.Abs(doy - 265)), Math.Min(Math.Abs(doy - 79 - 365), Math.Abs(doy - 265 + 365)));
            double c = Math.Cos(Math.PI * Math.Min(d, 91.5) / 183);
            return 0.35 + 0.65 * c * c;
        }

        public static double CurrentSfi()
        {
            double v;
            return double.TryParse(PropData.S("solarflux"), NumberStyles.Float, CultureInfo.InvariantCulture, out v) && v > 0 ? v : double.NaN;
        }

        public static double SfiFactor(double sfi) { return double.IsNaN(sfi) ? 0.6 : 0.2 + 0.8 * Math.Max(0, Math.Min(1, (sfi - 65) / 85)); }
        public static double KpFactor(double kp) { return double.IsNaN(kp) || kp <= 3 ? 1 : kp <= 4 ? 0.7 : kp <= 5 ? 0.5 : 0.3; }

        public static List<TepAlert> Compute()
        {
            List<TepAlert> res = new List<TepAlert>();
            if (!Enabled || !Station.Valid) return res;
            DateTime now = DateTime.UtcNow;
            double fs = SeasonFactor(now), sfi = CurrentSfi(), kp = SpaceWeather.LatestKp;
            double fsfi = SfiFactor(sfi), fkp = KpFactor(kp);
            // zone raggiunte dal TEP: si cerca nelle direzioni e distanze della regola del TEP
            Dictionary<string, TepAlert> zones = new Dictionary<string, TepAlert>();
            for (double az = 0; az < 360; az += 4)
                for (double d = EsModel.TepMinKm; d <= EsModel.TepMaxKm; d += 250)
                {
                    double la, lo; Geo.Destination(Station.Lat, Station.Lon, az, d, out la, out lo);
                    if (!EsModel.IsTransEquatorialWorld(Station.Lat, Station.Lon, la, lo)) continue;
                    if (!Chains.HasOperators(la, lo, 300)) continue;
                    double ml, mo; Geo.Intermediate(Station.Lat, Station.Lon, la, lo, 0.5, out ml, out mo);
                    string zone = Chains.ZoneName(la, lo);
                    double sym = Math.Abs(Math.Abs(Geo.InclinationDeg(la, lo)) - Math.Abs(Geo.InclinationDeg(Station.Lat, Station.Lon)));
                    TepAlert old;
                    if (zones.TryGetValue(zone, out old) && old.FoF2 <= sym) continue;   // FoF2 usato qui come scarto di simmetria
                    zones[zone] = new TepAlert { Region = zone, EndLat = la, EndLon = lo, MidLat = ml, MidLon = mo, DistKm = d, Az = az, FoF2 = sym,
                                                 TargetLat = la, TargetLon = lo, Band = "6 m", FreqMHz = 50.2 };
                }
            List<Spot> tepSpots = SpotStore.Recent(60, s => s.HasBoth && s.IsTep).ToList();
            foreach (TepAlert a in zones.Values)
            {
                a.LocalHour = ((now.Hour + now.Minute / 60.0 + a.MidLon / 15.0) % 24 + 24) % 24;
                double fa = Gauss(a.LocalHour, 17, 1.5), fe = Gauss(a.LocalHour, 21, 1.3);
                a.Kind = fe > fa ? "eTEP" : "aTEP";
                a.Band = fe > fa ? "6/2 m" : "6 m";
                a.FTime = Math.Max(0.05, Math.Max(fa, fe));
                // foF2 delle ionosonde vicine al punto medio (cresta dell'anomalia equatoriale)
                double best = double.NaN; string names = "";
                foreach (GiroStation g in Giro.Stations)
                {
                    if (Geo.DistanceKm(a.MidLat, a.MidLon, g.Lat, g.Lon) > 2500) continue;
                    DateTime when; double f = g.LastValue(r => r.FoF2, Giro.FreshMin, out when);
                    if (double.IsNaN(f)) continue;
                    if (double.IsNaN(best) || f > best) best = f;
                    if (names.Length < 60) names += (names.Length > 0 ? ", " : "") + g.Name + " " + f.ToString("F1", CultureInfo.CurrentCulture);
                }
                a.FoF2 = best; a.GiroNames = names;
                a.FGiro = double.IsNaN(best) ? 1 : 0.3 + 0.7 * Math.Max(0, Math.Min(1, (best - 8) / 6));
                a.FSeason = fs; a.FSfi = fsfi; a.FKp = a.Kind == "eTEP" ? fkp : 1; a.Sfi = sfi; a.Kp = kp;
                a.Index = 100 * fs * fsfi * a.FKp * a.FTime * a.FGiro;
                a.Spots = tepSpots.Count(s => Math.Min(Geo.DistanceKm(s.LatS, s.LonS, Station.Lat, Station.Lon), Geo.DistanceKm(s.LatD, s.LonD, Station.Lat, Station.Lon)) <= SpotKm
                                            || Math.Min(Geo.DistanceKm(s.LatS, s.LonS, a.EndLat, a.EndLon), Geo.DistanceKm(s.LatD, s.LonD, a.EndLat, a.EndLon)) <= SpotKm);
                a.Level = a.Spots > 0 || a.Index >= ProbableIdx ? 2 : a.Index >= PossibleIdx ? 1 : 0;
                if (a.Level > 0) res.Add(a);
            }
            return res.OrderByDescending(a => a.Spots).ThenByDescending(a => a.Index).Take(4).ToList();
        }
    }
}
