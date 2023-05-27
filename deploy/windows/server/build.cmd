pushd ..\..\..\Botticelli\
dotnet publish -c Release -r linux-x64 --output ../publish Botticelli.Server.csproj
popd
