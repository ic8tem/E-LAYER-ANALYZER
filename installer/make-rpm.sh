#!/usr/bin/env bash
# E-LAYER ANALYZER - pacchetto .rpm (Fedora, openSUSE, RHEL e derivate), ricavato dal .deb dello stesso rid:
# stessi file (/opt/e-layer-analyzer, lanciatore /usr/bin/e-layer-analyzer che al primo avvio copia il programma in
# ~/E-LAYER-ANALYZER, icona e voce di menu). Serve il comando rpmbuild (pacchetto "rpm" su Ubuntu).
# Uso: installer/make-rpm.sh <rid> <versione> <file .deb> <cartella di uscita>
set -euo pipefail
RID="$1"; VERSION="$2"; DEB="$3"; OUT="$4"
case "$RID" in
  linux-x64)   ARCH=x86_64 ;;
  linux-arm64) ARCH=aarch64 ;;
  linux-arm)   ARCH=armv7hl ;;
  *) echo "rid non gestito: $RID"; exit 1 ;;
esac
TOP="$(mktemp -d)"
TREE="$TOP/albero"
mkdir -p "$TREE" "$TOP/SPECS" "$TOP/RPMS"
dpkg-deb -x "$DEB" "$TREE"
echo "rpm" > "$TREE/opt/e-layer-analyzer/.pacchetto"
RV="${VERSION//-/_}"
cat > "$TOP/SPECS/e-layer-analyzer.spec" <<SPEC
Name:           e-layer-analyzer
Version:        ${RV}
Release:        1
Summary:        Propagazione dello strato E: E-sporadico, F.A.I., meteor scatter, aurora
License:        Freeware
URL:            https://github.com/ic8tem/E-LAYER-ANALYZER
Packager:       Costantino Cerrotta IC8TEM
AutoReqProv:    no
Requires:       fontconfig, libX11, libICE, libSM
%global debug_package %{nil}
%global __strip /bin/true
%global __os_install_post %{nil}
%global _build_id_links none

%description
Analisi dei modi di propagazione dello strato E in VHF: E-sporadico,
F.A.I., meteor scatter (YA-MS) e aurora. Al primo avvio il programma
si copia in ~/E-LAYER-ANALYZER con la sua cartella data.
A cura di Costantino Cerrotta IC8TEM.

%install
mkdir -p %{buildroot}
cp -a ${TREE}/. %{buildroot}/

%post
command -v update-desktop-database >/dev/null 2>&1 && update-desktop-database -q || true
command -v gtk-update-icon-cache >/dev/null 2>&1 && gtk-update-icon-cache -q /usr/share/icons/hicolor || true
exit 0

%files
/opt/e-layer-analyzer
/usr/bin/e-layer-analyzer
/usr/share/applications/e-layer-analyzer.desktop
/usr/share/icons/hicolor/256x256/apps/e-layer-analyzer.png
%doc /usr/share/doc/e-layer-analyzer
SPEC
rpmbuild --define "_topdir $TOP" --target "$ARCH" -bb "$TOP/SPECS/e-layer-analyzer.spec" >/dev/null
mkdir -p "$OUT"
cp "$TOP"/RPMS/*/e-layer-analyzer-*.rpm "$OUT/e-layer-analyzer-${VERSION}.${ARCH}.rpm"
rpm -qpi "$OUT/e-layer-analyzer-${VERSION}.${ARCH}.rpm" | head -8 || true
