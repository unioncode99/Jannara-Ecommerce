import { useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import Input from "../ui/Input";
import TextArea from "../ui/TextArea";
import { Camera, Globe, Loader2, Upload, X } from "lucide-react";
import "./ProductInfo.css";
import BrandsDropdown from "../BrandsDropdown";
import ProductCategoriesDropdown from "../ProductCategoriesDropdown";
import { isImageValid } from "../../utils/utils";
import Button from "../ui/Button";

const ProductInfo = ({
  productData,
  setProductData,
  errors,
  updateProduct,
  isModeUpdate,
  updateProductLoading,
}) => {
  const [productImagePreview, setProductImagePreview] = useState(
    productData?.defaultImageUrl || null
  );
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
    general_category_label,
  } = translations.general.pages.add_product;

  const { save } = translations.general.pages.products;

  const handleChange = (e) => {
    const { name, value } = e.target;
    setProductData({ ...productData, [name]: value });

    console.log("name -> ", name);
    console.log("value -> ", value);
  };

  const handleProductImageChange = (e) => {
    const file = e.target.files[0];

    if (!isImageValid(file)) {
      return;
    }

    setProductData((prev) => ({
      ...prev,
      defaultImageFile: file,
      defaultImageUrl: null,
    }));

    setProductImagePreview(URL.createObjectURL(file));
  };

  function cancelUpload() {
    // setProductData((prev) => ({
    //   ...prev,
    //   defaultImageFile: null,
    // }));
    setProductData((prev) => ({
      ...prev,
      defaultImageFile: null,
      defaultImageUrl: null,
    }));
  }

  return (
    <div className="product-info-container">
      <h3 className="step-title">
        <Globe /> {general_info}
      </h3>
      <ProductCategoriesDropdown
        name="categoryId"
        value={productData.categoryId}
        onChange={handleChange}
        label={general_category_label}
        showLabel={true}
        errorMessage={errors?.categoryId}
      />
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
        min={0}
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
        {(productImagePreview || productData.defaultImageUrl) && (
          <div className="product-preview-container">
            <img
              src={productImagePreview || productData.defaultImageUrl}
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
      {/* updateProductLoading */}
      {isModeUpdate && (
        <div className="save-container">
          <Button
            onClick={updateProduct}
            disabled={updateProductLoading}
            className="btn btn-primary "
          >
            {updateProductLoading ? <Loader2 className="animate-spin" /> : save}
          </Button>
        </div>
      )}
    </div>
  );
};
export default ProductInfo;
