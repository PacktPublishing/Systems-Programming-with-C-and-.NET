#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Chapter 13/WorkerForDocker/13_WorkerForDocker.csproj", "Chapter 13/WorkerForDocker/"]
RUN dotnet restore "./Chapter 13/WorkerForDocker/WorkerForDocker.csproj"
COPY . .
WORKDIR "/src/Chapter 13/WorkerForDocker"
RUN dotnet build "./WorkerForDocker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WorkerForDocker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerForDocker.dll"]