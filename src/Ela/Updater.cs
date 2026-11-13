// E-LAYER ANALYZER - avviso e aggiornamento automatico dalle Release di GitHub, controllo dei file mancanti
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using YAMS;

namespace ELA
{
    // Aggiornamento automatico e silenzioso (richiesta dell'autore, 13/11/2026; agg_automatici=1, di partenza): quando
    // c'e' una versione piu' recente la si scarica e installa e il programma si riavvia da solo, senza finestre; anche i
    // file mancanti si riscaricano senza chiedere. Con agg_automatici=0 in data/config.ini torna il comportamento qui sotto.
    // Aggiornamento automatico.
    // All'avvio (dopo 20 s) e poi ogni 6 ore legge l'ultima Release di GitHub; se e' piu' recente della versione in uso
    // lo segnala nella barra in alto (scritta lampeggiante cliccabile) e con una finestra: Aggiorna ora / Più tardi / Salta questa versione.
    // "Aggiorna ora" scarica il pacchetto portatile della propria piattaforma, aggiorna i file del programma nella cartella data
    // (call3.txt dell'utente resta), poi chiude il programma: uno script sostituisce i file e lo riavvia.
    public static class Updater
    {
        public const string Repo = "ic8tem/E-LAYER-ANALYZER";
        public static readonly TimeSpan Interval = TimeSpan.FromHours(6);

        public static string LatestVersion { get; private set; }   // es. "0.4.5", null se non ancora letta
        public static string LatestNotes { get; private set; }
        public static string LatestUrl { get; private set; }
        public static bool Newer { get; private set; }
        public static event Action Changed;
        static List<KeyValuePair<string, string>> assets = new List<KeyValuePair<string, string>>();   // nome, indirizzo di scaricamento
        static bool shownThisSession;

        static HttpClient NewClient(int minutes)
        {
            HttpClient h = new HttpClient { Timeout = TimeSpan.FromMinutes(minutes) };
            h.DefaultRequestHeaders.UserAgent.ParseAdd("E-LAYER-ANALYZER/" + ElaWindow.Version + " (programma per radioamatori)");
            return h;
        }

        public static bool Enabled { get { return Station.Cfg == null || Station.Cfg.GetB("agg_controlla", true); } }
        public static bool Silent { get { return Enabled && (Station.Cfg == null || Station.Cfg.GetB("agg_automatici", true)); } }

        // legge l'ultima Release; true se c'e' una versione piu' recente
        public static async Task<bool> CheckAsync()
        {
            try
            {
                string json;
                using (HttpClient h = NewClient(1))
                {
                    h.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
                    json = await h.GetStringAsync("https://api.github.com/repos/" + Repo + "/releases/latest");
                }
                using (JsonDocument d = JsonDocument.Parse(json))
                {
                    JsonElement r = d.RootElement;
                    string tag = r.GetProperty("tag_name").GetString() ?? "";
                    LatestVersion = tag.TrimStart('v', 'V');
                    LatestNotes = r.TryGetProperty("body", out JsonElement b) ? b.GetString() ?? "" : "";
                    LatestUrl = r.TryGetProperty("html_url", out JsonElement u) ? u.GetString() : "https://github.com/" + Repo + "/releases/latest";
                    List<KeyValuePair<string, string>> a = new List<KeyValuePair<string, string>>();
                    if (r.TryGetProperty("assets", out JsonElement arr))
                        foreach (JsonElement x in arr.EnumerateArray())
                            a.Add(new KeyValuePair<string, string>(x.GetProperty("name").GetString() ?? "", x.GetProperty("browser_download_url").GetString() ?? ""));
                    assets = a;
                }
                Newer = IsNewer(LatestVersion, ElaWindow.Version);
            }
            catch { }
            Changed?.Invoke();
            return Newer;
        }

        public static bool IsNewer(string a, string b)
        {
            Version va, vb;
            return Version.TryParse(Norm(a), out va) && Version.TryParse(Norm(b), out vb) && va > vb;
        }

