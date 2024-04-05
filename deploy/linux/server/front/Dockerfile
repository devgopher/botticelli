FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything
RUN git clone https://github.com/devgopher/botticelli.git
WORKDIR /app/botticelli/Botticelli.Server.FrontNew 
RUN git fetch --all
RUN git switch dev/0.4
RUN git pull


COPY . ./
RUN dotnet dev-certs https --trust

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "run"]