#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# --platform=linux/amd64
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5005

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["wcs-console-server/wcs-console-server.csproj", "wcs-console-server/"]
COPY ["wcs.core/wcs.core.csproj", "wcs.core/"]
COPY ["wcs.webapi/wcs.webapi.csproj", "wcs.webapi/"]
RUN dotnet restore "wcs-console-server/wcs-console-server.csproj"
COPY . .
WORKDIR "/src/wcs-console-server"
RUN dotnet build "wcs-console-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "wcs-console-server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "wcs-console-server.dll"]
