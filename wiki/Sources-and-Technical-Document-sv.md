# Källor och dokument

[← Home](Home-sv)

- Fliken **KÄLLOR** listar alla källor, numrerade [1]-[63].
- Fliken **SAMMANFATTNING AV BERÄKNINGSVALEN** (tidigare *Tekniskt dokument*) förklarar varje beräkning med dess formler och etiketterna **KÄLLA** (steg hämtat från en källa), **KONTROLL** (verifierat på programmet eller på ett verkligt fall) och **VAL** (ett val gjort av programmet, som ska bekräftas i luften).
- Fliken **ANVÄNDARHANDBOK** är programmets fördjupade användarhandbok, flik för flik.
- Fliken **TEKNISK RAPPORT** redogör för de val som ledde fram till programmet: arkitektur, fysikaliska modeller, igenkänning av data, kontroller, begränsningar och framtida utveckling.
- De tre dokumenten har samma formatering, är översatta till alla språk och kan exporteras till Word.

Huvudformler:

- elevation mot en punkt i skiktet: `e = atan2((r+H)·cos x − (r+h), (r+H)·sin x)`, med x = d / r och refraktion k = 4/3;
- sekantlagen: `MUF = foEs · sec φ`;
- reflexionsvillkoret för F.A.I./aurora: `c(A) + c(B) = 0`, där c = vinkeln mellan siktlinjen och planet vinkelrätt mot IGRF-14-fältet.