        static string Norm(string v)
        {
            v = (v ?? "").Trim();
            int i = v.IndexOfAny(new[] { '-', '+', ' ' });
            if (i > 0) v = v.Substring(0, i);
            return v.Split('.').Length == 1 ? v + ".0" : v;
        }

        // pacchetto della piattaforma in uso: zip portatile su Windows, tar.gz portatile su Linux, app.zip su Mac
        public static string AssetSuffix()
        {
            Architecture a = RuntimeInformation.ProcessArchitecture;
            if (OperatingSystem.IsWindows()) return a == Architecture.X86 ? "-win-x86-portatile.zip" : "-win-x64-portatile.zip";
            if (OperatingSystem.IsMacOS()) return a == Architecture.Arm64 ? "-osx-arm64-app.zip" : "-osx-x64-app.zip";
            if (a == Architecture.Arm64) return "-linux-arm64-portatile.tar.gz";
            if (a == Architecture.Arm) return "-linux-arm-portatile.tar.gz";
            return "-linux-x64-portatile.tar.gz";
        }

        static string FindAsset(out string name)
        {
            string suf = AssetSuffix();
            foreach (KeyValuePair<string, string> kv in assets)
                if (kv.Key.EndsWith(suf, StringComparison.OrdinalIgnoreCase)) { name = kv.Key; return kv.Value; }
            name = null;
            return null;
        }

        // cartella del pacchetto .app (Mac), null altrove
        static string MacBundle()
        {
            string b = AppContext.BaseDirectory.TrimEnd('/', '\\');
            int i = b.IndexOf(".app/Contents", StringComparison.OrdinalIgnoreCase);
            return i > 0 ? b.Substring(0, i + 4) : null;
        }

        static bool CanWrite(string dir)
        {
            try
            {
                string t = Path.Combine(dir, ".prova_scrittura");
                File.WriteAllText(t, "x");
                File.Delete(t);
                return true;
            }
            catch { return false; }
        }

        // ---------- avvio: controllo dopo 20 s, poi ogni 6 ore
        static DispatcherTimer timer;
        public static void Start(Window owner)
        {
            if (Program.ScreenshotDir != null || timer != null) return;
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(20) };
            timer.Tick += async (s, e) =>
            {
                timer.Interval = Interval;
                if (!Enabled) return;
                bool n = await CheckAsync();
                if (n && Silent)
                {
                    // scarica, installa e riavvia senza chiedere; se non si puo' (nessun pacchetto, cartella non
                    // scrivibile, rete) resta solo la scritta in alto e si riprova al controllo successivo
                    string err = await Install(LatestVersion, p => { }, x => { });
                    if (err == null) QuitForUpdate();
                    return;
                }
                string skip = Station.Cfg.Get("agg_salta", "");
                if (n && !shownThisSession && skip != LatestVersion)
                {
                    shownThisSession = true;
                    await ShowDialog(ActiveOwner(owner));
                }
            };
            timer.Start();
        }

        static Window ActiveOwner(Window fallback)
        {
            try
            {
                if (Application.Current.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime l && l.MainWindow != null) return l.MainWindow;
            }
            catch { }
            return fallback;
        }

