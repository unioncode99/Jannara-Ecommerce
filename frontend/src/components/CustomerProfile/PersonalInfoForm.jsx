import { useEffect, useState } from "react";
import Input from "../ui/Input";
import { useLanguage } from "../../hooks/useLanguage";
import { CalendarRange, Camera, Loader2, Phone, User } from "lucide-react";
import Select from "../ui/Select";
import Button from "../ui/Button";
import "./PersonalInfoForm.css";
import { update } from "../../api/apiWrapper";
import { toast } from "../ui/Toast";
import { useAuth } from "../../hooks/useAuth";

const initialFormState = {
  firstName: "",
  lastName: "",
  phone: "",
  gender: "",
  dateOfBirth: "",
};

const PersonalInfoForm = () => {
  const [formData, setFormData] = useState(initialFormState);
  const [profileImageFile, setProfileImageFile] = useState(null);
  const [profileImagePreview, setProfileImagePreview] = useState(null);
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [deleteProfileImage, setDeleteProfileImage] = useState(false);
  const { person, setPerson, user } = useAuth();

  useEffect(() => {
    setFormData({
      firstName: person?.firstName || "",
      lastName: person?.lastName || "",
      phone: person?.phone || "",
      gender: person?.gender || "",
      dateOfBirth: person?.dateOfBirth || "",
    });
    setProfileImagePreview(person?.imageUrl || null);
  }, [person]);

  console.log("person", person);
  const { translations, language } = useLanguage();
  const { first_name, last_name, phone, date_of_birth, gender } =
    translations.general.form;
  const { save, personal_info } = translations.general.pages.customer_profile;

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

  function validateFormData() {
    let temp = {};
    const msgs = translations.general.form.errors;

    if (!formData.firstName?.trim()) {
      temp.firstName = msgs.required;
    }

    if (!formData.lastName?.trim()) {
      temp.lastName = msgs.required;
    }

    if (!formData.phone?.trim() || formData.phone.length < 8) {
      temp.phone = msgs.invalid_phone;
    }

    if (!formData.gender) {
      temp.gender = msgs.required;
    }

    if (!formData.dateOfBirth) {
      temp.dateOfBirth = msgs.invalid_date;
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

    await updatePersonalInfo();
  };

  async function updatePersonalInfo() {
    setLoading(true);
    let response_message = "";
    const { server_messages } = translations.general;
    try {
      const payload = new FormData();
      for (let key in formData) {
        payload.append(key, formData[key]);
      }

      payload.append("id", person.id);

      if (profileImageFile) {
        payload.append("ProfileImage", profileImageFile);
      }

      if (deleteProfileImage) {
        payload.append("deleteProfileImage", true);
      }

      const result = await update(`people/${person.id}`, payload);

      console.log("result ", result);

      setPerson(result);
      setDeleteProfileImage(false);
      setProfileImageFile(null);

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
    <form onSubmit={handleSubmit} className="personal-info-form">
      <h2>{personal_info}</h2>
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
        <span>
          {language == "en" ? user?.roles[0].nameEn : user?.roles[0].nameAr}
        </span>
      </div>
      <Input
        label={first_name}
        name="firstName"
        placeholder={first_name}
        icon={<User />}
        value={formData.firstName}
        onChange={(e) => updateField("firstName", e.target.value)}
        errorMessage={errors.firstName}
      />
      <Input
        label={last_name}
        name="lastName"
        placeholder={last_name}
        icon={<User />}
        value={formData.lastName}
        onChange={(e) => updateField("lastName", e.target.value)}
        errorMessage={errors.lastName}
      />
      <Input
        label={phone}
        name="phone"
        placeholder={phone}
        icon={<Phone />}
        value={formData.phone}
        onChange={(e) => updateField("phone", e.target.value)}
        errorMessage={errors.phone}
      />
      <Select
        showLabel={true}
        options={language == "en" ? genderOptions.en : genderOptions.ar}
        value={formData.gender}
        label={gender}
        onChange={(e) => updateField("gender", e.target.value)}
        errorMessage={errors.gender}
      />
      <Input
        label={date_of_birth}
        name="dateOfBirth"
        placeholder={date_of_birth}
        type="date"
        icon={<CalendarRange />}
        value={formData.dateOfBirth}
        onChange={(e) => updateField("dateOfBirth", e.target.value)}
        errorMessage={errors.dateOfBirth}
      />
      <div className="btns-container">
        <Button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? <Loader2 className="animate-spin" /> : save}
        </Button>
      </div>
    </form>
  );
};
export default PersonalInfoForm;
