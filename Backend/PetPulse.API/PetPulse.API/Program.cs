using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// 1. MONGODB BAĞLANTISINI SİSTEME TANITIYORUZ (Hatanın Çözümü)
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB");
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase("PetPulseDB"); // Atlas'taki DB adının bu olduğundan emin ol
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

// 2. CORS AYARI (Frontend'in hata vermeden Backend'e bağlanabilmesi için)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
//app.UseHttpsRedirection();

// CORS Kuralını Devreye Sokuyoruz
app.UseCors("AllowAll");
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();