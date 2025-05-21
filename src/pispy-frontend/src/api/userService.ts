import { getAuthHeaders, API_BASE } from "./apiService";

export interface UserDTO {
    id: number;
    username: string;
}

export const getUserList = async (): Promise<UserDTO[]> => {
    const res = await fetch(`${API_BASE}/User/getall`, {
        headers: getAuthHeaders(),
    });
    if (!res.ok) {
        throw new Error("Fehler beim Laden der Nutzer");
    }
    const data = await res.json();
    return data.value;
};

export const deleteUser = async (id: number): Promise<void> => {
    const res = await fetch(`${API_BASE}/User/delete/${id}`, {
        method: "DELETE",
        headers: {
            ...getAuthHeaders(),
        },
    });
    if (!res.ok) {
        throw new Error("Fehler beim Löschen des Nutzers");
    }
};

export const updateUsername = async (id: number, username: string): Promise<void> => {
    const res = await fetch(`${API_BASE}/User/updateUsername`, {
        method: "PUT",
        headers: {
            ...getAuthHeaders(),
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ id, username }),
    });
    if (!res.ok) {
        throw new Error("Fehler beim Ändern des Benutzernamens");
    }
};

export const updatePassword = async (id: number, password: string): Promise<void> => {
    const res = await fetch(`${API_BASE}/User/updatePassword`, {
        method: "PUT",
        headers: {
            ...getAuthHeaders(),
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ id, password }),
    });
    if (!res.ok) {
        throw new Error("Fehler beim Ändern des Passworts");
    }
};

export const addUser = async (user: { username: string; password: string; }): Promise<void> => {
    const res = await fetch(`${API_BASE}/User/create`, {
        method: "POST",
        headers: {
            ...getAuthHeaders(),
            "Content-Type": "application/json",
        },
        body: JSON.stringify(user),
    });
    if (!res.ok) {
        throw new Error("Fehler beim Hinzufügen des Nutzers");
    }
};

export const userService = {
    deleteUser,
    updateUsername,
    updatePassword,
    addUser,
};
