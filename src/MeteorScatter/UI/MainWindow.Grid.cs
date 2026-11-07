// YA-MS - linguetta "Mappa quadrati": quadrati Maidenhead lavorati letti da un file ADIF (nessun collegamento a LoTW)
// Il caricamento di un nuovo ADIF sostituisce i dati precedenti; i dati restano in data/adif_quadrati.csv.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform.Storage;

namespace YAMS
{
    public partial class MainWindow
    {
        List<GridMapView> gridViews = new List<GridMapView>();
        TextBlock lblAdif;

        void BuildGridTab()
        {
            DockPanel p = Page(Lang.T("Mappa quadrati"));
            Button bLoad = B(Lang.T("Carica file ADIF..."));
            Button bClear = B(Lang.T("Cancella memoria"));
            bClear.Background = Pal.B(Pal.Rgb(127, 29, 29)); bClear.Foreground = Pal.B(Pal.Rgb(255, 255, 255));   // rosso scuro con testo bianco in tutti gli schemi
            lblAdif = L("");
            bLoad.Click += async (s, e) => await LoadAdifFile();
            bClear.Click += async (s, e) =>
            {
                try
                {
                    if (!await Dialogs.Confirm(Win, Lang.T("Cancella memoria"), Lang.T("Cancellare tutti i quadrati caricati dal file ADIF?"))) return;
                    AdifStore.Clear(dataDir);
                    RefreshGrids();
                }
                catch (Exception ex) { await Dialogs.Message(Win, Lang.T("Cancella memoria"), ex.Message); }
            };
            DockTop(p, Row(bLoad, bClear, lblAdif, L(Lang.T("Rotellina = zoom, trascina = sposta, mouse sopra un quadrato = QSO"))));
            // sotto-linguette a pillola, una per banda
            PillTabs sub = new PillTabs();
            for (int i = 0; i < AdifStore.Bands.Length; i++)
            {
                GridMapView v = new GridMapView();
                v.Band = AdifStore.Bands[i];
                sub.Add(AdifStore.BandNames[i], v);
                gridViews.Add(v);
            }
            DockTop(p, sub.Bar);
            p.Children.Add(sub.Host);
            try { AdifStore.Load(dataDir); } catch { }
            RefreshGrids();
        }

        async Task LoadAdifFile()
        {
            try
            {
                IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = Lang.T("File ADIF del log"),
                    AllowMultiple = false,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType(Lang.T("File ADIF")) { Patterns = new[] { "*.adi", "*.adif", "*.ADI", "*.ADIF" } },
                        new FilePickerFileType(Lang.T("Tutti i file")) { Patterns = new[] { "*" } }
                    }
                });
                if (files == null || files.Count == 0) return;
                string path = files[0].TryGetLocalPath();
                if (path == null) { await Dialogs.Message(Win, Lang.T("File ADIF"), Lang.T("Il file scelto non e' un file locale.")); return; }
                try
                {
                    string msg;
                    AdifStore.LoadAdif(path, out msg);   // sostituisce i dati precedenti
                    AdifStore.Save(dataDir);
                    status.Text = msg;
                }
                catch (Exception ex) { await Dialogs.Message(Win, Lang.T("File ADIF"), ex.Message); }
                RefreshGrids();
            }
            catch (Exception ex) { await Dialogs.Message(Win, Lang.T("File ADIF"), ex.Message); }
        }

        void RefreshGrids()
        {
            if (lblAdif == null) return;
            lblAdif.Text = AdifStore.Qsos.Count == 0 ? Lang.T("nessun ADIF caricato") : Lang.F("ADIF: {0} ({1} quadrati-QSO)", AdifStore.SourceName, AdifStore.Qsos.Count);
            string home = txtHome != null ? Up(txtHome) : "";
            foreach (GridMapView v in gridViews)
            {
                v.HomeGrid = home.Length >= 4 ? home.Substring(0, 4) : "";
                if (sky != null) { v.Coast = sky.MapLines; v.Borders = sky.MapBorders; }
                v.InvalidateVisual();
            }
        }
    }
}
