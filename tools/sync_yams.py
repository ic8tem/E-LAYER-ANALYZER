# E-LAYER ANALYZER - allinea il pannello METEOR SCATTER all'ultima versione di YA-MS.
# Uso: python3 tools/sync_yams.py <cartella del repository YA-MS>
# Copia src/Core, src/Controls, src/UI di YA-MS in src/MeteorScatter e i dati di YA-MS in data/ (senza toccare
# i file di E-LAYER ANALYZER), poi applica le modifiche che trasformano la finestra di YA-MS in un pannello
# (UserControl) ospitato nella linguetta METEOR SCATTER. Ogni modifica controlla di aver trovato il suo testo.
import os, re, shutil, sys

YA = sys.argv[1]
EL = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DST = os.path.join(EL, 'src', 'MeteorScatter')

for sub in ('Core', 'Controls', 'UI'):
    d = os.path.join(DST, sub)
    if os.path.isdir(d): shutil.rmtree(d)
    shutil.copytree(os.path.join(YA, 'src', sub), d)

# dati di YA-MS (i file di E-LAYER ANALYZER restano)
ELA_DATA = ('ela_', 'call3', 'igrf14', 'demo')
for f in os.listdir(os.path.join(YA, 'data')):
    src = os.path.join(YA, 'data', f)
    if os.path.isfile(src) and not f.startswith(ELA_DATA) and f != 'config.ini':
        shutil.copy2(src, os.path.join(EL, 'data', f))
# sottocartelle dei dati di YA-MS (dalla 1.8.0: data/spiegazione con la spiegazione dell'efficienza in tutte le lingue)
for d in os.listdir(os.path.join(YA, 'data')):
    src = os.path.join(YA, 'data', d)
    if os.path.isdir(src) and d not in ('lingue', 'backup', 'aggiornamento'):
        dst = os.path.join(EL, 'data', d)
        if os.path.isdir(dst): shutil.rmtree(dst)
        shutil.copytree(src, dst)
os.makedirs(os.path.join(EL, 'data', 'lingue'), exist_ok=True)
for f in os.listdir(os.path.join(YA, 'data', 'lingue')):
    shutil.copy2(os.path.join(YA, 'data', 'lingue', f), os.path.join(EL, 'data', 'lingue', f))
shutil.copy2(os.path.join(YA, 'src', 'App.axaml'), os.path.join(EL, 'src', 'App.axaml'))

def rd(p): return open(p, encoding='utf-8-sig').read()
def wr(p, s): open(p, 'w', encoding='utf-8').write(s)

def rep(s, a, b, what):
    if a not in s: raise SystemExit('NON TROVATO (' + what + '): ' + a[:80])
    return s.replace(a, b)

def method_span(s, signature):
    """inizio e fine (dopo la graffa di chiusura) del metodo che inizia con signature"""
    i = s.index(signature)
    i = s.rfind('\n', 0, i) + 1
    j = s.index('{', i)
    depth = 0
    for k in range(j, len(s)):
        if s[k] == '{': depth += 1
        elif s[k] == '}':
            depth -= 1
            if depth == 0:
                return i, k + 1
    raise SystemExit('graffe non bilanciate: ' + signature)

ui = os.path.join(DST, 'UI')

# --- App.axaml: classe di E-LAYER ANALYZER
p = os.path.join(EL, 'src', 'App.axaml'); s = rd(p)
s = rep(s, 'x:Class="YAMS.App"', 'x:Class="ELA.App"', 'App.axaml')
wr(p, s)

# --- MainWindow.cs: finestra -> pannello
p = os.path.join(ui, 'MainWindow.cs'); s = rd(p)
if 'using Avalonia.Platform.Storage;' not in s:
    s = rep(s, 'using Avalonia.Platform;\n', 'using Avalonia.Platform;\nusing Avalonia.Platform.Storage;\n', 'using')
