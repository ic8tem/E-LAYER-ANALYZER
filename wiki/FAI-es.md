# F.A.I.

[← Home](Home-es)

El F.A.I. (Field Aligned Irregularities) es la dispersión producida por irregularidades alineadas con el campo geomagnético, típica de las tardes de verano después de la esporádica E. La señal dispersada sale sobre un **cono** cuyo eje está orientado según el campo: dos estaciones pueden comunicar si se encuentran sobre **contornos suplementarios** (a1 + a2 = 180°, artículo IC8FAX-IC8TEM; contornos ±c de K9SE). El campo magnético se calcula con el modelo **IGRF-14**.

**Alerta F.A.I.** (desde la versión 0.6.5): de mayo a septiembre, entre las 18 y las 24 hora local, tras una nube Es a 300-1300 km de tu estación, la pestaña **parpadea** y una fila roja arriba dice dónde es posible el F.A.I. (reglas del artículo IC8FAX-IC8TEM); con spots F.A.I. recientes dice que está en curso. Clic en la fila: vista Isorreflexión sobre el punto.

## Vista Scatter trail (pareja A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Introduce **Estación A** y **Estación B** (indicativo buscado en CALL3 y/o locator). A vacía = tu propia estación.
- El mapa muestra el **scatter trail**: todos los puntos a la altura de la capa, vistos por ambas estaciones, que cumplen la condición de reflexión dentro de 2,5° (clases 0-1, 1-1,5, 1,5-2, 2-2,5°).
- El recuadro de arriba a la derecha muestra **QTF y elevación** de A (rojo) y B (verde) hacia cada punto.
- El programa sugiere el **scatter point más probable** (desviación mínima, trayecto más corto).
- **Refracción 4/3**: activada por defecto, amplía los horizontes.

## Vista Isorreflexión (lugares de isorreflexión)

![Isorreflexión](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Es la construcción geométrica que sirve para **verificar si un punto de reflexión es correcto**.

- **Clic izquierdo** en el mapa: eliges el punto a verificar. **Clic derecho**: desplaza la estación B.
- **Verificar el punto recomendado**: abre el gráfico sobre el punto sugerido por el programa.
- Curvas de nivel a **0, ±1, ±2, ±4, ±8, ±16°**: bandas de color en los esquemas oscuros, **líneas gruesas** en los esquemas claros.
- Veredicto sobre la pareja: **satisfactorio** dentro de 1°, **aceptable** dentro de 2,5°, **no satisfactorio** más allá.
- Si el punto está bajo el horizonte de una de las estaciones, aparece la ventana **Cálculo imposible** y nada más.

## Verificaciones

| Contacto | Scatter point | Desviación | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44EH (sobre Génova) | 0,96° | 313° / 77° |
| IC8FAX - G4LOH | JN49BR (SO de Alemania, a 18 km de Bruselas) | 0,93° | 337° / 88° |
| Baliza DB0FAI (305°) - Dresde vía Sheffield (DL5MCG; Dresde apuntaba a 290° y había declarado aurora) | – | 0,3° | – |
| IC8FAX - FA1MXE (1990, campo IGRF de 1990) | JN45QR, estela a 35 km de Ginebra (Ginebra: 3,79°) | 0,92° | 327° / 87° |
| DL5MCG - SV1DH (prueba de 1987 vía Budapest, 1569 km, campo IGRF de 1987) | Budapest a 2,22°, dentro de los 2,5° (1,85° con el campo de hoy); estela a 21 km; más probable KN06TQ | 2,22° (KN06TQ 0,92°) | 95° / 341° |

Desde la versión 0.4.18 los dos casos con fecha (1987 y 1990) usan el campo IGRF-14 de ese año, mientras las estaciones sigan siendo las del caso.

Scatter points conocidos en la lista: Ginebra, Bruselas, Budapest, JN92, JN41/42.

El menú **Verificaciones...** carga las parejas de prueba IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE y DL5MCG-SV1DH.

**Equipo** (DL5MCG): el F.A.I. en 144 MHz requiere al menos una yagi larga y 100 W (SV1DH también fue trabajado con una de 8 elementos y 50 W). Apunte la antena al scatter point indicado por el programa, no al corresponsal.
