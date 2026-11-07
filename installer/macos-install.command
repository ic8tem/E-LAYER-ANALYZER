#!/bin/sh
# E-LAYER ANALYZER - installazione per Mac nella cartella personale ~/E-LAYER-ANALYZER (app e cartella data insieme)
SRC="$(cd "$(dirname "$0")" && pwd)"
DEST="$HOME/E-LAYER-ANALYZER"
echo "Installo E-LAYER ANALYZER in $DEST"
mkdir -p "$DEST"
rm -rf "$DEST/E-LAYER-ANALYZER.app"
cp -R "$SRC/E-LAYER-ANALYZER.app" "$DEST/"
[ -f "$SRC/LEGGIMI.txt" ] && cp "$SRC/LEGGIMI.txt" "$DEST/"
# il programma non e' firmato da Apple: tolgo il blocco di "scaricato da Internet"
xattr -dr com.apple.quarantine "$DEST/E-LAYER-ANALYZER.app" 2>/dev/null || true
echo "Fatto. Apro E-LAYER ANALYZER..."
open "$DEST/E-LAYER-ANALYZER.app"
