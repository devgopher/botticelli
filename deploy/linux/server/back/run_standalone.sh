sudo apt-get update && \  sudo apt-get install -y dotnet-sdk-7.0

rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout release/0.2
git pull

pushd Botticelli

dotnet run Botticelli.Server.csproj &

popd