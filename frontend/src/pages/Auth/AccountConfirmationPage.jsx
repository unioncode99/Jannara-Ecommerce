import { CheckCircle2, Loader2 } from "lucide-react";
import { useEffect, useState } from "react";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import Button from "../../components/ui/Button";
import { useLanguage } from "../../hooks/useLanguage";
import { create } from "../../api/apiWrapper";
import { toast } from "../../components/ui/Toast";
import "./AccountConfirmationPage.css";

const AccountConfirmationPage = () => {
  const { translations } = useLanguage();
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const [status, setStatus] = useState("loading");

  const token = searchParams.get("token");

  useEffect(() => {
    if (!token) {
      setStatus("error");
      toast.show("Invalid confirmation link", "error");
      return;
    }

    const confirmAccount = async () => {
      try {
        await create("auth/confirm-account", { token });
        setStatus("success");
      } catch (error) {
        toast.show(error.message || "Confirmation failed", "error");
        setStatus("error");
      } finally {
        setIsLoading(false);
      }
    };

    confirmAccount();
  }, [token]);
  if (isLoading) {
    return (
      <div className="page-center">
        <Loader2 className="spinner" size={48} />
      </div>
    );
  }
  return (
    <div className="account-confirmation-page">
      <form className="form">
        <div className="form-header">
          {status === "success" && (
            <>
              <CheckCircle2 size={48} className="success-icon" />
              <h2>{translations.general.pages.account_confirmation.title}</h2>
              <p>{translations.general.pages.account_confirmation.subtitle}</p>
            </>
          )}

          {status === "error" && (
            <>
              <h2>Confirmation Failed</h2>
              <p>Invalid or expired confirmation link.</p>
            </>
          )}
        </div>

        <Button
          className="btn btn-primary btn-block"
          onClick={() => navigate("/login")}
        >
          {translations.general.pages.account_confirmation.login_button}
        </Button>
      </form>
    </div>
  );
};

export default AccountConfirmationPage;
