// utils/formatMoney.js

/**
 * Format a number as currency with locale support
 * @param {number} amount - The numeric value to format
 * @param {Object} options - Formatting options
 * @param {string} options.currencySymbol - Currency symbol (default: 'SDG')
 * @param {boolean} options.showSymbol - Whether to show the symbol (default: true)
 * @param {number} options.decimals - Number of decimal places (default: 0)
 * @param {string} options.locale - Locale for number formatting (default: 'en-SD')
 * @returns {string} Formatted currency string
 */
export function formatMoney(
  amount,
  {
    currencySymbol = "SDG",
    showSymbol = false,
    decimals = 2,
    locale = "en-SD",
  } = {}
) {
  if (typeof amount !== "number") return "";

  const formatted = amount.toLocaleString(locale, {
    minimumFractionDigits: decimals,
    maximumFractionDigits: decimals,
  });

  return showSymbol ? `${currencySymbol} ${formatted}` : formatted;
}
