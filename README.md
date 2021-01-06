# Docker 

## publish at

https://hub.docker.com/repository/docker/mjdocker31

## test

http://localhost:8880/api/AirTrafficInfo

## status check

- docker -v
- docker ps
- docker ps --all
- docker image list
- docker image ls
- docker container ls -a (works best)
- docker container list -all
- docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" metalsprices_test1

## build
- docker build -t image_name . 
- docker run -d -p 8080:80 --name container_name image_name

-d: This is short for detach and means that the Docker container will run in the background. We won’t be seeing any output from the application that’s running inside the container. If we want to see that output, we can use:
docker logs container_name

## cleanup

- docker rmi image_id
- docker rm container_id
- docker stop container_id
- docker container prune -f (remove all containers, not asking for confirmation)
- docker-compose down - stops and removes containers

## compose

- docker-compose up
- docker-compose up --build --force-recreate --no-deps

  --force-recreate    Recreate containers even if their configuration
                      and image haven't changed.
                      
  --build             Build images before starting containers.
  
  --no-deps           Don't start linked services.

- alternate restart:

    stop docker compose: $ docker-compose down

    remove the container: $ docker system prune -a

    start docker compose: $ docker-compose up -d

docker system prune -a will delete all images, even ones for other projects. It's recommended against the usage of this.

## hub
- docker login -u "user" -p "password" docker.io
- docker push mjdocker31/test2api

## net core
- dotnet new webapp -o aspnetcoreapp
- dotnet run --MetalsPrices.Api

## useful links:

### Azure Deploy
https://medium.com/@pugillum/asp-net-core-2-web-api-docker-and-azure-f84e28aa6267

### Localhost run
https://docs.docker.com/engine/examples/dotnetcore/

### Dockerfile

https://softchris.github.io/pages/dotnet-dockerize.html#why

### VS project setup

https://docs.microsoft.com/pl-pl/visualstudio/containers/tutorial-multicontainer?view=vs-2019

### Docker Hub push common error

https://forums.docker.com/t/docker-push-error-requested-access-to-the-resource-is-denied/64468/6