# dalla 1.8.11 YA-MS ha il suo Hosted = false: qui lo sostituisce quello del pannello
s = s.replace('        public static bool Hosted = false;   // true dentro E-LAYER ANALYZER (tools/sync_yams.py)\n', '')
s = rep(s, 'public partial class MainWindow : Window\n    {', '''public partial class MainWindow : UserControl
    {
        // E-LAYER ANALYZER: YA-MS e' un pannello (UserControl) nella linguetta METEOR SCATTER (tools/sync_yams.py)
        Window Win { get { return TopLevel.GetTopLevel(this) as Window; } }
        IStorageProvider StorageProvider { get { TopLevel t = TopLevel.GetTopLevel(this); return t != null ? t.StorageProvider : null; } }
        public static Action<string> LanguageRequested;   // cambio di lingua: lo esegue la finestra di E-LAYER ANALYZER
        public static Action<int> ThemeRequested;         // cambio dello schema di colori: idem
        bool openedOnce;
        public void SelectTab(int i) { if (tabs != null) tabs.Select(i); }
        public void SaveSettings() { SaveConfig(); }
        public void Shutdown(bool save)
        {
            if (timer != null) timer.Stop();
            if (save && !replaced) SaveConfig();
            replaced = true;
        }
''', 'classe')
# Hosted: dentro E-LAYER ANALYZER le righe telnet arrivano da DATA COLLECTION (dalla 1.8.11 YA-MS dichiara gia' il campo)
if re.search(r'public static bool Hosted = false;', s):
    s = re.sub(r'public static bool Hosted = false;', 'public static bool Hosted = true; ', s, count=1)
elif 'public static bool Hosted' not in s:
    s = rep(s, '        bool openedOnce;\n', '        public static bool Hosted = true;                 // dentro E-LAYER ANALYZER: le righe telnet arrivano da DATA COLLECTION\n        bool openedOnce;\n', 'Hosted')
s = re.sub(r'\n[ \t]*Title = AppPaths\.AppName[^\n]*', '', s)
s = re.sub(r'\n[ \t]*Width = 1600; Height = 980;', '', s)
s = re.sub(r'\n[ \t]*WindowStartupLocation = WindowStartupLocation\.CenterScreen;', '', s)
s = re.sub(r'\n[ \t]*try \{ Icon = new WindowIcon\([^\n]*', '', s)
m = re.search(r'Opened \+= OnOpened;\s*Closing \+= \(s, e\) =>\s*\{.*?\n\s*\};', s, re.S)
if not m: raise SystemExit('NON TROVATO: Opened/Closing')
s = s[:m.start()] + '''AttachedToVisualTree += (s, e) =>
            {
                if (openedOnce) return;
                openedOnce = true;
                OnOpened(s, EventArgs.Empty);
            };''' + s[m.end():]
a, b = method_span(s, 'void SwitchLanguage(string code)')
s = s[:a] + '''        void SwitchLanguage(string code)
        {
            if (replaced || code == Lang.Code) return;
            if (LanguageRequested != null) LanguageRequested(code);
        }''' + s[b:]
if 'void SwitchTheme(int schema)' in s:
    a, b = method_span(s, 'void SwitchTheme(int schema)')
    s = s[:a] + '''        void SwitchTheme(int schema)
        {
            if (replaced || schema == Tema.Schema) return;
            if (ThemeRequested != null) ThemeRequested(schema);
        }''' + s[b:]
if 'void ReplaceWindow()' in s:
    a, b = method_span(s, 'void ReplaceWindow()')
    s = s[:a] + s[b:]
# lingua e schema si scelgono nella barra di E-LAYER ANALYZER: nel pannello sono nascosti
s = rep(s, 'tabs.Line.Children.Insert(0, cmbLang);', 'tabs.Line.Children.Insert(0, cmbLang);\n            cmbLang.IsVisible = false;', 'cmbLang')
if 'tabs.Line.Children.Insert(2, lblTheme);' in s:
    s = s.replace('tabs.Line.Children.Insert(2, lblTheme);', 'tabs.Line.Children.Insert(2, lblTheme);\n            cmbTheme.IsVisible = false; lblTheme.IsVisible = false;')
wr(p, s)

