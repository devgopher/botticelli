docker build -t 103-252-118-172.cloud-xip.com:5000/botticelli-server:0.4-dev . --no-cache
docker login 103-252-118-172.cloud-xip.com:5000/botticelli-server
docker tag 103-252-118-172.cloud-xip.com:5000/botticelli-server:0.4-dev testuser/103-252-118-172.cloud-xip.com:5000
docker push 103-252-118-172.cloud-xip.com:5000/botticelli-server:0.4-dev
