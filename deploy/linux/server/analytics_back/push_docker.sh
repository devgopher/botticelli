function check_and_setup()
{
  read -p "Enter $1 $2: " $1
}

rm -rf /tmp/tmp_docker_botticelli
mkdir /tmp/tmp_docker_botticelli

pushd Botticelli.Server.Analytics

check_and_setup http_port "(example:80)"
check_and_setup https_port "(example:8080)"
check_and_setup db_host "(example:127.0.0.1)"
check_and_setup db_port "(example:5432)"
check_and_setup db_user "(example:my_own_db_user)"
check_and_setup db_password "(example:12345678)"

cp Dockerfile /tmp/tmp_docker_botticelli/

sed -i 's/\$http_port/$http_port/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$https_port/$https_port/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$db_host/$db_host/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$db_port/$db_port/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$db_user/$db_user/g' /tmp/tmp_docker_botticelli/Dockerfile
sed -i 's/\$db_password/$db_password/g' /tmp/tmp_docker_botticelli/Dockerfile


docker build --tag 'botticelli_server_back_dev:0.4' . --no-cache

#docker image push --all-tags <registry-host:5000/myname/myimage>