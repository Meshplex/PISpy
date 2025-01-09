import React from "react";

interface AlarmStatusProps {
  isAlarmActive: boolean;
  onToggleAlarm: () => void;
}

const AlarmStatus: React.FC<AlarmStatusProps> = ({
  isAlarmActive,
  onToggleAlarm,
}) => {
  return (
    <section className="bg-[#1A1A1A] rounded-md p-6 flex flex-col items-center justify-center">
      <h2 className="text-2xl font-semibold mb-4 text-[#007ACC]">
        Alarm Status
      </h2>
      <p className="text-lg mb-4">
        Die Alarmanlage ist derzeit:{" "}
        <span
          className={`font-bold ${
            isAlarmActive ? "text-green-400" : "text-red-400"
          }`}
        >
          {isAlarmActive ? "AKTIV" : "INAKTIV"}
        </span>
      </p>
      {/* Toggle Switch */}
      <label className="relative inline-flex items-center cursor-pointer">
        <input
          type="checkbox"
          className="sr-only peer"
          checked={isAlarmActive}
          onChange={onToggleAlarm}
        />
        <div
          className="w-14 h-8 bg-gray-300 rounded-full peer dark:bg-gray-700
                    peer-checked:after:translate-x-full 
                    peer-checked:after:border-white
                    after:content-[''] 
                    after:absolute after:top-[2px] after:left-[2px] 
                    after:bg-white after:border-gray-300 after:border 
                    after:rounded-full after:h-7 after:w-7 after:transition-all
                    peer-checked:bg-[#007ACC]"
        />
        <span className="ml-3 text-lg font-medium">
          {isAlarmActive ? "Ausschalten" : "Einschalten"}
        </span>
      </label>
    </section>
  );
};

export default AlarmStatus;
