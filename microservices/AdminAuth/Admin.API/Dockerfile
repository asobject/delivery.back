# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["microservices/AdminAuth/Admin.API/Admin.API.csproj", "microservices/AdminAuth/Admin.API/"]
COPY ["microservices/AdminAuth/Infrastructure/Infrastructure.csproj", "microservices/AdminAuth/Infrastructure/"]
COPY ["buildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "buildingBlocks/BuildingBlocks.Messaging/"]
COPY ["microservices/AdminAuth/Application/Application.csproj", "microservices/AdminAuth/Application/"]
COPY ["buildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "buildingBlocks/BuildingBlocks/"]
COPY ["microservices/AdminAuth/Domain/Domain.csproj", "microservices/AdminAuth/Domain/"]
RUN dotnet restore "./microservices/AdminAuth/Admin.API/Admin.API.csproj"
COPY . .
WORKDIR "/src/microservices/AdminAuth/Admin.API"
RUN dotnet build "./Admin.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Admin.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Admin.API.dll"]