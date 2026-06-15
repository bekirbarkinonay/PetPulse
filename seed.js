// Bu script, veritabanını test verileriyle doldurur.
db = db.getSiblingDB('PetPulseDB');
db.Users.insertMany([
  { "Email": "admin@petpulse.com", "Password": "admin", "FirstName": "Admin", "LastName": "System", "Role": "Admin" },
  { "Email": "test@petpulse.com", "Password": "123", "FirstName": "Test", "LastName": "User", "Role": "User" }
]);