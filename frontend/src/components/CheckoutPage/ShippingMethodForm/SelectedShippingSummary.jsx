import { useLanguage } from "../../../hooks/useLanguage";
import { formatMoney } from "../../../utils/utils";

const SelectedShippingSummary = ({ method, cost }) => {
  const { translations, language } = useLanguage();
  if (!method) return null;

  const {
    selected_shipping,
    shipping_cost,
    free,
    delivery_label,
    delivery_days,
    same_day_delivery,
  } = translations.general.pages.checkout;

  return (
    <div className="selected-summary">
      <>
        <h3>{selected_shipping}:</h3>
        <div className="summary-details">
          <p>
            <strong>
              {language == "en" ? method?.nameEn : method?.nameAr}
            </strong>
          </p>
          {(method.descriptionEn || method.descriptionAr) && (
            <p>
              {language == "en" ? method.descriptionEn : method.descriptionAr}
            </p>
          )}

          <p className="total-cost">
            {shipping_cost}: {cost === 0 ? free : formatMoney(cost)}
          </p>
          {method.daysMin == 0 || method.daysMax == 0 ? (
            <p>{same_day_delivery}</p>
          ) : (
            <p>
              {delivery_label}: {method?.daysMin}-{method?.daysMax}{" "}
              {delivery_days}
            </p>
          )}
        </div>
      </>
    </div>
  );
};

export default SelectedShippingSummary;
