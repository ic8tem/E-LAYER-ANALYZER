// E-LAYER ANALYZER: l'aggiornamento di YA-MS e' disattivato nel pannello (tools/sync_yams.py); aggiorna E-LAYER ANALYZER
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
