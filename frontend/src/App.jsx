import "./App.css";
import AppSettings from "./components/AppSettings";
import Register from "./pages/Register";
import { useLanguage } from "./hooks/useLanguage";
import ToastContainer from "./components/ui/Toast";

function App() {
  const { language } = useLanguage();
  return (
    <div dir={language === "en" ? "ltr" : "rtl"}>
      <AppSettings />
      <ToastContainer />
      <Register />
    </div>
  );
}

export default App;
