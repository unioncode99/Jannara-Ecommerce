import { Code, Loader2, Mail } from "lucide-react";
import React, { useState } from "react";
import Input from "../components/ui/Input";
import { Link, useLocation, useNavigate } from "react-router-dom";
import "./VerifyCodePage.css";
import Button from "../components/ui/Button";
import { useLanguage } from "../hooks/useLanguage";
import { create } from "../api/apiWrapper";
import { toast } from "../components/ui/Toast";

const VerifyCodePage = () => {
  const { translations } = useLanguage();
  const [code, setCode] = useState("");
  const [isVerifying, setIsVerifying] = useState(false);
  const [isResending, setIsResending] = useState(false);
  const navigate = useNavigate();
  const handleCodeChange = (e) => {
    if (/^\d*$/.test(e.target.value)) {
      setCode(e.target.value);
    }
  };
  const maskedEmail = (email) => {
    if (!email) return "";
    const [user, domain] = email.split("@");
    if (user.length <= 2) return `**@${domain}`;
    const visible = user.slice(0, 2);
    return `${visible}****@${domain}`;
  };
  const location = useLocation();
  const email = location.state?.email || "";
  if (!email)
    throw new Error(
      "Email is required to display VerifyCodePage. Please navigate here from the appropriate flow."
    );

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsVerifying(true);
    try {
      const response = await create("auth/verify-code", { code });
      if (response.purpose == "VerifyEmail") {
        navigate(`/confirm-account?token=${response.token}`);
      } else if (response.purpose == "ResetPassword") {
        navigate(`/reset-password?token=${response.token}`);
      } else {
        throw new Error("Unknown verification purpose.");
      }
    } catch (error) {
      toast.show(error.message, "error");
    } finally {
      setIsVerifying(false);
    }
  };
  const handleResend = async () => {
    setIsResending(true);
    try {
      await create("auth/resend-code", { email });
      toast.show("Verification code resent.", "success");
    } catch (error) {
      console.log(error);
      toast.show(error.message, "error");
    } finally {
      setIsResending(false);
    }
  };
  return (
    <div className="verify-code-page">
      <form className="form" onSubmit={handleSubmit}>
        <div className="form-header">
          <h2>{translations.general.pages.verify_code.title}</h2>
          <p>
            {translations.general.pages.verify_code.message_prefix}{" "}
            <strong>{maskedEmail(email)}</strong>.{" "}
            {translations.general.pages.verify_code.message_suffix}
          </p>
        </div>

        <div>
          <Input
            type="text"
            label="Verification Code"
            name="code"
            placeholder={
              translations.general.pages.verify_code.form.input_placeholder
            }
            required={true}
            maxLength={4}
            minLength={4}
            value={code}
            onChange={handleCodeChange}
          />
        </div>
        <Button
          disabled={isVerifying || isResending}
          className="btn btn-primary btn-block"
          type="submit"
        >
          {isVerifying ? (
            <Loader2 className="animate-spin" />
          ) : (
            translations.general.pages.verify_code.form.submit_button
          )}
        </Button>
        <Button
          disabled={isResending || isVerifying}
          className=" btn btn-primary-outline "
          onClick={handleResend}
        >
          {isResending ? (
            <Loader2 className="animate-spin" />
          ) : (
            translations.general.pages.verify_code.form.resend_code
          )}
        </Button>
        <div className="link-container">
          <Link to="/login">
            {translations.general.pages.verify_code.back_to_login}
          </Link>
        </div>
      </form>
    </div>
  );
};

export default VerifyCodePage;
