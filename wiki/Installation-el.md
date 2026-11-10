# Εγκατάσταση

[← Αρχική](Home-el)

Κατεβάστε το κατάλληλο αρχείο από τη σελίδα **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Το πρόγραμμα και ο φάκελος `data` μένουν πάντα μαζί σε έναν δικό σας φάκελο: **κανένα αρχείο δεν γράφεται ποτέ στο C:\Users\...\AppData**.

| Σύστημα | Αρχείο | Πώς εγκαθίσταται |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Διπλό κλικ. Εγκαθίσταται στο `C:\E-LAYER-ANALYZER`, χωρίς να χρειάζονται δικαιώματα διαχειριστή. Χωρίς συντόμευση στο μενού Έναρξη (θα βρισκόταν στο AppData), μόνο το προαιρετικό εικονίδιο στην επιφάνεια εργασίας. |
| Windows 32 bit | `...-win-x86-setup.exe` | Όπως παραπάνω, για Windows 32 bit. |
| Windows φορητό | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Αποσυμπιέστε οπουδήποτε και εκτελέστε το `E-LAYER-ANALYZER.exe`. |
| Linux 64 bit (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, μετά ξεκινήστε το από το μενού: στην πρώτη εκκίνηση το πρόγραμμα αντιγράφεται στο `~/E-LAYER-ANALYZER`, με τον φάκελο `data` δίπλα του. |
| Linux φορητό | `...-linux-x64-portatile.tar.gz` | Αποσυμπιέστε και εκτελέστε `./E-LAYER-ANALYZER`, ή εκτελέστε `./installa.sh`: εγκαθίσταται στο `~/E-LAYER-ANALYZER` με εικονίδιο στο μενού. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` ή `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 ή 5 με **Raspberry Pi OS 64 bit** και επιφάνεια εργασίας (συνιστώνται 4 GB RAM). Το .deb όπως παραπάνω· το φορητό όπως για το Linux. |
| Raspberry Pi (32 bit) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 ή 4 με **Raspberry Pi OS 32 bit**. Αποσυμπιέστε και εκτελέστε `./E-LAYER-ANALYZER` ή `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) ή `...-osx-x64.dmg` (Intel) | Ανοίξτε το .dmg, δεξί κλικ στο **Installa E-LAYER-ANALYZER.command** > Άνοιγμα. Εγκαθίσταται στο `~/E-LAYER-ANALYZER`. |
| Mac, μόνο η εφαρμογή | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Αποσυμπιέστε το **E-LAYER-ANALYZER.app** σε έναν δικό σας φάκελο (για παράδειγμα `~/E-LAYER-ANALYZER`, όχι στις Εφαρμογές): ο φάκελος `data` δημιουργείται δίπλα στην εφαρμογή. Στην πρώτη εκκίνηση δεξί κλικ > Άνοιγμα. |

## Ενημέρωση

Εγκαταστήστε τη νέα έκδοση πάνω από την παλιά: οι ρυθμίσεις, τα συλλεγμένα spot, τα log και το `call3.txt` διατηρούνται.

## Απαιτήσεις

- Δεν χρειάζεται εγκατάσταση .NET: το πρόγραμμα περιέχει ό,τι χρειάζεται.
- Σύνδεση στο διαδίκτυο για τον χάρτη (Natural Earth, στην πρώτη εκκίνηση), τα δεδομένα σέλαος της NOAA, τα ραντάρ του YA-MS και τις συνεδρίες telnet.
- Τα Raspberry Pi Zero και Pi 1 δεν υποστηρίζονται (οι επεξεργαστές τους δεν υποστηρίζονται από το .NET).
