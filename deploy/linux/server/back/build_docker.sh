docker build -t botticelli-server:0.4 .
docker login --username testuser --password 378@312
docker tag botticelli-server testuser/103-252-118-172.cloud-xip.com:5000
docker push testuser/103-252-118-172.cloud-xip.com:5000
