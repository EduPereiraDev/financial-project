# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["FinancialControl.API/FinancialControl.API.csproj", "FinancialControl.API/"]
RUN dotnet restore "FinancialControl.API/FinancialControl.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/FinancialControl.API"
RUN dotnet build "FinancialControl.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "FinancialControl.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Copy published app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "FinancialControl.API.dll"]
