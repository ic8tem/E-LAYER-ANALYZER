# E-LAYER ANALYZER

*โดย Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · [বাংলা](Home-bn) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [日本語](Home-ja) · [한국어](Home-ko) · [中文](Home-zh) · **ไทย**

**E-LAYER ANALYZER** เป็นโปรแกรมฟรีสำหรับนักวิทยุสมัครเล่น ที่วิเคราะห์โหมดการแพร่กระจาย VHF ทุกแบบที่เกี่ยวข้องกับ**ชั้น E** ของไอโอโนสเฟียร์: **sporadic-E** (รวมถึงหลายฮอปข้ามทวีป), **F.A.I.** (field-aligned irregularities ความไม่สม่ำเสมอที่เรียงตัวตามสนามแม่เหล็ก), **meteor scatter** (ด้วยโปรแกรม YA-MS ที่รวมไว้) และ**ออโรราทางวิทยุ** ทำงานบน **Windows (64 และ 32 บิต), Linux, Raspberry Pi และ Mac** ใน **36 ภาษา** และไม่เขียนสิ่งใดนอกโฟลเดอร์ของตนเอง

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## แท็บต่าง ๆ

| แท็บ | หน้าที่ |
|---|---|
| [SPORADIC-E](Sporadic-E-th) | สปอต Es บนแผนที่, ฮอปแรกของทั้งสองสถานี, เมฆ Es บนช่องกริด Maidenhead, MUF สำหรับสถานีของคุณ |
| [BENCHMARK Es](Benchmark-Es-th) | แท่นทดสอบบนล็อก ADIF ทั้งไฟล์ (แม้เป็นของสถานีอื่น): QSO ต่อวันและต่อชั่วโมง, เขตข้ามตามเซกเตอร์, foEs โดยประมาณ, การตรวจสอบหลายฮอป, การเคลื่อนตัวของเมฆ |
| [BENCHMARK 50 MHz](Benchmark-50-th) | sporadic-E หลายฮอปบน 50 MHz จากล็อก ADIF หนึ่งหรือสองชุด: แผนที่ Mercator, วันและเวลา, foEs (ไม่บังคับ), เส้นทางขยายพร้อม WWL, QSO ภายใน 150 km |
| [F.A.I.](FAI-th) | Scatter trail ของจุดกระเจิงสำหรับคู่สถานี และโลคัสไอโซเฟล็กชันเพื่อตรวจสอบจุด |
| [METEOR SCATTER](Meteor-Scatter-th) | โปรแกรม YA-MS ฉบับสมบูรณ์ (ประสิทธิภาพของฝนดาวตก, เรดาร์, การพยากรณ์) |
| [AURORA](Aurora-th) | วงรีออโรรา, Kp และลมสุริยะจาก NOAA, Scatter trail และไอโซเฟล็กชันของออโรรา, ทิศการหันสายอากาศที่แนะนำ |
| [DATA COLLECTION](Data-Collection-th) | เซสชัน telnet สามเซสชัน (DX cluster, RBN, DM4X พร้อม PSKReporter), สปอตที่เก็บรวบรวม, ฐานข้อมูล CALL3 |
| [LOG ADIF](Log-ADIF-th) | ช่องกริดที่ติดต่อได้และยืนยันแล้วทีละย่าน ตั้งแต่ 28 MHz ขึ้นไป จาก ADIF ของคุณหรือจาก LoTW |
| [แหล่งอ้างอิง, สรุปทางเลือกในการคำนวณ, คู่มือผู้ใช้, รายงานทางเทคนิค](Sources-and-Technical-Document-th) | แหล่งอ้างอิง สูตรและทางเลือกในการคำนวณ คู่มือผู้ใช้ฉบับละเอียด รายงานทางเทคนิค: ส่งออกเป็น Word ได้ |

## เริ่มต้นใช้งาน

1. [การติดตั้ง](Installation-th)
2. [เริ่มต้นใช้งาน](Getting-Started-th)
3. [สีและภาษา](Colours-and-Languages-th)
4. [โฟลเดอร์ข้อมูล](Data-Folder-th)
5. [คำถามที่พบบ่อย](FAQ-th)

## แหล่งอ้างอิงหลัก

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, คาปรี, 2011
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html)
- F. Egano IK3XTV, ตารางระยะทางของ sporadic-E ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/))
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER* (แหล่งอ้างอิงของ YA-MS)

รายการทั้งหมดอยู่ในแท็บ แหล่งอ้างอิง ของโปรแกรม
