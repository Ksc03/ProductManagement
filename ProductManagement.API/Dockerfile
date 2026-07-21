# ===========================
# Build Stage
# ===========================

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project files first (better Docker layer caching)
COPY ["ProductManagement.API/ProductManagement.API.csproj", "ProductManagement.API/"]
COPY ["ProductManagement.Application/ProductManagement.Application.csproj", "ProductManagement.Application/"]
COPY ["ProductManagement.Domain/ProductManagement.Domain.csproj", "ProductManagement.Domain/"]
COPY ["ProductManagement.Infrastructure/ProductManagement.Infrastructure.csproj", "ProductManagement.Infrastructure/"]

RUN dotnet restore "ProductManagement.API/ProductManagement.API.csproj"

# Copy the remaining source code
COPY . .

WORKDIR "/src/ProductManagement.API"

RUN dotnet publish "ProductManagement.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ===========================
# Runtime Stage
# ===========================

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ProductManagement.API.dll"]