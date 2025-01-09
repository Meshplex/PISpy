import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

// (1) Typen und Daten
import { User } from "../types";
import { initialUsers } from "../data/InitialUsers";

// (2) Eigene Komponenten
import NavBar from "../components/NavBar";
import AlarmStatus from "../components/AlarmStatus";
import UserAccounts from "../components/UserAccounts";
import Events from "../components/Events";

import Modal from "../components/Modal";
import AddKeyForm from "../components/AddKeyForm";
import CreateUserForm from "../components/CreateUserForm";

const MainPage: React.FC = () => {
  const navigate = useNavigate();

  // ---------------------
  //   STATE / LOGIK
  // ---------------------
  const [users, setUsers] = useState<User[]>(initialUsers);

  // Ausgewählter Nutzer / Schlüssel
  const [selectedUser, setSelectedUser] = useState<User | null>(null);
  const [selectedKey, setSelectedKey] = useState<string | null>(null);

  // Modals
  const [isConfirmDeleteUserOpen, setIsConfirmDeleteUserOpen] = useState(false);
  const [isKeyListOpen, setIsKeyListOpen] = useState(false);
  const [isConfirmDeleteKeyOpen, setIsConfirmDeleteKeyOpen] = useState(false);
  const [isAddKeyOpen, setIsAddKeyOpen] = useState(false);
  const [isCreateUserOpen, setIsCreateUserOpen] = useState(false);

  // Alarmstatus
  const [isAlarmActive, setIsAlarmActive] = useState(false);
  const handleToggleAlarm = () => setIsAlarmActive(!isAlarmActive);

  // Logout-Funktion
  const handleLogout = () => {
    localStorage.removeItem("jwt_token");
    navigate("/login");
  };

  // -------------- Nutzer löschen --------------
  const openConfirmDeleteUser = (user: User) => {
    setSelectedUser(user);
    setIsConfirmDeleteUserOpen(true);
  };
  const confirmDeleteUser = () => {
    if (!selectedUser) return;
    setUsers((prev) => prev.filter((u) => u !== selectedUser));
    closeAllPopups();
  };

  // -------------- Schlüssel-Liste --------------
  const openKeyList = (user: User) => {
    setSelectedUser(user);
    setIsKeyListOpen(true);
  };

  // -------------- Schlüssel löschen --------------
  const openConfirmDeleteKey = (user: User, key: string) => {
    setSelectedUser(user);
    setSelectedKey(key);
    setIsConfirmDeleteKeyOpen(true);
  };
  const confirmDeleteKey = () => {
    if (!selectedUser || !selectedKey) return;
    setUsers((prev) =>
      prev.map((u) => {
        if (u === selectedUser) {
          return { ...u, keys: u.keys.filter((k) => k !== selectedKey) };
        }
        return u;
      })
    );
    closeAllPopups();
  };

  // -------------- Schlüssel hinzufügen --------------
  const openAddKeyModal = (user: User) => {
    setSelectedUser(user);
    setIsAddKeyOpen(true);
  };
  const handleAddKey = (newKey: string) => {
    if (!selectedUser) return;
    setUsers((prev) =>
      prev.map((u) => {
        if (u === selectedUser) {
          return { ...u, keys: [...u.keys, newKey] };
        }
        return u;
      })
    );
    closeAllPopups();
  };

  // -------------- Neuen Nutzer erstellen --------------
  const openCreateUserModal = () => {
    setIsCreateUserOpen(true);
  };
  const handleCreateUser = (username: string) => {
    setUsers((prev) => [...prev, { username, keys: [] }]);
    closeAllPopups();
  };

  // -------------- Alle Popups schließen --------------
  const closeAllPopups = () => {
    setIsConfirmDeleteUserOpen(false);
    setIsKeyListOpen(false);
    setIsConfirmDeleteKeyOpen(false);
    setIsAddKeyOpen(false);
    setIsCreateUserOpen(false);
    setSelectedUser(null);
    setSelectedKey(null);
  };

  // ---------------------
  //   RENDER
  // ---------------------
  return (
    <div className="min-h-screen bg-[#0A0A0A] text-[#E2E8F0]">

      {/* NAVIGATION */}
      <NavBar onLogout={handleLogout} />

      {/* MAIN CONTENT */}
      <main className="container mx-auto p-4">

        {/* Grid: links Alarmstatus, rechts Nutzerkonten */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
          <AlarmStatus
            isAlarmActive={isAlarmActive}
            onToggleAlarm={handleToggleAlarm}
          />

          <UserAccounts
            users={users}
            onOpenKeyList={openKeyList}
            onOpenConfirmDeleteUser={openConfirmDeleteUser}
            onOpenCreateUserModal={openCreateUserModal}
          />
        </div>

        {/* Events-Feld */}
        <Events />
      </main>

      {/*
        --------------------------------------------------------
        ALLE MODALS / POPUPS
        --------------------------------------------------------
      */}

      {/* (1) Bestätigungs-Popup: Nutzer löschen */}
      <Modal
        isOpen={isConfirmDeleteUserOpen}
        onClose={closeAllPopups}
        title="Nutzer löschen?"
        zIndex="z-50"
      >
        <p className="mb-4">
          Möchtest du den Nutzer{" "}
          <span className="font-semibold">{selectedUser?.username}</span>{" "}
          wirklich löschen?
        </p>
        <div className="flex justify-end gap-3">
          <button
            onClick={closeAllPopups}
            className="bg-gray-500 hover:bg-gray-600 px-4 py-2 rounded"
          >
            Abbrechen
          </button>
          <button
            onClick={confirmDeleteUser}
            className="bg-red-600 hover:bg-red-700 px-4 py-2 rounded"
          >
            Löschen
          </button>
        </div>
      </Modal>

      {/* (2) Schlüssel-Liste (Popup) */}
      <Modal
        isOpen={isKeyListOpen}
        onClose={closeAllPopups}
        title={`Schlüssel für ${selectedUser?.username}`}
        zIndex="z-50"
      >
        <ul className="mb-4">
          {selectedUser?.keys?.map((key, idx) => (
            <li
              key={idx}
              className="flex justify-between items-center py-2 border-b border-[#2C2C2C]"
            >
              <span>{key}</span>
              <button
                className="text-red-400 hover:text-red-500 transition-colors"
                onClick={() => openConfirmDeleteKey(selectedUser!, key)}
              >
                {/* Trash-Icon */}
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="currentColor"
                  className="w-5 h-5"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M6.75 
                    9L7.5 
                    19.5a2.25 
                    2.25 0 
                    002.25 
                    2.25h4.5a2.25 
                    2.25 0 
                    002.25-2.25L17.25 
                    9m-10.5 
                    0h10.5M9 
                    9v10.5m3
                    -10.5v10.5m3
                    -10.5v10.5M15.75 
                    4.5v1.5h-7.5V4.5
                    a1.5 1.5 0 
                    011.5-1.5h4.5a1.5 
                    1.5 0 
                    011.5 1.5z"
                  />
                </svg>
              </button>
            </li>
          ))}
        </ul>

        <button
          onClick={() => openAddKeyModal(selectedUser!)}
          className="bg-[#007ACC] hover:bg-[#0063a1] px-4 py-2 rounded text-[#0A0A0A]"
        >
          Schlüssel hinzufügen
        </button>
      </Modal>

      {/* (3) Bestätigungs-Popup: Schlüssel löschen */}
      <Modal
        isOpen={isConfirmDeleteKeyOpen}
        onClose={closeAllPopups}
        title="Schlüssel löschen?"
        zIndex="z-60"
      >
        <p className="mb-4">
          Möchtest du den Schlüssel{" "}
          <span className="font-semibold">"{selectedKey}"</span> von Nutzer{" "}
          <span className="font-semibold">{selectedUser?.username}</span>{" "}
          wirklich löschen?
        </p>
        <div className="flex justify-end gap-3">
          <button
            onClick={closeAllPopups}
            className="bg-gray-500 hover:bg-gray-600 px-4 py-2 rounded"
          >
            Abbrechen
          </button>
          <button
            onClick={confirmDeleteKey}
            className="bg-red-600 hover:bg-red-700 px-4 py-2 rounded"
          >
            Löschen
          </button>
        </div>
      </Modal>

      {/* (4) Schlüssel hinzufügen */}
      <Modal
        isOpen={isAddKeyOpen}
        onClose={closeAllPopups}
        title="Neuen Schlüssel hinzufügen"
        zIndex="z-60"
      >
        <AddKeyForm onSubmit={handleAddKey} onCancel={closeAllPopups} />
      </Modal>

      {/* (5) Neuen Nutzer erstellen */}
      <Modal
        isOpen={isCreateUserOpen}
        onClose={closeAllPopups}
        title="Neuen Nutzer erstellen"
        zIndex="z-50"
      >
        <CreateUserForm onSubmit={handleCreateUser} onCancel={closeAllPopups} />
      </Modal>
    </div>
  );
};

export default MainPage;
