#define MyAppName "ID Card Reader Thales"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Newlogic"
#define MyAppURL "http://www.newlogic.com/"
#define MyAppExeName "setup-id-card-reader-thales"
#include ".\envpath.iss"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{6880BC32-C400-4663-AE06-A6A6B9C093F5}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputBaseFilename={#MyAppExeName}-{#MyAppVersion}
Compression=lzma
SolidCompression=yes
PrivilegesRequired = admin
ArchitecturesInstallIn64BitMode=x64
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoTextVersion={#MyAppVersion}
VersionInfoProductVersion={#MyAppVersion}
VersionInfoDescription={#MyAppName}
ChangesEnvironment=yes

[Code]
function InitializeSetup(): Boolean;
label return;
begin
  if not IsWin64 then
  begin
    MsgBox('Please run the installer in Windows 64bit', mbCriticalError, MB_OK);
    Result := False;
    goto return;
  end;
  Result := True;
  return:
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
    if CurStep = ssInstall
     then EnvAddPath(ExpandConstant('{app}') +'\SDK\Bin');
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
    if CurUninstallStep = usPostUninstall
    then EnvRemovePath(ExpandConstant('{app}') +'\SDK\Bin');
end;

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "publish\net6.0\win10-x64\appsettings.Development.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\net6.0\win10-x64\appsettings.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\net6.0\win10-x64\aspnetcorev2_inprocess.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\net6.0\win10-x64\ID Card Reader Thales.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "SDK\Bin\*"; DestDir: "{app}\SDK\Bin"; Flags: ignoreversion
Source: "SDK\Config\*"; DestDir: "{app}\SDK\Config"; Flags: ignoreversion
Source: "SDK\Plugins\*"; DestDir: "{app}\SDK\Plugins"; Flags: ignoreversion

[Run]
Filename: "{sys}\sc.exe"; Parameters: "create ""{#MyAppName}"" binpath= ""{app}\ID Card Reader Thales.exe --urls http://localhost:12212"" start= auto"; Flags: runhidden;
Filename: "{sys}\sc.exe"; Parameters: "start ""{#MyAppName}"""; Flags: runhidden;

[UninstallRun]
Filename: "{sys}\sc.exe"; Parameters: "stop ""{#MyAppName}"""; Flags: runhidden;
Filename: "{sys}\sc.exe"; Parameters: "delete ""{#MyAppName}"""; Flags: runhidden; RunOnceId: "Uninstall {#MyAppName}";