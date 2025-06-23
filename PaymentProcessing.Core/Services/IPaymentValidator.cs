using PaymentProcessing.Core.Models;

namespace PaymentProcessing.Core.Services
{
    /// <summary>
    /// Interface for payment validation services
    /// </summary>
    public interface IPaymentValidator
    {
        /// <summary>
        /// Validates a payment request
        /// </summary>
        /// <param name="request">The payment request to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        bool ValidatePaymentRequest(PaymentRequest request);

        /// <summary>
        /// Validates credit card information
        /// </summary>
        /// <param name="creditCard">The credit card to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        bool ValidateCreditCard(CreditCard creditCard);

        /// <summary>
        /// Gets validation error message
        /// </summary>
        string GetValidationError();
    }
}
