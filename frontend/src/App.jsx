import "./App.css";
import Register from "./pages/Register";
import { useLanguage } from "./hooks/useLanguage";
import LoginPage from "./pages/LoginPage";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import ToastContainer from "./components/ui/Toast";
import NotFoundPage from "./pages/NotFoundPage";
import AppSettings from "./components/AppSettings";

function App() {
  const { language } = useLanguage();
  return (
    // <div dir={language === "en" ? "ltr" : "rtl"}>
    <div>
      <ToastContainer />
      <AppSettings />
      <BrowserRouter>
        <Routes>
          {/* <Route path="/" element={<h1>Home Page</h1>} /> */}
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
