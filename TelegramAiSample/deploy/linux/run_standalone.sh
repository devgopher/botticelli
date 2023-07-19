sudo apt-get update && \  sudo apt-get install -y dotnet-sdk-7.0

rm -rf botticelli/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/
git checkout mvp/0.1
git pull

pushd TelegramAiSample
dotnet run TelegramAiChatGptSample.csproj &

popd