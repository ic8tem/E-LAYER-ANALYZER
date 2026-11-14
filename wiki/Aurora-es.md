# AURORA

[← Home](Home-es)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Óvalo auroral** NOAA OVATION Prime (sombreado), **Kp** de los últimos 3 días, **viento solar** (Bz, velocidad, densidad), actualizados cada 5 minutos y guardados en `data/swpc`.
- Las mismas vistas que en F.A.I.: **Scatter trail** para una pareja de estaciones e **Isorreflexión** para verificar un punto, con una altura por defecto de 110 km.
- **Apuntamiento recomendado para ambas estaciones**: a lo largo de la línea de perpendicularidad de cada estación (puntos de su color), el punto con la mayor probabilidad del óvalo. Regla práctica (G3WPO): empezar hacia unos 45°, entre el norte y el este.
- Ejemplo IO70VJ - JO59BC: scatter point JP01AK frente a las Shetland, desviación 0,97°, QTF 11° y 299°.
- **Spots aurora en el mapa**: los spots con **<AU>** o **<AUR>** en el texto son spots de aurora (regla del autor). Con la casilla *Spots <AU>/<AUR> en el mapa con el cálculo* marcada, la pestaña dibuja los spots de las últimas 2 horas (30 como máximo): para cada pareja spotter-DX calcula el scatter trail y muestra el scatter point más probable (círculo naranja), con QTF y elevación de ambas estaciones en los resultados.
- Además del scatter point más probable, los resultados indican el **intervalo de QTF** de ambas estaciones a lo largo de todo el Scatter trail (desviación ≤ 1,5°).
- **Ventana de Lange-Hesse**: el scatter point se prefiere dentro de 370-1100 km de cada estación, la ventana calculada por Lange-Hesse a 54° de latitud geomagnética. Desde la versión 0.4.5 la ventana se desplaza con la inclinación IGRF-14 de cada estación: más cerca para las estaciones más al sur, más lejos para las más al norte (p. ej. Capri unos 260-990 km, Oslo 470-1200 km). Los resultados muestran la ventana de ambas estaciones.

## BENCHMARK AURORA

La pestaña AURORA tiene dos subpestañas: **REAL-TIME** (el cálculo descrito arriba) y **BENCHMARK AURORA**, un banco de pruebas del modelo sobre **298 QSO de aurora reales** con hora y locator: los logs publicados de EI5FK (IO51RT, Irlanda, 2003-2005, 144 y 432 MHz) y el mapa del log de PA0O (JO33HG, Países Bajos).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Para cada QSO el programa calcula el scatter point con el mismo modelo (IGRF-14, altura 110 km, refracción 4/3, tolerancia 3°).
- Colores: **verde** = scatter point dentro de la ventana de Lange-Hesse de ambas estaciones; **amarillo** = explicado pero fuera de esa ventana; **naranja** = QSO más allá de 2000 km; **rojo** = ningún scatter point, QSO no explicado por el modelo.
- El resumen da los porcentajes, también con la ventana fija 370-1100 km para comparar, y, para cada log, el intervalo y la mediana del QTF calculado. Haga clic en una fila para dibujar el trayecto y los dos apuntamientos.
- Resultado (versión 0.4.18, campo IGRF-14 del año de cada QSO): **297 de 298 QSO explicados (100 %)**, 267 (90 %) dentro de la ventana de Lange-Hesse de ambas estaciones (275, el 92 %, con la ventana fija de 370-1100 km); QTF mediano de EI5FK entre 32° y 53°, es decir, el nordeste sugerido por G3WPO. Los logs no indican el apuntamiento: la verificación es sobre la geometría, no sobre el QTF.
- **Corrección**: de la versión 0.4.4 a la 0.4.17 el banco de pruebas arrancaba antes de cargar IGRF-14 y calculaba con el dipolo y la ventana fija; los resultados publicados entonces (243 QSO, 82 %, QTF mediano 25-41°) eran del dipolo. Corregido en la 0.4.18.
