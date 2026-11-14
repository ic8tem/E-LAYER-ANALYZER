# Sources and documents

[← Home](Home)

- The **SOURCES** tab lists all the sources, numbered [1]-[66].
- The **SUMMARY OF THE CALCULATION CHOICES** tab (formerly *Technical document*) explains every calculation with its formulas and the labels **SOURCE** (step taken from a source), **CHECK** (verified on the program or on a real case) and **CHOICE** (a choice made by the program, to be confirmed on the air).
- The **USER MANUAL** tab is the in-depth user manual of the program, tab by tab.
- The **TECHNICAL REPORT** tab sets out the choices that led to the making of the program: architecture, physical models, data recognition, checks, limits and future developments.
- The three documents share the same formatting, are translated into every language and can be exported to Word.

Main formulas:

- elevation towards a point of the layer: `e = atan2((r+H)·cos x − (r+h), (r+H)·sin x)`, with x = d / r and refraction k = 4/3;
- secant law: `MUF = foEs · sec φ`;
- F.A.I./aurora reflection condition: `c(A) + c(B) = 0`, where c = angle between the line of sight and the plane perpendicular to the IGRF-14 field.
