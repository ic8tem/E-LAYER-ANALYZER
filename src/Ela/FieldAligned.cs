// E-LAYER ANALYZER - geometria dello scatter da irregolarita' allineate al campo magnetico (F.A.I. e aurora radio).
// Condizione di Bragg per irregolarita' allungate lungo B: il raggio diffuso sta sul cono con asse B che contiene
// il raggio incidente, cioe' (k_out - k_in) . B = 0 (articolo F.A.I. IC8FAX-IC8TEM: a1 + a2 = 180 gradi).
// Nel caso di eco verso la stessa stazione (backscatter) diventa: raggio perpendicolare a B (Lange-Hesse: +-2..3 gradi).
using System;
using System.Collections.Generic;

namespace ELA
{
    public class PerpPoint { public double Az, DistKm, Lat, Lon, Elev, Off; }

    public static class FieldAligned
    {
        // Per ogni azimut dalla stazione, la distanza (km) a cui il raggio verso la quota H e' perpendicolare al campo.
        // Restituisce un punto per azimut (passo stepAz), solo se lo scarto minimo e' entro tol e l'elevazione >= minElev.
        public static List<PerpPoint> PerpLine(double lat, double lon, double hKm, double Hkm, double tolDeg, double minElevDeg, double maxDistKm, double stepAz = 2)
        {
            List<PerpPoint> res = new List<PerpPoint>();
            Geo.V3 st = Geo.Ecef(lat, lon, hKm);
            for (double az = 0; az < 360; az += stepAz)
            {
                PerpPoint best = null;
                double prevOff = double.NaN;
                for (double d = 50; d <= maxDistKm; d += 10)
                {
                    double el = Geo.ElevationDeg(d, Hkm, hKm);
                    if (el < minElevDeg) break;
                    double la, lo; Geo.Destination(lat, lon, az, d, out la, out lo);
                    double off = Geo.AspectOffDeg(st, Geo.Ecef(la, lo, Hkm));
                    if (best == null || off < best.Off) best = new PerpPoint { Az = az, DistKm = d, Lat = la, Lon = lo, Elev = el, Off = off };
                    prevOff = off;
                }
                if (best != null && best.Off <= tolDeg) res.Add(best);
            }
            return res;
        }

        // Zona di ricezione di uno scatter point SP (lat, lon, quota H): punti al suolo G da cui la linea di vista verso SP
        // e' perpendicolare al campo in SP (intersezione del "cono" dell'articolo con il suolo). Un punto per azimut.
        public static List<PerpPoint> ReceptionLine(double spLat, double spLon, double Hkm, double tolDeg, double minElevDeg, double maxDistKm, double stepAz = 3)
        {
            List<PerpPoint> res = new List<PerpPoint>();
            Geo.V3 sp = Geo.Ecef(spLat, spLon, Hkm);
            for (double az = 0; az < 360; az += stepAz)
            {
                PerpPoint best = null;
                for (double d = 30; d <= maxDistKm; d += 10)
                {
                    double el = Geo.ElevationDeg(d, Hkm, 0);
                    if (el < minElevDeg) break;
                    double la, lo; Geo.Destination(spLat, spLon, az, d, out la, out lo);
                    double off = Geo.AspectOffDeg(Geo.Ecef(la, lo, 0), sp);
                    if (best == null || off < best.Off) best = new PerpPoint { Az = az, DistKm = d, Lat = la, Lon = lo, Elev = el, Off = off };
                }
                if (best != null && best.Off <= tolDeg) res.Add(best);
            }
            return res;
        }

        // Scarto (gradi) dalla condizione di Bragg per TX in a, scatter point p, RX in g:
        // differenza tra gli angoli che raggio incidente e raggio diffuso formano con il piano perpendicolare a B in p.
        public static double BraggOffDeg(Geo.V3 a, Geo.V3 p, Geo.V3 g)
        {
            Geo.V3 b = Geo.FieldDir(p);
            Geo.V3 ki = (p - a).Unit, ko = (g - p).Unit;
            double s1 = Math.Max(-1, Math.Min(1, ki.Dot(b))), s2 = Math.Max(-1, Math.Min(1, ko.Dot(b)));
            return Math.Abs(Math.Asin(s1) - Math.Asin(s2)) * Geo.R2D;
        }

