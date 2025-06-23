using System;

namespace PaymentProcessing.Core.Models
{
    /// <summary>
    /// Represents a credit card for payment processing
    /// </summary>
    public class CreditCard
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CVV { get; set; }
        public string CardType { get; set; }

        /// <summary>
        /// Gets the masked card number for display purposes
        /// </summary>
        public string MaskedCardNumber
        {
            get
            {
                if (string.IsNullOrEmpty(CardNumber) || CardNumber.Length < 4)
                    return "****";
                
                return "****-****-****-" + CardNumber.Substring(CardNumber.Length - 4);
            }
        }

        /// <summary>
        /// Validates if the credit card has expired
        /// </summary>
        public bool IsExpired
        {
            get
            {
                var currentDate = DateTime.Now;
                return currentDate.Year > ExpiryYear || 
                       (currentDate.Year == ExpiryYear && currentDate.Month > ExpiryMonth);
            }
        }
    }
}
