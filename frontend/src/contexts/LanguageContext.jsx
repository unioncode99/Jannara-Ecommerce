import React, { createContext, useEffect, useState } from "react";
import enGeneral from "../locales/en/general.json";
import arGeneral from "../locales/ar/general.json";

export const LanguageContext = createContext();

export const LanguageProvider = ({ children }) => {
  const [language, setLanguage] = useState(() => {
    return localStorage.getItem("jannara-language") || "en";
  });

  useEffect(() => {
    localStorage.setItem("jannara-language", language);
    document.documentElement.setAttribute(
      "dir",
      language === "ar" ? "rtl" : "ltr"
    );
  }, [language]);

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
