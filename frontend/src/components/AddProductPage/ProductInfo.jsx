import { useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import Input from "../ui/Input";
import { Camera, Upload, X } from "lucide-react";
import "./ProductInfo.css";
import BrandsDropdown from "../BrandsDropdown";

const ProductInfo = ({ productData, setProductData }) => {
  const [productImagePreview, setProductImagePreview] = useState(null);
  const { translations, language } = useLanguage();
  const {
    general_brandId_label,
    general_brandId_placeholder,
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
  } = translations.general.pages.add_product;

  const handleChange = (e) => {
    const { name, value } = e.target;
    setProductData({ ...productData, [name]: value });

    console.log("name -> ", name);
    console.log("value -> ", value);
  };

  const handleFileChange = (e) => {
    setProductData({ ...productData, DefaultImageFile: e.target.files[0] });
  };

  const handleProductImageChange = (e) => {
    const file = e.target.files[0];
    if (!file) {
      return;
    }

    handleFileChange(e);

    const imageUrl = URL.createObjectURL(file);
    setProductImagePreview(imageUrl);
  };

  function cancelUpload() {
    setProductData({ ...productData, DefaultImageFile: null });
    setProductImagePreview(null);
  }

  const errors = {};

  return (
    <div className="product-info-container">
      <Input
        label={general_brandId_label}
        name="BrandId"
        placeholder={general_brandId_placeholder}
        value={productData.BrandId}
        onChange={handleChange}
        errorMessage={errors?.BrandId}
        showLabel={true}
      />

      <BrandsDropdown value={productData.BrandId} onChange={handleChange} />

      <Input
        label={general_nameEn_label}
        name="NameEn"
        placeholder={general_nameEn_placeholder}
        value={productData.NameEn}
        onChange={handleChange}
        errorMessage={errors?.NameEn}
        showLabel={true}
      />
      <Input
        label={general_nameAr_label}
        name="NameAr"
        placeholder={general_nameAr_placeholder}
        value={productData.NameAr}
        onChange={handleChange}
        errorMessage={errors?.NameAr}
        showLabel={true}
      />
      <Input
        label={general_descriptionEn_label}
        name="DescriptionEn"
        placeholder={general_descriptionEn_placeholder}
        value={productData.DescriptionEn}
        onChange={handleChange}
        errorMessage={errors?.DescriptionEn}
        showLabel={true}
      />
      <Input
        label={general_descriptionAr_label}
        name="DescriptionAr"
        placeholder={general_descriptionAr_placeholder}
        value={productData.DescriptionAr}
        onChange={handleChange}
        errorMessage={errors?.DescriptionAr}
        showLabel={true}
      />
      <Input
        label={general_weightKg_label}
        name="WeightKg"
        type="number"
        placeholder={general_weightKg_placeholder}
        value={productData.WeightKg}
        onChange={handleChange}
        errorMessage={errors?.WeightKg}
        showLabel={true}
      />
      <div className="form-row product-image-container">
        <label className="upload-product-image-container">
          <Upload className="upload-icon" />
          <span>Upload</span>
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
    </div>
  );
};
export default ProductInfo;
