# genera data/ela_abaco[_<lingua>].txt: salti in 50 MHz da 500 a 12000 km, passo 100 km (frasi per lingua in tr/<lingua>.py)
import math,sys,importlib.util,os
R=6372.7955; H=110.0; F=50.2; VER='0.6.3'
def elev(half):
    x=half/R; b=R+H; return math.atan2(b*math.cos(x)-R, b*math.sin(x))
def sec(hop):
    e=elev(hop/2); phi=math.asin(min(1,R*math.cos(e)/(R+H))); return 1/max(0.05,math.cos(phi))
def need(hop): return F/sec(hop)
HMAX=2*R*math.acos(R/(R+H))
def n_prog(d):
    if d<=2400: return 1
    if d<=5000: return 2
    if d<=6900: return 3
    return math.ceil(d/2300)
IT=dict(
dec=',',
title="Abaco dei salti dell'E-sporadico in 50 MHz",
sub="Da 500 a 12000 km, passo 100 km - a cura di Costantino IC8TEM - versione {ver}, novembre 2026. I numeri tra parentesi quadre rimandano alla linguetta FONTI.",
h1a="1. Come si legge", h1b="2. Abaco", h1c="3. Verifiche sulle decodifiche dell'autore",
intro="Per ogni distanza l'abaco dice quanti salti conta il programma (regola del paragrafo 3.1 del documento tecnico: 1 salto fino a 2400 km, 2 fino a 5000 km, 3 fino a 6900 km, poi la distanza divisa per 2300 km arrotondata per eccesso), qual è lo schema tipico dei salti secondo le fonti e l'esperienza dell'autore, la lunghezza dei salti con la foEs che ognuno chiede in 50 MHz e le fonti da cui viene l'assegnazione.",
li1="foEs necessaria: legge della secante [23] con la nube a 110 km di quota, foEs = 50,2 MHz / sec φ, dove φ è l'angolo d'incidenza sulla nube per quel salto. A 110 km il salto più lungo possibile è di circa {hm} km (elevazione 0°); salti più lunghi chiedono nubi più alte.",
li2="Fino a 2400 km: salto singolo. I salti corti (500-1200 km) chiedono una foEs alta (21 MHz a 500 km, 11,3 MHz a 1200 km) e sono più rari; il salto più frequente è fra 1500 e 2000 km.",
li3="Fra 2400 e 2800 km: zona di passaggio. Il salto singolo non arriva, il doppio salto chiede due salti corti con foEs alta: collegamenti rari.",
li4="Da 2800 a circa 4700 km: doppio salto a salti uguali, come nel metodo ITU-R P.534-6 [76]. Per l'autore è il caso tipico, per esempio dall'Europa verso il Kazakhstan (22/11/2026); il limite di 4700 km è quello di due salti da 2350 km, il salto più lungo con la nube a 110 km di quota.",
li5="Oltre circa 4700 km, verso gli altri continenti: catena con i salti corti alle estremità [4]. L'abaco mostra i salti corti alla lunghezza tipica di 950 km (picco del salto corto di DF5AI [71]) e divide il resto in salti centrali fino a 2200 km. Il numero di salti è quindi di solito più alto di quello della regola del programma, che conta salti uguali.",
li6="Sulle tratte nord-sud che attraversano l'equatore magnetico, oltre circa 5000 km, la propagazione è di solito transequatoriale (TEP, [49] e [66]) e non E-sporadico: l'abaco vale per l'E-sporadico.",
hdr="Distanza (km);Salti (regola 3.1);Schema tipico;Salti e foEs necessaria in 50 MHz;Fonti",
sch_short_rare="Salto singolo corto (raro)", sch_short="Salto singolo corto", sch_single="Salto singolo", sch_single_opt="Salto singolo, distanza ottimale",
sch_single_max="Salto singolo, vicino al massimo", sch_single_lim="Salto singolo al limite geometrico", sch_pass="Zona di passaggio: due salti corti",
sch_eq="Doppio salto a salti uguali", sch_eq_opt="Doppio salto a salti uguali, distanza ottimale",
sch_chain1="Catena: salto corto + {n} salto centrale + salto corto ({t} salti)", sch_chain="Catena: salto corto + {n} salti centrali + salto corto ({t} salti)",
g1="1 × {d} km: foEs ≥ {f} MHz", g2="2 × {h} km: foEs ≥ {f} MHz per salto",
glim1="1 × {d} km: oltre il limite di {hm} km a 110 km di quota (nube più alta)", glim2="2 × {h} km: oltre il limite di {hm} km a 110 km di quota (nubi più alte)",
gch="950 + {n} × {m} + 950 km: foEs ≥ {a} MHz nei salti corti, ≥ {b} MHz nei centrali",
s3min="[3] minimo 500-650 km", s71few="[71] pochissimi sotto 700 km", vrare="verifica IC8TEM: 400-800 km rari",
s4short="[4] salto corto fino a 1200 km", s71peak="[71] picco del salto corto intorno a 950 km",
s3between="[3] fra il minimo (500-650 km) e l'ottimale (1500-2000 km)", s3opt="[3] ottimale 1500-2000 km", s71long="[71] picco 1500-1600 km",
vmax="verifica IC8TEM: massimo a 1600-2000 km", s3max="[3] massimo 2200-2400 km", s23="[23] legge della secante",
s3dbl="[3] doppio salto da 2800 km", s71mult="[71] multipli del salto corto a 2700-2800 km", vmin="verifica IC8TEM: minimo fra 2400 e 3600 km",
s76="[76] doppio salto con i punti a un quarto e tre quarti", s3dblr="[3] doppio 2800-5000 km, ottimale 3200-4000 km",
s4rule="[4] regola del 22/11/2026: tipico fino a circa 4700 km (per esempio Kazakhstan)", veq="verifica IC8TEM: nube a un quarto della tratta 74% contro 44%",
s4chain="[4] catene con i salti corti alle estremità", s71short="[71] salto corto intorno a 950 km",
s3tri="[3] triplo salto ottimale 4800-6400 km, massimo 6900 km", s56="[56], [58] collegamenti oltre 11000 km",
ril1="Distanze delle stazioni sentite in 50 MHz (decodifiche JTDX di IC8TEM, maggio-agosto 2024-2026; stazioni distinte per giorno, divise per il numero di nominativi CALL3 alla stessa distanza per togliere l'effetto della geografia): massimo fra 1600 e 2000 km; minimi fra 400 e 800 km (circa due quinti del massimo) e fra 2400 e 3600 km (circa un quinto). Oltre i 3600 km i nominativi CALL3 sono troppo pochi o troppo concentrati (Nord America, Giappone) per un confronto.",
ril2="Doppio salto (aperture di 3100-4800 km, 197 con un riferimento nello stesso orario): nei 30 minuti prima dell'apertura una nube a un quarto della tratta, con la foEs che serve a un salto di metà tratta, c'era nel 74% dei casi contro il 44% degli stessi orari senza apertura; una nube forte del primo salto corto (250-600 km) nell'82% contro il 61%. La nube a tre quarti della tratta si vede di rado (9,9% contro 0,8%).",
)
def build(T):
    f1=lambda x: f"{x:.1f}".replace('.',T['dec'])
    J=lambda *a: ' · '.join(a)
    rows=[]
    for d in range(500,12001,100):
        np_=n_prog(d)
        if d<=700: sch=T['sch_short_rare']; geo=T['g1'].format(d=d,f=f1(need(d))); src=J(T['s3min'],T['s71few'],T['vrare'])
        elif d<=1200: sch=T['sch_short']; geo=T['g1'].format(d=d,f=f1(need(d))); src=J(T['s4short'],T['s71peak'])
        elif d<=1400: sch=T['sch_single']; geo=T['g1'].format(d=d,f=f1(need(d))); src=T['s3between']
        elif d<=2000: sch=T['sch_single_opt']; geo=T['g1'].format(d=d,f=f1(need(d))); src=J(T['s3opt'],T['s71long'],T['vmax'])
        elif d<=2300: sch=T['sch_single_max']; geo=T['g1'].format(d=d,f=f1(need(d))); src=T['s3max']
        elif d<=2400: sch=T['sch_single_lim']; geo=T['glim1'].format(d=d,hm=f"{HMAX:.0f}"); src=J(T['s3max'],T['s23'])
        elif d<=2700: sch=T['sch_pass']; geo=T['g2'].format(h=d//2,f=f1(need(d/2))); src=J(T['s3dbl'],T['s71mult'],T['vmin'])
        elif d<=4700:
            sch=T['sch_eq_opt'] if 3200<=d<=4000 else T['sch_eq']
            geo=T['g2'].format(h=d//2,f=f1(need(d/2))) if d/2<=HMAX else T['glim2'].format(h=d//2,hm=f"{HMAX:.0f}")
            src=J(T['s76'],T['s3dblr'],T['s4rule'],T['veq'])
        else:
            N=math.ceil((d-1900)/2200); m=(d-1900)/N
            sch=(T['sch_chain1'] if N==1 else T['sch_chain']).format(n=N,t=N+2)
            geo=T['gch'].format(n=N,m=f"{m:.0f}",a=f1(need(950)),b=f1(need(m)))
            parts=[T['s4chain'],T['s71short']]
            if d<=6900: parts.append(T['s3tri'])
            if d>=9000: parts.append(T['s56'])
            src=J(*parts)
        for x in (sch,geo,src): assert ';' not in x
        rows.append(f"TAB|{d};{np_};{sch};{geo};{src}")
    return ['TITLE|'+T['title'],'SUB|'+T['sub'].format(ver=VER),'H1|'+T['h1a'],'P|'+T['intro'],'LI|'+T['li1'].format(hm=f"{HMAX:.0f}"),
            'LI|'+T['li2'],'LI|'+T['li3'],'LI|'+T['li4'],'LI|'+T['li5'],'LI|'+T['li6'],'H1|'+T['h1b'],'TAB|'+T['hdr']]+rows+['H1|'+T['h1c'],'TAG|RILIEVO|'+T['ril1'],'TAG|RILIEVO|'+T['ril2']]
D='/home/claude/e-layer-analyzer/data/'
langs=sys.argv[1:] or ['it']
for c in langs:
    if c=='it': T=IT
    else:
        sp=importlib.util.spec_from_file_location('t',os.path.join(os.path.dirname(__file__),'tr',c.replace('-','_')+'.py')); m=importlib.util.module_from_spec(sp); sp.loader.exec_module(m); T=dict(IT); T.update(m.T)
        miss=[k for k in IT if k not in m.T]; assert not miss,(c,miss)
    out='\n'.join(build(T))+'\n'
    fn=D+('ela_abaco.txt' if c=='it' else 'ela_abaco_%s.txt'%c)
    open(fn,'w',encoding='utf-8').write(out); print(c,'ok')
