using System.ServiceModel;
using System.ServiceModel.Web;
using PaymentProcessing.WebAPI.Models;

namespace PaymentProcessing.WebAPI.Services
{
    /// <summary>
    /// WCF Service contract for payment API operations
    /// </summary>
    [ServiceContract]
    public interface IPaymentApiService
    {
        /// <summary>
        /// Process a payment transaction
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST", 
                   UriTemplate = "/payments", 
                   RequestFormat = WebMessageFormat.Json, 
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        ApiPaymentResponse ProcessPayment(ApiPaymentRequest request);

        /// <summary>
        /// Get transaction status
        /// </summary>
        [OperationContract]
        [WebGet(UriTemplate = "/payments/{transactionId}/status", 
                ResponseFormat = WebMessageFormat.Json)]
        ApiPaymentResponse GetTransactionStatus(string transactionId);

        /// <summary>
        /// Process a refund
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST", 
                   UriTemplate = "/payments/{transactionId}/refund", 
                   RequestFormat = WebMessageFormat.Json, 
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        ApiPaymentResponse ProcessRefund(string transactionId, ApiRefundRequest request);

        /// <summary>
        /// Get API health status
        /// </summary>
        [OperationContract]
        [WebGet(UriTemplate = "/health", 
                ResponseFormat = WebMessageFormat.Json)]
        string GetHealthStatus();

        /// <summary>
        /// Get API version information
        /// </summary>
        [OperationContract]
        [WebGet(UriTemplate = "/version", 
                ResponseFormat = WebMessageFormat.Json)]
        string GetVersion();
    }
}
