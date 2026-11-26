// YA-MS - sottolinguetta "Crediti" (Documenti tecnici, dopo "Fonti"): programma, autore, contatti, licenza GPL-3.0 e
// crediti in schede su tre colonne come in YA-NEC (richiesta dell'autore, IC8TEM, 15/11/2026). Le fonti scientifiche
// complete restano nella linguetta "Fonti".
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;

namespace YAMS
{
    public partial class MainWindow
    {
        sealed class Credit
        {
            public string Title, Who, What, Link, License; public bool Translate;
            public Credit(string t, string w, string d, bool tr, string l, string lic) { Title = t; Who = w; What = d; Translate = tr; Link = l; License = lic; }
        }

        // licenze da tradurre (le altre, GPL-3.0, MIT, CC BY 4.0, OFL, sono nomi propri)
        static readonly string[] CreditLicT = { "fonte citata", "solo idee e formule, nessun codice", "dati dell'ente, citati", "dati pubblici, citazione richiesta", "servizio pubblico gratuito", "dominio pubblico" };

        static readonly Credit[] Credits =
        {
            new Credit("METEOR-SCATTER (articolo)", "Costantino Cerrotta IC8TEM e Gianluigi \"Gigi\" Sartori IV3GTH, in memoria di Angelo Farina IZ8IBB", "La fonte principale: geometria del meteor scatter, Hot A e Hot B, elevazione d'antenna e attenuazione di tratta.", true, "", "fonte citata"),
            new Credit("MS-Soft 5.1 / 5.2", "Ilkka Yrjölä OH5IY", "Il concetto di efficienza dello sciame, da cui nasce il calcolo di YA-MS.", true, "www.kolumbus.fi/oh5iy", "solo idee e formule, nessun codice"),
            new Credit("MCG_MS 1.61", "Wilhelm Rauh DL5MCG", "Il calcolatore di partenza dell'efficienza per direzioni e i suoi valori di prova.", true, "qsl.net/dl5mcg", "solo idee e formule, nessun codice"),
            new Credit("432 MHz Meteorscatter, RadiantCalc", "Ingolf SM6FHZ", "Le regole λ³ e λ² della previsione sperimentale, il punto medio della tratta e la direzione migliore.", true, "Nordic VUSHF 2025", "fonte citata"),
            new Credit("QTFGUI, Die Geometrie von Meteorscatterverbindungen", "Klaus von der Heide DJ5HG", "La diffusione a cono, la scelta tra Hot A e Hot B e il caso di prova Amburgo - Barcellona usato per collaudare il modello.", true, "", "fonte citata"),
            new Credit("Meteorscatter auf UKW", "Jens DF5HC; formula di DH9GCD", "La formula della direzione migliore usata nella riga di confronto del Calcolo dell'efficienza.", true, "", "fonte citata"),
            new Credit("Antenne Yagi", "Lionel VE7BQH, Günter Hoch DL6WU, J.D. Kraus, L.B. Cebik W4RNL, John Drew VK5DJ", "Il guadagno in funzione della lunghezza del boom e l'apertura del lobo d'antenna.", true, "", "fonte citata"),
            new Credit("Attenuazione di tratta", "D.W.R. McKinley, John GM8JBJ, John Worsnop G4BAO, ITU-R P.843", "La perdita delle scie sottodense, l'angolo beta tra scia e tratta e il raggio iniziale della scia.", true, "", "fonte citata"),
            new Credit("International Meteor Organization", "IMO", "Il calendario degli sciami: periodi, massimi, radianti, velocità e ZHR.", true, "www.imo.net", "fonte citata"),
            new Credit("IAU Meteor Data Center", "IAU MDC, Astronomical Institute of the Slovak Academy of Sciences", "Il catalogo degli sciami stabiliti, per il confronto dei radianti nella linguetta Sciami.", true, "www.ta3.sk/IAUC22DB/MDC2022", "dati pubblici, citazione richiesta"),
            new Credit("Global Meteor Network", "Denis Vida e collaboratori", "Flusso e ZHR misurati degli sciami e traiettorie del giorno.", true, "globalmeteornetwork.org", "CC BY 4.0"),
            new Credit("Radar meteorico di Collm", "Università di Lipsia (LIM) e IAP Kühlungsborn", "Il flusso orario delle meteore, base della previsione sperimentale.", true, "wetterdaten.meteo.uni-leipzig.de", "dati dell'ente, citati"),
            new Credit("Radar meteorico di Sodankylä", "Sodankylä Geophysical Observatory", "Il conteggio delle meteore in diretta.", true, "sgo.fi", "dati dell'ente, citati"),
            new Credit("IPRMO, file RMOJ", "International Project for Radio Meteor Observations e gli osservatori HRO giapponesi", "I conteggi HRO mensili del Giappone (fari JA2SDA, JE3AKE, JJ1RLQ).", true, "www.iprmo.org", "dati dell'ente, citati"),
            new Credit("HRO di Ogaki (HROFFT)", "Stazione HRO di Ogaki; fari JA2SDA (Toyokawa) e JE3AKE (Otsu)", "Il conteggio automatico in diretta dei fari giapponesi a 53 MHz.", true, "hro-ogaki.org", "dati dell'ente, citati"),
            new Credit("Open-Meteo e modello GFS", "Open-Meteo.com; NOAA/NCEP", "I profili di temperatura e umidità per la rifrazione troposferica facoltativa.", true, "open-meteo.com", "CC BY 4.0"),
            new Credit("ITU-R P.453", "Unione Internazionale delle Telecomunicazioni", "La formula della rifrattività e l'atmosfera di riferimento.", true, "www.itu.int", "fonte citata"),
            new Credit("CMOR", "University of Western Ontario e NASA Meteoroid Environment Office", "Le mappe dei radianti misurate dal radar canadese.", true, "fireballs.ndc.nasa.gov/cmor-radiants", "dati dell'ente, citati"),
            new Credit("RMOB", "Pierre Terrier e gli osservatori di Radio Meteor Observatories On Line", "I conteggi radio orari degli osservatori amatoriali.", true, "www.rmob.org", "dati dell'ente, citati"),
            new Credit("Radar GRAVES", "Rob Hardenberg PE1ITR; Bolidozor", "Posizione del trasmettitore, settori e tempi del fascio.", true, "pe1itr.com/graves", "fonte citata"),
            new Credit("PSKReporter", "Philip Gladstone N1DQ; flusso MQTT di Tom M0LTE", "Gli spot e le stazioni in ascolto del Traffico in diretta, e il rimando dei rapporti.", true, "pskreporter.info - mqtt.pskreporter.info", "servizio pubblico gratuito"),
            new Credit("Reverse Beacon Network e DX cluster", "RBN; cluster DM4X e ON0DXK", "Gli spot della Raccolta dati.", true, "reversebeacon.net", "servizio pubblico gratuito"),
            new Credit("CALL3", "MMMonVHF", "L'archivio dei nominativi e dei locator, aggiornato ogni 12 ore.", true, "www.mmmonvhf.de", "servizio pubblico gratuito"),
            new Credit("Locator per nominativo", "G7VRD", "Il locator dei nominativi che mancano nel CALL3.", true, "api.g7vrd.co.uk", "servizio pubblico gratuito"),
            new Credit("JTDX", "Il gruppo di sviluppo di JTDX", "Il file grid_data.bin dei locator, usato come ultimo livello per i locator mancanti.", true, "github.com/jtdx-project", "GPL-3.0"),
            new Credit("WSJT-X", "Joe Taylor K1JT e il gruppo WSJT", "Il protocollo UDP con cui YA-MS riceve le decodifiche, anche da JTDX e MSHV.", true, "wsjt.sourceforge.io", "GPL-3.0"),
            new Credit("MSHV", "Christo Hristov LZ2HV", "Il riferimento per la posizione degli hot spot ai lati della tratta.", true, "sourceforge.net/projects/mshv", "fonte citata"),
            new Credit("MSQP", "Rob Suggs NN4NT, HamSCI Meteor Scatter Working Group", "Un confronto per la previsione degli sciami.", true, "github.com/HamSCI/MSQP", "fonte citata"),
            new Credit("Swan Meteor Skyview", "DL1DBC", "Il riferimento per la grafica della volta celeste.", true, "swan.ms", "solo idee e formule, nessun codice"),
            new Credit("Broker MQTT pubblici", "HiveMQ, Eclipse Mosquitto, EMQX", "Lo scambio cifrato dei messaggi tra gli utenti di YA-MS.", true, "broker.hivemq.com - test.mosquitto.org - broker.emqx.io", "servizio pubblico gratuito"),
            new Credit("WGS 84", "National Geospatial-Intelligence Agency (NGA)", "L'ellissoide della Terra (semiasse e schiacciamento).", true, "earth-info.nga.mil", "fonte citata"),
            new Credit("GeographicLib e GeographicLib.NET", "Charles F. F. Karney; i collaboratori di GeographicLib.NET", "Distanze, azimut e punti della tratta sulla geodetica dell'ellissoide WGS84.", true, "geographiclib.sourceforge.io - github.com/noelex/GeographicLib.NET", "MIT"),
            new Credit("Natural Earth", "Natural Earth", "Le coste e i confini delle mappe.", true, "www.naturalearthdata.com", "dominio pubblico"),
            new Credit("Foto della schermata di avvio", "ESO/S. Guisard", "\"The 2010 Perseids over the VLT\".", false, "www.eso.org", "CC BY 4.0"),
            new Credit("Avalonia UI 11.2.3, .NET 8 e .NET 6", "Il gruppo di Avalonia UI; Microsoft e .NET Foundation", "L'interfaccia multipiattaforma (Windows, Linux, Mac) e l'ambiente di esecuzione.", true, "github.com/AvaloniaUI/Avalonia - dotnet.microsoft.com", "MIT"),
            new Credit("Carattere Inter", "Rasmus Andersson", "Il carattere dell'interfaccia.", true, "rsms.me/inter", "SIL Open Font License 1.1"),
        };

