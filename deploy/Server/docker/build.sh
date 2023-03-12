dotnet publish ../../ -r linux-x64 -o app/
docker build -f Dockerfile -t dotnet4u/botticelli_server:0.1 .
docker push dotnet4u/botticelli_server:0.1
