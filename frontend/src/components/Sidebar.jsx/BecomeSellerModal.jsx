import { useLanguage } from "../../hooks/useLanguage";
import Modal from "../ui/Modal";
import BecomeSellerFrom from "./BecomeSellerFrom";

const BecomeSellerModal = ({ show, onClose }) => {
  const { translations } = useLanguage();
  const { become_seller } = translations.general.sidebar;

  return (
    <Modal show={show} onClose={onClose} title={become_seller}>
      <BecomeSellerFrom closeModal={onClose} />
    </Modal>
  );
};
export default BecomeSellerModal;
