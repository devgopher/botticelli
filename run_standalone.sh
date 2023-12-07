sudo apt-get update && \  sudo apt-get install -y dotnet-sdk-7.0

rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout release/0.3
git pull

pushd Botticelli.Server.Analytics

nohup dotnet run Botticelli.Server.Analytics.csproj &

popd