import React from "react";
import { UserDTO } from "../../api/userService";

interface DeleteUserModalProps {
  isOpen: boolean;
  user: UserDTO | null;
  onClose: () => void;
  onConfirm: () => Promise<void>;
}

const DeleteUserModal: React.FC<DeleteUserModalProps> = ({
  isOpen,
  user,
  onClose,
  onConfirm,
}) => (
  <div
    aria-hidden={!isOpen}
    className={`${
      isOpen
        ? "opacity-100 pointer-events-auto"
        : "opacity-0 pointer-events-none"
    } fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50 transition-opacity`}
  >
    <div className="bg-gray-800 text-blue-500 p-4 rounded-md shadow-md w-80">
      <h3 className="text-lg font-semibold mb-2">Löschen bestätigen</h3>
      <p className="text-sm text-blue-300">
        Willst du <strong>{user?.username}</strong> wirklich löschen?
      </p>
      <div className="mt-4 flex justify-end space-x-2">
        <button
          onClick={onClose}
          className="px-4 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded"
        >
          Abbrechen
        </button>
        <button
          onClick={async () => {
            onClose();
            onConfirm().catch(console.error);
          }}
          className="px-4 py-2 bg-red-600 hover:bg-red-500 text-white rounded"
        >
          Löschen
        </button>
      </div>
    </div>
  </div>
);

export default DeleteUserModal;
