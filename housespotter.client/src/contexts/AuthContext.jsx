import React, { createContext, useContext, useState } from "react";

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(() =>
    JSON.parse(localStorage.getItem("user"))
  );

  const login = (userData) => {
    setUser(userData);
    localStorage.setItem("user", JSON.stringify(userData));
  };

  const logout = () => {
    setUser(null);
    localStorage.removeItem("user");
  };

  const [refreshFavoriteHousing, setRefreshFavoriteHousing] = useState(false);

  const refreshFavorites = () => {
    setRefreshFavoriteHousing(!refreshFavoriteHousing);
  }

  return (
    <AuthContext.Provider value={{ user, login, logout, refreshFavorites, refreshFavoriteHousing }}>
      {children}
    </AuthContext.Provider>
  );
};