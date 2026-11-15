# Es CLOUDS OVER TIME

[← Home](Home)

![Es CLOUDS OVER TIME](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Sub-tab of **SPORADIC-E** (from version 0.5.2): how the **sporadic-E clouds** form, move and fade, step by step.

## The method

Method of **Chartier, Hanley and Emmons (2022)**: each sporadic-E link says that at the reflection point the foEs is **at least f / sec φ**. The points are gathered in cells and time steps, and each cell takes the **highest value** of the step. The minimum electron density is NmE = 1.24·10¹⁰ · foEs² electrons per cubic metre.

The program uses the reflection points of its own model (all hops), not only the midpoint. The value is a **lower limit**: an empty cell only means no link.

## Source and controls

- **Session spots** (refreshed every time the tab is opened) or **Load ADIF log or CSV spots**.
- **Export the session spots (CSV)**: the spot archive is emptied when the program closes, so you can review them later.
- **Bands MHz** (28, 40, 50, 70, 144), **Step** (5, 10 or 15 minutes), **Cell** (1°, 2° or 4°), **Es height km**.
- Time bar: ⏮ ◀ *Start/Stop* ▶ ⏭ and the slider; on opening it starts from the step with the most cells.
- *Earlier steps faded* shows the three steps before the selected one; *Links* draws the paths in the band colours.

## Table and chart

- Cells of the step: Cell (south-west corner and locator of the centre), foEs≥, NmE≥, Links, Bands. Click on a cell: its links are highlighted.
- Chart: highest minimum foEs (yellow line) and number of cells (bars) for each step.
