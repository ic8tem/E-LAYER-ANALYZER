#!/bin/sh
# E-LAYER ANALYZER - avvio dal pacchetto .deb.
# Il pacchetto mette il programma in /opt/e-layer-analyzer (sola lettura). La cartella data deve stare accanto al
# programma ed essere scrivibile, quindi al primo avvio e dopo ogni aggiornamento il programma si copia in
# ~/E-LAYER-ANALYZER, come fa installa.sh dei pacchetti portatili; il file call3.txt dell'utente non si sovrascrive.
SRC=/opt/e-layer-analyzer
DEST="$HOME/E-LAYER-ANALYZER"
if ! cmp -s "$SRC/.versione" "$DEST/.versione" 2>/dev/null; then
  mkdir -p "$DEST/data"
  [ -f "$DEST/data/call3.txt" ] && cp "$DEST/data/call3.txt" "$DEST/data/call3.txt.keep"
  cp -R "$SRC/." "$DEST/"
  [ -f "$DEST/data/call3.txt.keep" ] && mv "$DEST/data/call3.txt.keep" "$DEST/data/call3.txt"
  chmod +x "$DEST/E-LAYER-ANALYZER"
fi
cd "$DEST" && exec "$DEST/E-LAYER-ANALYZER" "$@"
