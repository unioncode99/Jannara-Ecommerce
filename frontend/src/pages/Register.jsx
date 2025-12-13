import {
  CalendarRange,
  Camera,
  Hash,
  Loader2,
  Lock,
  Mail,
  Phone,
  Store,
  User,
} from "lucide-react";
import Input from "../components/ui/Input";
import { useLanguage } from "../hooks/useLanguage";
import { useEffect, useState } from "react";
import Select from "../components/ui/Select";
import Button from "../components/ui/Button";
import { toast } from "../components/ui/Toast";
import { create } from "../api/apiWrapper";
import "./Register.css";
import AppSettings from "../components/AppSettings";

const initialFormState = {
  role: "customer",
  firstName: "",
  lastName: "",
  phone: "",
  gender: "",
  dateOfBirth: "",
  email: "",
  username: "",
  password: "",
  websiteUrl: "",
  businessName: "",
};

const Register = () => {
  const { translations, language } = useLanguage();
  const [formData, setFormData] = useState(initialFormState);
  const [profileImageFile, setProfileImageFile] = useState(null);
  const [profileImagePreview, setProfileImagePreview] = useState(null);
  const [isLoading, setIsLoading] = useState(false);

  const [errors, setErrors] = useState({});
  useEffect(() => {
    // Re-validate form whenever language changes
    if (Object.keys(errors).length > 0) {
      validateFormData();
    }
  }, [language]);
  const genderOptions = {
    en: [
      { label: "Male", value: "male" },
      { label: "Female", value: "female" },
      { label: "Other", value: "other" },
    ],
    ar: [
      { label: "ذكر", value: "male" },
      { label: "أنثى", value: "female" },
      { label: "آخر", value: "other" },
    ],
  };

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

    if (!formData.businessName.trim() && formData.role === "seller") {
      temp.businessName = msgs.required;
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

      let result = null;
      if (formData.role === "seller") {
        result = await create("Sellers", payload);
      } else {
        result = await create("Customers", payload);
      }

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show(
          translations.general.form.messages.general_success,
          "success"
        );
      }
      clearRegisterForm(e);
    } catch (err) {
      if (translations.general.server_messages[err.message]) {
        toast.show(translations.general.server_messages[err.message], "error");
      } else {
        toast.show(translations.general.form.messages.general_error, "error");
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
    <div className="register-page">
      <AppSettings isTopLeft={true} />
      <form className="form" onSubmit={handleSubmit}>
        <h2>{translations.general.form.register_title}</h2>
        <p className="text-small">{translations.general.form.create_account}</p>

        <div className="form-row role">
          <div>
            <input
              type="radio"
              name="role"
              value="seller"
              id="seller"
              checked={formData.role === "seller"}
              onChange={(e) => updateField("role", e.target.value)}
            />
            <label htmlFor="seller">
              <Store />
              <span>{translations.general.form.seller_option}</span>
            </label>
          </div>
          <div>
            <input
              type="radio"
              name="role"
              value="customer"
              id="customer"
              checked={formData.role === "customer"}
              onChange={(e) => updateField("role", e.target.value)}
            />
            <label htmlFor="customer">
              <User />
              <span>{translations.general.form.customer_option}</span>
            </label>
          </div>
        </div>
        <div className="form-section">
          <div>
            <h3>{translations.general.form.personal_info}</h3>
            <div className="form-row profile-image">
              <label>
                <div>
                  {profileImagePreview ? (
                    <img
                      src={profileImagePreview}
                      alt="Profile Image"
                      className="profile-img"
                    />
                  ) : (
                    <Camera className="camera-icon" />
                  )}
                </div>
                <input
                  type="file"
                  accept="image/*"
                  style={{ display: "none" }}
                  onChange={handleProfileImageChange}
                />
              </label>
            </div>
            <Input
              label={translations.general.form.first_name}
              name="firstName"
              placeholder={translations.general.form.first_name}
              icon={<User />}
              value={formData.firstName}
              onChange={(e) => updateField("firstName", e.target.value)}
              errorMessage={errors.firstName}
            />
            <Input
              label={translations.general.form.last_name}
              name="lastName"
              placeholder={translations.general.form.last_name}
              icon={<User />}
              value={formData.lastName}
              onChange={(e) => updateField("lastName", e.target.value)}
              errorMessage={errors.lastName}
            />
            <Input
              label={translations.general.form.phone}
              name="phone"
              placeholder={translations.general.form.phone}
              icon={<Phone />}
              value={formData.phone}
              onChange={(e) => updateField("phone", e.target.value)}
              errorMessage={errors.phone}
            />
            <Select
              options={language == "en" ? genderOptions.en : genderOptions.ar}
              value={formData.gender}
              onChange={(e) => updateField("gender", e.target.value)}
              errorMessage={errors.gender}
            />
            <Input
              label={translations.general.form.date_of_birth}
              name="dateOfBirth"
              placeholder={translations.general.form.date_of_birth}
              type="date"
              icon={<CalendarRange />}
              value={formData.dateOfBirth}
              onChange={(e) => updateField("dateOfBirth", e.target.value)}
              errorMessage={errors.dateOfBirth}
            />
          </div>
          <div>
            <h3>{translations.general.form.user_info}</h3>
            <Input
              label={translations.general.form.email}
              name="email"
              placeholder={translations.general.form.email}
              icon={<Mail />}
              value={formData.email}
              onChange={(e) => updateField("email", e.target.value)}
              errorMessage={errors.email}
            />
            <Input
              label={translations.general.form.username}
              name="username"
              placeholder={translations.general.form.username}
              icon={<Hash />}
              value={formData.username}
              onChange={(e) => updateField("username", e.target.value)}
              errorMessage={errors.username}
            />
            <Input
              label={translations.general.form.password}
              name="password"
              placeholder={translations.general.form.password}
              type="password"
              icon={<Lock />}
              value={formData.password}
              onChange={(e) => updateField("password", e.target.value)}
              errorMessage={errors.password}
            />
            {formData.role === "seller" && (
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
            )}

            {formData.role === "seller" && (
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
            )}
          </div>
        </div>
        <Button
          className="btn btn-primary btn-block clear-btn"
          onClick={() => clearRegisterForm()}
        >
          {translations.general.form.clear_button}
        </Button>
        <Button
          disabled={isLoading}
          className={`btn-primary btn-block ${
            isLoading ? "btn-disabled" : ""
          } `}
          type="submit"
        >
          {isLoading ? (
            <Loader2 className="animate-spin" />
          ) : (
            translations.general.form.register_button
          )}
        </Button>

        <div className="link-container">
          <Link to="/login">
            {translations.general.form.already_have_account}
          </Link>
        </div>
      </form>
    </div>
  );
};
export default Register;
