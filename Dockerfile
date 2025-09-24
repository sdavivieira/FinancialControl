# ============================================
# Fase base (runtime)
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Ajustar permissões para o usuário app existente
RUN chown -R app:app /app

# ============================================
# Fase build
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar csproj e restaurar dependências
COPY ["FinancialControl/FinancialControl.csproj", "./"]
RUN dotnet restore "./FinancialControl.csproj" --verbosity minimal

# Copiar o restante do código
COPY . .

# Build do projeto com log detalhado
RUN dotnet build "./FinancialControl.csproj" -c $BUILD_CONFIGURATION -o /app/build /v:diag

# ============================================
# Publicação
# ============================================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FinancialControl.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /v:diag

# ============================================
# Fase final (runtime)
# ============================================
FROM base AS final
WORKDIR /app

# Copiar publicação
COPY --from=publish /app/publish .

# Rodar como usuário app (segurança)
USER app

# Configuração do ASP.NET
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "FinancialControl.dll"]
