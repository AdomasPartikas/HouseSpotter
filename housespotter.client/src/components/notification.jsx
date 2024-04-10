import React from 'react';
import { useNotification } from "../contexts/NotificationContext";

const Notification = () => {
  const { notification, clearNotification } = useNotification();

  if (!notification.message) return null;

  return (
    <div
      className={`notification ${notification.type === "error" ? "error" : ""}`}
    >
      <div className="layout">
        <div className="notification__content">
          {notification.message}
          <span
            className="notification__close"
            onClick={clearNotification}
            data-testid="close-button"
          ></span>
        </div>
      </div>
    </div>
  );
};

export default Notification;
