using System;
using System.Web.Mvc;
using PaymentProcessing.Core.Models;
using PaymentProcessing.Core.Services;
using PaymentProcessing.Web.Models;

namespace PaymentProcessing.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IPaymentValidator _paymentValidator;

        public PaymentController()
        {
            _paymentProcessor = new PaymentProcessor();
            _paymentValidator = new PaymentValidator();
        }

        // GET: Payment
        public ActionResult Index()
        {
            var model = new PaymentViewModel
            {
                Currency = "USD",
                ExpiryMonth = DateTime.Now.Month,
                ExpiryYear = DateTime.Now.Year
            };
            return View(model);
        }

        // POST: Payment/Process
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Process(PaymentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }

                // Convert view model to payment request
                var paymentRequest = new PaymentRequest
                {
                    Amount = model.Amount,
                    Currency = model.Currency ?? "USD",
                    CardNumber = model.CardNumber?.Replace(" ", ""),
                    CardHolderName = model.CardHolderName,
                    ExpiryMonth = model.ExpiryMonth,
                    ExpiryYear = model.ExpiryYear,
                    CVV = model.CVV,
                    MerchantId = "MERCHANT_001",
                    CustomerEmail = model.CustomerEmail,
                    Description = model.Description
                };

                // Validate the payment request
                var validationResult = _paymentValidator.ValidatePayment(paymentRequest);
                if (!validationResult.IsValid)
                {
                    ModelState.AddModelError("", validationResult.ErrorMessage);
                    return View("Index", model);
                }

                // Process the payment
                var response = _paymentProcessor.ProcessPayment(paymentRequest);

                // Return result view
                return View("Result", response);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your payment: " + ex.Message);
                return View("Index", model);
            }
        }

        // GET: Payment/Status/{transactionId}
        public ActionResult Status(string transactionId)
        {
            try
            {
                if (string.IsNullOrEmpty(transactionId))
                {
                    return RedirectToAction("Index");
                }

                var status = _paymentProcessor.GetTransactionStatus(transactionId);
                
                var response = new PaymentResponse
                {
                    TransactionId = transactionId,
                    Status = status,
                    Message = $"Transaction status: {status}",
                    ProcessedAt = DateTime.UtcNow
                };

                return View("Result", response);
            }
            catch (Exception ex)
            {
                var errorResponse = new PaymentResponse
                {
                    TransactionId = transactionId,
                    Status = PaymentStatus.Error,
                    Message = "Error retrieving transaction status: " + ex.Message,
                    ProcessedAt = DateTime.UtcNow
                };

                return View("Result", errorResponse);
            }
        }
    }
}
