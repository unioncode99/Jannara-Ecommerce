import { Box, Upload, WandSparkles, X } from "lucide-react";
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
  const { language } = useLanguage();
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
    const validFiles = Array.from(files).filter((file) => isImageValid(file));
    const items = [...productData.ProductItems];
    if (!items[index].images) {
      items[index].images = [];
    }
    items[index].images.push(...validFiles);
    setProductData({ ...productData, ProductItems: items });
  };

  const removeImage = (itemIndex, imageIndex) => {
    const items = [...productData.ProductItems];
    items[itemIndex].images.splice(imageIndex, 1);
    setProductData({ ...productData, ProductItems: items });
  };

  const getImagePrview = (file) => {
    return URL.createObjectURL(file);
  };

  return (
    <div className="product-items-container">
      <header>
        <h3 className="step-title">
          <Box /> {"Product Items"}
        </h3>
        <Button className="btn btn-primary" onClick={generateItems}>
          <WandSparkles /> Generate Items
        </Button>
      </header>
      {productData?.ProductItems?.map((item, index) => (
        <div key={index} className="product-item-row">
          {/* Options only */}
          <div className="product-variants">
            <small>Variant</small>
            {item.options.map((opt, i) => (
              <small className="variant" key={i}>
                {language == "en" ? opt.valueEn : opt.valueAr}
              </small>
            ))}
          </div>
          {/* SKU */}
          <Input
            showLabel={true}
            label="SKU"
            placeholder="SKU"
            value={item.sku}
            onChange={(e) => updateItem(index, "sku", e.target.value)}
            errorMessage={errors?.ProductItems?.[index]?.sku}
          />

          {/* Price */}
          <Input
            showLabel={true}
            label="Price"
            placeholder="Price"
            type="number"
            value={item.price}
            onChange={(e) => updateItem(index, "price", e.target.value)}
            errorMessage={errors?.ProductItems?.[index]?.price}
          />

          {/* Stock */}
          <Input
            showLabel={true}
            label="Stock"
            placeholder="Stock"
            type="number"
            value={item.stock}
            onChange={(e) => updateItem(index, "stock", e.target.value)}
            errorMessage={errors?.ProductItems?.[index]?.stock}
          />

          {/* Images */}

          <label className="upload-product-image-container">
            <Upload className="upload-icon" />
            <span>Click to upload images</span>
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
              <div key={imgIndex} className="product-preview-container">
                <img
                  src={getImagePrview(img)}
                  alt="Product Image"
                  className="product-img"
                />
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
