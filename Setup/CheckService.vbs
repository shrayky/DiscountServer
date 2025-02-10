Function CheckService
    On Error Resume Next
    
    Set WshShell = CreateObject("WScript.Shell")
    Set fso = CreateObject("Scripting.FileSystemObject")
    
    ' Создаем лог файл
    Dim logFile : logFile = "C:\Windows\Temp\DiscountServer_Install.log"
    Set log = fso.OpenTextFile(logFile, 8, True)
    
    log.WriteLine "Starting service check at " & Now
    
    ' Пробуем запустить службу
    Err.Clear
    WshShell.Run "net start DiscountService", 0, True
    
    If Err.Number <> 0 Then
        log.WriteLine "Service start failed. Error: " & Err.Number & " - " & Err.Description
        
        ' Если служба не запустилась, предлагаем скачать .NET 8
        If MsgBox("Служба не запустилась. Возможно, требуется установка .NET 8 Runtime." & vbCrLf & _
                  "Хотите открыть страницу загрузки .NET 8 Runtime?", _
                  vbYesNo + vbQuestion, "Требуется .NET 8") = vbYes Then
            WshShell.Run "https://dotnet.microsoft.com/download/dotnet/8.0", 1, False
        End If
    Else
        log.WriteLine "Service started successfully"
    End If
    
    log.WriteLine "Finished service check at " & Now
    log.Close
    
    CheckService = True
End Function