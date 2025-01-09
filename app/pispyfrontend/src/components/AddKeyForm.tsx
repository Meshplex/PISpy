// src/components/AddKeyForm.tsx

import React, { useState } from "react";

interface AddKeyFormProps {
  onSubmit: (newKey: string) => void;
  onCancel: () => void;
}

const AddKeyForm: React.FC<AddKeyFormProps> = ({ onSubmit, onCancel }) => {
  const [newKeyName, setNewKeyName] = useState("");

  const handleSubmit = () => {
    if (newKeyName.trim() !== "") {
      onSubmit(newKeyName.trim());
    }
  };

  return (
    <>
      <label className="block mb-2">
        Schlüsselname:
        <input
          type="text"
          className="mt-1 block w-full rounded bg-[#101010] border border-[#2C2C2C] p-2 
                     text-white focus:outline-none focus:border-[#007ACC]"
          value={newKeyName}
          onChange={(e) => setNewKeyName(e.target.value)}
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
          Hinzufügen
        </button>
      </div>
    </>
  );
};

export default AddKeyForm;
