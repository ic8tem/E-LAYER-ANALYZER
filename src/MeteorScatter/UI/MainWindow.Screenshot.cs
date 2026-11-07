// YA-MS - fotografie delle linguette per il controllo automatico su GitHub (opzione nascosta --screenshot)
using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;

namespace YAMS
{
    public partial class MainWindow
    {
        // --screenshot <cartella> [--dx LOCATOR] [--ore H] [--carattere N]: imposta la tratta, sposta la volta celeste
        // di H ore (passi di 15 minuti), porta il carattere a N % (100-300), fotografa ogni linguetta e chiude il programma.
        public async Task RunScreenshots(string dir, string dx, double hours, Visual target)
        {
            try
            {
                Directory.CreateDirectory(dir);
                await Task.Delay(4000);
                if (Program.ScreenshotFontPct > 0)
                {
                    double pct = Math.Max(100, Math.Min(300, Math.Round(Program.ScreenshotFontPct / 10) * 10));
                    sldFont.Value = pct;
                    UiScale.Set(pct / 100.0);   // anche se il cursore non cambia valore
                    ApplyFontScale();
                    if (pct >= 200) SetRowsVisible(false, false);   // con caratteri grandi: comandi ripiegati
                }
                if (!string.IsNullOrEmpty(dx)) { txtDx.Text = dx; Recalc(); }
                trackTime.Value = Math.Max(trackTime.Minimum, Math.Min(trackTime.Maximum, Math.Round(hours * 4)));
                UpdateSky();
                // attende la mappa (Natural Earth) e i dati dei radar, al massimo 60 s
                for (int k = 0; k < 60 && (sky.MapLines == null || sky.MapLines.Count == 0); k++) await Task.Delay(1000);
                for (int k = 0; k < 60 && liveFetching; k++) await Task.Delay(1000);   // GMN e CMOR
                await Task.Delay(3000);
                for (int i = 0; i < tabs.Count; i++)   // tutte le linguette, qualunque sia il loro numero
                {
                    tabs.Select(i);
                    await Task.Delay(1500);
                    Rect bb = (target ?? this).Bounds;
                    PixelSize ps = new PixelSize(Math.Max(100, (int)bb.Width), Math.Max(100, (int)bb.Height));
                    using (RenderTargetBitmap rtb = new RenderTargetBitmap(ps, new Vector(96, 96)))
                    {
                        rtb.Render(target ?? this);
                        rtb.Save(Path.Combine(dir, "linguetta_" + (i + 1) + ".png"));
                    }
                }
            }
            catch (Exception ex)
            {
                try { File.WriteAllText(Path.Combine(dir, "errore.txt"), ex.ToString()); } catch { }
            }
        }
    }
}
