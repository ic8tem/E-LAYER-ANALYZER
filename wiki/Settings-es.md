# AJUSTES

[← Home](Home-es)

Desde la versión 0.4.18 la pestaña **AJUSTES** reúne las opciones generales del programa y el reparto de los spots entre las pestañas. Tiene dos subpestañas.

## General

| Sección | Qué se configura |
|---|---|
| Estación | Indicativo, locator y altura, los mismos de la barra superior; **Aplicar** los confirma para todas las pestañas. |
| Aspecto | Idioma, colores (los esquemas de YAEC y YA-MS y, desde la versión 0.5.0, los seis temas de IP-TRX-CTRL: Claro, Noche, Negro, Azul, Violeta, Verde) y **tamaño de la letra de todo el programa** (85-140 %, como en YA-MS). El idioma y los colores reconstruyen la ventana: los ajustes y los spots se conservan. |
| Mapas y datos de previsión | El continente, el mismo menú de la barra de pestañas. |
| Hora | Estado NTP y diferencia entre la hora de los servidores y el reloj del PC. |
| Actualizaciones | Comprobación en GitHub al arrancar y cada 30 minutos, actualización automática y silenciosa, botón **Comprobar ahora**. Las actualizaciones automáticas no valen para los paquetes del sistema (.deb, .rpm, Arch, Slackware). |
| Datos | Botón que abre la carpeta `data`. La clave API del Bureau of Meteorology sigue en DATA COLLECTION › Datos de propagación. |

## Reparto de los spots

A la izquierda una tabla enumera las reglas con las que el programa decide adónde va cada spot, **en el orden en que se aplican**: la primera regla que se cumple decide la pestaña. Los valores mostrados son los configurados en los filtros.

| # | Si el spot... | va a |
|---|---|---|
| 1 | está fuera del filtro local (por debajo de 27990 kHz o por encima de 145 MHz; 430-440 MHz solo aurora) | descartado |
| 2 | contiene AUE, AU-E o AURORAL E | AURORA (aurora E) |
| 3 | contiene una palabra de la aurora (por defecto AU, AUR) | AURORA |
| 4 | contiene una palabra del F.A.I. (por defecto FAI; bandas de 6, 4 y 2 m: el F.A.I. no llega a 432 MHz) | F.A.I. |
| 5 | tiene un modo del meteor scatter o `<MS>` | METEOR SCATTER |
| 6 | cae en una ventana MS del plan de bandas (en 50 MHz no exclusiva; FT8 y FT4 en 50 y 70 MHz excluidos) | METEOR SCATTER |
| 7 | es FT8 o FT4 en 50 MHz sin otro spot en 4 min con el punto medio a menos de 150 km | meteor scatter (no esporádica E) |
| 8 | es Q65 o JT65 en 144 MHz (salvo `<ES>`) | EME: descartado |
| 9 | contiene una etiqueta de otra propagación | no esporádica E |
| 10 | es transecuatorial | no esporádica E (TEP) |
| 11-15 | está en 28, 40, 50, 70 o 144 MHz dentro de las reglas de distancia | SPORADIC-E |
| 16 | no entra en ninguna regla (tropo, 432 MHz, HF...) | ninguna pestaña (se queda en Spots recogidos) |

A la derecha están el **filtro local** y los filtros de cada modo, en cinco subpestañas: Meteor scatter (el filtro de YA-MS se configura en la pestaña METEOR SCATTER), Esporádica E, F.A.I., Aurora y **Prueba de los filtros**, donde se pega una línea del cluster para ver cómo se clasifica. El botón **Restablecer las reglas del autor** devuelve los valores por defecto.
