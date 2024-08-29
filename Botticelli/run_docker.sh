function check_and_setup()
{
  read -p "Enter $1 $2: " $1
}

check_and_setup http_port "(example:80)"
check_and_setup https_port "(example:8080)"
check_and_setup db_password "(example:12345678)"
check_and_setup email_addr "(example:foo@bar.com)"
check_and_setup email_smtp_server "(example:smtp.bar.com)"
check_and_setup email_smtp_port "(example:465)"
check_and_setup email_smtp_pwd "(smtp server/application password)"
check_and_setup email_use_ssl "(true/false)"
check_and_setup requires_authentification "(true/false)"

export SecureStorageSettings__ConnectionString="Filename=/data/database.db;Connection=shared;Password=$db_password"
export ServerSettings__TokenLifetimeMin=1000
export ServerSettings__SmtpClientOptions__Server=$email_smtp_server
export ServerSettings__SmtpClientOptions__Port=$email_smtp_port
export ServerSettings__SmtpClientOptions__User=$email
export ServerSettings__SmtpClientOptions__Password=$email_smtp_pwd
export ServerSettings__SmtpClientOptions__UseSsl=$email_use_ssl
export ServerSettings__SmtpClientOptions__RequiresAuthentication=$requires_authentification
export ServerSettings__SmtpClientOptions__PreferredEncoding=null
export ServerSettings__SmtpClientOptions__UsePickupDirectory=false
export ServerSettings__SmtpClientOptions__MailPickupDirectory=/tmp
export ServerSettings__SmtpClientOptions__SocketOptions=2
export ServerSettings__ServerEmail=$email
export ServerSettings__ServerUrl=$email_smtp_server

mkdir Data
docker build --tag "botticelli_server_back_dev:0.6" . --no-cache
docker run "botticelli_server_back_dev:0.6" -v Data:/data