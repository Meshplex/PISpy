// src/components/CreateUserForm.tsx

import React, { useState } from "react";

interface CreateUserFormProps {
  onSubmit: (username: string) => void;
  onCancel: () => void;
}

const CreateUserForm: React.FC<CreateUserFormProps> = ({
  onSubmit,
  onCancel,
}) => {
  const [username, setUsername] = useState("");

  const handleSubmit = () => {
    if (username.trim() !== "") {
      onSubmit(username.trim());
    }
  };

  return (
    <>
      <label className="block mb-2">
        Nutzername:
        <input
          type="text"
          className="mt-1 block w-full rounded bg-[#101010] border border-[#2C2C2C] p-2 
                     text-white focus:outline-none focus:border-[#007ACC]"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
      </label>
      <div className="flex justify-end gap-3">
        <button
          onClick={onCancel}
          className="bg-gray-500 hover:bg-gray-600 px-4 py-2 rounded"
        >
          Abbrechen
        </button>
        <button
          onClick={handleSubmit}
          className="bg-[#007ACC] hover:bg-[#0063a1] px-4 py-2 rounded"
        >
          Erstellen
        </button>
      </div>
    </>
  );
};

export default CreateUserForm;
