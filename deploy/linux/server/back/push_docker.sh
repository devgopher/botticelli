function check_and_setup()
{
  read -p "Enter $1 $2: " $1
}

rm -rf /tmp/tmp_docker_botticelli
mkdir /tmp/tmp_docker_botticelli

pushd Botticelli

check_and_setup http_port "(example:80)"
check_and_setup https_port "(example:8080)"
check_and_setup db_password "(example:12345678)"
check_and_setup email "(example:foo@bar.com)"
check_and_setup email_smtp_server "(example:smtp.bar.com)"
check_and_setup email_smtp_port "(example:465)"
check_and_setup email_smtp_pwd "(smtp server/application password)"
check_and_setup email_use_ssl "(true/false)"
check_and_setup requires_authentification "(true/false)"

cp Dockerfile /tmp/tmp_docker_botticelli/

sed -i 's/\$http_port/$http_port/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$https_port/$https_port/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$db_password/$db_password/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$email/$email/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$email_smtp_port/$email_smtp_port/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$email_smtp_pwd/$email_smtp_pwd/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$email_use_ssl/$email_use_ssl/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$requires_authentification/$requires_authentification/g' /tmp/tmp_docker_botticelli/Dockerfile


docker build --tag 'botticelli_server_back_dev:0.4' . --no-cache

#docker image push --all-tags <registry-host:5000/myname/myimage>