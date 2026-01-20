import { useEffect, useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import { read, update } from "../../api/apiWrapper";
import { toast } from "../ui/Toast";
import Input from "../ui/Input";
import { Loader2, Store, User } from "lucide-react";
import Button from "../ui/Button";

const initialFormState = {
  websiteUrl: "",
  businessName: "",
};
const SellerInfoForm = () => {
  const [formData, setFormData] = useState(initialFormState);
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [currentSeller, setCurrentSeller] = useState(null);

  const { translations, language } = useLanguage();

  const { seller_info, save } = translations.general.pages.customer_profile;

  const updateField = (name, value) => {
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  useEffect(() => {
    fetchSellerInfo();
  }, []);

  function validateFormData() {
    let temp = {};
    const msgs = translations.general.form.errors;

    if (!formData.businessName.trim()) {
      temp.businessName = msgs.required;
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

  async function updateSellerInfo() {
    setLoading(true);
    let response_message = "";
    const { server_messages } = translations.general;
    try {
      const payload = {
        ...formData,
      };

      const result = await update(`sellers`, payload);

      console.log("result ", result);
      setFormData({
        websiteUrl: result.websiteUrl,
        businessName: result.businessName,
      });

      response_message = result?.message?.message;

      if (server_messages[response_message]) {
        toast.show(server_messages[response_message], "success");
      } else {
        toast.show(
          translations.general.form.messages.general_success,
          "success",
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

  async function fetchSellerInfo() {
    setLoading(true);
    let response_message = "";
    const { server_messages } = translations.general;
    try {
      const result = await read(`sellers/me`);

      console.log("result ", result);
      setCurrentSeller(result);

      setFormData({
        websiteUrl: result.websiteUrl,
        businessName: result.businessName,
      });
      //   response_message = result?.message?.message;

      //   if (server_messages[response_message]) {
      //     toast.show(server_messages[response_message], "success");
      //   } else {
      //     toast.show(
      //       translations.general.form.messages.general_success,
      //       "success",
      //     );
      //   }
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

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateFormData()) {
      toast.show(translations.general.form.messages.general_error, "error");
      return;
    }

    await updateSellerInfo();
  };

  return (
    <form onSubmit={handleSubmit} className="user-info-form">
      <h2>{seller_info}</h2>
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
      <div className="btns-container">
        <Button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? <Loader2 className="animate-spin" /> : save}
        </Button>
      </div>
    </form>
  );
};
export default SellerInfoForm;
