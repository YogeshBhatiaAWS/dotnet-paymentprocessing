using System;
using Newtonsoft.Json;

namespace PaymentProcessing.Core.Models
{
    public class PaymentResponse
    {
        public string TransactionId { get; set; }
        
        public PaymentStatus Status { get; set; }
        
        public decimal Amount { get; set; }
        
        public string Currency { get; set; }
        
        public string Message { get; set; }
        
        public string ErrorCode { get; set; }
        
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
        
        public string MerchantId { get; set; }
        
        public string AuthorizationCode { get; set; }
        
        public string ReferenceNumber { get; set; }

        [JsonIgnore]
        public bool IsSuccessful => Status == PaymentStatus.Approved;

        [JsonIgnore]
        public bool IsDeclined => Status == PaymentStatus.Declined;

        [JsonIgnore]
        public bool HasError => Status == PaymentStatus.Error;

        public static PaymentResponse CreateSuccessful(string transactionId, decimal amount, string currency, string merchantId)
        {
            return new PaymentResponse
            {
                TransactionId = transactionId,
                Status = PaymentStatus.Approved,
                Amount = amount,
                Currency = currency,
                MerchantId = merchantId,
                Message = "Payment processed successfully",
                AuthorizationCode = GenerateAuthCode(),
                ReferenceNumber = GenerateReferenceNumber()
            };
        }

        public static PaymentResponse CreateDeclined(string transactionId, decimal amount, string currency, string merchantId, string reason = "Insufficient funds")
        {
            return new PaymentResponse
            {
                TransactionId = transactionId,
                Status = PaymentStatus.Declined,
                Amount = amount,
                Currency = currency,
                MerchantId = merchantId,
                Message = reason,
                ErrorCode = "DECLINED"
            };
        }

        public static PaymentResponse CreateError(string transactionId, decimal amount, string currency, string merchantId, string error)
        {
            return new PaymentResponse
            {
                TransactionId = transactionId,
                Status = PaymentStatus.Error,
                Amount = amount,
                Currency = currency,
                MerchantId = merchantId,
                Message = error,
                ErrorCode = "PROCESSING_ERROR"
            };
        }

        private static string GenerateAuthCode()
        {
            return "AUTH" + DateTime.UtcNow.Ticks.ToString().Substring(10);
        }

        private static string GenerateReferenceNumber()
        {
            return "REF" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
}
