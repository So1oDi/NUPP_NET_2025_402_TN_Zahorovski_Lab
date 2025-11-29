FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["Pharmacy.REST/Pharmacy.REST.csproj", "Pharmacy.REST/"]
COPY ["PharmacyApp.Infrastructure/PharmacyApp.Infrastructure.csproj", "PharmacyApp.Infrastructure/"]
COPY ["PharmacyApp.Common/PharmacyApp.Common.csproj", "PharmacyApp.Common/"]

# Restore dependencies
RUN dotnet restore "Pharmacy.REST/Pharmacy.REST.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/Pharmacy.REST"
RUN dotnet publish "Pharmacy.REST.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Pharmacy.REST.dll"]