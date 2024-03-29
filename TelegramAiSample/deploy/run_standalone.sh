sudo apt-get update
sudo apt-get install -y dotnet-sdk-7.0 dotnet-runtime-7.0 aspnetcore-runtime-7.0  dotnet-apphost-pack-7.0


rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout release/0.3
git pull

pushd TelegramAiSample


echo -n "Enter your ChatGpt API key:"
read ChatGptSettings__ApiKey
echo "API key : $ChatGptSettings__ApiKey"

sed -i 's/chatgpt-api-key-placeholder/'$ChatGptSettings__ApiKey'/g' Properties/launchSettings.json

nohup dotnet run TelegramAiChatGptSample.csproj &

echo "BOT ID:"
cat Data/botId

popd