const API_BASE = 'https://petpulse-api-xz76.onrender.com/api';

document.addEventListener('DOMContentLoaded', () => {
    // 1. Yetki Kontrolü: Giriş yapılmamışsa login'e at
    if (!localStorage.getItem('userEmail')) {
        window.location.href = 'login.html';
        return;
    }
    
    // 2. Karşılama Mesajı: Kullanıcının ismini ekrana yazdır
    const welcomeEl = document.getElementById('welcomeMessage');
    if (welcomeEl && localStorage.getItem('userName')) {
        welcomeEl.innerText = 'Welcome, ' + localStorage.getItem('userName');
    }
    
    // 3. Sayfa açılır açılmaz hayvanları listele
    fetchPets();

    // 4. HAYVAN EKLEME İŞLEMİ (Sadece Dashboard'da çalışır)
    const addPetForm = document.getElementById('addPetForm');
    if (addPetForm) {
        addPetForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const newPet = {
                name: document.getElementById('petName').value,
                breed: document.getElementById('petBreed').value,
                age: parseInt(document.getElementById('petAge').value),
                weight: parseFloat(document.getElementById('petWeight').value),
                vaccinationSchedule: document.getElementById('petVaccine').value,
                dietaryRequirements: document.getElementById('petDiet').value,
                ownerEmail: localStorage.getItem('userEmail')
            };

            await fetch(`${API_BASE}/Pets`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newPet)
            });
            addPetForm.reset();
            fetchPets(); // Listeyi güncelle
        });
    }
});

// --- HAYVANLARI LİSTELEME VE BUTONLARI OLUŞTURMA ---
let allPets = []; // Global pet listesi

async function fetchPets() {
    const email = localStorage.getItem('userEmail');
    const role = localStorage.getItem('userRole');
    
    try {
        const res = await fetch(`${API_BASE}/Pets?email=${email}&role=${role}`);
        allPets = await res.json();
        renderPets(allPets);
    } catch (error) {
        console.error("Veriler çekilirken hata oluştu:", error);
    }
}

function renderPets(pets) {
    const role = localStorage.getItem('userRole');
    const tableBody = document.getElementById('petsTableBody') || document.getElementById('adminTable');
    
    if(tableBody) {
        tableBody.innerHTML = pets.map(p => {
            const petId = p.id || p.Id;
            const ownerCol = role === 'Admin' ? `<td><small class="text-primary">${p.ownerEmail}</small></td>` : '';
            return `
            <tr>
                <td><strong>${p.name}</strong></td>
                <td>${p.breed}</td>
                <td>${p.age} years, ${p.weight} kg</td>
                <td><small>Vac: ${p.vaccinationSchedule || '-'} <br> Diet: ${p.dietaryRequirements || '-'}</small></td>
                ${ownerCol}
                <td>
                    <button class="btn btn-warning btn-sm text-dark" onclick="openEditModal('${petId}', '${p.name}', '${p.breed}', ${p.age}, ${p.weight}, '${p.vaccinationSchedule}', '${p.dietaryRequirements}', '${p.ownerEmail}')">Edit</button>
                    <button class="btn btn-info btn-sm text-white" onclick="openLogsModal('${petId}')">Logs</button>
                    <button class="btn btn-danger btn-sm" onclick="deletePet('${petId}')">Delete</button>
                </td>
            </tr>`
        }).join('');
    }
}

// ARAMA FONKSİYONU
// ARAMA FONKSİYONU
function searchPets() {
    const query = document.getElementById('searchInput') 
        ? document.getElementById('searchInput').value.toLowerCase().trim() 
        : '';
    
    if (!query) {
        renderPets(allPets);
        return;
    }

    const filtered = allPets.filter(p => 
        (p.name && p.name.toLowerCase().includes(query)) || 
        (p.breed && p.breed.toLowerCase().includes(query))
    );
    renderPets(filtered);
}

// SIRALAMA FONKSİYONU
function sortPets(field) {
    if (!field) {
        renderPets(allPets);
        return;
    }
    const sorted = [...allPets].sort((a, b) => {
        if (typeof a[field] === 'string') return a[field].localeCompare(b[field]);
        return a[field] - b[field];
    });
    renderPets(sorted);
}

// --- SİLME (DELETE) ---
async function deletePet(id) {
    if(confirm("Are you sure you want to delete this pet?")) {
        await fetch(`${API_BASE}/Pets/${id}`, { method: 'DELETE' });
        fetchPets();
    }
}

// --- DÜZENLEME (EDIT) FONKSİYONLARI ---
function openEditModal(id, name, breed, age, weight, vac, diet, ownerEmail) {
    document.getElementById('editPetId').value = id;
    document.getElementById('editPetName').value = name;
    document.getElementById('editPetBreed').value = breed;
    document.getElementById('editPetAge').value = age;
    document.getElementById('editPetWeight').value = weight;
    document.getElementById('editPetVaccine').value = vac !== 'undefined' ? vac : '';
    document.getElementById('editPetDiet').value = diet !== 'undefined' ? diet : '';
    document.getElementById('editPetOwner').value = ownerEmail; // YENİ: Sahip bilgisi modala yazılıyor
    
    // Modalı ekranda göster
    new bootstrap.Modal(document.getElementById('editModal')).show();
}

async function updatePet() {
    const id = document.getElementById('editPetId').value;
    const updatedPet = {
        name: document.getElementById('editPetName').value,
        breed: document.getElementById('editPetBreed').value,
        age: parseInt(document.getElementById('editPetAge').value),
        weight: parseFloat(document.getElementById('editPetWeight').value),
        vaccinationSchedule: document.getElementById('editPetVaccine').value,
        dietaryRequirements: document.getElementById('editPetDiet').value,
        ownerEmail: document.getElementById('editPetOwner').value // YENİ: Hayvanın asıl sahibi korunuyor
    };

    await fetch(`${API_BASE}/Pets/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updatedPet)
    });
    
    // Modalı gizle ve tabloyu yenile
    const modalElement = document.getElementById('editModal');
    const modalInstance = bootstrap.Modal.getInstance(modalElement);
    if(modalInstance) modalInstance.hide();
    
    fetchPets();
}

// --- GÜNLÜK (LOG) FONKSİYONLARI ---
async function openLogsModal(petId) {
    document.getElementById('logPetId').value = petId;
    await loadLogs(petId);
    new bootstrap.Modal(document.getElementById('logsModal')).show();
}

async function loadLogs(petId) {
    const res = await fetch(`${API_BASE}/Logs/${petId}`);
    const logs = await res.json();
    const logsList = document.getElementById('logsList');
    
    if (logs.length === 0) {
        logsList.innerHTML = `<li class="list-group-item text-muted">No logs found for this pet yet.</li>`;
        return;
    }

    logsList.innerHTML = logs.map(l => {
        const date = new Date(l.logDate).toLocaleDateString();
        return `<li class="list-group-item">
            <strong>[${date}] ${l.logType}</strong>: ${l.description}
        </li>`
    }).join('');
}

async function saveLog() {
    const logData = {
        petId: document.getElementById('logPetId').value,
        logType: document.getElementById('logType').value,
        description: document.getElementById('logDesc').value
    };

    await fetch(`${API_BASE}/Logs`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(logData)
    });

    // Formu temizle ve log listesini anında yenile
    document.getElementById('logType').value = '';
    document.getElementById('logDesc').value = '';
    loadLogs(logData.petId);
}