@echo off
echo Executando docker-compose -f docker-compose.yml up -d ...
docker-compose -f docker-compose.yml -p bufunfa up -d
echo.
pause