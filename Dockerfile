# .NET SDK kullanarak uygulamayı derle
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Backend klasörünün içindeki her şeyi Docker'ın içine kopyala
COPY Backend/ .

# Derleme işlemini yap
RUN dotnet publish -c Release -o out

# Çalıştırma ortamını hazırla
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Uygulamanın çalışacağı port ve komut
ENTRYPOINT ["dotnet", "PetPulse.API.dll"]