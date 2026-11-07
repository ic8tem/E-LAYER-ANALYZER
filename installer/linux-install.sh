#!/bin/sh
# E-LAYER ANALYZER - installazione per Linux nella cartella personale ~/E-LAYER-ANALYZER (programma e cartella data insieme)
set -e
SRC="$(cd "$(dirname "$0")" && pwd)"
DEST="$HOME/E-LAYER-ANALYZER"
echo "Installo E-LAYER ANALYZER in $DEST"
mkdir -p "$DEST/data"
# il file call3.txt dell'utente non si sovrascrive
[ -f "$DEST/data/call3.txt" ] && cp "$DEST/data/call3.txt" "$DEST/data/call3.txt.keep"
cp -R "$SRC/." "$DEST/"
[ -f "$DEST/data/call3.txt.keep" ] && mv "$DEST/data/call3.txt.keep" "$DEST/data/call3.txt"
rm -f "$DEST/installa.sh"
chmod +x "$DEST/E-LAYER-ANALYZER"
mkdir -p "$HOME/.local/share/applications" "$HOME/.local/share/icons/hicolor/256x256/apps"
cp "$DEST/ela.png" "$HOME/.local/share/icons/hicolor/256x256/apps/e-layer-analyzer.png"
cat > "$HOME/.local/share/applications/e-layer-analyzer.desktop" <<DESK
[Desktop Entry]
Type=Application
Name=E-LAYER ANALYZER
Comment=Propagazione dello strato E: sporadico E, F.A.I., meteor scatter, aurora
Exec="$DEST/E-LAYER-ANALYZER"
Path=$DEST
Icon=e-layer-analyzer
Terminal=false
Categories=HamRadio;Science;Utility;
DESK
command -v update-desktop-database >/dev/null 2>&1 && update-desktop-database "$HOME/.local/share/applications" || true
echo "Fatto. Avvia E-LAYER ANALYZER dal menu delle applicazioni oppure con: $DEST/E-LAYER-ANALYZER"
