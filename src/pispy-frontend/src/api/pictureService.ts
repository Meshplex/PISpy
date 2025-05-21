import { getAuthHeaders, API_BASE } from "./apiService";

export const getPictures = async () => {
    const res = await fetch(`${API_BASE}/Picture/getallpictures`, {
      headers: getAuthHeaders(),
    });
    if (!res.ok) {
      throw new Error('Fehler beim Laden der Bilder');
    }
    const data = await res.json();
    return data.value;
};

export interface PictureDTO {
    id: number;
    path: string;
    createdAt: Date;
}