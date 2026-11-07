const fs = require('fs');
const d = require('docx');
const { Document, Packer, Paragraph, TextRun, HeadingLevel, AlignmentType, TabStopType, Table, TableRow, TableCell,
  WidthType, ShadingType, BorderStyle, LevelFormat, Footer, PageNumber, TableOfContents,
  Math: M, MathRun, MathFraction, MathSubScript, MathSuperScript, MathSubSuperScript, MathRadical, MathRoundBrackets,
  MathSquareBrackets, MathFunction, MathSum } = d;

// ---------- mini DSL per le equazioni ----------
const A = (x) => (Array.isArray(x) ? x : [typeof x === 'string' ? new MathRun(x) : x]).flat()
  .map(e => typeof e === 'string' ? new MathRun(e) : e);
const r = (t) => new MathRun(t);
const fr = (n, den) => new MathFraction({ numerator: A(n), denominator: A(den) });
const sb = (b, s) => new MathSubScript({ children: A(b), subScript: A(s) });
const sp = (b, s) => new MathSuperScript({ children: A(b), superScript: A(s) });
const sbsp = (b, s, p) => new MathSubSuperScript({ children: A(b), subScript: A(s), superScript: A(p) });
const sq = (x, deg) => new MathRadical(deg ? { children: A(x), degree: A(deg) } : { children: A(x) });
const br = (...x) => new MathRoundBrackets({ children: A(x.flat()) });
const sbr = (...x) => new MathSquareBrackets({ children: A(x.flat()) });
const fn = (name, ...x) => new MathFunction({ name: A(name), children: A(x.flat()) });
const sum = (s, x) => new MathSum({ children: A(x), subScript: A(s) });

let eqn = 0;
function EQ(...parts) {
  eqn++;
  return new Paragraph({
    tabStops: [{ type: TabStopType.CENTER, position: 4819 }, { type: TabStopType.RIGHT, position: 9638 }],
    spacing: { before: 100, after: 140 },
    children: [new TextRun('\t'), new M({ children: A(parts.flat()) }), new TextRun('\t(' + eqn + ')')],
  });
}
const FONT = 'Calibri';
function P(...runs) {
  return new Paragraph({ spacing: { after: 120 }, children: runs.map(x => typeof x === 'string' ? new TextRun(x) : x) });
}
const B = (t) => new TextRun({ text: t, bold: true });
const I = (t) => new TextRun({ text: t, italics: true });
function TAG(kind, text) {
  const col = { FONTE: '1F5FAD', RILIEVO: '2E7D32', SCELTA: 'B45309' }[kind];
  return new Paragraph({ spacing: { after: 120 }, children: [new TextRun({ text: '[' + kind + '] ', bold: true, color: col }), new TextRun(text)] });
}
const H1 = (t) => new Paragraph({ heading: HeadingLevel.HEADING_1, children: [new TextRun(t)] });
const H2 = (t) => new Paragraph({ heading: HeadingLevel.HEADING_2, children: [new TextRun(t)] });
const bullet = (t) => new Paragraph({ numbering: { reference: 'bul', level: 0 }, spacing: { after: 60 }, children: (Array.isArray(t) ? t : [t]).map(x => typeof x === 'string' ? new TextRun(x) : x) });

const border = { style: BorderStyle.SINGLE, size: 4, color: 'B0B7C3' };
const borders = { top: border, bottom: border, left: border, right: border };
function table(widths, rows) {
  const tw = widths.reduce((a, b) => a + b, 0);
  return new Table({
    width: { size: tw, type: WidthType.DXA }, columnWidths: widths,
    rows: rows.map((row, ri) => new TableRow({
      tableHeader: ri === 0,
      children: row.map((c, ci) => new TableCell({
        width: { size: widths[ci], type: WidthType.DXA }, borders,
        shading: ri === 0 ? { fill: 'DCE6F2', type: ShadingType.CLEAR, color: 'auto' } : undefined,
        margins: { top: 60, bottom: 60, left: 100, right: 100 },
        children: [new Paragraph({ children: [ri === 0 ? new TextRun({ text: c, bold: true }) : (typeof c === 'string' ? new TextRun(c) : new M({ children: A(c) }))] })],
      })),
    })),
  });
}

// ---------- contenuto ----------
const C = [];
C.push(new Paragraph({ heading: HeadingLevel.TITLE, children: [new TextRun('YA-MS - Manuale dei calcoli')] }));
C.push(P(I('Previsione e calcolo in diretta dell\'efficienza degli sciami meteorici per il meteor scatter. Versione 1.0.0, a cura di Costantino IC8TEM, novembre 2026.')));
C.push(new TableOfContents('Indice', { hyperlink: true, headingStyleRange: '1-2' }));
C.push(P(I('(Se l\'indice è vuoto: in Word, clic destro sull\'indice > Aggiorna campo.)')));

C.push(H1('1. Convenzioni'));
C.push(P('Ogni passo del calcolo è accompagnato da un\'etichetta che ne indica l\'origine. Le fonti sono numerate nel capitolo 11.'));
C.push(TAG('FONTE', 'il passo riprende una fonte citata.'));
C.push(TAG('RILIEVO', 'confronto tra il programma e le fonti, con l\'esito della verifica.'));
C.push(TAG('SCELTA', 'scelta del programma, non presa da una fonte; va verificata con l\'esperienza in banda.'));
C.push(P('Angoli in gradi salvo diversa indicazione; distanze in km; tempi in UTC. I vettori sono in grassetto e sono riferiti a un sistema geocentrico solidale con la Terra (asse x verso il meridiano di Greenwich all\'equatore, asse z verso il polo nord).'));
C.push(table([1900, 7738], [
  ['Simbolo', 'Significato'],
  [['r'], 'raggio terrestre medio, 6372,7955 km [1]'],
  [['H'], 'quota della scia (strato E), dal radar SKiYMET'],
  [['h'], 'quota della stazione sul livello del mare'],
  [[sb('φ', 'A'), ', ', sb('λ', 'A')], 'latitudine e longitudine della stazione A (la mia) e della stazione B (il corrispondente)'],
  [['α, δ'], 'ascensione retta e declinazione del radiante'],
  [[sb('λ', '⊙')], 'longitudine solare (equinozio J2000)'],
  [['ŝ'], 'versore della direzione del radiante'],
  [['Az, El'], 'azimut (da nord verso est) ed elevazione'],
]));

