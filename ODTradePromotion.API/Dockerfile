#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ENV TZ="Asia/Ho_Chi_Minh"
RUN apt-get update -y && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ODTradePromotion.API/ODTradePromotion.API.csproj", "ODTradePromotion.API/"]
COPY ["Sys.Common/Sys.Common.csproj", "Sys.Common/"]
RUN dotnet restore "ODTradePromotion.API/ODTradePromotion.API.csproj"
COPY . .
WORKDIR "/src/ODTradePromotion.API"
RUN dotnet build "ODTradePromotion.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ODTradePromotion.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ODTradePromotion.API.dll"]