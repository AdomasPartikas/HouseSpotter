import React, { useState } from 'react';
import Login from './Login.jsx';
import Register from './Register.jsx';
import './assets/styles/App.scss';

function App() {

  const [currentPage, setCurrentPage] = useState('home');
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [username, setUsername] = useState('');

  const handleLoginClick = () => {
    setCurrentPage('login');
  };

  const handleHomeClick = () => {
    setCurrentPage('home');
  };

  const onLoginSuccess = (username) => {
    setIsLoggedIn(true);
    setUsername(username);
    setCurrentPage('home');
  };

  const handleRegisterClick = () => {
    setCurrentPage('register');
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
    setUsername('');
  };

  return (
    <div className="app-container">
      <nav className="navbar">
        <div className="nav-item home">
          <button onClick={handleHomeClick}>Home</button>
        </div>
        <div className="nav-item login">
          {isLoggedIn ? (
            <button onClick={handleLogout}>Logout</button> // Clicking this logs out the user
          ) : (
            <button onClick={handleLoginClick}>Login</button>
          )}
        </div>
      </nav>
      {currentPage === 'home' && <h1>Welcome to HouseSpotter{isLoggedIn ? `, ${username}!` : '!'}</h1>}
      {currentPage === 'login' && <Login onLoginSuccess={onLoginSuccess} onRegister={handleRegisterClick} />}
      {currentPage === 'register' && <Register />}
    </div>
  );
}

export default App;