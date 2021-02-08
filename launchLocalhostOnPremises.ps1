$currentDir = (Get-Item -Path ".\" -Verbose).FullName;
#Write-Output $currentDir

$paramsAirport = @("/C"; $currentDir + "\Backend\Airport\Airport"; "dotnet run"; )

$dirAirTrafficInfoApi = $currentDir + '\Backend\AirTrafficInfo\AirTrafficInfoApi';
$dirAirport = $currentDir + '\Backend\Airport\Airport';
$dirPlane = $currentDir + '\Backend\Plane\Plane';

Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirTrafficInfoApi -ArgumentList 'run'
Start-Sleep -s 4

Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirport -ArgumentList 'run -name="New York" --color=#0000FF --latitude=40.68 --longitude=-74.17'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirport -ArgumentList 'run --name=London --color=#FF0000 --latitude=51.48 --longitude=-0.11'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirport -ArgumentList 'run --name=Tokyo --color=#28FFB8 --latitude=35.67 --longitude=139.75'
Start-Sleep -s 2

Start-Process -FilePath 'dotnet' -WorkingDirectory $dirPlane -ArgumentList 'run --name="Plane 1"'
Start-Sleep -s 1
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirPlane -ArgumentList 'run --name="Plane 2"'
Start-Sleep -s 1
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirPlane -ArgumentList 'run --name="Plane 3"'
Start-Sleep -s 1

#https://localhost:62462/api/AirTrafficInfo