C.push(H1('2. Tempo e posizione del radiante'));
C.push(H2('2.1 Giorno giuliano'));
C.push(TAG('FONTE', 'Meeus, Astronomical Algorithms [6], cap. 7. Con Y, M l\'anno e il mese (per gennaio e febbraio Y−1 e M+12) e D il giorno con la frazione decimale:'));
C.push(EQ('JD=', fn('int', br('365,25', br('Y+4716'))), '+', fn('int', br('30,6001', br('M+1'))), '+D+B−1524,5'));
C.push(EQ('B=2−A+', fn('int', fr('A', '4')), ',   A=', fn('int', fr('Y', '100'))));
C.push(EQ('T=', fr('JD−2451545,0', '36525')));
C.push(H2('2.2 Tempo siderale di Greenwich'));
C.push(TAG('FONTE', 'Meeus [6], cap. 12:'));
C.push(EQ('θ', '=280,46061837+360,98564736629', br('JD−2451545'), '+0,000387933 ', sp('T', '2'), '−', fr(sp('T', '3'), '38710000')));
C.push(H2('2.3 Longitudine solare'));
C.push(TAG('FONTE', 'Meeus [6], cap. 25 (precisione bassa, circa 0,01°). L\'IMO esprime i massimi in longitudine solare riferita all\'equinozio J2000 [5]; il termine −1,397 T riporta il valore all\'equinozio J2000.'));
C.push(EQ(sb('L', '0'), '=280,46646+36000,76983 T+0,0003032 ', sp('T', '2')));
C.push(EQ('M=357,52911+35999,05029 T−0,0001537 ', sp('T', '2')));
C.push(EQ('C=', br('1,914602−0,004817 T'), fn('sin', 'M'), '+', br('0,019993−0,000101 T'), fn('sin', '2M'), '+0,000289 ', fn('sin', '3M')));
C.push(EQ(sb('λ', '⊙'), '=', sb('L', '0'), '+C−1,397 T'));
C.push(TAG('SCELTA', 'L\'istante del massimo di uno sciame si trova risolvendo λ⊙(JD) = λmax con il metodo di Newton (8 iterazioni, passo 0,98561° al giorno).'));
C.push(TAG('RILIEVO', 'Massimi 2026 calcolati: Quadrantidi 3/1, Perseidi 13/8, Orionidi 21/10, Leonidi 17/11, Geminidi 14/12. Coincidono con il calendario IMO 2026 [5].'));
C.push(H2('2.4 Radiante e sua deriva'));
C.push(TAG('FONTE', 'α, δ e λmax dai calendari IMO 2026 e 2027 [5], tabelle 5 e 7, compresi gli sciami minori della lista IMO (per esempio Camelopardalidi di ottobre, Andromedidi, sciami diurni radio di aprile e maggio); per la sorgente Antielio la tabella 6, interpolata linearmente tra le date. Gli sciami minori contribuiscono all\'indice della tratta e al consiglio dell\'hot spot con il loro ZHR.'));
C.push(EQ('α=', sb('α', '0'), '+', fr('dα', 'dλ'), ' Δλ,   δ=', sb('δ', '0'), '+', fr('dδ', 'dλ'), ' Δλ,   Δλ=', sb('λ', '⊙'), '−', sb('λ', 'max')));
C.push(TAG('SCELTA', 'Le derive dα/dλ e dδ/dλ sono valori di lavoro scritti in data\\sciami.csv: la tabella delle derive IMO non è ancora stata inserita.'));
C.push(H2('2.5 Dal radiante all\'azimut e all\'elevazione'));
C.push(TAG('FONTE', 'Conversione vettoriale equivalente alle formule di Meeus [6], cap. 13. Angolo orario di Greenwich H = θ − α:'));
C.push(EQ('ŝ=', sbr(fn('cos', 'δ'), fn('cos', 'H'), ' ;  −', fn('cos', 'δ'), fn('sin', 'H'), ' ;  ', fn('sin', 'δ'))));
C.push(P('Nel punto di latitudine φ e longitudine λ si usano i versori locali alto (û), est (ê) e nord (n̂):'));
C.push(EQ('û=', sbr(fn('cos', 'φ'), fn('cos', 'λ'), ' ; ', fn('cos', 'φ'), fn('sin', 'λ'), ' ; ', fn('sin', 'φ')), ',   ê=', sbr('−', fn('sin', 'λ'), ' ; ', fn('cos', 'λ'), ' ; 0'), ',   n̂=û×ê'));
C.push(EQ('El=', fn('arcsin', br('ŝ·û')), ',    Az=', fn('atan2', br('ŝ·ê , ŝ·n̂'))));
C.push(P('Nota: nel programma il segno di sin H è incluso nella forma (θ − α); il risultato è lo stesso.'));
C.push(H2('2.6 Attività dello sciame'));
C.push(TAG('FONTE', 'ZHRmax dal calendario IMO 2026 [5].'));
C.push(TAG('SCELTA', 'Profilo esponenziale attorno al massimo, con pendenza diversa in salita e in discesa (valori di lavoro in sciami.csv):'));
C.push(EQ('ZHR', br(sb('λ', '⊙')), '=', sb('ZHR', 'max'), '·', sp('10', ['−', sb('B', '±'), '|Δλ|'])));

