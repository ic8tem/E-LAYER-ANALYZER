// YA-MS - tabella disegnata (sostituisce ListView e DataGridView del vecchio programma)
// Intestazione chiara (sfondo quasi bianco, testo nero), righe su fondo scuro, colori per riga e per cella,
// riga selezionata evidenziata, scorrimento verticale con la rotellina e indicatore sottile a destra.
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Threading;

namespace YAMS
{
    public class TableRow
    {
        public string[] Cells;
        public Color? Fore;          // colore del testo della riga (null = Pal.Fore)
        public Color? Back;          // velatura di evidenziazione della riga (null = nessuna)
        public Color?[] CellBack;    // sfondo per cella (null = fondo della riga)
        public Color?[] CellFore;    // colore del testo per cella (null = colore della riga)
        public bool Italic;
        public bool Bold;
        public double FontSize = 12;
        public object Tag;
        public string[] CellTips;    // fumetto per cella al passaggio del mouse (null o "" = nessuno)
        public bool Blink;           // la riga lampeggia sempre (spot da notare: fondo Back pieno a fasi alterne)
    }

    public class TableView : Control
    {
        public List<string> Columns = new List<string>();
        public string[] HeaderTips;   // fumetto delle intestazioni di colonna (null o "" = nessuno)
        public List<double> ColumnWidths = new List<double>();   // px; l'ultima colonna puo' riempire
        public List<TableRow> Rows = new List<TableRow>();
        public int SelectedIndex = -1;
        public bool Selectable = true;
        public double RowHeight = 20;
        public double HeaderHeight = 24;
        public event Action<int> RowClicked;   // indice della riga cliccata (dopo aver impostato SelectedIndex)

        // opzioni in piu' (facoltative)
        public bool StretchLastColumn = true;                  // l'ultima colonna arriva fino al bordo destro
        public List<int> ColumnAlign = new List<int>();        // per colonna: 0 = sinistra, 1 = centro, 2 = destra
        public double HeaderFontSize = 12;
        // FitWidth: la tabella occupa esattamente la larghezza disponibile; larghezze delle colonne, altezze
        // delle righe e caratteri crescono (o calano) tutti dello stesso fattore. Senza FitWidth la scala e'
        // quella del cursore "Carattere". RowHeight, HeaderHeight, ColumnWidths e FontSize restano i valori di base.
        public bool FitWidth;
        double measuredWidth = double.NaN;

        double scrollY = 0;
        const double DefaultColumnWidth = 80;
        const double CellPad = 5;

        // intestazione: colori dello schema scelto (Pal.TableHead*), letti a ogni disegno
        static Color HeaderBack { get { return Pal.TableHead; } }
        static Color HeaderLine { get { return Pal.TableHeadLine; } }
        static Color HeaderFore { get { return Pal.TableHeadFore; } }

        readonly DispatcherTimer blinkTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        bool blinkOn = true;

        public TableView()
        {
            ClipToBounds = true;
            Focusable = true;
            blinkTimer.Tick += (s, e) =>
            {
                bool any = false;
                if (Rows != null) foreach (TableRow r in Rows) if (r != null && r.Blink) { any = true; break; }
                if (!any) { blinkOn = true; return; }
                blinkOn = !blinkOn;
                if (IsEffectivelyVisible) InvalidateVisual();
            };
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            UiScale.Changed += OnScaleChanged;
            blinkTimer.Start();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            UiScale.Changed -= OnScaleChanged;
            blinkTimer.Stop();
        }

        void OnScaleChanged() { ClampScroll(); InvalidateMeasure(); InvalidateVisual(); }

        // somma delle larghezze di base delle colonne
        double BaseWidthSum()
        {
            double sum = 0;
            foreach (double x in Widths(0, 1)) sum += x;
            return sum;
        }

        // fattore di scala attuale (caratteri, righe, colonne)
        double Factor()
        {
            if (FitWidth)
            {
                double w = Bounds.Width > 1 ? Bounds.Width : measuredWidth;
                double sum = BaseWidthSum();
                if (!double.IsNaN(w) && w > 1 && sum > 1) return w / sum;
            }
            return UiScale.Font;
        }

        double RH { get { return RowHeight * Factor(); } }
        double HH { get { return HeaderHeight * Factor(); } }

        // ridisegna e ricalcola lo scorrimento
        public void Refresh()
        {
            ClampScroll();
            InvalidateMeasure();
            InvalidateVisual();
        }

        public void EnsureVisible(int row)
        {
            if (Rows == null || row < 0 || row >= Rows.Count) return;
            double body = BodyHeight();
            double top = row * RH, bottom = top + RH;
            if (top < scrollY) scrollY = top;
            else if (bottom > scrollY + body) scrollY = bottom - body;
            ClampScroll();
            InvalidateVisual();
        }

        // porta la riga in cima alla parte visibile della tabella
        public void ScrollToRow(int row)
        {
            if (Rows == null || row < 0 || row >= Rows.Count) return;
            scrollY = row * RH;
            ClampScroll();
            InvalidateVisual();
        }

