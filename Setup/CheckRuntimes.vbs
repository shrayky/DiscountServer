Function CompareVersions(version1, version2)
    Dim v1Parts, v2Parts
    v1Parts = Split(version1, ".")
    v2Parts = Split(version2, ".")
    
    For i = 0 To UBound(v1Parts)
        If CLng(v1Parts(i)) > CLng(v2Parts(i)) Then
            CompareVersions = 1
            Exit Function
        ElseIf CLng(v1Parts(i)) < CLng(v2Parts(i)) Then
            CompareVersions = -1
            Exit Function
        End If
    Next
    
    CompareVersions = 0
End Function

Function ExtractVersion(line, runtime)
    Dim start, finish
    start = InStr(line, runtime & " ")
    If start > 0 Then
        start = start + Len(runtime) + 1
        finish = InStr(start, line, " [")
        If finish > 0 Then
            ExtractVersion = Mid(line, start, finish - start)
        End If
    End If
End Function

Function CheckDotNetRuntimes
    Dim shell, runtimesOutput, aspnetOutput
    Set shell = CreateObject("WScript.Shell")
    Dim minVersion : minVersion = "8.0.2"
    
    ' Проверяем .NET Runtime
    Set runtimesOutput = shell.Exec("dotnet --list-runtimes")
    Dim runtimeFound : runtimeFound = False
    
    Do While Not runtimesOutput.StdOut.AtEndOfStream
        Dim line : line = runtimesOutput.StdOut.ReadLine()
        If InStr(line, "Microsoft.NETCore.App") > 0 Then
            Dim version : version = ExtractVersion(line, "Microsoft.NETCore.App")
            If Not IsEmpty(version) Then
                If Left(version, 4) = "8.0." Then
                    If CompareVersions(version, minVersion) >= 0 Then
                        runtimeFound = True
                        Exit Do
                    End If
                End If
            End If
        End If
    Loop
    
    ' Проверяем ASP.NET Core Runtime
    Set aspnetOutput = shell.Exec("dotnet --list-runtimes")
    Dim aspnetFound : aspnetFound = False
    
    Do While Not aspnetOutput.StdOut.AtEndOfStream
        line = aspnetOutput.StdOut.ReadLine()
        If InStr(line, "Microsoft.AspNetCore.App") > 0 Then
            version = ExtractVersion(line, "Microsoft.AspNetCore.App")
            If Not IsEmpty(version) Then
                If Left(version, 4) = "8.0." Then
                    If CompareVersions(version, minVersion) >= 0 Then
                        aspnetFound = True
                        Exit Do
                    End If
                End If
            End If
        End If
    Loop
    
    If Not (runtimeFound And aspnetFound) Then
        MsgBox "Для работы приложения требуется установить .NET Runtime 8.0.2 (или выше) и ASP.NET Core Runtime 8.0.2 (или выше)." & vbCrLf & _
               "Скачайте и установите их с сайта https://dotnet.microsoft.com/download/dotnet/8.0", _
               vbExclamation + vbOKOnly, _
               "Отсутствуют необходимые компоненты"
        CheckDotNetRuntimes = 1603 ' ERROR_INSTALL_FAILURE
    Else
        CheckDotNetRuntimes = 0
    End If
End Function