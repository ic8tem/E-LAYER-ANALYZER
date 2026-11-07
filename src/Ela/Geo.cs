// E-LAYER ANALYZER - geometria: locator Maidenhead, cerchio massimo, salti nello strato E, campo geomagnetico (dipolo)
// Tutte le formule sono descritte nel Documento tecnico (data/ela_documento_tecnico.txt).
using System;
using System.Globalization;

namespace ELA
{
    public static class Geo
    {
        public const double R = 6372.7955;          // raggio terrestre medio (km), come nell'articolo F.A.I. [1] e in YA-MS
        public const double D2R = Math.PI / 180.0;
        public const double R2D = 180.0 / Math.PI;

        // Polo nord geomagnetico del dipolo centrato (IGRF-14, epoca 2025): 80,8 N, 72,7 W
        public static double PoleLat = 80.8, PoleLon = -72.7;

        // ------------------------------------------------------------ locator Maidenhead
        public static bool IsLocator(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            s = s.Trim().ToUpperInvariant();
            if (s.Length != 4 && s.Length != 6 && s.Length != 8) return false;
            if (s[0] < 'A' || s[0] > 'R' || s[1] < 'A' || s[1] > 'R') return false;
            if (!char.IsDigit(s[2]) || !char.IsDigit(s[3])) return false;
            if (s.Length >= 6 && (s[4] < 'A' || s[4] > 'X' || s[5] < 'A' || s[5] > 'X')) return false;
            if (s.Length == 8 && (!char.IsDigit(s[6]) || !char.IsDigit(s[7]))) return false;
            return true;
        }

        // centro del quadrato indicato dal locator (4, 6 o 8 caratteri)
        public static bool LocToLatLon(string loc, out double lat, out double lon)
        {
            lat = lon = double.NaN;
            if (!IsLocator(loc)) return false;
            string s = loc.Trim().ToUpperInvariant();
            lon = (s[0] - 'A') * 20.0 - 180.0 + (s[2] - '0') * 2.0;
            lat = (s[1] - 'A') * 10.0 - 90.0 + (s[3] - '0') * 1.0;
            double dlon = 2.0, dlat = 1.0;
            if (s.Length >= 6)
            {
                dlon = 2.0 / 24.0; dlat = 1.0 / 24.0;
                lon += (s[4] - 'A') * dlon;
                lat += (s[5] - 'A') * dlat;
            }
            if (s.Length == 8)
            {
                dlon /= 10.0; dlat /= 10.0;
                lon += (s[6] - '0') * dlon;
                lat += (s[7] - '0') * dlat;
            }
            lon += dlon / 2; lat += dlat / 2;
            return true;
        }

        public static string LatLonToLoc(double lat, double lon, int len = 6)
        {
            lon = Norm180(lon) + 180.0; lat = Math.Max(-89.999, Math.Min(89.999, lat)) + 90.0;
            char[] c = new char[len];
            int a = (int)(lon / 20); c[0] = (char)('A' + Math.Min(17, a)); lon -= a * 20;
            int b = (int)(lat / 10); c[1] = (char)('A' + Math.Min(17, b)); lat -= b * 10;
            int d = (int)(lon / 2); c[2] = (char)('0' + d); lon -= d * 2;
            int e = (int)lat; c[3] = (char)('0' + e); lat -= e;
            if (len >= 6)
            {
                int f = (int)(lon * 12); c[4] = (char)('A' + Math.Min(23, f));
                int g = (int)(lat * 24); c[5] = (char)('A' + Math.Min(23, g));
            }
            return new string(c, 0, len >= 6 ? 6 : 4);
        }

        public static double Norm180(double lon)
        {
            lon = (lon + 180.0) % 360.0;
            if (lon < 0) lon += 360.0;
            return lon - 180.0;
        }

        public static double Norm360(double a)
        {
            a %= 360.0;
            return a < 0 ? a + 360.0 : a;
        }

