FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy everything
COPY . .
RUN dotnet restore "./ALMACENHV/ALMACENHV.csproj"
RUN dotnet publish "./ALMACENHV/ALMACENHV.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE ${PORT}

ENTRYPOINT ["dotnet", "ALMACENHV.dll"]