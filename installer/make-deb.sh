#!/usr/bin/env bash
# E-LAYER ANALYZER - pacchetto .deb (Debian, Ubuntu, Mint, Raspberry Pi OS).
# Il pacchetto mette il programma in /opt/e-layer-analyzer; al primo avvio (e dopo ogni aggiornamento) il lanciatore
# /usr/bin/e-layer-analyzer (deb-avvio.sh) lo copia in ~/E-LAYER-ANALYZER e lo avvia da li': la cartella data resta
# accanto al programma, scrivibile dall'utente. Con i pacchetti di sistema (.deb, .rpm, Arch, Slackware) il programma non
# si aggiorna da solo (file .pacchetto): si installa il pacchetto nuovo e il lanciatore copia la nuova versione.
# Uso: installer/make-deb.sh <rid> <versione> <cartella pubblicata> <cartella di uscita>
set -euo pipefail
RID="$1"; VER="$2"; SRC="$3"; OUT="$4"
case "$RID" in
  linux-x64)   A=amd64 ;;
  linux-arm64) A=arm64 ;;
  linux-arm)   A=armhf ;;
  *) echo "rid non gestito: $RID"; exit 1 ;;
esac
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
D="$(mktemp -d)/deb-$A"
mkdir -p $D/DEBIAN $D/opt/e-layer-analyzer $D/usr/bin $D/usr/share/applications $D/usr/share/icons/hicolor/256x256/apps $D/usr/share/doc/e-layer-analyzer
cp -R "$SRC/." $D/opt/e-layer-analyzer/
echo "$VER" > $D/opt/e-layer-analyzer/.versione
echo "deb" > $D/opt/e-layer-analyzer/.pacchetto   # pacchetto di sistema: niente aggiornamento automatico
chmod +x $D/opt/e-layer-analyzer/E-LAYER-ANALYZER
cp "$ROOT/installer/deb-avvio.sh" $D/usr/bin/e-layer-analyzer && chmod 755 $D/usr/bin/e-layer-analyzer
cp "$ROOT/src/Assets/ela-256.png" $D/usr/share/icons/hicolor/256x256/apps/e-layer-analyzer.png
cp "$ROOT/LEGGIMI.txt" $D/usr/share/doc/e-layer-analyzer/LEGGIMI.txt
printf '%s\n' "[Desktop Entry]" "Type=Application" "Name=E-LAYER ANALYZER" \
  "Comment=Propagazione dello strato E: E-sporadico, F.A.I., meteor scatter, aurora" \
  "Exec=e-layer-analyzer" "Icon=e-layer-analyzer" "Terminal=false" "Categories=HamRadio;Science;Utility;" \
  > $D/usr/share/applications/e-layer-analyzer.desktop
SIZE=$(du -sk $D/opt | cut -f1)
printf '%s\n' "Package: e-layer-analyzer" "Version: $VER" "Section: hamradio" "Priority: optional" "Architecture: $A" \
  "Installed-Size: $SIZE" "Maintainer: Costantino Cerrotta IC8TEM <ic8tem@gmail.com>" \
  "Depends: libc6, libfontconfig1, libx11-6, libice6, libsm6, libicu76 | libicu74 | libicu72 | libicu71 | libicu70 | libicu67 | libicu66 | libicu63" \
  "Homepage: https://github.com/ic8tem/E-LAYER-ANALYZER" \
  "Description: E-LAYER ANALYZER - propagazione dello strato E" \
  " E-sporadico, F.A.I., meteor scatter (YA-MS) e aurora in VHF, a cura di Costantino IC8TEM." \
  > $D/DEBIAN/control
mkdir -p "$OUT"
dpkg-deb --root-owner-group --build $D "$OUT/e-layer-analyzer_${VER}_$A.deb"
