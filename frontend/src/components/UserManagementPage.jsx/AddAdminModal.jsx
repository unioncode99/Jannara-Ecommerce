import { useLanguage } from "../../hooks/useLanguage";
import Modal from "../ui/Modal";
import AddAdminForm from "./AddAdminForm";

const AddAdminModal = ({ show, onClose, setIsAdminAdded }) => {
  const { translations } = useLanguage();

  const { add_admin } = translations.general.pages.users_management;

  async function AddAdmin() {}

  return (
    <Modal
      show={show}
      onClose={onClose}
      title={add_admin}
      // className="confirm-modal"
    >
      <AddAdminForm closeModal={onClose} setIsAdminAdded={setIsAdminAdded} />
    </Modal>
  );
};
export default AddAdminModal;
