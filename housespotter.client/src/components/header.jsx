import React from 'react';
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import { useNotification } from "../contexts/NotificationContext";

const Header = () => {
  const { user, logout } = useAuth();
  const { notify } = useNotification();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    notify("Sėkmingai atsijungėte!", "success");
    navigate("/");
  }

  return (
    <header>
      <div className="layout">
        <div className="header__col-1">
          <a href="/" className="header__logo">
            HouseSpotter
          </a>
        </div>
        <div className="header__col-2">
          <nav className="header__nav">
            {user?.isAdmin && <Link to="/scraper">Scraper</Link>}
            {user && <Link to="/megstamiausi">Mėgstamiausi</Link>}
            {user && (
              <button className="header__btn" onClick={handleLogout}>
                Atsijungti
              </button>
            )}
            {!user && (
              <Link to="/prisijungti" className="header__btn">
                Prisijungti
              </Link>
            )}
          </nav>
        </div>
      </div>
    </header>
  );
};

export default Header;