C.push(H1('3. Geografia della tratta'));
C.push(H2('3.1 Dal locator alle coordinate'));
C.push(TAG('FONTE', 'Sistema Maidenhead (IARU). Per un locator di 6 caratteri L1 L2 N3 N4 L5 L6 (lettere contate da A = 0), al centro del quadratino:'));
C.push(EQ('λ=20·', sb('L', '1'), '−180+2·', sb('N', '3'), '+', fr(sb('L', '5'), '12'), '+', fr('1', '24'), ',    φ=10·', sb('L', '2'), '−90+', sb('N', '4'), '+', fr(sb('L', '6'), '24'), '+', fr('1', '48')));
C.push(H2('3.2 Distanza, azimut e punto medio'));
C.push(TAG('FONTE', 'Formule del cerchio massimo su sfera di raggio r [1]:'));
C.push(EQ('D=r·', fn('arccos', br(sb('û', 'A'), '·', sb('û', 'B')))));
C.push(EQ(sb('Az', 'AB'), '=', fn('atan2', br(fn('sin', 'Δλ'), fn('cos', sb('φ', 'B')), ' , ', fn('cos', sb('φ', 'A')), fn('sin', sb('φ', 'B')), '−', fn('sin', sb('φ', 'A')), fn('cos', sb('φ', 'B')), fn('cos', 'Δλ')))));
C.push(P('Il punto medio M è il punto a distanza D/2 da A lungo l\'azimut AzAB.'));
C.push(TAG('FONTE', 'Il calcolo si riferisce al punto medio della tratta, perché tra le due estremità l\'ora migliore può differire fino a 1,5 ore (SM6FHZ [4]).'));
C.push(TAG('SCELTA', 'La tratta è sempre quella reale tra i due locator. L\'antenna si considera sempre puntata verso il corrispondente (Az(antenna) = AzAB), per il controllo del lobo (capitolo 7).'));

C.push(H1('4. Geometria del meteor scatter'));
C.push(H2('4.1 Quota delle scie'));
C.push(TAG('FONTE', 'Lo sciame produce scie ionizzate nello strato E, tra 80 e 120 km; i radar SKiYMET danno una quota media di circa 90 km [1].'));
C.push(TAG('SCELTA', 'Si usa la quota più frequente (moda) dell\'istogramma del radar di Collm [7] (capitolo 8).'));
C.push(TAG('RILIEVO', 'Grafico del 5/11/2026: moda 89,5 km, media 88,6 km, deviazione standard 6,8 km. Conferma i 90 km dell\'articolo [1].'));
C.push(H2('4.2 Elevazione d\'antenna'));
C.push(TAG('FONTE', 'Formula dell\'articolo [1] (Immagine 7), con x = d/r e d la distanza a terra dal punto di riflessione:'));
C.push(EQ('e=90°−57,3·', fn('arctan', sbr(fr(['r ', fn('tan', 'x'), '+', br('H−', fr(['r', br('1−', fn('cos', 'x'))], fn('cos', 'x'))), fn('sin', 'x')], ['H−', fr(['r', br('1−', fn('cos', 'x'))], fn('cos', 'x')), '−h ', fn('sin', 'x')])))));
C.push(TAG('RILIEVO', 'Coincide entro 0,03° con la geometria sferica esatta. Il programma usa l\'arcotangente a due argomenti, che oltre l\'orizzonte dà valori negativi ed evita la periodicità segnalata nell\'articolo.'));
C.push(H2('4.3 Portata alla quota delle scie'));
C.push(TAG('SCELTA', 'Distanza a terra alla quale un raggio con elevazione e raggiunge la quota H (inversa geometrica della 4.2):'));
C.push(EQ(sb('d', 'H'), '=r', sbr(fr('π', '2'), '−e−', fn('arcsin', br(fr([br('r+h'), fn('cos', 'e')], 'r+H'))))));
C.push(TAG('RILIEVO', 'Con H = 89,5 km ed e = 0° si ottengono 1062 km. Nell\'esempio dell\'articolo (JN44BB–IO53RT) il disegno pone gli incroci delle curve d\'orizzonte (paragrafo 4.4) a circa 870 km dalle stazioni, che corrispondono a un lobo elevato di circa 2° (865 km).'));
C.push(H2('4.4 Zona comune e suoi limiti (Limite A, Limite B)'));
C.push(TAG('FONTE', 'L\'articolo [1] individua gli incroci delle curve dove l\'orizzonte di ciascuna stazione incontra lo strato E. Sono gli incroci di due cerchi di raggio angolare ρ = dH/r centrati in A e B:'));
C.push(EQ(sb('p', '0'), '=', fr(fn('cos', 'ρ'), ['1+', sb('û', 'A'), '·', sb('û', 'B')]), br(sb('û', 'A'), '+', sb('û', 'B'))));
C.push(EQ(sb('p', 'A,B'), '=', sb('p', '0'), '±', sq(fr(['1−', sp(['|', sb('p', '0'), '|'], '2')], sp(['|', sb('û', 'A'), '×', sb('û', 'B'), '|'], '2'))), br(sb('û', 'A'), '×', sb('û', 'B'))));
C.push(P('Se 1 − |p0|² < 0 la tratta è più lunga di 2·dH e le due stazioni non vedono una zona comune dello strato E.'));
C.push(TAG('RILIEVO', 'Questi due punti sono i bordi più lontani della zona di strato E vista da entrambe le stazioni, non i punti dove arrivano più riflessioni. Sulla tratta JN70CN–IN78UO (1710 km, azimut 308°) cadono a 1062 km dalla stazione, 37° a sinistra e a destra della direzione diretta: troppo di lato per il puntamento. Il programma li chiama "Limite A" e "Limite B" e li disegna tenui sulla mappa, insieme ai due cerchi della zona comune.'));
C.push(H2('4.5 Hot A e Hot B (hot spot per il puntamento)'));
C.push(TAG('FONTE', 'Le riflessioni delle meteore sporadiche si concentrano in due zone ("hot spot") a circa 50–150 km ai lati del punto medio della tratta. Da qui viene la tabella degli offset d\'antenna dell\'AMS Bulletin n. 203, ripresa da W8WN [15]: 10° di scarto per tratte di 1500 e di 1800 km. MSHV di LZ2HV [16] dà gli stessi valori.'));
C.push(TAG('SCELTA', 'Hot A e Hot B sono i punti a distanza s dal punto medio M, in direzione perpendicolare alla tratta (s = 150 km, modificabile da 30 a 400 km); A a sinistra guardando dalla stazione verso il corrispondente, B a destra:'));
C.push(EQ(sb('P', 'A,B'), '=destinazione', br('M , ', sb('Az', 'M→B'), '∓90° , s')));
C.push(P('Lo scarto dalla direzione diretta, visto dalla stazione, vale con buona approssimazione:'));
C.push(EQ('Δ', sb('Az', 'hot'), '≈', fn('arctan', fr('s', 'D/2'))));
C.push(TAG('RILIEVO', 'JN70CN–IN78UO, D = 1710 km: con s = 100 km lo scarto è 6,7°, con s = 150 km 10,0°, con s = 200 km 13,2°. Il valore di 150 km riproduce i 10° di MSHV e della tabella AMS/W8WN [15, 16].'));
C.push(H2('4.6 Hot consigliato dall\'efficienza'));
C.push(TAG('SCELTA', 'Per l\'ora mostrata sulla volta celeste il programma somma, su tutti gli sciami sopra l\'orizzonte, il peso ZHR·η/100 diviso tra lato A (frazione fA, capitolo 5.5) e lato B. L\'hot spot del lato con il peso maggiore è quello consigliato e compare in rosso sulla mappa:'));
C.push(EQ(sb('W', 'A'), '=', sum('sciami', [fr(['ZHR·η'], '100'), '·', sb('f', 'A')]), ',    ', sb('W', 'B'), '=', sum('sciami', [fr(['ZHR·η'], '100'), '·', br('1−', sb('f', 'A'))]), ',    consigliato = lato con W maggiore'));
C.push(TAG('FONTE', 'Lo spostamento di 10–30° verso il lato delle riflessioni (side-scatter) è indicato nell\'articolo [1].'));
C.push(TAG('FONTE', 'DJ5HG, riportato da DF5HC [17]: la riflessione avviene su un cono attorno alla scia; per le tratte più corte le antenne vanno puntate a lato della direzione diretta, entrambe le stazioni verso lo stesso punto, e secondo la posizione del radiante uno dei due punti (HOT-A o HOT-B) è più favorevole. È lo stesso criterio del consiglio del programma.'));