# --- tutti i file dell'interfaccia: messaggi con la finestra che ospita il pannello
for f in os.listdir(ui):
    if not f.startswith('MainWindow'): continue
    p = os.path.join(ui, f); s = rd(p)
    s = s.replace('Dialogs.Message(this,', 'Dialogs.Message(Win,').replace('Dialogs.Confirm(this,', 'Dialogs.Confirm(Win,')
    wr(p, s)

p = os.path.join(ui, 'MainWindow.Spiegazione.cs'); s = rd(p)
s = rep(s, '        static void FillExplain(StackPanel doc, string file)', '        internal static void FillExplain(StackPanel doc, string file)', 'FillExplain')
if 'Text = Lang.T(tag),' not in s:   # dalla 1.8.0 YA-MS traduce gia' le etichette
    s = rep(s, '                                    Text = tag,\n', '                                    Text = Lang.T(tag),\n', 'etichette FONTE/RILIEVO/SCELTA')
# E-LAYER ANALYZER: righe di tabella "TAB|cella;cella;..." (la prima riga di ogni gruppo e' l'intestazione)
s = rep(s, '                    case "SRC":\n',
        '                    case "TAB":\n'
        '                        {\n'
        '                            string[] cells = text.Split(\';\');\n'
        '                            Grid prev = doc.Children.Count > 0 ? doc.Children[doc.Children.Count - 1] as Grid : null;\n'
        '                            bool head = prev == null || !"tab".Equals(prev.Tag);\n'
        '                            Grid row = new Grid { Tag = "tab", Margin = new Thickness(14, head ? 4 : 0, 14, 0) };\n'
        '                            for (int ci = 0; ci < cells.Length; ci++)\n'
        '                            {\n'
        '                                row.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(ci == 0 ? 1.3 : 1, GridUnitType.Star)));\n'
        '                                Border cb = new Border\n'
        '                                {\n'
        '                                    Background = Pal.B(head ? Pal.Rgb(232, 236, 248) : Pal.Rgb(255, 255, 255)),\n'
        '                                    BorderBrush = Pal.B(Pal.Rgb(200, 205, 225)),\n'
        '                                    BorderThickness = new Thickness(ci == 0 ? 1 : 0, head ? 1 : 0, 1, 1),\n'
        '                                    Padding = new Thickness(8, 4),\n'
        '                                    Child = DocText(cells[ci].Trim(), 13, head ? DocH1 : DocInk, head || ci == 0 ? FontWeight.SemiBold : FontWeight.Normal, FontStyle.Normal, new Thickness(0))\n'
        '                                };\n'
        '                                Grid.SetColumn(cb, ci); row.Children.Add(cb);\n'
        '                            }\n'
        '                            doc.Children.Add(row);\n'
        '                        }\n'
        '                        break;\n'
        '                    case "SRC":\n', 'righe di tabella TAB')
wr(p, s)

p = os.path.join(ui, 'MainWindow.Screenshot.cs'); s = rd(p)
s = rep(s, 'public async Task RunScreenshots(string dir, string dx, double hours)', 'public async Task RunScreenshots(string dir, string dx, double hours, Visual target)', 'RunScreenshots')
s = rep(s, 'rtb.Render(this);', 'rtb.Render(target ?? this);', 'Render')
s = rep(s, 'PixelSize ps = new PixelSize(Math.Max(100, (int)Bounds.Width), Math.Max(100, (int)Bounds.Height));',
        'Rect bb = (target ?? this).Bounds;\n                    PixelSize ps = new PixelSize(Math.Max(100, (int)bb.Width), Math.Max(100, (int)bb.Height));', 'Bounds')
s = s.replace('            Close();\n', '')
# dalla 1.8.1 anche Shot() fotografa: il pannello da fotografare sta in un campo
s = s.replace('(target ?? this)', '(shotTarget ?? this)')
s = rep(s, 'public async Task RunScreenshots(string dir, string dx, double hours, Visual target)\n        {\n',
        'Visual shotTarget;   // E-LAYER ANALYZER: finestra da fotografare (tools/sync_yams.py)\n\n        public async Task RunScreenshots(string dir, string dx, double hours, Visual target)\n        {\n            shotTarget = target;\n', 'shotTarget')
wr(p, s)

