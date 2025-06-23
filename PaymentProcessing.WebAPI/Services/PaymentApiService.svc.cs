using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using PaymentProcessing.Core.Services;
using PaymentProcessing.WebAPI.Models;

namespace PaymentProcessing.WebAPI.Services
{
    /// <summary>
    /// WCF REST service implementation for payment processing API
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PaymentApiService : IPaymentApiService
    {
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IPaymentValidator _paymentValidator;

        public PaymentApiService()
        {
            _paymentValidator = new PaymentValidator();
            _paymentProcessor = new PaymentProcessor(_paymentValidator);
        }

        public ApiPaymentResponse ProcessPayment(ApiPaymentRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException("request", "Payment request cannot be null");
                }

                // Convert API request to core payment request
                var paymentRequest = request.ToPaymentRequest();

                // Process the payment
                var paymentResponse = _paymentProcessor.ProcessPayment(paymentRequest);

                // Convert core response to API response
                return ApiPaymentResponse.FromPaymentResponse(paymentResponse);
            }
            catch (Exception ex)
            {
                // Return error response
                return new ApiPaymentResponse
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    Status = "Error",
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    ResponseMessage = "Payment processing failed",
                    ProcessedDateTime = DateTime.Now
                };
            }
        }

        public ApiPaymentResponse GetTransactionStatus(string transactionId)
        {
            try
            {
                if (string.IsNullOrEmpty(transactionId))
                {
                    throw new ArgumentException("Transaction ID is required", "transactionId");
                }

                var paymentResponse = _paymentProcessor.GetTransactionStatus(transactionId);
                return ApiPaymentResponse.FromPaymentResponse(paymentResponse);
            }
            catch (Exception ex)
            {
                return new ApiPaymentResponse
                {
                    TransactionId = transactionId,
                    Status = "Error",
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    ResponseMessage = "Failed to retrieve transaction status",
                    ProcessedDateTime = DateTime.Now
                };
            }
        }

        public ApiPaymentResponse ProcessRefund(string transactionId, ApiRefundRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(transactionId))
                {
                    throw new ArgumentException("Transaction ID is required", "transactionId");
                }

                if (request == null)
                {
                    throw new ArgumentNullException("request", "Refund request cannot be null");
                }

                var paymentResponse = _paymentProcessor.RefundPayment(transactionId, request.Amount);
                return ApiPaymentResponse.FromPaymentResponse(paymentResponse);
            }
            catch (Exception ex)
            {
                return new ApiPaymentResponse
                {
                    TransactionId = transactionId,
                    Status = "Error",
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    ResponseMessage = "Refund processing failed",
                    ProcessedDateTime = DateTime.Now
                };
            }
        }

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

        public string GetVersion()
        {
            return "{ \"version\": \"1.0.0\", \"name\": \"Payment Processing API\", \"framework\": \".NET Framework 3.5\", \"build\": \"" + DateTime.Now.ToString("yyyyMMdd") + "\" }";
        }
    }
}
