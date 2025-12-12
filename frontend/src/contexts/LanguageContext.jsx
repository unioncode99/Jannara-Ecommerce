import React, { createContext, useEffect, useState } from "react";
import enGeneral from "../locales/en/general.json";
import arGeneral from "../locales/ar/general.json";

export const LanguageContext = createContext();

export const LanguageProvider = ({ children }) => {
  const [language, setLanguage] = useState("en");

  const translations =
    language === "en" ? { general: enGeneral } : { general: arGeneral };

  const toggleLanguage = () => {
    setLanguage((prev) => (prev === "en" ? "ar" : "en"));
  };

  useEffect(() => {
    const html = document.documentElement;
    if (language === "ar") {
      html.setAttribute("dir", "rtl");
      html.style.setProperty("--heading-font", "'Cairo', sans-serif");
      html.style.setProperty("--body-font", "'Tajawal', sans-serif");
    } else {
      html.setAttribute("dir", "ltr");
      html.style.setProperty("--heading-font", "'Roboto', sans-serif");
      html.style.setProperty("--body-font", "'Nunito', sans-serif");
    }
  }, [language]);

  return (
    <LanguageContext.Provider
      value={{ language, translations, toggleLanguage }}
    >
      {children}
    </LanguageContext.Provider>
  );
};
