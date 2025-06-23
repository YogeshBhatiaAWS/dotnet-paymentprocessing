using System;
using Newtonsoft.Json;

namespace PaymentProcessing.WebAPI.Models
{
    /// <summary>
    /// Payment response model for API
    /// </summary>
    public class ApiPaymentResponse
    {
        /// <summary>
        /// Unique transaction identifier
        /// </summary>
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Payment status (Approved, Declined, Error)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Payment amount
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Error code (if applicable)
        /// </summary>
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Processing timestamp
        /// </summary>
        [JsonProperty("processedAt")]
        public DateTime ProcessedAt { get; set; }

        /// <summary>
        /// Authorization code (for approved transactions)
        /// </summary>
        [JsonProperty("authorizationCode")]
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// Reference number
        /// </summary>
        [JsonProperty("referenceNumber")]
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Merchant identifier
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }
    }
}
