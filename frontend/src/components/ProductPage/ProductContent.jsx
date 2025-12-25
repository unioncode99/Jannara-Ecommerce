import Button from "../ui/Button";
import ProductFeatures from "./ProductFeatures";
import ProductHeader from "./ProductHeader";
import ProductPrice from "./ProductPrice";
import ProductRating from "./ProductRating";
import ProductSellers from "./ProductSellers";
import ProductVariations from "./ProductVariations";
import "./ProductContent.css";
import { useLanguage } from "../../hooks/useLanguage";
import ProductQuantity from "./ProductQuantity";
import { useCart } from "../../contexts/CartContext";
import { ShoppingCart } from "lucide-react";
import { toast } from "../../components/ui/Toast";
import { useEffect, useState } from "react";

const ProductContent = ({
  product,
  handleToggleFavorite,
  minPrice,
  selectedOptions,
  handleSelect,
  selectedItem,
  selectedSellerProductId,
  setSelectedSellerProductId,
  quantity,
  setQuantity,
  selectedSeller,
}) => {
  const { translations } = useLanguage();
  const {
    add_to_cart,
    added_to_cart,
    seller_not_selected,
    already_in_cart,
    quantity_not_selected,
  } = translations.general.pages.product_details;
  const { addOrUpdateItem, isInCart } = useCart();
  const customerId = 1; // for test
  // const [isAddedToCart, setIsAddedToCart] = useState(false);
  const isAddedToCart = isInCart(selectedSellerProductId);

  async function addToCart() {
    console.log("selectedSellerProductId -> ", selectedSellerProductId);
    console.log("isAddedToCart -> ", isAddedToCart);
    console.log("selectedItem -> ", selectedItem);

    if (!selectedSellerProductId) {
      toast.show(seller_not_selected, "error");
      return;
    }

    if (isAddedToCart) {
      toast.show(already_in_cart, "error");
      return;
    }

    if (!quantity || quantity <= 0) {
      toast.show(quantity_not_selected, "error");
      return;
    }

    await addOrUpdateItem({
      customerId: customerId,
      sellerProductId: selectedSellerProductId,
      quantity: quantity,
    });
    toast.show(added_to_cart, "success");
  }

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
        selectedSellerProductId={selectedSellerProductId}
        setSelectedSellerProductId={setSelectedSellerProductId}
      />
      {selectedSeller && (
        <ProductQuantity
          quantity={quantity}
          setQuantity={setQuantity}
          stockQuantity={selectedSeller.stockQuantity}
        />
      )}

      <Button
        className={`btn btn-primary btn-block add-to-cart-btn ${
          isAddedToCart && "added"
        }`}
        onClick={addToCart}
      >
        <ShoppingCart />
        <span>{isAddedToCart ? added_to_cart : add_to_cart}</span>
      </Button>
      <ProductFeatures />
    </div>
  );
};
export default ProductContent;