C.push(H1('5. Efficienza dello sciame'));
C.push(H2('5.1 Concetto'));
C.push(TAG('FONTE', 'L\'efficienza indica in percentuale quanto la posizione dello sciame permette riflessioni su una tratta (OH5IY [2], ripreso nell\'articolo [1]); MCG_MS [3] la calcola per quattro direzioni fisse.'));
C.push(H2('5.2 Condizione di riflessione speculare'));
C.push(TAG('FONTE', 'Una scia rimanda il segnale tra A e B solo se è tangente a un ellissoide che ha A e B nei fuochi (McKinley [8]). Le scie di uno sciame sono parallele alla direzione del radiante [1, 8]. Per un punto P dello strato E, con i versori verso le stazioni:'));
C.push(EQ(sb('ê', 'A'), '=', fr(['A−P'], '|A−P|'), ',   ', sb('ê', 'B'), '=', fr(['B−P'], '|B−P|'), ',   n̂=', fr([sb('ê', 'A'), '+', sb('ê', 'B')], ['|', sb('ê', 'A'), '+', sb('ê', 'B'), '|'])));
C.push(EQ('condizione speculare:   ŝ·n̂=0'));
C.push(TAG('FONTE', 'La stessa legge è alla base di QTFGUI di DJ5HG: il raggio che colpisce la scia (o, in aurora, la linea del campo magnetico) viene diffuso su un cono attorno ad essa, e solo chi sta sul cono riceve il segnale. La condizione del cono equivale a ŝ·n̂ = 0. [18]'));
C.push(TAG('SCELTA', 'La condizione si accetta con una tolleranza gaussiana di σ = 0,05 rad (circa 3°), per tenere conto della larghezza della zona utile della scia:'));
C.push(EQ(sb('S', 'P'), '=', sp('e', ['−', fr(sp(br('ŝ·n̂'), '2'), ['2', sp('σ', '2')])])));
C.push(TAG('RILIEVO', 'DJ5HG indica per l\'aurora una perdita di circa 10 dB per grado di scarto dalla legge di riflessione (mai QSO oltre 2,5°), mentre per il meteor scatter QTFGUI disegna scarti fino a 32°. Provata nel modello, la legge di 10 dB/grado darebbe ancora l\'11 % alle 08:00 dell\'esempio, contro la "nessuna zona" di QTFGUI; la tolleranza gaussiana di 0,05 rad resta quindi quella del programma. Con il campionamento a 5 km invece di 15 km le percentuali cambiano meno di 0,3 punti. [18]'));
C.push(H2('5.3 Peso di ciascun punto'));
C.push(TAG('FONTE', 'Potenza di una scia sottodensa (McKinley [8]): proporzionale a λ³ / [R1 R2 (R1+R2) cos²φ], con R1 = |A−P|, R2 = |B−P| e φ metà dell\'angolo di scattering:'));
C.push(EQ('φ=', fr('1', '2'), fn('arccos', br(sb('ê', 'A'), '·', sb('ê', 'B'))), ',    ', sb('w', 'P'), '=', fr('1', [sb('R', '1'), sb('R', '2'), br(sb('R', '1'), '+', sb('R', '2')), sp(fn('cos', 'φ'), '2')])));
C.push(TAG('SCELTA', 'φ limitato a 85° per evitare valori infiniti nelle direzioni radenti.'));
C.push(TAG('FONTE', 'Il numero di meteore che arrivano in un punto è proporzionale al seno dell\'elevazione del radiante in quel punto (la stessa correzione usata dall\'IMO per lo ZHR [5]): ŝ·ûP.'));
C.push(H2('5.4 Somma e percentuale'));
C.push(TAG('SCELTA', 'I punti P si campionano ogni 15 km alla quota H, solo dove entrambe le stazioni vedono il punto sopra l\'elevazione minima del lobo. Per ogni direzione del radiante:'));
C.push(EQ('F', br('ŝ'), '=', sum('P', [sb('w', 'P'), sb('S', 'P'), 'max', br('0 , ŝ·', sb('û', 'P'))])));
C.push(EQ('η', br('ŝ'), '=100·', fr(['F', br('ŝ')], [sb('max', 'ŝ′'), ' F', br('ŝ′')]), '  %'));
C.push(P('F si calcola una volta per tratta su una griglia di direzioni (azimut ogni 5°, elevazione ogni 2,5°, nel riferimento del punto medio) e poi si interpola in modo bilineare.'));
C.push(TAG('RILIEVO', 'Il risultato segue la regola di SM6FHZ [4]: la tratta migliore è ortogonale all\'azimut del radiante. Il miglior puntamento d\'antenna è quindi Az(radiante) ± 90°, con tolleranza ±40–50°. Con il radiante allineato alla tratta e alto nel cielo l\'efficienza è quasi nulla.'));
C.push(TAG('RILIEVO', 'Termine di confronto indipendente: la formula di DH9GCD riportata da DF5HC [17], con El e Az elevazione e azimut del radiante e P azimut del corrispondente. È massima per El = 45° e P − Az = 90°; DF5HC considera il radiante troppo basso sotto 20° e troppo alto sopra 70°. Non tiene conto della distanza né delle quote:'));
C.push(EQ(sb('E', 'DH9GCD'), '=', fn('sin', 'El'), '·', fn('cos', 'El'), '·', fn('sin', br('P−Az'))));
C.push(TAG('SCELTA', 'Nella linguetta "Calcolo dell\'efficienza" il programma mostra sotto ogni sciame, in azzurro, la formula in percentuale del suo massimo, con El e Az visti dalla stazione e P azimut del corrispondente; con il simbolo ↑ (e un avviso sulla volta celeste) segnala il radiante sopra 70°:'));
C.push(EQ(sb('E', '%'), '=200·', fn('sin', 'El'), '·', fn('cos', 'El'), '·|', fn('sin', br('P−Az')), '|'));
C.push(TAG('RILIEVO', 'Confronto con QTFGUI di DJ5HG (esempio del suo manuale): tratta Amburgo JO53IM - Barcellona, Quadrantidi, 4 gennaio. QTFGUI dà alle 07:00 UTC puntamento 252°, elevazione 1°, efficacia relativa -8 dB, radiante nella zona di riflessione az. 83° el. 75°; alle 08:00 nessuna zona di riflessione; alle 09:00 puntamento 168°, elevazione 2°, -4 dB, radiante az. 292° el. 74°. Il modello del programma (quota 102 km come QTFGUI) dà alle 07:00 241°, 0°, -5,4 dB, radiante az. 77° el. 74°; alle 08:00 0,6 % (-22 dB), cioè praticamente nulla; alle 09:00 161°, 4°, -4,0 dB, radiante az. 286° el. 75°. DH9GCD dà 26 %, 2 % e 32 %: tutti e tre i metodi vedono il buco delle 08:00, con il radiante quasi allo zenit [18].'));
C.push(TAG('RILIEVO', 'La scala di OH5IY e di MCG_MS non è nota in dettaglio (i manuali di MS-Soft non sono disponibili). Le percentuali vanno confrontate con quei programmi prima di essere considerate equivalenti.'));
C.push(H2('5.5 Lato delle riflessioni e indice della tratta'));
C.push(TAG('SCELTA', 'Frazione delle riflessioni dal lato A (sinistra della tratta); nella tabella oraria compare la lettera del lato prevalente:'));
C.push(EQ(sb('f', 'A'), '=', fr(sum('P∈A', [sb('w', 'P'), sb('S', 'P'), '(ŝ·', sb('û', 'P'), ')']), sum('P', [sb('w', 'P'), sb('S', 'P'), '(ŝ·', sb('û', 'P'), ')']))));
C.push(TAG('FONTE', 'Side-scatter: spostamento del puntamento di 10–30° verso il lato prevalente [1].'));
C.push(TAG('SCELTA', 'Indice della tratta, somma su tutti gli sciami attivi:'));
C.push(EQ('I', br('t'), '=', sum('sciami', [fr(['ZHR', br('t'), '·', 'η', br('t')], '100')])));

