using MongoDB.Driver;
using PetPulse.API.Models;

namespace PetPulse.API.Services
{
    public class PetService
    {
        private readonly IMongoCollection<Pet> _petsCollection;

        public PetService(IConfiguration config)
        {
            // Veritabanı bağlantı adresini appsettings.json'dan alıyoruz
            var connectionString = config.GetSection("MongoDBSettings:ConnectionString").Value;
            var databaseName = config.GetSection("MongoDBSettings:DatabaseName").Value;

            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);

            // "Pets" adında bir koleksiyon (tablo) kullanacağımızı belirtiyoruz
            _petsCollection = mongoDatabase.GetCollection<Pet>("Pets");
        }

        // Tüm evcil hayvanları getirir (Read)
        public async Task<List<Pet>> GetAsync() =>
            await _petsCollection.Find(_ => true).ToListAsync();

        // Sadece ID'si verilen spesifik bir evcil hayvanı getirir (Read)
        public async Task<Pet?> GetAsync(string id) =>
            await _petsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Yeni evcil hayvan ekler (Create)
        public async Task CreateAsync(Pet newPet) =>
            await _petsCollection.InsertOneAsync(newPet);

        // Mevcut evcil hayvanı günceller (Update)
        public async Task UpdateAsync(string id, Pet updatedPet) =>
            await _petsCollection.ReplaceOneAsync(x => x.Id == id, updatedPet);

        // Evcil hayvanı siler (Delete)
        public async Task RemoveAsync(string id) =>
            await _petsCollection.DeleteOneAsync(x => x.Id == id);
    }
}