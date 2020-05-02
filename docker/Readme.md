docker build -t counter-image -f Dockerfile .

docker ps -a

docker create --name core-counter counter-image

docker start core-counter

docker attach --sig-proxy=false core-counter

docker stop core-counter

docker rm core-counter

docker rmi counter-image:latest

docker rmi mcr.microsoft.com/dotnet/core/aspnet:3.1

(single run example with no create or start)

docker run -it --rm counter-image

docker run -it --rm counter-image 3 (runs exactly 3 times!)