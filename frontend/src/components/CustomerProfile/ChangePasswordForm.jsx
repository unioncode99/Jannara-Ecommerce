import { useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import { toast } from "../ui/Toast";
import Input from "../ui/Input";
import Button from "../ui/Button";
import { Key } from "lucide-react";
import { update } from "../../api/apiWrapper";

const initialFormState = {
  oldPassword: "",
  newPassword: "",
  confirmPassword: "",
};

const ChangePasswordForm = () => {
  const { translations } = useLanguage();
  const [formData, setFormData] = useState(initialFormState);
  const [errors, setErrors] = useState({});
  const {
    old_password,
    new_password,
    confirm_password,
    change_password_submit,
    password_changed,
  } = translations.general.pages.customer_profile;

  const updateField = (name, value) => {
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const validateFormData = () => {
    let temp = {};
    const msgs = translations.general.form.errors;

    if (!formData.oldPassword.trim()) {
      temp.oldPassword = msgs.required;
    }

    if (!formData.newPassword.trim()) {
      temp.newPassword = msgs.required;
    }

    if (!formData.confirmPassword.trim()) {
      temp.confirmPassword = msgs.required;
    }

    if (
      formData.newPassword.trim() &&
      formData.confirmPassword.trim() &&
      formData.newPassword.trim() !== formData.confirmPassword.trim()
    ) {
      temp.confirmPassword = msgs.passwords_do_not_match;
    }

    setErrors(temp);
    return Object.keys(temp).length === 0; // true = valid
  };

  async function handleChangePassword(e) {
    e.preventDefault();

    if (!validateFormData()) {
      toast.show(translations.general.form.messages.general_error, "error");
      return;
    }
    await changePassword();
  }

  async function changePassword() {
    try {
      await update("users/reset-password", {
        oldPassword: formData.oldPassword,
        newPassword: formData.newPassword,
      });
      setFormData(initialFormState);
      toast.show(password_changed, "success");
    } catch (error) {
      toast.show(
        error.message || translations.general.form.messages.general_error,
        "error"
      );
    }
  }

  return (
    <form onSubmit={handleChangePassword}>
      <Input
        label={old_password}
        name="old_password"
        type="password"
        placeholder={old_password}
        icon={<Key />}
        value={formData.oldPassword}
        onChange={(e) => updateField("oldPassword", e.target.value)}
        errorMessage={errors.oldPassword}
      />
      <Input
        label={new_password}
        name="new_password"
        type="password"
        placeholder={new_password}
        icon={<Key />}
        value={formData.newPassword}
        onChange={(e) => updateField("newPassword", e.target.value)}
        errorMessage={errors.newPassword}
      />
      <Input
        label={confirm_password}
        name="confirm_password"
        type="password"
        placeholder={confirm_password}
        icon={<Key />}
        value={formData.confirmPassword}
        onChange={(e) => updateField("confirmPassword", e.target.value)}
        errorMessage={errors.confirmPassword}
      />
      <Button type="submit" className="btn btn-primary btn-block">
        {change_password_submit}
      </Button>
    </form>
  );
};
export default ChangePasswordForm;
