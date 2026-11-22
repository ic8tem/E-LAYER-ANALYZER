# genera data/ela_abaco.txt (italiano): salti in 50 MHz da 500 a 12000 km, passo 100 km
import math
R=6372.7955; H=110.0; F=50.2
def elev(half):      # Geo.ElevationDeg(hopKm/2, Hkm, 0)
    x=half/R; b=R+H; return math.atan2(b*math.cos(x)-R, b*math.sin(x))
def sec(hop):
    e=elev(hop/2); phi=math.asin(min(1,R*math.cos(e)/(R+H))); return 1/max(0.05,math.cos(phi))
def need(hop): return F/sec(hop)
HMAX=2*R*math.acos(R/(R+H))      # salto massimo geometrico a 110 km (elevazione 0°)
def f1(x): return f"{x:.1f}".replace('.',',')
def n_prog(d):
    if d<=2400: return 1
    if d<=5000: return 2
    if d<=6900: return 3
    return math.ceil(d/2300)
rows=[]
for d in range(500,12001,100):
    np_=n_prog(d)
    if d<=700:
        sch="Salto singolo corto (raro)"; geo=f"1 × {d} km: foEs ≥ {f1(need(d))} MHz"; src="[3] minimo 500-650 km; [71] pochissimi sotto 700 km; verifica IC8TEM: 400-800 km rari"
    elif d<=1200:
        sch="Salto singolo corto"; geo=f"1 × {d} km: foEs ≥ {f1(need(d))} MHz"; src="[4] salto corto fino a 1200 km; [71] picco del salto corto intorno a 950 km"
    elif d<=1400:
        sch="Salto singolo"; geo=f"1 × {d} km: foEs ≥ {f1(need(d))} MHz"; src="[3] fra il minimo (500-650 km) e l'ottimale (1500-2000 km)"
    elif d<=2000:
        sch="Salto singolo, distanza ottimale"; geo=f"1 × {d} km: foEs ≥ {f1(need(d))} MHz"; src="[3] ottimale 1500-2000 km; [71] picco 1500-1600 km; verifica IC8TEM: massimo a 1600-2000 km"
    elif d<=2300:
        sch="Salto singolo, vicino al massimo"; geo=f"1 × {d} km: foEs ≥ {f1(need(d))} MHz"; src="[3] massimo 2200-2400 km"
    elif d<=2400:
        sch="Salto singolo al limite geometrico"; geo=f"1 × {d} km: oltre il limite di {HMAX:.0f} km a 110 km di quota (nube più alta)"; src="[3] massimo 2200-2400 km; [23] legge della secante"
    elif d<=2700:
        sch="Zona di passaggio: due salti corti"; geo=f"2 × {d//2} km: foEs ≥ {f1(need(d/2))} MHz per salto"; src="[3] doppio salto da 2800 km; [71] multipli del salto corto a 2700-2800 km; verifica IC8TEM: minimo fra 2400 e 3600 km"
    elif d<=5100:
        h=d/2
        sch="Doppio salto a salti uguali"+(", distanza ottimale" if 3200<=d<=4000 else "")
        geo=f"2 × {d//2} km: foEs ≥ {f1(need(h))} MHz per salto" if h<=HMAX else f"2 × {d//2} km: oltre il limite di {HMAX:.0f} km a 110 km di quota (nubi più alte)"
        src="[76] doppio salto con i punti a un quarto e tre quarti; [3] doppio 2800-5000 km, ottimale 3200-4000 km; [4] regola del 22/11/2026: tipico fino a circa 5100 km (per esempio Kazakhstan)"
        if d<=4800: src+="; verifica IC8TEM: nube a un quarto della tratta 74% contro 44%"
    else:
        N=math.ceil((d-1900)/2200); m=(d-1900)/N
        sch=f"Catena: salto corto + {N} salt{'o centrale' if N==1 else 'i centrali'} + salto corto ({N+2} salti)"
        geo=f"950 + {N} × {m:.0f} + 950 km: foEs ≥ {f1(need(950))} MHz nei salti corti, ≥ {f1(need(m))} MHz nei centrali"
        src="[4] catene con i salti corti alle estremità; [71] salto corto intorno a 950 km"
        if d<=6900: src+="; [3] triplo salto ottimale 4800-6400 km, massimo 6900 km"
        if d>=9000: src+="; [56], [58] collegamenti oltre 11000 km"
    rows.append(f"TAB|{d};{np_};{sch};{geo};{src.replace('; ', ' · ')}")
