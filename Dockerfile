FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ALMACENHV/ALMACENHV.csproj", "ALMACENHV/"]
RUN dotnet restore "ALMACENHV/ALMACENHV.csproj"
COPY . .
WORKDIR "/src/ALMACENHV"
RUN dotnet build "ALMACENHV.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ALMACENHV.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_URLS=http://+:80
ENTRYPOINT ["dotnet", "ALMACENHV.dll"]
