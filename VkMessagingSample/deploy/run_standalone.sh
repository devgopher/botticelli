sudo apt-get update && \  sudo apt-get install -y dotnet-sdk-7.0

rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout release/0.3
git pull

pushd VkMessagingSample

dotnet run VkMessagingSample.csproj &

echo BOT ID:
cat Data/botId

popd