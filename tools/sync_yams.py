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
wr(p, s)

p = os.path.join(ui, 'MainWindow.Screenshot.cs'); s = rd(p)
s = rep(s, 'public async Task RunScreenshots(string dir, string dx, double hours)', 'public async Task RunScreenshots(string dir, string dx, double hours, Visual target)', 'RunScreenshots')
s = rep(s, 'rtb.Render(this);', 'rtb.Render(target ?? this);', 'Render')
s = rep(s, 'PixelSize ps = new PixelSize(Math.Max(100, (int)Bounds.Width), Math.Max(100, (int)Bounds.Height));',
        'Rect bb = (target ?? this).Bounds;\n                    PixelSize ps = new PixelSize(Math.Max(100, (int)bb.Width), Math.Max(100, (int)bb.Height));', 'Bounds')
s = s.replace('            Close();\n', '')
wr(p, s)

# --- Lang: anche le voci di E-LAYER ANALYZER (data/lingue/ela-<codice>.txt)
p = os.path.join(ui, 'Lang.cs'); s = rd(p)
s = rep(s, '                string file = FindFile(code);\n                if (file != null)',
        '                foreach (string file in new[] { FindFile(code), FindFile("ela-" + code) })   // YA-MS + E-LAYER ANALYZER\n                if (file != null)', 'Lang.Load')
wr(p, s)

# --- MapData: mappa del mondo intero e scaricamento con un solo thread
p = os.path.join(DST, 'Core', 'MapData.cs'); s = rd(p)
s = rep(s, '''        public static List<PointF[]> Load(string file)
        {''', '''        // Mondo intero (E-LAYER ANALYZER: mappe di sporadico E, F.A.I. e aurora)
        public static List<PointF[]> LoadAll(string file)
        {
            world = true;
            try { return Load(file); }
            finally { world = false; }
        }

        [ThreadStatic] static bool world;

        public static List<PointF[]> Load(string file)
        {''', 'MapData.Load')
s = rep(s, 'bool inside = p.Y >= 15 && p.Y <= 82 && p.X >= -50 && p.X <= 75;', 'bool inside = world || (p.Y >= 15 && p.Y <= 82 && p.X >= -50 && p.X <= 75);', 'inside')
s = rep(s, '        public static bool EnsureFiles(string dataDir, out string msg)\n        {\n',
        '        public static bool EnsureFiles(string dataDir, out string msg)\n        {\n            lock (fileLock) return EnsureFilesLocked(dataDir, out msg);\n        }\n\n        static readonly object fileLock = new object();\n\n        static bool EnsureFilesLocked(string dataDir, out string msg)\n        {\n', 'EnsureFiles')
wr(p, s)
print('YA-MS allineato in', DST)