        // ------------------------------------------------------------ cerchio massimo
        // angolo al centro (radianti) tra due punti
        public static double CentralAngle(double lat1, double lon1, double lat2, double lon2)
        {
            double p1 = lat1 * D2R, p2 = lat2 * D2R, dl = (lon2 - lon1) * D2R;
            double a = Math.Sin((p2 - p1) / 2), b = Math.Sin(dl / 2);
            double h = a * a + Math.Cos(p1) * Math.Cos(p2) * b * b;
            return 2 * Math.Asin(Math.Min(1, Math.Sqrt(h)));
        }

        public static double DistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            return R * CentralAngle(lat1, lon1, lat2, lon2);
        }

        // azimut iniziale (gradi da nord, senso orario)
        public static double Bearing(double lat1, double lon1, double lat2, double lon2)
        {
            double p1 = lat1 * D2R, p2 = lat2 * D2R, dl = (lon2 - lon1) * D2R;
            double y = Math.Sin(dl) * Math.Cos(p2);
            double x = Math.Cos(p1) * Math.Sin(p2) - Math.Sin(p1) * Math.Cos(p2) * Math.Cos(dl);
            return Norm360(Math.Atan2(y, x) * R2D);
        }

        // punto a distanza d (km) lungo l'azimut brg
        public static void Destination(double lat, double lon, double brg, double d, out double lat2, out double lon2)
        {
            double p1 = lat * D2R, l1 = lon * D2R, t = brg * D2R, dd = d / R;
            double p2 = Math.Asin(Math.Sin(p1) * Math.Cos(dd) + Math.Cos(p1) * Math.Sin(dd) * Math.Cos(t));
            double l2 = l1 + Math.Atan2(Math.Sin(t) * Math.Sin(dd) * Math.Cos(p1), Math.Cos(dd) - Math.Sin(p1) * Math.Sin(p2));
            lat2 = p2 * R2D; lon2 = Norm180(l2 * R2D);
        }

        // punto intermedio a frazione f (0..1) del cerchio massimo
        public static void Intermediate(double lat1, double lon1, double lat2, double lon2, double f, out double lat, out double lon)
        {
            double d = CentralAngle(lat1, lon1, lat2, lon2);
            if (d < 1e-9) { lat = lat1; lon = lon1; return; }
            double p1 = lat1 * D2R, l1 = lon1 * D2R, p2 = lat2 * D2R, l2 = lon2 * D2R;
            double a = Math.Sin((1 - f) * d) / Math.Sin(d), b = Math.Sin(f * d) / Math.Sin(d);
            double x = a * Math.Cos(p1) * Math.Cos(l1) + b * Math.Cos(p2) * Math.Cos(l2);
            double y = a * Math.Cos(p1) * Math.Sin(l1) + b * Math.Cos(p2) * Math.Sin(l2);
            double z = a * Math.Sin(p1) + b * Math.Sin(p2);
            lat = Math.Atan2(z, Math.Sqrt(x * x + y * y)) * R2D;
            lon = Math.Atan2(y, x) * R2D;
        }

        // ------------------------------------------------------------ strato E: elevazione, salti, angolo di incidenza
        // Elevazione (gradi) di un punto alla quota H (km) visto da una stazione alla quota h (km) a distanza al suolo d (km).
        // Geometria sferica esatta, equivalente alla formula (2) dell'articolo F.A.I. [1] e alla formula di YA-MS.
        public static double ElevationDeg(double dKm, double Hkm, double hKm)
        {
            double x = dKm / R, a = R + hKm, b = R + Hkm;
            return Math.Atan2(b * Math.Cos(x) - a, b * Math.Sin(x)) * R2D;
        }

        // Distanza al suolo (km) del punto di riflessione di un salto che parte con elevazione e (gradi), quota H (km)
        // = meta' della lunghezza del salto.
        public static double HalfHopKm(double elevDeg, double Hkm)
        {
            double e = elevDeg * D2R;
            double phi = Math.Asin(R * Math.Cos(e) / (R + Hkm));   // angolo di incidenza sullo strato
            double theta = Math.PI / 2 - e - phi;                  // angolo al centro tra stazione e punto di riflessione
            return R * theta;
        }

