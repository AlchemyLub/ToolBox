FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY Directory.Build.props ./
COPY ["src/FloralHub.Arch.Api/FloralHub.Arch.Api.csproj", "FloralHub.Arch.Api/"]
COPY ["src/FloralHub.Arch.Core/FloralHub.Arch.Core.csproj", "FloralHub.Arch.Core/"]
RUN dotnet restore "./FloralHub.Arch.Api/FloralHub.Arch.Api.csproj"

COPY . .
WORKDIR "/src/src/FloralHub.Arch.Api"
RUN dotnet build "./FloralHub.Arch.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./FloralHub.Arch.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FloralHub.Arch.Api.dll"]
