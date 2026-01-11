import { useState } from "react";
import { create } from "../../api/apiWrapper.js";
import ProductInfo from "../../components/AddProductPage/ProductInfo";
import Variations from "../../components/AddProductPage/Variations.jsx";
import ProductItems from "../../components/AddProductPage/ProductItems.jsx";
import "./AddProductPage.css";
import { Link } from "react-router-dom";
import { ArrowLeft, Save } from "lucide-react";
import Button from "../../components/ui/Button";
import Header from "../../components/AddProductPage/Header.jsx";
import Tabs from "../../components/AddProductPage/Tabs.jsx";
import StepNavigation from "../../components/AddProductPage/StepNavigation.jsx";
import { useLanguage } from "../../hooks/useLanguage.jsx";

const intialFormData = {
  BrandId: "",
  NameEn: "",
  NameAr: "",
  DescriptionEn: "",
  DescriptionAr: "",
  WeightKg: 0,
  DefaultImageFile: null,
  Variations: [],
  ProductItems: [],
};

const AddProductPage = () => {
  const [step, setStep] = useState(1);
  const [productData, setProductData] = useState(intialFormData);

  const handleNext = () => setStep(Math.min(step + 1, steps.length));
  const handlePrevious = () => setStep(Math.max(step - 1, 1));

  const { translations } = useLanguage();
  const { general, variations, items } = translations.general.pages.add_product;

  const steps = [
    { id: 1, name: general },
    { id: 2, name: variations },
    { id: 3, name: items },
  ];

  const handleSubmit = async () => {
    try {
      const formData = new FormData();
      formData.append("BrandId", productData.BrandId);
      formData.append("NameEn", productData.NameEn);
      formData.append("NameAr", productData.NameAr);
      formData.append("DescriptionEn", productData.DescriptionEn || "");
      formData.append("DescriptionAr", productData.DescriptionAr || "");
      formData.append("WeightKg", productData.WeightKg);

      if (productData.DefaultImageFile) {
        formData.append("DefaultImageFile", productData.DefaultImageFile);
      }

      formData.append("Variations", JSON.stringify(productData.Variations));

      const productItems = productData.ProductItems.map((item) => {
        return {
          ...item,
          ProductItemImages: item.ProductItemImages.map((img) => ({
            IsPrimary: img.IsPrimary,
          })),
        };
      });
      formData.append("ProductItems", JSON.stringify(productItems));

      productData.ProductItems.forEach((item, iIndex) => {
        item.ProductItemImages.forEach((img, imgIndex) => {
          if (img.ImageFile) {
            formData.append(
              `ProductItems[${iIndex}][ProductItemImages][${imgIndex}][ImageFile]`,
              img.ImageFile
            );
          }
        });
      });

      const data = await create(`products`, formData);
      console.log("data -> ", data);
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="add-product-container">
      <Header />
      <Tabs steps={steps} selectedStep={step} setStep={setStep} />
      <h2>step {step}</h2>
      {step == 1 && (
        <ProductInfo
          productData={productData}
          setProductData={setProductData}
        />
      )}
      {step == 2 && (
        <Variations productData={productData} setProductData={setProductData} />
      )}
      {step == 3 && (
        <ProductItems
          productData={productData}
          setProductData={setProductData}
        />
      )}
      <StepNavigation
        step={step}
        stepsLength={steps.length}
        handlePrevious={handlePrevious}
        handleNext={handleNext}
        handleSubmit={handleSubmit}
      />
    </div>
  );
};
export default AddProductPage;
