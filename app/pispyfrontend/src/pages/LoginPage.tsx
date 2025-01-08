// LoginPage.tsx

import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { mockFetch } from "../util/mockFetch";

const LoginPage: React.FC = () => {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      // Beispiel: Anfrage an dein Backend
      // Achtung: URL an dein Backend anpassen!
      const response = await mockFetch("/api/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ username, password }),
      });

      if (!response.ok) {
        throw new Error("Login fehlgeschlagen");
      }

      const data = await response.json();

      // Beispiel: Das Backend schickt ein JWT im Feld "token"
      const token = data.token;

      // Speichere das Token im localStorage
      localStorage.setItem("jwt_token", token);

      // Weiterleitung zur MainPage
      navigate("/main");
    } catch (error) {
      setErrorMessage("Fehler beim Login")
      // Hier könntest du Fehlermeldungen behandeln
      console.error(error);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="w-full max-w-sm bg-white p-6 rounded shadow">
        <h1 className="text-xl font-semibold mb-4 text-center">Login</h1>

        {errorMessage && (
          <p className="text-red-600 text-sm text-center mb-4">
            {errorMessage}
          </p>
        )}

        <form onSubmit={handleLogin} className="space-y-4">
          <div>
            <label className="block text-gray-700 mb-1">Nutzername</label>
            <input
              type="text"
              className="w-full border border-gray-300 p-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-400"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Username"
            />
          </div>
          <div>
            <label className="block text-gray-700 mb-1">Passwort</label>
            <input
              type="password"
              className="w-full border border-gray-300 p-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-400"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="••••••••"
            />
          </div>

          <button
            type="submit"
            className="w-full bg-blue-500 text-white py-2 rounded hover:bg-blue-600 transition-colors"
          >
            Einloggen
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
