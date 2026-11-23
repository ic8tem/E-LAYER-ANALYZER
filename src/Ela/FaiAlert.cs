// E-LAYER ANALYZER - ALLERTA F.A.I. dopo l'E-sporadico (dal 22/11/2026, richiesta dell'autore: «produci le allerte del caso,
// magari facendo lampeggiare la linguetta corrispondente»). Regole dell'articolo IC8FAX-IC8TEM [1] (2011): il F.A.I. e' piu'
// frequente da maggio a settembre fra le 18 e le 24 ora locale, e va in coppia con l'E-sporadico in 50 MHz (F.A.I. in 144 MHz
// dopo le nubi Es in 50 MHz); fonte [77] (radar MU): le irregolarita' allineate al campo si formano anche sulle scie meteoriche.
//  - F.A.I. IN CORSO: spot F.A.I. (<FAI>, modo FAI) negli ultimi 30 minuti;
//  - F.A.I. POSSIBILE: nube Es (spot in singolo salto in 50, 70 o 144 MHz dell'ultima ora) in un'ora locale fra le 18 e le 24,
//    nei mesi da maggio a settembre (da novembre a marzo nell'emisfero sud), a 300-1300 km dalla propria stazione, vista con
//    l'elevazione minima, e con la linea di massima ricezione (scarto di Bragg entro 1°) che passa dove ci sono stazioni.
// Luoghi noti dall'articolo: Ginevra (JN26-JN36), Budapest (JN97), Bruxelles (JO20), Albania (JN92), Corsica (JN41-JN42).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ELA
{
    public class FaiCandidate
    {
        public double Lat, Lon, Km, Az, LocalHour;
        public string Zone = "", Known = "", Partners = "";
        public Spot Spot;
    }

    public static class FaiAlert
    {
        public const double MinKm = 300, MaxKm = 1300, TolDeg = 1.0, Hkm = 105;
        public static int Level;                                  // 0 nulla, 1 possibile, 2 in corso
        public static string Text = "";
        public static List<FaiCandidate> Candidates = new List<FaiCandidate>();
        public static List<Spot> FaiSpots = new List<Spot>();

        // nomi tradotti con Lang.T: Lang.T("Ginevra") Lang.T("Budapest") Lang.T("Bruxelles") Lang.T("Albania") Lang.T("Corsica")
        static readonly (string Name, string[] Sq)[] Known =
        {
            ("Ginevra", new[] { "JN26", "JN36" }), ("Budapest", new[] { "JN97" }), ("Bruxelles", new[] { "JO20" }),
            ("Albania", new[] { "JN92" }), ("Corsica", new[] { "JN41", "JN42" }),
        };

        public static bool Season(DateTime utc, double lat)
        {
            int m = utc.Month;
            return lat >= 0 ? m >= 5 && m <= 9 : m >= 11 || m <= 3;
        }

        public static void Compute(DateTime now)
        {
            List<FaiCandidate> res = new List<FaiCandidate>();
            FaiSpots = SpotStore.Recent(30, s => Filters.IsFaiSpot(s)).ToList();
            if (Station.Valid && Season(now, Station.Lat))
            {
                double minEl = Math.Max(0, EsModel.DefaultElevMin);
                foreach (Spot s in SpotStore.Recent(60, x => x.HasBoth && x.EsCandidate && !x.IsTep && (x.Band == "6m" || x.Band == "4m" || x.Band == "2m")))
                {
                    double d = s.DistanceKm;
                    if (d < 400 || d > Precursors.MaxHopKm) continue;
                    double la, lo; Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
                    double lt = ((now.Hour + now.Minute / 60.0 + lo / 15.0) % 24 + 24) % 24;      // ora solare locale sulla nube
                    if (lt < 18) continue;
                    double km = Geo.DistanceKm(Station.Lat, Station.Lon, la, lo);
                    if (km < MinKm || km > MaxKm) continue;
                    if (Geo.ElevationTo(Station.Lat, Station.Lon, Station.AltKm, la, lo, Hkm) < minEl) continue;
                    if (res.Any(c => Geo.DistanceKm(c.Lat, c.Lon, la, lo) <= 250)) continue;            // una per zona
                    List<PerpPoint> line = FieldAligned.ConeLine(Station.Lat, Station.Lon, Station.AltKm, la, lo, Hkm, TolDeg, minEl, 1500, 6);
                    List<string> who = new List<string>();
                    foreach (PerpPoint p in line)
                    {
                        if (Geo.DistanceKm(Station.Lat, Station.Lon, p.Lat, p.Lon) < 200) continue;
                        if (!Chains.HasOperators(p.Lat, p.Lon, 150)) continue;
                        string z = Geo.LatLonToLoc(p.Lat, p.Lon, 4);
                        if (!who.Contains(z)) who.Add(z);
                    }
                    if (who.Count == 0) continue;
                    FaiCandidate c = new FaiCandidate { Lat = la, Lon = lo, Km = km, Az = Geo.Bearing(Station.Lat, Station.Lon, la, lo), LocalHour = lt, Spot = s, Zone = Chains.ZoneName(la, lo) };
                    string sq = Geo.LatLonToLoc(la, lo, 4);
                    foreach (var k in Known) if (k.Sq.Contains(sq)) c.Known = k.Name;
                    c.Partners = string.Join(", ", who.Take(6));
                    res.Add(c);
                }
            }
            Candidates = res.OrderBy(c => c.Known.Length > 0 ? 0 : 1).ThenBy(c => c.Km).Take(8).ToList();
            Level = FaiSpots.Count > 0 ? 2 : Candidates.Count > 0 ? 1 : 0;
            Text = BuildText();
        }

        static string BuildText()
        {
            if (Level == 0) return "";
            List<string> l = new List<string>();
            if (Level == 2)
            {
                Spot s = FaiSpots[FaiSpots.Count - 1];
                l.Add(YAMS.Lang.F("F.A.I. IN CORSO: {0} spot F.A.I. negli ultimi 30 minuti (ultimo {1} - {2}, {3} MHz).", FaiSpots.Count, s.Spotter, s.DxCall, s.FreqMHz.ToString("F3", CultureInfo.CurrentCulture)));
            }
            foreach (FaiCandidate c in Candidates.Take(3))
                l.Add(YAMS.Lang.F("F.A.I. POSSIBILE: nube Es su {0} ({1}), {2:F0} km, QTF {3:F0}°, ore {4:00}:{5:00} locali; linea di massima ricezione su {6}.",
                    c.Known.Length > 0 ? YAMS.Lang.T(c.Known) : c.Zone, Geo.LatLonToLoc(c.Lat, c.Lon, 4), c.Km, c.Az, Math.Floor(c.LocalHour), Math.Floor((c.LocalHour % 1) * 60), c.Partners));
            return string.Join("\n", l);
        }
    }
}
