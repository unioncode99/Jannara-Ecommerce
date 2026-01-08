import { Hash, Lock, Mail } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import Input from "../ui/Input";

const UserInfoSection = ({ formData, updateField, errors }) => {
  const { translations } = useLanguage();
  const { user_info, email, username, password } = translations.general.form;

  return (
    <div>
      <h3>{user_info}</h3>
      <Input
        label={email}
        name="email"
        placeholder={email}
        icon={<Mail />}
        value={formData.email}
        onChange={(e) => updateField("email", e.target.value)}
        errorMessage={errors.email}
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
      <Input
        label={password}
        name="password"
        placeholder={password}
        type="password"
        icon={<Lock />}
        value={formData.password}
        onChange={(e) => updateField("password", e.target.value)}
        errorMessage={errors.password}
      />
    </div>
  );
};
export default UserInfoSection;
