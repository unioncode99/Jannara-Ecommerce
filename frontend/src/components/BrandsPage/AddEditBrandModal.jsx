import { useEffect, useState } from "react";
import Modal from "../ui/Modal";
import { useLanguage } from "../../hooks/useLanguage";
import Input from "../ui/Input";
import Button from "../ui/Button";
import TextArea from "../ui/TextArea";
import { toast } from "../ui/Toast";

const initialFormState = {
  nameEn: "",
  nameAr: "",
  descriptionEn: "",
  descriptionAr: "",
  logoUrl: "",
  websiteUrl: "",
};

const AddEditBrandModal = ({
  show,
  onClose,
  onConfirm,
  brand,
  brands = [],
}) => {
  const [formData, setFormData] = useState(initialFormState);
  const { translations, language } = useLanguage();
  const [errors, setErrors] = useState({});

  const { name_en, name_ar, description_en, description_ar } =
    translations.general.form;

  useEffect(() => {
    setFormData({
      nameEn: brand?.nameEn || "",
      nameAr: brand?.nameAr || "",
      descriptionEn: brand?.descriptionEn || "",
      descriptionAr: brand?.descriptionAr || "",
      logoUrl: brand?.logoUrl || "",
      websiteUrl: brand?.websiteUrl || "",
    });
  }, [brand]);

  const updateField = (name, value) => {
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const validateFormData = () => {
    let temp = {};
    const msgs = translations.general.form.errors;

    if (!formData.nameEn.trim()) {
      temp.nameEn = msgs.required;
    }

    if (!formData.nameAr.trim()) {
      temp.nameAr = msgs.required;
    }
    if (!formData.websiteUrl.trim()) {
      temp.websiteUrl = msgs.required;
    }
    if (!formData.logoUrl.trim()) {
      temp.logoUrl = msgs.required;
    }

    setErrors(temp);

    return Object.keys(temp).length === 0; // true = valid
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateFormData()) {
      toast.show(translations.general.form.messages.general_error, "error");
      return;
    }

    let payload = {};

    if (formData.nameEn) {
      payload.nameEn = formData.nameEn;
    }
    if (formData.nameAr) {
      payload.nameAr = formData.nameAr;
    }
    if (formData.descriptionEn) {
      payload.descriptionEn = formData.descriptionEn;
    }
    if (formData.descriptionAr) {
      payload.descriptionAr = formData.descriptionAr;
    }
    if (formData.logoUrl) {
      payload.logoUrl = formData.logoUrl;
    }
    if (formData.websiteUrl) {
      payload.websiteUrl = formData.websiteUrl;
    }

    onConfirm?.(payload);
  };

  return (
    <div className="add-edit-brand-modal">
      <Modal
        show={show}
        onClose={onClose}
        title={
          <>
            <span>{brand ? "edit_brand" : "add_brand"}</span>
          </>
        }
        className="confirm-modal"
      >
        <form className="add-edit-brand-form">
          <Input
            label={name_en}
            name="nameEn"
            placeholder={name_en}
            value={formData.nameEn}
            onChange={(e) => updateField("nameEn", e.target.value)}
            errorMessage={errors.nameEn}
          />
          <Input
            label={name_ar}
            name="nameAr"
            placeholder={name_ar}
            value={formData.nameAr}
            onChange={(e) => updateField("nameAr", e.target.value)}
            errorMessage={errors.nameAr}
          />
          <TextArea
            label={description_en}
            name="descriptionEn"
            placeholder={description_en}
            value={formData.descriptionEn}
            onChange={(e) => updateField("descriptionEn", e.target.value)}
            errorMessage={errors.descriptionEn}
          />
          <TextArea
            label={description_ar}
            name="descriptionAr"
            placeholder={description_ar}
            value={formData.descriptionAr}
            onChange={(e) => updateField("descriptionAr", e.target.value)}
            errorMessage={errors.descriptionAr}
          />
          <Input
            label={"name_ar"}
            name="websiteUrl"
            placeholder={"name_ar"}
            value={formData.websiteUrl}
            onChange={(e) => updateField("websiteUrl", e.target.value)}
            errorMessage={errors.websiteUrl}
          />
          <Input
            label={"name_ar"}
            name="logoUrl"
            placeholder={"name_ar"}
            value={formData.logoUrl}
            onChange={(e) => updateField("logoUrl", e.target.value)}
            errorMessage={errors.logoUrl}
          />
          <div className="btns-container">
            <Button className="cancel-btn" onClick={onClose}>
              {"cancel_text"}
            </Button>
            <Button className="btn btn-primary" onClick={handleSubmit}>
              {"save"}
            </Button>
          </div>
        </form>
      </Modal>
    </div>
  );
};
export default AddEditBrandModal;
