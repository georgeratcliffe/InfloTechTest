FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src


COPY Directory.Build.props ./

COPY UserManagement.Services/. /src/UserManagement.Services
COPY UserManagement.Shared/. /src/UserManagement.Shared
COPY UserManagement.Data/. /src/UserManagement.Data/
COPY UserManagement.Web/. /src/UserManagement


RUN dotnet restore "UserManagement"

WORKDIR "/src/UserManagement"

RUN dotnet build "UserManagement.Web.csproj" -c $BUILD_CONFIGURATION -o app/build

FROM build AS publish
WORKDIR "/src/UserManagement"
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserManagement.Web.csproj" -c $BUILD_CONFIGURATION -o app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /src/UserManagement/app/publish .
ENTRYPOINT [ "dotnet","UserManagement.Web.dll" ]
