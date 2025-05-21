import React, { useState, useEffect } from "react";
import { UserDTO } from "../../api/userService";

interface EditUserModalProps {
  isOpen: boolean;
  user: UserDTO | null;
  onClose: () => void;
  onSaveUsername: (newName: string) => Promise<void>;
  onRequestPasswordChange: () => void;
}

const EditUserModal: React.FC<EditUserModalProps> = ({
  isOpen,
  user,
  onClose,
  onSaveUsername,
  onRequestPasswordChange,
}) => {
  const [username, setUsername] = useState("");

  useEffect(() => {
    setUsername(user?.username || "");
  }, [user]);

  return (
    <div
      aria-hidden={!isOpen}
      className={`${
        isOpen
          ? "opacity-100 pointer-events-auto"
          : "opacity-0 pointer-events-none"
      } fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50 transition-opacity`}
    >
      <div className="bg-gray-800 text-blue-500 p-4 rounded-md shadow-md w-96">
        <h3 className="text-lg font-semibold mb-2">Benutzer bearbeiten</h3>
        <input
          type="text"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          className="w-full p-2 bg-gray-700 border border-gray-600 text-blue-300 rounded mb-4"
          placeholder="Neuer Benutzername"
        />
        <button
          onClick={onRequestPasswordChange}
          className="w-full px-3 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded mb-4"
        >
          Passwort ändern
        </button>
        <div className="flex justify-end space-x-2">
          <button
            onClick={onClose}
            className="px-4 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded"
          >
            Abbrechen
          </button>
          <button
            onClick={async () => {
              onClose();
              onSaveUsername(username).catch(console.error);
            }}
            className="px-4 py-2 bg-blue-600 hover:bg-blue-500 text-white rounded"
          >
            Speichern
          </button>
        </div>
      </div>
    </div>
  );
};

export default EditUserModal;
