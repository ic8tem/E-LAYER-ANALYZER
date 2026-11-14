// YA-MS - cartelle del programma: tutti i dati stanno nella cartella "data" accanto al programma
// (Windows C:\YA-MS, Linux ~/YA-MS, Mac ~/YA-MS accanto a YA-MS.app). Niente AppData.
using System;
using System.IO;

namespace YAMS
{
    public static class AppPaths
    {
        public const string AppName = "YA-MS";
        // versione dal file VERSION (scritta nell'eseguibile durante la compilazione), senza il codice del commit
        public static readonly string Version = "1.8.9";   // E-LAYER ANALYZER: versione di YA-MS incorporata (tools/sync_yams.py)
        static string ReadVersion()
        {
            try
            {
                System.Reflection.AssemblyInformationalVersionAttribute a = (System.Reflection.AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(
                    typeof(AppPaths).Assembly, typeof(System.Reflection.AssemblyInformationalVersionAttribute));
                string v = a != null ? a.InformationalVersion : typeof(AppPaths).Assembly.GetName().Version.ToString(3);
                int i = v.IndexOf('+');
                return i > 0 ? v.Substring(0, i) : v;
            }
            catch { return "0.0.0"; }
        }
        public const string Author = "a cura di Costantino IC8TEM";
        public static string Title { get { return AppName + " " + Version + " - " + Author; } }

        static string dataDir;

        // Cartella del programma: quella dell'eseguibile; su Mac, quella che contiene YA-MS.app
        public static string AppDir
        {
            get
            {
                string b = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                int i = b.IndexOf(".app" + Path.DirectorySeparatorChar + "Contents", StringComparison.OrdinalIgnoreCase);
                if (i > 0)
                {
                    string bundle = b.Substring(0, i + 4);
                    return Path.GetDirectoryName(bundle) ?? b;
                }
                return b;
            }
        }

        public static string DataDir
        {
            get
            {
                if (dataDir != null) return dataDir;
                dataDir = Path.Combine(AppDir, "data");
                Directory.CreateDirectory(dataDir);
                SeedFrom(Path.Combine(AppContext.BaseDirectory, "data"));
                // su Mac i dati di partenza sono dentro il pacchetto (Contents/Resources/data)
                SeedFrom(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "Resources", "data")));
                return dataDir;
            }
        }

        // copia i file di partenza mancanti (non sovrascrive quelli gia' presenti o modificati dall'utente)
        static void SeedFrom(string src)
        {
            try
            {
                if (!Directory.Exists(src)) return;
                if (Path.GetFullPath(src).TrimEnd('/', '\\') == Path.GetFullPath(dataDir).TrimEnd('/', '\\')) return;
                foreach (string f in Directory.GetFiles(src))
                {
                    string d = Path.Combine(dataDir, Path.GetFileName(f));
                    if (!File.Exists(d)) File.Copy(f, d);
                }
                // traduzioni dell'interfaccia (data/lingue): sono file del programma, si aggiornano se quelli nuovi sono piu' recenti
                string ls = Path.Combine(src, "lingue");
                if (Directory.Exists(ls))
                {
                    string ld = Path.Combine(dataDir, "lingue");
                    Directory.CreateDirectory(ld);
                    foreach (string f in Directory.GetFiles(ls))
                    {
                        try
                        {
                            string d = Path.Combine(ld, Path.GetFileName(f));
                            if (!File.Exists(d) || File.GetLastWriteTimeUtc(f) > File.GetLastWriteTimeUtc(d)) File.Copy(f, d, true);
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        public static string DocDir { get { return Path.Combine(AppDir, "doc"); } }
    }
}
