import { Box, Star, Upload, WandSparkles, X } from "lucide-react";
import "./ProductItems.css";
import Input from "../ui/Input";
import Button from "../ui/Button";
import { isImageValid } from "../../utils/utils";
import { useLanguage } from "../../hooks/useLanguage";

const computeCombinations = (variations) => {
  if (!variations?.length) return [];

  const optionGroups = variations.map((v) =>
    v.options.map((o) => ({ valueEn: o.ValueEn, valueAr: o.ValueAr }))
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
    const combos = computeCombinations(productData.Variations);
    const items = combos.map((options) => ({
      sku: options
        .map((o) => o.valueEn)
        .join("-")
        .toUpperCase(),
      options,
      price: "",
      stock: "",
      images: [],
    }));

    setProductData({ ...productData, ProductItems: items });
  };

  const updateItem = (index, field, value) => {
    const items = [...productData.ProductItems];
    items[index][field] = value;
    setProductData({ ...productData, ProductItems: items });
  };

  const addImages = (index, files) => {
    const validFiles = Array.from(files)
      .filter((file) => isImageValid(file))
      .map((file) => ({ file, isPrimary: false }));

    const items = [...productData.ProductItems];
    if (!items[index].images) {
      items[index].images = [];
    }

    if (items[index].images.length === 0 && validFiles.length > 0) {
      validFiles[0].isPrimary = true;
    }

    items[index].images.push(...validFiles);
    setProductData({ ...productData, ProductItems: items });
  };

  const removeImage = (itemIndex, imageIndex) => {
    const items = [...productData.ProductItems];

    const removedImage = items[itemIndex].images.splice(imageIndex, 1)[0];

    if (removedImage?.isPrimary && items[itemIndex].images.length > 0) {
      items[itemIndex].images[0].isPrimary = true;
    }

    setProductData({ ...productData, ProductItems: items });
  };

  const setPrimaryImage = (itemIndex, imgIndex) => {
    const items = [...productData.ProductItems];
    items[itemIndex].images = items[itemIndex].images.map((img, i) => ({
      ...img,
      isPrimary: i === imgIndex,
    }));
    setProductData({ ...productData, ProductItems: items });
  };

  const getImagePrview = (file) => {
    return URL.createObjectURL(file);
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
      {productData?.ProductItems?.map((item, index) => (
        <div key={index} className="product-item-row">
          {/* Options only */}
          <div className="product-variants">
            <small>{variant_label}</small>
            {item.options.map((opt, i) => (
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
            errorMessage={errors?.ProductItems?.[index]?.sku}
          />

          {/* Price */}
          <Input
            showLabel={true}
            label={price_label}
            placeholder={price_placeholder}
            type="number"
            value={item.price}
            onChange={(e) => updateItem(index, "price", e.target.value)}
            errorMessage={errors?.ProductItems?.[index]?.price}
          />

          {/* Stock */}
          <Input
            showLabel={true}
            label={stock_label}
            placeholder={stock_placeholder}
            type="number"
            value={item.stock}
            onChange={(e) => updateItem(index, "stock", e.target.value)}
            errorMessage={errors?.ProductItems?.[index]?.stock}
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
            {item?.images?.map((img, imgIndex) => (
              <div
                key={imgIndex}
                className={`product-preview-container ${
                  img?.isPrimary ? "primary-image" : ""
                } `}
              >
                <img
                  src={getImagePrview(img?.file)}
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

          {errors?.ProductItems?.[index]?.images && (
            <div className="form-alert">
              {errors.ProductItems[index].images}
            </div>
          )}
        </div>
      ))}
    </div>
  );
};
export default ProductItems;
