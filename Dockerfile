# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy csproj and restore dependencies
COPY ["ALMACENHV/ALMACENHV.csproj", "./"]
RUN dotnet restore

# Copy the rest of the code
COPY . .
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Environment variables
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ=America/Lima

EXPOSE ${PORT}
ENTRYPOINT ["dotnet", "ALMACENHV.dll"]
