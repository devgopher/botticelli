function check_and_setup()
{
  read -p "Enter $1 $2: " $1
}

check_and_setup http_port "(example:80)"
check_and_setup https_port "(example:8080)"
check_and_setup db_password "(example:12345678)"
check_and_setup email "(example:foo@bar.com)"
check_and_setup email_smtp_server "(example:smtp.bar.com)"
check_and_setup email_smtp_port "(example:465)"
check_and_setup email_smtp_pwd "(smtp server/application password)"
check_and_setup email_use_ssl "(true/false)"
check_and_setup requires_authentification "(true/false)"

export SecureStorageSettings__ConnectionString="Filename=database.db;Connection=shared;Password=$db_password"
export ServerSettings__TokenLifetimeMin=1000
export ServerSettings__SmtpClientOptions__Server=$email_smtp_server
export ServerSettings__SmtpClientOptions__Port=$email_smtp_port
export ServerSettings__SmtpClientOptions__User=$email
export ServerSettings__SmtpClientOptions__Password=$email_smtp_pwd
export ServerSettings__SmtpClientOptions__UseSsl=$email_use_ssl
export ServerSettings__SmtpClientOptions__RequiresAuthentication=$requires_authentification
export ServerSettings__SmtpClientOptions__PreferredEncoding=null
export ServerSettings__SmtpClientOptions__UsePickupDirectory=false
export ServerSettings__SmtpClientOptions__MailPickupDirectory=C:\\Temp
export ServerSettings__SmtpClientOptions__SocketOptions=2
export ServerSettings__ServerEmail=$email
export ServerSettings__ServerUrl=$email_smtp_server

sudo docker run -d -t -i -p 5042:5042 -p 7247:7247 \
-v /data:/data \
-e ASPNETCORE_URLS="https://0.0.0.0:7247;http://0.0.0.0:5042" \
-e ASPNETCORE_ENVIRONMENT="Release" \
-e SecureStorageSettings__ConnectionString="Filename=/data/database.db;Connection=shared;Password=$db_password" \
-e ServerSettings__TokenLifetimeMin=1000 \
-e ServerSettings__SmtpClientOptions__Server="$email_smtp_server" \
-e ServerSettings__SmtpClientOptions__Port="$email_smtp_port" \
-e ServerSettings__SmtpClientOptions__User="$email_addr" \
-e ServerSettings__SmtpClientOptions__Password="$email_smtp_pwd" \
-e ServerSettings__SmtpClientOptions__UseSsl="$email_use_ssl" \
-e ServerSettings__SmtpClientOptions__RequiresAuthentication="$requires_authentification" \
-e ServerSettings__smtpClientOptions__PreferredEncoding="null" \
-e ServerSettings__SmtpClientOptions__UsePickupDirectory="false" \
-e ServerSettings__SmtpClientOptions__MailPickupDirectory="C:\\" \
-e ServerSettings__SmtpClientOptions__SocketOptions="2" \
-e ServerSettings__ServerEmail="$email_addr" \
-e ServerSettings__ServerUrl="$email_smtp_server" \
103-252-118-172.cloud-xip.com:5000/botticelli-server:0.4

