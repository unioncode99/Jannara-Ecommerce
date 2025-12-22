import "./ProductSellers.css";
import { useLanguage } from "../../hooks/useLanguage";
import { formatMoney } from "../../utils/utils";

function ProductSellers({
  selectedItem,
  selectedSellerId,
  setSelectedSellerId,
}) {
  const { translations } = useLanguage();
  const {
    available_sellers,
    seller: seller_text,
    price: price_text,
    stock: stock_text,
    selected_seller,
    no_seller_selected,
    no_sellers_available,
  } = translations.general.pages.product_details;

  const selectedSeller = selectedItem?.sellerProducts?.find(
    (s) => s.sellerProductId === selectedSellerId
  );

  return (
    <>
      {!selectedItem?.sellerProducts?.length ? (
        <p>{no_sellers_available}</p>
      ) : (
        <>
          <h3>{available_sellers}</h3>
          <div className="seller-list-container">
            {selectedItem.sellerProducts.map((seller) => {
              const isSelected = seller.sellerProductId === selectedSellerId;
              return (
                <p
                  key={seller.sellerProductId}
                  className={`seller-list-item ${isSelected ? "active" : ""}`}
                  onClick={() => setSelectedSellerId(seller.sellerProductId)}
                >
                  {seller_text} #{seller.sellerId} | {price_text}:{" "}
                  {formatMoney(seller.price)} | {stock_text}:{" "}
                  {seller.stockQuantity}
                </p>
              );
            })}
          </div>
          <p className="select-seller">
            {selectedSeller
              ? `${selected_seller}: #${
                  selectedSeller.sellerId
                } | ${price_text}: ${formatMoney(
                  selectedSeller.price
                )} | ${stock_text}: ${selectedSeller.stockQuantity}`
              : `${no_seller_selected}`}
          </p>
        </>
      )}
    </>
  );
}

export default ProductSellers;
