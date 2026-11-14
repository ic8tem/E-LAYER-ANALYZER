# BENCHMARK 50 MHz

[← Kezdőlap](Home-hu)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Tesztpad a **többugrásos 50 MHz-es sporadikus E** vizsgálatához egy állomás, és opcionálisan egy második állomás ADIF-naplója alapján.

## A naplók betöltése

- **1. állomás**: *ADIF betöltése* / *ADIF eltávolítása*. A program a naplóból veszi a `STATION_CALLSIGN` és a `MY_GRIDSQUARE` értéket; az adó helye az egyes QSO-k `MY_GRIDSQUARE` mezője.
- **2. állomás** (opcionális): ugyanazok a gombok a második sorban; a *2. állomás* jelölőnégyzet megjeleníti vagy elrejti. Az 1. állomás útvonalai világoskékek, a 2. állomáséi lilák.
- A program a lokátorral rendelkező 50 MHz-es QSO-kat használja (a hiányzó vagy 4 karakteres lokátorokat a CALL3-ból veszi), többugrásos terjedésnél: 3100 km felett (a szerző szabálya; az ugrások száma az IK3XTV-táblázatot követi).
- A **transzekvatoriális** összeköttetések ki vannak zárva (a szerző szabálya): Afrika és Dél-Amerika felé, ha a felezőpont a -15° és +15° közötti mágneses inklinációs sávba esik, hacsak a naplóban nem `PROP_MODE=ES` szerepel.

## Nap és időpont

Válassza ki a **napot** (megnyitáskor a legtöbb többugrásos QSO-t tartalmazót) és az időpontot **ettől… eddig…** UTC, 15 perces lépésekben. A **foEs** jelölőnégyzet (opcionális) az útvonalakat a minimális foEs szerint színezi, és megadja a táblázatban.

## A térkép

Szabad középpontú **Mercator**-térkép: görgő = nagyítás, húzás = mozgatás.

![Útvonal](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- Egy **elmosódott sárga pötty** jelöli minden ugrás közepét, ahol az Es-felhőt feltételezzük; az egér alatti útvonalon nagyobb lesz.
- **Egér egy útvonal közelében**: a vonal és az ikonok kinagyítódnak, és megjelenik a DX állomás **6 karakteres WWL-négyzete**.
- **Bal kattintás** az adott útvonalra: egy felugró ablak megkérdezi, megjelenítse-e **a fájl összes QSO-ját** (minden napról), amelyeknél a túloldali végpont az adott ponttól **150 km-en belül** van. *Igen*: a térkép törlődik, és a narancs körön belüli QSO-kat mutatja; *Nem*: a térkép változatlan marad. A *Vissza a naphoz és időponthoz* gomb visszatér az előző kijelöléshez.

![Környék](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Részletek

A választott útvonalra: távolság és besorolás az IK3XTV-táblázat szerint, azimut mindkét irányban, egyenlő ugrások a visszaverődési pontok négyzeteivel, 1200 km-en belüli rövid első ugrás (IC8TEM tapasztalata), és a foEs jelölőnégyzettel a minimális foEs.
