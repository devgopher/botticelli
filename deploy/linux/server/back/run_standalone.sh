function check_and_setup()
{
  read -p "Enter $1 $2: " $1
}

sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0 dotnet-runtime-8.0 aspnetcore-runtime-8.0

rm -rf botticelli``/
git clone https://github.com/devgopher/botticelli.git
pushd botticelli/ || exit
git checkout dev/0.9
git pull

pushd Botticelli || exit


