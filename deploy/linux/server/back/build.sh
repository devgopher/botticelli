rm -rf botticelli
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout mvp/0.1
git pull

pushd Botticelli

dotnet publish -c Release -r linux-x64 --output ../publish Botticelli.Server.csproj
popd
popd

pushd botticelli/deploy/linux/server/back
mkdir -p app
cp -r ../../../../publish/* ./app

docker build -f Dockerfile .
docker push hub.docker.com botticelli_server:0.1