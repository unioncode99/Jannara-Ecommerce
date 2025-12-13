import { Loader2, Mail } from "lucide-react";
import Input from "../components/ui/Input";
import { useLanguage } from "../hooks/useLanguage";
import "./ForgetPasswordPage.css";
import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import { toast } from "../components/ui/Toast";

const ForgetPasswordPage = () => {
  const [email, setEmail] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const { translations } = useLanguage();
  const navigate = useNavigate();
  const handleSubmit = (e) => {
    e.preventDefault();
    toast.show("Not implemented yet", "error");
    navigate("/verify-code", { state: { email } });
  };

  return (
    <div className="forget-password-page">
      <form className="form" onSubmit={handleSubmit}>
        <div className="form-header">
          <h2>{translations.general.pages.forget_password.title}</h2>
          <p>{translations.general.pages.forget_password.subtitle}</p>
        </div>
        <Input
          label={translations.general.form.email}
          name="email"
          placeholder={translations.general.form.email}
          icon={<Mail />}
          type="email"
          required={true}
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <button type="submit">
          {isLoading ? (
            <Loader2 className="spinner" />
          ) : (
            translations.general.pages.forget_password.form.submit_button
          )}
        </button>
        <div className="link-container">
          <Link to="/login">
            {translations.general.pages.forget_password.form.back_to_login}
          </Link>
        </div>
      </form>
    </div>
  );
};

export default ForgetPasswordPage;
