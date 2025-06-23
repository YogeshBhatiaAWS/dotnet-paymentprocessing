using System;
using System.Web.Mvc;
using PaymentProcessing.Core.Services;
using PaymentProcessing.Web.Models;

namespace PaymentProcessing.Web.Controllers
{
    /// <summary>
    /// Controller for payment processing operations
    /// </summary>
    public class PaymentController : Controller
    {
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IPaymentValidator _paymentValidator;

        public PaymentController()
        {
            _paymentValidator = new PaymentValidator();
            _paymentProcessor = new PaymentProcessor(_paymentValidator);
        }

        /// <summary>
        /// Display payment form
        /// </summary>
        public ActionResult Index()
        {
            ViewData["Title"] = "Process Payment";
            return View(new PaymentViewModel());
        }

        /// <summary>
        /// Process payment form submission
        /// </summary>
        [HttpPost]
        public ActionResult Process(PaymentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["Title"] = "Process Payment";
                    ViewData["Error"] = "Please correct the errors below.";
                    return View("Index", model);
                }

                // Convert view model to payment request
                var paymentRequest = model.ToPaymentRequest();

                // Process the payment
                var paymentResponse = _paymentProcessor.ProcessPayment(paymentRequest);

                // Convert to result view model
                var resultModel = PaymentResultViewModel.FromPaymentResponse(paymentResponse);

                // Store result in TempData for redirect
                TempData["PaymentResult"] = resultModel;

                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                ViewData["Title"] = "Process Payment";
                ViewData["Error"] = "An error occurred while processing your payment: " + ex.Message;
                return View("Index", model);
            }
        }

        /// <summary>
        /// Display payment result
        /// </summary>
        public ActionResult Result()
        {
            var result = TempData["PaymentResult"] as PaymentResultViewModel;
            
            if (result == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["Title"] = "Payment Result";
            return View(result);
        }

        /// <summary>
        /// Check transaction status
        /// </summary>
        public ActionResult Status(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                ViewData["Error"] = "Transaction ID is required";
                return View("Index", new PaymentViewModel());
            }

            try
            {
                var response = _paymentProcessor.GetTransactionStatus(transactionId);
                var resultModel = PaymentResultViewModel.FromPaymentResponse(response);
                
                ViewData["Title"] = "Transaction Status";
                return View("Result", resultModel);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error retrieving transaction status: " + ex.Message;
                return View("Index", new PaymentViewModel());
            }
        }

        /// <summary>
        /// Process refund
        /// </summary>
        [HttpPost]
        public ActionResult Refund(string transactionId, decimal amount)
        {
            try
            {
                var response = _paymentProcessor.RefundPayment(transactionId, amount);
                var resultModel = PaymentResultViewModel.FromPaymentResponse(response);
                
                TempData["PaymentResult"] = resultModel;
                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error processing refund: " + ex.Message;
                return View("Index", new PaymentViewModel());
            }
        }
    }
}