# --- Raccolta dati di YA-MS (versioni successive alla 1.4.0): dentro E-LAYER ANALYZER non apre sessioni telnet
#     proprie (sarebbero doppioni con lo stesso nominativo sugli stessi cluster): riceve le righe delle sessioni di
#     DATA COLLECTION (MsCollector.Feed) e ne filtra gli spot meteor scatter.
p = os.path.join(ui, 'MainWindow.Raccolta.cs')
if os.path.exists(p):
    s = rd(p)
    # In E-LAYER ANALYZER la raccolta dei dati e' una sola (linguetta DATA COLLECTION): la sottolinguetta "Raccolta dati"
    # di YA-MS non si mostra, ma si costruisce lo stesso (tabella, contatori, filtro) e il suo filtro va nella
    # colonna dei filtri di DATA COLLECTION (MainWindow.MsFilterPanel).
    s = rep(s, '        TextBox txtRdLogin, txtRdTest;',
            '        public static Control MsFilterPanel;   // E-LAYER ANALYZER: filtro MS mostrato in DATA COLLECTION\n        TextBox txtRdLogin, txtRdTest;',
            'MsFilterPanel')
    s = rep(s, '            DockPanel p = Page(Lang.T("Raccolta dati"));',
            '            DockPanel p = Hosted ? new DockPanel() : Page(Lang.T("Raccolta dati"));   // dentro E-LAYER ANALYZER: raccolta unica in DATA COLLECTION',
            'Raccolta nascosta')
    s = rep(s, '            for (int i = 0; i < 3; i++) inner.Add((i + 1) + ". " + shortName[i], SessionBox(i, cols[i]));',
            '            // i riquadri si creano sempre: i loro campi servono all\'aggiornamento e al salvataggio\n'
            '            for (int i = 0; i < 3; i++) { Border sbx = SessionBox(i, cols[i]); if (!Hosted) inner.Add((i + 1) + ". " + shortName[i], sbx); }',
            'sessioni della Raccolta dati')
    s = rep(s, '            fp.Children.Add(chkLow);',
            '            chkLow.IsVisible = !Hosted;   // in E-LAYER ANALYZER c\'e\' gia\' il filtro locale di DATA COLLECTION\n            fp.Children.Add(chkLow);',
            'chkLow')
    s = rep(s, '            Grid.SetColumn(right, 1); g.Children.Add(right);',
            '            Grid.SetColumn(right, 1); g.Children.Add(right);\n'
            '            if (Hosted) { ((ScrollViewer)right.Child).Content = null; MsFilterPanel = fp; }',
            'filtro in DATA COLLECTION')
    # dalla 1.8.10: PSKReporter MQTT, WSJT-X UDP e i comandi di CALL3 si mostrano in DATA COLLECTION (raccolta unica);
    # all'avvio si connette solo MQTT (le sessioni telnet le apre DATA COLLECTION)
    s = rep(s, '            inner.Add("4. " + Lang.T("PSKReporter MQTT"), MqttBox(Pal.Green));\n            inner.Add("5. WSJT-X UDP", UdpBox(Pal.Azure));',
            '            Border mqb = MqttBox(Pal.Green), udb = UdpBox(Pal.Azure);\n'
            '            if (Hosted) { MqttPanel = mqb; UdpPanel = udb; }   // E-LAYER ANALYZER: in DATA COLLECTION\n'
            '            else { inner.Add("4. " + Lang.T("PSKReporter MQTT"), mqb); inner.Add("5. WSJT-X UDP", udb); }',
            'MQTT e UDP in DATA COLLECTION')
    s = rep(s, '        public static Control MsFilterPanel;',
            '        public static Control MsFilterPanel, MqttPanel, UdpPanel, Call3Panel;', 'pannelli statici')
    s = rep(s, '            DockTop(left, Row(L("CALL3"), bC3Upd, chkC3Auto, chkG7, bC3Add, bC3Man, lblC3Status));',
            '            Border c3row = Row(L("CALL3"), bC3Upd, chkC3Auto, chkG7, bC3Add, bC3Man, lblC3Status);\n'
            '            if (Hosted) { lblRdCall3.Margin = new Thickness(8, 4); Call3Panel = new StackPanel { Children = { c3row, lblRdCall3 } }; }   // E-LAYER ANALYZER: pagina CALL3\n'
            '            else DockTop(left, c3row);',
            'CALL3 in DATA COLLECTION')
    s = rep(s, '            DockTop(left, Row(L(Lang.T("Nominativo per il login")), txtRdLogin, bImp, lblRdCall3));',
            '            if (!Hosted) DockTop(left, Row(L(Lang.T("Nominativo per il login")), txtRdLogin, bImp, lblRdCall3));',
            'riga login nascosta')
    # in E-LAYER ANALYZER una scritta in cima al filtro MS: vale solo per la linguetta METEOR SCATTER
    s = rep(s, '            fp.Children.Add(new TextBlock { Text = Lang.T("Filtro degli spot meteor scatter"), FontSize = 17, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Azure) });',
            '            fp.Children.Add(new TextBlock { Text = Lang.T("Filtro degli spot meteor scatter"), FontSize = 17, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Azure) });\n'
            '            if (Hosted) fp.Children.Add(new TextBlock { Text = Lang.T("Questi filtri, che vengono da YA-MS (CW, distanza minima, finestre di frequenza, modi MS, bande), valgono solo per la linguetta METEOR SCATTER: decidono quali spot entrano nella sua raccolta. Le altre linguette ricevono tutti gli spot e usano i propri filtri."), TextWrapping = TextWrapping.Wrap, FontSize = 12.5, Foreground = Pal.B(Pal.Orange) });',
            'nota filtro MS')
    s = rep(s, '            if (MsCollector.Started) return;\n            MsCollector.Started = true;\n',
            '            if (MsCollector.Started) return;\n            MsCollector.Started = true;\n'
            '            if (Hosted) { if (mqStart != null && mqStart.IsChecked == true) MqConnect(); return; }   // E-LAYER ANALYZER: telnet da DATA COLLECTION\n',
            'RdAutoStart')
    wr(p, s)
    p = os.path.join(DST, 'Core', 'MsSpots.cs'); s = rd(p)
    m = re.search(r'\n        public static void Shutdown\(\) \{[^\n]*\}', s)   # dalla 1.8.0 chiude anche PskMqtt
    if not m: raise SystemExit('NON TROVATO (MsCollector.Shutdown)')
    s = rep(s, m.group(0)[1:],
            m.group(0)[1:] + '\n\n'
            '        // E-LAYER ANALYZER: riga ricevuta da una sessione di DATA COLLECTION (0 = cluster, 1 = RBN, 2 = PSKReporter)\n'
            '        public static void Feed(int source, string line) { lock (lk) pending.Enqueue(new KeyValuePair<int, string>(source, line)); }',
            'MsCollector.Feed')
    wr(p, s)

