using System;
using System.Runtime.Serialization;
using PaymentProcessing.Core.Models;

namespace PaymentProcessing.WebAPI.Models
{
    /// <summary>
    /// API model for payment responses
    /// </summary>
    [DataContract]
    public class ApiPaymentResponse
    {
        [DataMember]
        public string TransactionId { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string AuthorizationCode { get; set; }

        [DataMember]
        public string ResponseMessage { get; set; }

        [DataMember]
        public decimal ProcessedAmount { get; set; }

        [DataMember]
        public DateTime ProcessedDateTime { get; set; }

        [DataMember]
        public string ProcessorTransactionId { get; set; }

        [DataMember]
        public bool IsSuccessful { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Creates API response from core payment response
        /// </summary>
        public static ApiPaymentResponse FromPaymentResponse(PaymentResponse response)
        {
            return new ApiPaymentResponse
            {
                TransactionId = response.TransactionId,
                Status = response.Status.ToString(),
                AuthorizationCode = response.AuthorizationCode,
                ResponseMessage = response.ResponseMessage,
                ProcessedAmount = response.ProcessedAmount,
                ProcessedDateTime = response.ProcessedDateTime,
                ProcessorTransactionId = response.ProcessorTransactionId,
                IsSuccessful = response.IsSuccessful,
                ErrorCode = response.ErrorCode,
                ErrorMessage = response.ErrorMessage
            };
        }
    }

    /// <summary>
    /// API model for error responses
    /// </summary>
    [DataContract]
    public class ApiErrorResponse
    {
        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public DateTime Timestamp { get; set; }

        public ApiErrorResponse()
        {
            Timestamp = DateTime.Now;
        }

        public ApiErrorResponse(string error, string message) : this()
        {
            Error = error;
            Message = message;
        }
    }
}
