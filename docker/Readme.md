docker build -t counter-image -f Dockerfile .

docker ps -a

docker create --name core-counter counter-image

docker start core-counter

docker attach --sig-proxy=false core-counter

docker stop core-counter

docker rm core-counter

docker rmi counter-image:latest

docker rmi mcr.microsoft.com/dotnet/core/aspnet:3.1
