import React from "react";

interface NavBarProps {
  onLogout: () => void;
}

const NavBar: React.FC<NavBarProps> = ({ onLogout }) => {
  return (
    <nav className="flex items-center justify-between px-6 py-4 bg-[#101010]">
      <h1 className="text-2xl font-bold text-[#007ACC]">Security Dashboard</h1>
      <button
        onClick={onLogout}
        className="bg-red-600 px-5 py-2 rounded-md text-white font-semibold text-base
                   hover:bg-red-700 transition-colors"
      >
        Logout
      </button>
    </nav>
  );
};

export default NavBar;
