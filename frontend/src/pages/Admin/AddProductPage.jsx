import { useEffect, useState } from "react";
import { create } from "../../api/apiWrapper.js";
import ProductInfo from "../../components/AddProductPage/ProductInfo";
import Variations from "../../components/AddProductPage/Variations/Variations.jsx";
import ProductItems from "../../components/AddProductPage/ProductItems.jsx";
import "./AddProductPage.css";
import { Link, useNavigate } from "react-router-dom";
import { ArrowLeft, Save } from "lucide-react";
import Button from "../../components/ui/Button";
import Header from "../../components/AddProductPage/Header.jsx";
import Tabs from "../../components/AddProductPage/Tabs.jsx";
import StepNavigation from "../../components/AddProductPage/StepNavigation.jsx";
import { useLanguage } from "../../hooks/useLanguage.jsx";
import { isImageValid } from "../../utils/utils.jsx";
import { toast } from "../../components/ui/Toast";

const intialFormData = {
  categoryId: "",
  brandId: "",
  nameEn: "",
  nameAr: "",
  descriptionEn: "",
  descriptionAr: "",
  weightKg: 0,
  defaultImageFile: null,
  variations: [],
  productItems: [],
};

// const intialFormData = {
//   categoryId: "",
//   brandId: "",
//   nameEn: "",
//   nameAr: "",
//   descriptionEn: "",
//   descriptionAr: "",
//   weightKg: 0,
//   defaultImageFile: null,
//   variations: [
//     // for test
//     {
//       nameEn: "Color",
//       nameAr: "اللون",
//       variationOptions: [
//         { valueEn: "Red", valueAr: "أحمر" },
//         { valueEn: "Blue", valueAr: "أزرق" },
//       ],
//     },
//     {
//       nameEn: "Size",
//       nameAr: "الحجم",
//       variationOptions: [
//         { valueEn: "S", valueAr: "ص" },
//         { valueEn: "M", valueAr: "م" },
//       ],
//     },
//   ],
//   productItems: [
//     {
//       sku: "ddd",
//       variationOptions: [
//         { valueEn: "Red", valueAr: "أحمر" },
//         { valueEn: "Blue", valueAr: "أزرق" },
//       ],
//       productItemImages: [
//         {
//           imageFile: null,
//           isPrimary: false,
//         },
//       ],
//     },
//   ],
// };

