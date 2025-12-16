import { Link, useNavigate, useSearchParams } from "react-router-dom";
import Button from "../../components/ui/Button";
import Input from "../../components/ui/Input";
import { Loader2, Lock } from "lucide-react";
import "./ResetPasswordPage.css";
import { useLanguage } from "../../hooks/useLanguage";
import { useEffect, useState } from "react";
import { toast } from "../../components/ui/Toast";
import { create } from "../../api/apiWrapper";
const ResetPasswordPage = () => {
  const { translations, language } = useLanguage();
  const [isLoading, setIsLoading] = useState(false);
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token");
  if (!token) throw new Error("Token is required to reset password");

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validateFormData()) {
      toast.show(translations.general.form.messages.general_error, "error");
      return;
    }
    setIsLoading(true);
    try {
      await create("auth/reset-password", { token, newPassword: password });
      toast.show("Password reset successfully", "success");
      navigate(`/login`);
    } catch (error) {
      toast.show(error.message, "error");
    } finally {
      setIsLoading(false);
    }
  };
  const validateFormData = () => {
    let temp = {};
    const msgs = translations.general.form.errors;

    if (password.length < 6) {
      temp.password = msgs.weak_password;
    }
    if (confirmPassword !== password) {
      temp.confirmPassword = msgs.passwords_do_not_match;
    }
    setErrors(temp);
    console.log(temp);
    return Object.keys(temp).length === 0; // true = valid
  };
  const [errors, setErrors] = useState({});
  useEffect(() => {
    // Re-validate form whenever language changes
    if (Object.keys(errors).length > 0) {
      validateFormData();
    }
  }, [language]);

  return (
    <div className="reset-password-page">
      <form className="form" onSubmit={handleSubmit}>
        <div className="form-header">
          <h2>{translations.general.pages.reset_password.form.titile}</h2>
          <p>{translations.general.pages.reset_password.form.subtitle}</p>
        </div>

        <Input
          type="password"
          placeholder={
            translations.general.pages.reset_password.form
              .input_placeholder_password
          }
          icon={<Lock />}
          required
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          errorMessage={errors.password}
        />

        <Input
          type="password"
          label={"Confirm Password"}
          placeholder={
            translations.general.pages.reset_password.form
              .input_placeholder_confirm_password
          }
          icon={<Lock />}
          required
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          errorMessage={errors.confirmPassword}
        />

        <Button
          disabled={isLoading}
          className="btn btn-primary btn-block"
          type="submit"
        >
          {isLoading ? (
            <Loader2 className="animate-spin" />
          ) : (
            translations.general.pages.reset_password.form.submit_button
          )}
        </Button>

        <div className="link-container">
          <Link to="/login">
            {translations.general.pages.reset_password.form.back_to_login}
          </Link>
        </div>
      </form>
    </div>
  );
};

export default ResetPasswordPage;
