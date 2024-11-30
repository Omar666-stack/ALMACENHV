# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything
COPY . ./
RUN dotnet restore "./ALMACENHV/ALMACENHV.csproj"
RUN dotnet publish "./ALMACENHV/ALMACENHV.csproj" -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Environment variables
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ=America/Lima

EXPOSE ${PORT}
ENTRYPOINT ["dotnet", "ALMACENHV.dll"]
