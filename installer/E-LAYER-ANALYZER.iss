; E-LAYER ANALYZER - installatore per Windows (Inno Setup 6)
; Installa nella cartella personale C:\E-LAYER-ANALYZER (programma e cartella data insieme), senza permessi di amministratore.
; Nessun file in C:\Users\...\AppData: niente collegamento nel menu Start (che Windows mette in AppData\Roaming),
; solo l'icona facoltativa sul desktop. Versioni a 64 bit (x64) e 32 bit (x86).
#ifndef AppVersion
  #define AppVersion "0.4.0"
#endif
#ifndef Arch
  #define Arch "x64"
#endif
#ifndef SourceDir
  #define SourceDir "..\publish\win-x64"
#endif

[Setup]
AppId={{3E8A51C2-7D44-4B9E-A1C6-2F5D9B7E0A13}
AppName=E-LAYER ANALYZER
AppVersion={#AppVersion}
AppVerName=E-LAYER ANALYZER {#AppVersion}
AppPublisher=Costantino Cerrotta IC8TEM
AppComments=Analisi dei modi di propagazione dello strato E: sporadico E, F.A.I., meteor scatter, aurora
DefaultDirName={sd}\E-LAYER-ANALYZER
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
#if Arch == "x64"
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
#endif
OutputDir=..\dist
OutputBaseFilename=E-LAYER-ANALYZER-{#AppVersion}-windows-{#Arch}-setup
SetupIconFile=..\src\Assets\ela.ico
UninstallDisplayIcon={app}\E-LAYER-ANALYZER.exe
Compression=lzma2
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"

[Tasks]
Name: "desktopicon"; Description: "Crea un'icona sul desktop"; GroupDescription: "Icone:"; Flags: checkedonce

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Excludes: "data\*,doc\*"; Flags: ignoreversion recursesubdirs createallsubdirs
; dati di riferimento: aggiornati a ogni installazione (call3.txt solo se manca: l'utente lo tiene aggiornato)
Source: "{#SourceDir}\data\*"; DestDir: "{app}\data"; Excludes: "call3.txt"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#SourceDir}\data\call3.txt"; DestDir: "{app}\data"; Flags: onlyifdoesntexist
Source: "{#SourceDir}\doc\*"; DestDir: "{app}\doc"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{userdesktop}\E-LAYER ANALYZER"; Filename: "{app}\E-LAYER-ANALYZER.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\E-LAYER-ANALYZER.exe"; Description: "Avvia E-LAYER ANALYZER"; Flags: nowait postinstall skipifsilent
