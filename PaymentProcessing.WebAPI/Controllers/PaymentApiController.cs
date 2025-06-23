using System;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using PaymentProcessing.Core.Services;
using PaymentProcessing.WebAPI.Models;

namespace PaymentProcessing.WebAPI.Controllers
{
    /// <summary>
    /// ASMX Web Service for payment processing (alternative to WCF)
    /// </summary>
    [WebService(Namespace = "http://paymentprocessing.api/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class PaymentApiController : WebService
    {
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IPaymentValidator _paymentValidator;

        public PaymentApiController()
        {
            _paymentValidator = new PaymentValidator();
            _paymentProcessor = new PaymentProcessor(_paymentValidator);
        }

        /// <summary>
        /// Process a payment transaction via ASMX web service
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ProcessPayment(string paymentRequestJson)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                var request = serializer.Deserialize<ApiPaymentRequest>(paymentRequestJson);

                if (request == null)
                {
                    var errorResponse = new ApiErrorResponse("INVALID_REQUEST", "Payment request cannot be null");
                    return serializer.Serialize(errorResponse);
                }

                // Convert API request to core payment request
                var paymentRequest = request.ToPaymentRequest();

                // Process the payment
                var paymentResponse = _paymentProcessor.ProcessPayment(paymentRequest);

                // Convert core response to API response
                var apiResponse = ApiPaymentResponse.FromPaymentResponse(paymentResponse);
                return serializer.Serialize(apiResponse);
            }
            catch (Exception ex)
            {
                var serializer = new JavaScriptSerializer();
                var errorResponse = new ApiErrorResponse("PROCESSING_ERROR", ex.Message);
                return serializer.Serialize(errorResponse);
            }
        }

        /// <summary>
        /// Get transaction status via ASMX web service
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTransactionStatus(string transactionId)
        {
            try
            {
                var serializer = new JavaScriptSerializer();

                if (string.IsNullOrEmpty(transactionId))
                {
                    var errorResponse = new ApiErrorResponse("INVALID_REQUEST", "Transaction ID is required");
                    return serializer.Serialize(errorResponse);
                }

                var paymentResponse = _paymentProcessor.GetTransactionStatus(transactionId);
                var apiResponse = ApiPaymentResponse.FromPaymentResponse(paymentResponse);
                return serializer.Serialize(apiResponse);
            }
            catch (Exception ex)
            {
                var serializer = new JavaScriptSerializer();
                var errorResponse = new ApiErrorResponse("PROCESSING_ERROR", ex.Message);
                return serializer.Serialize(errorResponse);
            }
        }

        /// <summary>
        /// Process a refund via ASMX web service
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ProcessRefund(string transactionId, string refundRequestJson)
        {
            try
            {
                var serializer = new JavaScriptSerializer();

                if (string.IsNullOrEmpty(transactionId))
                {
                    var errorResponse = new ApiErrorResponse("INVALID_REQUEST", "Transaction ID is required");
                    return serializer.Serialize(errorResponse);
                }

                var refundRequest = serializer.Deserialize<ApiRefundRequest>(refundRequestJson);
                if (refundRequest == null)
                {
                    var errorResponse = new ApiErrorResponse("INVALID_REQUEST", "Refund request cannot be null");
                    return serializer.Serialize(errorResponse);
                }

                var paymentResponse = _paymentProcessor.RefundPayment(transactionId, refundRequest.Amount);
                var apiResponse = ApiPaymentResponse.FromPaymentResponse(paymentResponse);
                return serializer.Serialize(apiResponse);
            }
            catch (Exception ex)
            {
                var serializer = new JavaScriptSerializer();
                var errorResponse = new ApiErrorResponse("PROCESSING_ERROR", ex.Message);
                return serializer.Serialize(errorResponse);
            }
        }

        /// <summary>
        /// Get API health status
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetHealthStatus()
        {
            try
            {
                return "{ \"status\": \"healthy\", \"timestamp\": \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\", \"version\": \"1.0.0\" }";
            }
            catch (Exception ex)
            {
                return "{ \"status\": \"unhealthy\", \"error\": \"" + ex.Message + "\", \"timestamp\": \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\" }";
            }
        }

        /// <summary>
        /// Get API version information
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetVersion()
        {
            return "{ \"version\": \"1.0.0\", \"name\": \"Payment Processing API\", \"framework\": \".NET Framework 3.5\", \"build\": \"" + DateTime.Now.ToString("yyyyMMdd") + "\" }";
        }
    }
}
