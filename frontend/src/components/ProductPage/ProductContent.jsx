import Button from "../ui/Button";
import ProductFeatures from "./ProductFeatures";
import ProductHeader from "./ProductHeader";
import ProductPrice from "./ProductPrice";
import ProductRating from "./ProductRating";
import ProductSellers from "./ProductSellers";
import ProductVariations from "./ProductVariations";
import QuantitySelector from "./QuantitySelector";
import "./ProductContent.css";
import { useLanguage } from "../../hooks/useLanguage";

const ProductContent = ({
  product,
  handleToggleFavorite,
  minPrice,
  selectedOptions,
  handleSelect,
  selectedItem,
  selectedSellerId,
  setSelectedSellerId,
  quantity,
  setQuantity,
  addToCart,
  selectedSeller,
}) => {
  const { translations } = useLanguage();
  return (
    <div className="product-content">
      <ProductHeader
        product={product}
        onToggleFavorite={handleToggleFavorite}
      />
      <ProductRating
        averageRating={product.averageRating}
        ratingCount={product.ratingCount}
      />
      <ProductPrice minPrice={minPrice} />
      <ProductVariations
        variations={product.variations}
        selectedOptions={selectedOptions}
        handleSelect={handleSelect}
      />
      {/* SKU */}
      {selectedItem && (
        <p className="product-sku">
          <strong>SKU:</strong> <span>{selectedItem.sku}</span>
        </p>
      )}
      <ProductSellers
        selectedItem={selectedItem}
        selectedSellerId={selectedSellerId}
        setSelectedSellerId={setSelectedSellerId}
      />
      <QuantitySelector
        quantity={quantity}
        setQuantity={setQuantity}
        selectedSeller={selectedSeller}
      />
      <Button className="btn btn-primary btn-block" onClick={addToCart}>
        {translations.general.pages.product_details.add_to_cart}
      </Button>
      <ProductFeatures />
    </div>
  );
};
export default ProductContent;
