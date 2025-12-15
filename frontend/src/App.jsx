import "./App.css";
import Register from "./pages/Register";
import { useLanguage } from "./hooks/useLanguage";
import LoginPage from "./pages/LoginPage";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import ToastContainer from "./components/ui/Toast";
import NotFoundPage from "./pages/NotFoundPage";
import AppSettings from "./components/AppSettings";
import ForgetPasswordPage from "./pages/ForgetPasswordPage";
import VerifyCodePage from "./pages/VerifyCodePage";
import ResetPasswordPage from "./pages/ResetPasswordPage";
import AccountConfirmationPage from "./pages/AccountConfirmationPage";

function App() {
  const { language } = useLanguage();

  return (
    // <div dir={language === "en" ? "ltr" : "rtl"}>
    <div>
      <ToastContainer />
      <AppSettings isTopLeft={true} />
      <BrowserRouter>
        <Routes>
          {/* <Route path="/" element={<h1>Home Page</h1>} /> */}
          <Route path="*" element={<NotFoundPage />} />
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/forget-password" element={<ForgetPasswordPage />} />
          <Route path="/verify-code" element={<VerifyCodePage />} />
          <Route path="/reset-password" element={<ResetPasswordPage />} />
          <Route
            path="/confirm-account"
            element={<AccountConfirmationPage />}
          />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
