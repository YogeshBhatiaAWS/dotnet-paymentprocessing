using System;

namespace PaymentProcessing.Core.Models
{
    /// <summary>
    /// Represents a payment request containing transaction details
    /// </summary>
    public class PaymentRequest
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public CreditCard CreditCard { get; set; }
        public string MerchantId { get; set; }
        public string Description { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string CustomerEmail { get; set; }
        public string BillingAddress { get; set; }

        public PaymentRequest()
        {
            TransactionId = Guid.NewGuid().ToString();
            RequestDateTime = DateTime.Now;
            Currency = "USD";
        }
    }
}
