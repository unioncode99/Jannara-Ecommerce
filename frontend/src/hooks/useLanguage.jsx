import { useContext } from "react";
import { LanguageContext } from "../contexts/LanguageContext";

// Custom hook to use the language context
export const useLanguage = () => {
  return useContext(LanguageContext);
};
