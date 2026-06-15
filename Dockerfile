# .NET SDK ortamını hazırla
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Backend klasörünü Docker'ın içine kopyala
COPY Backend/ .

# İç içe geçmiş doğru proje klasörünün içine gir
WORKDIR /app/PetPulse.API/PetPulse.API

# Bulunduğun yerdeki projeyi derle ve ana 'out' klasörüne çıkart
RUN dotnet publish -c Release -o /app/out

# Uygulamayı çalıştırmak için hafif sürümü ayarla
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Projeyi ayağa kaldır
ENTRYPOINT ["dotnet", "PetPulse.API.dll"]