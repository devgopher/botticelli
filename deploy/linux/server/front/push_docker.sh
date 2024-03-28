function check_and_setup()
{
  read -p "Enter $1 $2: " $1
}

rm -rf /tmp/tmp_docker_botticelli
mkdir /tmp/tmp_docker_botticelli

pushd Botticelli

check_and_setup http_port "(example:80)"
check_and_setup https_port "(example:8080)"
check_and_setup backend_url "(example: https://10.10.10.10:50000)"
check_and_setup analytics_url "(example: https://10.10.10.10:50055)"

cp Dockerfile /tmp/tmp_docker_botticelli/

sed -i "s/\$http_port/$http_port/g" /tmp/tmp_docker_botticelli/Dockerfile
sed -i "s/\$https_port/$https_port/g" /tmp/tmp_docker_botticelli/Dockerfile
sed -i "s/\$backend_url/$backend_url/g" /tmp/tmp_docker_botticelli/Dockerfile
sed -i "s/\$analytics_url/$analytics_url/g" /tmp/tmp_docker_botticelli/Dockerfile

docker build --tag "botticelli_server_front_dev:0.4" . --no-cache

#docker image push --all-tags <registry-host:5000/myname/myimage>