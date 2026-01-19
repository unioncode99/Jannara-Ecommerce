import { useLanguage } from "../../hooks/useLanguage";
import Modal from "../ui/Modal";
import ReviewForm from "./ReviewForm";
import "./ReviewModal.css";

// entityType: "product" | "seller";
// entityId: number;

const ReviewModal = ({
  show,
  onClose,
  entityType,
  entityId,
  review,
  onSubmit,
  loading,
}) => {
  const { translations } = useLanguage();
  const { review_title } = translations.general.reviews;

  return (
    <Modal show={show} onClose={onClose} title={review_title}>
      <ReviewForm initialData={review} onSubmit={onSubmit} loading={loading} />
    </Modal>
  );
};

export default ReviewModal;
