# Built by ProgressPlayDevTeam/Pipelines build.yml, which passes only a context, so this file must
# stay at the repo root.

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY PP.BOGateway/PP.BOGateway.csproj PP.BOGateway/
RUN dotnet restore PP.BOGateway/PP.BOGateway.csproj
COPY . .
RUN dotnet publish PP.BOGateway/PP.BOGateway.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bookworm-slim-amd64
WORKDIR /app
COPY --from=build /app/publish .

# generic-chart sets containerPort 8080; the aspnet:7.0 image defaults to 80, so set it here.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "PP.BOGateway.dll"]
