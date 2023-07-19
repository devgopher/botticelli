sudo apt-get update && \  sudo apt-get install -y dotnet-sdk-7.0

rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout mvp/0.1
git pull

echo -n "Enter your ChatGpt API key:"
read ChatGptSettings__ApiKey
echo "API key : $ChatGptSettings__ApiKey"

pushd TelegramAiSample
sed -i 's/chatgpt-api-key-placeholder/'$ChatGptSettings__ApiKey'/g' Properties/launchSeettings.json

dotnet run TelegramAiChatGptSample.csproj &

echo "BOT ID:"
cat Data/botId

popd