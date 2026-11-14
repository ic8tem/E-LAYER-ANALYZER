# DATA COLLECTION

[← Home](Home-es)

## Sesiones telnet

| Sesión | Por defecto | Notas |
|---|---|---|
| 1. DX cluster a elección | on0dxk.dyndns.org 8000 | En ON0DXK los spots VHF están filtrados: `set/filter dxbm/pass 6` (luego 4, 2). Otro ejemplo: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW y RTTY, 7001 = FT8. |
| 3. Cluster con PSKReporter | **dm4x.ddns.net 8500** | Cluster de DM4X; como alternativa HamAlert (hamalert.org 7300, requiere cuenta). |

- Login automático con el indicativo (arriba o en el campo Login); **comandos iniciales** separados por `;`.
- **Reconectar** tras 30 s si se cae la línea; **Conectar al inicio**.
- Por defecto solo se guardan los spots de **28 MHz en adelante**.

## Spots recogidos

Los últimos 2000 spots, con filtros por banda y por fuente. Archivo histórico: un archivo CSV por día en `data/spot`. Cada vez que el programa se cierra, la base de datos de spots se vacía (memoria y archivos de `data/spot`): cada sesión empieza desde cero.

**Filtro local** (casilla opcional, en AJUSTES): las líneas "DX de" por debajo de 27990,0 kHz o por encima de 145 MHz se descartan, no se muestran en la consola y no se guardan; entre 430 y 440 MHz pasan solo los spots de aurora. Los **locators imprecisos** (menos de 6 caracteres) se toman de CALL3: si CALL3 tiene un locator de 6 caracteres, siempre prevalece. Como en YA-MS, los spots de meteor scatter (no de esporádica E) son los que están en las ventanas de frecuencia típicas de MS (50190-50300, 70100-70400, 144290-144399 kHz) sin etiquetas de otra propagación (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), y los que llevan modos exclusivos de MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) o `<MS>`. Excepción: FT8 y FT4 en 50 y 70 MHz, donde decide un modo conocido (el FT8 de 70,154 MHz va a la esporádica E incluso dentro de la ventana MS). Los spots con `<AS>`/`<ACS>` (aircraft scatter) no se usan para la esporádica E.

## Filtros de los spots

La recogida de datos es **una sola para todo el programa**: cada spot pasa por los filtros y va a la pestaña de cada modo de propagación. Desde la versión 0.4.18 los filtros, el filtro local y la prueba de los filtros están en la pestaña **[AJUSTES](Settings-es)**, subpestaña **Reparto de los spots**.

## CALL3

Base de datos **indicativo → locator** de JTDX (más de un millón de indicativos) en `data/call3.txt`. Botones **Recargar** e **Importar un nuevo CALL3.TXT**. Los locators de 6 caracteres leídos en los comentarios de los spots se añaden a `data/call3_nuovi.txt`.

Desde la versión 0.4.16 hay una sola base CALL3 para todo el programa, la de la Recogida de datos de YA-MS (pestaña METEOR SCATTER), con las mismas reglas que LIVETROPOVHF: actualización desde MMMonVHF, entradas manuales, archivos añadidos, correcciones del clúster, locators aprendidos y G7VRD cada 6 horas; las correcciones se escriben en data/call3.txt y los locators de data/call3_nuovi.txt se importan una vez. Los spots de WSJT-X, JTDX y MSHV llegan también por UDP (fuente WSJT-X UDP, puerto 2237 con reenvío al 2238).

Estos filtros, que vienen de YA-MS (CW, distancia mínima, ventanas de frecuencia, modos MS, bandas), valen solo para la pestaña METEOR SCATTER: deciden qué spots entran en su recogida. Las demás pestañas reciben todos los spots y usan sus propios filtros.

## Subpestañas Spot y Datos de propagación

La pestaña tiene dos subpestañas: **Spot** (sesiones, PSKReporter MQTT, WSJT-X UDP, spots recogidos y CALL3) y **Datos de propagación**:

- **N0NBH** (hamqsl.com): banner VHF y datos solares, E-skip de Europa y Norteamérica (DXrobot), aurora.
- **Bz para la aurora**: viento solar en tiempo real de NOAA SWPC (DSCOVR/ACE, punto L1), la mejor fuente para el Bz.
- **Meteor scatter**: actividad meteórica diaria (RMOB) de Make More Miles on VHF.

Se descargan como máximo una vez por hora. El resumen aparece en los mapas de SPORADIC-E, F.A.I. y AURORA. El **menú de continentes**, a la derecha de la barra de pestañas, mueve los mapas y elige los datos de ese continente.
