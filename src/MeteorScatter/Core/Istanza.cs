// YA-MS - una sola copia del programma per cartella data (Windows, Linux, Mac).
// La prima copia tiene aperto in esclusiva data/ya-ms.lock: il blocco si libera da solo quando il programma si chiude,
// anche se si chiude male. Una seconda copia lascia il segnale data/ya-ms.mostra e si chiude; la prima lo vede entro
// un secondo e torna in primo piano (anche se era ridotta a icona).
// Dopo un aggiornamento la copia vecchia scrive data/ya-ms.riavvio prima di avviare quella nuova: la nuova aspetta
// fino a 30 secondi che la vecchia si chiuda, invece di uscire.
using System;
using System.IO;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

namespace YAMS
{
    internal static class Istanza
    {
        static FileStream blocco;
        static DispatcherTimer timer;

        static string FileBlocco { get { return Path.Combine(AppPaths.DataDir, "ya-ms.lock"); } }
        static string FileMostra { get { return Path.Combine(AppPaths.DataDir, "ya-ms.mostra"); } }
        static string FileRiavvio { get { return Path.Combine(AppPaths.DataDir, "ya-ms.riavvio"); } }

        static bool Prova()
        {
            try
            {
                blocco = new FileStream(FileBlocco, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                try
                {
                    blocco.SetLength(0);
                    byte[] b = System.Text.Encoding.ASCII.GetBytes(Environment.ProcessId.ToString());
                    blocco.Write(b, 0, b.Length); blocco.Flush();
                }
                catch { }
                return true;
            }
            catch (IOException) { return false; }
            catch (UnauthorizedAccessException) { return true; }   // cartella in sola lettura: meglio aprire che bloccare
            catch { return true; }
        }

        // true: questa e' la sola copia, si prosegue; false: c'e' gia' un'altra copia, gia' avvisata, e si deve uscire
        public static bool Prima()
        {
            if (Prova()) { Pulisci(); return true; }
            bool riavvio = false;
            try { riavvio = File.Exists(FileRiavvio) && (DateTime.UtcNow - File.GetLastWriteTimeUtc(FileRiavvio)).TotalSeconds < 180; } catch { }
            if (riavvio)
            {
                DateTime fine = DateTime.UtcNow.AddSeconds(30);
                while (DateTime.UtcNow < fine)
                {
                    Thread.Sleep(300);
                    if (Prova()) { Pulisci(); return true; }
                }
            }
            try { File.WriteAllText(FileMostra, DateTime.UtcNow.ToString("o")); } catch { }
            return false;
        }

        static void Pulisci()
        {
            try { if (File.Exists(FileMostra)) File.Delete(FileMostra); } catch { }
            try { if (File.Exists(FileRiavvio)) File.Delete(FileRiavvio); } catch { }
        }

        // da chiamare subito prima di avviare la copia nuova dopo un aggiornamento
        public static void SegnalaRiavvio()
        {
            try { File.WriteAllText(FileRiavvio, DateTime.UtcNow.ToString("o")); } catch { }
        }

        // nella prima copia: controlla ogni secondo se un'altra copia ha chiesto di mostrare la finestra
        public static void AscoltaRichiami(IClassicDesktopStyleApplicationLifetime desktop)
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (s, e) =>
            {
                try
                {
                    if (!File.Exists(FileMostra)) return;
                    File.Delete(FileMostra);
                    Window w = desktop.MainWindow;
                    if (w == null) return;
                    if (w.WindowState == WindowState.Minimized) w.WindowState = WindowState.Normal;
                    w.Show();
                    w.Activate();
                    w.Topmost = true; w.Topmost = false;   // porta davanti anche dove Activate non basta
                }
                catch { }
            };
            timer.Start();
        }
    }
}
