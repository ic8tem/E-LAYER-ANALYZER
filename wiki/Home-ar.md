# E-LAYER ANALYZER

*إعداد Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Português (Brasil)](Home-pt-BR) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Polski](Home-pl) · [Čeština](Home-cs) · [Magyar](Home-hu) · [Română](Home-ro) · [Ελληνικά](Home-el) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · **العربية** · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · [বাংলা](Home-bn) · [ไทย](Home-th) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [日本語](Home-ja) · [한국어](Home-ko) · [中文](Home-zh) · [中文 (繁體)](Home-zh-TW)

**E-LAYER ANALYZER** برنامج مجاني لهواة الراديو يحلّل كل أنماط انتشار VHF التي تتدخل فيها **الطبقة E** من الأيونوسفير: **الطبقة E المتفرقة (sporadic-E)** (بما فيها تعدد القفزات بين القارات)، و**F.A.I.** (عدم الانتظام المحاذي لخطوط المجال المغناطيسي)، و**التشتت النيزكي (meteor scatter)** (مع برنامج YA-MS المدمج)، و**الشفق الراديوي (radio aurora)**. يعمل على **Windows (64 و32 بت) وLinux وRaspberry Pi وMac**، بـ**28 لغة**، ولا يكتب شيئاً خارج مجلده الخاص.

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## علامات التبويب

| علامة التبويب | وظيفتها |
|---|---|
| [SPORADIC-E](Sporadic-E-ar) | بلاغات Es على الخريطة، القفزة الأولى لكلتا المحطتين، سحب Es على مربعات Maidenhead، أعلى تردد قابل للاستخدام (MUF) لمحطتك |
| [BENCHMARK Es](Benchmark-Es-ar) | منصة اختبار على سجل ADIF كامل (ولو لمحطة أخرى): اتصالات QSO حسب اليوم والساعة، التخطي (skip) حسب القطاعات، foEs المقدَّر، التحقق من تعدد القفزات، انجراف السحب |
| [BENCHMARK 50 MHz](Benchmark-50-ar) | الطبقة E المتفرقة متعددة القفزات على 50 MHz من سجل ADIF واحد أو اثنين: خريطة مركاتور، اليوم والوقت، foEs اختيارياً، مسار مكبَّر مع مربع WWL، اتصالات QSO ضمن 150 km |
| [F.A.I.](FAI-ar) | Scatter trail لنقاط التشتت لزوج من المحطات، ومنحنيات تساوي الانعكاس للتحقق من نقطة |
| [METEOR SCATTER](Meteor-Scatter-ar) | برنامج YA-MS الكامل (كفاءة الزخات، الرادارات، التنبؤات) |
| [AURORA](Aurora-ar) | الحلقة الشفقية، مؤشر Kp من NOAA والرياح الشمسية، Scatter trail الشفقي وتساوي الانعكاس، اتجاه الحزمة الموصى به |
| [DATA COLLECTION](Data-Collection-ar) | ثلاث جلسات telnet (DX cluster وRBN وDM4X مع PSKReporter)، البلاغات المجمَّعة، قاعدة بيانات CALL3 |
| [LOG ADIF](Log-ADIF-ar) | المربعات التي تم الاتصال بها والمؤكدة نطاقاً بنطاق من 28 MHz فما فوق، من ملف ADIF الخاص بك أو من LoTW |
| [المصادر، ملخص خيارات الحساب، دليل الاستخدام، التقرير التقني](Sources-and-Technical-Document-ar) | المصادر، والصيغ وخيارات الحساب، ودليل استخدام مفصّل، وتقرير تقني: قابلة للتصدير إلى Word |

## البدء

1. [التثبيت](Installation-ar)
2. [البدء](Getting-Started-ar)
3. [الألوان واللغات](Colours-and-Languages-ar)
4. [مجلد البيانات](Data-Folder-ar)
5. [الأسئلة الشائعة](FAQ-ar)

## المصادر الرئيسية

- G. Cerrotta IC8FAX، C. Cerrotta IC8TEM، *IL F.A.I., quello strano fenomeno del dopo E-sporadico*، كابري، 2011.
- G. Cerrotta IC8FAX، [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html).
- F. Egano IK3XTV، جدول مسافات الطبقة E المتفرقة ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/)).
- C. Cerrotta IC8TEM، G. Sartori IV3GTH، *METEOR-SCATTER* (مصدر YA-MS).

القائمة الكاملة موجودة في علامة التبويب «المصادر» في البرنامج.
