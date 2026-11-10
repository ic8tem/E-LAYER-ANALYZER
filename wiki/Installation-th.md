# การติดตั้ง

[← Home](Home-th)

ดาวน์โหลดไฟล์ที่เหมาะสมจากหน้า **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** โปรแกรมและโฟลเดอร์ `data` จะอยู่ด้วยกันเสมอในโฟลเดอร์ของคุณเอง: **ไม่มีไฟล์ใดถูกเขียนลงใน C:\Users\...\AppData เลย**

| ระบบ | ไฟล์ | วิธีติดตั้ง |
|---|---|---|
| Windows 64 บิต | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | ดับเบิลคลิก ติดตั้งลงใน `C:\E-LAYER-ANALYZER` โดยไม่ต้องใช้สิทธิ์ผู้ดูแลระบบ ไม่มีทางลัดในเมนู Start (เพราะจะอยู่ใน AppData) มีเพียงไอคอนบนเดสก์ท็อปที่เลือกได้ |
| Windows 32 บิต | `...-win-x86-setup.exe` | เหมือนข้างบน สำหรับ Windows 32 บิต |
| Windows แบบพกพา | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | แตกไฟล์ไว้ที่ใดก็ได้แล้วรัน `E-LAYER-ANALYZER.exe` |
| Linux 64 บิต (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb` จากนั้นเปิดจากเมนู: เมื่อเริ่มครั้งแรกโปรแกรมจะคัดลอกตัวเองไปที่ `~/E-LAYER-ANALYZER` พร้อมโฟลเดอร์ `data` อยู่ข้าง ๆ |
| Linux แบบพกพา | `...-linux-x64-portatile.tar.gz` | แตกไฟล์แล้วรัน `./E-LAYER-ANALYZER` หรือรัน `./installa.sh`: ติดตั้งลงใน `~/E-LAYER-ANALYZER` พร้อมไอคอนในเมนู |
| Raspberry Pi (64 บิต) | `e-layer-analyzer_x.y.z_arm64.deb` หรือ `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 หรือ 5 ที่ใช้ **Raspberry Pi OS 64 บิต** พร้อมเดสก์ท็อป (แนะนำ RAM 4 GB) ไฟล์ .deb ติดตั้งเหมือนข้างบน ส่วนแบบพกพาเหมือน Linux |
| Raspberry Pi (32 บิต) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 หรือ 4 ที่ใช้ **Raspberry Pi OS 32 บิต** แตกไฟล์แล้วรัน `./E-LAYER-ANALYZER` หรือ `./installa.sh` |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) หรือ `...-osx-x64.dmg` (Intel) | เปิด .dmg คลิกขวาที่ **Installa E-LAYER-ANALYZER.command** > เปิด ติดตั้งลงใน `~/E-LAYER-ANALYZER` |
| Mac เฉพาะแอปพลิเคชัน | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | แตก **E-LAYER-ANALYZER.app** ลงในโฟลเดอร์ของคุณเอง (เช่น `~/E-LAYER-ANALYZER` ไม่ใช่ Applications): โฟลเดอร์ `data` จะถูกสร้างข้างแอป เมื่อเริ่มครั้งแรกให้คลิกขวา > เปิด |

## การอัปเดต

ติดตั้งเวอร์ชันใหม่ทับเวอร์ชันเก่า: การตั้งค่า สปอตที่เก็บได้ ล็อก และ `call3.txt` จะถูกเก็บไว้

## ความต้องการของระบบ

- ไม่ต้องติดตั้ง .NET: โปรแกรมมีทุกอย่างที่ต้องใช้อยู่แล้ว
- ต้องเชื่อมต่ออินเทอร์เน็ตสำหรับแผนที่ (Natural Earth เมื่อเริ่มครั้งแรก) ข้อมูลออโรราของ NOAA เรดาร์ของ YA-MS และเซสชัน telnet
- ไม่รองรับ Raspberry Pi Zero และ Pi 1 (.NET ไม่รองรับโปรเซสเซอร์ของรุ่นเหล่านี้)
