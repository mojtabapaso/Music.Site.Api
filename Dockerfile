# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src
# COPY ["Music/Music.csproj", "Music/"]
# RUN dotnet restore "./Music/./Music.csproj"
# COPY . .
# WORKDIR "/src/Music"
# RUN dotnet build "./Music.csproj" -c $BUILD_CONFIGURATION -o /app/build

# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "./Music.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "Music.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Music.Presentation/Music.Presentation.csproj", "Music.Presentation/"]
COPY ["Music.Application/Music.Application.csproj", "Music.Application/"]
COPY ["Music.Domain/Music.Domain.csproj", "Music.Domain/"]
COPY ["Music.Infrastructure/Music.Infrastructure.csproj", "Music.Infrastructure/"]


RUN dotnet restore "Music.Presentation/Music.Presentation.csproj"

COPY . .

# Build 
WORKDIR "/src/Music.Presentation"
RUN dotnet build "Music.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# Publish
FROM build AS publish

RUN dotnet publish "Music.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish # /p:UseAppHost=false --no-build

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Music.Presentation.dll"]