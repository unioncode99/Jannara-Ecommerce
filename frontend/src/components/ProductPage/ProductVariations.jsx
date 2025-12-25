import { useLanguage } from "../../hooks/useLanguage";
import "./ProductVariations.css";

function ProductVariations({ variations, selectedOptions, handleSelect }) {
  const { language, translations } = useLanguage();
  const { no_options_available } = translations.general.pages.product_details;

  if (!variations) {
    return null;
  }

  return (
    <div className="product-variations">
      {variations.map((variation) => {
        const selectedOption = variation?.options?.find(
          (option) =>
            option.variationOptionId === selectedOptions[variation.variationId]
        );
        return (
          <div key={variation.variationId}>
            <h4>
              {language === "en" ? variation.nameEn : variation.nameAr}:{" "}
              {selectedOption
                ? language === "en"
                  ? selectedOption.valueEn
                  : selectedOption.valueAr
                : ""}
            </h4>
            <div>
              {variation?.options && variation.options.length > 0 ? (
                variation?.options?.map((option) => {
                  const isSelected =
                    selectedOptions[variation.variationId] ===
                    option.variationOptionId;
                  return (
                    <button
                      key={option.variationOptionId}
                      className={isSelected ? "active" : ""}
                      onClick={() =>
                        handleSelect(
                          variation.variationId,
                          option.variationOptionId
                        )
                      }
                    >
                      {language === "en" ? option.valueEn : option.valueAr}
                    </button>
                  );
                })
              ) : (
                <span className="no-options">{no_options_available}</span>
              )}
            </div>
          </div>
        );
      })}
    </div>
  );
}

export default ProductVariations;
