using System;
using System.Text.RegularExpressions;
using PaymentProcessing.Core.Models;

namespace PaymentProcessing.Core.Services
{
    /// <summary>
    /// Implementation of payment validation services
    /// </summary>
    public class PaymentValidator : IPaymentValidator
    {
        private string _validationError;

        public bool ValidatePaymentRequest(PaymentRequest request)
        {
            if (request == null)
            {
                _validationError = "Payment request cannot be null";
                return false;
            }

            if (request.Amount <= 0)
            {
                _validationError = "Payment amount must be greater than zero";
                return false;
            }

            if (string.IsNullOrEmpty(request.Currency))
            {
                _validationError = "Currency is required";
                return false;
            }

            if (string.IsNullOrEmpty(request.MerchantId))
            {
                _validationError = "Merchant ID is required";
                return false;
            }

            if (!ValidateCreditCard(request.CreditCard))
            {
                return false;
            }

            _validationError = null;
            return true;
        }

        public bool ValidateCreditCard(CreditCard creditCard)
        {
            if (creditCard == null)
            {
                _validationError = "Credit card information is required";
                return false;
            }

            if (string.IsNullOrEmpty(creditCard.CardNumber))
            {
                _validationError = "Card number is required";
                return false;
            }

            // Remove spaces and dashes from card number
            string cleanCardNumber = creditCard.CardNumber.Replace(" ", "").Replace("-", "");
            
            if (!IsValidCardNumber(cleanCardNumber))
            {
                _validationError = "Invalid card number";
                return false;
            }

            if (string.IsNullOrEmpty(creditCard.CardHolderName))
            {
                _validationError = "Card holder name is required";
                return false;
            }

            if (creditCard.ExpiryMonth < 1 || creditCard.ExpiryMonth > 12)
            {
                _validationError = "Invalid expiry month";
                return false;
            }

            if (creditCard.ExpiryYear < DateTime.Now.Year)
            {
                _validationError = "Card has expired";
                return false;
            }

            if (creditCard.IsExpired)
            {
                _validationError = "Card has expired";
                return false;
            }

            if (string.IsNullOrEmpty(creditCard.CVV) || creditCard.CVV.Length < 3 || creditCard.CVV.Length > 4)
            {
                _validationError = "Invalid CVV";
                return false;
            }

            _validationError = null;
            return true;
        }

        public string GetValidationError()
        {
            return _validationError;
        }

        /// <summary>
        /// Validates card number using Luhn algorithm
        /// </summary>
        private bool IsValidCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || !Regex.IsMatch(cardNumber, @"^\d+$"))
                return false;

            if (cardNumber.Length < 13 || cardNumber.Length > 19)
                return false;

            // Luhn algorithm
            int sum = 0;
            bool alternate = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber[i].ToString());

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit = (digit % 10) + 1;
                }

                sum += digit;
                alternate = !alternate;
            }

            return (sum % 10) == 0;
        }
    }
}
