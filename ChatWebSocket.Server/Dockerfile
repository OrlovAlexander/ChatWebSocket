FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChatWebSocket.Server/ChatWebSocket.Server.csproj", "ChatWebSocket.Server/"]
RUN dotnet restore "ChatWebSocket.Server/ChatWebSocket.Server.csproj"
COPY . .
WORKDIR "/src/ChatWebSocket.Server"
RUN dotnet build "ChatWebSocket.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChatWebSocket.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 9006
ENTRYPOINT ["dotnet", "ChatWebSocket.Server.dll"]
