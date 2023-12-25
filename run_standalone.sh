sudo apt-get update
sudo apt-get install -y dotnet-sdk-7.0 dotnet-runtime-7.0 aspnetcore-runtime-7.0

check_and_setup http_port "(example:80)"
check_and_setup https_port "(example:8080)"

export ASPNETCORE_ENVIRONMENT=Release
export ASPNETCORE_URLS="https://0.0.0.0:$https_port;http://0.0.0.0:$http_port"

rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout release/0.3
git pull

pushd Botticelli.Server.Analytics

nohup dotnet run Botticelli.Server.Analytics.csproj &

popd