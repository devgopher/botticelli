FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything
RUN git clone https://github.com/devgopher/botticelli.git
WORKDIR /app/botticelli/Botticelli
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

EXPOSE 53635
EXPOSE 53636

ENV ASPNETCORE_URLS https://0.0.0.0:53636;http://0.0.0.0:53635
ENV ASPNETCORE_ENVIRONMENT=Release
ENV SecureStorageSettings__ConnectionString="Filename=database.db;Connection=shared;Password=$db_password"
ENV ServerSettings__TokenLifetimeMin=1000
ENV ServerSettings__SmtpClientOptions__Server $email_smtp_server
ENV ServerSettings__SmtpClientOptions__Port $email_smtp_port
ENV ServerSettings__SmtpClientOptions__User $email
ENV ServerSettings__SmtpClientOptions__Password $email_smtp_pwd
ENV ServerSettings__SmtpClientOptions__UseSsl $email_use_ssl
ENV ServerSettings__SmtpClientOptions__RequiresAuthentication $requires_authentification
ENV ServerSettings__smtpClientOptions__PreferredEncoding null
ENV ServerSettings__SmtpClientOptions__UsePickupDirectory false
ENV ServerSettings__SmtpClientOptions__MailPickupDirectory C:\\Temp
ENV ServerSettings__SmtpClientOptions__SocketOptions 2
ENV ServerSettings__ServerEmail $email
ENV ServerSettings__ServerUrl $email_smtp_server


ENTRYPOINT ["dotnet", "Botticelli.Server.dll"]