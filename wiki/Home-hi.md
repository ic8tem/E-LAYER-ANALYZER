# E-LAYER ANALYZER

*Costantino IC8TEM द्वारा* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Português (Brasil)](Home-pt-br) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Polski](Home-pl) · [Čeština](Home-cs) · [Magyar](Home-hu) · [Română](Home-ro) · [Ελληνικά](Home-el) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · **हिन्दी** · [বাংলা](Home-bn) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [ไทย](Home-th) · [日本語](Home-ja) · [한국어](Home-ko) · [中文](Home-zh) · [中文 (繁體)](Home-zh-tw)

**E-LAYER ANALYZER** रेडियो शौकीनों के लिए एक निःशुल्क प्रोग्राम है जो आयनमंडल की **E परत** से जुड़े हर VHF प्रसार मोड का विश्लेषण करता है: **स्पोरैडिक-E** (अंतरमहाद्वीपीय बहु-हॉप सहित), **F.A.I.** (क्षेत्र-संरेखित अनियमितताएँ), **मीटियोर स्कैटर** (अंतर्निहित YA-MS प्रोग्राम के साथ) और **रेडियो ऑरोरा**। यह **Windows (64 और 32 बिट), Linux, Raspberry Pi और Mac** पर, **36 भाषाओं** में चलता है, और अपने फ़ोल्डर के बाहर कुछ भी नहीं लिखता।

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## टैब

| टैब | क्या करता है |
|---|---|
| [SPORADIC-E](Sporadic-E-hi) | मानचित्र पर Es स्पॉट, दोनों स्टेशनों का पहला हॉप, Maidenhead वर्गों पर Es बादल, आपके स्टेशन की MUF |
| [BENCHMARK Es](Benchmark-Es-hi) | पूरे ADIF लॉग (किसी अन्य स्टेशन का भी) पर परीक्षण: दिन और घंटे के अनुसार QSO, सेक्टर अनुसार स्किप, अनुमानित foEs, बहु-हॉप जाँच, बादलों का खिसकाव |
| [BENCHMARK 50 MHz](Benchmark-50-hi) | एक या दो ADIF लॉग से 50 MHz पर बहु-हॉप स्पोरैडिक-E: मर्केटर मानचित्र, दिन और समय, वैकल्पिक foEs, WWL सहित बड़ा किया गया पथ, 150 km के भीतर के QSO |
| [आयनोसोंड](Ionosondes-hi) | GIRO आयनोसोंड से सीधे foEs, foF2 और MUF(D), 24 घंटे का ग्राफ़, खुलने वाले बैंडों सहित स्पोरैडिक-E की MUF, स्पॉट से तुलना |
| [समय के साथ Es बादल](Es-Clouds-Over-Time-hi) | Chartier 2022 की विधि से चरण दर चरण Es बादल: हर सेल की न्यूनतम foEs और NmE, सत्र के स्पॉट, ADIF लॉग या CSV स्पॉट से |
| [F.A.I.](FAI-hi) | स्टेशनों की एक जोड़ी के स्कैटर पॉइंट्स का Scatter trail, और किसी बिंदु की जाँच के लिए आइसोफ्लेक्शन रेखाएँ |
| [METEOR SCATTER](Meteor-Scatter-hi) | पूरा YA-MS प्रोग्राम (उल्का वर्षा दक्षता, रडार, पूर्वानुमान) |
| [AURORA](Aurora-hi) | ऑरोरल अंडाकार, NOAA Kp और सौर पवन, ऑरोरल Scatter trail और आइसोफ्लेक्शन, अनुशंसित बीम दिशा |
| [DATA COLLECTION](Data-Collection-hi) | तीन टेलनेट सत्र (DX cluster, RBN, PSKReporter सहित DM4X), एकत्रित स्पॉट, CALL3 डेटाबेस |
| [LOG ADIF](Log-ADIF-hi) | 28 MHz से ऊपर बैंड-दर-बैंड संपर्कित और पुष्ट वर्ग, आपकी ADIF या LoTW से |
| [स्रोत, गणना के विकल्पों का सारांश, उपयोगकर्ता पुस्तिका, तकनीकी रिपोर्ट](Sources-and-Technical-Document-hi) | स्रोत, सूत्र और गणना के विकल्प, विस्तृत उपयोगकर्ता पुस्तिका, तकनीकी रिपोर्ट: Word में निर्यात योग्य |

## शुरुआत

1. [इंस्टॉलेशन](Installation-hi)
2. [पहली बार शुरू करना](Getting-Started-hi)
3. [रंग और भाषाएँ](Colours-and-Languages-hi)
4. [डेटा फ़ोल्डर](Data-Folder-hi)
5. [अक्सर पूछे जाने वाले प्रश्न](FAQ-hi)

## मुख्य स्रोत

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, Capri, 2011।
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html)।
- F. Egano IK3XTV, स्पोरैडिक-E दूरी तालिका ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/))।
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER* (YA-MS का स्रोत)।

पूरी सूची प्रोग्राम के स्रोत टैब में है।

## आभार

50 MHz में स्पोरैडिक-E के विकास के लिए: **Johan IW2DNI** और **Fabio IZ7NLJ**।

## लाइसेंस

E-LAYER ANALYZER मुक्त सॉफ़्टवेयर है, **GNU GPL-3.0** लाइसेंस के साथ (COPYING फ़ाइल), वही जो YA-MS का है। यह बिना किसी वारंटी के वितरित किया जाता है। बाहरी स्रोतों का डेटा संबंधित संस्थाओं का ही रहता है, उनकी अपनी शर्तों के साथ (उदाहरण के लिए GIRO का डेटा: CC BY-NC-SA 4.0, केवल गैर-व्यावसायिक उपयोग)।