        // ---------- finestra dell'aggiornamento
        public static async Task ShowDialog(Window owner)
        {
            if (LatestVersion == null) await CheckAsync();
            Window w = new Window
            {
                Title = Lang.T("Aggiornamento del programma"), Width = 620, SizeToContent = SizeToContent.Height, CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner, Background = Pal.B(Pal.Back), Foreground = Pal.B(Pal.Fore)
            };
            StackPanel sp = new StackPanel { Margin = new Thickness(14) };
            TextBlock head = new TextBlock { FontSize = 15, FontWeight = FontWeight.SemiBold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 8) };
            TextBlock info = new TextBlock { TextWrapping = TextWrapping.Wrap, Foreground = Pal.B(Pal.Muted), Margin = new Thickness(0, 0, 0, 8) };
            ProgressBar bar = new ProgressBar { Minimum = 0, Maximum = 100, Height = 8, IsVisible = false, Margin = new Thickness(0, 4, 0, 8) };
            CheckBox chk = Ui.C(Lang.T("Controlla gli aggiornamenti all'avvio e ogni 6 ore"), Enabled);
            chk.Click += (s, e) => { Station.Cfg.Set("agg_controlla", chk.IsChecked == true); Station.Save(); };
            sp.Children.Add(head);
            bool newer = Newer;
            if (LatestVersion == null)
            {
                head.Text = Lang.T("Impossibile controllare gli aggiornamenti: GitHub non risponde o manca la connessione a internet.");
                head.Foreground = Pal.B(Pal.Red);
            }
            else if (newer)
            {
                head.Text = Lang.F("È disponibile la versione {0} (in uso: {1}).", LatestVersion, ElaWindow.Version);
                head.Foreground = Pal.B(Pal.Green);
                string notes = (LatestNotes ?? "").Trim();
                if (notes.Length > 900) notes = notes.Substring(0, 900) + "…";
                if (notes.Length > 0)
                    sp.Children.Add(new Border
                    {
                        Background = Pal.B(Pal.Panel2), Padding = new Thickness(8), Margin = new Thickness(0, 0, 0, 8), MaxHeight = 220,
                        Child = new ScrollViewer { Content = new TextBlock { Text = notes, TextWrapping = TextWrapping.Wrap, FontSize = 12 } }
                    });
                info.Text = Lang.T("«Aggiorna ora» scarica il pacchetto per questo computer, chiude il programma, sostituisce i file e lo riavvia. Impostazioni, spot raccolti e call3.txt restano.");
            }
            else
            {
                head.Text = Lang.F("La versione in uso ({0}) è la più recente.", ElaWindow.Version);
                head.Foreground = Pal.B(Pal.Green);
            }
            sp.Children.Add(info);
            sp.Children.Add(bar);
            sp.Children.Add(chk);

            Button bNow = Ui.B(Lang.T("Aggiorna ora"), true), bLater = Ui.B(Lang.T("Più tardi")), bSkip = Ui.B(Lang.T("Salta questa versione"));
            Button bPage = Ui.B(Lang.T("Pagina della release"));
            Grid btns = new Grid { ColumnDefinitions = new ColumnDefinitions("*,*,*"), Margin = new Thickness(0, 12, 0, 0) };
            if (newer)
            {
                bNow.HorizontalAlignment = HorizontalAlignment.Left; bLater.HorizontalAlignment = HorizontalAlignment.Center; bSkip.HorizontalAlignment = HorizontalAlignment.Right;
                Grid.SetColumn(bNow, 0); Grid.SetColumn(bLater, 1); Grid.SetColumn(bSkip, 2);
                btns.Children.Add(bNow); btns.Children.Add(bLater); btns.Children.Add(bSkip);
            }
            else
            {
                bPage.HorizontalAlignment = HorizontalAlignment.Left; bLater.HorizontalAlignment = HorizontalAlignment.Right;
                bLater.Content = Lang.T("Chiudi");
                Grid.SetColumn(bPage, 0); Grid.SetColumn(bLater, 2);
                btns.Children.Add(bPage); btns.Children.Add(bLater);
            }
            sp.Children.Add(btns);
            w.Content = sp;

