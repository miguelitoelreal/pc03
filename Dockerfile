# Dockerfile para desplegar NewsPortal en Render
# Usa la imagen oficial de .NET para build y runtime

# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore "NewsPortal.csproj"
RUN dotnet publish "NewsPortal.csproj" -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Puerto por defecto para ASP.NET Core
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "NewsPortal.dll"]
