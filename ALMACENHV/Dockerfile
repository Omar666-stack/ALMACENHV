# Usar la imagen base de SDK para compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0.100 AS build
WORKDIR /app

# Copiar csproj y restaurar dependencias
COPY ["ALMACENHV/ALMACENHV.csproj", "./"]
RUN dotnet restore

# Copiar el resto del código y compilar
COPY . .
RUN dotnet build -c Release -o out

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configurar variables de entorno
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# Punto de entrada
ENTRYPOINT ["dotnet", "ALMACENHV.dll"]
