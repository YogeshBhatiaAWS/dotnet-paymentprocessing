using System;
using System.Collections.Generic;
using PaymentProcessing.Core.Models;

namespace PaymentProcessing.Core.Services
{
    /// <summary>
    /// Implementation of payment processing services
    /// </summary>
    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly IPaymentValidator _validator;
        private static readonly Dictionary<string, PaymentResponse> _transactionHistory = new Dictionary<string, PaymentResponse>();

        public PaymentProcessor(IPaymentValidator validator)
        {
            _validator = validator ?? throw new ArgumentNullException("validator");
        }

        public PaymentResponse ProcessPayment(PaymentRequest request)
        {
            var response = new PaymentResponse(request.TransactionId);

            try
            {
                // Validate the payment request
                if (!_validator.ValidatePaymentRequest(request))
                {
                    response.Status = PaymentStatus.Error;
                    response.IsSuccessful = false;
                    response.ErrorMessage = _validator.GetValidationError();
                    response.ResponseMessage = "Validation failed: " + response.ErrorMessage;
                    return response;
                }

                // Simulate payment processing
                response = SimulatePaymentProcessing(request);
                
                // Store transaction in history
                _transactionHistory[request.TransactionId] = response;

                return response;
            }
            catch (Exception ex)
            {
                response.Status = PaymentStatus.Error;
                response.IsSuccessful = false;
                response.ErrorMessage = ex.Message;
                response.ResponseMessage = "Payment processing failed";
                return response;
            }
        }

        public PaymentResponse RefundPayment(string transactionId, decimal amount)
        {
            var response = new PaymentResponse(Guid.NewGuid().ToString());

            try
            {
                if (!_transactionHistory.ContainsKey(transactionId))
                {
                    response.Status = PaymentStatus.Error;
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Transaction not found";
                    response.ResponseMessage = "Cannot refund: Transaction not found";
                    return response;
                }

                var originalTransaction = _transactionHistory[transactionId];
                
                if (originalTransaction.Status != PaymentStatus.Approved)
                {
                    response.Status = PaymentStatus.Error;
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Cannot refund non-approved transaction";
                    response.ResponseMessage = "Refund failed: Original transaction not approved";
                    return response;
                }

                if (amount > originalTransaction.ProcessedAmount)
                {
                    response.Status = PaymentStatus.Error;
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Refund amount exceeds original transaction amount";
                    response.ResponseMessage = "Refund failed: Amount exceeds original transaction";
                    return response;
                }

                // Simulate refund processing
                response.Status = PaymentStatus.Refunded;
                response.IsSuccessful = true;
                response.ProcessedAmount = amount;
                response.AuthorizationCode = "REF" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                response.ProcessorTransactionId = "REFUND_" + Guid.NewGuid().ToString("N").Substring(0, 12);
                response.ResponseMessage = "Refund processed successfully";

                // Update original transaction status
                originalTransaction.Status = PaymentStatus.Refunded;

                return response;
            }
            catch (Exception ex)
            {
                response.Status = PaymentStatus.Error;
                response.IsSuccessful = false;
                response.ErrorMessage = ex.Message;
                response.ResponseMessage = "Refund processing failed";
                return response;
            }
        }

        public PaymentResponse GetTransactionStatus(string transactionId)
        {
            if (_transactionHistory.ContainsKey(transactionId))
            {
                return _transactionHistory[transactionId];
            }

            var response = new PaymentResponse(transactionId);
            response.Status = PaymentStatus.Error;
            response.IsSuccessful = false;
            response.ErrorMessage = "Transaction not found";
            response.ResponseMessage = "Transaction not found in system";
            return response;
        }

        /// <summary>
        /// Simulates payment processing with different scenarios
        /// </summary>
        private PaymentResponse SimulatePaymentProcessing(PaymentRequest request)
        {
            var response = new PaymentResponse(request.TransactionId);
            response.ProcessedAmount = request.Amount;

            // Simulate different payment scenarios based on card number
            string lastDigit = request.CreditCard.CardNumber.Substring(request.CreditCard.CardNumber.Length - 1);
            
            switch (lastDigit)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                    // Approved transactions (80% success rate)
                    response.Status = PaymentStatus.Approved;
                    response.IsSuccessful = true;
                    response.AuthorizationCode = "AUTH" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                    response.ProcessorTransactionId = "TXN_" + Guid.NewGuid().ToString("N").Substring(0, 12);
                    response.ResponseMessage = "Payment approved successfully";
                    break;
                
                case "9":
                    // Declined transaction
                    response.Status = PaymentStatus.Declined;
                    response.IsSuccessful = false;
                    response.ErrorCode = "DECLINED";
                    response.ErrorMessage = "Insufficient funds";
                    response.ResponseMessage = "Payment declined by issuer";
                    break;
                
                case "0":
                    // Error scenario
                    response.Status = PaymentStatus.Error;
                    response.IsSuccessful = false;
                    response.ErrorCode = "PROCESSING_ERROR";
                    response.ErrorMessage = "Unable to process payment at this time";
                    response.ResponseMessage = "Payment processing error";
                    break;
            }

            return response;
        }
    }
}
