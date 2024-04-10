import React, { createContext, useContext, useState } from "react";

const NotificationContext = createContext();

export const useNotification = () => useContext(NotificationContext);

export const NotificationProvider = ({ children }) => {
  const [notification, setNotification] = useState({ message: "", type: "" });

  // Show notification
  const notify = (message, type = "success") => {
    setNotification({ message, type });
  };

  // Clear notification
  const clearNotification = () => {
    setNotification({ message: "", type: "" });
  };

  return (
    <NotificationContext.Provider
      value={{ notification, notify, clearNotification }}
    >
      {children}
    </NotificationContext.Provider>
  );
};
