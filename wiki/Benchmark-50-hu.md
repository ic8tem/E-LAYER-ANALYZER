# BENCHMARK 50 MHz

[← Home](Home-hu)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Próbapad a **többugrásos sporadikus E-hez 50 MHz-en**, egy állomás és opcionálisan egy második állomás ADIF naplója alapján.

## A naplók betöltése

- **1. állomás**: *ADIF betöltése* / *ADIF eltávolítása*. A program a naplóból veszi a `STATION_CALLSIGN` és `MY_GRIDSQUARE` mezőket; az adási hely minden QSO `MY_GRIDSQUARE` mezője.
- **2. állomás** (opcionális): ugyanazok a gombok a második sorban; a *2. állomás* jelölőnégyzet megjeleníti vagy elrejti. Az 1. állomás útvonalai világoskékek, a 2. állomáséi lilák.
- A program a lokátorral rendelkező 50 MHz-es QSO-kat használja (a hiányzó vagy 4 karakteres lokátorokat a CALL3-ból veszi), többugrásosan: 3100 km felett (a szerző szabálya; az ugrások száma az IK3XTV táblázatot követi).
- A **transzekvatoriális** összeköttetések ki vannak zárva (a szerző szabálya): Afrika és Dél-Amerika felé, ha a felezőpont a -20° és +20° közötti mágneses inklinációs sávba esik, kivéve, ha a napló `PROP_MODE=ES` értéket tartalmaz.

## Nap és időpont

Válassza ki a **napot** (megnyitáskor a legtöbb többugrásos QSO-t tartalmazó napot) és az időpontot **…-tól …-ig** UTC, 15 perces lépésekben. Az **foEs** jelölőnégyzet (opcionális) a minimális foEs szerint színezi az útvonalakat, és megjeleníti a táblázatban.

## A térkép

**Mercator**-térkép szabad középponttal: görgő = nagyítás, húzás = mozgatás.

![Path](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Egér egy útvonal közelében**: a vonal és az ikonok felnagyítódnak, és megjelenik a DX állomás **6 karakteres WWL négyzete**.
- **Bal kattintás** az útvonalon: egy ablak megkérdezi, megjelenítse-e **a fájl összes QSO-ját** (minden nap), amelyek távoli végpontja **150 km-en belül** van ettől a ponttól. *Igen*: a térkép törlődik, és a narancs körön belül mutatja ezeket a QSO-kat; *Nem*: a térkép változatlan marad. A *Vissza a naphoz és időponthoz* visszaállítja az előző kijelölést.

![Surroundings](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Részletek

A választott útvonalra: távolság és az IK3XTV táblázat szerinti minősítés, azimut mindkét irányban, egyenlő ugrások a visszaverődési pontok négyzeteivel, rövid első ugrás 1200 km-en belül (IC8TEM tapasztalata), valamint bejelölt foEs esetén a minimális foEs.
