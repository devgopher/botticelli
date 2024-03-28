FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything
RUN git clone https://github.com/devgopher/botticelli.git
WORKDIR /app/botticelli/Botticelli
RUN git fetch --all
RUN git switch dev/0.4

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
ENTRYPOINT ["dotnet", "Botticelli.Server.dll"]
