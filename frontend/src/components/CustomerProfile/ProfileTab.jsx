import { useAuth } from "../../hooks/useAuth";
import PersonalInfoForm from "./PersonalInfoForm";
import SellerInfoForm from "./SellerInfoForm";
import UserInfoForm from "./UserInfoForm";

const ProfileTab = () => {
  const { currentRole } = useAuth();
  return (
    <div>
      <PersonalInfoForm />
      <UserInfoForm />
      {currentRole == "seller" && <SellerInfoForm />}
    </div>
  );
};
export default ProfileTab;
