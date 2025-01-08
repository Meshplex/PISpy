// src/context/AuthContext.tsx
import { createContext, useState, ReactNode, useContext } from 'react';

interface AuthContextType {
  isAuthenticated: boolean;
  login: () => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType>({
  isAuthenticated: false,
  login: () => {},
  logout: () => {},
});

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  const login = () => {
    // Hier würde man in einer echten App ggf. ein Token bekommen,
    // in localStorage speichern o.Ä.
    setIsAuthenticated(true);
  };

  const logout = () => {
    setIsAuthenticated(false);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// Kleiner Hilfs-Hook, um den AuthContext bequem zu verwenden
export const useAuth = () => {
  return useContext(AuthContext);
};
