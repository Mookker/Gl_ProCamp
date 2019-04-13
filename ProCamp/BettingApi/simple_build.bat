dotnet restore && dotnet publish -c Release -o build && docker build -t bettingapi:simple .
docker run -p 26003:6003 -d --name simple_betting bettingapi:simple