import { useEffect, useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import { toast } from "../ui/Toast";
import { create } from "../../api/apiWrapper";
import PersonalInfoSection from "./PersonalInfoSection";
import UserInfoSection from "./UserInfoSection";
import FormActions from "./FormActions";
import "./AddAdminForm.css";

const initialFormState = {
  firstName: "",
  lastName: "",
  phone: "",
  gender: "",
  dateOfBirth: "",
  email: "",
  username: "",
  password: "",
};

const AddAdminForm = ({ closeModal, setIsAdminAdded }) => {
  const { translations, language } = useLanguage();
  const [formData, setFormData] = useState(initialFormState);
  const [profileImageFile, setProfileImageFile] = useState(null);
  const [profileImagePreview, setProfileImagePreview] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState({});
  const { add_admin, add_admin_success, add_admin_error } =
    translations.general.pages.users_management;

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

    if (!formData.firstName.trim()) {
      temp.firstName = msgs.required;
    }

    if (!formData.lastName.trim()) {
      temp.lastName = msgs.required;
    }

    if (!formData.phone.trim() || formData.phone.length < 8) {
      temp.phone = msgs.invalid_phone;
    }

    if (!formData.gender) {
      temp.gender = msgs.required;
    }

    if (!formData.dateOfBirth) {
      temp.dateOfBirth = msgs.invalid_date;
    }

    if (!formData.email.includes("@")) {
      temp.email = msgs.invalid_email;
    }

    if (formData.username.length < 3) {
      temp.username = msgs.required;
    }

    if (formData.password.length < 6) {
      temp.password = msgs.weak_password;
    }

    setErrors(temp);

    return Object.keys(temp).length === 0; // true = valid
  };

  const handleProfileImageChange = (e) => {
    const file = e.target.files[0];
    if (!file) {
      return;
    }

    setProfileImageFile(file);

    const imageUrl = URL.createObjectURL(file);
    setProfileImagePreview(imageUrl);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateFormData()) {
      toast.show(translations.general.form.messages.general_error, "error");
      return;
    }
    try {
      setIsLoading(true);
      const payload = new FormData();
      for (let key in formData) {
        payload.append(key, formData[key]);
      }

      if (profileImageFile) {
        payload.append("ProfileImage", profileImageFile);
      }

      let result = await create(`users`, payload);

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show(add_admin_success, "success");
      }
      setIsAdminAdded(true);
      clearRegisterForm(e);
      closeModal();
    } catch (err) {
      if (translations.general.server_messages[err.message]) {
        toast.show(translations.general.server_messages[err.message], "error");
      } else {
        toast.show(add_admin_error, "error");
      }
      console.log("Error -> ", err?.message);
    } finally {
      setIsLoading(false);
    }
  };

  const clearRegisterForm = (e) => {
    e?.preventDefault();
    setFormData(initialFormState);
    setProfileImageFile(null);
    setProfileImagePreview(null);
  };

  return (
    <form className="form add-admin-form" onSubmit={handleSubmit}>
      <h2>{add_admin}</h2>
      <PersonalInfoSection
        formData={formData}
        profileImagePreview={profileImagePreview}
        handleProfileImageChange={handleProfileImageChange}
        updateField={updateField}
        errors={errors}
      />
      <UserInfoSection
        formData={formData}
        updateField={updateField}
        errors={errors}
      />
      <FormActions
        cancel={closeModal}
        clearRegisterForm={clearRegisterForm}
        isLoading={isLoading}
      />
    </form>
  );
};
export default AddAdminForm;
