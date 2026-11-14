# התקנה

[← Home](Home-he)

הורידו את הקובץ המתאים מדף ה-**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. התוכנה ותיקיית ה-`data` נשארות תמיד יחד בתיקייה משלכם: **אף קובץ אינו נכתב לעולם ב-C:\Users\...\AppData**.

| מערכת | קובץ | איך מתקינים |
|---|---|---|
| Windows ‏64 ביט | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | לחיצה כפולה. מותקן ב-`C:\E-LAYER-ANALYZER`, ללא צורך בהרשאות מנהל. אין קיצור דרך בתפריט התחל (הוא היה נשמר ב-AppData), רק סמל אופציונלי בשולחן העבודה. |
| Windows ‏32 ביט | `...-win-x86-setup.exe` | כנ״ל, עבור Windows ‏32 ביט. |
| Windows על ARM ‏(מחשבי Snapdragon) | `...-win-arm64-setup.exe` | כנ״ל, עבור Windows על מעבדי ARM. |
| Windows נייד | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | חלצו בכל מקום והפעילו את `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, ואז הפעילו מהתפריט: בהפעלה הראשונה התוכנה מעתיקה את עצמה ל-`~/E-LAYER-ANALYZER`, עם תיקיית ה-`data` לצידה. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (וגם `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (או `zypper install`), ואז כמו ה-‎.deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (או `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, ואז כמו ה-‎.deb. |
| Slackware ‏(x86_64 ו-ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (או `-aarch64-`) | כ-root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, ואז הפעילו את `e-layer-analyzer` מהתפריט או מהמסוף: בהפעלה הראשונה התוכנה מעתיקה את עצמה ל-`~/E-LAYER-ANALYZER`. |
| כל Linux ‏(AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (או `-aarch64.AppImage`) | הפכו אותו לבר-הרצה והפעילו: בהפעלה הראשונה הוא מעתיק את התוכנה ל-`~/E-LAYER-ANALYZER` ומפעיל אותה משם. |
| Linux נייד | `...-linux-x64-portatile.tar.gz` | חלצו והריצו `./E-LAYER-ANALYZER`, או הריצו `./installa.sh`: מותקן ב-`~/E-LAYER-ANALYZER` עם סמל בתפריט. |
| Alpine Linux ‏(musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, ואז כמו הגרסה הניידת ל-Linux. |
| Raspberry Pi ‏(64 ביט) | `e-layer-analyzer_x.y.z_arm64.deb` או `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 או 5 עם **Raspberry Pi OS ‏64 ביט** ושולחן עבודה (מומלץ 4 GB של RAM). ה-‎.deb כמו למעלה; הגרסה הניידת כמו ב-Linux. |
| Raspberry Pi ‏(32 ביט) | `e-layer-analyzer_x.y.z_armhf.deb` או `...-linux-arm-portatile.tar.gz` | Pi 2, 3 או 4 עם **Raspberry Pi OS ‏32 ביט**. ה-‎.deb כמו למעלה; הגרסה הניידת: חלצו והריצו `./E-LAYER-ANALYZER` או `./installa.sh`. |
| Raspberry Pi עם Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` או `...-linux-arm64-portatile.tar.gz` | כמו Slackware למעלה. הגרסה הניידת כמו ב-Linux. |
| Linux armv7l ‏(לוחות ARM אחרים של 32 ביט) | `...-linux-armv7l-portatile.tar.gz` | כמו הגרסה הניידת ל-Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **ניסיוני, לא מאומת.** דורש את ה-‎.NET 8 של הקהילה (`pkg install dotnet`) ואת הספריות המקוריות SkiaSharp ו-HarfBuzzSharp שנבנו עבור FreeBSD, ש-Avalonia אינה מספקת: בלעדיהן התוכנה אינה עולה. ראו `LEGGIMI-FREEBSD.txt` בחבילה. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) או `...-osx-x64.dmg` (Intel) | פתחו את ה-‎.dmg, לחצו לחיצה ימנית על **Installa E-LAYER-ANALYZER.command** > Open. מותקן ב-`~/E-LAYER-ANALYZER`. |
| Mac, אפליקציה בלבד | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | חלצו את **E-LAYER-ANALYZER.app** לתיקייה משלכם (למשל `~/E-LAYER-ANALYZER`, לא Applications): תיקיית ה-`data` נוצרת ליד האפליקציה. בהפעלה הראשונה לחיצה ימנית > Open. |

## עדכון

התקינו את הגרסה החדשה מעל הישנה: ההגדרות, הספוטים שנאספו, הלוגים ו-`call3.txt` נשמרים.

**חבילות מערכת (.deb, .rpm, Arch, Slackware) אינן מתעדכנות אוטומטית.** איתן התוכנה רק מודיעה על הגרסה החדשה: הורידו את החבילה החדשה מדף ה-Releases והתקינו אותה כמו בפעם הראשונה (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). העדכונים האוטומטיים עובדים עם המתקין וקובצי ה-zip של Windows, עם יישום ה-Mac, עם חבילות ה-tar.gz הניידות ועם ה-AppImage.

**עדכון אוטומטי (מגרסה 0.4.5).** בהפעלה ולאחר מכן כל 30 דקות התוכנה בודקת את ה-Releases ב-GitHub. כשיוצאת גרסה חדשה היא פותחת חלון ומציגה **▲ x.y.z** בכתום ליד מחוון ה-NTP בסרגל העליון:

**מגרסה 0.4.12 העדכונים אוטומטיים ושקטים**: כשיש גרסה חדשה ב-GitHub התוכנה מורידה אותה, מתקינה אותה ומפעילה את עצמה מחדש בלי לשאול דבר. גם קבצים חסרים מורדים מחדש בלי לשאול. חלון העדכון מופיע רק בלחיצה על הגרסה למעלה, או אם מכבים את העדכונים האוטומטיים בהגדרות (כללי).

- **עדכן עכשיו** מוריד את החבילה הניידת למחשב שלכם (Windows ‏64/32 ביט, Linux, Raspberry Pi, Mac), סוגר את התוכנה, מחליף את הקבצים ומפעיל אותה מחדש. ההגדרות, הספוטים שנאספו, הלוגים ו-`call3.txt` נשמרים.
- **מאוחר יותר** שואל שוב בבדיקה הבאה; **דלג על גרסה זו** מפסיק להציע את הגרסה הזו.
- לחיצה על הגרסה המוצגת בסרגל העליון (לדוגמה `v0.4.5`) בודקת עדכונים מיד. ניתן לכבות את הבדיקה האוטומטית בתיבת הסימון שבאותו חלון.
- אם לא ניתן לכתוב בתיקיית התוכנה (לדוגמה תיקיית מערכת), השתמשו במקום זאת בחבילת ההתקנה מדף ה-Release.

**קבצים חסרים.** בהפעלה התוכנה בודקת שכל הקבצים שאיתם הופצה קיימים (תרגומים, מסמכים, טבלאות, הרשומים ב-`data/manifest.txt`). אם חסר קובץ כלשהו, חלון מציג אותם ו-**הורד מ-GitHub** מוריד אותם שוב מאותה גרסה ב-GitHub.

## דרישות

- אין צורך להתקין ‎.NET: התוכנה מכילה את כל מה שהיא צריכה.
- חיבור לאינטרנט עבור המפה (Natural Earth, בהפעלה הראשונה), נתוני האורורה של NOAA, המכ״מים של YA-MS וחיבורי ה-telnet.
- ‏Raspberry Pi Zero ו-Pi 1 אינם נתמכים (‏.NET אינו תומך במעבדים שלהם).
- אין גרסאות ל-Linux i686 ‏(מחשבים של 32 ביט) ול-OpenBSD, כי ‎.NET אינו קיים למערכות אלה. ל-FreeBSD יש רק את החבילה הניסיונית.
