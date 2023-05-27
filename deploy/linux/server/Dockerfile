FROM mcr.microsoft.com/dotnet/sdk:7.0.5-alpine3.17-amd64 AS build
WORKDIR /app

COPY ../../../publish ./

RUN dotnet build -c Release --no-restore
RUN dotnet test --no-restore --verbosity normal
RUN dotnet publish -c Release -r linux-x64 --output ../publish Botticelli.Server.csproj

FROM mcr.microsoft.com/dotnet/runtime:7.0.5-alpine3.17-amd64 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Botticelli.Server.dll"]