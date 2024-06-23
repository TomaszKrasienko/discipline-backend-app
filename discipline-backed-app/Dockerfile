FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY src/discipline.application/discipline.application.csproj ./discipline.application/
COPY src/discipline.api/discipline.api.csproj ./discipline.api/
RUN dotnet restore ./discipline.api/discipline.api.csproj -s https://api.nuget.org/v3/index.json 
COPY . ./
RUN dotnet publish ./discipline-backed-app.sln -c Release -o out

RUN dotnet test tests/discipline.application.unit-tests
# //RUN dotnet test tests/discipline.api.integration-tests

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "discipline.api.dll"]

ENV ASPNETCORE_ENVIRONMENT="docker"
ENV TZ="Europe/Warsaw"
EXPOSE 80
EXPOSE 443