const AddProductPage = () => {
  const [step, setStep] = useState(1);
  const [productData, setProductData] = useState(intialFormData);
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const { translations, language } = useLanguage();
  const {
    general,
    variations,
    items,
    general_nameEn_error,
    general_nameAr_error,
    // general_descriptionEn_error,
    // general_descriptionAr_error,
    general_weightKg_error,
    // general_brand_error,
    general_defaultImage_error,
    at_least_one_option,
    product_item_sku_error,
    product_item_price_error,
    product_item_stock_error,
    product_item_images_error,
    general_category_error,
    product_add_success,
    product_add_failed,
  } = translations.general.pages.add_product;

  const steps = [
    { id: 1, name: general },
    { id: 2, name: variations },
    { id: 3, name: items },
  ];

  function validateGeneralStep() {
    let newErrors = {};

    // if (!productData.brandId || productData.brandId <= 0) {
    //   newErrors.brandId = general_brand_error;
    // }

    if (!productData.categoryId) {
      newErrors.categoryId = general_category_error;
    }

    if (!productData.nameEn.trim()) {
      newErrors.nameEn = general_nameEn_error;
    }
    if (!productData.nameAr.trim()) {
      newErrors.nameAr = general_nameAr_error;
    }
    // if (!productData.descriptionEn.trim()) {
    //   newErrors.descriptionEn = general_descriptionEn_error;
    // }
    // if (!productData.descriptionAr.trim()) {
    //   newErrors.descriptionAr = general_descriptionAr_error;
    // }

    if (!productData.weightKg || productData.weightKg < 0) {
      newErrors.weightKg = general_weightKg_error;
    }

    if (!isImageValid(productData.defaultImageFile)) {
      newErrors.defaultImageFile = general_defaultImage_error;
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0; // true = valid
  }

  function validateVariationStep() {
    const variations = productData?.variations;

    if (!variations || variations.length === 0) {
      return false;
    }

    for (let i = 0; i < variations.length; i++) {
      if (
        !variations[i].variationOptions ||
        variations[i].variationOptions.length === 0
      ) {
        return false;
      }
    }

    return true;
  }

  function validateProductItemsStep() {
    const itemsErrors = {};
    let hasError = false;

    if (!productData.productItems || productData.productItems.length === 0) {
      toast.show(at_least_one_option, "error");
      return false;
    }

    productData.productItems.forEach((item, index) => {
      itemsErrors[index] = {};

      if (!item.sku.trim()) {
        itemsErrors[index].sku = product_item_sku_error;
        hasError = true;
      }
      if (!item.price || Number(item.price) <= 0) {
        itemsErrors[index].price = product_item_price_error;
        hasError = true;
      }
      if (!item.stock || Number(item.stock) < 0) {
        itemsErrors[index].stock = product_item_stock_error;
        hasError = true;
      }
      if (!item.productItemImages || item.productItemImages.length == 0) {
        itemsErrors[index].productItemImages = product_item_images_error;
        hasError = true;
      }
    });

    setErrors((prev) => ({ ...prev, productItems: itemsErrors }));
    return !hasError;
  }

  const handleNext = () => {
    if (step == 1 && !validateGeneralStep()) {
      return;
    }

    if (step == 2 && !validateVariationStep()) {
      toast.show(at_least_one_option, "error");
      return;
    }

    setStep(Math.min(step + 1, steps.length));
  };
  const handlePrevious = () => setStep(Math.max(step - 1, 1));

  useEffect(() => {
    if (Object.keys(errors).length > 0) {
      if (step == 1) {
        validateGeneralStep();
      }

      if (step == 3) {
        validateProductItemsStep();
      }
    }
  }, [language]);

  const handleSubmit = async () => {
    if (!validateProductItemsStep()) {
      return;
    }

    if (!validateVariationStep()) {
      return;
    }

    if (!validateGeneralStep()) {
      return;
    }

    await uploadProduct();
  };

  // async function uploadProduct() {
  //   try {
  //     const formData = new FormData();
  //     formData.append("brandId", productData.brandId);
  //     formData.append("nameEn", productData.nameEn);
  //     formData.append("nameAr", productData.nameAr);
  //     formData.append("descriptionEn", productData.descriptionEn || "");
  //     formData.append("descriptionAr", productData.descriptionAr || "");
  //     formData.append("weightKg", productData.weightKg);

  //     if (productData.defaultImageFile) {
  //       formData.append("defaultImageFile", productData.defaultImageFile);
  //     }

  //     formData.append("variations", JSON.stringify(productData.variations));

  //     const productItemsMeta = productData?.productItems?.map((item) => ({
  //       sku: item.sku,
  //       variationOptions: item?.variationOptions,
  //       productItemImages: item?.productItemImages?.map((img) => ({
  //         isPrimary: img.isPrimary,
  //       })),
  //     }));

  //     formData.append("productItems", JSON.stringify(productItemsMeta));

  //     productData?.productItems?.forEach((item, iIndex) => {
  //       item.productItemImages.forEach((img, imgIndex) => {
  //         if (img.imageFile) {
  //           formData.append(
  //             `productItems[${iIndex}][productItemImages][${imgIndex}][imageFile]`,
  //             img.imageFile
  //           );

  //           formData.append(
  //             `productItems[${iIndex}][productItemImages][${imgIndex}][isPrimary]`,
  //             img.isPrimary ? "true" : "false"
  //           );
  //         }
  //       });
  //     });

  //     const data = await create(`products`, formData);
  //     console.log("data -> ", data);
  //   } catch (err) {
  //     console.error(err);
  //   }
  // }

  async function uploadProduct() {
    try {
      setLoading(true);
      const formData = new FormData();
      formData.append("categoryId", productData.categoryId);
      formData.append("brandId", productData.brandId);
      formData.append("nameEn", productData.nameEn);
      formData.append("nameAr", productData.nameAr);
      formData.append("descriptionEn", productData.descriptionEn || "");
      formData.append("descriptionAr", productData.descriptionAr || "");
      formData.append("weightKg", productData.weightKg);

      if (productData.defaultImageFile) {
        formData.append("defaultImageFile", productData.defaultImageFile);
      }
      // variations
      productData.variations.forEach((v, vi) => {
        formData.append(`variations[${vi}].nameEn`, v.nameEn);
        formData.append(`variations[${vi}].nameAr`, v.nameAr);

        v.variationOptions.forEach((opt, oi) => {
          formData.append(
            `variations[${vi}].variationOptions[${oi}].valueEn`,
            opt.valueEn
          );
          formData.append(
            `variations[${vi}].variationOptions[${oi}].valueAr`,
            opt.valueAr
          );
        });
      });

      // Product Items
      productData.productItems.forEach((pi, piIndex) => {
        formData.append(`productItems[${piIndex}].sku`, pi.sku);

        // Variation options of product item
        pi.variationOptions.forEach((opt, oi) => {
          formData.append(
            `productItems[${piIndex}].variationOptions[${oi}].valueEn`,
            opt.valueEn
          );
          formData.append(
            `productItems[${piIndex}].variationOptions[${oi}].valueAr`,
            opt.valueAr
          );
        });

        // Product item images
        pi.productItemImages.forEach((img, ii) => {
          if (img.imageFile) {
            formData.append(
              `productItems[${piIndex}].productItemImages[${ii}].imageFile`,
              img.imageFile
            );
          }
          formData.append(
            `productItems[${piIndex}].productItemImages[${ii}].isPrimary`,
            img.isPrimary ? "true" : "false"
          );
        });
      });

      const result = await create(`products`, formData);
      console.log("result -> ", result);
      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show(product_add_success, "success");
      }
      navigate("/products");
    } catch (err) {
      if (translations.general.server_messages[err.message]) {
        toast.show(translations.general.server_messages[err.message], "error");
      } else {
        toast.show(product_add_failed, "error");
      }
      console.error(err);
      setProductData(intialFormData);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    console.log("productData -> ", productData);
  }, [productData]);

  return (
    <div className="add-product-container">
      <Header />
      <Tabs
        loading={loading}
        steps={steps}
        selectedStep={step}
        setStep={setStep}
      />
      {step == 1 && (
        <ProductInfo
          productData={productData}
          setProductData={setProductData}
          errors={errors}
        />
      )}
      {step == 2 && (
        <Variations productData={productData} setProductData={setProductData} />
      )}
      {step == 3 && (
        <ProductItems
          productData={productData}
          setProductData={setProductData}
          errors={errors}
        />
      )}
      <StepNavigation
        step={step}
        stepsLength={steps.length}
        loading={loading}
        handlePrevious={handlePrevious}
        handleNext={handleNext}
        handleSubmit={handleSubmit}
      />
    </div>
  );
};
export default AddProductPage;
