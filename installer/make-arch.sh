#!/usr/bin/env bash
# E-LAYER ANALYZER - pacchetto Arch Linux / Manjaro / EndeavourOS / Arch Linux ARM (.pkg.tar.zst), ricavato dal .deb
# dello stesso rid (stessi file). Si costruisce con makepkg dentro un contenitore Docker archlinux
# (si impacchettano file gia' compilati, quindi l'architettura del contenitore non conta).
# Si installa con: sudo pacman -U e-layer-analyzer-<versione>-1-<arch>.pkg.tar.zst
# Uso: installer/make-arch.sh <rid> <versione> <file .deb> <cartella di uscita>
set -euo pipefail
RID="$1"; VERSION="$2"; DEB="$3"; OUT="$4"
case "$RID" in
  linux-x64)   ARCH=x86_64 ;;
  linux-arm64) ARCH=aarch64 ;;
  *) echo "rid non gestito: $RID"; exit 1 ;;
esac
W="$(mktemp -d)"
mkdir -p "$W/albero"
dpkg-deb -x "$DEB" "$W/albero"
echo "Arch" > "$W/albero/opt/e-layer-analyzer/.pacchetto"
PV="${VERSION//-/_}"
cat > "$W/PKGBUILD" <<PKG
pkgname=e-layer-analyzer
pkgver=${PV}
pkgrel=1
pkgdesc="Propagazione dello strato E: E-sporadico, F.A.I., meteor scatter, aurora (a cura di Costantino Cerrotta IC8TEM)"
arch=('${ARCH}')
url="https://github.com/ic8tem/E-LAYER-ANALYZER"
license=('custom:freeware')
depends=('fontconfig' 'libx11' 'libice' 'libsm' 'icu')
options=('!strip' '!debug')
package() {
  cp -a "\$startdir/albero/." "\$pkgdir/"
}
PKG
# il contenitore archlinux esiste solo per x86_64: su un runner ARM gira emulato con qemu
PLAT=linux/amd64
if [ "$(uname -m)" != "x86_64" ]; then docker run --privileged --rm tonistiigi/binfmt --install amd64 >/dev/null; fi
docker run --rm --platform $PLAT -v "$W:/w" archlinux:latest bash -c "
  set -e
  pacman --disable-sandbox -Sy --noconfirm --needed base-devel >/dev/null
  sed -i 's/^CARCH=.*/CARCH=\"${ARCH}\"/' /etc/makepkg.conf
  useradd -m b && chown -R b /w
  su b -c 'cd /w && makepkg --nodeps --skipinteg -f'
  chmod -R a+rwX /w
"
mkdir -p "$OUT"
cp "$W"/e-layer-analyzer-*.pkg.tar.zst "$OUT/"
ls -la "$OUT"/e-layer-analyzer-*.pkg.tar.zst
