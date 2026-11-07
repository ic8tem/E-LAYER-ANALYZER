// E-LAYER ANALYZER - mattoncini dell'interfaccia (stesso stile "Scia meteorica" di YA-MS) e linguette a pillola
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using YAMS;

namespace ELA
{
    public static class Ui
    {
        public static TextBlock L(string t)
        {
            TextBlock l = new TextBlock { Text = t };
            l.Classes.Add("label");
            return l;
        }

        public static TextBlock Title(string t, Color c)
        {
            return new TextBlock { Text = t, Foreground = Pal.B(c), FontWeight = FontWeight.SemiBold, FontSize = 14, Margin = new Thickness(4, 4, 4, 2), TextWrapping = TextWrapping.Wrap };
        }

        public static NumericUpDown N(decimal min, decimal max, decimal val, int dec, decimal inc, double width)
        {
            NumericUpDown n = new NumericUpDown
            {
                Minimum = min, Maximum = max, Increment = inc,
                FormatString = "F" + dec.ToString(CultureInfo.InvariantCulture),
                NumberFormat = CultureInfo.CurrentCulture.NumberFormat,
                ClipValueToMinMax = true,
                Width = width + 80,
                Margin = new Thickness(2, 3),
                VerticalAlignment = VerticalAlignment.Center,
                Background = Pal.B(Pal.Back),
                Foreground = Pal.B(Pal.Fore),
                CornerRadius = new CornerRadius(12)
            };
            n.Value = Math.Max(min, Math.Min(max, val));
            return n;
        }

        public static double V(NumericUpDown n) { return (double)(n.Value ?? n.Minimum); }
        public static decimal Dm(double v) { if (double.IsNaN(v) || double.IsInfinity(v) || Math.Abs(v) > 1e9) return 0; return (decimal)v; }

        public static TextBox T(string val, double width, bool upper)
        {
            TextBox t = new TextBox
            {
                Text = val, Width = width,
                FontFamily = Pal.Mono, FontSize = 14,
                Margin = new Thickness(2, 3),
                VerticalAlignment = VerticalAlignment.Center,
                Background = Pal.B(Pal.Back),
                Foreground = Pal.B(Pal.Fore),
                CornerRadius = new CornerRadius(12)
            };
            if (upper)
                t.LostFocus += (s, e) => { string u = (t.Text ?? "").ToUpperInvariant(); if (u != t.Text) t.Text = u; };
            return t;
        }

        public static string Up(TextBox t) { return (t.Text ?? "").Trim().ToUpperInvariant(); }

        public static Button B(string t, bool accent = false)
        {
            Button b = new Button { Content = t, Margin = new Thickness(6, 2, 2, 2), VerticalAlignment = VerticalAlignment.Center };
            b.Classes.Add("pill");
            if (accent) b.Classes.Add("accent");
            return b;
        }

        public static CheckBox C(string t, bool val)
        {
            return new CheckBox { Content = t, IsChecked = val, Margin = new Thickness(6, 0, 4, 0), VerticalAlignment = VerticalAlignment.Center, Foreground = Pal.B(Pal.Fore) };
        }

        public static ComboBox Combo(IEnumerable<string> items, int sel, double minWidth = 90)
        {
            ComboBox c = new ComboBox { ItemsSource = new List<string>(items), MinWidth = minWidth, Margin = new Thickness(2, 3), VerticalAlignment = VerticalAlignment.Center };
            c.SelectedIndex = sel;
            return c;
        }

        // riga di comandi (fondo Panel2): gli elementi vanno a capo se lo spazio e' stretto
        public static Border Row(params Control[] items)
        {
            WrapPanel wp = new WrapPanel { Orientation = Orientation.Horizontal };
            foreach (Control c in items) { c.VerticalAlignment = VerticalAlignment.Center; wp.Children.Add(c); }
            return new Border { Background = Pal.B(Pal.Panel2), Padding = new Thickness(6, 2), Child = wp };
        }

        // riquadro con titolo
        public static Border Box(string title, Color tc, Control content)
        {
            DockPanel d = new DockPanel();
            if (!string.IsNullOrEmpty(title))
            {
                TextBlock t = Title(title, tc);
                DockPanel.SetDock(t, Avalonia.Controls.Dock.Top);
                d.Children.Add(t);
            }
            d.Children.Add(content);
            return new Border { Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(10), Padding = new Thickness(6), Margin = new Thickness(4), Child = d };
        }

        public static SelectableTextBlock Text(double size = 12.5)
        {
            return new SelectableTextBlock { FontFamily = Pal.Mono, FontSize = size, Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(4) };
        }

        public static void Dock(DockPanel p, Control c, Avalonia.Controls.Dock d)
        {
            DockPanel.SetDock(c, d);
            p.Children.Add(c);
        }

        // pagina con mappa a sinistra (65 %) e dati a destra (35 %), con divisore trascinabile
        public static Grid MapAndData(Control map, Control data)
        {
            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("65*,5,35*") };
            Grid.SetColumn(map, 0);
            GridSplitter sp = new GridSplitter { Background = Pal.B(Pal.Track), ResizeDirection = GridResizeDirection.Columns };
            Grid.SetColumn(sp, 1);
            Grid.SetColumn(data, 2);
            g.Children.Add(map); g.Children.Add(sp); g.Children.Add(data);
            return g;
        }
    }

    // Barra delle linguette a pillola (come YA-MS)
    public sealed class PillTabs
    {
        public readonly Border Bar;
        public readonly Grid Host = new Grid();
        readonly WrapPanel strip = new WrapPanel { Orientation = Orientation.Horizontal };
        public readonly DockPanel Line = new DockPanel();
        readonly List<ToggleButton> buttons = new List<ToggleButton>();
        readonly List<Control> pages = new List<Control>();
        public int Selected = -1;
        public int Count { get { return pages.Count; } }
        public event Action<int> SelectionChanged;

        public PillTabs(bool big = false)
        {
            Line.Children.Add(strip);
            Bar = new Border { Background = Pal.B(Pal.Track), Padding = new Thickness(8, big ? 7 : 4), Child = Line };
            this.big = big;
        }
        readonly bool big;

        public void Add(string title, Control page, Color? accent = null)
        {
            int idx = buttons.Count;
            ToggleButton b = new ToggleButton { Content = title, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 1, 4, 1) };
            b.Classes.Add("tab");
            if (big) { b.FontSize = 14; b.FontWeight = FontWeight.SemiBold; b.Padding = new Thickness(16, 5); }
            b.Click += (s, e) => Select(idx);
            buttons.Add(b);
            strip.Children.Add(b);
            page.IsVisible = false;
            pages.Add(page);
            Host.Children.Add(page);
            if (idx == 0) Select(0);
        }

        public void Select(int i)
        {
            if (i < 0 || i >= pages.Count) return;
            Selected = i;
            for (int k = 0; k < buttons.Count; k++)
            {
                buttons[k].IsChecked = k == i;
                pages[k].IsVisible = k == i;
            }
            Action<int> a = SelectionChanged;
            if (a != null) a(i);
        }
    }
}
