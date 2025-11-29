FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Pharmacy.REST/Pharmacy.REST.csproj", "Pharmacy.REST/"]
COPY ["PharmacyApp.Infrastructure/PharmacyApp.Infrastructure.csproj", "PharmacyApp.Infrastructure/"]
COPY ["PharmacyApp.Common/PharmacyApp.Common.csproj", "PharmacyApp.Common/"]

RUN dotnet restore "Pharmacy.REST/Pharmacy.REST.csproj"

COPY . .

WORKDIR "/src/Pharmacy.REST"
RUN dotnet publish "Pharmacy.REST.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p /app/Data && chmod 777 /app/Data

ENTRYPOINT ["dotnet", "Pharmacy.REST.dll"]