        // lunghezza massima di un salto (km) con elevazione minima e0
        public static double MaxHopKm(double minElevDeg, double Hkm) { return 2 * HalfHopKm(minElevDeg, Hkm); }

        // Angolo di incidenza sullo strato (gradi dalla verticale) per un salto di lunghezza hopKm
        public static double IncidenceDeg(double hopKm, double Hkm)
        {
            double e = ElevationDeg(hopKm / 2, Hkm, 0) * D2R;
            return Math.Asin(Math.Min(1, R * Math.Cos(e) / (R + Hkm))) * R2D;
        }

        // Fattore della legge della secante (MUF = foEs * sec(phi)) per un salto di lunghezza hopKm
        public static double SecantFactor(double hopKm, double Hkm)
        {
            double phi = IncidenceDeg(hopKm, Hkm) * D2R;
            return 1.0 / Math.Max(0.05, Math.Cos(phi));
        }

        // ------------------------------------------------------------ vettori (coordinate terrestri, km)
        public struct V3
        {
            public double X, Y, Z;
            public V3(double x, double y, double z) { X = x; Y = y; Z = z; }
            public static V3 operator -(V3 a, V3 b) { return new V3(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }
            public static V3 operator +(V3 a, V3 b) { return new V3(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }
            public static V3 operator *(double k, V3 a) { return new V3(k * a.X, k * a.Y, k * a.Z); }
            public double Dot(V3 b) { return X * b.X + Y * b.Y + Z * b.Z; }
            public double Len { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }
            public V3 Unit { get { double l = Len; return l > 0 ? new V3(X / l, Y / l, Z / l) : this; } }
        }

        public static V3 Ecef(double lat, double lon, double hKm)
        {
            double r = R + hKm, p = lat * D2R, l = lon * D2R;
            return new V3(r * Math.Cos(p) * Math.Cos(l), r * Math.Cos(p) * Math.Sin(l), r * Math.Sin(p));
        }

        // Direzione del campo geomagnetico in un punto: IGRF-14 se caricato, altrimenti dipolo centrato inclinato B ~ 3(m.r)r - m.
        // Il verso non conta: serve solo per la perpendicolarita'.
        public static V3 FieldDir(V3 p)
        {
            if (Igrf.Loaded) return Igrf.Dir(p);
            V3 m = Ecef(PoleLat, PoleLon, -R + 1).Unit;   // versore verso il polo nord geomagnetico
            V3 r = p.Unit;
            return (3 * m.Dot(r) * r - m).Unit;
        }

        // Latitudine geomagnetica (dipolo) di un punto
        public static double GeomagLat(double lat, double lon)
        {
            V3 m = Ecef(PoleLat, PoleLon, -R + 1).Unit;
            V3 r = Ecef(lat, lon, 0).Unit;
            return Math.Asin(Math.Max(-1, Math.Min(1, m.Dot(r)))) * R2D;
        }

        // Inclinazione magnetica (gradi): IGRF-14, oppure dipolo con tan I = 2 tan(lat. geomagnetica)
        public static double InclinationDeg(double lat, double lon)
        {
            if (Igrf.Loaded) { double inc, dec; Igrf.IncDec(lat, lon, out inc, out dec); return inc; }
            return Math.Atan(2 * Math.Tan(GeomagLat(lat, lon) * D2R)) * R2D;
        }

        // Scarto dalla perpendicolarita' (gradi): 90 - angolo tra la linea di vista (da 'from' a 'to') e il campo in 'to'.
        // 0 = raggio perpendicolare alle linee di campo (condizione dello scatter da irregolarita' allineate al campo).
        public static double AspectOffDeg(V3 from, V3 to)
        {
            V3 k = (to - from).Unit;
            V3 b = FieldDir(to);
            double c = Math.Abs(k.Dot(b));
            return Math.Asin(Math.Min(1, c)) * R2D;
        }

        public static string Fmt(double v, int dec)
        {
            if (double.IsNaN(v) || double.IsInfinity(v)) return "-";
            return v.ToString("F" + dec, CultureInfo.CurrentCulture);
        }
    }
}
