# การติดตั้ง

[← หน้าหลัก](Home-th)

ดาวน์โหลดไฟล์ที่เหมาะสมจากหน้า **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** โปรแกรมและโฟลเดอร์ `data` จะอยู่ด้วยกันเสมอในโฟลเดอร์ของคุณเอง: **ไม่มีไฟล์ใดถูกเขียนลงใน C:\Users\...\AppData เลย**

| ระบบ | ไฟล์ | วิธีติดตั้ง |
|---|---|---|
| Windows 64 บิต | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | ดับเบิลคลิก ติดตั้งไปที่ `C:\E-LAYER-ANALYZER` ไม่ต้องใช้สิทธิ์ผู้ดูแลระบบ ไม่มีทางลัดในเมนู Start (เพราะจะอยู่ใน AppData) มีเพียงไอคอนบนเดสก์ท็อปซึ่งเลือกได้ |
| Windows 32 บิต | `...-win-x86-setup.exe` | เหมือนข้างต้น สำหรับ Windows 32 บิต |
| Windows บน ARM (PC Snapdragon) | `...-win-arm64-setup.exe` | เหมือนข้างต้น สำหรับ Windows บนโปรเซสเซอร์ ARM |
| Windows แบบพกพา | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | แตกไฟล์ไว้ที่ใดก็ได้ แล้วเรียกใช้ `E-LAYER-ANALYZER.exe` |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb` แล้วเปิดจากเมนู: เมื่อเริ่มครั้งแรก โปรแกรมจะคัดลอกตัวเองไปที่ `~/E-LAYER-ANALYZER` โดยมีโฟลเดอร์ `data` อยู่ข้างกัน |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (มี `.aarch64.rpm`, `.armv7hl.rpm` ด้วย) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (หรือ `zypper install`) จากนั้นเหมือน .deb |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (หรือ `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` จากนั้นเหมือน .deb |
| Slackware (x86_64 และ ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (หรือ `-aarch64-`) | ในฐานะ root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` แล้วเปิด `e-layer-analyzer` จากเมนูหรือเทอร์มินัล: เมื่อเริ่มครั้งแรก โปรแกรมจะคัดลอกตัวเองไปที่ `~/E-LAYER-ANALYZER` |
| Linux ใดก็ได้ (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (หรือ `-aarch64.AppImage`) | ทำให้เรียกใช้ได้แล้วเปิด: เมื่อเริ่มครั้งแรกจะคัดลอกโปรแกรมไปที่ `~/E-LAYER-ANALYZER` และเรียกใช้จากที่นั่น |
| Linux แบบพกพา | `...-linux-x64-portatile.tar.gz` | แตกไฟล์แล้วเรียกใช้ `./E-LAYER-ANALYZER` หรือเรียกใช้ `./installa.sh`: ติดตั้งไปที่ `~/E-LAYER-ANALYZER` พร้อมไอคอนในเมนู |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig` จากนั้นเหมือน Linux แบบพกพา |
| Raspberry Pi (64 บิต) | `e-layer-analyzer_x.y.z_arm64.deb` หรือ `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 หรือ 5 ที่ใช้ **Raspberry Pi OS 64 บิต** พร้อมเดสก์ท็อป (แนะนำ RAM 4 GB) ไฟล์ .deb ติดตั้งเหมือนข้างต้น; แบบพกพาเหมือนของ Linux |
| Raspberry Pi (32 บิต) | `e-layer-analyzer_x.y.z_armhf.deb` หรือ `...-linux-arm-portatile.tar.gz` | Pi 2, 3 หรือ 4 ที่ใช้ **Raspberry Pi OS 32 บิต** ไฟล์ .deb ติดตั้งเหมือนข้างต้น; แบบพกพา: แตกไฟล์แล้วเรียกใช้ `./E-LAYER-ANALYZER` หรือ `./installa.sh` |
| Raspberry Pi ที่ใช้ Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` หรือ `...-linux-arm64-portatile.tar.gz` | เหมือน Slackware ข้างต้น แบบพกพาเหมือนของ Linux |
| Linux armv7l (บอร์ด ARM 32 บิตอื่น ๆ) | `...-linux-armv7l-portatile.tar.gz` | เหมือน Linux แบบพกพา |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **ทดลอง ยังไม่ได้ตรวจสอบ** ต้องใช้ .NET 8 ของชุมชน (`pkg install dotnet`) และไลบรารีเนทีฟ SkiaSharp และ HarfBuzzSharp ที่คอมไพล์สำหรับ FreeBSD ซึ่ง Avalonia ไม่ได้ให้มา: หากไม่มีไลบรารีเหล่านี้โปรแกรมจะไม่เริ่มทำงาน ดู `LEGGIMI-FREEBSD.txt` ในแพ็กเกจ |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) หรือ `...-osx-x64.dmg` (Intel) | เปิดไฟล์ .dmg คลิกขวาที่ **Installa E-LAYER-ANALYZER.command** > Open ติดตั้งไปที่ `~/E-LAYER-ANALYZER` |
| Mac เฉพาะแอปพลิเคชัน | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | แตก **E-LAYER-ANALYZER.app** ไว้ในโฟลเดอร์ของคุณเอง (เช่น `~/E-LAYER-ANALYZER` ไม่ใช่ Applications): โฟลเดอร์ `data` จะถูกสร้างข้างแอป เมื่อเริ่มครั้งแรกให้คลิกขวา > Open |

## การอัปเดต

ติดตั้งเวอร์ชันใหม่ทับเวอร์ชันเก่า: การตั้งค่า สปอตที่เก็บรวบรวม ล็อก และ `call3.txt` จะยังคงอยู่

**แพ็กเกจของระบบ (.deb, .rpm, Arch, Slackware) ไม่อัปเดตอัตโนมัติ** กับแพ็กเกจเหล่านี้โปรแกรมจะแจ้งเวอร์ชันใหม่เท่านั้น: ดาวน์โหลดแพ็กเกจใหม่จากหน้า Releases แล้วติดตั้งเหมือนครั้งแรก (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`) การอัปเดตอัตโนมัติใช้ได้กับตัวติดตั้งและ zip ของ Windows แอปของ Mac แพ็กเกจ tar.gz แบบพกพา และ AppImage

