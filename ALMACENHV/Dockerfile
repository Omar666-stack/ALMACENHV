# Usar la imagen base de SDK para compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY ["ALMACENHV/ALMACENHV.csproj", "ALMACENHV/"]
RUN dotnet restore "ALMACENHV/ALMACENHV.csproj"

# Copiar el resto del código
COPY . .

# Compilar la aplicación
WORKDIR "/src/ALMACENHV"
RUN dotnet build "ALMACENHV.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "ALMACENHV.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configurar variables de entorno
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV ASPNETCORE_ENVIRONMENT=Production

# Punto de entrada
ENTRYPOINT ["dotnet", "ALMACENHV.dll"]
