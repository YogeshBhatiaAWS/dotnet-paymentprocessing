using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PaymentProcessing.WebAPI.Models
{
    /// <summary>
    /// Payment request model for API
    /// </summary>
    public class ApiPaymentRequest
    {
        /// <summary>
        /// Payment amount
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency code (USD, EUR, GBP, etc.)
        /// </summary>
        [StringLength(3, MinimumLength = 3)]
        [JsonProperty("currency")]
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// Credit card number
        /// </summary>
        [Required]
        [CreditCard]
        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        /// <summary>
        /// Cardholder name
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [JsonProperty("cardHolderName")]
        public string CardHolderName { get; set; }

        /// <summary>
        /// Card expiry month (1-12)
        /// </summary>
        [Required]
        [Range(1, 12)]
        [JsonProperty("expiryMonth")]
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// Card expiry year
        /// </summary>
        [Required]
        [Range(2024, 2050)]
        [JsonProperty("expiryYear")]
        public int ExpiryYear { get; set; }

        /// <summary>
        /// Card verification value
        /// </summary>
        [Required]
        [StringLength(4, MinimumLength = 3)]
        [JsonProperty("cvv")]
        public string CVV { get; set; }

        /// <summary>
        /// Merchant identifier
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        /// Customer email address
        /// </summary>
        [EmailAddress]
        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Payment description
        /// </summary>
        [StringLength(500)]
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
