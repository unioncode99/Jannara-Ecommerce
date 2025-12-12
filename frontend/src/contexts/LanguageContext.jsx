import React, { createContext, useState } from "react";
import enGeneral from "../locales/en/general.json";
import arGeneral from "../locales/ar/general.json";

// Create a context
export const LanguageContext = createContext();

// Provider component
export const LanguageProvider = ({ children }) => {
  const [language, setLanguage] = useState("en"); // Default language is English

  // Switch between English and Arabic translations
  const translations =
    language === "en" ? { general: enGeneral } : { general: arGeneral };

  const toggleLanguage = () => {
    setLanguage((prev) => (prev === "en" ? "ar" : "en"));
  };

  return (
    <LanguageContext.Provider
      value={{ language, translations, toggleLanguage }}
    >
      {children}
    </LanguageContext.Provider>
  );
};
