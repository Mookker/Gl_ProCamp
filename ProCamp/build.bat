docker kill int_autapi
docker kill int_fixturesapi
docker kill int_bettingapi

docker rm int_autapi
docker rm int_fixturesapi
docker rm int_bettingapi

docker rmi authapi
docker rmi fixturesapi
docker rmi bettingapi

cd AuthApi && dotnet restore && dotnet publish -c Release -o build && docker build -t authapi .
docker run -p 6001:6001 -d --network=gl-procamp --ip=192.168.101.1 --name int_autapi authapi
cd ../FixturesApi
dotnet restore && dotnet publish -c Release -o build && docker build -t fixturesapi .
docker run -p 6002:6002 -d --network=gl-procamp --ip=192.168.101.2 --name int_fixturesapi fixturesapi
cd ../BettingApi
dotnet restore && dotnet publish -c Release -o build && docker build -t bettingapi .
docker run -p 6003:6003 -d --network=gl-procamp --ip=192.168.101.3 --name int_bettingapi bettingapi

cd ..