# --- Aggiornamento automatico di YA-MS (dalla 1.8.0): dentro E-LAYER ANALYZER lo fa E-LAYER ANALYZER (src/Ela/Updater.cs),
#     quindi il pannello non controlla ne' installa le Release di YA-MS e il pulsante Aggiornamenti non compare
p = os.path.join(ui, 'MainWindow.Aggiornamenti.cs')
if os.path.exists(p):
    wr(p, '''// E-LAYER ANALYZER: l'aggiornamento di YA-MS e' disattivato nel pannello (tools/sync_yams.py); aggiorna E-LAYER ANALYZER
using System.Threading.Tasks;
using Avalonia.Controls;

namespace YAMS
{
    public partial class MainWindow
    {
        Button btnUpdate;
        Task StartupUpdateChecks() { return Task.CompletedTask; }
        void CheckUpdatesManual() { }
    }
}
''')
    up = os.path.join(DST, 'Core', 'Updater.cs')
    if os.path.exists(up): os.remove(up)
    p = os.path.join(ui, 'MainWindow.cs'); s = rd(p)
    s = rep(s, '            sbLine.Children.Add(btnUpdate);', '            btnUpdate.IsVisible = false;   // E-LAYER ANALYZER: aggiornamento dalla finestra principale\n            sbLine.Children.Add(btnUpdate);', 'pulsante Aggiornamenti')
    wr(p, s)