out=[
"TITLE|Abaco dei salti dell'E-sporadico in 50 MHz",
"SUB|Da 500 a 12000 km, passo 100 km - a cura di Costantino IC8TEM - versione 0.6.3, novembre 2026. I numeri tra parentesi quadre rimandano alla linguetta FONTI.",
"H1|1. Come si legge",
"P|Per ogni distanza l'abaco dice quanti salti conta il programma (regola del paragrafo 3.1 del documento tecnico: 1 salto fino a 2400 km, 2 fino a 5000 km, 3 fino a 6900 km, poi la distanza divisa per 2300 km arrotondata per eccesso), qual è lo schema tipico dei salti secondo le fonti e l'esperienza dell'autore, la lunghezza dei salti con la foEs che ognuno chiede in 50 MHz e le fonti da cui viene l'assegnazione.",
f"LI|foEs necessaria: legge della secante [23] con la nube a 110 km di quota, foEs = 50,2 MHz / sec φ, dove φ è l'angolo d'incidenza sulla nube per quel salto. A 110 km il salto più lungo possibile è di circa {HMAX:.0f} km (elevazione 0°); salti più lunghi chiedono nubi più alte.",
"LI|Fino a 2400 km: salto singolo. I salti corti (500-1200 km) chiedono una foEs alta (21 MHz a 500 km, 11,3 MHz a 1200 km) e sono più rari; il salto più frequente è fra 1500 e 2000 km.",
"LI|Fra 2400 e 2800 km: zona di passaggio. Il salto singolo non arriva, il doppio salto chiede due salti corti con foEs alta: collegamenti rari.",
"LI|Da 2800 a circa 5100 km: doppio salto a salti uguali, come nel metodo ITU-R P.534-6 [76]. Per l'autore è il caso tipico fino a circa 5100 km, per esempio dall'Europa verso il Kazakhstan (decisione del 22/11/2026).",
"LI|Oltre circa 5100 km, verso gli altri continenti: catena con i salti corti alle estremità [4]. L'abaco mostra i salti corti alla lunghezza tipica di 950 km (picco del salto corto di DF5AI [71]) e divide il resto in salti centrali fino a 2200 km. Il numero di salti è quindi di solito più alto di quello della regola del programma, che conta salti uguali.",
"LI|Sulle tratte nord-sud che attraversano l'equatore magnetico, oltre circa 5000 km, la propagazione è di solito transequatoriale (TEP, [49] e [66]) e non E-sporadico: l'abaco vale per l'E-sporadico.",
"H1|2. Abaco",
"TAB|Distanza (km);Salti (regola 3.1);Schema tipico;Salti e foEs necessaria in 50 MHz;Fonti",
]+rows+[
"H1|3. Verifiche sulle decodifiche dell'autore",
"TAG|RILIEVO|Distanze delle stazioni sentite in 50 MHz (decodifiche JTDX di IC8TEM, maggio-agosto 2024-2026; stazioni distinte per giorno, divise per il numero di nominativi CALL3 alla stessa distanza per togliere l'effetto della geografia): massimo fra 1600 e 2000 km; minimi fra 400 e 800 km (circa due quinti del massimo) e fra 2400 e 3600 km (circa un quinto). Oltre i 3600 km i nominativi CALL3 sono troppo pochi o troppo concentrati (Nord America, Giappone) per un confronto.",
"TAG|RILIEVO|Doppio salto (aperture di 3100-4800 km, 197 con un riferimento nello stesso orario): nei 30 minuti prima dell'apertura una nube a un quarto della tratta, con la foEs che serve a un salto di metà tratta, c'era nel 74% dei casi contro il 44% degli stessi orari senza apertura; una nube forte del primo salto corto (250-600 km) nell'82% contro il 61%. La nube a tre quarti della tratta si vede di rado (9,9% contro 0,8%).",
]
open('/home/claude/e-layer-analyzer/data/ela_abaco.txt','w',encoding='utf-8').write('\n'.join(out)+'\n')
print(len(out), 'righe;', len(rows),'righe di tabella; salto massimo',round(HMAX))
