## UI

- C:\...\UI\Proj2_Planes> npm start
- http://localhost:4200/pages/maps/bubble (use Chrome, no ad blockers etc. else CORS issiue may prevent calls to traffic api)
- can be paired with: simulated, powershell or docker type of backend
- source with separate readme https://github.com/mjoniec/UI/tree/main/Proj2_Planes

## localhost docker

- !!! needs to be updated after multiple code changes !!!
- traffic api + multiple airport and plane services
- hosted on docker containers
- all launched by executing: PowerShell C:\...\Planes\HostedServices> Docker Compose - docker-compose up 
- http://localhost:8880/api/TrafficInfo

## localhost powershell

- traffic api + multiple airport and plane services 
- hosted on premises
- all launched by executing: PowerShell C:\...\Planes\HostedServices> .\launchLocalhostOnPremises.ps1 
- https://localhost:44389/api/TrafficInfo

## localhost simulated

- traffic api + traffic simulator service
- hosted on premises
- manual launch TrafficInfoHttpApi and then TrafficSimulatorService from Visual Studio
- https://localhost:44389/api/TrafficInfo

## Running single project from its Dockerfile from powershell command

powershell active directory set for solution and then pointing with sub folder path to Dockerfile is necessary for Docker to be able to see separate subtrees to folders with libraries and hostable projects

- PS C:\Users\Seraf\Desktop\Planes> docker build -t trafficinfohttpapi -f HostedServices/TrafficInfoHttpApi/Dockerfile .

open new PS terminal (any directory) and run container with logs outputing (without -d), open in browser
- docker run -p 8080:80 --name trafficinfohttpapi trafficinfohttpapi
- http://localhost:8080/api/TrafficInfo

stop container, remove container, remove image (previous PS terminal)
- docker stop trafficinfohttpapi
- docker container rm trafficinfohttpapi
- docker rmi trafficinfohttpapi

## Docker general notes

compose with images rebuild and proper shotdown so it could be launched again

- docker-compose up --build --force-recreate --no-deps 
- ctrl + c - wait till all containers stops
- docker-compose down - wait till all containers closes

status check

- docker -v
- docker ps - lists running containers
- docker ps --all - not always shows what expected
- docker image list
- docker image ls
- docker container ls -a (works best)
- docker container list -all
- docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" metalsprices_test1

build

- docker build -t image_name . (only lower case accepted as repository name)

run

- docker run -d -p 8080:80 --name container_name image_name
- docker run -d -p 8081:81 --name plane_1 plane
- docker run -d -p 8082:82 --name plane_2 plane

-d: This is short for detach and means that the Docker container will run in the background. We won’t be seeing any output from the application that’s running inside the container. If we want to see that output, we can use:
docker logs container_name

cleanup

- docker stop 1111_container_id
- docker container rm 1111_container_id
- docker rmi 2222_image_id
- docker rmi 2222_image_id -f (force if referenced by other)
- docker rm 1111_container_id
- docker stop 1111_container_id
- docker container prune -f (remove all containers, not asking for confirmation)
- docker-compose down - stops and removes containers

## compose info

--force-recreate    Recreate containers even if their configuration and image haven't changed.

--build             Build images before starting containers.
 
--no-deps           Don't start linked services.

alternate restart:

- stop docker compose: $ docker-compose down
- remove the container: $ docker system prune -a
- start docker compose: $ docker-compose up -d

docker system prune -a will delete all images, even ones for other projects. It's recommended against the usage of this.

## docker hub

- docker login -u "user" -p "password" docker.io
- https://hub.docker.com/repository/docker/mjdocker31
- docker build -t mjdocker31/planes:trafficinfoapi .
- docker run mjdocker31/planes:trafficinfoapi
- docker push mjdocker31/planes:trafficinfoapi
- docker build -t mjdocker31/planes:plane .
- docker run mjdocker31/planes:plane
- docker push mjdocker31/planes:plane
- docker build -t mjdocker31/planes:airport .
- docker run mjdocker31/planes:airport
- docker push mjdocker31/planes:airport