# --- Versione di YA-MS (dalla 1.8.0 letta dall'assembly, che dentro E-LAYER ANALYZER e' quello di E-LAYER ANALYZER):
#     si scrive quella del file VERSION di YA-MS
p = os.path.join(DST, 'Core', 'AppPaths.cs'); s = rd(p)
if 'Version = ReadVersion();' in s:
    yv = open(os.path.join(YA, 'VERSION'), encoding='utf-8').read().strip()
    s = rep(s, 'public static readonly string Version = ReadVersion();',
            'public static readonly string Version = "' + yv + '";   // E-LAYER ANALYZER: versione di YA-MS incorporata (tools/sync_yams.py)', 'AppPaths.Version')
    wr(p, s)

# --- Lang: anche le voci di E-LAYER ANALYZER (data/lingue/ela-<codice>.txt)
p = os.path.join(ui, 'Lang.cs'); s = rd(p)
s = rep(s, '                string file = FindFile(code);\n                if (file != null)',
        '                foreach (string file in new[] { FindFile(code), FindFile("ela-" + code) })   // YA-MS + E-LAYER ANALYZER\n                if (file != null)', 'Lang.Load')
wr(p, s)

# --- Lang: varianti di lingua del sistema (portoghese brasiliano, cinese tradizionale)
p = os.path.join(ui, 'Lang.cs'); s = rd(p)
A = "                    string os = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();"
if '"pt-br"; else' not in s and 'os = "pt-br";' not in s:   # dalla 1.7.0 YA-MS riconosce gia' le varianti
  s = rep(s, A, A + '''
                    string full = CultureInfo.CurrentUICulture.Name.ToLowerInvariant();   // varianti: portoghese brasiliano, cinese tradizionale
                    if (full == "pt-br") os = "pt-br";
                    else if (full == "zh-tw" || full == "zh-hk" || full == "zh-mo" || full.StartsWith("zh-hant")) os = "zh-tw";''', 'Lang.Init')
wr(p, s)

# --- MapData: mappa del mondo intero e scaricamento con un solo thread
p = os.path.join(DST, 'Core', 'MapData.cs'); s = rd(p)
LOADALL = '''        // Mondo intero (E-LAYER ANALYZER: mappe di sporadico E, F.A.I. e aurora)
        public static List<PointF[]> LoadAll(string file)
        {
            world = true;
            try { return Load(file); }
            finally { world = false; }
        }

        [ThreadStatic] static bool world;

'''
if 'public static List<PointF[]> Load(string file) { return Load(file, -50, 75); }' in s:   # YA-MS 1.4.0 e successive
    s = rep(s, '        public static List<PointF[]> Load(string file) { return Load(file, -50, 75); }',
            LOADALL + '        public static List<PointF[]> Load(string file) { return Load(file, -50, 75); }', 'MapData.Load')
    s = rep(s, 'bool inside = p.Y >= 15 && p.Y <= 82 && p.X >= lonMin && p.X <= lonMax;',
            'bool inside = world || (p.Y >= 15 && p.Y <= 82 && p.X >= lonMin && p.X <= lonMax);', 'inside')
else:
    s = rep(s, '''        public static List<PointF[]> Load(string file)
        {''', LOADALL + '''        public static List<PointF[]> Load(string file)
        {''', 'MapData.Load')
    s = rep(s, 'bool inside = p.Y >= 15 && p.Y <= 82 && p.X >= -50 && p.X <= 75;', 'bool inside = world || (p.Y >= 15 && p.Y <= 82 && p.X >= -50 && p.X <= 75);', 'inside')
s = rep(s, '        public static bool EnsureFiles(string dataDir, out string msg)\n        {\n',
        '        public static bool EnsureFiles(string dataDir, out string msg)\n        {\n            lock (fileLock) return EnsureFilesLocked(dataDir, out msg);\n        }\n\n        static readonly object fileLock = new object();\n\n        static bool EnsureFilesLocked(string dataDir, out string msg)\n        {\n', 'EnsureFiles')
wr(p, s)
print('YA-MS allineato in', DST)

