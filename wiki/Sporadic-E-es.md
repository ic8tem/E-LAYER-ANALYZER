# SPORADIC-E

[← Home](Home-es)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## El mapa

Un botón encima del mapa elige la vista:

- **Azimutal - salto único**: mapa azimutal equidistante centrado en tu estación, radio fijo de 2500 km, con las zonas de la tabla IK3XTV (500 km mínimo, 1500-2000 km óptimo, 2400 km máximo del salto único).
- **Azimutal - distancia máxima (multisalto)**: el mismo mapa con el regulador **Distancia máxima** (de 300 a 20000 km, también con la rueda del ratón); aparecen además los límites del doble (5000 km) y del triple salto (6900 km), para poder seguir la evolución del multisalto.
- **Rectangular**: el mapa mundial anterior, con los botones Europa y Mundo.

En las vistas azimutales los anillos discontinuos indican los **km desde el centro** (el paso cambia con el zoom), el borde marca la distancia máxima mostrada y el cursor del ratón da la distancia y el azimut desde el centro.

![Multisalto](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Qué muestra

- Los **spots** de los últimos minutos (15 min - 12 h) en las bandas de **28, 50, 70 y 144 MHz**, tomados de las sesiones telnet. Los locators proceden del comentario del spot (p. ej. `JN70CN<ES>JO62QM`) o de la base de datos CALL3.
- Los spots con **<MS>**, **<AU>** o **<TR>** en el texto se **descartan**: no afectan a la esporádica E, pero son útiles para meteor scatter, aurora o tropo.
- **Transecuatorial, regla del autor (IC8TEM)**: hacia África y Sudamérica, si el punto medio cae en la franja de inclinación magnética entre -20° y +20°, el contacto es transecuatorial (TEP) y no esporádica E, salvo que el spot indique `<ES>`.
- **144 MHz, regla del autor (IC8TEM)**: la esporádica E en 2 m empieza **más allá de 900 km** (los spots más cortos se descartan). Un spot aislado de 144 MHz **no se dibuja por sí solo**: aparece cuando en **5 minutos** llega un segundo spot de 144 MHz con la nube en el mismo sitio (puntos de reflexión a menos de 300 km), o cuando hay cerca un spot de **50 MHz de salto corto** (a menos de 1200 km, en los últimos 15 minutos). La barra superior muestra cuántos spots de 144 MHz esperan confirmación.
- Para cada spot: distancia, **número de saltos** según la tabla **IK3XTV**, elevación, **foEs mínima** (ley de la secante).
- **Primer salto de ambas estaciones**: cuadrado naranja = primer salto del spotter, violeta = primer salto del DX (saltos iguales); amarillo = punto medio de un salto único.
- **Primer salto corto** (círculos huecos): en las aperturas multisalto el autor ha comprobado un primer salto corto dentro de **1200 km** de su QTH; la longitud se puede ajustar.
- **Nubes Es** sobre las **cuadrículas Maidenhead** (2° x 1°), coloreadas según la foEs mínima: azul < 5, celeste < 7, verde < 10, amarillo < 14, naranja < 20, rojo > 20 MHz.
- Tabla **Nubes Es**: para cada cuadrícula, la **MUF para tu estación** (rojo = 144 MHz posible, naranja = 70, amarillo = 50).

## Tabla IK3XTV

| | mínima | óptima | máxima |
|---|---|---|---|
| Salto único | 500-650 km | 1500-2000 km | 2200-2400 km |
| Doble salto | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Triple salto | – | 4800-6400 km | 6900 km |

## Cómo usarla

- **Clic en un spot**: el trayecto se resalta, con puntos de reflexión, locators y azimuts en los detalles.
- **Clic en un spot** de la tabla: el trayecto se ilumina en el mapa (halo amarillo) y, si está fuera de la vista, el mapa se desplaza o amplía su radio para mostrarlo. El spot sigue seleccionado cuando llegan spots nuevos.
- **La tabla sigue al mapa**: con un zoom aplicado, la tabla de spots y la de nubes muestran solo lo que es visible en el mapa.
- **Clic en el mapa**: el punto se trata como una posible nube Es, con la foEs necesaria para 28, 50, 70 y 144 MHz.

## Nota sobre 432 MHz

La esporádica E **no se aplica a 432 MHz**: la MUF de la Es llega a unos 150-200 MHz y los récords de la ARRL recogen contactos Es solo en 50 y 144 MHz. Los spots de 70 cm no se analizan aquí.
