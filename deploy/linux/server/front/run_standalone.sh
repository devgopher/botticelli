sudo apt-get update && \  sudo apt-get install -y dotnet-sdk-7.0

rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout release/0.2
git pull

pushd Botticelli.Server.FrontNew

# Let's propose, that server back is on the same server
my_ip=$(ip route get 8.8.8.8 | awk -F"src " 'NR==1{split($2,a," ");print a[1]}')

sed -i 's/localhost/'$my_ip'/g' wwwroot/appsettings.json

nohup dotnet run Botticelli.Server.FrontNew.csproj &

popd