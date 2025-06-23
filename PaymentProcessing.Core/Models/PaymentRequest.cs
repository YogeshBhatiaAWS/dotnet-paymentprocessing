using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PaymentProcessing.Core.Models
{
    public class PaymentRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = "USD";

        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CardHolderName { get; set; }

        [Required]
        [Range(1, 12)]
        public int ExpiryMonth { get; set; }

        [Required]
        [Range(2024, 2050)]
        public int ExpiryYear { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; }

        [Required]
        public string MerchantId { get; set; }

        [EmailAddress]
        public string CustomerEmail { get; set; }

        public string Description { get; set; }

        public DateTime RequestTimestamp { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public CreditCard CreditCard => new CreditCard
        {
            Number = CardNumber,
            HolderName = CardHolderName,
            ExpiryMonth = ExpiryMonth,
            ExpiryYear = ExpiryYear,
            CVV = CVV
        };
    }
}
