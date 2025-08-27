import { clsx } from "clsx";
import { twMerge } from "tailwind-merge"

/**
 * Utility function to merge Tailwind CSS classes
 * Combines clsx and tailwind-merge for optimal class handling
 * @param {...any} inputs - Class names to merge
 * @returns {string} Merged class names
 */
export function cn(...inputs) {
  return twMerge(clsx(inputs));
}

/**
 * Formats a number as currency
 * @param {number} amount - Amount to format
 * @param {string} currency - Currency code (default: 'EGP')
 * @returns {string} Formatted currency string
 */
export function formatCurrency(amount, currency = 'EGP') {
  return new Intl.NumberFormat('ar-EG', {
    style: 'currency',
    currency: currency,
    minimumFractionDigits: 2
  }).format(amount);
}

/**
 * Formats a date in Arabic locale
 * @param {Date|string} date - Date to format
 * @param {Object} options - Intl.DateTimeFormat options
 * @returns {string} Formatted date string
 */
export function formatDate(date, options = {}) {
  const defaultOptions = {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  };
  
  return new Intl.DateTimeFormat('ar-EG', { ...defaultOptions, ...options })
    .format(new Date(date));
}

/**
 * Formats a date and time in Arabic locale
 * @param {Date|string} date - Date to format
 * @returns {string} Formatted date and time string
 */
export function formatDateTime(date) {
  return new Intl.DateTimeFormat('ar-EG', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  }).format(new Date(date));
}

/**
 * Formats a number with Arabic numerals and thousands separators
 * @param {number} number - Number to format
 * @param {number} decimals - Number of decimal places (default: 2)
 * @returns {string} Formatted number string
 */
export function formatNumber(number, decimals = 2) {
  return new Intl.NumberFormat('ar-EG', {
    minimumFractionDigits: decimals,
    maximumFractionDigits: decimals
  }).format(number);
}

/**
 * Gets the status color class based on status name
 * @param {string} status - Status name
 * @returns {string} Tailwind CSS color class
 */
export function getStatusColor(status) {
  const statusColors = {
    'مسودة': 'bg-gray-100 text-gray-800',
    'مؤكدة': 'bg-blue-100 text-blue-800',
    'مدفوعة جزئياً': 'bg-yellow-100 text-yellow-800',
    'مدفوعة كاملة': 'bg-green-100 text-green-800',
    'ملغية': 'bg-red-100 text-red-800',
    'متوفر': 'bg-green-100 text-green-800',
    'كمية منخفضة': 'bg-yellow-100 text-yellow-800',
    'نفدت الكمية': 'bg-red-100 text-red-800',
    'مدين': 'bg-red-100 text-red-800',
    'دائن': 'bg-green-100 text-green-800',
    'متوازن': 'bg-gray-100 text-gray-800'
  };
  
  return statusColors[status] || 'bg-gray-100 text-gray-800';
}
