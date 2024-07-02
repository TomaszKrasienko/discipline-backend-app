FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

RUN ls -la /app
# Kopiowanie plików projektów i ich przywracanie
COPY src/discipline.application/discipline.application.csproj ./discipline.application/
RUN ls -la /app/discipline.application

COPY src/discipline.api/discipline.api.csproj ./discipline.api/
RUN ls -la /app/discipline.application

RUN dotnet restore ./discipline.api/discipline.api.csproj -s https://api.nuget.org/v3/index.json 

# Kopiowanie całego kodu źródłowego i budowanie
COPY . ./
RUN dotnet publish ./discipline-backed-app.sln -c Release -o out

# Uruchomienie testów
RUN dotnet test tests/discipline.application.unit-tests
# RUN dotnet test -e DOTNET_ENVIRONMENT="docker.tests" tests/discipline.api.integration-tests

# Budowanie obrazu runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Ustawienie zmiennych środowiskowych
ENV ASPNETCORE_ENVIRONMENT="docker"
ENV TZ="Europe/Warsaw"

# Otwieranie portów
EXPOSE 80
EXPOSE 443

# Ustawienie punktu wejścia
ENTRYPOINT ["dotnet", "discipline.api.dll"]
