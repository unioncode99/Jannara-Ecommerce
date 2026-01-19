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
  } = {},
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
  } = {},
) {
  if (!date) return "";
  const dt = typeof date === "string" ? new Date(date) : date;
  if (isNaN(dt)) return "";

  return new Intl.DateTimeFormat(locale, dateOptions).format(dt);
}

export function isImageValid(file) {
  if (!file) {
    return false;
  }
  if (!file.type.startsWith("image/")) {
    return false;
  }
  const maxSize = 2 * 1024 * 1024;

  if (file.size > maxSize) {
    return false;
  }

  return true;
}

/**
 * Format Sudan local phone numbers as 10 digits without spaces
 * Examples:
 * 965000000      -> 0965000000
 * 0965000000     -> 0965000000
 * +249965000000  -> 0965000000
 * @param {string} phone
 * @returns {string} formatted phone or "-" if invalid
 */
export const formatSudanPhoneLocal = (phone) => {
  if (!phone) return "-";

  // Remove non-digit characters
  let digits = phone.replace(/\D/g, "");

  // Remove country code if present (249)
  if (digits.startsWith("249")) {
    digits = "0" + digits.slice(3);
  }

  // Add leading 0 if only 9 digits
  if (digits.length === 9) digits = "0" + digits;

  // Return only if 10 digits
  if (digits.length !== 10) return phone;

  return digits;
};
