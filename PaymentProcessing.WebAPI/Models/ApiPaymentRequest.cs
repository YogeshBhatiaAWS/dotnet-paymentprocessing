using System;
using System.Runtime.Serialization;
using PaymentProcessing.Core.Models;

namespace PaymentProcessing.WebAPI.Models
{
    /// <summary>
    /// API model for payment requests
    /// </summary>
    [DataContract]
    public class ApiPaymentRequest
    {
        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string CardNumber { get; set; }

        [DataMember]
        public string CardHolderName { get; set; }

        [DataMember]
        public int ExpiryMonth { get; set; }

        [DataMember]
        public int ExpiryYear { get; set; }

        [DataMember]
        public string CVV { get; set; }

        [DataMember]
        public string MerchantId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string CustomerEmail { get; set; }

        [DataMember]
        public string BillingAddress { get; set; }

        /// <summary>
        /// Converts API request to core payment request
        /// </summary>
        public PaymentRequest ToPaymentRequest()
        {
            return new PaymentRequest
            {
                Amount = this.Amount,
                Currency = this.Currency ?? "USD",
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
    /// API model for refund requests
    /// </summary>
    [DataContract]
    public class ApiRefundRequest
    {
        [DataMember]
        public string TransactionId { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Reason { get; set; }
    }

    /// <summary>
    /// API model for transaction status requests
    /// </summary>
    [DataContract]
    public class ApiStatusRequest
    {
        [DataMember]
        public string TransactionId { get; set; }
    }
}
