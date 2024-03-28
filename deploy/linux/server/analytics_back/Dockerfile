FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything
RUN git clone https://github.com/devgopher/botticelli.git
WORKDIR /app/botticelli/Botticelli.Server.Analytics
RUN git fetch --all
RUN git switch dev/0.4
RUN git pull

COPY . ./

# Restore as distinct layers
# RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o /app/out
RUN ls .

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 5251
EXPOSE 5252

ENV ASPNETCORE_URLS https://0.0.0.0:$https_port;http://0.0.0.0:$http_port
ENV ASPNETCORE_ENVIRONMENT Release
ENV SecureStorageConnectionString "User ID=$db_user;Password=$db_password;Host=$db_host;Port=$db_port;Database=botticelli_analytics;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;"

ENTRYPOINT ["dotnet", "Botticelli.Server.Analytics.dll"]
