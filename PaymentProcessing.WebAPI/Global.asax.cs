using System;
using System.Web;

namespace PaymentProcessing.WebAPI
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Application startup logic
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Session start logic
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Handle CORS for API requests
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");
            
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Authentication logic
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Global error handling
            Exception ex = Server.GetLastError();
            
            // Log the error (in a real application, use proper logging)
            System.Diagnostics.Debug.WriteLine("Application Error: " + ex.Message);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Session end logic
        }

        protected void Application_End(object sender, EventArgs e)
        {
            // Application end logic
        }
    }
}
