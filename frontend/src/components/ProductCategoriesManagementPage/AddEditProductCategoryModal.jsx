import { useEffect, useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import Button from "../ui/Button";
import Input from "../ui/Input";
import Modal from "../ui/Modal";
import { toast } from "../ui/Toast";
import TextArea from "../ui/TextArea";
import Select from "../ui/Select";
import "./AddEditProductCategoryModal.css";

const initialFormState = {
  nameEn: "",
  nameAr: "",
  descriptionEn: "",
  descriptionAr: "",
  parentCategoryId: "",
};

const AddEditProductCategoryModal = ({
  show,
  onClose,
  onConfirm,
  productCategory,
  productCategories = [],
}) => {
  const [formData, setFormData] = useState(initialFormState);
  const { translations, language } = useLanguage();
  const [errors, setErrors] = useState({});

  const { add_category, edit_category, save, cancel_text } =
    translations.general.pages.product_categories_management;

  const updateField = (name, value) => {
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  useEffect(() => {
    setFormData({
      nameEn: productCategory?.nameEn || "",
      nameAr: productCategory?.nameAr || "",
      descriptionEn: productCategory?.descriptionEn || "",
      descriptionAr: productCategory?.descriptionAr || "",
      parentCategoryId: productCategory?.parentCategoryId || "",
    });
  }, [productCategory]);

  const options = productCategories?.map((cat) => ({
    value: cat.id,
    label: language === "en" ? cat.nameEn : cat.nameAr,
  }));

  const validateFormData = () => {
    let temp = {};
    const msgs = translations.general.form.errors;

    if (!formData.nameEn.trim()) {
      temp.nameEn = msgs.required;
    }

    if (!formData.nameAr.trim()) {
      temp.nameAr = msgs.required;
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
    if (formData.parentCategoryId) {
      payload.parentCategoryId = formData.parentCategoryId;
    }

    onConfirm?.(payload);
  };

  return (
    <div className="add-edit-product-category-modal">
      <Modal
        show={show}
        onClose={onClose}
        title={
          <>
            <span>{productCategory ? edit_category : add_category}</span>
          </>
        }
        className="confirm-modal"
      >
        <form className="add-edit-product-category-form">
          <Select
            options={options}
            label={language == "en" ? "Category" : "التصنيف"}
            name="parentCategoryId"
            value={formData.parentCategoryId}
            onChange={(e) => updateField("parentCategoryId", e.target.value)}
            errorMessage={errors.parentCategoryId}
            showLabel={true}
          />
          <Input
            label={translations.general.form.name_en}
            name="nameEn"
            placeholder={translations.general.form.name_en}
            value={formData.nameEn}
            onChange={(e) => updateField("nameEn", e.target.value)}
            errorMessage={errors.nameEn}
          />
          <Input
            label={translations.general.form.name_ar}
            name="nameAr"
            placeholder={translations.general.form.name_ar}
            value={formData.nameAr}
            onChange={(e) => updateField("nameAr", e.target.value)}
            errorMessage={errors.nameAr}
          />
          <TextArea
            label={translations.general.form.description_en}
            name="descriptionEn"
            placeholder={translations.general.form.description_en}
            value={formData.descriptionEn}
            onChange={(e) => updateField("descriptionEn", e.target.value)}
            errorMessage={errors.descriptionEn}
          />
          <TextArea
            label={translations.general.form.description_ar}
            name="descriptionAr"
            placeholder={translations.general.form.description_ar}
            value={formData.descriptionAr}
            onChange={(e) => updateField("descriptionAr", e.target.value)}
            errorMessage={errors.descriptionAr}
          />
          <div className="btns-container">
            <Button className="cancel-btn" onClick={onClose}>
              {cancel_text}
            </Button>
            <Button className="btn btn-primary" onClick={handleSubmit}>
              {save}
            </Button>
          </div>
        </form>
      </Modal>
    </div>
  );
};
export default AddEditProductCategoryModal;
