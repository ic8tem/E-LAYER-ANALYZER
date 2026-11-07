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
                // schermata di avvio: fotografata a meta' della barra
                try
                {
                    SplashWindow sp = new SplashWindow();
                    sp.Show();
                    await Task.Delay(2600);
                    PixelSize sps = new PixelSize((int)sp.Width, (int)sp.Height);
                    using (RenderTargetBitmap rtb = new RenderTargetBitmap(sps, new Vector(96, 96)))
                    {
                        rtb.Render(sp);
                        rtb.Save(Path.Combine(dir, "avvio.png"));
                    }
                    sp.Close();
                }
                catch (Exception ex) { try { File.WriteAllText(Path.Combine(dir, "errore_avvio_schermata.txt"), ex.ToString()); } catch { } }
                AddDemoTraffic();
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
                for (int i = 0; i < tabs.Count; i++)   // tutte le linguette e sottolinguette, qualunque sia il loro numero
                {
                    tabs.Select(i);
                    PillTabs sub = tabs.Subs[i];
                    int nsub = sub == null ? 1 : sub.Count;
                    for (int k = 0; k < nsub; k++)
                    {
                        if (sub != null) sub.Select(k);
                        // traffico in diretta: ridisegno subito (di solito ogni 2 s)
                        RefreshTraffic(true);
                        await Task.Delay(1500);
                        Rect bb = (target ?? this).Bounds;
                    PixelSize ps = new PixelSize(Math.Max(100, (int)bb.Width), Math.Max(100, (int)bb.Height));
                        using (RenderTargetBitmap rtb = new RenderTargetBitmap(ps, new Vector(96, 96)))
                        {
                            rtb.Render(target ?? this);
                            rtb.Save(Path.Combine(dir, "linguetta_" + (i + 1) + (sub == null ? "" : "_" + (k + 1)) + ".png"));
                        }
                    }
                    if (sub != null) sub.Select(0);
                }
            }
            catch (Exception ex)
            {
                try { File.WriteAllText(Path.Combine(dir, "errore.txt"), ex.ToString()); } catch { }
            }
        }
    }
}
