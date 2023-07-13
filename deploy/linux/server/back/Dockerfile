FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Botticelli.Server.dll"]