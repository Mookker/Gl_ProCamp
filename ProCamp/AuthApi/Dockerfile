FROM microsoft/dotnet:2.2.1-aspnetcore-runtime

EXPOSE 6001/tcp
ENV ASPNETCORE_URLS http://*:6001

RUN mkdir -p /app
WORKDIR /app
COPY build/ /app

ENTRYPOINT ["dotnet", "AuthApi.dll"]
