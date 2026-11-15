// YA-MS - fascia sempre visibile sotto la riga dell'orario, sopra le linguette (richiesta dell'autore, IC8TEM, 14/11/2026).
// Prima stava nella linguetta "Calcolo dell'efficienza" (quattro riquadri). Ora contiene:
//  - ORA MIGLIORE DEL GIORNO e SCIAME PIU' FAVOREVOLE del giorno scelto (come prima, da Update24h);
//  - sei orizzonti di previsione per la tratta: adesso, tra 1, 3, 6, 12 e 24 ore. Per ogni orizzonte il valore e' la MEDIA
//    dell'ora che comincia li' (quattro campioni, a 7,5 - 22,5 - 37,5 - 52,5 minuti): indice della tratta (somma di
//    ZHR x efficienza degli sciami attivi), lato prevalente, sciame che contribuisce di piu' e, quando c'e' il dato del radar,
//    le riflessioni all'ora della previsione sperimentale (Collm, regole SM6FHZ).
// I vecchi riquadri "Adesso" e "Previsione sperimentale" sono confluiti negli orizzonti.
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace YAMS
{
    public partial class MainWindow
    {
        static readonly int[] Horizons = { 0, 1, 3, 6, 12, 24 };
        TextBlock[] hzText;
        Border[] hzCard;

        Control BuildHorizonBand()
        {
            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("1.15*,1.35*,*,*,*,*,*,*"), Margin = new Thickness(4, 2, 4, 2) };
            cardBest = Card(Lang.T("ORA MIGLIORE DEL GIORNO"), Pal.Azure, out glBest);
            cardShower = Card(Lang.T("SCIAME PIÙ FAVOREVOLE"), Pal.Yellow, out glShower);
            Grid.SetColumn(cardBest, 0); Grid.SetColumn(cardShower, 1);
            g.Children.Add(cardBest); g.Children.Add(cardShower);
            hzText = new TextBlock[Horizons.Length];
            hzCard = new Border[Horizons.Length];
            string tip = Lang.T("Previsione per la tratta scelta: media dell'ora che comincia a quell'orizzonte (quattro campioni nell'ora). Indice della tratta = somma di ZHR × efficienza degli sciami attivi; poi il lato prevalente della tratta e lo sciame che contribuisce di più. Le riflessioni all'ora vengono dalla previsione sperimentale (radar di Collm, regole SM6FHZ), quando c'è il dato del radar.");
            for (int k = 0; k < Horizons.Length; k++)
            {
                int h = Horizons[k];
                string title = h == 0 ? Lang.T("ADESSO") : h == 1 ? Lang.T("TRA 1 ORA") : Lang.F("TRA {0} ORE", h);
                Border c = Card(title, h == 0 ? Pal.Green : Pal.Orange, out hzText[k]);
                ToolTip.SetTip(c, tip);
                hzCard[k] = c;
                Grid.SetColumn(c, 2 + k);
                g.Children.Add(c);
            }
            return new Border { Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(0, 0, 0, 1), Child = g };
        }

        // ogni minuto (OnTick) e dopo ogni calcolo (Update24h)
        void UpdateHorizons()
        {
            if (hzText == null) return;
            DateTime now = DateTime.UtcNow;
            DateTime hNow = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc);
            for (int k = 0; k < Horizons.Length; k++)
            {
                DateTime t0 = now.AddHours(Horizons[k]);
                string when = (t0.Date == now.Date ? t0.ToString("HH:mm", CultureInfo.InvariantCulture) : t0.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture));
                if (model == null || !model.Valid) { hzText[k].Text = when + " UTC\n-"; continue; }
                double idx = 0, wa = 0, wb = 0;
                Dictionary<string, double> per = new Dictionary<string, double>();
                const int N = 4;
                for (int i = 0; i < N; i++)
                {
                    DateTime t = t0.AddMinutes(7.5 + 15 * i);
                    foreach (Shower s in Catalog.Showers)
                    {
                        if (!s.IsActive(t)) continue;
                        double z; Vec3 v = RadiantVec(s, t, out z);
                        if (z <= 0) continue;
                        double lf; double e = model.Efficiency(v, out lf);
                        double c = z * e / 100.0 / N;
                        if (c <= 0) continue;
                        idx += c; wa += c * lf; wb += c * (1 - lf);
                        double o; per.TryGetValue(s.Code, out o); per[s.Code] = o + c;
                    }
                }
                string top = ""; double topV = 0;
                foreach (KeyValuePair<string, double> kv in per) if (kv.Value > topV) { topV = kv.Value; top = kv.Key; }
                string l2 = idx > 0.05 ? Lang.F("lato {0} · {1}", wa >= wb ? "A" : "B", top) : Lang.T("nessuno sciame utile");
                double fc = ForecastAt(hNow.AddHours(Horizons[k]));
                string l3 = fc >= 0 ? Lang.F("{0:F0} rifl./ora previste", fc) : (k == 0 && recommendShort.Length > 0 ? recommendShort.Trim() : "");
                hzText[k].Text = when + " UTC\n" + Lang.F("indice {0:F1}", idx) + "\n" + l2 + (l3.Length > 0 ? "\n" + l3 : "");
            }
        }
    }
}
