// YA-MS - calcoli astronomici e geografici
// Formule standard (Meeus, Astronomical Algorithms). Dalla versione 1.8.24: ellissoide WGS84 (NGA.STND.0036) per le posizioni,
// geodetiche con il metodo di Karney (GeographicLib) per distanze, azimut e punti lungo la tratta; le formule sferiche
// (elevazione dell'articolo Cerrotta-Sartori, portata, rifrazione) usano il raggio medio dell'ellissoide. Prima: sfera R = 6372.7955 km.
using System;
using System.Globalization;

namespace YAMS
{
    public struct Vec3
    {
        public double X, Y, Z;
        public Vec3(double x, double y, double z) { X = x; Y = y; Z = z; }
        public static Vec3 operator +(Vec3 a, Vec3 b) { return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }
        public static Vec3 operator -(Vec3 a, Vec3 b) { return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }
        public static Vec3 operator *(Vec3 a, double k) { return new Vec3(a.X * k, a.Y * k, a.Z * k); }
        public static Vec3 operator *(double k, Vec3 a) { return new Vec3(a.X * k, a.Y * k, a.Z * k); }
        public double Dot(Vec3 b) { return X * b.X + Y * b.Y + Z * b.Z; }
        public Vec3 Cross(Vec3 b) { return new Vec3(Y * b.Z - Z * b.Y, Z * b.X - X * b.Z, X * b.Y - Y * b.X); }
        public double Length { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }
        public Vec3 Unit { get { double l = Length; return l > 0 ? new Vec3(X / l, Y / l, Z / l) : this; } }
    }

    public struct LatLon
    {
        public double Lat, Lon;
        public LatLon(double lat, double lon) { Lat = lat; Lon = lon; }
    }

    public static class Astro
    {
        // Ellissoide WGS84 (NGA.STND.0036): semiasse a, schiacciamento f, eccentricita' al quadrato e2
        public const double WgsA = 6378.137, WgsF = 1 / 298.257223563, WgsE2 = WgsF * (2 - WgsF);
        // Raggio medio dell'ellissoide R1 = (2a + b) / 3 (km): solo per le formule sferiche (elevazione, portata, rifrazione)
        public const double R = 6371.0088;
        static readonly GeographicLib.Geodesic Geod = GeographicLib.Geodesic.WGS84;
        public const double D2R = Math.PI / 180.0;
        public static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        public static double Norm360(double a) { a = a % 360.0; if (a < 0) a += 360.0; return a; }
        public static double Norm180(double a) { a = Norm360(a); if (a > 180) a -= 360; return a; }

        // ---------- tempo ----------
        public static double JulianDay(DateTime utc)
        {
            int y = utc.Year, m = utc.Month;
            double d = utc.Day + (utc.Hour + (utc.Minute + (utc.Second + utc.Millisecond / 1000.0) / 60.0) / 60.0) / 24.0;
            if (m <= 2) { y -= 1; m += 12; }
            int a = y / 100; int b = 2 - a + a / 4;
            return Math.Floor(365.25 * (y + 4716)) + Math.Floor(30.6001 * (m + 1)) + d + b - 1524.5;
        }

        public static DateTime FromJulianDay(double jd)
        {
            double j = jd + 0.5; double z = Math.Floor(j); double f = j - z;
            double alpha = Math.Floor((z - 1867216.25) / 36524.25);
            double a = z + 1 + alpha - Math.Floor(alpha / 4);
            double b = a + 1524; double c = Math.Floor((b - 122.1) / 365.25);
            double d = Math.Floor(365.25 * c); double e = Math.Floor((b - d) / 30.6001);
            double day = b - d - Math.Floor(30.6001 * e) + f;
            int mon = (int)(e < 14 ? e - 1 : e - 13);
            int yr = (int)(mon > 2 ? c - 4716 : c - 4715);
            int iday = (int)Math.Floor(day);
            DateTime dt = new DateTime(yr, mon, iday, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddDays(day - iday);
        }

        // Tempo siderale medio di Greenwich, gradi
        public static double Gmst(double jd)
        {
            double t = (jd - 2451545.0) / 36525.0;
            return Norm360(280.46061837 + 360.98564736629 * (jd - 2451545.0) + 0.000387933 * t * t - t * t * t / 38710000.0);
        }

        // Longitudine solare riferita all'equinozio J2000 (come usata dall'IMO), gradi
        public static double SolarLongitude(double jd)
        {
            double t = (jd - 2451545.0) / 36525.0;
            double l0 = 280.46646 + 36000.76983 * t + 0.0003032 * t * t;
            double m = (357.52911 + 35999.05029 * t - 0.0001537 * t * t) * D2R;
            double c = (1.914602 - 0.004817 * t - 0.000014 * t * t) * Math.Sin(m)
                     + (0.019993 - 0.000101 * t) * Math.Sin(2 * m) + 0.000289 * Math.Sin(3 * m);
            return Norm360(l0 + c - 1.397 * t);
        }

        // Istante (giorno giuliano) in cui il Sole ha longitudine lambda nell'anno indicato
        public static double JdOfSolarLongitude(double lambda, int year)
        {
            double jd = JulianDay(new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc)) + Norm360(lambda - 280.0) / 0.9856;
            for (int i = 0; i < 8; i++)
            {
                double d = Norm180(lambda - SolarLongitude(jd));
                jd += d / 0.98561;
            }
            return jd;
        }

