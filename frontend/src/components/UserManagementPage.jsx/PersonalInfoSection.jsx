import { CalendarRange, Camera, Phone, User } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import Input from "../ui/Input";
import Select from "../ui/Select";

const PersonalInfoSection = ({
  profileImagePreview,
  handleProfileImageChange,
  formData,
  updateField,
  errors,
}) => {
  const { translations, language } = useLanguage();
  const { personal_info, first_name, last_name, date_of_birth, phone } =
    translations.general.form;

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

  return (
    <div className="form-section">
      <h3>{personal_info}</h3>
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
        options={language == "en" ? genderOptions.en : genderOptions.ar}
        value={formData.gender}
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
    </div>
  );
};
export default PersonalInfoSection;
