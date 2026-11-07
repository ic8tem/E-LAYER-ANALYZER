// E-LAYER ANALYZER - linguette FONTI, SINTESI DELLE SCELTE DI CALCOLO, MANUALE D'USO e RELAZIONE TECNICA: foglio chiaro
// come in YA-MS (stesso formato di testo), con esportazione in Word.
using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using YAMS;

namespace ELA
{
    public static class DocTab
    {
        public static Control Build(string txtFile, string docxFile, string exportName, string note)
        {
            DockPanel p = new DockPanel { Background = Pal.B(Pal.Back) };
            StackPanel doc = new StackPanel();
            YAMS.MainWindow.FillExplain(doc, txtFile);
            if (docxFile != null)
            {
                Button bExp = Ui.B(Lang.T("Esporta in Word (.docx)..."));
                bExp.Click += async (s, e) =>
                {
                    try
                    {
                        if (!File.Exists(docxFile)) { await Dialogs.Message(TopLevel.GetTopLevel(p) as Window, Lang.T("Esporta"), Lang.F("File mancante: {0}", docxFile)); return; }
                        TopLevel tl = TopLevel.GetTopLevel(p);
                        IStorageFile f = await tl.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                        {
                            Title = Lang.T("Esporta il documento in Word"),
                            SuggestedFileName = exportName,
                            DefaultExtension = "docx",
                            FileTypeChoices = new[] { new FilePickerFileType(Lang.T("Documento Word")) { Patterns = new[] { "*.docx" } } }
                        });
                        if (f == null) return;
                        string dest = f.TryGetLocalPath();
                        if (dest != null) File.Copy(docxFile, dest, true);
                        else
                            using (Stream dst = await f.OpenWriteAsync())
                            using (FileStream src = File.OpenRead(docxFile))
                                await src.CopyToAsync(dst);
                    }
                    catch (Exception ex) { await Dialogs.Message(TopLevel.GetTopLevel(p) as Window, Lang.T("Esporta"), ex.Message); }
                };
                Button bOpen = Ui.B(Lang.T("Apri in Word"));
                bOpen.Click += (s, e) => { try { Dialogs.OpenWithDefaultApp(docxFile); } catch { } };
                Ui.Dock(p, Ui.Row(bExp, bOpen, Ui.L(note)), Dock.Top);
            }
            else Ui.Dock(p, Ui.Row(Ui.L(note)), Dock.Top);
            Border sheet = new Border
            {
                Background = Brushes.White,
                BorderBrush = Pal.B(Pal.Rgb(200, 200, 210)),
                BorderThickness = new Thickness(1),
                MaxWidth = 900,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(56, 40, 40, 32),
                Margin = new Thickness(20, 16, 20, 24),
                BoxShadow = new BoxShadows(new BoxShadow { OffsetX = 3, OffsetY = 4, Blur = 18, Color = Pal.Argb(160, 0, 0, 0) }),
                Child = doc
            };
            p.Children.Add(new ScrollViewer
            {
                Background = Pal.B(Pal.Desk),
                HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
                Content = sheet
            });
            return p;
        }
    }
}
