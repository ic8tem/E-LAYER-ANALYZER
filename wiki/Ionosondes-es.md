# IONOSONDAS

[← Home](Home-es)

![IONOSONDAS](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Subpestaña de **SPORADIC-E** (desde la versión 0.5.2): la **foEs medida** por las ionosondas de la red **GIRO** en todo el mundo, junto a la que el programa obtiene de los spots.

## Los datos

- 62 ionosondas GIRO (lista ULCAR), con la última **foEs**, **foF2** y **MUF(D)** leídas automáticamente de los ionogramas.
- Los datos llegan la primera vez que se abre la pestaña y luego **cada 15 minutos**, solos (casilla *Actualizar solo cada 15 minutos*); *Actualizar ahora* los pide de inmediato. El archivo guarda las últimas 48 horas en la carpeta `giro` de los datos.
- Datos de **GIRO** (Reinisch y Galkin 2011), licencia **CC BY-NC-SA 4.0**: solo uso no comercial. El servicio es el mismo de *MUF Muncher* de DM6AX.

## Mapa y tabla

- Cada ionosonda es un punto de color con la última foEs (mismos colores de las nubes Es) y el valor en MHz; **gris** si no tiene datos recientes, con el **anillo** si el último ionograma tiene más de 90 minutos.
- Tabla: Código, Ionosonda, km desde tu estación, UTC del último ionograma, foEs, foF2, MUF(D) y **Spot foEs≥**: la foEs mínima obtenida de los spots Es de los últimos 30 minutos que se reflejan a menos de 500 km de la ionosonda (ajustable), con el número de spots.
- **h'Es GIRO** (desde la versión 0.6.5, casilla en REAL-TIME): la altura Es pasa a ser la mediana de las h'Es recientes de las ionosondas a menos de 2500 km (80-120 km).

## La ionosonda elegida

Clic en una fila o cerca de una ionosonda en el mapa:

- **gráfico de las últimas 24 horas**: foEs amarilla, foF2 celeste, MUF(D) violeta de puntos;
- **MUF de la esporádica E** a 1000, 1500, 2000 y 2400 km (MUF = foEs · sec φ, a la altura Es elegida) con las **bandas que se abren** (28, 40, 50, 70, 144 MHz);
- si los spots piden más que la foEs medida, el programa lo señala: la nube puede estar lejos de la ionosonda, o la ionosonda ha llegado al límite de su rango de frecuencias (Chartier 2022).
