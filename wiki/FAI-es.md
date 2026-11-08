# F.A.I.

[← Home](Home-es)

El F.A.I. (Field Aligned Irregularities) es la dispersión producida por irregularidades alineadas con el campo geomagnético, típica de las tardes de verano después de la esporádica E. La señal dispersada sale sobre un **cono** cuyo eje está orientado según el campo: dos estaciones pueden comunicar si se encuentran sobre **contornos suplementarios** (a1 + a2 = 180°, artículo IC8FAX-IC8TEM; contornos ±c de K9SE). El campo magnético se calcula con el modelo **IGRF-14**.

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
| IC8FAX - F6DRO | JN44GH (sobre Génova) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (SO de Alemania, a 28 km de Bruselas) | 0,88° | 338° / 87° |
| DB0FAI - Dresde vía Sheffield (DL5MCG) | – | 0,3° | – |

Scatter points conocidos en la lista: Ginebra, Bruselas, Budapest, JN92, JN41/42.
