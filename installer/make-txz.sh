#!/usr/bin/env bash
# E-LAYER ANALYZER - pacchetto Slackware (.txz), ricavato dal .deb dello stesso rid: stessi file del .deb
# (/opt/e-layer-analyzer, lanciatore /usr/bin/e-layer-analyzer che al primo avvio copia il programma in ~/E-LAYER-ANALYZER).
# Si installa da root con: installpkg e-layer-analyzer-<versione>-<arch>-1_ic8tem.txz
# Uso: installer/make-txz.sh <rid> <versione> <file .deb> <cartella di uscita>
set -euo pipefail
RID="$1"; VERSION="$2"; DEB="$3"; OUT="$4"
case "$RID" in
  linux-x64)   ARCH=x86_64 ;;
  linux-arm64) ARCH=aarch64 ;;
  linux-arm)   ARCH=arm ;;
  *) echo "rid non gestito: $RID"; exit 1 ;;
esac
T="$(mktemp -d)"
dpkg-deb -x "$DEB" "$T"
echo "Slackware" > "$T/opt/e-layer-analyzer/.pacchetto"
mkdir -p "$T/install"
# slack-desc: 11 righe che iniziano con il nome del pacchetto
cat > "$T/install/slack-desc" <<'DESC'
                |-----handy-ruler------------------------------------------------------|
e-layer-analyzer: e-layer-analyzer (propagazione dello strato E)
e-layer-analyzer:
e-layer-analyzer: E-sporadico, F.A.I., meteor scatter (YA-MS) e aurora in VHF.
e-layer-analyzer: A cura di Costantino Cerrotta IC8TEM.
e-layer-analyzer: Al primo avvio il programma si copia in ~/E-LAYER-ANALYZER,
e-layer-analyzer: con la cartella data accanto.
e-layer-analyzer:
e-layer-analyzer: https://github.com/ic8tem/E-LAYER-ANALYZER
e-layer-analyzer:
e-layer-analyzer:
e-layer-analyzer:
DESC
cat > "$T/install/doinst.sh" <<'DOIN'
if [ -x /usr/bin/update-desktop-database ]; then /usr/bin/update-desktop-database -q usr/share/applications >/dev/null 2>&1; fi
if [ -x /usr/bin/gtk-update-icon-cache ]; then /usr/bin/gtk-update-icon-cache -q usr/share/icons/hicolor >/dev/null 2>&1; fi
DOIN
SV="${VERSION//-/_}"
mkdir -p "$OUT"
tar -C "$T" --owner=0 --group=0 -cJf "$OUT/e-layer-analyzer-${SV}-${ARCH}-1_ic8tem.txz" install opt usr
ls -la "$OUT/e-layer-analyzer-${SV}-${ARCH}-1_ic8tem.txz"
