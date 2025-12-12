import { Globe, Moon, Sun } from "lucide-react";
import { useLanguage } from "../hooks/useLanguage";
import { useTheme } from "../hooks/useTheme";

function AppSettings() {
  const { language, toggleLanguage } = useLanguage();
  const { theme, toggleTheme } = useTheme();

  return (
    <div className="app-settings">
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
