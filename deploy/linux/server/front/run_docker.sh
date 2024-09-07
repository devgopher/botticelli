sudo docker run -d -t -i -p 5042:5000 -p 7247:5001 \
-e ASPNETCORE_URLS="https://0.0.0.0:5001;http://0.0.0.0:5000" \
-e ASPNETCORE_ENVIRONMENT="Release" \
-e BackSettings__BackUrl="$back_url" \
-e BackSettings__AnalyticsUrl="$analytics_url" \
103-252-118-172.cloud-xip.com:5000/botticelli-server-front:0.6