C.push(H2('5.6 Zone PREV in pianta e sezione vista dall\'antenna'));
C.push(TAG('SCELTA', 'Per l\'ora mostrata il programma somma, punto per punto dello strato E, il contributo di ogni sciame attivo all\'efficienza (la stessa condizione di riflessione speculare del capitolo 5), pesato con lo ZHR. I punti con almeno il 3 % del massimo formano le zone PREV, una per lato della tratta, disegnate in pianta come ellissi: centro = media pesata dei punti, assi = due volte la loro dispersione. È la regola pratica: se lo sciame scorre da nord a sud, le scie utili cadono a est e a ovest. La linea gialla indica la direzione di scorrimento (azimut del radiante) dello sciame selezionato, o del più favorevole.'));
C.push(EQ(sb('W', 'P'), '=', sum('sciami', ['ZHR·', sb('c', 'P'), br('ŝ')]), ',    ', sb('c', 'P'), '=100·', fr([sb('w', 'P'), sb('S', 'P'), 'max', br('0 , ŝ·', sb('û', 'P'))], [sb('max', 'ŝ′'), ' F', br('ŝ′')]), ',    PREV: ', sb('W', 'P'), '≥0,03·max W'));
C.push(TAG('SCELTA', 'La sezione mostra le stesse scie viste dalla stazione con l\'antenna puntata verso il corrispondente: in orizzontale l\'azimut rispetto al corrispondente (A a sinistra, B a destra), in verticale l\'elevazione. Per ogni sciame si prendono i 14 punti di riflessione più forti; ogni scia è un segmento orientato verso il radiante, da 10 km sopra a 10 km sotto la quota delle scie (al massimo 60 km per lato), più spesso quanto più è probabile. Sono disegnati anche il lobo dell\'antenna (cerchio a −3 dB attorno all\'elevazione del lobo), il corrispondente e Hot A / Hot B.'));
C.push(EQ('Q=P±', sb('t', '½'), 'ŝ,    ', sb('t', '½'), '=min', br('60 km , ', fr('10 km', ['max(0,17 , |ŝ·', sb('û', 'P'), '|)']))));
C.push(H1('6. Attenuazione e bilancio'));
C.push(TAG('FONTE', 'Equazione di McKinley [8] per le scie sottodense (β = 0, sin α = 1), guadagni d\'antenna esclusi; q densità lineare di elettroni della scia, re raggio classico dell\'elettrone = 2,818·10⁻¹⁵ m:'));
C.push(EQ('L=10 ', sb('log', '10'), sbr(fr(['16 ', sp('π', '2'), sb('R', '1'), sb('R', '2'), br(sb('R', '1'), '+', sb('R', '2')), sp(fn('cos', 'φ'), '2')], [sp('λ', '3'), sp('q', '2'), sp(sb('r', 'e'), '2')])), '  dB'));
C.push(TAG('RILIEVO', 'Con q = 10¹⁴ elettroni/m a 144 MHz risultano 181–192 dB tra 600 e 2200 km, in linea con i 182 dB minimi (GM8JBJ [9]) e i 200 dB medi (OH5IY) citati nell\'articolo [1].'));
C.push(EQ(sb('P', 'rx'), '=', sb('P', 'tx'), br('dBm'), '+', sb('G', 'tx'), '−2,15−L+', sb('G', 'rx'), '−2,15'));
C.push(EQ('margine=', sb('P', 'rx'), '−', sb('P', 'min'), ',    ', sb('P', 'min'), '=−130 dBm  [1]'));

