# התקנה

[← Home](Home-he)

הורידו את הקובץ המתאים מדף ה-**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. התוכנה ותיקיית ה-`data` נשארות תמיד יחד בתיקייה משלכם: **אף קובץ אינו נכתב לעולם ב-C:\Users\...\AppData**.

| מערכת | קובץ | איך מתקינים |
|---|---|---|
| Windows ‏64 ביט | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | לחיצה כפולה. מותקן ב-`C:\E-LAYER-ANALYZER`, ללא צורך בהרשאות מנהל. אין קיצור דרך בתפריט התחל (הוא היה נשמר ב-AppData), רק סמל אופציונלי בשולחן העבודה. |
| Windows ‏32 ביט | `...-win-x86-setup.exe` | כנ״ל, עבור Windows ‏32 ביט. |
| Windows נייד | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | חלצו בכל מקום והפעילו את `E-LAYER-ANALYZER.exe`. |
| Linux ‏64 ביט (מחשב אישי) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, ואז הפעילו מהתפריט: בהפעלה הראשונה התוכנה מעתיקה את עצמה ל-`~/E-LAYER-ANALYZER`, עם תיקיית ה-`data` לצידה. |
| Linux נייד | `...-linux-x64-portatile.tar.gz` | חלצו והריצו `./E-LAYER-ANALYZER`, או הריצו `./installa.sh`: מותקן ב-`~/E-LAYER-ANALYZER` עם סמל בתפריט. |
| Raspberry Pi ‏(64 ביט) | `e-layer-analyzer_x.y.z_arm64.deb` או `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 או 5 עם **Raspberry Pi OS ‏64 ביט** ושולחן עבודה (מומלץ 4 GB של RAM). ה-‎.deb כמו למעלה; הגרסה הניידת כמו ב-Linux. |
| Raspberry Pi ‏(32 ביט) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 או 4 עם **Raspberry Pi OS ‏32 ביט**. חלצו והריצו `./E-LAYER-ANALYZER` או `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) או `...-osx-x64.dmg` (Intel) | פתחו את ה-‎.dmg, לחצו לחיצה ימנית על **Installa E-LAYER-ANALYZER.command** > Open. מותקן ב-`~/E-LAYER-ANALYZER`. |
| Mac, אפליקציה בלבד | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | חלצו את **E-LAYER-ANALYZER.app** לתיקייה משלכם (למשל `~/E-LAYER-ANALYZER`, לא Applications): תיקיית ה-`data` נוצרת ליד האפליקציה. בהפעלה הראשונה לחיצה ימנית > Open. |

## עדכון

התקינו את הגרסה החדשה מעל הישנה: ההגדרות, הספוטים שנאספו, הלוגים ו-`call3.txt` נשמרים.

**עדכון אוטומטי (מגרסה 0.4.5).** בהפעלה ולאחר מכן כל 6 שעות התוכנה בודקת את ה-Releases ב-GitHub. כשיוצאת גרסה חדשה היא פותחת חלון ומציגה **▲ x.y.z** בכתום ליד מחוון ה-NTP בסרגל העליון:

- **עדכן עכשיו** מוריד את החבילה הניידת למחשב שלכם (Windows ‏64/32 ביט, Linux, Raspberry Pi, Mac), סוגר את התוכנה, מחליף את הקבצים ומפעיל אותה מחדש. ההגדרות, הספוטים שנאספו, הלוגים ו-`call3.txt` נשמרים.
- **מאוחר יותר** שואל שוב בבדיקה הבאה; **דלג על גרסה זו** מפסיק להציע את הגרסה הזו.
- לחיצה על הגרסה המוצגת בסרגל העליון (לדוגמה `v0.4.5`) בודקת עדכונים מיד. ניתן לכבות את הבדיקה האוטומטית בתיבת הסימון שבאותו חלון.
- אם לא ניתן לכתוב בתיקיית התוכנה (לדוגמה תיקיית מערכת), השתמשו במקום זאת בחבילת ההתקנה מדף ה-Release.

**קבצים חסרים.** בהפעלה התוכנה בודקת שכל הקבצים שאיתם הופצה קיימים (תרגומים, מסמכים, טבלאות, הרשומים ב-`data/manifest.txt`). אם חסר קובץ כלשהו, חלון מציג אותם ו-**הורד מ-GitHub** מוריד אותם שוב מאותה גרסה ב-GitHub.

## דרישות

- אין צורך להתקין ‎.NET: התוכנה מכילה את כל מה שהיא צריכה.
- חיבור לאינטרנט עבור המפה (Natural Earth, בהפעלה הראשונה), נתוני האורורה של NOAA, המכ״מים של YA-MS וחיבורי ה-telnet.
- ‏Raspberry Pi Zero ו-Pi 1 אינם נתמכים (‏.NET אינו תומך במעבדים שלהם).
