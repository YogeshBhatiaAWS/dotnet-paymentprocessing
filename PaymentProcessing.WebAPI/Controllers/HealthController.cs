using System;
using System.Web.Http;

namespace PaymentProcessing.WebAPI.Controllers
{
    [RoutePrefix("api/health")]
    public class HealthController : ApiController
    {
        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns>API health status</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetHealth()
        {
            var response = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Service = "Payment Processing API"
            };

            return Ok(response);
        }

        /// <summary>
        /// Get API version information
        /// </summary>
        /// <returns>Version details</returns>
        [HttpGet]
        [Route("version")]
        public IHttpActionResult GetVersion()
        {
            var response = new
            {
                Version = "1.0.0",
                BuildDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Framework = ".NET Framework 4.7.2",
                ApiVersion = "v1"
            };

            return Ok(response);
        }
    }
}
