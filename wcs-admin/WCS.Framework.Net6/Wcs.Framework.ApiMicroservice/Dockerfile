#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 19001

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Wcs.Framework.ApiMicroservice/Wcs.Framework.ApiMicroservice.csproj", "Wcs.Framework.ApiMicroservice/"]
COPY ["Wcs.Framework.WebCore/Wcs.Framework.WebCore.csproj", "Wcs.Framework.WebCore/"]
COPY ["Wcs.Framework.Core/Wcs.Framework.Core.csproj", "Wcs.Framework.Core/"]
COPY ["Wcs.Framework.Task/Wcs.Framework.Job.csproj", "Wcs.Framework.Task/"]
COPY ["Wcs.Framework.Model/Wcs.Framework.Model.csproj", "Wcs.Framework.Model/"]
COPY ["Wcs.Framework.Common/Wcs.Framework.Common.csproj", "Wcs.Framework.Common/"]
COPY ["Wcs.Framework.Language/Wcs.Framework.Language.csproj", "Wcs.Framework.Language/"]
COPY ["Wcs.Framework.Service/Wcs.Framework.Service.csproj", "Wcs.Framework.Service/"]
COPY ["Wcs.Framework.Interface/Wcs.Framework.Interface.csproj", "Wcs.Framework.Interface/"]
COPY ["Wcs.Framework.DTOModel/Wcs.Framework.DTOModel.csproj", "Wcs.Framework.DTOModel/"]
COPY ["Wcs.Framework.Repository/Wcs.Framework.Repository.csproj", "Wcs.Framework.Repository/"]
RUN dotnet restore "Wcs.Framework.ApiMicroservice/Wcs.Framework.ApiMicroservice.csproj"
COPY . .
WORKDIR "/src/Wcs.Framework.ApiMicroservice"
RUN dotnet build "Wcs.Framework.ApiMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wcs.Framework.ApiMicroservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wcs.Framework.ApiMicroservice.dll"]