C.push(H1('7. Antenna e lobo'));
C.push(TAG('FONTE', 'Guadagno di una Yagi lunga in funzione della lunghezza del boom L (curva DL6WU [10], valida da circa 2 λ):'));
C.push(EQ('G=7,773 ', sb('log', '10'), br(fr('L', 'λ')), '+11,43  dBi'));
C.push(TAG('RILIEVO', 'Verifica con le simulazioni NEC di W4RNL a 432 MHz [12][19][20]: DL6WU 13, 22 e 31 elementi (3,23 / 6,82 / 10,42 λ) 15,28 / 17,82 / 19,35 dBi, formula 15,38 / 17,91 / 19,34 dBi; Yagi VK3AUU di David Tanner, con boom più corto a pari elementi, 16, 28 e 40 elementi (3,12 / 6,88 / 10,79 λ) 15,17 / 17,91 / 19,46 dBi, formula 15,27 / 17,94 / 19,45 dBi. Lo scarto non supera 0,1 dB: il guadagno dipende dalla lunghezza del boom più che dal progetto. Il calcolatore di VK5DJ [21] offre anche stime per le varianti VK3AUU, WA2PHW e DJ9BV, di cui non sono pubblicate le formule.'));
C.push(TAG('FONTE', 'Apertura a −3 dB dal guadagno, regola di Kraus [11], con lobo circolare (θE = θH = θK):'));
C.push(EQ(sb('G', 'lin'), '=', fr('41253', [sb('θ', 'E'), sb('θ', 'H')]), '   ⇒   ', sb('θ', 'K'), '=', sq(fr('41253', sp('10', 'G/10')))));
C.push(TAG('RILIEVO', 'Le simulazioni NEC delle Yagi DL6WU [19] danno un lobo orizzontale più stretto di Kraus del 5–10 %: 33,8° / 26,4° / 22,6° per 12, 19 e 26 elementi, contro 37,3° / 28,0° / 23,9° di Kraus. Le formule semplici ignorano i lobi laterali in avanti [12]. Sui boom lunghi il lobo verticale è di poco più largo dell\'orizzontale (14 λ: 18,4° / 18,8°).'));
C.push(TAG('SCELTA', 'Lobo orizzontale (piano E, azimut con polarizzazione orizzontale) = 0,93 · θK; lobo verticale (piano H) = θK. Regole approssimate, ricavate da poche antenne DL6WU:'));
C.push(EQ(sb('θ', 'oriz'), '=0,93·', sb('θ', 'K'), ',    ', sb('θ', 'vert'), '=', sb('θ', 'K')));
C.push(TAG('RILIEVO', 'Yagi di 14 λ a 432 MHz: 20,3 dBi, lobo 18,1° orizzontale e 19,5° verticale. I modelli di W4RNL [12] danno 20,4 dBi e 18,4° / 18,8°.'));
C.push(TAG('SCELTA', 'L\'antenna si considera puntata verso il corrispondente. Un hot spot (paragrafo 4.5) è nel lobo se |Az(Hot) − Az(antenna)| ≤ θoriz/2: in quel caso è segnato in grigio invece che in rosa e la finestra principale avvisa. L\'hot spot consigliato dall\'efficienza (paragrafo 4.6) è sempre in rosso.'));

