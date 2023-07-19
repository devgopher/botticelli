git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout mvp/0.1
git pull

pushd Botticelli.Server.FrontNew

dotnet publish -c Release -r linux-x64 --output ../publish Botticelli.Server.FrontNew.csproj
popd

pushd botticelli/deploy/linux/server/front
docker build -f Dockerfile .
docker push hub.docker.com botticelli_server_front:0.1