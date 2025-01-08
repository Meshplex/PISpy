// App.tsx

import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import MainPage from "./pages/MainPage";

function PrivateRoute({ children }: { children: JSX.Element }) {
  // Hier prüfst du, ob ein Token vorhanden ist
  const token = localStorage.getItem("jwt_token");

  if (!token) {
    // Kein Token => Weiterleitung zur Login-Seite
    return <Navigate to="/login" replace />;
  }

  // Token vorhanden => weiter zur gewünschten Seite
  return children;
}

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        {/* Login Route */}
        <Route path="/login" element={<LoginPage />} />

        {/* Geschützte Route für die MainPage */}
        <Route
          path="/main"
          element={
            <PrivateRoute>
              <MainPage />
            </PrivateRoute>
          }
        />

        {/* Startseite oder Redirect-Fallback */}
        <Route path="/" element={<Navigate to="/login" replace />} />
      </Routes>
    </Router>
  );
};

export default App;
