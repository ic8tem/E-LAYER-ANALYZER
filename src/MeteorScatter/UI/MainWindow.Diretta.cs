// YA-MS - linguetta "Sciami in diretta" (subito dopo "Flusso radar")
//
// Due fonti esterne, mostrate come immagini (non pubblicano numeri da leggere):
//  - Global Meteor Network (GMN), globalmeteornetwork.org/flux: flusso e ZHR MISURATI degli sciami attivi, dalla rete
//    mondiale di telecamere, aggiornati una volta al giorno. La pagina elenca i grafici degli sciami attivi; si legge
//    la pagina e si scaricano i grafici dell'anno in corso e il riepilogo annuale. Dati CC BY 4.0 (Vida et al. 2021, 2022).
//  - CMOR, Canadian Meteor Orbit Radar (University of Western Ontario, per il NASA Meteoroid Environment Office),
//    fireballs.ndc.nasa.gov/cmor-radiants: mappa del cielo con i radianti rilevati dal radar in tempo quasi reale
//    (anche gli sciami diurni), vista da 43° N 81° O, e la stessa mappa colorata per velocita'.
// Ogni file si riscarica solo se e' cambiato (If-Modified-Since), al massimo ogni 30 minuti o con "Scarica adesso".
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace YAMS
{
    public partial class MainWindow
    {
        const string GmnPage = "https://globalmeteornetwork.org/flux/";
        const string GmnPlots = "https://globalmeteornetwork.org/flux/plots/";
        const string CmorBase = "https://fireballs.ndc.nasa.gov/cmor-radiants/";

        public class GmnPlot { public string Code, File, Url; }

        DateTime lastLiveFetchUtc = DateTime.MinValue;
        bool liveFetching;
        StackPanel gmnList;
        Image picCmorAct, picCmorVel;
        TextBlock lblLive;

        string LiveDir { get { return Path.Combine(dataDir, "radar", "diretta"); } }

        void BuildLiveTab()
        {
            DockPanel p = Page(Lang.T("Sciami in diretta"));
            Button bt = B(Lang.T("Scarica adesso"));
            bt.Click += (s, e) => StartLiveFetch(true);
            lblLive = L("");
            DockTop(p, Row(bt, lblLive));
            TextBlock note = new TextBlock
            {
                Foreground = Pal.B(Pal.OrangeText), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 2, 6, 4),
                Text = Lang.T("A sinistra: flusso e ZHR misurati degli sciami attivi dal Global Meteor Network (telecamere, aggiornamento giornaliero; " +
                              "linea rossa = ultimo aggiornamento). A destra: radianti rilevati dal radar CMOR (Canada, 43° N) in tempo quasi reale, " +
                              "anche gli sciami diurni; sotto, gli stessi radianti colorati per velocità.")
            };
            DockTop(p, note);
            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("62*,38*") };
            gmnList = new StackPanel { Orientation = Orientation.Vertical, Spacing = 8, Margin = new Thickness(4) };
            ScrollViewer sv = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = gmnList
            };
            Grid.SetColumn(sv, 0);
            Grid right = new Grid { RowDefinitions = new RowDefinitions("*,*") };
            picCmorAct = MakePicture(); picCmorVel = MakePicture();
            Border fa = Framed(picCmorAct, Colors.Black), fv = Framed(picCmorVel, Colors.Black);
            fa.Margin = new Thickness(4, 4, 4, 2); fv.Margin = new Thickness(4, 2, 4, 4);
            Grid.SetRow(fa, 0); Grid.SetRow(fv, 1);
            right.Children.Add(fa); right.Children.Add(fv);
            Grid.SetColumn(right, 1);
            g.Children.Add(sv); g.Children.Add(right);
            p.Children.Add(g);
            ShowLive(LoadGmnIndex());
        }

        // elenco dei grafici GMN letto dalla pagina salvata
        List<GmnPlot> LoadGmnIndex()
        {
            List<GmnPlot> l = new List<GmnPlot>();
            try
            {
                string f = Path.Combine(LiveDir, "gmn_flux.html");
                if (!File.Exists(f)) return l;
                string html = File.ReadAllText(f);
                HashSet<string> seen = new HashSet<string>();
                foreach (Match m in Regex.Matches(html, @"flux_([A-Z]{3})_sol=[0-9.]+-[0-9.]+_year_\d{4}\.png"))
                {
                    string name = m.Value;
                    if (!seen.Add(name)) continue;
                    l.Add(new GmnPlot { Code = m.Groups[1].Value, Url = GmnPlots + name, File = Path.Combine(LiveDir, "gmn_" + m.Groups[1].Value + ".png") });
                }
            }
            catch { }
            return l;
        }

        void StartLiveFetch(bool force)
        {
            if (liveFetching) return;
            if (!force && (DateTime.UtcNow - lastLiveFetchUtc).TotalMinutes < 30) return;
            liveFetching = true;
            lastLiveFetchUtc = DateTime.UtcNow;
            string dir = LiveDir;
            System.Threading.ThreadPool.QueueUserWorkItem(_ =>
            {
                List<string> errs = new List<string>();
                int newCount = 0;
                Func<string, string, int> get = (url, file) =>
                {
                    DateTime lm = File.Exists(file) ? File.GetLastWriteTimeUtc(file) : DateTime.MinValue;
                    bool off; string err;
                    int r = RadarFlux.Fetch(url, file, ref lm, out off, out err);
                    if (r < 0) errs.Add(Path.GetFileName(file) + ": " + err);
                    if (r == 1) newCount++;
                    return r;
                };
                try
                {
                    Directory.CreateDirectory(dir);
                    get(GmnPage, Path.Combine(dir, "gmn_flux.html"));
                    List<GmnPlot> plots = LoadGmnIndex();
                    get(GmnPlots + "yearly_zhr_overview.png", Path.Combine(dir, "gmn_overview.png"));
                    foreach (GmnPlot pl in plots) get(pl.Url, pl.File);
                    get(CmorBase + "skymap-activity.png", Path.Combine(dir, "cmor_activity.png"));
                    get(CmorBase + "skymap-velocity.png", Path.Combine(dir, "cmor_velocity.png"));
                    get(GmnDaily.Url, GmnDaily.LocalFile(dataDir));
                    // catalogo IAU MDC: si riscarica al massimo una volta al mese (prima il file dell'anno, poi quello generale)
                    string mdc = IauMdc.LocalFile(dataDir);
                    if (!File.Exists(mdc) || (DateTime.UtcNow - File.GetLastWriteTimeUtc(mdc)).TotalDays > 30)
                    {
                        int before = errs.Count;
                        if (get(string.Format(IauMdc.UrlYear, DateTime.UtcNow.Year), mdc) < 0 && get(string.Format(IauMdc.UrlYear, DateTime.UtcNow.Year - 1), mdc) < 0)
                            get(IauMdc.UrlLatest, mdc);
                        if (File.Exists(mdc)) errs.RemoveRange(before, errs.Count - before);
                    }
                    GmnDaily.Load(dataDir); IauMdc.Load(dataDir); GmnPopIndex.Load(Path.Combine(dir, "gmn_flux.html"));
                    Dispatcher.UIThread.Post(() =>
                    {
                        liveFetching = false;
                        if (replaced) return;
                        string t = DateTime.UtcNow.ToString("HH:mm", CultureInfo.InvariantCulture);
                        lblLive.Text = errs.Count == 0
                            ? Lang.F("GMN e CMOR controllati alle {0} UTC ({1} file nuovi). Dati GMN: CC BY 4.0.", t, newCount)
                            : Lang.F("GMN e CMOR alle {0} UTC: {1} errori ({2})", t, errs.Count, errs[0]);
                        ShowLive(plots);
                        UpdateShowerGrid();
                        UpdatePrevTab();
                    });
                }
                catch (Exception ex)
                {
                    string m = ex.Message;
                    Dispatcher.UIThread.Post(() => { liveFetching = false; if (!replaced) lblLive.Text = Lang.F("errore: {0}", m); });
                }
            });
        }

        // immagini grandi (i grafici GMN sono 4500 px): si decodificano gia' ridotte
        static Bitmap LoadScaled(string file, int width)
        {
            try
            {
                if (!File.Exists(file)) return null;
                byte[] b = File.ReadAllBytes(file);
                using (MemoryStream ms = new MemoryStream(b))
                    return Bitmap.DecodeToWidth(ms, width, BitmapInterpolationMode.HighQuality);
            }
            catch { return null; }
        }

        void ShowLive(List<GmnPlot> plots)
        {
            if (gmnList == null) return;
            gmnList.Children.Clear();
            string dir = LiveDir;
            if (GmnDaily.Total > 0)
            {
                List<KeyValuePair<string, int>> l = new List<KeyValuePair<string, int>>(GmnDaily.Counts);
                l.Sort((a, b) => b.Value.CompareTo(a.Value));
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (KeyValuePair<string, int> kv in l)
                {
                    string nm = kv.Key;
                    foreach (Shower s in Catalog.Showers) if (s.Code == kv.Key) { nm = s.Code + " " + Lang.T(s.Name); break; }
                    sb.Append(nm).Append(": ").Append(kv.Value).Append("   ");
                }
                gmnList.Children.Add(new TextBlock { Text = Lang.F("Traiettorie confermate dal GMN dal {0} al {1} UTC: {2} (sporadiche {3})",
                    GmnDaily.FirstUtc.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture), GmnDaily.LastUtc.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture), GmnDaily.Total, GmnDaily.Sporadic),
                    FontWeight = FontWeight.SemiBold, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(4, 2, 0, 0) });
                gmnList.Children.Add(new TextBlock { Text = sb.ToString().TrimEnd() + "\n" + Lang.T("Conteggio delle telecamere, non flusso: dipende da quante stazioni vedono il cielo sereno e di notte."),
                    TextWrapping = TextWrapping.Wrap, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(4, 0, 4, 4) });
            }
            Bitmap ov = LoadScaled(Path.Combine(dir, "gmn_overview.png"), 1800);
            if (plots.Count == 0 && ov == null)
                gmnList.Children.Add(new TextBlock { Foreground = Pal.B(Pal.Muted), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6),
                    Text = Lang.T("Nessun dato GMN: premi \"Scarica adesso\".") });
            foreach (GmnPlot pl in plots)
            {
                Bitmap bmp = LoadScaled(pl.File, 1800);
                if (bmp == null) continue;
                string name = pl.Code;
                foreach (Shower s in Catalog.Showers) if (s.Code == pl.Code) { name = s.Code + " " + Lang.T(s.Name); break; }
                gmnList.Children.Add(new TextBlock { Text = name, FontWeight = FontWeight.SemiBold, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(4, 2, 0, 0) });
                gmnList.Children.Add(Framed(new Image { Source = bmp, Stretch = Stretch.Uniform }, Colors.White));
            }
            if (ov != null)
            {
                gmnList.Children.Add(new TextBlock { Text = Lang.T("Riepilogo dell'anno (ZHR misurati, GMN)"), FontWeight = FontWeight.SemiBold, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(4, 6, 0, 0) });
                gmnList.Children.Add(Framed(new Image { Source = ov, Stretch = Stretch.Uniform }, Colors.White));
            }
            Bitmap ca = LoadScaled(Path.Combine(dir, "cmor_activity.png"), 1059);
            Bitmap cv = LoadScaled(Path.Combine(dir, "cmor_velocity.png"), 1059);
            if (ca != null) picCmorAct.Source = ca;
            if (cv != null) picCmorVel.Source = cv;
        }
    }
}
