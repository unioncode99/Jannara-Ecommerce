import { Globe, Moon, Sun } from "lucide-react";
import { useLanguage } from "../hooks/useLanguage";
import { useTheme } from "../hooks/useTheme";
import "./AppSettings.css";

function AppSettings({ isTopLeft, className }) {
  const { language, toggleLanguage } = useLanguage();
  const { theme, toggleTheme } = useTheme();

  return (
    <div
      dir="ltr"
      className={`app-settings ${className} ${isTopLeft ? "top-left" : ""}`}
    >
      <button className="lang" onClick={toggleLanguage}>
        <Globe />
        <span>{language == "en" ? "AR" : "EN"}</span>
      </button>
      <button onClick={toggleTheme}>
        {theme === "light" ? <Moon /> : <Sun />}
      </button>
    </div>
  );
}
export default AppSettings;
