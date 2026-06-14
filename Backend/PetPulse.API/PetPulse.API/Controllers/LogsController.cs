using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PetPulse.API.Models;

namespace PetPulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IMongoCollection<PetLog> _logs;

        public LogsController(IMongoDatabase database)
        {
            _logs = database.GetCollection<PetLog>("PetLogs");
        }

        // --- BİR HAYVANA AİT TÜM GÜNLÜKLERİ GETİR ---
        [HttpGet("{petId}")]
        public async Task<IActionResult> GetLogs(string petId)
        {
            var logs = await _logs.Find(l => l.PetId == petId).ToListAsync();
            return Ok(logs);
        }

        // --- YENİ GÜNLÜK NOTU EKLE ---
        [HttpPost]
        public async Task<IActionResult> AddLog([FromBody] PetLog newLog)
        {
            // Tarihi arka planda otomatik olarak "Şu An" olarak ayarlıyoruz
            newLog.LogDate = DateTime.Now;
            await _logs.InsertOneAsync(newLog);
            return Ok();
        }
    }
}