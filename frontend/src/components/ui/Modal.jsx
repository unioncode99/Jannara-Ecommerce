import { X } from "lucide-react";
import Button from "./Button";
import "./Modal.css"; // Import the modal styles

const Modal = ({ show, onClose, title, children, footer }) => {
  if (!show) return null;

  return (
    <div className="modal-overlay">
      <div className="modal">
        <div className="modal-header">
          <h3>{title}</h3>
          <Button
            onClick={onClose}
            className="close-btn"
            aria-label="Close Modal"
            type="button"
          >
            <X />
          </Button>
        </div>
        <div className="modal-body">{children}</div>
        {footer && <div className="modal-footer">{footer}</div>}
      </div>
    </div>
  );
};

export default Modal;
