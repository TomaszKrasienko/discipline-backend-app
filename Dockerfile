FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY discipline-backed-app/src/discipline.application/discipline.application.csproj ./discipline.application/
COPY discipline-backed-app/src/discipline.api/discipline.api.csproj ./discipline.api/
RUN dotnet restore ./discipline.api/discipline.api.csproj -s https://api.nuget.org/v3/index.json 
COPY . ./
RUN dotnet publish ./discipline-backed-app/discipline-backed-app.sln -c Release -o out

RUN dotnet test ./discipline-backed-app/tests/discipline.application.unit-tests

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "discipline.api.dll"]

ENV ASPNETCORE_ENVIRONMENT="docker"
ENV TZ="Europe/Warsaw"
EXPOSE 80
EXPOSE 443