@echo off
echo Executando docker-compose -f docker-compose-mysql.yml -f docker-compose.yml up -d ...
docker-compose -f docker-compose-mysql.yml -f docker-compose.yml up -d
echo.
pause