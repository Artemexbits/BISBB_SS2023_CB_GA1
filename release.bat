dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
copy .\bin\Release\net7.0\win-x64\publish\BISBB_SS2023_CB_GA1.exe .\AsciiGame.exe