<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Processing Web API</title>
    <style type="text/css">
        body { font-family: Arial, sans-serif; margin: 40px; background-color: #f5f5f5; }
        .container { max-width: 800px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }
        .header { background-color: #2c3e50; color: white; padding: 20px; margin: -30px -30px 30px -30px; border-radius: 5px 5px 0 0; }
        .endpoint { background-color: #f8f9fa; padding: 15px; margin: 10px 0; border-left: 4px solid #007bff; }
        .method { font-weight: bold; color: #007bff; }
        .url { font-family: monospace; background-color: #e9ecef; padding: 2px 5px; border-radius: 3px; }
        .example { background-color: #f8f9fa; padding: 10px; margin: 10px 0; border-radius: 3px; font-family: monospace; font-size: 12px; }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1>Payment Processing Web API</h1>
            <p>RESTful API for payment processing operations</p>
        </div>
        
        <h2>Available Endpoints</h2>
        
        <div class="endpoint">
            <h3><span class="method">POST</span> Process Payment</h3>
            <p><span class="url">/Services/PaymentApiService.svc/payments</span></p>
            <p>Process a new payment transaction</p>
            <div class="example">
{
  "Amount": 100.00,
  "Currency": "USD",
  "CardNumber": "4111111111111111",
  "CardHolderName": "John Doe",
  "ExpiryMonth": 12,
  "ExpiryYear": 2025,
  "CVV": "123",
  "MerchantId": "MERCHANT_001",
  "CustomerEmail": "customer@example.com"
}
            </div>
        </div>
        
        <div class="endpoint">
            <h3><span class="method">GET</span> Transaction Status</h3>
            <p><span class="url">/Services/PaymentApiService.svc/payments/{transactionId}/status</span></p>
            <p>Get the status of a specific transaction</p>
        </div>
        
        <div class="endpoint">
            <h3><span class="method">POST</span> Process Refund</h3>
            <p><span class="url">/Services/PaymentApiService.svc/payments/{transactionId}/refund</span></p>
            <p>Process a refund for an existing transaction</p>
            <div class="example">
{
  "Amount": 50.00,
  "Reason": "Customer request"
}
            </div>
        </div>
        
        <div class="endpoint">
            <h3><span class="method">GET</span> Health Check</h3>
            <p><span class="url">/Services/PaymentApiService.svc/health</span></p>
            <p>Check API health status</p>
        </div>
        
        <div class="endpoint">
            <h3><span class="method">GET</span> Version Info</h3>
            <p><span class="url">/Services/PaymentApiService.svc/version</span></p>
            <p>Get API version information</p>
        </div>
        
        <h2>Alternative ASMX Web Service</h2>
        <p>For legacy compatibility, ASMX web services are also available:</p>
        <ul>
            <li><a href="Controllers/PaymentApiController.asmx">PaymentApiController.asmx</a></li>
        </ul>
        
        <h2>Test Card Numbers</h2>
        <ul>
            <li><strong>4111111111111111</strong> - Approved transaction</li>
            <li><strong>4111111111111119</strong> - Declined transaction</li>
            <li><strong>4111111111111110</strong> - Processing error</li>
        </ul>
        
        <div style="margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee; color: #666; text-align: center;">
            <p>&copy; 2025 Payment Processing API. All rights reserved.</p>
        </div>
    </div>
</body>
</html>
