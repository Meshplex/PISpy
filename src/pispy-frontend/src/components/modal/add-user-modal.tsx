import React, { useState } from "react";

interface AddUserModalProps {
  isOpen: boolean;
  onClose: () => void;
  onAdd: (username: string, password: string) => Promise<void>;
}

const AddUserModal: React.FC<AddUserModalProps> = ({
  isOpen,
  onClose,
  onAdd,
}) => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

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
        <h3 className="text-lg font-semibold mb-2">
          Neuen Benutzer hinzufügen
        </h3>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          className="w-full p-2 bg-gray-700 border border-gray-600 text-blue-300 rounded mb-3"
        />
        <input
          type="password"
          placeholder="Passwort"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full p-2 bg-gray-700 border border-gray-600 text-blue-300 rounded mb-4"
        />
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
              onAdd(username, password).catch(console.error);
            }}
            className="px-4 py-2 bg-blue-600 hover:bg-blue-500 text-white rounded"
          >
            Hinzufügen
          </button>
        </div>
      </div>
    </div>
  );
};

export default AddUserModal;
