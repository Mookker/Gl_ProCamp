FROM microsoft/dotnet:2.2.1-aspnetcore-runtime

EXPOSE 6003/tcp
ENV ASPNETCORE_URLS http://*:6003

RUN mkdir -p /app
WORKDIR /app
COPY build/ /app

ENTRYPOINT ["dotnet", "BettingApi.dll"]