C.push(H1('8. Flusso dal radar e traslazione di frequenza'));
C.push(H2('8.1 Lettura del grafico di Collm'));
C.push(TAG('SCELTA', 'Il grafico SKiYMET di Collm [7] (720×720 pixel, colori puri) ha posizioni fisse degli assi, ma il radar adatta ogni giorno la scala verticale ai valori misurati (per esempio 0–1,2 ·10³ il 5/11/2026 e 0–1,0 ·10³ il 6/11/2026, con tacche spostate). Il programma trova le tacche lunghe dell\'asse e legge le etichette della più alta e della più bassa (0.0) confrontando ogni carattere con i modelli del carattere a matrice del grafico. Per l\'ora i-esima (0–47, ieri e oggi) la colonna centrale e il primo pixel ciano dall\'alto y danno:'));
C.push(EQ(sb('x', 'i'), '=101+', br('i+0,5'), fr('676−101', '48'), ',    N=', br([sb('y', '0'), '−y']), fr(sb('N', 'top'), [sb('y', '0'), '−', sb('y', 'top')]), '  meteore/ora'));
C.push(P('ytop e y0 sono le righe della tacca più alta e di quella dello zero, Ntop il valore letto sull\'etichetta più alta (×1000). La risoluzione è di circa 7–8 meteore/ora per pixel. Se un\'etichetta non si riesce a leggere, il programma torna alla taratura del 5/11/2026 (0–1200) e lo segnala nella linguetta Flusso radar.'));
C.push(H2('8.2 Statistica delle quote'));
C.push(TAG('SCELTA', 'Istogramma per km tra 70 e 110 km (nk meteore nel km k, quota media hk):'));
C.push(EQ(sb('h', 'moda'), '=', sb('h', [sb('k', '*')]), ',   ', sb('k', '*'), '=', sb('argmax', 'k'), ' ', sb('n', 'k'), ',    ', 'h̄=', fr(sum('k', [sb('n', 'k'), sb('h', 'k')]), sum('k', sb('n', 'k'))), ',    ', 's=', sq(fr(sum('k', [sb('n', 'k'), sp(br(sb('h', 'k'), '−h̄'), '2')]), sum('k', sb('n', 'k'))))));
C.push(H2('8.3 Traslazione a un\'altra frequenza'));
C.push(TAG('FONTE', 'Regole sperimentali di SM6FHZ [4]: il numero (e l\'intensità) delle riflessioni scala con λ³, la durata con λ². Con f0 = 36,2 MHz (Collm) e f la frequenza di arrivo:'));
C.push(EQ(sb('N', 'f'), '=', sb('N', sb('f', '0')), '·', sp(br(fr(sb('f', '0'), 'f')), 'k'), '·K,    k=3'));
C.push(EQ(sb('τ', 'f'), '=', sb('τ', sb('f', '0')), '·', sp(br(fr(sb('f', '0'), 'f')), '2')));
C.push(TAG('RILIEVO', 'La dipendenza da λ³ coincide con quella dell\'equazione di McKinley [8] (capitolo 6). Con k = 3, 1000 meteore/ora a Collm diventano circa 16 riflessioni/ora a 144 MHz. Gli osservatori RMOB del radar GRAVES a 143,050 MHz [13] contano in genere da 20 a 140 eco/ora: l\'ordine di grandezza è compatibile, e la differenza è il fattore di stazione K.'));
C.push(H2('8.4 Esponente ricavato da un file RMOB'));
C.push(TAG('SCELTA', 'Con K = 1, dalla mediana dei rapporti tra i conteggi RMOB e quelli di Collm nelle stesse ore:'));
C.push(EQ(sb('k', 'emp'), '=−', fr(['ln ', fn('mediana', br(fr(sb('N', 'RMOB'), sb('N', 'Collm'))))], ['ln ', br(fr('f', sb('f', '0')))])));
C.push(H2('8.5 Previsione delle prossime 24 ore'));
C.push(TAG('SCELTA', 'Persistenza: il valore traslato della stessa ora del giorno prima, corretto con la variazione dell\'indice della tratta (5.5). La costante 10 rappresenta il fondo sporadico e limita la correzione quando l\'indice è piccolo:'));
C.push(EQ(sb('N', 'prev'), br('t'), '=', sb('N', 'f'), br('t−24 h'), '·', fr(['I', br('t'), '+10'], ['I', br('t−24 h'), '+10'])));

C.push(H1('9. Mappa dei quadrati da ADIF'));
C.push(TAG('FONTE', 'Formato ADIF (campi CALL, GRIDSQUARE, VUCC_GRIDS, BAND, FREQ, QSO_DATE, QSL_RCVD, LOTW_QSL_RCVD). Banda dal campo BAND oppure dalla frequenza: 50–54 MHz = 6 m, 69,9–71,5 MHz = 4 m, 144–148 MHz = 2 m, 219–225 MHz = 1,25 m, 420–450 MHz = 70 cm.'));
C.push(TAG('SCELTA', 'Un quadrato (4 caratteri) è "confermato" se almeno un QSO ha QSL_RCVD = Y o LOTW_QSL_RCVD = Y. Il caricamento di un nuovo ADIF sostituisce i dati precedenti.'));

C.push(H1('10. Limiti noti'));
['Il modello considera solo le scie sottodense. Le sovradense (burst lunghi) e il fondo sporadico non entrano nell\'efficienza; il fondo sporadico è rappresentato dal flusso radar.',
 'Le antenne sono considerate isotrope nel calcolo dell\'efficienza; il lobo reale serve solo per il controllo degli Hot.',
 'Le derive del radiante e i coefficienti B del profilo di attività sono valori di lavoro.',
 'La lettura del grafico di Collm dipende dal formato attuale: se cambia, va ritarata.',
 'Le percentuali di efficienza vanno confrontate con MCG_MS, MS-Soft e con le osservazioni in banda.'].forEach(t => C.push(bullet(t)));

