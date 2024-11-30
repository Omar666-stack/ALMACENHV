# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["ALMACENHV/ALMACENHV.csproj", "ALMACENHV/"]
RUN dotnet restore "ALMACENHV/ALMACENHV.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "ALMACENHV/ALMACENHV.csproj" -c Release -o /app/build
RUN dotnet publish "ALMACENHV/ALMACENHV.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE ${PORT}

ENTRYPOINT ["dotnet", "ALMACENHV.dll"]