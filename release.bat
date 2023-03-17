dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
copy .\bin\Release\net7.0\win-x64\*.exe .\AsciiGame.exe