using System;
using System.ComponentModel.DataAnnotations;
using PaymentProcessing.Core.Models;

namespace PaymentProcessing.Web.Models
{
    /// <summary>
    /// View model for payment processing forms
    /// </summary>
    public class PaymentViewModel
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Amount must be between $0.01 and $999,999.99")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Card number is required")]
        [StringLength(19, MinimumLength = 13, ErrorMessage = "Card number must be between 13 and 19 digits")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Card holder name is required")]
        [StringLength(100, ErrorMessage = "Card holder name cannot exceed 100 characters")]
        public string CardHolderName { get; set; }

        [Required(ErrorMessage = "Expiry month is required")]
        [Range(1, 12, ErrorMessage = "Expiry month must be between 1 and 12")]
        public int ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Expiry year is required")]
        [Range(2025, 2035, ErrorMessage = "Expiry year must be between 2025 and 2035")]
        public int ExpiryYear { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV must be 3 or 4 digits")]
        public string CVV { get; set; }

        [Required(ErrorMessage = "Merchant ID is required")]
        public string MerchantId { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Customer email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string CustomerEmail { get; set; }

        public string BillingAddress { get; set; }

        public PaymentViewModel()
        {
            Currency = "USD";
            MerchantId = "MERCHANT_001";
        }

        /// <summary>
        /// Converts view model to payment request
        /// </summary>
        public PaymentRequest ToPaymentRequest()
        {
            return new PaymentRequest
            {
                Amount = this.Amount,
                Currency = this.Currency,
                MerchantId = this.MerchantId,
                Description = this.Description,
                CustomerEmail = this.CustomerEmail,
                BillingAddress = this.BillingAddress,
                CreditCard = new CreditCard
                {
                    CardNumber = this.CardNumber,
                    CardHolderName = this.CardHolderName,
                    ExpiryMonth = this.ExpiryMonth,
                    ExpiryYear = this.ExpiryYear,
                    CVV = this.CVV
                }
            };
        }
    }

    /// <summary>
    /// View model for payment results
    /// </summary>
    public class PaymentResultViewModel
    {
        public string TransactionId { get; set; }
        public PaymentStatus Status { get; set; }
        public string AuthorizationCode { get; set; }
        public string ResponseMessage { get; set; }
        public decimal ProcessedAmount { get; set; }
        public DateTime ProcessedDateTime { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }

        public static PaymentResultViewModel FromPaymentResponse(PaymentResponse response)
        {
            return new PaymentResultViewModel
            {
                TransactionId = response.TransactionId,
                Status = response.Status,
                AuthorizationCode = response.AuthorizationCode,
                ResponseMessage = response.ResponseMessage,
                ProcessedAmount = response.ProcessedAmount,
                ProcessedDateTime = response.ProcessedDateTime,
                IsSuccessful = response.IsSuccessful,
                ErrorMessage = response.ErrorMessage
            };
        }
    }
}
