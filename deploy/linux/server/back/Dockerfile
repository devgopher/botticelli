FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything
RUN git clone https://github.com/devgopher/botticelli.git
WORKDIR /app/botticelli/Botticelli
RUN git fetch --all
RUN git switch dev/0.4
RUN git pull

COPY . ./


# Build and publish a release
RUN dotnet publish -c Release -o /app/out
RUN dotnet dev-certs https --trust

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .



EXPOSE 5000
EXPOSE 5001


ENTRYPOINT ["dotnet", "Botticelli.Server.dll"]