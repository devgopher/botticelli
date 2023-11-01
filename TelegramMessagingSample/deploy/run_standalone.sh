sudo apt-get update && \  sudo apt-get install -y dotnet-sdk-7.0

rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout dev/0.3_pre
git pull

pushd TelegramMessagingSample

dotnet run TelegramMessagingSample.csproj &

echo BOT ID:
cat Data/botId

popd