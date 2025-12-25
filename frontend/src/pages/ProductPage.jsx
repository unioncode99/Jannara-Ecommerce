import { Link, useParams } from "react-router-dom";
import "./ProductPage.css";
import { ChevronLeft, ChevronRight } from "lucide-react";
import { useEffect, useState } from "react";
import { create, read, remove } from "../api/apiWrapper";
import { useLanguage } from "../hooks/useLanguage";
import ProductGallery from "../components/ProductPage/ProductGallery";
import ProductContent from "../components/ProductPage/ProductContent";
import SpinnerLoader from "../components/ui/SpinnerLoader";

const ProductPage = () => {
  const { publicId } = useParams();
  const { language, translations } = useLanguage();
  console.log("publicId", publicId);
  const [product, setProduct] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [selectedOptions, setSelectedOptions] = useState({});
  const [selectedItem, setSelectedItem] = useState(null);
  const [selectedSellerProductId, setSelectedSellerProductId] = useState(null);
  const [quantity, setQuantity] = useState(1);
  const customerId = 1;

  const { back_to_products, product_not_found } =
    translations.general.pages.product_details;

  // Find selected seller
  const selectedSeller = selectedItem?.sellerProducts?.find(
    (s) => s.sellerProductId === selectedSellerProductId
  );

  useEffect(() => {
    setQuantity(0);
  }, [selectedSellerProductId]);

  // Fetch product details
  const fetchProduct = async () => {
    try {
      setIsLoading(true);
      const queryParams = new URLSearchParams();
      queryParams.append("publicId", publicId);
      if (customerId) queryParams.append("customerId", customerId);
      const url = `products/details?${queryParams}`;
      const data = await read(url);
      setProduct(data.data);
    } catch (err) {
      console.error("Failed to fetch product", err);
    } finally {
      setIsLoading(false);
    }
  };

  // Preselect first option of each variation
  useEffect(() => {
    fetchProduct();
  }, [publicId]);

  useEffect(() => {
    if (!product?.variations) return;

    const defaults = {};

    product.variations.forEach((v) => {
      if (v?.options && v.options.length > 0) {
        defaults[v.variationId] = v.options[0].variationOptionId;
      }
    });

    setQuantity(0);
    setSelectedOptions(defaults);
  }, [product]);

  // Match SKU when options change
  useEffect(() => {
    if (!product?.productItems) return;
    const item = product.productItems.find((item) =>
      item.productItemVariationOptions.every((opt) =>
        Object.values(selectedOptions).includes(opt.variationOptionId)
      )
    );
    setSelectedSellerProductId(0);
    setSelectedItem(item || null);
    setQuantity(0);
  }, [selectedOptions, product]);

  // Handle option selection
  const handleSelect = (variationId, optionId) => {
    setSelectedOptions((prev) => ({ ...prev, [variationId]: optionId }));
    setQuantity(0);
    setSelectedSellerProductId(0);
  };

  // Toggle favorite
  const handleToggleFavorite = async () => {
    try {
      if (!product.isFavorite) {
        await create("customer-wish-list", {
          customerId,
          productId: product.productId,
        });
      } else {
        await remove("customer-wish-list", {
          customerId,
          productId: product.productId,
        });
      }
      setProduct({ ...product, isFavorite: !product.isFavorite });
    } catch (error) {
      console.error("Favorite error:", error);
      setProduct({ ...product, isFavorite: !product.isFavorite });
    }
  };

  const minPrice = selectedItem?.sellerProducts?.length
    ? Math.min(...selectedItem.sellerProducts.map((s) => s.price))
    : null;

  return (
    <>
      <Link to="/" className="back-to-home-link">
        {language == "en" ? <ChevronLeft /> : <ChevronRight />}{" "}
        <span>{back_to_products}</span>
      </Link>
      {isLoading ? (
        <div className="loader-container">
          <SpinnerLoader />
        </div>
      ) : !product ? (
        <p>{product_not_found}</p>
      ) : (
        <div className="product-page-container">
          <ProductGallery
            selectedItem={selectedItem}
            selectedSellerProductId={selectedSellerProductId}
          />
          <ProductContent
            product={product}
            selectedItem={selectedItem}
            selectedSellerProductId={selectedSellerProductId}
            setSelectedSellerProductId={setSelectedSellerProductId}
            selectedOptions={selectedOptions}
            handleSelect={handleSelect}
            quantity={quantity}
            setQuantity={setQuantity}
            selectedSeller={selectedSeller}
            handleToggleFavorite={handleToggleFavorite}
            minPrice={minPrice}
          />
        </div>
      )}
    </>
  );
};
export default ProductPage;
