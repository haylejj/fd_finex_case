# Mini Gorev Yonetimi API + Arayuzu

Bu proje, gorev (todo) olusturma, listeleme, guncelleme ve silme islemlerini yapan bir mini uygulamadir.

## Teknoloji Tercihleri ve Kisa Gerekceler

- **.NET 8 Web API**: .Net 8 sürümü uzum kapsamlı destek aldığı için seçildi.
- **Katmanli Mimari (API, Application, Domain, Infrastructure)**: Sorumluluklari ayirmak, test edilebilirligi ve bakimi kolaylastirmak icin kullanildi.
- **Entity Framework Core + SQL Server**: Veritabani islemlerini tip guvenli ve hizli yonetmek icin tercih edildi.
- **FluentValidation**: Request dogrulamalarini merkezi, okunabilir ve yeniden kullanilabilir yapmak icin secildi.
- **AutoMapper**: DTO-Entity donusumlerini sade ve tekrarsiz hale getirmek icin kullanildi.
- **Frontend: React**: Bilesen tabanli, hizli ve yaygin oldugu icin arayuz tarafinda tercih edildi.

## Proje Yapisi

- `WEBAPI`: API katmani (controller, middleware, Program)
- `WEBAPI.Application`: DTO, service contract, validation, mapping
- `WEBAPI.Domain`: Entity modelleri
- `WEBAPI.Infrastructure`: EF Core DbContext, service implementasyonlari

## Backend Kurulum ve Calistirma

### 1) Gereksinimler

- .NET 8 SDK
- SQL Server (localdb veya SQL Server instance)
- Visual Studio 2022 veya `dotnet` CLI

### 2) Connection String Ayari

`WEBAPI/appsettings.Development.json` dosyasinda `ConnectionStrings:DefaultConnection` degerini kendi ortamina gore guncellemek lazım.:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=DB_fd_finex_case;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
- Windows Authentication kullaniyorsanız `Trusted_Connection=True` ile girin.
- SQL login kullaniyorsan `User Id=...;Password=...;` eklenmeli ve `Trusted_Connection=True` kaldirilmalidir.

### 3) Migration ve Veritabani Guncelleme

#### Package Manager Console

```powershell
Add-Migration InitialCreate -Project WEBAPI.Infrastructure -StartupProject WEBAPI -OutputDir Migrations
Update-Database -Project WEBAPI.Infrastructure -StartupProject WEBAPI
```

#### dotnet CLI (alternatif)

```bash
dotnet ef migrations add InitialCreate -p WEBAPI.Infrastructure -s WEBAPI -o Migrations
dotnet ef database update -p WEBAPI.Infrastructure -s WEBAPI
```

### 4) Uygulamayi Calistirma

Solution acildiktan sonra `WEBAPI` startup project olarak secilir ve proje calistirilir.

CLI ile:

```bash
dotnet run --project WEBAPI
```

Swagger uzerinden endpointler test edilebilir.

## Notlar

- API tarafinda CORS ayarlari eklidir ve ortama göre değiştirilebilir.
- Global exception middleware ve request logging middleware aktiftir.
- `CreatedAt` alani `DbContext.SaveChangesAsync` ile otomatik oluşturulur.
