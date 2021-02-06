$currentDir = (Get-Item -Path ".\" -Verbose).FullName;
#Write-Output $currentDir

$paramsAirport = @("/C"; $currentDir + "\Backend\Airport\Airport"; "dotnet run"; )

$dirAirTrafficInfoApi = $currentDir + '\Backend\AirTrafficInfo\AirTrafficInfoApi';
$dirAirport = $currentDir + '\Backend\Airport\Airport';
$dirPlane = $currentDir + '\Backend\Plane\Plane';

Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirTrafficInfoApi -ArgumentList 'run'
Start-Sleep -s 4

Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirport -ArgumentList 'run --name=PS_Airport1 --color=#FF0000 --latitude=50 --longitude=50'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirport -ArgumentList 'run --name=PS_Airport2 --color=#00FF00 --latitude=30 --longitude=100'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirport -ArgumentList 'run --name=PS_Airport3 --color=#0000FF --latitude=10 --longitude=10'
Start-Sleep -s 2

Start-Process -FilePath 'dotnet' -WorkingDirectory $dirPlane -ArgumentList 'run --name=PS_Plane1'
Start-Sleep -s 1
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirPlane -ArgumentList 'run --name=PS_Plane2'
Start-Sleep -s 1
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirPlane -ArgumentList 'run --name=PS_Plane3'
Start-Sleep -s 1

#https://localhost:62462/api/AirTrafficInfo