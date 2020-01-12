@echo off
echo Executando docker-compose -f docker-compose-mysql.yml up -d ...
docker-compose -f docker-compose-mysql.yml up -d
docker exec -it dbServer sh