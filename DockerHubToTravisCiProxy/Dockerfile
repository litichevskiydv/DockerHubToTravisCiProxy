FROM microsoft/dotnet:1.1-runtime
LABEL name="DockerHubToTravisCiProxy"
WORKDIR /app
COPY . ./
ENV ASPNETCORE_URLS http://*:80
EXPOSE 80
ENTRYPOINT ["dotnet", "DockerHubToTravisCiProxy.dll"]
