sudo apt-get update && \  sudo apt-get install -y dotnet-sdk-7.0

rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout dev/0.3_pre
git pull

pushd TelegramAiSample

echo -n "Enter your ChatGpt API key:"
read ChatGptSettings__ApiKey
echo "API key : $ChatGptSettings__ApiKey"

sed -i 's/chatgpt-api-key-placeholder/'$ChatGptSettings__ApiKey'/g' Properties/launchSettings.json

dotnet run TelegramAiChatGptSample.csproj &

echo "BOT ID:"
cat Data/botId

popd