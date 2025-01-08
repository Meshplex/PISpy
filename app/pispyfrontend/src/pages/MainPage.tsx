// MainPage.tsx
import React from "react";
import { useNavigate } from "react-router-dom";

const MainPage: React.FC = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    // Token entfernen
    localStorage.removeItem("jwt_token");

    // Zurück zur Login-Seite navigieren
    navigate("/login");
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50 p-4">
      <h1 className="text-2xl font-bold mb-4">Willkommen auf der MainPage!</h1>
      <p className="mb-6">Hier steht geschützter Inhalt.</p>

      <button
        onClick={handleLogout}
        className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition-colors"
      >
        Logout
      </button>
    </div>
  );
};

export default MainPage;
