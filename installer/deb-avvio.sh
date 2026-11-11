#!/bin/sh
# E-LAYER ANALYZER - avvio dal pacchetto .deb.
# Il pacchetto mette il programma in /opt/e-layer-analyzer (sola lettura). La cartella data deve stare accanto al
# programma ed essere scrivibile, quindi al primo avvio e dopo ogni aggiornamento il programma si copia in
# ~/E-LAYER-ANALYZER, come fa installa.sh dei pacchetti portatili; il file call3.txt dell'utente non si sovrascrive.
# Il programma puo' anche aggiornarsi da solo dalle Release di GitHub (scrive la nuova versione in ~/E-LAYER-ANALYZER/.versione).
SRC=/opt/e-layer-analyzer
DEST="$HOME/E-LAYER-ANALYZER"
# si copia se manca o se il pacchetto e' piu' recente; se il programma si e' gia' aggiornato da solo (versione in ~ piu'
# recente di quella del pacchetto) non si torna indietro
SV=$(cat "$SRC/.versione" 2>/dev/null); DV=$(cat "$DEST/.versione" 2>/dev/null)
NEWEST=$(printf '%s\n%s\n' "$SV" "$DV" | sort -V | tail -n 1)
if [ ! -x "$DEST/E-LAYER-ANALYZER" ] || { [ "$SV" != "$DV" ] && [ "$NEWEST" = "$SV" ]; }; then
  mkdir -p "$DEST/data"
  [ -f "$DEST/data/call3.txt" ] && cp "$DEST/data/call3.txt" "$DEST/data/call3.txt.keep"
  cp -R "$SRC/." "$DEST/"
  [ -f "$DEST/data/call3.txt.keep" ] && mv "$DEST/data/call3.txt.keep" "$DEST/data/call3.txt"
  chmod +x "$DEST/E-LAYER-ANALYZER"
fi
cd "$DEST" && exec "$DEST/E-LAYER-ANALYZER" "$@"
