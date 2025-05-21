import React, { useState } from "react";
import { UserDTO } from "../../api/userService";

interface ChangePasswordModalProps {
  isOpen: boolean;
  user: UserDTO | null;
  onBack: () => void;
  onClose: () => void;
  onConfirm: (newPassword: string) => Promise<void>;
}

const ChangePasswordModal: React.FC<ChangePasswordModalProps> = ({
  isOpen,
  user,
  onBack,
  onClose,
  onConfirm,
}) => {
  const [newPassword, setNewPassword] = useState("");

  return (
    <div
      aria-hidden={!isOpen}
      className={`${
        isOpen
          ? "opacity-100 pointer-events-auto"
          : "opacity-0 pointer-events-none"
      } fixed inset-0 z-60 flex items-center justify-center bg-black bg-opacity-50 transition-opacity`}
    >
      <div className="bg-gray-800 text-blue-500 p-4 rounded-md shadow-md w-80">
        <h4 className="text-lg font-semibold mb-2">
          Neues Passwort für {user?.username}
        </h4>
        <input
          type="password"
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
          className="w-full p-2 bg-gray-700 border border-gray-600 text-blue-300 rounded mb-4"
          placeholder="Neues Passwort"
        />
        <div className="flex justify-end space-x-2">
          <button
            onClick={onBack}
            className="px-3 py-1 bg-gray-700 hover:bg-gray-600 text-white rounded"
          >
            Zurück
          </button>
          <button
            onClick={async () => {
              onClose();
              onConfirm(newPassword).catch(console.error);
            }}
            className="px-3 py-1 bg-blue-600 hover:bg-blue-500 text-white rounded"
          >
            Ändern
          </button>
        </div>
      </div>
    </div>
  );
};

export default ChangePasswordModal;
