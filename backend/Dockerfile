# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["FinancialControl.Api/FinancialControl.Api.csproj", "FinancialControl.Api/"]
RUN dotnet restore "FinancialControl.Api/FinancialControl.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/FinancialControl.Api"
RUN dotnet build "FinancialControl.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "FinancialControl.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Copy published app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "FinancialControl.Api.dll"]
