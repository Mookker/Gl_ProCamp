FROM microsoft/dotnet:2.2.1-aspnetcore-runtime

EXPOSE 6002/tcp
ENV ASPNETCORE_URLS http://*:6002

RUN mkdir -p /app
WORKDIR /app
COPY build/ /app

ENTRYPOINT ["dotnet", "FixturesApi.dll"]
