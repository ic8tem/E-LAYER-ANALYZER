# E-LAYER ANALYZER

*โดย Costantino IC8TEM* — **English** · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · [বাংলা](Home-bn) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [日本語](Home-ja) · [한국어](Home-ko) · [中文](Home-zh)

**E-LAYER ANALYZER** เป็นโปรแกรมฟรีสำหรับนักวิทยุสมัครเล่นที่วิเคราะห์การแพร่กระจายคลื่นย่าน VHF ทุกรูปแบบที่เกี่ยวข้องกับ **ชั้น E** ของไอโอโนสเฟียร์ ได้แก่ **ชั้น E สปอราดิก (sporadic-E)** (รวมถึงแบบหลายฮอปข้ามทวีป) **F.A.I.** (ความไม่สม่ำเสมอที่เรียงตัวตามสนามแม่เหล็ก) **meteor scatter** (พร้อมโปรแกรม YA-MS ในตัว) และ **ออโรราทางวิทยุ** โปรแกรมทำงานบน **Windows (64 และ 32 บิต), Linux, Raspberry Pi และ Mac** มี **28 ภาษา** และไม่เขียนสิ่งใดออกนอกโฟลเดอร์ของตัวเอง

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## แท็บต่าง ๆ

| แท็บ | ทำอะไร |
|---|---|
| [SPORADIC-E](Sporadic-E-th) | สปอต Es บนแผนที่ ฮอปแรกของทั้งสองสถานี เมฆ Es บนช่องกริด Maidenhead และ MUF สำหรับสถานีของคุณ |
| [BENCHMARK Es](Benchmark-Es-th) | ชุดทดสอบบนล็อก ADIF ทั้งไฟล์ (แม้เป็นของสถานีอื่น): QSO ต่อวันและต่อชั่วโมง ระยะสคิปตามเซกเตอร์ foEs โดยประมาณ การตรวจสอบหลายฮอป การเคลื่อนตัวของเมฆ |
| [BENCHMARK 50 MHz](Benchmark-50-th) | ชั้น E สปอราดิกแบบหลายฮอปบน 50 MHz จากล็อก ADIF หนึ่งหรือสองไฟล์: แผนที่เมอร์เคเตอร์ วันและเวลา foEs (เลือกได้) เส้นทางขยายพร้อม WWL และ QSO ภายในรัศมี 150 km |
| [F.A.I.](FAI-th) | Scatter trail ของจุดกระเจิงสำหรับสถานีคู่หนึ่ง และเส้นไอโซเฟล็กชันสำหรับตรวจสอบจุด |
| [METEOR SCATTER](Meteor-Scatter-th) | โปรแกรม YA-MS ฉบับสมบูรณ์ (ประสิทธิภาพของฝนดาวตก เรดาร์ การพยากรณ์) |
| [AURORA](Aurora-th) | วงรีออโรรา Kp ของ NOAA และลมสุริยะ scatter trail และไอโซเฟล็กชันของออโรรา ทิศทางสายอากาศที่แนะนำ |
| [DATA COLLECTION](Data-Collection-th) | เซสชัน telnet สามช่อง (DX cluster, RBN, DM4X พร้อม PSKReporter) สปอตที่เก็บได้ ฐานข้อมูล CALL3 |
| [LOG ADIF](Log-ADIF-th) | ช่องกริดที่ติดต่อได้และได้รับการยืนยันทีละย่านความถี่ตั้งแต่ 28 MHz ขึ้นไป จาก ADIF ของคุณหรือจาก LoTW |
| [แหล่งอ้างอิง สรุปทางเลือกในการคำนวณ คู่มือผู้ใช้ รายงานทางเทคนิค](Sources-and-Technical-Document-th) | แหล่งอ้างอิง สูตรและทางเลือกในการคำนวณ คู่มือผู้ใช้ฉบับละเอียด รายงานทางเทคนิค: ส่งออกเป็น Word ได้ |

## เริ่มต้นใช้งาน

1. [การติดตั้ง](Installation-th)
2. [การเริ่มใช้งานครั้งแรก](Getting-Started-th)
3. [สีและภาษา](Colours-and-Languages-th)
4. [โฟลเดอร์ข้อมูล](Data-Folder-th)
5. [คำถามที่พบบ่อย](FAQ-th)

## แหล่งอ้างอิงหลัก

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, Capri, 2011.
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html).
- F. Egano IK3XTV, ตารางระยะทางของชั้น E สปอราดิก ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/))
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER* (แหล่งอ้างอิงของ YA-MS)

รายการทั้งหมดอยู่ในแท็บ แหล่งอ้างอิง ของโปรแกรม
