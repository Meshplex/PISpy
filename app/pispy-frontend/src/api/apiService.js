// src/api/apiService.js

const API_BASE = 'http://localhost:5272/api';

const getAuthHeaders = (isJson = false) => {
  const headers = {
    Authorization: 'Bearer ' + localStorage.getItem('token'),
  };
  if (isJson) {
    headers['Content-Type'] = 'application/json';
  }
  return headers;
};

export const fetchEvents = async () => {
  const res = await fetch(`${API_BASE}/Event/getEvents`, {
    headers: getAuthHeaders(),
  });
  if (!res.ok) {
    throw new Error('Fehler beim Laden der Events');
  }
  const data = await res.json();
  return data.value;
};

export const fetchUsers = async () => {
  const res = await fetch(`${API_BASE}/User/getall`, {
    headers: getAuthHeaders(),
  });
  if (!res.ok) {
    throw new Error('Fehler beim Laden der Benutzer');
  }
  const data = await res.json();
  return data.value;
};

export const changeUserPassword = async (userId, newPassword) => {
  const res = await fetch(`${API_BASE}/User/update`, {
    method: 'POST',
    headers: getAuthHeaders(true),
    body: JSON.stringify({ userId, newPassword }),
  });
  if (!res.ok) {
    throw new Error('Passwort konnte nicht geändert werden.');
  }
};

export const deleteUser = async (userId) => {
  const res = await fetch(`${API_BASE}/User/delete`, {
    method: 'DELETE',
    headers: getAuthHeaders(),
    body: JSON.stringify({ userId }),
  });
  if (!res.ok) {
    throw new Error('Benutzer konnte nicht gelöscht werden.');
  }
};

export const AddKeyToUser = async (newString) => {
  const res = await fetch(`${API_BASE}/Key/mapkeytouser/${newString}`, {
    method: 'POST',
    headers: getAuthHeaders(true),
  });
  if (!res.ok) {
    throw new Error('String konnte nicht aktualisiert werden.');
  }
};

export const setAlarmStatus = async (isArmed) => {
  if (isArmed === true) {
    const res = await fetch(`${API_BASE}/Alarm/activate`, {
      method: 'POST',
      headers: getAuthHeaders(true),
    });
    if (!res.ok) {
      throw new Error('Konnte Alarmstatus nicht ändern.');
    }
  } else if (isArmed === false) {
    const res = await fetch(`${API_BASE}/Alarm/deactivate`, {
      method: 'POST',
      headers: getAuthHeaders(true),
    });
    if (!res.ok) {
      throw new Error('Konnte Alarmstatus nicht ändern.');
    }
  }
};

export async function fetchUserKeys() {
  const res = await fetch(`${API_BASE}/Key/getkeysFromUser`, {
    method: 'GET',
    headers: getAuthHeaders(true),
  });
  if (!res.ok) {
    throw new Error('Konnte Alarmstatus nicht ändern.');
  }
  const data = await res.json();
  return data.value;
}

export async function deleteUserKey(keyId) {
  const res = await fetch(`${API_BASE}/Key/removekey/${keyId}`, {
    method: 'DELETE',
    headers: getAuthHeaders(true),
  });
  if (!res.ok) {
    throw new Error('Konnte Alarmstatus nicht ändern.');
  }
}

// Funktion zum Hinzufügen eines neuen Schlüssels
export async function addUserKey(userId, keyValue) {
  const response = await fetch(`/api/users/${userId}/keys`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ key: keyValue }),
  });
  if (!response.ok) {
    throw new Error('Fehler beim Hinzufügen des Schlüssels');
  }
  return await response.json();
}
