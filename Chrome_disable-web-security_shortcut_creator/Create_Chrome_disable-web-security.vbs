Option Explicit

Dim objShell
Dim appData
Dim googlePath
Dim targetPath
Dim objFS
Dim desktop
Dim shortcut
Dim commitPath

' GooglePath
googlePath = "C:\Program Files\Google\Chrome\Application\chrome.exe"

Set objShell = CreateObject("WScript.Shell")
appData = objShell.ExpandEnvironmentStrings("%LOCALAPPDATA%")

' target
targetPath = appData & "\Google\Chrome\User Data2"

'ファイルシステムオブジェクト作成
Set objFS = CreateObject("Scripting.FileSystemObject")

If objFS.FolderExists(targetPath) Then
  DeletefolderAndFiles(targetPath)
  'msgbox("ワークディレクトリを削除しました")
End If

'"C:\Program Files\Google\Chrome\Application\chrome.exe" --user-data-dir="C:\Users\adminuser\AppData\Local\Google\Chrome\User Data2" --disable-web-security
'"C:\Program Files\Google\Chrome\Application"
' ショートカット生成
desktop = objShell.SpecialFolders("Desktop")
Set shortcut = objShell.CreateShortcut(desktop & "\secretChrome.lnk")
With shortcut
'    .TargetPath = "%SystemRoot%\System32\calc.exe"
'    .WorkingDirectory = desktop
    .TargetPath = googlePath
		.Arguments = " --user-data-dir=" & """" & targetPath & """" & " --disable-web-security"
    .WorkingDirectory = "C:\Program Files\Google\Chrome\Application"
    .Save
End With

msgbox("ショートカットを生成しました")

'解放
Set objShell = Nothing
Set objFS = Nothing

'指定したフォルダの中身をすべて削除する。
Sub DeletefolderAndFiles(DirectoryPath)
    Dim objFile
    Set objFile = CreateObject( "Scripting.FileSystemObject" )

    Dim objFolder
    Set objFolder = objFile.GetFolder(DirectoryPath)

    Dim objSubFolder

    ' サブフォルダを取得して再起呼び出し。
    For each objSubFolder in objFolder.SubFolders
        DeletefolderAndFiles(DirectoryPath & "\" & objSubFolder.Name)
        objFile.DeleteFolder DirectoryPath & "\" & objSubFolder.Name, True
    Next

    Dim fileName
    ' フォルダ内のファイルを取得し、削除する。
    For each fileName in objFolder.files
        objFile.DeleteFile DirectoryPath & "\" & fileName.Name, True
    Next

End Sub