        // ---------- locator ----------
        public static bool TryLocator(string loc, out LatLon p)
        {
            p = new LatLon(0, 0);
            if (loc == null) return false;
            string l = loc.Trim().ToUpperInvariant();
            if (l.Length != 4 && l.Length != 6 && l.Length != 8) return false;
            if (l[0] < 'A' || l[0] > 'R' || l[1] < 'A' || l[1] > 'R') return false;
            if (!char.IsDigit(l[2]) || !char.IsDigit(l[3])) return false;
            double lon = (l[0] - 'A') * 20 - 180 + (l[2] - '0') * 2;
            double lat = (l[1] - 'A') * 10 - 90 + (l[3] - '0');
            if (l.Length >= 6)
            {
                if (l[4] < 'A' || l[4] > 'X' || l[5] < 'A' || l[5] > 'X') return false;
                lon += (l[4] - 'A') / 12.0; lat += (l[5] - 'A') / 24.0;
                if (l.Length == 8)
                {
                    if (!char.IsDigit(l[6]) || !char.IsDigit(l[7])) return false;
                    lon += (l[6] - '0') / 120.0 + 1.0 / 240.0; lat += (l[7] - '0') / 240.0 + 1.0 / 480.0;
                }
                else { lon += 1.0 / 24.0; lat += 1.0 / 48.0; }
            }
            else { lon += 1.0; lat += 0.5; }
            p = new LatLon(lat, lon);
            return true;
        }

        public static string ToLocator(LatLon p)
        {
            double lon = p.Lon + 180.0, lat = p.Lat + 90.0;
            if (lon < 0) lon = 0; if (lon >= 360) lon = 359.9999;
            if (lat < 0) lat = 0; if (lat >= 180) lat = 179.9999;
            char a = (char)('A' + (int)(lon / 20)); char b = (char)('A' + (int)(lat / 10));
            char c = (char)('0' + (int)((lon % 20) / 2)); char d = (char)('0' + (int)(lat % 10));
            char e = (char)('A' + (int)((lon % 2) * 12)); char f = (char)('A' + (int)((lat % 1) * 24));
            return new string(new char[] { a, b, c, d, e, f });
        }

        // ---------- vettori ----------
        public static Vec3 Vec(LatLon p, double r)
        {
            double la = p.Lat * D2R, lo = p.Lon * D2R;
            return new Vec3(r * Math.Cos(la) * Math.Cos(lo), r * Math.Cos(la) * Math.Sin(lo), r * Math.Sin(la));
        }

        // Punto dell'ellissoide WGS84 (latitudine geodetica, quota sull'ellissoide in km) in coordinate terrestri (km)
        public static Vec3 Ecef(LatLon p, double altKm)
        {
            double la = p.Lat * D2R, lo = p.Lon * D2R, sl = Math.Sin(la);
            double n = WgsA / Math.Sqrt(1 - WgsE2 * sl * sl);
            return new Vec3((n + altKm) * Math.Cos(la) * Math.Cos(lo), (n + altKm) * Math.Cos(la) * Math.Sin(lo), (n * (1 - WgsE2) + altKm) * sl);
        }

        public static LatLon ToLatLon(Vec3 v)
        {
            Vec3 u = v.Unit;
            return new LatLon(Math.Asin(Math.Max(-1, Math.Min(1, u.Z))) / D2R, Math.Atan2(u.Y, u.X) / D2R);
        }

        // Versori locali: alto, est, nord
        public static void LocalFrame(LatLon p, out Vec3 up, out Vec3 east, out Vec3 north)
        {
            up = Vec(p, 1.0);
            east = new Vec3(-Math.Sin(p.Lon * D2R), Math.Cos(p.Lon * D2R), 0);
            north = up.Cross(east);
        }

        // Direzione (versore, terra fissa) di un punto celeste RA/Dec all'istante jd
        public static Vec3 RaDecToEcef(double raDeg, double decDeg, double jd)
        {
            double h = (raDeg - Gmst(jd)) * D2R, d = decDeg * D2R;
            return new Vec3(Math.Cos(d) * Math.Cos(h), Math.Cos(d) * Math.Sin(h), Math.Sin(d));
        }

        public static void AltAz(LatLon p, Vec3 s, out double azDeg, out double elDeg)
        {
            Vec3 up, e, n; LocalFrame(p, out up, out e, out n);
            elDeg = Math.Asin(Math.Max(-1, Math.Min(1, s.Dot(up)))) / D2R;
            azDeg = Norm360(Math.Atan2(s.Dot(e), s.Dot(n)) / D2R);
        }

        public static Vec3 FromAltAz(LatLon p, double azDeg, double elDeg)
        {
            Vec3 up, e, n; LocalFrame(p, out up, out e, out n);
            double a = azDeg * D2R, el = elDeg * D2R;
            return (Math.Cos(el) * Math.Cos(a)) * n + (Math.Cos(el) * Math.Sin(a)) * e + Math.Sin(el) * up;
        }

        // ---------- geodetica (ex cerchio massimo) ----------
        // Geodetica dell'ellissoide WGS84 con il metodo di Karney (Journal of Geodesy 87, 2013), libreria GeographicLib
        public static double DistanceKm(LatLon a, LatLon b)
        {
            double s12;
            Geod.Inverse(a.Lat, a.Lon, b.Lat, b.Lon, out s12);
            return s12 / 1000.0;
        }

        public static double Bearing(LatLon a, LatLon b)
        {
            double azi1, azi2;
            Geod.Inverse(a.Lat, a.Lon, b.Lat, b.Lon, out azi1, out azi2);
            return double.IsNaN(azi1) ? 0 : Norm360(azi1);
        }

        public static LatLon Destination(LatLon a, double bearingDeg, double km)
        {
            double la2, lo2;
            Geod.Direct(a.Lat, a.Lon, bearingDeg, km * 1000.0, out la2, out lo2);
            return new LatLon(la2, Norm180(lo2));
        }

        public static string F(double v, int dec)
        {
            return v.ToString("F" + dec, CultureInfo.CurrentCulture);
        }
    }
}
