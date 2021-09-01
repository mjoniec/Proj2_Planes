$currentDir = (Get-Item -Path ".\" -Verbose).FullName;
Write-Output $currentDir

$dirTrafficInfoHttpApi = $currentDir + '\TrafficInfoHttpApi';
$dirAirportService = $currentDir + '\AirportService';
$dirPlaneService = $currentDir + '\PlaneService';

Start-Process -FilePath 'dotnet' -WorkingDirectory $dirTrafficInfoHttpApi -ArgumentList 'run'
Start-Sleep -s 5

#USA
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name="New York" --color=#0000FF --latitude=40.68 --longitude=-74.17'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name="Miami" --color=#0b40f4 --latitude=25.78 --longitude=-80.17'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name="Los Angeles" --color=#0a3bdb --latitude=33.93 --longitude=-118.4'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name="San Francisco" --color=#082eaa --latitude=37.78 --longitude=-122.41'
Start-Sleep -s 2

#Europe
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name=London --color=#FF0000 --latitude=51.48 --longitude=-0.11'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name=Sevilla --color=#ff1a1a --latitude=37.37 --longitude=-5.98'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name=Rome --color=#ff1a1a --latitude=41.89 --longitude=12.5'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name=Moscow --color=#ff3333 --latitude=55.75 --longitude=37.63'
Start-Sleep -s 2

#Asia
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name=Tokyo --color=#28FFB8 --latitude=35.67 --longitude=139.75'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name="Kuala Lumpur" --color=#1affff --latitude=3.13 --longitude=101.68'
Start-Sleep -s 2

#Middle East
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name=Cairo --color=#ffff00 --latitude=29.9 --longitude=31.4'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name=Dubai --color=#ffff33 --latitude=25.3 --longitude=55.2'
Start-Sleep -s 2

#Australia
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name=Sydney --color=#c61aff --latitude=-33.8 --longitude=151.2'
Start-Sleep -s 2

#South America
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirAirportService -ArgumentList 'run --name="Rio De Janeiro" --color=#40ff00 --latitude=-22.9 --longitude=-43.2'
Start-Sleep -s 2

#Planes and Pilots
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirPlaneService -ArgumentList 'run --name="Maverick"'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirPlaneService -ArgumentList 'run --name="Iceman"'
Start-Sleep -s 2
Start-Process -FilePath 'dotnet' -WorkingDirectory $dirPlaneService -ArgumentList 'run --name="Slider"'

