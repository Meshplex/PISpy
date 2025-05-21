import { useState } from "react";
import { setAlarmStatus } from "../api/alarmService";

function AlarmToggle() {
  const [isOn, setIsOn] = useState(false);

  const handleToggle = async () => {
    const newStatus = !isOn;
    setIsOn(newStatus);
    try {
      await setAlarmStatus(newStatus);
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="bg-gray-800 text-blue-300 p-4 rounded-md shadow-md">
      <h2 className="text-lg font-semibold mb-4 text-blue-400">
        🚨 Alarmsystem
      </h2>

      <div className="flex items-center justify-between">
        <span className="text-sm">Alarmstatus</span>

        <label
          htmlFor="alarm-toggle"
          className="relative inline-block w-11 h-6 cursor-pointer"
        >
          <input
            type="checkbox"
            id="alarm-toggle"
            className="peer sr-only"
            checked={isOn}
            onChange={handleToggle}
          />

          {/* Track */}
          <span
            className="
      absolute inset-0 
      bg-gray-200 rounded-full 
      transition-colors duration-200 ease-in-out 
      peer-checked:!bg-green-600      /* ← wichtig: das ! sorgt für higher specificity */
      dark:bg-neutral-700 
      dark:peer-checked:!bg-green-500
      peer-disabled:opacity-50 peer-disabled:pointer-events-none
    "
          ></span>

          {/* Thumb */}
          <span
            className="
      absolute top-1/2 start-0.5 -translate-y-1/2 
      w-5 h-5 bg-white rounded-full shadow-xs 
      transition-transform duration-200 ease-in-out 
      peer-checked:translate-x-full
      peer-disabled:opacity-50 peer-disabled:pointer-events-none
    "
          ></span>
        </label>
      </div>

      <div className="mt-4 text-sm text-blue-400 italic">
        {isOn ? "Alarm ist AKTIV" : "Alarm ist deaktiviert"}
      </div>
    </div>
  );
}

export default AlarmToggle;
