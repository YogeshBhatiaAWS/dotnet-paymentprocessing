using PaymentProcessing.Core.Models;

namespace PaymentProcessing.Core.Services
{
    /// <summary>
    /// Interface for payment processing services
    /// </summary>
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Processes a payment request
        /// </summary>
        /// <param name="request">The payment request to process</param>
        /// <returns>Payment response with transaction details</returns>
        PaymentResponse ProcessPayment(PaymentRequest request);

        /// <summary>
        /// Refunds a processed payment
        /// </summary>
        /// <param name="transactionId">The transaction ID to refund</param>
        /// <param name="amount">The amount to refund</param>
        /// <returns>Payment response for the refund</returns>
        PaymentResponse RefundPayment(string transactionId, decimal amount);

        /// <summary>
        /// Gets the status of a transaction
        /// </summary>
        /// <param name="transactionId">The transaction ID to check</param>
        /// <returns>Payment response with current status</returns>
        PaymentResponse GetTransactionStatus(string transactionId);
    }
}
