rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout mvp/0.1
git pull

pushd Botticelli

dotnet run Botticelli.Server.csproj &
