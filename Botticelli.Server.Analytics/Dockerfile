FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5251

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Botticelli.Server.Analytics/Botticelli.Server.Analytics.csproj", "Botticelli.Server.Analytics/"]
COPY ["Botticelli.SecureStorage/Botticelli.SecureStorage.csproj", "Botticelli.SecureStorage/"]
COPY ["Botticelli.Shared/Botticelli.Shared.csproj", "Botticelli.Shared/"]
COPY ["Botticelli.Analytics.Shared/Botticelli.Analytics.Shared.csproj", "Botticelli.Analytics.Shared/"]
RUN dotnet restore "Botticelli.Server.Analytics/Botticelli.Server.Analytics.csproj"
COPY . .
WORKDIR "/src/Botticelli.Server.Analytics"
RUN dotnet build "Botticelli.Server.Analytics.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Botticelli.Server.Analytics.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Botticelli.Server.Analytics.dll"]
