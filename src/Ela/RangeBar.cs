// E-LAYER ANALYZER - cursore della distanza massima (raggio) della mappa azimutale, in scala logaritmica,
// con l'indicazione dei km dal centro; resta allineato con lo zoom fatto con la rotella sulla mappa.
using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using YAMS;

namespace ELA
{
    public sealed class RangeBar
    {
        public readonly Border Root;
        readonly Slider sld;
        readonly TextBlock lbl;
        readonly MapView map;
        bool busy;
        public event Action<double> Changed;

        public RangeBar(MapView m, double km)
        {
            map = m;
            sld = new Slider { Minimum = 0, Maximum = 1000, Width = 300, Margin = new Thickness(6, 0), VerticalAlignment = VerticalAlignment.Center };
            lbl = new TextBlock { Foreground = Pal.B(Pal.Yellow), FontWeight = FontWeight.SemiBold, VerticalAlignment = VerticalAlignment.Center, MinWidth = 170 };
            sld.PropertyChanged += (s, e) =>
            {
                if (e.Property != RangeBase.ValueProperty || busy) return;
                Apply(FromSlider(sld.Value), false);
            };
            map.RangeChanged += r => Apply(r, true);
            Root = Ui.Row(Ui.L(Lang.T("Distanza massima")), sld, lbl);
            Apply(km, true);
        }

        double FromSlider(double v) { return map.MinRangeKm * Math.Pow(map.MaxRangeKm / map.MinRangeKm, v / 1000.0); }
        double ToSlider(double km) { return 1000.0 * Math.Log(km / map.MinRangeKm) / Math.Log(map.MaxRangeKm / map.MinRangeKm); }

        public double Km { get { return map.RangeKm; } }

        public void Apply(double km, bool moveSlider)
        {
            km = Math.Max(map.MinRangeKm, Math.Min(map.MaxRangeKm, km));
            km = km >= 2000 ? Math.Round(km / 100) * 100 : Math.Round(km / 10) * 10;
            map.RangeKm = km;
            if (moveSlider) { busy = true; sld.Value = Math.Max(0, Math.Min(1000, ToSlider(km))); busy = false; }
            lbl.Text = Lang.F("{0:N0} km dal centro", km);
            map.InvalidateVisual();
            map.NotifyView();
            Action<double> c = Changed; if (c != null) c(km);
        }
    }
}
