import { useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import Input from "../ui/Input";
import TextArea from "../ui/TextArea";
import { Camera, Globe, Upload, X } from "lucide-react";
import "./ProductInfo.css";
import BrandsDropdown from "../BrandsDropdown";
import { isImageValid } from "../../utils/utils";

const ProductInfo = ({ productData, setProductData, errors }) => {
  const [productImagePreview, setProductImagePreview] = useState(null);
  const { translations } = useLanguage();
  const {
    general_brand_label,
    general_nameEn_label,
    general_nameEn_placeholder,
    general_nameAr_label,
    general_nameAr_placeholder,
    general_descriptionEn_label,
    general_descriptionEn_placeholder,
    general_descriptionAr_label,
    general_descriptionAr_placeholder,
    general_weightKg_label,
    general_weightKg_placeholder,
    general_info,
    upload,
  } = translations.general.pages.add_product;

  const handleChange = (e) => {
    const { name, value } = e.target;
    setProductData({ ...productData, [name]: value });

    console.log("name -> ", name);
    console.log("value -> ", value);
  };

  const handleFileChange = (e) => {
    setProductData({ ...productData, defaultImageFile: e.target.files[0] });
  };

  const handleProductImageChange = (e) => {
    const file = e.target.files[0];

    if (!isImageValid(file)) {
      return;
    }

    handleFileChange(e);

    const imageUrl = URL.createObjectURL(file);
    setProductImagePreview(imageUrl);
  };

  function cancelUpload() {
    setProductData({ ...productData, defaultImageFile: null });
    setProductImagePreview(null);
  }

  return (
    <div className="product-info-container">
      <h3>
        {" "}
        <h3 className="step-title">
          <Globe /> {general_info}
        </h3>
      </h3>
      <BrandsDropdown
        name="brandId"
        onChange={handleChange}
        label={general_brand_label}
        showLabel={true}
        value={productData.brandId}
        errorMessage={errors?.brandId}
      />

      <Input
        label={general_nameEn_label}
        name="nameEn"
        placeholder={general_nameEn_placeholder}
        value={productData.nameEn}
        onChange={handleChange}
        errorMessage={errors?.nameEn}
        showLabel={true}
      />
      <Input
        label={general_nameAr_label}
        name="nameAr"
        placeholder={general_nameAr_placeholder}
        value={productData.nameAr}
        onChange={handleChange}
        errorMessage={errors?.nameAr}
        showLabel={true}
      />

      <TextArea
        label={general_descriptionEn_label}
        name="descriptionEn"
        placeholder={general_descriptionEn_placeholder}
        value={productData.descriptionEn}
        onChange={handleChange}
        errorMessage={errors?.descriptionEn}
        showLabel={true}
      />
      <TextArea
        label={general_descriptionAr_label}
        name="descriptionAr"
        placeholder={general_descriptionAr_placeholder}
        value={productData.descriptionAr}
        onChange={handleChange}
        errorMessage={errors?.descriptionAr}
        showLabel={true}
      />
      <Input
        label={general_weightKg_label}
        name="weightKg"
        type="number"
        placeholder={general_weightKg_placeholder}
        value={productData.weightKg}
        onChange={handleChange}
        errorMessage={errors?.weightKg}
        showLabel={true}
      />
      <div className="form-row product-image-container">
        <label className="upload-product-image-container">
          <Upload className="upload-icon" />
          <span>{upload}</span>
          <input
            type="file"
            accept="image/*"
            style={{ display: "none" }}
            onChange={handleProductImageChange}
          />
        </label>
        {productImagePreview && (
          <div className="product-preview-container">
            <img
              src={productImagePreview}
              alt="Product Image"
              className="product-img"
            />
            <button onClick={cancelUpload}>
              <X />
            </button>
          </div>
        )}
      </div>
      {errors.defaultImageFile && (
        <div className="form-alert">{errors.defaultImageFile}</div>
      )}
    </div>
  );
};
export default ProductInfo;
