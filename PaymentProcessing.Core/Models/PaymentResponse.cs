using System;

namespace PaymentProcessing.Core.Models
{
    /// <summary>
    /// Represents the response from a payment processing request
    /// </summary>
    public class PaymentResponse
    {
        public string TransactionId { get; set; }
        public PaymentStatus Status { get; set; }
        public string AuthorizationCode { get; set; }
        public string ResponseMessage { get; set; }
        public decimal ProcessedAmount { get; set; }
        public DateTime ProcessedDateTime { get; set; }
        public string ProcessorTransactionId { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public PaymentResponse()
        {
            ProcessedDateTime = DateTime.Now;
            Status = PaymentStatus.Pending;
        }

        public PaymentResponse(string transactionId) : this()
        {
            TransactionId = transactionId;
        }
    }
}
