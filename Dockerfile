# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restaurar dependencias
COPY ["ALMACENHV/ALMACENHV.csproj", "ALMACENHV/"]
RUN dotnet restore "ALMACENHV/ALMACENHV.csproj"

# Copiar y publicar
COPY . .
WORKDIR "/src/ALMACENHV"
RUN dotnet publish "ALMACENHV.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Configuración de la aplicación
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ=America/Lima
ENV PORT=10000

# Health check
HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost:${PORT}/health || exit 1

EXPOSE ${PORT}
ENTRYPOINT ["dotnet", "ALMACENHV.dll"]
