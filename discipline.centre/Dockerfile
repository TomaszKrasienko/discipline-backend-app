FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

ARG BUILD_CONFIGURATION=Release

# Central Management files
COPY src/Directory.Build.props ./
COPY src/Directory.Packages.props ./

# ActivityRules module
COPY src/Modules/ActivityRules/discipline.centre.activityrules.api/discipline.centre.activityrules.api.csproj ./discipline.centre.activityrules.api/
COPY src/Modules/ActivityRules/discipline.centre.activityrules.application/discipline.centre.activityrules.application.csproj ./discipline.centre.activityrules.application/
COPY src/Modules/ActivityRules/discipline.centre.activityrules.domain/discipline.centre.activityrules.domain.csproj ./discipline.centre.activityrules.domain/
COPY src/Modules/ActivityRules/discipline.centre.activityrules.infrastructure/discipline.centre.activityrules.infrastructure.csproj ./discipline.centre.activityrules.infrastructure/

# DailyTrackers module
COPY src/Modules/DailyTrackers/discipline.centre.dailytrackers.api/discipline.centre.dailytrackers.api.csproj ./discipline.centre.dailytrackers.api/
COPY src/Modules/DailyTrackers/discipline.centre.dailytrackers.application/discipline.centre.dailytrackers.application.csproj ./discipline.centre.dailytrackers.application/
COPY src/Modules/DailyTrackers/discipline.centre.dailytrackers.domain/discipline.centre.dailytrackers.domain.csproj ./discipline.centre.dailytrackers.domain/
COPY src/Modules/DailyTrackers/discipline.centre.dailytrackers.infrastructure/discipline.centre.dailytrackers.infrastructure.csproj ./discipline.centre.dailytrackers.infrastructure/

# UsersModule module
COPY src/Modules/Users/discipline.centre.users.api/discipline.centre.users.api.csproj ./discipline.centre.users.api/
COPY src/Modules/Users/discipline.centre.users.application/discipline.centre.users.application.csproj ./discipline.centre.users.application/
COPY src/Modules/Users/discipline.centre.users.domain/discipline.centre.users.domain.csproj ./discipline.centre.users.domain/
COPY src/Modules/Users/discipline.centre.users.infrastructure/discipline.centre.users.infrastructure.csproj ./discipline.centre.users.infrastructure/

# Shared module
COPY src/Shared/discipline.centre.shared.abstractions/discipline.centre.shared.abstractions.csproj ./discipline.centre.shared.abstractions/ 
COPY src/Shared/discipline.centre.shared.infrastructure/discipline.centre.shared.infrastructure.csproj ./discipline.centre.shared.infrastructure/

# Bootstrappers
COPY src/Bootstrappers/discipline.centre.bootstrappers.api/discipline.centre.bootstrappers.api.csproj ./discipline.centre.bootstrappers.api/

RUN dotnet restore ./discipline.centre.bootstrappers.api/discipline.centre.bootstrappers.api.csproj -s https://api.nuget.org/v3/index.json  --verbosity detailed
COPY . ./
RUN dotnet publish ./discipline.centre.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "discipline.centre.bootstrappers.api.dll"]

ENV ASPNETCORE_ENVIRONMENT="docker"
ENV TZ="Europe/Warsaw"
EXPOSE 80
EXPOSE 443