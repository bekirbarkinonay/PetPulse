using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PetPulse.API.Models;

namespace PetPulse.API.Controllers
{
    // Giriş yaparken sadece gereken verileri taşıyan küçük model
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;

        public AuthController(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

       [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] UserLoginDto loginUser)
{
    // Veritabanında eşleşen kullanıcıyı kontrol et
    var user = await _users.Find(u => u.Email == loginUser.Email).FirstOrDefaultAsync();

    if (user == null)
    {
        return Unauthorized($"Kullanıcı bulunamadı: {loginUser.Email}");
    }
    
    if (user.Password != loginUser.password) // küçük harf/büyük harf kontrolü
    {
        return Unauthorized($"Şifre eşleşmedi! DB: {user.Password} - Giriş: {loginUser.password}");
    }

    return Ok(new { email = user.Email, role = user.Role, firstName = user.FirstName, lastName = user.LastName });
}

        // --- YENİ KAYIT OLMA (REGISTER) ---
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User newUser)
        {
            var existingUser = await _users.Find(u => u.Email == newUser.Email).FirstOrDefaultAsync();
            if (existingUser != null) 
            {
                return BadRequest("Bu e-posta adresi zaten kullanımda.");
            }

            newUser.Role = "User";
            await _users.InsertOneAsync(newUser);
            
            return Ok(new { message = "Kayıt başarıyla oluşturuldu!" });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers(string role)
        {
            if (role != "Admin") return Unauthorized("Sadece adminler görebilir.");
            var users = await _users.Find(_ => true).ToListAsync();
            return Ok(users);
        }
    }
}