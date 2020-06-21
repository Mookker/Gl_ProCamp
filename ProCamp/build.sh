docker kill int_authapi
docker kill int_fixturesapi
docker kill int_bettingapi

docker rm int_authapi
docker rm int_fixturesapi
docker rm int_bettingapi

docker rmi authapi:oldone
docker rmi fixturesapi:oldone
docker rmi bettingapi:oldone

docker tag authapi:latest authapi:oldone
docker tag fixturesapi fixturesapi:oldone
docker tag bettingapi bettingapi:oldone

docker kill local-redis
docker rm local-redis

docker run --name local-redis --network=gl-procamp --ip=192.168.101.4 -p 6379 -d redis

cd AuthApi
rm -rf build
dotnet restore && dotnet publish -c Release -o build && docker build -t authapi:latest .
docker run -p 6001:6001 -d --network=gl-procamp --ip=192.168.101.1 --dns 8.8.8.8 --name int_authapi authapi:latest
cd ../FixturesApi
rm -rf build
dotnet restore && dotnet publish -c Release -o build && docker build -t fixturesapi:latest .
docker run -p 6002:6002 -d --network=gl-procamp --ip=192.168.101.2  --dns 8.8.8.8 --name int_fixturesapi fixturesapi:latest -redis 192.168.101.4:6379
cd ../BettingApi
rm -rf build
dotnet restore && dotnet publish -c Release -o build && docker build -t bettingapi:latest .
docker run -p 6003:6003 -d --network=gl-procamp --ip=192.168.101.3  --name int_bettingapi bettingapi:latest -fixtures "http://192.168.101.2:6002/" -auth http://192.168.101.1:6001/ 

cd ..