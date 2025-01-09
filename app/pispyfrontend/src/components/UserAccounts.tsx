import React from "react";
import { User } from "../types";

interface UserAccountsProps {
  users: User[];
  onOpenKeyList: (user: User) => void;
  onOpenConfirmDeleteUser: (user: User) => void;
  onOpenCreateUserModal: () => void;
}

const UserAccounts: React.FC<UserAccountsProps> = ({
  users,
  onOpenKeyList,
  onOpenConfirmDeleteUser,
  onOpenCreateUserModal,
}) => {
  return (
    <section className="bg-[#1A1A1A] rounded-md p-6">
      <h2 className="text-2xl font-semibold mb-4 text-[#007ACC]">
        Nutzerkonten
      </h2>

      {/* Liste aller Nutzer */}
      <ul className="mb-6">
        {users.map((user, idx) => (
          <li
            key={idx}
            className="border-b border-[#2C2C2C] py-3 text-base flex items-center justify-between"
          >
            <span>{user.username}</span>
            <div className="flex items-center gap-4">
              {/* Schlüssel-Liste öffnen */}
              <button
                className="text-[#007ACC] hover:text-white transition-colors"
                onClick={() => onOpenKeyList(user)}
              >
                {/* Key-Icon */}
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
                    d="M15.75 
                    5.25a3.75 3.75 0 01-3.75 
                    3.75m0 0a3.75 3.75 0 
                    013.75-3.75m-3.75 
                    3.75L8.25 
                    12H7.5v1.5h-1.5v1.5H4.5
                    v1.5h1.17m6.33-7.5
                    a3.75 3.75 0 
                    11-3.75-3.75
                    3.75 3.75 0
                    013.75 3.75z"
                  />
                </svg>
              </button>

              {/* Nutzer löschen */}
              <button
                className="text-red-400 hover:text-red-500 transition-colors"
                onClick={() => onOpenConfirmDeleteUser(user)}
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
                    2.25 
                    0 002.25 
                    2.25h4.5a2.25 
                    2.25 
                    0 002.25-2.25
                    L17.25 
                    9m-10.5 
                    0h10.5M9 
                    9v10.5m3
                    -10.5v10.5m3
                    -10.5v10.5M15.75 
                    4.5v1.5h-7.5V4.5
                    a1.5 
                    1.5 
                    0 
                    011.5-1.5h4.5
                    a1.5 1.5 
                    0 011.5 1.5z"
                  />
                </svg>
              </button>
            </div>
          </li>
        ))}
      </ul>

      <button
        className="bg-[#007ACC] px-5 py-2 rounded-md text-[#0A0A0A] font-semibold text-base
                   hover:bg-[#0063a1] transition-colors"
        onClick={onOpenCreateUserModal}
      >
        Neuen Nutzer erstellen
      </button>
    </section>
  );
};

export default UserAccounts;
