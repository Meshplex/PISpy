import React, { useState, useEffect } from "react";
import { UserDTO, getUserList, userService } from "../api/userService";
import DeleteUserModal from "./modal/delete-user-modal";
import EditUserModal from "./modal/edit-user-modal";
import ChangePasswordModal from "./modal/change-password-modal";
import KeyModal from "./modal/key-modal";
import AddUserModal from "./modal/add-user-modal";

type ModalType =
  | "delete"
  | "edit"
  | "changePassword"
  | "key"
  | "addUser"
  | null;

const UserList: React.FC = () => {
  const [users, setUsers] = useState<UserDTO[]>([]);
  const [activeModal, setActiveModal] = useState<ModalType>(null);
  const [selectedUser, setSelectedUser] = useState<UserDTO | null>(null);

  useEffect(() => {
    getUserList().then(setUsers);
  }, []);

  const openModal = (type: ModalType, user: UserDTO | null = null) => {
    setSelectedUser(user);
    setActiveModal(type);
  };

  return (
    <div className="bg-gray-800 text-blue-300 p-4 rounded-md shadow-md h-full">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-lg font-semibold text-blue-400">
          👤 Nutzerverwaltung
        </h2>
        <button
          onClick={() => openModal("addUser")}
          className="px-2 py-1 text-sm bg-blue-600 hover:bg-blue-500 text-white rounded"
        >
          Benutzer Hinzufügen +
        </button>
      </div>
      <div className="max-h-[220px] overflow-y-auto pr-1 space-y-3">
        {users.map((user) => (
          <div
            key={user.id}
            className="flex justify-between items-center bg-gray-700 rounded p-3"
          >
            <span className="font-mono text-sm">{user.username}</span>
            <div className="space-x-2">
              <button
                onClick={() => openModal("edit", user)}
                className="px-2 py-1 text-sm bg-blue-600 hover:bg-blue-500 text-white rounded"
              >
                🖊️
              </button>
              <button
                onClick={() => openModal("delete", user)}
                className="px-2 py-1 text-sm bg-red-600 hover:bg-red-500 text-white rounded"
              >
                🗑️
              </button>
              <button
                onClick={() => openModal("key", user)}
                className="px-2 py-1 text-sm bg-gray-600 hover:bg-gray-500 text-white rounded"
              >
                🔑
              </button>
            </div>
          </div>
        ))}
      </div>

      {/* Modals */}
      <DeleteUserModal
        isOpen={activeModal === "delete"}
        user={selectedUser}
        onClose={() => setActiveModal(null)}
        onConfirm={async () => {
          if (!selectedUser) return;
          await userService.deleteUser(selectedUser.id);
          setUsers(users.filter((u) => u.id !== selectedUser.id));
          setActiveModal(null);
        }}
      />

      <EditUserModal
        isOpen={activeModal === "edit"}
        user={selectedUser}
        onClose={() => setActiveModal(null)}
        onSaveUsername={async (newName) => {
          if (!selectedUser) return;
          await userService.updateUsername(selectedUser.id, newName);
          setUsers(
            users.map((u) =>
              u.id === selectedUser.id ? { ...u, username: newName } : u
            )
          );
          setActiveModal(null);
        }}
        onRequestPasswordChange={() => setActiveModal("changePassword")}
      />

      <ChangePasswordModal
        isOpen={activeModal === "changePassword"}
        user={selectedUser}
        onBack={() => setActiveModal("edit")}
        onClose={() => setActiveModal(null)}
        onConfirm={async (newPass) => {
          if (!selectedUser) return;
          await userService.updatePassword(selectedUser.id, newPass);
          setActiveModal(null);
        }}
      />

      <KeyModal
        isOpen={activeModal === "key"}
        user={selectedUser}
        onClose={() => setActiveModal(null)}
      />

      <AddUserModal
        isOpen={activeModal === "addUser"}
        onClose={() => setActiveModal(null)}
        onAdd={async (username, password) => {
          await userService.addUser({ username, password });
          const updated = await getUserList();
          setUsers(updated);
          setActiveModal(null);
        }}
      />
    </div>
  );
};

export default UserList;