        static string LicName(string l) { return Array.IndexOf(CreditLicT, l) >= 0 ? Lang.T(l) : l; }

        Border CreditCard(Credit c)
        {
            StackPanel sp = new StackPanel { Spacing = 3 };
            sp.Children.Add(new TextBlock { Text = c.Title, FontWeight = FontWeight.SemiBold, FontSize = 14, Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap });
            sp.Children.Add(new TextBlock { Text = c.Who, FontSize = 12.5, Foreground = Pal.B(Pal.Azure), TextWrapping = TextWrapping.Wrap });
            sp.Children.Add(new TextBlock { Text = c.Translate ? Lang.T(c.What) : c.What, FontSize = 12.5, Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 2, 0, 0) });
            string foot = (c.Link.Length > 0 ? c.Link + "   " : "") + Lang.T("licenza") + ": " + LicName(c.License);
            sp.Children.Add(new SelectableTextBlock { Text = foot, FontSize = 11, Foreground = Pal.B(Pal.Muted), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 2, 0, 0) });
            return new Border
            {
                Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8), Padding = new Thickness(12, 10), Margin = new Thickness(5), Child = sp
            };
        }

        void BuildCreditsTab()
        {
            DockPanel p = Page(Lang.T("Crediti"));
            StackPanel col = new StackPanel { Margin = new Thickness(14, 10, 14, 14), Spacing = 6 };

            // programma, autore, contatti, licenza
            StackPanel head = new StackPanel { Spacing = 3 };
            head.Children.Add(new TextBlock { Text = AppPaths.AppName + " " + AppPaths.Version, FontSize = 18, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Azure) });
            head.Children.Add(new SelectableTextBlock { Text = Lang.T("Autore") + ": Costantino Cerrotta IC8TEM", FontSize = 13, Foreground = Pal.B(Pal.Fore) });
            head.Children.Add(new SelectableTextBlock { Text = Lang.T("Contatti") + ": ccerrotta@gmail.com · ic8tem@gmail.com · www.qrz.com/db/ic8tem · ko-fi.com/ic8tem", FontSize = 13, Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap });
            head.Children.Add(new SelectableTextBlock { Text = Lang.T("Codice sorgente") + ": github.com/ic8tem/YA-MS", FontSize = 13, Foreground = Pal.B(Pal.Fore) });
            head.Children.Add(new SelectableTextBlock { Text = Lang.T("Licenza") + ": GNU GPL-3.0. " + Lang.T("YA-MS è software libero: puoi ridistribuirlo e modificarlo secondo i termini della GNU General Public License versione 3 (file COPYING). È distribuito senza alcuna garanzia."), FontSize = 12.5, Foreground = Pal.B(Pal.Muted), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 2, 0, 0) });
            col.Children.Add(new Border
            {
                Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8), Padding = new Thickness(14, 10), Margin = new Thickness(5, 0, 5, 6), Child = head
            });

            col.Children.Add(new TextBlock { Text = Lang.T("Crediti e ringraziamenti"), FontSize = 16, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(5, 4, 5, 0) });
            col.Children.Add(new TextBlock { Text = Lang.T("YA-MS esiste grazie al lavoro di colleghi radioamatori, ricercatori ed enti che rendono pubblici i loro dati e il loro codice. Un ringraziamento sentito a ciascuno di loro."), FontSize = 12.5, Foreground = Pal.B(Pal.Muted), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(5, 0, 5, 4) });

            UniformGrid grid = new UniformGrid { Columns = 3 };
            foreach (Credit c in Credits) grid.Children.Add(CreditCard(c));
            col.Children.Add(grid);

            p.Children.Add(new ScrollViewer { Content = col, HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled, VerticalScrollBarVisibility = ScrollBarVisibility.Auto });
        }
    }
}
