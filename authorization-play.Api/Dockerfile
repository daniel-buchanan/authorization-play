#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["authorization-play.Api/authorization-play.Api.csproj", "authorization-play.Api/"]
COPY ["authorization-play.Core/authorization-play.Core.csproj", "authorization-play.Core/"]
RUN dotnet restore "authorization-play.Api/authorization-play.Api.csproj"
COPY . .
WORKDIR "/src/authorization-play.Api"
RUN dotnet build "authorization-play.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "authorization-play.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "authorization-play.Api.dll"]
