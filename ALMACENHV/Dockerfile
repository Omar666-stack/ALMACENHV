# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restaurar dependencias
COPY ["ALMACENHV/ALMACENHV.csproj", "./"]
RUN dotnet restore "ALMACENHV.csproj"

# Copiar y publicar
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Configuración de la aplicación
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ=America/Lima

# Health check más simple
HEALTHCHECK --interval=30s --timeout=3s \
    CMD wget -qO- http://localhost:${PORT}/health || exit 1

EXPOSE ${PORT}
ENTRYPOINT ["dotnet", "ALMACENHV.dll"]
