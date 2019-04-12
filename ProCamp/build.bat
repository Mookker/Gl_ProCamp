docker kill int_authapi
docker kill int_fixturesapi
docker kill int_bettingapi

docker rm int_authapi
docker rm int_fixturesapi
docker rm int_bettingapi

docker rmi authapi
docker rmi fixturesapi
docker rmi bettingapi

docker kill local-redis
docker rm local-redis

docker run --name local-redis --network=gl-procamp --ip=192.168.101.4 -p 6379 -d redis

cd AuthApi
del build /F /Q
dotnet restore && dotnet publish -c Release -o build && docker build -t authapi .
docker run -p 6001:6001 -d --network=gl-procamp --ip=192.168.101.1 --dns 8.8.8.8 --name int_authapi authapi
cd ../FixturesApi
del build /F /Q
dotnet restore && dotnet publish -c Release -o build && docker build -t fixturesapi .
docker run -p 6002:6002 -d --network=gl-procamp --ip=192.168.101.2  --dns 8.8.8.8 --name int_fixturesapi fixturesapi -redis 192.168.101.4:6379
cd ../BettingApi
del build /F /Q
dotnet restore && dotnet publish -c Release -o build && docker build -t bettingapi .
docker run -p 6003:6003 -d --network=gl-procamp --ip=192.168.101.3  --name int_bettingapi bettingapi -fixtures "http://192.168.101.2:6002/" -auth http://192.168.101.1:6001/ 

cd ..