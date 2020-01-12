@echo off
echo Executando docker-compose -f docker-compose-mysql.yml -f docker-compose.yml down ...
docker-compose -f docker-compose-mysql.yml -f docker-compose.yml down
echo.
pause