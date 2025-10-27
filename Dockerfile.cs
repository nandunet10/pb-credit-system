# ========================================
# src/Services/CustomerService/PB.CustomerService.API/Dockerfile
# ========================================

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto
COPY ["src/Services/CustomerService/PB.CustomerService.API/PB.CustomerService.API.csproj", "src/Services/CustomerService/PB.CustomerService.API/"]
COPY ["src/Services/CustomerService/PB.CustomerService.Application/PB.CustomerService.Application.csproj", "src/Services/CustomerService/PB.CustomerService.Application/"]
COPY ["src/Services/CustomerService/PB.CustomerService.Domain/PB.CustomerService.Domain.csproj", "src/Services/CustomerService/PB.CustomerService.Domain/"]
COPY ["src/Services/CustomerService/PB.CustomerService.Infrastructure/PB.CustomerService.Infrastructure.csproj", "src/Services/CustomerService/PB.CustomerService.Infrastructure/"]
COPY ["src/Shared/PB.Shared.Core/PB.Shared.Core.csproj", "src/Shared/PB.Shared.Core/"]
COPY ["src/Shared/PB.Shared.Messaging/PB.Shared.Messaging.csproj", "src/Shared/PB.Shared.Messaging/"]

# Restaurar dependências
RUN dotnet restore "src/Services/CustomerService/PB.CustomerService.API/PB.CustomerService.API.csproj"

# Copiar todo o código
COPY . .

WORKDIR "/src/src/Services/CustomerService/PB.CustomerService.API"
RUN dotnet build "PB.CustomerService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PB.CustomerService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PB.CustomerService.API.dll"]

# ========================================
# TESTES UNITÁRIOS
# ========================================