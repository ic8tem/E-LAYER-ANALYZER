#!/usr/bin/env bash
# E-LAYER ANALYZER - AppImage (un solo file, gira su quasi tutte le distribuzioni Linux con glibc).
# L'AppImage e' in sola lettura: come il lanciatore del .deb, al primo avvio (e quando l'AppImage contiene una versione
# piu' recente) copia il programma in ~/E-LAYER-ANALYZER e lo avvia da li'. La copia si aggiorna da sola dalle Release;
# un'AppImage piu' vecchia della copia non la riporta indietro.
# Uso: installer/make-appimage.sh <rid> <versione> <cartella pubblicata> <cartella di uscita>
set -euo pipefail
RID="$1"; VERSION="$2"; SRC="$3"; OUT="$4"
case "$RID" in
  linux-x64)   ARCH=x86_64 ;;
  linux-arm64) ARCH=aarch64 ;;
  *) echo "rid non gestito: $RID"; exit 1 ;;
esac
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
A="$(mktemp -d)/E-LAYER-ANALYZER.AppDir"
mkdir -p "$A/opt/e-layer-analyzer" "$A/usr/share/icons/hicolor/256x256/apps"
cp -R "$SRC/." "$A/opt/e-layer-analyzer/"
chmod 755 "$A/opt/e-layer-analyzer/E-LAYER-ANALYZER"
echo "$VERSION" > "$A/opt/e-layer-analyzer/.versione"
rm -f "$A/opt/e-layer-analyzer/.pacchetto"   # l'AppImage si aggiorna da sola (copia in ~/E-LAYER-ANALYZER)
cp "$ROOT/src/Assets/ela-256.png" "$A/e-layer-analyzer.png"
cp "$ROOT/src/Assets/ela-256.png" "$A/usr/share/icons/hicolor/256x256/apps/e-layer-analyzer.png"
cat > "$A/e-layer-analyzer.desktop" <<'DESK'
[Desktop Entry]
Type=Application
Name=E-LAYER ANALYZER
Comment=Propagazione dello strato E: E-sporadico, F.A.I., meteor scatter, aurora
Exec=AppRun
Icon=e-layer-analyzer
Terminal=false
Categories=HamRadio;Science;Utility;
DESK
# AppRun = lanciatore del .deb con la sorgente dentro l'AppImage
sed 's#^SRC=/opt/e-layer-analyzer#SRC="$(dirname "$(readlink -f "$0")")/opt/e-layer-analyzer"#' "$ROOT/installer/deb-avvio.sh" > "$A/AppRun"
grep -q 'readlink' "$A/AppRun"
chmod 755 "$A/AppRun"
T="$(mktemp -d)"
curl -fsSL -o "$T/appimagetool" "https://github.com/AppImage/appimagetool/releases/download/continuous/appimagetool-$(uname -m).AppImage"
chmod +x "$T/appimagetool"
mkdir -p "$OUT"
ARCH=$ARCH "$T/appimagetool" --appimage-extract-and-run "$A" "$OUT/E-LAYER-ANALYZER-${VERSION}-${ARCH}.AppImage"
ls -la "$OUT/E-LAYER-ANALYZER-${VERSION}-${ARCH}.AppImage"
