import { useLanguage } from "../../../hooks/useLanguage";
import { formatMoney } from "../../../utils/utils";

const ShippingCartSummary = ({ cartSubTotal, cartTotalWeight }) => {
  const { translations } = useLanguage();
  if (!cartTotalWeight) return null;

  const { cart_weight, cart_total, weight_unit } =
    translations.general.pages.shipping_method;

  return (
    <div className="cart-summary">
      <p>
        <strong>{cart_weight}:</strong> {cartTotalWeight} {weight_unit}
      </p>
      <p>
        <strong>{cart_total}:</strong> {formatMoney(cartSubTotal || 0)}
      </p>
    </div>
  );
};

export default ShippingCartSummary;
