import { getAuthHeaders, API_BASE } from "./apiService";

export const setAlarmStatus = async (isArmed) => {
  if (isArmed === true) {
    const res = await fetch(`${API_BASE}/Alarm/activate`, {
      method: 'PUT',
      headers: getAuthHeaders(true),
    });
    if (!res.ok) {
      throw new Error('Konnte Alarmstatus nicht ändern.');
    }
  } else if (isArmed === false) {
    const res = await fetch(`${API_BASE}/Alarm/deactivate`, {
      method: 'PUT',
      headers: getAuthHeaders(true),
    });
    console.log(res);
    if (!res.ok) {
      throw new Error('Konnte Alarmstatus nicht ändern.');
    }
  }
};