        double BodyHeight() { return Math.Max(0, Bounds.Height - HH); }

        double ContentHeight() { return (Rows != null ? Rows.Count : 0) * RH; }

        double MaxScroll() { return Math.Max(0, ContentHeight() - BodyHeight()); }

        void ClampScroll()
        {
            double m = MaxScroll();
            if (scrollY > m) scrollY = m;
            if (scrollY < 0) scrollY = 0;
        }

        int ColumnCount()
        {
            int n = Columns != null ? Columns.Count : 0;
            if (Rows != null) foreach (TableRow r in Rows) if (r != null && r.Cells != null) n = Math.Max(n, r.Cells.Length);
            return n;
        }

        // larghezze effettive delle colonne (l'ultima puo' riempire lo spazio rimasto)
        double[] Widths(double total, double k)
        {
            int n = ColumnCount();
            double[] w = new double[n];
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                w[i] = (ColumnWidths != null && i < ColumnWidths.Count && ColumnWidths[i] > 0 ? ColumnWidths[i] : DefaultColumnWidth) * k;
                sum += w[i];
            }
            if (StretchLastColumn && n > 0 && sum < total) w[n - 1] += total - sum;
            return w;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double k = UiScale.Font;
            if (FitWidth && !double.IsInfinity(availableSize.Width) && availableSize.Width > 1)
            {
                measuredWidth = availableSize.Width;
                double sum = BaseWidthSum();
                if (sum > 1) k = availableSize.Width / sum;
            }
            double[] w = Widths(0, k);
            double sw = 0; foreach (double x in w) sw += x;
            if (FitWidth && !double.IsInfinity(availableSize.Width)) sw = availableSize.Width;
            double h = (HeaderHeight + (Rows != null ? Rows.Count : 0) * RowHeight) * k;   // l'altezza segue le righe scalate
            if (!double.IsInfinity(availableSize.Width)) sw = Math.Min(sw, availableSize.Width);
            if (!double.IsInfinity(availableSize.Height)) h = Math.Min(h, availableSize.Height);
            return new Size(sw, h);
        }

        static FormattedText Ft(string s, double size, Color c, bool bold, bool italic)
        {
            return new FormattedText(s ?? "", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(FontFamily.Default, italic ? FontStyle.Italic : FontStyle.Normal, bold ? FontWeight.Bold : FontWeight.Normal),
                size, new ImmutableSolidColorBrush(c));
        }

        int Align(int col) { return ColumnAlign != null && col < ColumnAlign.Count ? ColumnAlign[col] : 0; }

        void DrawCellText(DrawingContext ctx, FormattedText ft, Rect cell, int align)
        {
            double x;
            if (align == 1) x = cell.X + (cell.Width - ft.Width) / 2;
            else if (align == 2) x = cell.Right - CellPad - ft.Width;
            else x = cell.X + CellPad;
            if (align != 0 && x < cell.X + 1) x = cell.X + 1;
            double y = cell.Y + (cell.Height - ft.Height) / 2;
            using (ctx.PushClip(cell))
                ctx.DrawText(ft, new Point(x, y));
        }

