# AURORA

[← Home](Home-es)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Óvalo auroral** NOAA OVATION Prime (sombreado), **Kp** de los últimos 3 días, **viento solar** (Bz, velocidad, densidad), actualizados cada 5 minutos y guardados en `data/swpc`.
- Las mismas vistas que en F.A.I.: **Scatter trail** para una pareja de estaciones e **Isorreflexión** para verificar un punto, con una altura por defecto de 110 km.
- **Apuntamiento recomendado para ambas estaciones**: a lo largo de la línea de perpendicularidad de cada estación (puntos de su color), el punto con la mayor probabilidad del óvalo. Regla práctica (G3WPO): empezar hacia unos 45°, entre el norte y el este.
- Ejemplo IO70VJ - JO59BC: scatter point JP01AK frente a las Shetland, desviación 0,97°, QTF 11° y 299°.
- **Spots aurora en el mapa**: los spots con **<AU>** o **<AUR>** en el texto son spots de aurora (regla del autor). Con la casilla *Spots <AU>/<AUR> en el mapa con el cálculo* marcada, la pestaña dibuja los spots de las últimas 2 horas (30 como máximo): para cada pareja spotter-DX calcula el scatter trail y muestra el scatter point más probable (círculo naranja), con QTF y elevación de ambas estaciones en los resultados.
- Además del scatter point más probable, los resultados indican el **intervalo de QTF** de ambas estaciones a lo largo de todo el Scatter trail (desviación ≤ 1,5°).

## BENCHMARK AURORA

La pestaña AURORA tiene dos subpestañas: **REAL-TIME** (el cálculo descrito arriba) y **BENCHMARK AURORA**, un banco de pruebas del modelo sobre **298 QSO de aurora reales** con hora y locator: los logs publicados de EI5FK (IO51RT, Irlanda, 2003-2005, 144 y 432 MHz) y el mapa del log de PA0O (JO33HG, Países Bajos).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Para cada QSO el programa calcula el scatter point con el mismo modelo (IGRF-14, altura 110 km, refracción 4/3, tolerancia 3°).
- Colores: **verde** = scatter point a 370-1100 km de ambas estaciones (Lange-Hesse); **amarillo** = explicado pero fuera de esa ventana; **naranja** = QSO más allá de 2000 km; **rojo** = ningún scatter point, QSO no explicado por el modelo.
- El resumen da los porcentajes y, para cada log, el intervalo y la mediana del QTF calculado. Haga clic en una fila para dibujar el trayecto y los dos apuntamientos.
- Resultado (versión 0.4.4): **296 de 298 QSO explicados (99 %)**, 243 (82 %) dentro de la ventana de 370-1100 km; QTF mediano de EI5FK entre 25° y 41°, es decir, el nordeste sugerido por G3WPO. Los logs no indican el apuntamiento: la verificación es sobre la geometría, no sobre el QTF.