            bool busy = false, failed = false;
            bLater.Click += (s, e) => { if (!busy) w.Close(); };
            bPage.Click += (s, e) => OpenUrl(LatestUrl ?? "https://github.com/" + Repo + "/releases");
            bSkip.Click += (s, e) =>
            {
                if (failed) { OpenUrl(LatestUrl ?? "https://github.com/" + Repo + "/releases"); return; }
                Station.Cfg.Set("agg_salta", LatestVersion ?? ""); Station.Save(); w.Close();
            };
            w.Closing += (s, e) => { if (busy) e.Cancel = true; };
            bNow.Click += async (s, e) =>
            {
                busy = true;
                bNow.IsEnabled = bLater.IsEnabled = bSkip.IsEnabled = false;
                bar.IsVisible = true;
                string err = await Install(LatestVersion, p => Dispatcher.UIThread.Post(() => { bar.Value = p; info.Text = Lang.F("Scaricamento: {0:0}%", p); }),
                                           t => Dispatcher.UIThread.Post(() => info.Text = t));
                busy = false;
                if (err != null)
                {
                    info.Text = err; info.Foreground = Pal.B(Pal.Red);
                    bLater.IsEnabled = true; bLater.Content = Lang.T("Chiudi");
                    failed = true;
                    bSkip.IsEnabled = true; bSkip.Content = Lang.T("Pagina della release");
                    return;
                }
                w.Close();
                QuitForUpdate();
            };
            w.Opened += (s, e) => UiScale.Apply(w);
            if (owner != null && owner.IsVisible) await w.ShowDialog(owner); else w.Show();
        }

        public static void OpenUrl(string url)
        {
            try
            {
                if (OperatingSystem.IsWindows()) Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                else if (OperatingSystem.IsMacOS()) Process.Start("open", url);
                else Process.Start("xdg-open", url);
            }
            catch { }
        }

        // ---------- scaricamento, estrazione e preparazione dello script; null se tutto bene, altrimenti il messaggio d'errore
        static string script;
        static async Task<string> Install(string ver, Action<double> progress, Action<string> say)
        {
            string name, url = FindAsset(out name);
            if (url == null) return Lang.F("Nella release {0} manca il pacchetto per questo computer ({1}): scaricalo a mano dalla pagina della release.", ver, AssetSuffix().TrimStart('-'));
            string appDir = AppPaths.AppDir, dataDir = AppPaths.DataDir;
            string bundle = MacBundle();
            if (!CanWrite(appDir) || (bundle != null && !CanWrite(bundle)))
                return Lang.F("La cartella del programma ({0}) non è scrivibile: aggiorna con il pacchetto di installazione dalla pagina della release.", appDir);
            string work = Path.Combine(dataDir, "aggiornamento");
            return await Task.Run(async () =>
            {
                try
                {
                    if (Directory.Exists(work)) Directory.Delete(work, true);
                    Directory.CreateDirectory(work);
                    string pkg = Path.Combine(work, name);
                    using (HttpClient h = NewClient(30))
                    using (HttpResponseMessage r = await h.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        r.EnsureSuccessStatusCode();
                        long total = r.Content.Headers.ContentLength ?? -1, done = 0;
                        using (Stream src = await r.Content.ReadAsStreamAsync())
                        using (FileStream dst = File.Create(pkg))
                        {
                            byte[] buf = new byte[81920];
                            int n; double last = -1;
                            while ((n = await src.ReadAsync(buf, 0, buf.Length)) > 0)
                            {
                                await dst.WriteAsync(buf, 0, n);
                                done += n;
                                if (total > 0) { double p = 100.0 * done / total; if (p - last >= 1) { last = p; progress(p); } }
                            }
                        }
                    }
                    say(Lang.T("Estrazione dei file..."));
                    string ex = Path.Combine(work, "nuovo");
                    Directory.CreateDirectory(ex);
                    if (name.EndsWith(".tar.gz", StringComparison.OrdinalIgnoreCase)) Run("tar", "-xzf \"" + pkg + "\" -C \"" + ex + "\"");
                    else if (OperatingSystem.IsMacOS()) Run("ditto", "-x -k \"" + pkg + "\" \"" + ex + "\"");
                    else ZipFile.ExtractToDirectory(pkg, ex, true);

                    // radice dei file nuovi
                    string root = ex, newApp = null, newData;
                    if (OperatingSystem.IsMacOS())
                    {
                        newApp = Path.Combine(ex, "E-LAYER-ANALYZER.app");
                        if (!Directory.Exists(newApp)) return Lang.T("Il pacchetto scaricato non contiene il programma.");
                        newData = Path.Combine(newApp, "Contents", "Resources", "data");
                    }
                    else
                    {
                        if (Directory.Exists(Path.Combine(ex, "E-LAYER-ANALYZER")) && !File.Exists(Path.Combine(ex, "E-LAYER-ANALYZER.exe"))) root = Path.Combine(ex, "E-LAYER-ANALYZER");
                        if (!File.Exists(Path.Combine(root, OperatingSystem.IsWindows() ? "E-LAYER-ANALYZER.exe" : "E-LAYER-ANALYZER")))
                            return Lang.T("Il pacchetto scaricato non contiene il programma.");
                        newData = Path.Combine(root, "data");
                    }

                    // file del programma nella cartella data: si aggiornano subito (non sono in uso); call3.txt dell'utente resta
                    say(Lang.T("Aggiornamento dei file della cartella data..."));
                    CopyData(newData, dataDir);
                    if (!OperatingSystem.IsMacOS()) Directory.Delete(newData, true);   // lo script copia solo il resto

                    // script che aspetta la chiusura del programma, sostituisce i file e lo riavvia
                    int pid = Environment.ProcessId;
                    if (OperatingSystem.IsWindows())
                    {
                        script = Path.Combine(work, "aggiorna.cmd");
                        string exe = Path.Combine(appDir, "E-LAYER-ANALYZER.exe");
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("@echo off");
                        sb.AppendLine("chcp 65001 >nul");
                        sb.AppendLine(":attesa");
                        sb.AppendLine("tasklist /FI \"PID eq " + pid + "\" 2>nul | find \"" + pid + "\" >nul && (timeout /t 1 /nobreak >nul & goto attesa)");
                        sb.AppendLine("robocopy \"" + root + "\" \"" + appDir + "\" /E /R:10 /W:1 /NFL /NDL /NJH /NJS /NP >nul");
                        sb.AppendLine("start \"\" \"" + exe + "\"");
                        File.WriteAllText(script, sb.ToString(), new UTF8Encoding(false));
                    }
                    else
                    {
                        script = Path.Combine(work, "aggiorna.sh");
                        StringBuilder sb = new StringBuilder();
                        sb.Append("#!/bin/sh\n");
                        sb.Append("while kill -0 " + pid + " 2>/dev/null; do sleep 1; done\n");
                        if (OperatingSystem.IsMacOS())
                        {
                            sb.Append("rm -rf \"" + bundle + "\"\n");
                            sb.Append("ditto \"" + newApp + "\" \"" + bundle + "\"\n");
                            sb.Append("open \"" + bundle + "\"\n");
                        }
                        else
                        {
                            sb.Append("cp -R \"" + root + "/.\" \"" + appDir + "/\"\n");
                            sb.Append("chmod +x \"" + appDir + "/E-LAYER-ANALYZER\"\n");
                            sb.Append("echo \"" + ver + "\" > \"" + appDir + "/.versione\"\n");   // per l'avvio dal pacchetto .deb
                            sb.Append("cd \"" + appDir + "\" && nohup \"" + appDir + "/E-LAYER-ANALYZER\" >/dev/null 2>&1 &\n");
                        }
                        File.WriteAllText(script, sb.ToString(), new UTF8Encoding(false));
                        Run("chmod", "+x \"" + script + "\"");
                    }
                    return null;
                }
                catch (Exception e) { return Lang.F("Aggiornamento non riuscito: {0}", e.Message); }
            });
        }

        static void CopyData(string src, string dst)
        {
            if (!Directory.Exists(src)) return;
            foreach (string f in Directory.GetFiles(src, "*", SearchOption.AllDirectories))
            {
                string rel = Path.GetRelativePath(src, f);
                string d = Path.Combine(dst, rel);
                if (rel.Equals("call3.txt", StringComparison.OrdinalIgnoreCase) && File.Exists(d)) continue;
                try { Directory.CreateDirectory(Path.GetDirectoryName(d)); File.Copy(f, d, true); } catch { }
            }
        }

        static void Run(string exe, string args)
        {
            using (Process p = Process.Start(new ProcessStartInfo(exe, args) { UseShellExecute = false, CreateNoWindow = true }))
                p.WaitForExit(120000);
        }

        // avvia lo script staccato dal programma e chiude il programma (con il salvataggio normale)
        static void QuitForUpdate()
        {
            try
            {
                if (OperatingSystem.IsWindows())
                    Process.Start(new ProcessStartInfo("cmd.exe", "/c \"\"" + script + "\"\"") { UseShellExecute = false, CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden });
                else
                    Process.Start(new ProcessStartInfo("/bin/sh", "-c \"nohup '" + script + "' >/dev/null 2>&1 &\"") { UseShellExecute = false });
            }
            catch { return; }
            try
            {
                if (Application.Current.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime l)
                {
                    if (l.MainWindow != null) l.MainWindow.Close();
                    l.Shutdown();
                }
            }
            catch { Environment.Exit(0); }
        }

        // ================= controllo dei file mancanti =================
        // data/manifest.txt (creato dalla compilazione su GitHub) elenca tutti i file distribuiti con il programma.
        // All'avvio si controlla che ci siano tutti; se ne mancano, una finestra li elenca e propone di riscaricarli da GitHub.
        public static List<string> MissingFiles()
        {
            List<string> miss = new List<string>();
            string dataDir = AppPaths.DataDir, baseDir = AppContext.BaseDirectory;
            // l'elenco si legge da quello distribuito con il programma (su Mac dentro il pacchetto), poi dalla cartella data
            string man = null;
            foreach (string c in new[] { Path.Combine(baseDir, "data", "manifest.txt"), Path.GetFullPath(Path.Combine(baseDir, "..", "Resources", "data", "manifest.txt")), Path.Combine(dataDir, "manifest.txt") })
                if (File.Exists(c)) { man = c; break; }
            if (man == null) man = Path.Combine(dataDir, "manifest.txt");
            List<string> list = new List<string>();
            if (File.Exists(man))
            {
                foreach (string l in File.ReadAllLines(man))
                {
                    string t = l.Trim().Replace('\\', '/');
                    if (t.Length > 0 && !t.StartsWith("#") && t != "data/manifest.txt") list.Add(t);
                }
            }
            else
            {
                // senza elenco: i file indispensabili
                list.Add("data/manifest.txt");
                list.Add("data/lingue/chiavi.txt"); list.Add("data/lingue/ela-chiavi.txt");
                if (Lang.Code != "it") { list.Add("data/lingue/" + Lang.Code + ".txt"); list.Add("data/lingue/ela-" + Lang.Code + ".txt"); }
                string suf = Lang.Code == "it" ? "" : "_" + Lang.Code;
                foreach (string d in new[] { "ela_fonti", "ela_documento_tecnico", "ela_manuale", "ela_relazione" }) list.Add("data/" + d + suf + ".txt");
                list.Add("data/igrf14.csv"); list.Add("data/sciami.csv"); list.Add("data/call3.txt"); list.Add("data/aurora_benchmark.txt");
            }
            foreach (string rel in list)
            {
                string p = rel.StartsWith("data/") ? Path.Combine(dataDir, rel.Substring(5)) : Path.Combine(baseDir, rel);
                if (rel == "data/manifest.txt") p = man;
                if (!File.Exists(p)) miss.Add(rel);
            }
            return miss;
        }

        public static async Task CheckMissing(Window owner)
        {
            if (Program.ScreenshotDir != null) return;
            List<string> miss = await Task.Run(() => { try { return MissingFiles(); } catch { return new List<string>(); } });
            if (miss.Count == 0) return;
            if (Silent) { await DownloadMissing(miss, null); return; }   // aggiornamenti automatici: senza chiedere
            Window w = new Window
            {
                Title = Lang.T("File mancanti"), Width = 620, SizeToContent = SizeToContent.Height, CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner, Background = Pal.B(Pal.Back), Foreground = Pal.B(Pal.Fore)
            };
            StackPanel sp = new StackPanel { Margin = new Thickness(14) };
            sp.Children.Add(new TextBlock
            {
                Text = Lang.F("Mancano {0} file del programma: alcune funzioni o documenti potrebbero non funzionare.", miss.Count),
                FontSize = 15, FontWeight = FontWeight.SemiBold, Foreground = Pal.B(Pal.Orange), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 8)
            });
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < miss.Count && i < 25; i++) sb.AppendLine(miss[i]);
            if (miss.Count > 25) sb.AppendLine(Lang.F("... e altri {0}", miss.Count - 25));
            sp.Children.Add(new Border
            {
                Background = Pal.B(Pal.Panel2), Padding = new Thickness(8), MaxHeight = 260,
                Child = new ScrollViewer { Content = new TextBlock { Text = sb.ToString().TrimEnd(), FontFamily = Pal.Mono, FontSize = 12 } }
            });
            TextBlock info = new TextBlock
            {
                Text = Lang.F("«Scarica da GitHub» li riprende dalla versione {0} pubblicata su GitHub; in alternativa reinstalla il programma.", ElaWindow.Version),
                TextWrapping = TextWrapping.Wrap, Foreground = Pal.B(Pal.Muted), Margin = new Thickness(0, 8, 0, 0)
            };
            sp.Children.Add(info);
            Button bGet = Ui.B(Lang.T("Scarica da GitHub"), true), bClose = Ui.B(Lang.T("Chiudi"));
            Grid btns = new Grid { ColumnDefinitions = new ColumnDefinitions("*,*"), Margin = new Thickness(0, 12, 0, 0) };
            bGet.HorizontalAlignment = HorizontalAlignment.Left; bClose.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetColumn(bClose, 1);
            btns.Children.Add(bGet); btns.Children.Add(bClose);
            sp.Children.Add(btns);
            w.Content = sp;
            bClose.Click += (s, e) => w.Close();
            bGet.Click += async (s, e) =>
            {
                bGet.IsEnabled = false;
                info.Text = Lang.T("Scaricamento in corso...");
                int[] r = await DownloadMissing(miss, (o, n) => Dispatcher.UIThread.Post(() => info.Text = Lang.F("Scaricati {0} di {1}...", o, n)));
                int ok = r[0], bad = r[1];
                info.Text = bad == 0
                    ? Lang.F("Scaricati {0} file. Riavvia il programma per usarli.", ok)
                    : Lang.F("Scaricati {0} file, {1} non disponibili: reinstalla il programma dalla pagina delle release.", ok, bad);
                info.Foreground = Pal.B(bad == 0 ? Pal.Green : Pal.Red);
            };
            w.Opened += (s, e) => UiScale.Apply(w);
            if (owner != null && owner.IsVisible) await w.ShowDialog(owner); else w.Show();
        }

        // riscarica da GitHub (stessa versione) i file mancanti; { scaricati, non disponibili }
        static async Task<int[]> DownloadMissing(List<string> miss, Action<int, int> progress)
        {
            int ok = 0, bad = 0;
            await Task.Run(async () =>
            {
                using (HttpClient h = NewClient(5))
                    foreach (string rel in miss)
                    {
                        try
                        {
                            byte[] b = await h.GetByteArrayAsync("https://raw.githubusercontent.com/" + Repo + "/v" + ElaWindow.Version + "/" + rel);
                            string p = rel.StartsWith("data/") ? Path.Combine(AppPaths.DataDir, rel.Substring(5)) : Path.Combine(AppContext.BaseDirectory, rel);
                            Directory.CreateDirectory(Path.GetDirectoryName(p));
                            File.WriteAllBytes(p, b);
                            ok++;
                        }
                        catch { bad++; }
                        if (progress != null) progress(ok + bad, miss.Count);
                    }
            });
            return new[] { ok, bad };
        }
    }
}
