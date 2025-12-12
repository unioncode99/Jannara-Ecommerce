// Toast.js
import React, { useEffect, useState } from "react";
import "./Toast.css";

let triggerToast = null;

export const toast = {
  show: (message, type = "default") => {
    triggerToast && triggerToast(message, type);
  },
};

const ToastContainer = () => {
  const [toasts, setToasts] = useState([]);

  const createToast = (message, type) => {
    const id = Date.now();
    setToasts((prev) => [...prev, { id, message, type }]);

    setTimeout(() => removeToast(id), 3000);
  };

  const removeToast = (id) => {
    setToasts((prev) => prev.filter((t) => t.id !== id));
  };

  useEffect(() => {
    triggerToast = createToast;
  }, []);

  return (
    <div className="toast-container">
      {toasts.map((t) => (
        <div key={t.id} className={`toast toast-${t.type}`}>
          <span>{t.message}</span>

          {/* Close button */}
          <button className="toast-close" onClick={() => removeToast(t.id)}>
            ✕
          </button>

          {/* Progress line (type-colored) */}
          <div className={`toast-progress toast-progress-${t.type}`}></div>
        </div>
      ))}
    </div>
  );
};

export default ToastContainer;
