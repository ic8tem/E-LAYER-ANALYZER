# E-LAYER ANALYZER

*by Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Português (Brasil)](Home-pt-BR) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Polski](Home-pl) · [Čeština](Home-cs) · [Magyar](Home-hu) · [Română](Home-ro) · **Ελληνικά** · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · [বাংলা](Home-bn) · [ไทย](Home-th) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [日本語](Home-ja) · [한국어](Home-ko) · [中文](Home-zh) · [中文 (繁體)](Home-zh-TW)

Το **E-LAYER ANALYZER** είναι ένα δωρεάν πρόγραμμα για ραδιοερασιτέχνες που αναλύει κάθε τρόπο διάδοσης VHF στον οποίο συμμετέχει το **στρώμα E** της ιονόσφαιρας: το **σποραδικό E** (μαζί με τα διηπειρωτικά πολλαπλά άλματα), το **F.A.I.** (ανομοιογένειες ευθυγραμμισμένες με το πεδίο), το **meteor scatter** (με το ενσωματωμένο πρόγραμμα YA-MS) και το **ραδιο-σέλας**. Τρέχει σε **Windows (64 και 32 bit), Linux, Raspberry Pi και Mac**, σε **28 γλώσσες**, και δεν γράφει τίποτα έξω από τον δικό του φάκελο.

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Οι καρτέλες

| Καρτέλα | Τι κάνει |
|---|---|
| [SPORADIC-E](Sporadic-E-el) | Spot Es στον χάρτη, πρώτο άλμα και των δύο σταθμών, νέφη Es στα τετράγωνα Maidenhead, MUF για τον σταθμό σας |
| [BENCHMARK Es](Benchmark-Es-el) | Πάγκος δοκιμών σε ολόκληρο ημερολόγιο ADIF (ακόμη και άλλου σταθμού): QSO ανά ημέρα και ανά ώρα, skip ανά τομέα, εκτιμώμενη foEs, έλεγχος πολλαπλού άλματος, μετατόπιση των νεφών |
| [BENCHMARK 50 MHz](Benchmark-50-el) | Σποραδικό E πολλαπλού άλματος στα 50 MHz από ένα ή δύο ημερολόγια ADIF: χάρτης Mercator, ημέρα και ώρα, προαιρετική foEs, μεγεθυμένη διαδρομή με το WWL, QSO εντός 150 km |
| [F.A.I.](FAI-el) | Scatter trail των σημείων σκέδασης για ένα ζεύγος σταθμών και καμπύλες ισοανάκλασης για τον έλεγχο ενός σημείου |
| [METEOR SCATTER](Meteor-Scatter-el) | Το πλήρες πρόγραμμα YA-MS (απόδοση βροχών, ραντάρ, προβλέψεις) |
| [AURORA](Aurora-el) | Σελαϊκό οβάλ, Kp και ηλιακός άνεμος της NOAA, scatter trail και ισοανάκλαση σέλαος, προτεινόμενη κατεύθυνση κεραίας |
| [DATA COLLECTION](Data-Collection-el) | Τρεις συνεδρίες telnet (DX cluster, RBN, DM4X με PSKReporter), spot που συλλέχθηκαν, βάση δεδομένων CALL3 |
| [LOG ADIF](Log-ADIF-el) | Τετράγωνα που δουλεύτηκαν και επιβεβαιώθηκαν ανά μπάντα από τα 28 MHz και πάνω, από το ADIF σας ή από το LoTW |
| [ΠΗΓΕΣ, ΣΥΝΟΨΗ ΤΩΝ ΕΠΙΛΟΓΩΝ ΥΠΟΛΟΓΙΣΜΟΥ, ΕΓΧΕΙΡΙΔΙΟ ΧΡΗΣΗΣ, ΤΕΧΝΙΚΗ ΕΚΘΕΣΗ](Sources-and-Technical-Document-el) | Πηγές, τύποι και επιλογές υπολογισμού, αναλυτικό εγχειρίδιο χρήσης, τεχνική έκθεση: εξαγώγιμα σε Word |

## Πρώτα βήματα

1. [Εγκατάσταση](Installation-el)
2. [Πρώτη εκκίνηση](Getting-Started-el)
3. [Χρώματα και γλώσσες](Colours-and-Languages-el)
4. [Φάκελος δεδομένων](Data-Folder-el)
5. [Συχνές ερωτήσεις](FAQ-el)

## Κύριες πηγές

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, Capri, 2011.
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html).
- F. Egano IK3XTV, πίνακας αποστάσεων για το σποραδικό E ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/)).
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER* (πηγή του YA-MS).

Ο πλήρης κατάλογος βρίσκεται στην καρτέλα ΠΗΓΕΣ του προγράμματος.
