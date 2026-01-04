import { useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import Button from "../ui/Button";
import { Hash, Loader2, Mail, User } from "lucide-react";
import Input from "../ui/Input";
import { toast } from "../ui/Toast";
import { update } from "../../api/apiWrapper";
import "./UserInfoForm.css";

import { useLanguage } from "../../hooks/useLanguage";

const initialFormState = {
  username: "",
};

const UserInfoForm = () => {
  const [formData, setFormData] = useState(initialFormState);
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const { user, setUser } = useAuth();

  useEffect(() => {
    setFormData({
      username: user?.username || "",
    });
  }, [user]);

  console.log("user", user);
  const { translations, language } = useLanguage();
  const { email, username } = translations.general.form;
  const { user_info, save } = translations.general.pages.customer_profile;

  const updateField = (name, value) => {
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  function validateFormData() {
    let temp = {};
    const msgs = translations.general.form.errors;

    if (!formData.username?.trim()) {
      temp.username = msgs.required;
    }

    setErrors(temp);

    return Object.keys(temp).length === 0; // true = valid
  }

  useEffect(() => {
    // Re-validate form whenever language changes
    if (Object.keys(errors).length > 0) {
      validateFormData();
    }
  }, [language]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateFormData()) {
      toast.show(translations.general.form.messages.general_error, "error");
      return;
    }

    await updateUserInfo();
  };

  async function updateUserInfo() {
    setLoading(true);
    let response_message = "";
    const { server_messages } = translations.general;
    try {
      const payload = {
        ...formData,
        id: user?.id,
      };

      const result = await update(`users/${user.id}`, payload);

      console.log("result ", result);
      setUser(result);

      response_message = result?.message?.message;

      if (server_messages[response_message]) {
        toast.show(server_messages[response_message], "success");
      } else {
        toast.show(
          translations.general.form.messages.general_success,
          "success"
        );
      }
    } catch (err) {
      if (server_messages[err.message]) {
        toast.show(server_messages[err.message], "error");
      } else {
        toast.show(translations.general.form.messages.general_error, "error");
      }
      console.log("Error -> ", err?.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <form onSubmit={handleSubmit} className="user-info-form">
      <h2>{user_info}</h2>
      <Input
        label={email}
        name="email"
        type="email"
        placeholder={email}
        icon={<Mail />}
        value={user?.email || ""}
        readOnly
      />
      <Input
        label={username}
        name="username"
        placeholder={username}
        icon={<Hash />}
        value={formData.username}
        onChange={(e) => updateField("username", e.target.value)}
        errorMessage={errors.username}
      />

      <div className="btns-container">
        <Button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? <Loader2 className="animate-spin" /> : save}
        </Button>
      </div>
    </form>
  );
};
export default UserInfoForm;
