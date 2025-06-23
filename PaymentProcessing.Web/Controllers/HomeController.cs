using System.Web.Mvc;

namespace PaymentProcessing.Web.Controllers
{
    /// <summary>
    /// Home controller for the payment processing web application
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Default home page
        /// </summary>
        public ActionResult Index()
        {
            ViewData["Title"] = "Payment Processing System";
            ViewData["Message"] = "Welcome to the Payment Processing System";
            return View();
        }

        /// <summary>
        /// About page
        /// </summary>
        public ActionResult About()
        {
            ViewData["Title"] = "About";
            ViewData["Message"] = "Payment Processing System v1.0";
            return View();
        }
    }
}
