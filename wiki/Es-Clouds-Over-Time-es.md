# NUBES Es EN EL TIEMPO

[← Home](Home-es)

![NUBES Es EN EL TIEMPO](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Subpestaña de **SPORADIC-E** (desde la versión 0.5.2): cómo nacen, se desplazan y se apagan las **nubes de esporádica E**, paso a paso.

## El método

Método de **Chartier, Hanley y Emmons (2022)**: cada enlace en esporádica E dice que en el punto de reflexión la foEs es **al menos f / sec φ**. Los puntos se agrupan en celdas y en pasos de tiempo, y cada celda toma el **valor más alto** del paso. La densidad electrónica mínima es NmE = 1,24·10¹⁰ · foEs² electrones por metro cúbico.

El programa usa los puntos de reflexión de su propio modelo (todos los saltos), no solo el punto medio. El valor es un **límite inferior**: una celda vacía solo quiere decir que no hay ningún enlace.

## Fuente y controles

- **Spots de la sesión** (se actualizan cada vez que se abre la pestaña) o **Cargar log ADIF o spots CSV**.
- **Exportar los spots de la sesión (CSV)**: el archivo de los spots se vacía al cerrar el programa, así puedes revisarlos más tarde.
- **Bandas MHz** (28, 40, 50, 70, 144), **Paso** (5, 10 o 15 minutos), **Celda** (1°, 2° o 4°), **Altura Es km**.
- Barra del tiempo: ⏮ ◀ *Iniciar/Detener* ▶ ⏭ y el cursor; al abrir se parte del paso con más celdas.
- *Pasos anteriores atenuados* muestra los tres pasos anteriores al elegido; *Enlaces* dibuja los trayectos con los colores de las bandas.

## Tabla y gráfico

- Celdas del paso: Celda (esquina suroeste y locator del centro), foEs≥, NmE≥, Enlaces, Bandas. Clic en una celda: resalta sus enlaces.
- Gráfico: foEs mínima más alta (línea amarilla) y número de celdas (barras) para cada paso.
