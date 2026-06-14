using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PetPulse.API.Models;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly IMongoCollection<Pet> _pets;
    public PetsController(IMongoDatabase db) => _pets = db.GetCollection<Pet>("Pets");

    [HttpGet]
    public async Task<List<Pet>> Get(string email, string role)
    {
        // Eğer gelen kişi Admin ise, tüm hayvanları (veritabanındaki herkesi) gönder
        if (role == "Admin")
        {
            return await _pets.Find(_ => true).ToListAsync();
        }

        // Eğer gelen kişi User ise, sadece kendi OwnerEmail'ine ait olanları gönder
        return await _pets.Find(p => p.OwnerEmail == email).ToListAsync();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        // MongoDB'ye git ve ID'si eşleşen hayvanı sil
        var result = await _pets.DeleteOneAsync(p => p.Id == id);

        // Eğer böyle bir kayıt yoksa hata dön
        if (result.DeletedCount == 0)
        {
            return NotFound();
        }

        // Başarıyla silindiyse OK dön
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Post(Pet newPet)
    {
        await _pets.InsertOneAsync(newPet);
        return Ok();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Pet updatedPet)
    {
        // Mevcut hayvanı ID ile bul ve yeni bilgilerle (updatedPet) değiştir
        var result = await _pets.ReplaceOneAsync(p => p.Id == id, updatedPet);

        if (result.MatchedCount == 0) return NotFound();
        return Ok();
    }
}