# התקנה

[← Home](Home-he)

הורידו את הקובץ המתאים מדף ה-**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. התוכנה ותיקיית ה-`data` נשארות תמיד יחד בתיקייה משלכם: **אף קובץ אינו נכתב לעולם ב-C:\Users\...\AppData**.

| מערכת | קובץ | איך מתקינים |
|---|---|---|
| Windows ‏64 ביט | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | לחיצה כפולה. מותקן ב-`C:\E-LAYER-ANALYZER`, ללא צורך בהרשאות מנהל. אין קיצור דרך בתפריט התחל (הוא היה נשמר ב-AppData), רק סמל אופציונלי בשולחן העבודה. |
| Windows ‏32 ביט | `...-windows-x86-setup.exe` | כנ״ל, עבור Windows ‏32 ביט. |
| Windows נייד | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | חלצו בכל מקום והפעילו את `E-LAYER-ANALYZER.exe`. |
| Linux (מחשב אישי) | `...-linux-x64.tar.gz` | חלצו והריצו `./installa.sh`: מותקן ב-`~/E-LAYER-ANALYZER` עם סמל בתפריט. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 או 5 עם **Raspberry Pi OS ‏64 ביט** ושולחן עבודה (מומלץ 4 GB של RAM). חלצו והריצו `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) או `...-mac-intel.dmg` | פתחו את ה-‎.dmg, לחצו לחיצה ימנית על **Installa E-LAYER-ANALYZER.command** > Open. מותקן ב-`~/E-LAYER-ANALYZER`. |

## עדכון

התקינו את הגרסה החדשה מעל הישנה: ההגדרות, הספוטים שנאספו, הלוגים ו-`call3.txt` נשמרים.

## דרישות

- אין צורך להתקין ‎.NET: התוכנה מכילה את כל מה שהיא צריכה.
- חיבור לאינטרנט עבור המפה (Natural Earth, בהפעלה הראשונה), נתוני האורורה של NOAA, המכ״מים של YA-MS וחיבורי ה-telnet.
- ‏Raspberry Pi Zero ו-Pi 1 אינם נתמכים (‏.NET אינו תומך במעבדים שלהם).