        public override void Render(DrawingContext ctx)
        {
            double Wd = Bounds.Width, Ht = Bounds.Height;
            ctx.DrawRectangle(Pal.B(Pal.Back), null, new Rect(0, 0, Wd, Ht));
            if (Wd < 2 || Ht < 2) return;
            double k = Factor();
            double rh = RowHeight * k, hh = HeaderHeight * k;
            ClampScroll();
            double[] w = Widths(Wd, k);
            int n = w.Length;

            // righe
            double bodyTop = hh, bodyH = Math.Max(0, Ht - hh);
            if (Rows != null && Rows.Count > 0 && bodyH > 0)
            {
                using (ctx.PushClip(new Rect(0, bodyTop, Wd, bodyH)))
                {
                    int first = Math.Max(0, (int)Math.Floor(scrollY / rh));
                    int last = Math.Min(Rows.Count - 1, (int)Math.Ceiling((scrollY + bodyH) / rh));
                    IBrush odd = Pal.B(Pal.Panel);
                    IBrush selB = Pal.B(Pal.Select);
                    IPen selP = Pal.P(Pal.Azure, 1);
                    for (int r = first; r <= last; r++)
                    {
                        TableRow row = Rows[r];
                        if (row == null) continue;
                        double y = bodyTop + r * rh - scrollY;
                        Rect rr = new Rect(0, y, Wd, rh);
                        bool sel = Selectable && r == SelectedIndex;
                        if (sel) ctx.DrawRectangle(selB, null, rr);
                        else if (r % 2 == 1) ctx.DrawRectangle(odd, null, rr);
                        bool flash = row.Blink && blinkOn;
                        if (!sel && row.Back.HasValue && (!row.Blink || flash)) ctx.DrawRectangle(Pal.B(row.Back.Value), null, rr);
                        double x = 0;
                        Color rowFore = row.Blink && flash ? Color.FromRgb(20, 83, 45) : (row.Fore ?? Pal.Fore);
                        double fs = (row.FontSize > 0 ? row.FontSize : 12) * k;
                        for (int c = 0; c < n; c++)
                        {
                            Rect cell = new Rect(x, y, w[c], rh);
                            x += w[c];
                            if (row.CellBack != null && c < row.CellBack.Length && row.CellBack[c].HasValue)
                                ctx.DrawRectangle(Pal.B(row.CellBack[c].Value), null, new Rect(cell.X + 1, cell.Y + 1, Math.Max(0, cell.Width - 2), Math.Max(0, cell.Height - 2)));
                            if (row.Cells == null || c >= row.Cells.Length || string.IsNullOrEmpty(row.Cells[c])) continue;
                            Color fc = row.CellFore != null && c < row.CellFore.Length && row.CellFore[c].HasValue ? row.CellFore[c].Value : rowFore;
                            DrawCellText(ctx, Ft(row.Cells[c], fs, fc, row.Bold || row.Blink, row.Italic), cell, Align(c));
                        }
                        if (sel) ctx.DrawRectangle(null, selP, new Rect(0.5, y + 0.5, Math.Max(0, Wd - 1), Math.Max(0, rh - 1)));
                    }
                }
                // indicatore di scorrimento
                double ch = ContentHeight();
                if (ch > bodyH + 0.5)
                {
                    double th = Math.Max(20, bodyH * bodyH / ch);
                    double ty = bodyTop + (bodyH - th) * (scrollY / Math.Max(1, ch - bodyH));
                    ctx.DrawRectangle(Pal.B(Pal.A(60, Pal.Muted)), null, new Rect(Wd - 5, bodyTop, 4, bodyH), 2, 2);
                    ctx.DrawRectangle(Pal.B(Pal.A(190, Pal.Muted)), null, new Rect(Wd - 5, ty, 4, th), 2, 2);
                }
            }

            // intestazione
            if (hh > 0)
            {
                ctx.DrawRectangle(Pal.B(HeaderBack), null, new Rect(0, 0, Wd, hh));
                IPen hl = Pal.P(HeaderLine, 1);
                double x = 0;
                for (int c = 0; c < n; c++)
                {
                    Rect cell = new Rect(x, 0, w[c], hh);
                    x += w[c];
                    if (Columns != null && c < Columns.Count && !string.IsNullOrEmpty(Columns[c]))
                        DrawCellText(ctx, Ft(Columns[c], HeaderFontSize * k, HeaderFore, false, false), cell, Align(c));
                    if (c < n - 1) ctx.DrawLine(hl, new Point(Math.Round(x) - 0.5, 4), new Point(Math.Round(x) - 0.5, hh - 4));
                }
                ctx.DrawLine(hl, new Point(0, hh - 0.5), new Point(Wd, hh - 0.5));
            }
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            double m = MaxScroll();
            if (m <= 0 || e.Delta.Y == 0) return;   // niente da scorrere: la rotellina passa al contenitore
            double old = scrollY;
            scrollY -= e.Delta.Y * RH * 3;
            ClampScroll();
            if (scrollY != old) { e.Handled = true; InvalidateVisual(); }
        }

        // fumetto della cella sotto il mouse (TableRow.CellTips)
        string shownTip;

        string TipAt(Point p)
        {
            double hh = HH;
            if (p.Y < hh && HeaderTips != null)
            {
                double[] hw = Widths(Bounds.Width, Factor());
                double hx = 0;
                for (int c = 0; c < hw.Length; c++)
                {
                    if (p.X >= hx && p.X < hx + hw[c]) return c < HeaderTips.Length && !string.IsNullOrEmpty(HeaderTips[c]) ? HeaderTips[c] : null;
                    hx += hw[c];
                }
                return null;
            }
            if (p.Y < hh || Rows == null) return null;
            int idx = (int)Math.Floor((p.Y - hh + scrollY) / RH);
            if (idx < 0 || idx >= Rows.Count || Rows[idx] == null || Rows[idx].CellTips == null) return null;
            double[] w = Widths(Bounds.Width, Factor());
            double x = 0;
            for (int c = 0; c < w.Length; c++)
            {
                if (p.X >= x && p.X < x + w[c])
                    return c < Rows[idx].CellTips.Length && !string.IsNullOrEmpty(Rows[idx].CellTips[c]) ? Rows[idx].CellTips[c] : null;
                x += w[c];
            }
            return null;
        }

        void SetTip(string tip)
        {
            if (tip == shownTip) return;
            shownTip = tip;
            ToolTip.SetIsOpen(this, false);
            ToolTip.SetTip(this, tip);
            if (tip != null) ToolTip.SetIsOpen(this, true);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            SetTip(TipAt(e.GetPosition(this)));
        }

        protected override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);
            SetTip(null);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
            Point p = e.GetPosition(this);
            double hh = HH;
            if (p.Y < hh || Rows == null) return;
            int idx = (int)Math.Floor((p.Y - hh + scrollY) / RH);
            if (idx < 0 || idx >= Rows.Count) return;
            if (Selectable) SelectedIndex = idx;
            InvalidateVisual();
            e.Handled = true;
            RowClicked?.Invoke(idx);
        }
    }
}
