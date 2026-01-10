import { useEffect, useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import { toast } from "../ui/Toast";
import { create } from "../../api/apiWrapper";
import Input from "../ui/Input";
import { Store, User } from "lucide-react";
import FormActions from "../UserManagementPage.jsx/FormActions";
import { useAuth } from "../../hooks/useAuth";
import "./BecomeSellerFrom.css";

const initialFormState = {
  websiteUrl: "",
  businessName: "",
};

const BecomeSellerFrom = ({ closeModal }) => {
  const { translations, language } = useLanguage();
  const [formData, setFormData] = useState(initialFormState);
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState({});
  const { setUser } = useAuth();
  const { become_seller, become_seller_success, become_seller_error } =
    translations.general.sidebar;

  useEffect(() => {
    // Re-validate form whenever language changes
    if (Object.keys(errors).length > 0) {
      validateFormData();
    }
  }, [language]);

  const updateField = (name, value) => {
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const validateFormData = () => {
    let temp = {};
    const msgs = translations.general.form.errors;

    if (!formData.businessName.trim()) {
      temp.businessName = msgs.required;
    }

    setErrors(temp);

    return Object.keys(temp).length === 0; // true = valid
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateFormData()) {
      toast.show(translations.general.form.messages.general_error, "error");
      return;
    }

    await becomeSeller();
  };

  async function becomeSeller() {
    try {
      let result = await create(`customers/become-seller`, formData);
      setUser((prev) => ({
        ...prev,
        roles: [...(prev.roles || []), result],
      }));
      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show(become_seller_success, "success");
      }
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error"
        );
      } else {
        toast.show(become_seller_error, "error");
      }
    } finally {
      closeModal();
    }
  }

  const clearRegisterForm = (e) => {
    e?.preventDefault();
    setFormData(initialFormState);
  };

  return (
    <form className="form become-seller-form" onSubmit={handleSubmit}>
      <h2>{become_seller}</h2>
      <Input
        label={translations.general.form.business_name}
        name="businessName"
        placeholder={translations.general.form.business_name}
        type="text  "
        icon={<Store />}
        value={formData.businessName}
        onChange={(e) => updateField("businessName", e.target.value)}
        errorMessage={errors.businessName}
      />
      <Input
        label={translations.general.form.website_url}
        name="websiteUrl"
        placeholder={translations.general.form.website_url}
        type="url"
        value={formData.websiteUrl}
        icon={<User />}
        onChange={(e) => updateField("websiteUrl", e.target.value)}
        errorMessage={errors.websiteUrl}
      />

      <FormActions
        cancel={closeModal}
        clearRegisterForm={clearRegisterForm}
        isLoading={isLoading}
      />
    </form>
  );
};
export default BecomeSellerFrom;
