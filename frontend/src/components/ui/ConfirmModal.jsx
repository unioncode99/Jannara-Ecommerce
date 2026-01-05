import "./ConfirmModal.css";
import Modal from "./Modal";
import Button from "./Button";

const ConfirmModal = (props) => {
  const {
    show,
    onClose,
    onConfirm,
    message,
    title = "Are you sure you want to do this action?",
    cancelLabel = "Cancel",
    confirmLabel = "Confirm",
  } = props;

  return (
    <Modal
      show={show}
      onClose={onClose}
      title={title}
      className="confirm-modal"
    >
      <p>{message}</p>
      <div className="confirm-modal-actions">
        <Button onClick={onClose} className="cancel-btn">
          {cancelLabel}
        </Button>
        <Button onClick={onConfirm} className="btn btn-primary">
          {confirmLabel}
        </Button>
      </div>
    </Modal>
  );
};

export default ConfirmModal;
