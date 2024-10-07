import React, { createContext, useContext, useState } from 'react';

const NotificationContext = createContext();

export const useNotification = () => useContext(NotificationContext);

export const NotificationProvider = ({ children }) => {
  const [notification, setNotification] = useState({ message: "", type: "" });

  const notify = (message, type = "success") => {
    setNotification({ message, type });
    setTimeout(() => {
      clearNotification();
    }, 3000);
  };

  const clearNotification = () => {
    setNotification({ message: "", type: "" });
  };

  return (
    <NotificationContext.Provider value={{ notification, notify, clearNotification }}>
      {children}
    </NotificationContext.Provider>
  );
};
