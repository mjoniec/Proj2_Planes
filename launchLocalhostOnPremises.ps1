#cd Backend
#cd AirTrafficInfo\AirTrafficInfoApi
#dotnet run 
#cd ..\..
#cd Airport
#dotnet run --color='#888111'
#cd ..

$currentDir = (Get-Item -Path ".\" -Verbose).FullName;

$paramsAirport = @("/C"; $currentDir + "\Backend\Airport\Airport"; "dotnet run"; )

#Write-Output $paramsAirTrafficInfoApi 

$dirAirTrafficInfoApi = $currentDir + '\Backend\AirTrafficInfo\AirTrafficInfoApi';
$dirAirport = $currentDir + '\Backend\Airport\Airport';

Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirTrafficInfoApi -ArgumentList 'run'
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirport -ArgumentList 'run --color=#FF0000 --latitude=50 --longitude=50'
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirport -ArgumentList 'run --color=#00FF00 --latitude=30 --longitude=100'
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirport -ArgumentList 'run --color=#0000FF --latitude=10 --longitude=10'

#http://localhost:62462/api/AirTrafficInfo
#https://localhost:62462/api/AirTrafficInfo