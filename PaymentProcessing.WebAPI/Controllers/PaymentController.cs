using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PaymentProcessing.Core.Models;
using PaymentProcessing.Core.Services;
using PaymentProcessing.WebAPI.Models;

namespace PaymentProcessing.WebAPI.Controllers
{
    [RoutePrefix("api/payments")]
    public class PaymentController : ApiController
    {
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IPaymentValidator _paymentValidator;

        public PaymentController()
        {
            _paymentProcessor = new PaymentProcessor();
            _paymentValidator = new PaymentValidator();
        }

        /// <summary>
        /// Process a payment transaction
        /// </summary>
        /// <param name="request">Payment request details</param>
        /// <returns>Payment response with transaction details</returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(ApiPaymentResponse))]
        public async Task<IHttpActionResult> ProcessPayment([FromBody] ApiPaymentRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Payment request is required");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Convert API request to core request
                var paymentRequest = new PaymentRequest
                {
                    Amount = request.Amount,
                    Currency = request.Currency ?? "USD",
                    CardNumber = request.CardNumber,
                    CardHolderName = request.CardHolderName,
                    ExpiryMonth = request.ExpiryMonth,
                    ExpiryYear = request.ExpiryYear,
                    CVV = request.CVV,
                    MerchantId = request.MerchantId ?? "MERCHANT_001",
                    CustomerEmail = request.CustomerEmail,
                    Description = request.Description
                };

                // Validate the payment request
                var validationResult = _paymentValidator.ValidatePayment(paymentRequest);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ErrorMessage);
                }

                // Process the payment
                var response = await Task.FromResult(_paymentProcessor.ProcessPayment(paymentRequest));

                // Convert core response to API response
                var apiResponse = new ApiPaymentResponse
                {
                    TransactionId = response.TransactionId,
                    Status = response.Status.ToString(),
                    Amount = response.Amount,
                    Currency = response.Currency,
                    Message = response.Message,
                    ErrorCode = response.ErrorCode,
                    ProcessedAt = response.ProcessedAt,
                    AuthorizationCode = response.AuthorizationCode,
                    ReferenceNumber = response.ReferenceNumber
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get payment transaction status
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>Transaction status information</returns>
        [HttpGet]
        [Route("{transactionId}/status")]
        [ResponseType(typeof(ApiPaymentResponse))]
        public async Task<IHttpActionResult> GetTransactionStatus(string transactionId)
        {
            try
            {
                if (string.IsNullOrEmpty(transactionId))
                {
                    return BadRequest("Transaction ID is required");
                }

                var status = await Task.FromResult(_paymentProcessor.GetTransactionStatus(transactionId));

                var response = new ApiPaymentResponse
                {
                    TransactionId = transactionId,
                    Status = status.ToString(),
                    Message = $"Transaction status: {status}"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Process a refund for a transaction
        /// </summary>
        /// <param name="transactionId">Original transaction ID</param>
        /// <param name="refundRequest">Refund details</param>
        /// <returns>Refund response</returns>
        [HttpPost]
        [Route("{transactionId}/refund")]
        [ResponseType(typeof(ApiPaymentResponse))]
        public async Task<IHttpActionResult> ProcessRefund(string transactionId, [FromBody] RefundRequest refundRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(transactionId))
                {
                    return BadRequest("Transaction ID is required");
                }

                if (refundRequest == null || refundRequest.Amount <= 0)
                {
                    return BadRequest("Valid refund amount is required");
                }

                var response = await Task.FromResult(_paymentProcessor.ProcessRefund(transactionId, refundRequest.Amount, refundRequest.Reason));

                var apiResponse = new ApiPaymentResponse
                {
                    TransactionId = response.TransactionId,
                    Status = response.Status.ToString(),
                    Amount = response.Amount,
                    Currency = response.Currency,
                    Message = response.Message,
                    ProcessedAt = response.ProcessedAt
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

    public class RefundRequest
    {
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
