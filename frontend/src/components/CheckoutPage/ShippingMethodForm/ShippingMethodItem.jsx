import { useLanguage } from "../../../hooks/useLanguage";
import { formatMoney } from "../../../utils/utils";

const ShippingMethodItem = ({
  method,
  selectedShippingMethodId,
  setSelectedShippingMethodId,
  isFree,
  cost,
  cartTotalWeight,
  cartSubTotal,
}) => {
  const { translations, language } = useLanguage();
  const {
    free,
    base_price,
    free_shipping_over,
    same_day_delivery,
    delivery_days,
    weight_unit,
  } = translations.general.pages.shipping_method;

  return (
    <div>
      <input
        type="radio"
        id={method.id}
        name="shippingMethod"
        value={method.id}
        checked={selectedShippingMethodId === method.id}
        onClick={() => setSelectedShippingMethodId(method.id)}
        // onChange={(e) => setSelectedShippingMethodId(e.target.value)}
      />
      <label htmlFor={method.id}>
        <div className="method-header">
          <span className="method-name">
            {language == "en" ? method.nameEn : method.nameAr}
          </span>
          <span className={`method-price ${isFree ? `free` : ""}`}>
            {isFree ? free : formatMoney(cost)}
          </span>
        </div>
        <div className="method-details">
          {method.descriptionEn && (
            <p className="method-description">
              {language == "en" ? method.descriptionEn : method.descriptionAr}
            </p>
          )}
          <div className="method-info">
            {method.daysMin == 0 || method.daysMax == 0 ? (
              <span className="delivery-time">{same_day_delivery}</span>
            ) : (
              <span className="delivery-time">
                {method.daysMin}-{method.daysMax} {delivery_days}
              </span>
            )}
            {method.freeOver !== null &&
              method.freeOver !== undefined &&
              cartSubTotal < method.freeOver && (
                <span className="free-shipping-note">
                  {free_shipping_over} {formatMoney(method.freeOver)}
                </span>
              )}
            {cost > 0 && (
              <span className="price-breakdown">
                {formatMoney(method.basePrice)} {base_price} +
                {method.pricePerKg
                  ? ` ${cartTotalWeight || 0}${weight_unit} × ${formatMoney(
                      method.pricePerKg
                    )}`
                  : ""}
              </span>
            )}
          </div>
        </div>
      </label>
    </div>
  );
};
export default ShippingMethodItem;