# --- Lingue di E-LAYER ANALYZER (tutte quelle di YAEC, piu' danese e tagalog; dalla 0.4.4 anche pt-br, zh-tw, pl, cs, hu, ro, el, th): elenco e culture
LANGS = [('it','Italiano','it-IT'),('en','English','en-GB'),('de','Deutsch','de-DE'),('fr','Français','fr-FR'),('es','Español','es-ES'),
         ('pt','Português','pt-PT'),('pt-br','Português (Brasil)','pt-BR'),('nl','Nederlands','nl-NL'),('sv','Svenska','sv-SE'),('nb','Norsk (bokmål)','nb-NO'),('da','Dansk','da-DK'),
         ('is','Íslenska','is-IS'),('et','Eesti','et-EE'),('lv','Latviešu','lv-LV'),('lt','Lietuvių','lt-LT'),('pl','Polski','pl-PL'),('cs','Čeština','cs-CZ'),('hu','Magyar','hu-HU'),('ro','Română','ro-RO'),('el','Ελληνικά','el-GR'),('ru','Русский','ru-RU'),
         ('uk','Українська','uk-UA'),('tr','Türkçe','tr-TR'),('af','Afrikaans','af-ZA'),('ar','العربية','ar-EG'),('he','עברית','he-IL'),
         ('ur','اردو','ur-PK'),('hi','हिन्दी','hi-IN'),('bn','বাংলা','bn-BD'),('id','Bahasa Indonesia','id-ID'),('tl','Tagalog','fil-PH'),
         ('zh','中文 (简体)','zh-CN'),('zh-tw','中文 (繁體)','zh-TW'),('th','ไทย','th-TH'),('ja','日本語','ja-JP'),('ko','한국어','ko-KR')]
p = os.path.join(ui, 'Lang.cs'); s = rd(p)
langs = 'public static readonly string[][] Languages =\n        {\n' + ',\n'.join('            new[] { "%s", "%s" }' % (c, n) for c, n, _ in LANGS) + '\n        };'
cult = 'static readonly string[][] Cultures =\n        {\n' + ',\n'.join('            new[] { "%s", "%s" }' % (c, k) for c, _, k in LANGS) + '\n        };'
s, n1 = re.subn(r'public static readonly string\[\]\[\] Languages =\s*\{.*?\n\s*\};', langs, s, flags=re.S)
s, n2 = re.subn(r'static readonly string\[\]\[\] Cultures =\s*\{.*?\n\s*\};', cult, s, flags=re.S)
if n1 != 1 or n2 != 1: raise SystemExit('NON TROVATO: elenco delle lingue in Lang.cs')
wr(p, s)
print('lingue:', len(LANGS))

# --- Linguetta Impostazioni di YA-MS (dalla 1.8.11): dentro E-LAYER ANALYZER carattere, colorazione, lingua e
#     aggiornamenti stanno nella linguetta IMPOSTAZIONI di E-LAYER ANALYZER; qui resta una nota (dalla 0.5.0)
p = os.path.join(ui, 'MainWindow.Impostazioni.cs')
if os.path.exists(p):
    s = rd(p)
    A = '            col.Children.Add(SettingsSection(Lang.T("Aspetto"), Pal.Azure, Lang.T("Tema e lingua sono anche in alto a destra, accanto alle linguette."), look.ToArray()));'
    s = rep(s, A, '            if (Hosted) col.Children.Add(SettingsSection(Lang.T("Aspetto"), Pal.Azure, Lang.T("Dentro E-LAYER ANALYZER carattere, colorazione, lingua e aggiornamenti si scelgono nella linguetta IMPOSTAZIONI di E-LAYER ANALYZER e valgono anche per METEOR SCATTER.")));   // E-LAYER ANALYZER (tools/sync_yams.py)\n            else ' + A.strip(), 'Aspetto nelle Impostazioni di YA-MS')
    wr(p, s)

# --- Temi di IP-TRX-CTRL (e colori in piu' di YAEC) aggiunti agli schemi di YA-MS (dalla 0.5.0, tools/temi_ip.py)
import subprocess
subprocess.check_call([sys.executable, os.path.join(EL, 'tools', 'temi_ip.py')])
