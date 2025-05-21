import React from "react";
import { UserDTO } from "../../api/userService";

interface KeyModalProps {
  isOpen: boolean;
  user: UserDTO | null;
  onClose: () => void;
}

const KeyModal: React.FC<KeyModalProps> = ({ isOpen, user, onClose }) => (
  <div
    aria-hidden={!isOpen}
    className={`${
      isOpen
        ? "opacity-100 pointer-events-auto"
        : "opacity-0 pointer-events-none"
    } fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50 transition-opacity`}
  >
    <div className="bg-gray-800 text-blue-500 p-4 rounded-md shadow-md w-64">
      <h3 className="text-lg font-semibold mb-2">
        Schlüssel für {user?.username}
      </h3>
      {/* TODO: Deine Felder hier */}
      <div className="mt-4 flex justify-end">
        <button
          onClick={onClose}
          className="px-4 py-2 bg-blue-600 hover:bg-blue-500 text-white rounded"
        >
          Schließen
        </button>
      </div>
    </div>
  </div>
);

export default KeyModal;
