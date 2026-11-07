// E-LAYER ANALYZER - modello dei salti di E-sporadico
// Numero di salti e giudizio sulla distanza: tabella di Flavio Egano IK3XTV (singolo, doppio, triplo salto).
// Punti di riflessione: salti uguali lungo il cerchio massimo; primo salto di ciascuna stazione a D/(2n) da essa.
// Ipotesi "primo salto corto" (esperienza dell'autore IC8TEM): nel multi-salto il primo salto di una stazione puo'
// essere corto, entro ~1200 km dal proprio QTH; il resto della tratta si divide in salti uguali.
// foEs minima: legge della secante sulla Terra sferica, foEs >= f / sec(phi) (phi = angolo di incidenza sullo strato).
using System;
using System.Collections.Generic;

namespace ELA
{
    public class EsHop { public double Lat, Lon; public int Index; }

    public class EsPath
    {
        public Spot Spot;
        public double DistKm;
        public int Hops;            // 0 = troppo corto per l'E-sporadico
        public double HopKm, SecPhi, FoEsMin, ElevDeg;
        public string Judgement = "";
        public List<EsHop> Points = new List<EsHop>();
        public EsHop ShortA, ShortB;     // ipotesi del primo salto corto (punto di riflessione) per spotter e DX
        public double ShortFoEsMin;      // foEs minima per il salto corto
        public EsHop FirstA { get { return Points.Count > 0 ? Points[0] : null; } }               // primo salto dello spotter
        public EsHop FirstB { get { return Points.Count > 0 ? Points[Points.Count - 1] : null; } } // primo salto del DX
    }

    public static class EsModel
    {
        // Tabella IK3XTV (km)
        public static readonly double[] SingleMin = { 500, 650 }, SingleOpt = { 1500, 2000 }, SingleMax = { 2200, 2400 };
        public static readonly double[] DoubleMin = { 2800, 3000 }, DoubleOpt = { 3200, 4000 }, DoubleMax = { 4400, 5000 };
        public static readonly double[] TripleOpt = { 4800, 6400 }; public const double TripleMax = 6900;

        public static int HopsFor(double d)
        {
            if (d < SingleMin[0]) return 0;
            if (d <= SingleMax[1]) return 1;
            if (d <= DoubleMax[1]) return 2;
            if (d <= TripleMax) return 3;
            return (int)Math.Ceiling(d / 2300.0);
        }

        public static string Judge(double d)
        {
            if (d < SingleMin[0]) return "sotto la distanza minima dell'Es (tropo o Es a MUF molto alta)";
            if (d < SingleMin[1]) return "singolo salto, vicino alla distanza minima";
            if (d < SingleOpt[0]) return "singolo salto";
            if (d <= SingleOpt[1]) return "singolo salto, distanza ottimale";
            if (d <= SingleMax[1]) return "singolo salto, vicino alla distanza massima";
            if (d < DoubleMin[0]) return "tra singolo e doppio salto (zona di passaggio)";
            if (d < DoubleOpt[0]) return "doppio salto";
            if (d <= DoubleOpt[1]) return "doppio salto, distanza ottimale";
            if (d <= DoubleMax[1]) return "doppio salto, vicino alla distanza massima";
            if (d < TripleOpt[0]) return "triplo salto";
            if (d <= TripleOpt[1]) return "triplo salto, distanza ottimale";
            if (d <= TripleMax) return "triplo salto, vicino alla distanza massima";
            return "oltre il triplo salto (ipotesi di salti multipli o Es + F2)";
        }

        public static EsPath Build(Spot s, double Hkm)
        {
            if (s == null || !s.HasBoth) return null;
            EsPath p = new EsPath { Spot = s };
            p.DistKm = s.DistanceKm;
            p.Hops = HopsFor(p.DistKm);
            p.Judgement = Judge(p.DistKm);
            int n = Math.Max(1, p.Hops);
            p.HopKm = p.DistKm / n;
            p.SecPhi = Geo.SecantFactor(p.HopKm, Hkm);
            p.FoEsMin = s.FreqMHz / p.SecPhi;
            p.ElevDeg = Geo.ElevationDeg(p.HopKm / 2, Hkm, 0);
            for (int k = 1; k <= n; k++)
            {
                double la, lo;
                Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, (2.0 * k - 1) / (2.0 * n), out la, out lo);
                p.Points.Add(new EsHop { Lat = la, Lon = lo, Index = k });
            }
            if (p.Hops >= 2 && ShortHopKm > 0 && ShortHopKm < p.HopKm)
            {
                double f = (ShortHopKm / 2) / p.DistKm, la, lo;
                Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, f, out la, out lo);
                p.ShortA = new EsHop { Lat = la, Lon = lo, Index = 1 };
                Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 1 - f, out la, out lo);
                p.ShortB = new EsHop { Lat = la, Lon = lo, Index = n };
                p.ShortFoEsMin = s.FreqMHz / Geo.SecantFactor(ShortHopKm, Hkm);
            }
            return p;
        }

        // lunghezza del primo salto corto (km); 0 = ipotesi spenta
        public static double ShortHopKm = 1200;

        // MUF della propria stazione attraverso una nube di foEs nota posta a distanza dKm (salto di lunghezza 2d)
        public static double MufVia(double foEs, double dKm, double Hkm) { return foEs * Geo.SecantFactor(2 * dKm, Hkm); }
    }
}
