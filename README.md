# amazon-asp

Create docker hub repository - publish
```
docker build -t amazon-asp-api . 
docker run -it --rm -p 6892:8080 --name amazon-asp_container amazon-asp-api
docker run -d --restart=always --name amazon-asp_container -p 6892:8080 amazon-asp-api
docker run -d --restart=always -v d:/volumes/amazon-asp/uploading:/app/uploading --name amazon-asp_container -p 6892:8080 amazon-asp-api
docker run -d --restart=always -v /volumes/amazon-asp/uploading:/app/uploading --name amazon-asp_container -p 6892:8080 amazon-asp-api
docker ps -a
docker stop amazon-asp_container
docker rm amazon-asp_container

docker uploading --all
docker rmi amazon-asp-api

docker login
docker tag amazon-asp-api:latest sashok9203/amazon-asp-api:latest
docker push sashok9203/amazon-asp-api:latest

docker pull sashok9203/amazon-asp-api:latest
docker ps -a
docker run -d --restart=always --name amazon-asp_container -p 6892:8080 sashok9203/amazon-asp-api

docker run -d --restart=always -v /volumes/amazon-asp/uploading:/app/uploading --name amazon-asp_container -p 6892:8080 sashok9203/amazon-asp-api


docker pull sashok9203/amazon-asp-api:latest
docker uploading --all
docker ps -a
docker stop amazon-asp_container
docker rm amazon-asp_container
docker run -d --restart=always --name amazon-asp_container -p 6892:8080 sashok9203/amazon-asp-api
```

```nginx options /etc/nginx/sites-available/default
server {
    server_name   amazonapi.itstep.click *.amazonapi.itstep.click;
    location / {
       proxy_pass         http://localhost:6892;
       proxy_http_version 1.1;
       proxy_set_header   Upgrade $http_upgrade;
       proxy_set_header   Connection keep-alive;
       proxy_set_header   Host $host;
       proxy_cache_bypass $http_upgrade;
       proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
       proxy_set_header   X-Forwarded-Proto $scheme;
    }
    location /hub {
        proxy_pass http://localhost:6892; # Replace with your SignalR server address
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;

        # Optional headers for better handling of websockets
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;

        # Increase buffer and timeout settings
        proxy_buffering off;
        proxy_read_timeout 60s;
        proxy_send_timeout 60s;
        proxy_connect_timeout 60s;
    }
}


sudo systemctl restart nginx
certbot
```
