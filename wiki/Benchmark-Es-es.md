# BENCHMARK Es

[← Home](Home-es)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Banco de pruebas para la **esporádica E** y el **multisalto** sobre un **log ADIF completo**, incluso de otra estación: a partir de los QSO realmente hechos, el programa reconstruye el skip y las nubes, día por día y hora por hora.

## Cargar el log

- **Cargar ADIF**: elige el archivo `.adi`/`.adif`. El programa recuerda el archivo y lo vuelve a cargar al arrancar.
- **Quitar ADIF**: retira el log de la pestaña.
- **Estación en el centro**: indicativo y locator de la estación que hizo el log. El programa propone el `STATION_CALLSIGN` (u `OPERATOR`) y el `MY_GRIDSQUARE` más frecuentes; se pueden cambiar a mano y confirmar con **Aplicar**. El centro no depende de los ajustes generales, así que se pueden estudiar logs de otras estaciones.
- Posición de transmisión: el `MY_GRIDSQUARE` de cada QSO (si falta, el locator del centro). Posiciones de recepción: los campos `GRIDSQUARE`.
- QSO usados: 28, 50, 70 y 144 MHz con locator. Se pueden excluir los QSO con otro `PROP_MODE` (TR, MS, FAI, AUR, EME...) y los de menos de 500 km. Los 432 MHz nunca se incluyen: la esporádica E no se aplica.

## Días y horas

- **QSO por día**: día, número de QSO, bandas, distancia máxima, multisaltos. Al abrir, el programa propone el día con más QSO.
- **Horas del día**: la fila *Todo el día* y una fila por cada hora UTC; un clic muestra solo los QSO de esa hora.
- **QSO**: la lista de la selección; al hacer clic en un QSO (o cerca del corresponsal en el mapa) se resalta y se muestran sus detalles.

## El mapa

Mapa **azimutal equidistante** centrado en la estación, con el regulador **Distancia máxima** para el zoom (también con la rueda del ratón): los anillos indican los **km desde el centro**.

- **Skip por sectores** (10° de azimut): arco naranja = QSO más cercano del sector (borde de la zona de skip), arco verde = QSO más lejano; entre ambos, la zona alcanzada.
- **Nubes Es** sobre las cuadrículas Maidenhead, coloreadas según la foEs estimada.
- Punto medio de los saltos únicos (amarillo), primer salto de la estación (naranja; círculo hueco = primer salto corto), primer salto del corresponsal (violeta).
- Para el día completo, la **línea roja** une hora por hora el baricentro de los puntos medios: es el desplazamiento de las nubes.

![Una hora](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Cálculos propuestos

1. **Skip por sectores**: para cada sector, la distancia de skip y el alcance, con los indicativos.
2. **foEs estimada** con la ley de la secante (foEs ≥ f / sec φ): el salto único más corto de cada banda da la estimación más alta; con esta foEs, la MUF esperada a 1000, 1500, 2000 y 2400 km y la distancia a partir de la cual se abrirían los 144 MHz.
3. **Nubes Es** estimadas sobre las cuadrículas, con el número de QSO y la foEs.
4. **Multisalto**: el primer salto de la estación según las dos hipótesis (saltos iguales, y primer salto corto dentro de 1200 km, experiencia de IC8TEM) comparado con las nubes vistas en salto único dentro de ±30 minutos; a menos de 300 km la hipótesis se confirma. El programa cuenta las confirmaciones de cada hipótesis.
5. **Desplazamiento de las nubes** hora por hora: km, dirección y velocidad.

Los cálculos son orientativos: un QSO dice que el trayecto estaba abierto, no exactamente dónde estaba la nube. Cuanto más limpios sean los datos (locators de 6 caracteres, `PROP_MODE` correcto), más fiable será el skip.
