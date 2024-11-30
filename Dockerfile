# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["ALMACENHV/ALMACENHV/ALMACENHV.csproj", "ALMACENHV/ALMACENHV/"]
RUN dotnet restore "ALMACENHV/ALMACENHV/ALMACENHV.csproj"

COPY . .
RUN dotnet publish "ALMACENHV/ALMACENHV/ALMACENHV.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE ${PORT}

ENTRYPOINT ["dotnet", "ALMACENHV.dll"]