**การอัปเดตอัตโนมัติ (ตั้งแต่เวอร์ชัน 0.4.5)** เมื่อเริ่มทำงานและทุก 6 ชั่วโมงหลังจากนั้น โปรแกรมจะตรวจ Releases บน GitHub เมื่อมีเวอร์ชันใหม่ จะเปิดหน้าต่างและแสดง **▲ x.y.z** สีส้มข้างตัวบอก NTP ในแถบด้านบน:

**ตั้งแต่เวอร์ชัน 0.4.12 การอัปเดตเป็นแบบอัตโนมัติและเงียบ**: เมื่อมีเวอร์ชันใหม่บน GitHub โปรแกรมจะดาวน์โหลด ติดตั้ง และเริ่มใหม่เองโดยไม่ถามอะไร ไฟล์ที่ขาดหายก็ดาวน์โหลดใหม่โดยไม่ถามเช่นกัน หน้าต่างอัปเดตจะปรากฏเฉพาะเมื่อคลิกที่เวอร์ชันด้านบน หรือเมื่อปิดการอัปเดตอัตโนมัติใน การตั้งค่า (ทั่วไป)

- **อัปเดตเลย** ดาวน์โหลดแพ็กเกจแบบพกพาสำหรับคอมพิวเตอร์ของคุณ (Windows 64/32 บิต, Linux, Raspberry Pi, Mac) ปิดโปรแกรม แทนที่ไฟล์ แล้วเปิดโปรแกรมใหม่ การตั้งค่า สปอตที่เก็บรวบรวม ล็อก และ `call3.txt` จะยังคงอยู่
- **ภายหลัง** จะถามอีกครั้งในการตรวจครั้งถัดไป; **ข้ามเวอร์ชันนี้** จะไม่เสนอเวอร์ชันนั้นอีก
- การคลิกเวอร์ชันที่แสดงในแถบด้านบน (เช่น `v0.4.5`) จะตรวจหาอัปเดตทันที ปิดการตรวจอัตโนมัติได้ด้วยช่องทำเครื่องหมายในหน้าต่างเดียวกัน
- หากเขียนลงโฟลเดอร์ของโปรแกรมไม่ได้ (เช่น โฟลเดอร์ระบบ) ให้ใช้แพ็กเกจติดตั้งจากหน้า Release แทน

**ไฟล์ที่ขาดหาย** เมื่อเริ่มทำงาน โปรแกรมจะตรวจว่ามีไฟล์ครบทุกไฟล์ที่แจกจ่ายมาด้วย (คำแปล เอกสาร ตาราง ตามรายการใน `data/manifest.txt`) ถ้าขาดไฟล์ใด หน้าต่างจะแสดงรายการไฟล์เหล่านั้น และ **ดาวน์โหลดจาก GitHub** จะดึงไฟล์กลับมาจากเวอร์ชันเดียวกันบน GitHub

## ความต้องการของระบบ

- ไม่ต้องติดตั้ง .NET: โปรแกรมมีทุกอย่างที่ต้องใช้อยู่ในตัว
- การเชื่อมต่ออินเทอร์เน็ตสำหรับแผนที่ (Natural Earth เมื่อเริ่มครั้งแรก) ข้อมูลออโรราของ NOAA เรดาร์ของ YA-MS และเซสชัน telnet
- ไม่รองรับ Raspberry Pi Zero และ Pi 1 (.NET ไม่รองรับโปรเซสเซอร์ของรุ่นเหล่านี้)
- ไม่มีเวอร์ชันสำหรับ Linux i686 (PC 32 บิต) และ OpenBSD เพราะไม่มี .NET สำหรับระบบเหล่านั้น FreeBSD มีเพียงแพ็กเกจทดลอง
