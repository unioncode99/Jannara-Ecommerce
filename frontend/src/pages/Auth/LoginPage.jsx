import React, { useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import Input from "../../components/ui/Input";
import { Loader2, LockIcon, Mail, User } from "lucide-react";
import Button from "../../components/ui/Button";
import { create, setAuthToken } from "../../api/apiWrapper";
import { toast } from "../../components/ui/Toast";
import "./LoginPage.css";
import { useAuth } from "../../hooks/useAuth";
import { Link, useNavigate } from "react-router-dom";

const LoginPage = () => {
  const { translations } = useLanguage();
  const [isLoading, setIsLoading] = useState(false);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { login } = useAuth();
  const navigate = useNavigate();

  const { login_success, login_failed } = translations.general.pages.auth;

  function navigateBasedOnRole(role) {
    switch (role) {
      case "customer":
        navigate("/customer-dashboard");
        break;
      case "seller":
        navigate("/seller-dashboard");
        break;
      case "admin":
        navigate("/admin-dashboard");
        break;
      case "superadmin":
        navigate("/superadmin-dashboard");
        break;
      default:
        navigate("/login");
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    try {
      const result = await create("auth/login", { email, password });
      toast.show(login_success, "success");
      console.log(result);
      login(result.user, result.person, result.accessToken);
      setAuthToken(result.accessToken.token);
      navigateBasedOnRole(result?.user?.roles[0]?.nameEn.toLowerCase());
      console.log(result.accessToken);
    } catch (error) {
      toast.show(error.message || login_failed, "error");
    } finally {
      setIsLoading(false);
    }
  };
  return (
    <div className="login-page">
      <form className="form" onSubmit={handleSubmit}>
        <div className="form-header">
          <h2 className="form-title">
            {translations.general.form.login_title}
          </h2>
          <p>{translations.general.form.login_subtitle}</p>
        </div>
        <div>
          <Input
            label={translations.general.form.email}
            name="email"
            placeholder={translations.general.form.email}
            icon={<Mail />}
            required={true}
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          <Input
            label={translations.general.form.password}
            name="password"
            placeholder={translations.general.form.password}
            icon={<LockIcon />}
            type="password"
            required={true}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <div className="form-options">
          <div className="remember-me">
            <input
              className="remember-me_checkbox"
              type="checkbox"
              name="rememberMe"
              id="rememberMe"
            />
            <label className="remember-me_label" htmlFor="rememberMe">
              {translations.general.form.remember_me}
            </label>
          </div>
          <Link to="/forget-password" className="forgot-password-link">
            {translations.general.form.forgot_password}
          </Link>
        </div>

        <Button
          disabled={isLoading}
          type="submit"
          className="btn btn-primary btn-block"
        >
          {isLoading ? (
            <Loader2 className="animate-spin" />
          ) : (
            translations.general.form.login_button
          )}
        </Button>
        <p className="form-hint">
          {translations.general.form.no_account}{" "}
          <Link to="/register">{translations.general.form.register_title}</Link>
        </p>
      </form>
    </div>
  );
};

export default LoginPage;
