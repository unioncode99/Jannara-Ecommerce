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

// utils/formatDateTime.js

/**
 * Format a Date or timestamp as a human-readable string
 * @param {string|Date} date - Date object or ISO string
 * @param {Object} options - Formatting options
 * @param {string} options.locale - Locale for formatting (default: 'en-SD')
 * @param {Object} options.dateOptions - Intl.DateTimeFormat options
 * @returns {string} Formatted date string
 */
export function formatDateTime(
  date,
  {
    locale = "en-SD",
    dateOptions = {
      year: "numeric",
      month: "short",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
      second: "2-digit",
      hour12: false,
    },
  } = {}
) {
  if (!date) return "";
  const dt = typeof date === "string" ? new Date(date) : date;
  if (isNaN(dt)) return "";

  return new Intl.DateTimeFormat(locale, dateOptions).format(dt);
}
