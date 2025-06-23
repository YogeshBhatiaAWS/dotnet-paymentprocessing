using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;

namespace PaymentProcessing.Core.Models
{
    public class CreditCard
    {
        [Required]
        [CreditCard]
        public string Number { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string HolderName { get; set; }

        [Required]
        [Range(1, 12)]
        public int ExpiryMonth { get; set; }

        [Required]
        [Range(2024, 2050)]
        public int ExpiryYear { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; }

        [JsonIgnore]
        public string MaskedNumber => MaskCardNumber(Number);

        [JsonIgnore]
        public string CardType => GetCardType(Number);

        [JsonIgnore]
        public bool IsExpired => DateTime.Now > new DateTime(ExpiryYear, ExpiryMonth, DateTime.DaysInMonth(ExpiryYear, ExpiryMonth));

        [JsonIgnore]
        public bool IsValid => IsValidLuhn(Number) && !IsExpired;

        private static string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
                return "****";

            return "****-****-****-" + cardNumber.Substring(cardNumber.Length - 4);
        }

        private static string GetCardType(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return "Unknown";

            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            if (cardNumber.StartsWith("4"))
                return "Visa";
            if (cardNumber.StartsWith("5") || cardNumber.StartsWith("2"))
                return "MasterCard";
            if (cardNumber.StartsWith("3"))
                return "American Express";
            if (cardNumber.StartsWith("6"))
                return "Discover";

            return "Unknown";
        }

        public static bool IsValidLuhn(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return false;

            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            if (!cardNumber.All(char.IsDigit))
                return false;

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

            return sum % 10 == 0;
        }
    }
}
