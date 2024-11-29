# Usar la imagen SDK como base para la compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo los archivos de proyecto primero y restaurar las dependencias
COPY ["ALMACENHV/ALMACENHV.csproj", "ALMACENHV/"]
RUN dotnet restore "ALMACENHV/ALMACENHV.csproj"

# Copiar el resto del código y compilar
COPY . .
WORKDIR "/src/ALMACENHV"
RUN dotnet build "ALMACENHV.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "ALMACENHV.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instalar el certificado HTTPS de desarrollo
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

# Copiar los archivos publicados
COPY --from=publish /app/publish .

# Pre-JIT compilation para mejorar el arranque
RUN apt-get update \
    && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

# Configurar el punto de entrada
ENTRYPOINT ["dotnet", "ALMACENHV.dll"]
