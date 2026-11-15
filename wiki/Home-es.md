# E-LAYER ANALYZER

*a cargo de Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · **Español** · [Português](Home-pt) · [Português (Brasil)](Home-pt-br) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Polski](Home-pl) · [Čeština](Home-cs) · [Magyar](Home-hu) · [Română](Home-ro) · [Ελληνικά](Home-el) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · [বাংলা](Home-bn) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [ไทย](Home-th) · [日本語](Home-ja) · [한국어](Home-ko) · [中文](Home-zh) · [中文 (繁體)](Home-zh-tw)

**E-LAYER ANALYZER** es un programa gratuito para radioaficionados que analiza todos los modos de propagación VHF en los que interviene la **capa E** de la ionosfera: **esporádica E** (incluido el multisalto intercontinental), **F.A.I.** (irregularidades alineadas con el campo), **meteor scatter** (con el programa YA-MS integrado) y **aurora radio**. Funciona en **Windows (64 y 32 bits), Linux, Raspberry Pi y Mac**, en **36 idiomas**, y no escribe nada fuera de su propia carpeta.

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Las pestañas

| Pestaña | Qué hace |
|---|---|
| [SPORADIC-E](Sporadic-E-es) | Spots Es en el mapa, primer salto de ambas estaciones, nubes Es sobre las cuadrículas Maidenhead, MUF para tu estación |
| [BENCHMARK Es](Benchmark-Es-es) | Banco de pruebas sobre un log ADIF completo (incluso de otra estación): QSO por día y por hora, skip por sectores, foEs estimada, verificación del multisalto, desplazamiento de las nubes |
| [BENCHMARK 50 MHz](Benchmark-50-es) | Esporádica E multisalto en 50 MHz a partir de uno o dos logs ADIF: mapa Mercator, día y horario, foEs opcional, trayecto ampliado con el WWL, QSO dentro de 150 km |
| [IONOSONDAS](Ionosondes-es) | foEs, foF2 y MUF(D) en directo de las ionosondas GIRO, gráfico de 24 horas, MUF de la esporádica E con las bandas que se abren, comparación con los spots |
| [NUBES Es EN EL TIEMPO](Es-Clouds-Over-Time-es) | Nubes Es paso a paso con el método de Chartier 2022: foEs mínima y NmE por celda, a partir de spots de la sesión, log ADIF o spots CSV |
| [F.A.I.](FAI-es) | Scatter trail de los scatter points para una pareja de estaciones, y lugares de isorreflexión para verificar un punto |
| [METEOR SCATTER](Meteor-Scatter-es) | El programa YA-MS completo (eficiencia de las lluvias, radares, previsiones) |
| [AURORA](Aurora-es) | Óvalo auroral, Kp de NOAA y viento solar, scatter trail auroral e isorreflexión, apuntamiento recomendado |
| [DATA COLLECTION](Data-Collection-es) | Tres sesiones telnet (DX cluster, RBN, DM4X con PSKReporter), spots recogidos, base de datos CALL3 |
| [LOG ADIF](Log-ADIF-es) | Cuadrículas trabajadas y confirmadas banda por banda de 28 MHz en adelante, desde tu ADIF o desde LoTW |
| [FUENTES, SÍNTESIS DE LAS OPCIONES DE CÁLCULO, MANUAL DE USO, INFORME TÉCNICO](Sources-and-Technical-Document-es) | Fuentes, fórmulas y opciones de cálculo, manual de uso detallado, informe técnico: exportables a Word |

## Primeros pasos

1. [Instalación](Installation-es)
2. [Primeros pasos](Getting-Started-es)
3. [Colores e idiomas](Colours-and-Languages-es)
4. [Carpeta de datos](Data-Folder-es)
5. [Preguntas frecuentes](FAQ-es)

## Fuentes principales

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, Capri, 2011.
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html).
- F. Egano IK3XTV, tabla de distancias de la esporádica E ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/)).
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER* (fuente de YA-MS).

La lista completa está en la pestaña FUENTES del programa.
