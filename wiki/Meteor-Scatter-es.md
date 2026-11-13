# METEOR SCATTER

[← Home](Home-es)

Esta pestaña contiene el **YA-MS 1.8.6** completo, el programa de meteor scatter de IC8TEM: esfera celeste con los radiantes, flujo de los radares SKiYMET (Collm, Sodankylä), cálculo de la eficiencia de las lluvias, implementación experimental de las previsiones, geometría y atenuación, GRAVES, mapa de cuadrículas, **Lluvias en directo** (ZHR medidas por la Global Meteor Network y radiantes del radar CMOR), **Mapa de las fuentes** (Collm, Sodankylä, GRAVES y CMOR respecto a la estación) y el documento **Explicación del cálculo de la eficiencia** con las fórmulas.

YA-MS 1.5.0 añade la pestaña **Recogida de datos** (spots de meteor scatter filtrados por MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Dentro de E-LAYER ANALYZER no abre sesiones telnet propias: recibe las líneas de las sesiones de DATA COLLECTION, de modo que no hay logins duplicados con el mismo indicativo.

YA-MS 1.6.0 reorganiza las pestañas en **En directo y datos en tiempo real** (tráfico meteor scatter en directo con la nube de cada contacto, flujo radar, GRAVES, recogida de datos) y **Cálculo de previsión** (previsión, geometría, cálculo de la eficiencia, implementación de las previsiones).

YA-MS 1.7.0 y 1.8.0 añaden la comparación de los radiantes con el catálogo IAU MDC, los recuentos diarios del Global Meteor Network por lluvia, el índice k obtenido de los índices de población, el radio inicial de la estela de Stober et al. (2021), la duración de los pings entre las bandas (λ² por defecto o las medidas HamSCI MSQP de NN4SA) y la comparación con las detecciones BRAMS de Sonneberg (Perseidas 2026). La actualización automática de YA-MS está desactivada dentro de E-LAYER ANALYZER, que se actualiza por sí mismo desde las Releases de GitHub.

YA-MS 1.8.1 trata el lóbulo de la antena como diagrama vertical (casilla «diagrama de antena», activada por defecto): los puntos de reflexión se ponderan con la ganancia vertical de las dos antenas y las eficiencias ya no desaparecen en los trayectos largos. Añade el aviso «ninguna zona común», destaca la casilla del ángulo β (la elección que más pesa), usa una sola fecha de cálculo, incorpora el «Manual de uso» y la comparación con 181 767 spots reales en 50 MHz del Meteor Scatter QSO Party de HamSCI de agosto de 2025: las horas y los lados del trayecto son fiables, los porcentajes son orientativos.

- YA-MS usa el idioma y el esquema de colores elegidos arriba en E-LAYER ANALYZER.
- Los ajustes de YA-MS se guardan en `data/config.ini`.
- Cada nueva versión de YA-MS se incorpora con el script `tools/sync_yams.py`.

Para más detalles, ver el [repositorio de YA-MS](https://github.com/ic8tem/YA-MS).
