import { useNavigate } from 'react-router-dom';

function Navbar() {

  const navigate = useNavigate();

  function handleLogout() {
    localStorage.removeItem('token');
    navigate('/login');
  }

  return (
    <nav className="fixed top-0 left-0 w-full h-16 bg-gray-950 text-blue-400 flex items-center justify-between px-6 shadow-md z-50">
      <div className="text-xl font-bold tracking-wide">🔒 PiSpy</div>
      <button onClick={handleLogout} className="text-sm bg-red-600 hover:bg-red-500 text-white px-4 py-1.5 rounded">
        Logout
      </button>
    </nav>
  );
}

export default Navbar;