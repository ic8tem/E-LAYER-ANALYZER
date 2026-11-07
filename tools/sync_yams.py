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
s = rep(s, 'public partial class MainWindow : Window\n    {', '''public partial class MainWindow : UserControl
    {
        // E-LAYER ANALYZER: YA-MS e' un pannello (UserControl) nella linguetta METEOR SCATTER (tools/sync_yams.py)
        Window Win { get { return TopLevel.GetTopLevel(this) as Window; } }
        IStorageProvider StorageProvider { get { TopLevel t = TopLevel.GetTopLevel(this); return t != null ? t.StorageProvider : null; } }
        public static Action<string> LanguageRequested;   // cambio di lingua: lo esegue la finestra di E-LAYER ANALYZER
        public static Action<int> ThemeRequested;         // cambio dello schema di colori: idem
        public static bool Hosted = true;                 // dentro E-LAYER ANALYZER: le righe telnet arrivano da DATA COLLECTION
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
s = rep(s, '                                    Text = tag,\n', '                                    Text = Lang.T(tag),\n', 'etichette FONTE/RILIEVO/SCELTA')
wr(p, s)

p = os.path.join(ui, 'MainWindow.Screenshot.cs'); s = rd(p)
s = rep(s, 'public async Task RunScreenshots(string dir, string dx, double hours)', 'public async Task RunScreenshots(string dir, string dx, double hours, Visual target)', 'RunScreenshots')
s = rep(s, 'rtb.Render(this);', 'rtb.Render(target ?? this);', 'Render')
s = rep(s, 'PixelSize ps = new PixelSize(Math.Max(100, (int)Bounds.Width), Math.Max(100, (int)Bounds.Height));',
        'Rect bb = (target ?? this).Bounds;\n                    PixelSize ps = new PixelSize(Math.Max(100, (int)bb.Width), Math.Max(100, (int)bb.Height));', 'Bounds')
s = s.replace('            Close();\n', '')
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
    s = rep(s, '            if (MsCollector.Started) return;', '            if (Hosted || MsCollector.Started) return;', 'RdAutoStart')
    wr(p, s)
    p = os.path.join(DST, 'Core', 'MsSpots.cs'); s = rd(p)
    s = rep(s, '        public static void Shutdown() { foreach (TelnetSession s in Sessions) s.Disconnect(); }',
            '        public static void Shutdown() { foreach (TelnetSession s in Sessions) s.Disconnect(); }\n\n'
            '        // E-LAYER ANALYZER: riga ricevuta da una sessione di DATA COLLECTION (0 = cluster, 1 = RBN, 2 = PSKReporter)\n'
            '        public static void Feed(int source, string line) { lock (lk) pending.Enqueue(new KeyValuePair<int, string>(source, line)); }',
            'MsCollector.Feed')
    wr(p, s)

# --- Lang: anche le voci di E-LAYER ANALYZER (data/lingue/ela-<codice>.txt)
p = os.path.join(ui, 'Lang.cs'); s = rd(p)
s = rep(s, '                string file = FindFile(code);\n                if (file != null)',
        '                foreach (string file in new[] { FindFile(code), FindFile("ela-" + code) })   // YA-MS + E-LAYER ANALYZER\n                if (file != null)', 'Lang.Load')
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

# --- Lingue di E-LAYER ANALYZER (tutte quelle di YAEC, piu' danese e tagalog): elenco e culture
LANGS = [('it','Italiano','it-IT'),('en','English','en-GB'),('de','Deutsch','de-DE'),('fr','Français','fr-FR'),('es','Español','es-ES'),
         ('pt','Português','pt-PT'),('nl','Nederlands','nl-NL'),('sv','Svenska','sv-SE'),('nb','Norsk (bokmål)','nb-NO'),('da','Dansk','da-DK'),
         ('is','Íslenska','is-IS'),('et','Eesti','et-EE'),('lv','Latviešu','lv-LV'),('lt','Lietuvių','lt-LT'),('ru','Русский','ru-RU'),
         ('uk','Українська','uk-UA'),('tr','Türkçe','tr-TR'),('af','Afrikaans','af-ZA'),('ar','العربية','ar-EG'),('he','עברית','he-IL'),
         ('ur','اردو','ur-PK'),('hi','हिन्दी','hi-IN'),('bn','বাংলা','bn-BD'),('id','Bahasa Indonesia','id-ID'),('tl','Tagalog','fil-PH'),
         ('zh','中文 (简体)','zh-CN'),('ja','日本語','ja-JP'),('ko','한국어','ko-KR')]
p = os.path.join(ui, 'Lang.cs'); s = rd(p)
langs = 'public static readonly string[][] Languages =\n        {\n' + ',\n'.join('            new[] { "%s", "%s" }' % (c, n) for c, n, _ in LANGS) + '\n        };'
cult = 'static readonly string[][] Cultures =\n        {\n' + ',\n'.join('            new[] { "%s", "%s" }' % (c, k) for c, _, k in LANGS) + '\n        };'
s, n1 = re.subn(r'public static readonly string\[\]\[\] Languages =\s*\{.*?\n\s*\};', langs, s, flags=re.S)
s, n2 = re.subn(r'static readonly string\[\]\[\] Cultures =\s*\{.*?\n\s*\};', cult, s, flags=re.S)
if n1 != 1 or n2 != 1: raise SystemExit('NON TROVATO: elenco delle lingue in Lang.cs')
wr(p, s)
print('lingue:', len(LANGS))
