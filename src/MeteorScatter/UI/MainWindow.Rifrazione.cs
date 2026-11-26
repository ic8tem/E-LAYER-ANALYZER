// YA-MS - casella "Rifrazione troposferica (GFS)": profili di rifrattivita' sopra le due stazioni dal modello GFS
// (Open-Meteo, come LIVETROPOVHF) e raggio tracciato fino alla quota delle scie (src/Core/Refraction.cs).
// Spenta all'avvio: senza rifrazione la zona comune e' piu' piccola, quindi il calcolo e' piu' prudente.
// (richiesta dell'autore, IC8TEM, 22/11/2026)
using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;

namespace YAMS
{
    public partial class MainWindow
    {
        CheckBox chkRefr;
        bool refrBusy;
        string refrNote = "";
        DateTime refrFetchedUtc = DateTime.MinValue;

        void BuildRefrCheck()
        {
            chkRefr = C(Lang.T("Rifrazione troposferica (GFS)"), cfg.GetB("rifrazione_gfs", false));
            ToolTip.SetTip(chkRefr, Lang.T("Spenta all'avvio (valore prudente). Accesa: il programma scarica dal modello GFS il profilo di temperatura e umidità sopra le due stazioni e traccia il raggio fino alla quota delle scie. I punti bassi si vedono un po' più alti e la zona comune si allarga (circa 60-70 km all'orizzonte); sopra 10° l'effetto è trascurabile. Non usa il k = 4/3, che vale per raggi tutti dentro la troposfera."));
            WhenChanged(chkRefr, ToggleButton.IsCheckedProperty, () =>
            {
                cfg.Set("rifrazione_gfs", chkRefr.IsChecked == true);
                if (chkRefr.IsChecked == true) StartRefrFetch(true);
                else { PathModel.RefrHomeProfile = null; PathModel.RefrDxProfile = null; refrNote = ""; Recalc(); }
            });
        }

        // da chiamare prima del calcolo: se la casella e' accesa e i profili non sono della tratta attuale (o sono vecchi), li riscarica
        void EnsureRefr()
        {
            if (chkRefr == null || chkRefr.IsChecked != true) { PathModel.RefrHomeProfile = null; PathModel.RefrDxProfile = null; return; }
            LatLon home, dx;
            if (!Astro.TryLocator(Up(txtHome), out home) || !Astro.TryLocator(Up(txtDx), out dx)) return;
            bool same = PathModel.RefrHomeProfile != null && Astro.DistanceKm(home, PathModel.RefrHomeAt) < 30 && Astro.DistanceKm(dx, PathModel.RefrDxAt) < 30;
            if (!same || DateTime.UtcNow - refrFetchedUtc > TimeSpan.FromHours(3)) StartRefrFetch(false);
        }

        void StartRefrFetch(bool recalcAfter)
        {
            if (refrBusy) return;
            LatLon home, dx;
            if (!Astro.TryLocator(Up(txtHome), out home) || !Astro.TryLocator(Up(txtDx), out dx)) return;
            refrBusy = true;
            DateTime t = DateTime.UtcNow;
            Task.Run(() =>
            {
                string e1, e2;
                RefrProfile ph = Refraction.FetchGfs(dataDir, home, t, out e1), pd = Refraction.FetchGfs(dataDir, dx, t, out e2);
                string note = "";
                if (ph == null || pd == null)
                {
                    note = Lang.F("GFS non disponibile ({0}): uso l'atmosfera di riferimento ITU-R P.453", (e1.Length > 0 ? e1 : e2));
                    if (ph == null) ph = Refraction.Reference();
                    if (pd == null) pd = Refraction.Reference();
                }
                Dispatcher.UIThread.Post(() =>
                {
                    refrBusy = false;
                    if (chkRefr.IsChecked != true) return;
                    PathModel.RefrHomeProfile = ph; PathModel.RefrDxProfile = pd;
                    PathModel.RefrHomeAt = home; PathModel.RefrDxAt = dx;
                    refrFetchedUtc = DateTime.UtcNow;
                    refrNote = note.Length > 0 ? note : Lang.F("profili {0} su {1} e {2}", ph.Source, Up(txtHome), Up(txtDx));
                    Recalc();
                });
            });
        }

        string RefrReportLine(PathModel m)
        {
            if (chkRefr == null || chkRefr.IsChecked != true || PathModel.RefrHomeProfile == null)
                return Lang.T("Rifrazione      non applicata (casella spenta): geometria a raggi diritti");
            return Lang.F("Rifrazione      +{0:F2}° verso il punto medio, portata alla quota delle scie +{1:F0} km ({2})", m.RefrCorrMidDeg, m.RefrRangeGainKm, refrNote);
        }
    }
}
