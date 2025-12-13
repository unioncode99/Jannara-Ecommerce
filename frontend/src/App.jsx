import "./App.css";
import AppSettings from "./components/AppSettings";
import Register from "./pages/Register";
import { useLanguage } from "./hooks/useLanguage";
import ToastContainer from "./components/ui/Toast";
import LoginPage from "./pages/LoginPage";

function App() {
  const { language } = useLanguage();
  return (
    // <div dir={language === "en" ? "ltr" : "rtl"}>
    <div>
      <AppSettings />
      <ToastContainer />
      {/* <Register /> */}
      <LoginPage />
    </div>
  );
}

export default App;
