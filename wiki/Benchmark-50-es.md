# BENCHMARK 50 MHz

[← Home](Home-es)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Banco de pruebas para la **esporádica E multisalto en 50 MHz** con el log ADIF de una estación y, opcionalmente, de una segunda estación.

## Cargar los logs

- **Estación 1**: *Cargar ADIF* / *Quitar ADIF*. El programa toma `STATION_CALLSIGN` y `MY_GRIDSQUARE` del log; la posición de transmisión es el `MY_GRIDSQUARE` de cada QSO.
- **Estación 2** (opcional): los mismos botones en la segunda fila; la casilla *Estación 2* la muestra u oculta. Los trayectos de la estación 1 son celestes, los de la estación 2 violetas.
- El programa usa los QSO de 50 MHz con locator (los locators que faltan o de 4 caracteres se toman de CALL3), multisalto: más allá de 3100 km (regla del autor; el número de saltos sigue la tabla IK3XTV).
- Se excluyen los contactos **transecuatoriales** (regla del autor): hacia África y Sudamérica, con el punto medio en la franja de inclinación magnética entre -20° y +20°, salvo que el log tenga `PROP_MODE=ES`.

## Día y horario

Elige el **día** (al abrir, el que tiene más QSO multisalto) y el horario **de… a…** UTC, en pasos de 15 minutos. La casilla **foEs** (opcional) colorea los trayectos según la foEs mínima y la muestra en la tabla.

## El mapa

Mapa **Mercator** con centro libre: rueda = zoom, arrastrar = mover.

![Trayecto](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Ratón cerca de un trayecto**: la línea y los iconos se amplían y aparece la **cuadrícula WWL de 6 caracteres** del corresponsal.
- **Clic izquierdo** sobre ese trayecto: una ventana pregunta si mostrar **todos los QSO del archivo** (todos los días) con el extremo lejano **dentro de 150 km** de ese punto. *Sí*: el mapa se vacía y muestra esos QSO dentro del círculo naranja; *No*: el mapa se queda como está. *Volver a día y horario* devuelve a la selección anterior.

![Alrededores](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Detalles

Para el trayecto elegido: distancia y valoración según la tabla IK3XTV, azimut en ambos sentidos, saltos iguales con las cuadrículas de los puntos de reflexión, primer salto corto dentro de 1200 km (experiencia de IC8TEM) y, con la casilla foEs, la foEs mínima.