        // Linea di massima ricezione (articolo F.A.I., fig. 1): punti al suolo G che ricevono il segnale della stazione
        // (lat, lon) diffuso dallo scatter point SP alla quota H. Un punto per azimut attorno a SP.
        public static List<PerpPoint> ConeLine(double lat, double lon, double hKm, double spLat, double spLon, double Hkm, double tolDeg, double minElevDeg, double maxDistKm, double stepAz = 2)
        {
            List<PerpPoint> res = new List<PerpPoint>();
            Geo.V3 tx = Geo.Ecef(lat, lon, hKm), sp = Geo.Ecef(spLat, spLon, Hkm);
            for (double az = 0; az < 360; az += stepAz)
            {
                PerpPoint best = null;
                for (double d = 30; d <= maxDistKm; d += 10)
                {
                    double el = Geo.ElevationDeg(d, Hkm, 0);
                    if (el < minElevDeg) break;
                    double la, lo; Geo.Destination(spLat, spLon, az, d, out la, out lo);
                    double off = BraggOffDeg(tx, sp, Geo.Ecef(la, lo, 0));
                    if (best == null || off < best.Off) best = new PerpPoint { Az = az, DistKm = d, Lat = la, Lon = lo, Elev = el, Off = off };
                }
                if (best != null && best.Off <= tolDeg) res.Add(best);
            }
            return res;
        }

        // Luogo degli scatter point possibili per il collegamento A-B alla quota H (celle di 'step' gradi con scarto <= tol)
        public static List<PerpPoint> PairLocus(double latA, double lonA, double latB, double lonB, double Hkm, double tolDeg, double minElev, double maxDistKm, double step)
        {
            List<PerpPoint> res = new List<PerpPoint>();
            Geo.V3 a = Geo.Ecef(latA, lonA, 0), b = Geo.Ecef(latB, lonB, 0);
            double cl = Math.Max(0.2, Math.Cos(Math.Min(80, Math.Max(Math.Abs(latA), Math.Abs(latB))) * Geo.D2R));
            double dl = maxDistKm / 111.0, dlo = dl / cl;
            double la0 = Math.Max(-85, Math.Min(latA, latB) - dl), la1 = Math.Min(85, Math.Max(latA, latB) + dl);
            double lo0 = Math.Min(lonA, lonB) - dlo, lo1 = Math.Max(lonA, lonB) + dlo;
            if (lo1 - lo0 > 200) { lo0 = Math.Min(lonA, lonB) - 30; lo1 = Math.Max(lonA, lonB) + 30; }
            for (double la = la0; la <= la1; la += step)
                for (double lo = lo0; lo <= lo1; lo += step)
                {
                    double dA = Geo.DistanceKm(latA, lonA, la, lo), dB = Geo.DistanceKm(latB, lonB, la, lo);
                    if (dA > maxDistKm || dB > maxDistKm) continue;
                    double eA = Geo.ElevationDeg(dA, Hkm, 0), eB = Geo.ElevationDeg(dB, Hkm, 0);
                    if (eA < minElev || eB < minElev) continue;
                    double off = BraggOffDeg(a, Geo.Ecef(la, lo, Hkm), b);
                    if (off <= tolDeg) res.Add(new PerpPoint { Lat = la, Lon = Geo.Norm180(lo), DistKm = dA, Az = Geo.Bearing(latA, lonA, la, lo), Elev = eA, Off = off });
                }
            return res;
        }

        // Dati di una stazione verso uno scatter point
        public static PerpPoint Toward(double lat, double lon, double hKm, double spLat, double spLon, double Hkm)
        {
            double d = Geo.DistanceKm(lat, lon, spLat, spLon);
            return new PerpPoint
            {
                Az = Geo.Bearing(lat, lon, spLat, spLon),
                DistKm = d,
                Lat = spLat, Lon = spLon,
                Elev = Geo.ElevationDeg(d, Hkm, hKm),
                Off = Geo.AspectOffDeg(Geo.Ecef(lat, lon, hKm), Geo.Ecef(spLat, spLon, Hkm))
            };
        }

        // Scatter point piu' probabile per un collegamento A-B: tra i punti del luogo (scarto di Bragg <= tol) quello
        // con il percorso piu' corto (minima somma dei quadrati delle distanze: minore attenuazione di tratta);
        // con 'weight' (es. probabilita' dell'ovale aurorale 0..1) si preferiscono i punti con peso maggiore.
        // Se nessun punto rispetta la tolleranza, si prende quello con lo scarto minimo (risultato da trattare con cautela).
        public static PerpPoint SolvePair(double latA, double lonA, double latB, double lonB, double Hkm, double tolDeg, double minElev, double maxDistKm, Func<double, double, double> weight, out double offPair)
        {
            offPair = double.NaN;
            List<PerpPoint> loc = PairLocus(latA, lonA, latB, lonB, Hkm, 30, minElev, maxDistKm, 0.25);
            PerpPoint best = null; double bestScore = double.MaxValue;
            foreach (PerpPoint p in loc)
            {
                double dB = Geo.DistanceKm(latB, lonB, p.Lat, p.Lon);
                double score = (p.Off <= tolDeg ? 0 : 1000 + p.Off * 100) + (p.DistKm * p.DistKm + dB * dB) / 1e6;
                if (weight != null) score += 5.0 * (1.0 - Math.Max(0, Math.Min(1, weight(p.Lat, p.Lon))));
                if (score < bestScore) { bestScore = score; best = p; }
            }
            if (best != null) offPair = best.Off;
            return best;
        }
    }
}
