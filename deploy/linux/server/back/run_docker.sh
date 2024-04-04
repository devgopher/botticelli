sudo docker run -d -t -i -p 5042:5000 -p 7247:5001 \
-e ASPNETCORE_URLS https://0.0.0.0:5001;http://0.0.0.0:5000 \
-e ASPNETCORE_-eIRONMENT=Release \
-e SecureStorageSettings__ConnectionString="Filename=database.db;Connection=shared;Password=$db_password" \
-e ServerSettings__TokenLifetimeMin=1000 \
-e ServerSettings__SmtpClientOptions__Server $email_smtp_server \
-e ServerSettings__SmtpClientOptions__Port $email_smtp_port \
-e ServerSettings__SmtpClientOptions__User $email_addr \
-e ServerSettings__SmtpClientOptions__Password $email_smtp_pwd \
-e ServerSettings__SmtpClientOptions__UseSsl $email_use_ssl \
-e ServerSettings__SmtpClientOptions__RequiresAuthentication $requires_authentification \
-e ServerSettings__smtpClientOptions__PreferredEncoding null \
-e ServerSettings__SmtpClientOptions__UsePickupDirectory false \
-e ServerSettings__SmtpClientOptions__MailPickupDirectory C:\\ \
-e ServerSettings__SmtpClientOptions__SocketOptions 2 \
-e ServerSettings__ServerEmail $email_addr \
-e ServerSettings__ServerUrl $email_smtp_server


