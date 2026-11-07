// YA-MS - linguetta "Efficienza - spiegazione" e caricamento della mappa
// Il documento sta in data/efficienza_spiegazione.txt (un blocco per riga: TITLE, SUB, H1, H2, P, TAG|FONTE|..., TAG|RILIEVO|...,
// TAG|SCELTA|..., EQ, LI, SRC) ed e' mostrato come un foglio chiaro; la versione per Word e' data/efficienza_spiegazione.docx.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace YAMS
{
    public partial class MainWindow
    {
        // colori del foglio
        static readonly Color DocInk = Pal.Rgb(30, 30, 42);
        static readonly Color DocTitle = Pal.Rgb(24, 32, 72);
        static readonly Color DocH1 = Pal.Rgb(31, 58, 120);
        static readonly Color DocH2 = Pal.Rgb(40, 40, 70);
        static readonly Color DocMuted = Pal.Rgb(100, 100, 120);
        static readonly Color TagFonte = Pal.Rgb(0x1F, 0x5F, 0xAD);
        static readonly Color TagRilievo = Pal.Rgb(0x2E, 0x7D, 0x32);
        static readonly Color TagScelta = Pal.Rgb(0xB4, 0x53, 0x09);
        static readonly FontFamily DocMath = new FontFamily("Cambria Math, Cambria, STIX Two Math, Times New Roman, DejaVu Serif, serif");

        void BuildExplainTab()
        {
            DockPanel p = Page(Lang.T("Efficienza - spiegazione"));
            string docx = Path.Combine(dataDir, "efficienza_spiegazione.docx");
            Button bExport = B(Lang.T("Esporta in Word (.docx)..."));
            bExport.Click += async (s, e) => await ExportDocx(docx);
            Button bOpen = B(Lang.T("Apri in Word"));
            bOpen.Click += async (s, e) =>
            {
                try
                {
                    if (!File.Exists(docx)) { await Dialogs.Message(Win, Lang.T("Apri in Word"), Lang.F("File mancante: {0}", docx)); return; }
                    Dialogs.OpenWithDefaultApp(docx);
                }
                catch (Exception ex) { await Dialogs.Message(Win, Lang.T("Apri in Word"), Lang.F("Non riesco ad aprire il documento: {0}", ex.Message)); }
            };
            DockTop(p, Row(bExport, bOpen,
                L(Lang.T("Capitoli: introduzione, strato E, geometria, radiante, riflessione, efficienza, lettura dei risultati, frequenza, limiti, fonti"))));

            // aspetto "pagina": foglio chiaro centrato, larghezza massima 860 px, ombra, sul fondo dello schema (Pal.Desk)
            StackPanel doc = new StackPanel();
            FillExplain(doc, Path.Combine(dataDir, "efficienza_spiegazione.txt"));
            Border sheet = new Border
            {
                Background = Brushes.White,
                BorderBrush = Pal.B(Pal.Rgb(200, 200, 210)),
                BorderThickness = new Thickness(1),
                MaxWidth = 860,
                HorizontalAlignment = HorizontalAlignment.Stretch,   // con MaxWidth il foglio resta centrato
                Padding = new Thickness(56, 40, 40, 32),
                Margin = new Thickness(20, 16, 20, 24),
                BoxShadow = new BoxShadows(new BoxShadow { OffsetX = 3, OffsetY = 4, Blur = 18, Color = Pal.Argb(160, 0, 0, 0) }),
                Child = doc
            };
            ScrollViewer sv = new ScrollViewer
            {
                Background = Pal.B(Pal.Desk),   // il foglio resta chiaro (carta) in tutti gli schemi
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = sheet
            };
            p.Children.Add(sv);
            explainSheet = sheet;
        }

        Border explainSheet;   // foglio del documento: piu' largo con il carattere ingrandito (ApplyFontScale)

        static SelectableTextBlock DocText(string text, double size, Color c, FontWeight w, FontStyle st, Thickness margin)
        {
            return new SelectableTextBlock
            {
                Text = text,
                FontSize = size,
                Foreground = Pal.B(c),
                FontWeight = w,
                FontStyle = st,
                Margin = margin,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = Math.Round(size * 1.45)
            };
        }

        // riga a due colonne: etichetta (o punto elenco, o numero) a sinistra, testo a destra
        static Grid DocHanging(Control left, Control right, Thickness margin)
        {
            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*"), Margin = margin };
            Grid.SetColumn(left, 0); Grid.SetColumn(right, 1);
            g.Children.Add(left); g.Children.Add(right);
            return g;
        }

        internal static void FillExplain(StackPanel doc, string file)
        {
            string[] lines;
            try
            {
                lines = File.Exists(file) ? File.ReadAllLines(file, Encoding.UTF8) : new[] { "P|" + Lang.F("File mancante: {0}", file) };
            }
            catch (Exception ex) { lines = new[] { "P|" + Lang.F("Impossibile leggere {0}: {1}", file, ex.Message) }; }

            foreach (string raw in lines)
            {
                if (raw.Trim().Length == 0) continue;
                int bar = raw.IndexOf('|');
                string kind = bar > 0 ? raw.Substring(0, bar).Trim().ToUpperInvariant() : "P";
                string text = bar > 0 ? raw.Substring(bar + 1) : raw;
                switch (kind)
                {
                    case "TITLE":
                        doc.Children.Add(DocText(text, 26, DocTitle, FontWeight.Bold, FontStyle.Normal, new Thickness(0, 0, 0, 4)));
                        break;
                    case "SUB":
                        doc.Children.Add(DocText(text, 13, DocMuted, FontWeight.Normal, FontStyle.Italic, new Thickness(0, 0, 0, 14)));
                        break;
                    case "H1":
                        doc.Children.Add(new Border
                        {
                            BorderBrush = Pal.B(Pal.Rgb(200, 205, 225)),
                            BorderThickness = new Thickness(0, 0, 0, 1),
                            Margin = new Thickness(0, 22, 0, 8),
                            Padding = new Thickness(0, 0, 0, 3),
                            Child = DocText(text, 20, DocH1, FontWeight.Bold, FontStyle.Normal, new Thickness(0))
                        });
                        break;
                    case "H2":
                        doc.Children.Add(DocText(text, 16, DocH2, FontWeight.SemiBold, FontStyle.Normal, new Thickness(0, 12, 0, 6)));
                        break;
                    case "TAG":
                        {
                            int b2 = text.IndexOf('|');
                            string tag = b2 > 0 ? text.Substring(0, b2).Trim().ToUpperInvariant() : "FONTE";
                            string body = b2 > 0 ? text.Substring(b2 + 1) : text;
                            Color tc = tag == "RILIEVO" ? TagRilievo : tag == "SCELTA" ? TagScelta : TagFonte;
                            Border pill = new Border
                            {
                                Background = Pal.B(tc),
                                CornerRadius = new CornerRadius(3),
                                Padding = new Thickness(6, 1),
                                Margin = new Thickness(0, 2, 10, 0),
                                MinWidth = 72,
                                VerticalAlignment = VerticalAlignment.Top,
                                Child = new TextBlock
                                {
                                    Text = Lang.T(tag),
                                    Foreground = Brushes.White,
                                    FontWeight = FontWeight.Bold,
                                    FontSize = 10.5,
                                    HorizontalAlignment = HorizontalAlignment.Center
                                }
                            };
                            doc.Children.Add(DocHanging(pill, DocText(body, 14, DocInk, FontWeight.Normal, FontStyle.Normal, new Thickness(0)), new Thickness(0, 0, 0, 8)));
                        }
                        break;
                    case "EQ":
                        {
                            SelectableTextBlock eq = DocText(text, 15, DocInk, FontWeight.Normal, FontStyle.Italic, new Thickness(0));
                            eq.FontFamily = DocMath;
                            eq.TextAlignment = TextAlignment.Center;
                            doc.Children.Add(new Border
                            {
                                Background = Pal.B(Pal.Rgb(244, 246, 252)),
                                CornerRadius = new CornerRadius(4),
                                Padding = new Thickness(10, 6),
                                Margin = new Thickness(24, 4, 24, 12),
                                Child = eq
                            });
                        }
                        break;
                    case "LI":
                        {
                            TextBlock dot = new TextBlock { Text = "•", FontSize = 14, Foreground = Pal.B(DocH1), Margin = new Thickness(0, 0, 10, 0), FontWeight = FontWeight.Bold };
                            doc.Children.Add(DocHanging(dot, DocText(text, 14, DocInk, FontWeight.Normal, FontStyle.Normal, new Thickness(0)), new Thickness(14, 0, 0, 6)));
                        }
                        break;
                    case "SRC":
                        {
                            // fonti numerate: "12. testo" -> numero in colonna, testo con rientro sporgente
                            Match m = Regex.Match(text, @"^\s*(\d+)\.\s*(.*)$");
                            string num = m.Success ? m.Groups[1].Value + "." : "";
                            string body = m.Success ? m.Groups[2].Value : text;
                            TextBlock n = new TextBlock { Text = num, Width = 30, FontSize = 13, Foreground = Pal.B(DocH1), FontWeight = FontWeight.Bold };
                            doc.Children.Add(DocHanging(n, DocText(body, 13, DocInk, FontWeight.Normal, FontStyle.Normal, new Thickness(0)), new Thickness(0, 0, 0, 5)));
                        }
                        break;
                    default:
                        doc.Children.Add(DocText(text, 14, DocInk, FontWeight.Normal, FontStyle.Normal, new Thickness(0, 0, 0, 8)));
                        break;
                }
            }
        }

        // Copia data/efficienza_spiegazione.docx dove sceglie l'utente
        async Task ExportDocx(string docx)
        {
            try
            {
                if (!File.Exists(docx)) { await Dialogs.Message(Win, Lang.T("Esporta"), Lang.F("File mancante: {0}", docx)); return; }
                IStorageFile f = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = Lang.T("Esporta il documento in Word"),
                    SuggestedFileName = "YA-MS - calcolo dell'efficienza.docx",
                    DefaultExtension = "docx",
                    FileTypeChoices = new[] { new FilePickerFileType(Lang.T("Documento Word")) { Patterns = new[] { "*.docx" } } }
                });
                if (f == null) return;
                string dest = f.TryGetLocalPath();
                if (dest != null) File.Copy(docx, dest, true);
                else
                {
                    using (Stream dst = await f.OpenWriteAsync())
                    using (FileStream src = File.OpenRead(docx))
                        await src.CopyToAsync(dst);
                }
                status.Text = Lang.F("Documento esportato: {0}", dest ?? f.Name);
            }
            catch (Exception ex) { await Dialogs.Message(Win, Lang.T("Esporta"), ex.Message); }
        }

        // Mappa d'Europa (Natural Earth): scaricata al primo avvio su un thread secondario, poi letta dal disco
        void StartMapLoad()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(_ =>
            {
                string msg = "";
                List<System.Drawing.PointF[]> coast = null, border = null;
                try
                {
                    MapData.EnsureFiles(dataDir, out msg);
                    coast = MapData.Load(MapData.CoastFile(dataDir));
                    border = MapData.Load(MapData.BorderFile(dataDir));
                }
                catch (Exception ex) { msg += " " + ex.Message; }
                if (coast == null) coast = new List<System.Drawing.PointF[]>();
                if (border == null) border = new List<System.Drawing.PointF[]>();
                string m2 = msg;
                Dispatcher.UIThread.Post(() =>
                {
                    sky.MapLines = coast; sky.MapBorders = border; sky.InvalidateMap();
                    RefreshGrids();
                    if (coast.Count == 0) status.Text = Lang.T("Mappa d'Europa non disponibile (serve la rete al primo avvio).") + " " + m2;
                });
            });
        }
    }
}
