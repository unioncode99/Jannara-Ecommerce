import { Box, Star, Trash2, Upload, WandSparkles, X } from "lucide-react";
import "./ProductItems.css";
import Input from "../ui/Input";
import Button from "../ui/Button";
import { isImageValid } from "../../utils/utils";
import { useLanguage } from "../../hooks/useLanguage";

const computeCombinations = (variations) => {
  if (!variations?.length) return [];

  const optionGroups = variations.map((v) =>
    v?.variationOptions?.map((o) => ({
      valueEn: o.valueEn,
      valueAr: o.valueAr,
    }))
  );

  return optionGroups.reduce((acc, group) => {
    if (!acc.length) return group.map((g) => [g]);
    return acc.flatMap((a) => group.map((g) => [...a, g]));
  }, []);
};

const ProductItems = ({ productData, setProductData, errors }) => {
  const { language, translations } = useLanguage();

  const {
    product_items,
    generate_items,
    variant_label,
    sku_label,
    sku_placeholder,
    price_label,
    price_placeholder,
    stock_label,
    stock_placeholder,
    upload_images,
  } = translations.general.pages.add_product;

  const generateItems = () => {
    const combos = computeCombinations(productData?.variations);
    const items = combos.map((variationOptions) => ({
      sku: variationOptions
        .map((o) => o.valueEn)
        .join("-")
        .toUpperCase(),
      variationOptions,
      price: "",
      stock: "",
      productItemImages: [],
    }));

    setProductData({ ...productData, productItems: items });
  };

  const updateItem = (index, field, value) => {
    const items = [...productData.productItems];
    items[index][field] = value;
    setProductData({ ...productData, productItems: items });
  };

  const addImages = (index, files) => {
    const validFiles = Array.from(files)
      .filter((file) => isImageValid(file))
      .map((file) => ({ imageFile: file, isPrimary: false }));

    const items = [...productData.productItems];
    if (!items[index].productItemImages) {
      items[index].productItemImages = [];
    }

    if (items[index].productItemImages.length === 0 && validFiles.length > 0) {
      validFiles[0].isPrimary = true;
    }

    items[index].productItemImages.push(...validFiles);
    setProductData({ ...productData, productItems: items });
  };

  const removeImage = (itemIndex, imageIndex) => {
    const items = [...productData.productItems];

    const removedImage = items[itemIndex].productItemImages.splice(
      imageIndex,
      1
    )[0];

    if (
      removedImage?.isPrimary &&
      items[itemIndex].productItemImages.length > 0
    ) {
      items[itemIndex].productItemImages[0].isPrimary = true;
    }

    setProductData({ ...productData, productItems: items });
  };

  const setPrimaryImage = (itemIndex, imgIndex) => {
    const items = [...productData.productItems];
    items[itemIndex].productItemImages = items[itemIndex].productItemImages.map(
      (img, i) => ({
        ...img,
        isPrimary: i === imgIndex,
      })
    );
    setProductData({ ...productData, productItems: items });
  };

  const getImagePrview = (file) => {
    return URL.createObjectURL(file);
  };

  const removeItem = (itemIndex) => {
    const items = [...productData.productItems];
    items.splice(itemIndex, 1); // remove the whole item
    setProductData({ ...productData, productItems: items });
  };

  return (
    <div className="product-items-container">
      <header>
        <h3 className="step-title">
          <Box /> {product_items}
        </h3>
        <Button className="btn btn-primary" onClick={generateItems}>
          <WandSparkles /> {generate_items}
        </Button>
      </header>
      {productData?.productItems?.map((item, index) => (
        <div key={index} className="product-item-row">
          <div className="product-item-row-header">
            <Button
              className="remove-item-btn"
              onClick={() => removeItem(index)}
            >
              <Trash2 />
            </Button>
          </div>

          {/* Options only */}
          <div className="product-variants">
            <small>{variant_label}</small>
            {item.variationOptions.map((opt, i) => (
              <small className="variant" key={i}>
                {language == "en" ? opt.valueEn : opt.valueAr}
              </small>
            ))}
          </div>
          {/* SKU */}
          <Input
            showLabel={true}
            label={sku_label}
            placeholder={sku_placeholder}
            value={item.sku}
            onChange={(e) => updateItem(index, "sku", e.target.value)}
            errorMessage={errors?.productItems?.[index]?.sku}
          />

          {/* Price */}
          <Input
            showLabel={true}
            label={price_label}
            placeholder={price_placeholder}
            type="number"
            value={item.price}
            onChange={(e) => updateItem(index, "price", e.target.value)}
            errorMessage={errors?.productItems?.[index]?.price}
          />

          {/* Stock */}
          <Input
            showLabel={true}
            label={stock_label}
            placeholder={stock_placeholder}
            type="number"
            value={item.stock}
            onChange={(e) => updateItem(index, "stock", e.target.value)}
            errorMessage={errors?.productItems?.[index]?.stock}
          />

          {/* Images */}

          <label className="upload-product-image-container">
            <Upload className="upload-icon" />
            <span>{upload_images}</span>
            <input
              type="file"
              accept="image/*"
              multiple
              style={{ display: "none" }}
              onChange={(e) => addImages(index, e.target.files)}
            />
          </label>

          <div className="sku-images">
            {item?.productItemImages?.map((img, imgIndex) => (
              <div
                key={imgIndex}
                className={`product-preview-container ${
                  img?.isPrimary ? "primary-image" : ""
                } `}
              >
                <img
                  src={getImagePrview(img?.imageFile)}
                  alt="Product Image"
                  className="product-img"
                  onClick={() => setPrimaryImage(index, imgIndex)}
                />
                {img.isPrimary && (
                  <span className="primary-badge">
                    <Star />
                  </span>
                )}
                <button onClick={() => removeImage(index, imgIndex)}>
                  <X />
                </button>
              </div>
            ))}
          </div>

          {errors?.productItems?.[index]?.productItemImages && (
            <div className="form-alert">
              {errors.productItems[index].productItemImages}
            </div>
          )}
        </div>
      ))}
    </div>
  );
};
export default ProductItems;