C.push(H1('11. Fonti'));
[
 'C. Cerrotta IC8TEM, G. Sartori IV3GTH - "METEOR-SCATTER. Ovvero come collegare l\'Europa in VHF tutto l\'anno senza tante pretese", articolo, versione 7 (fonte principale).',
 'I. Yrjölä OH5IY - MS-Soft, www.kolumbus.fi/oh5iy (sito storico), con i dati EURO.DAT, ZHRDATA e MSDATA.',
 'W. Rauh DL5MCG - MCG_MS, MS Efficiency Calculator v1.61, qsl.net/dl5mcg.',
 'SM6FHZ - "432 MHz Meteorscatter, Challenges and Techniques", 45th Nordic VUSHF, maggio 2025.',
 'International Meteor Organization - Meteor Shower Calendar 2026, www.imo.net.',
 'J. Meeus - Astronomical Algorithms, 2ª ed., Willmann-Bell, 1998.',
 'Radar meteorico di Collm, Università di Lipsia (LIM) e IAP Kühlungsborn - wetterdaten.meteo.uni-leipzig.de/radar_flux.php.',
 'D.W.R. McKinley - Meteor Science and Engineering, McGraw-Hill, 1961.',
 'GM8JBJ - Meteor scatter path loss, hamradio.engineering (da Kaiser & Closs, 1952).',
 'G. Hoch DL6WU - curva del guadagno delle Yagi lunghe, riportata da W8IO, www.w8io.com.',
 'J.D. Kraus - Antennas, McGraw-Hill.',
 'L.B. Cebik W4RNL - Long-Boom Yagi Rules of Thumb: A Comparison with Modeling Data.',
 'RMOB, Radio Meteor Observatories On Line - www.rmob.org, bollettino di giugno 2018 (osservatori GRAVES 143,050 MHz).',
 'Natural Earth - coste e confini 1:50 milioni, dominio pubblico, www.naturalearthdata.com.',
 'AMS Bulletin n. 203, "The AMS Radiometeor Project", tabella degli offset d\'antenna, ripresa da S. W8WN, qsl.net/w8wn/hscw/papers/ms_az_el.html.',
 'LZ2HV - MSHV, programma per i modi digitali MS (calcolo di Hot A / Hot B).',
 'J. DF5HC - "Meteorscatter auf UKW", conferenza DARC, gennaio 2018 (formula della migliore direzione di DH9GCD; diffusione a cono e HOT-A / HOT-B di DJ5HG, "Meteorscatter Propagation", www.vhfdx.de/mseff/MS-Geometrie.pdf; meteore sporadiche di KB0VUK).',
 'K. von der Heide DJ5HG - QTFGUI, programma per i puntamenti in aurora, FAI e meteor scatter, con manuale "QTFGUI Help" (2007) e dati meteorshowerdata.txt.',
 'L.B. Cebik W4RNL - Appreciating DL6WU Wide-Band Long-Boom Yagi Design (simulazioni NEC, 432 MHz).',
 'L.B. Cebik W4RNL - 70-CM Yagi Stacks Part 2: 10- to 40-Element VK3AUU Examples (Yagi di David Tanner VK3AUU).',
 'J. Drew VK5DJ - Yagi Calculator (stime del guadagno per le varianti VK3AUU, WA2PHW, DJ9BV).',
].forEach((t, i) => C.push(new Paragraph({ numbering: { reference: 'num', level: 0 }, spacing: { after: 60 }, children: [new TextRun(t)] })));

const doc = new Document({
  creator: 'Costantino IC8TEM', title: 'YA-MS - Manuale dei calcoli',
  styles: {
    default: { document: { run: { font: FONT, size: 22 } } },
    paragraphStyles: [
      { id: 'Title', name: 'Title', basedOn: 'Normal', next: 'Normal', run: { font: FONT, size: 44, bold: true, color: '1F3864' }, paragraph: { spacing: { before: 0, after: 160 }, keepNext: true } },
      { id: 'Heading1', name: 'Heading 1', basedOn: 'Normal', next: 'Normal', quickFormat: true, run: { font: FONT, size: 32, bold: true, color: '1F3864' }, paragraph: { spacing: { before: 360, after: 120 }, keepNext: true, outlineLevel: 0 } },
      { id: 'Heading2', name: 'Heading 2', basedOn: 'Normal', next: 'Normal', quickFormat: true, run: { font: FONT, size: 26, bold: true, color: '2F5496' }, paragraph: { spacing: { before: 240, after: 100 }, keepNext: true, outlineLevel: 1 } },
    ],
  },
  numbering: { config: [
    { reference: 'bul', levels: [{ level: 0, format: LevelFormat.BULLET, text: '•', alignment: AlignmentType.LEFT, style: { paragraph: { indent: { left: 720, hanging: 360 } } } }] },
    { reference: 'num', levels: [{ level: 0, format: LevelFormat.DECIMAL, text: '%1.', alignment: AlignmentType.LEFT, style: { paragraph: { indent: { left: 720, hanging: 400 } } } }] },
  ] },
  features: { updateFields: true },
  sections: [{
    properties: { page: { size: { width: 11906, height: 16838 }, margin: { top: 1134, bottom: 1134, left: 1134, right: 1134 } } },
    footers: { default: new Footer({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: 'YA-MS - Manuale dei calcoli - pagina ', size: 18, color: '808080' }), new TextRun({ children: [PageNumber.CURRENT], size: 18, color: '808080' })] })] }) },
    children: C,
  }],
});
Packer.toBuffer(doc).then(b => { fs.writeFileSync(process.argv[2], b); console.log('ok', eqn, 'equazioni'); });
