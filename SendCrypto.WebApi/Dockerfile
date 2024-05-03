#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SendCrypto.WebApi/SendCrypto.WebApi.csproj", "SendCrypto.WebApi/"]
COPY ["SendCrypto.Application/SendCrypto.Application.csproj", "SendCrypto.Application/"]
COPY ["SendCrypto.Domain/SendCrypto.Domain.csproj", "SendCrypto.Domain/"]
RUN dotnet restore "SendCrypto.WebApi/SendCrypto.WebApi.csproj"
COPY . .
WORKDIR "/src/SendCrypto.WebApi"
RUN dotnet build "SendCrypto.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SendCrypto.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SendCrypto.WebApi.dll"]