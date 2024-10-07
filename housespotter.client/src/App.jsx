import React, { useState, useEffect } from 'react';
import Login from './Login.jsx';
import Register from './Register.jsx';
import ProductCard from './components/ProductCard';
import './assets/styles/App.scss';
import './assets/styles/Notification.scss';
import Cookies from 'js-cookie';
import Notification from "./components/Notification";
import { NotificationProvider } from './contexts/NotificationContext';
import api from './net/api';


function App() {
  const [currentPage, setCurrentPage] = useState('home');
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [username, setUsername] = useState('');
  const [products, setProducts] = useState([]);

  useEffect(() => {
    const token = Cookies.get('jwt');
    const username = Cookies.get('username');
    if (token) {
      setIsLoggedIn(true);
      setUsername(username);
    }

    if (currentPage === 'home') {
      fetchProducts();
    }
  }, [currentPage]);

  const fetchProducts = async () => {
    try {
      const response = await api.get('/housespotter/db/getallhousing');
      setProducts(response.data);
    } catch (error) {
      console.error('Failed to fetch products:', error);
    }
  };

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
    Cookies.remove('jwt');
    Cookies.remove('username');
    setIsLoggedIn(false);
    setUsername('');
    setProducts([]);
  };

  return (
    <NotificationProvider>
      <div className="app-container">
        <nav className="navbar">
          <div className="nav-item home">
            <button onClick={handleHomeClick}>Home</button>
          </div>
          <div className="nav-item login">
            {isLoggedIn ? (
              <button onClick={handleLogout}>Logout</button>
            ) : (
              <button onClick={handleLoginClick}>Login</button>
            )}
          </div>
        </nav>
        {currentPage === 'home' && (
          <div>
            <h1>Welcome to HouseSpotter{isLoggedIn ? `, ${username}!` : '!'}</h1>
            <div className="products">
              <div className="layout">
                <h2>
                  {isLoggedIn ? `Skelbimai : ${products.length}` : `Norite pamatyti skelbimus? Prisijunkite!`}
                </h2>
                {products.map((product) => (
                  <ProductCard key={product.id} product={product} />
                ))}
              </div>
            </div>
          </div>
        )}
        {currentPage === 'login' && <Login onLoginSuccess={onLoginSuccess} onRegister={handleRegisterClick} />}
        {currentPage === 'register' && <Register />}
      </div>
      <Notification />
    </NotificationProvider>
  );
}

export default App;