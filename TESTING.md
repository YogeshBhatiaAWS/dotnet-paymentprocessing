# Testing Guide - Payment Processing System

## Prerequisites for Testing

### System Requirements
- Windows with .NET Framework 3.5 installed
- Visual Studio 2008+ or Visual Studio Community
- IIS Express or IIS (for web applications)
- Command prompt access

### Build the Solution First
```batch
# Option 1: Use the build script
build.bat

# Option 2: Use Visual Studio
# Open PaymentProcessing.sln and press Ctrl+Shift+B

# Option 3: Use MSBuild command line
%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe PaymentProcessing.sln /p:Configuration=Debug
```

## 1. Testing the Console Application

### Interactive Mode Testing

1. **Navigate to Console Application**:
```batch
cd PaymentProcessing.Console\bin\Debug
PaymentProcessing.Console.exe
```

2. **Test Menu Options**:
   - **Option 1**: Process Single Payment
   - **Option 2**: Batch Process Payments
   - **Option 3**: Check Transaction Status
   - **Option 4**: Process Refund
   - **Option 5**: Generate Report
   - **Option 6**: Test Payment Scenarios

### Command Line Testing

```batch
# Test batch processing with sample file
PaymentProcessing.Console.exe /batch Data\sample_payments.csv

# Test transaction status check (use a transaction ID from previous test)
PaymentProcessing.Console.exe /status 12345678-1234-1234-1234-123456789012

# Generate a report
PaymentProcessing.Console.exe /report test_report.txt

# Show help
PaymentProcessing.Console.exe /help
```

### Expected Results for Console Testing

**Single Payment Test**:
```
Enter amount: $100.00
Enter currency (USD): USD
Enter card number: 4111111111111111
Enter card holder name: John Doe
Enter expiry month (1-12): 12
Enter expiry year: 2025
Enter CVV: 123
Enter customer email: john.doe@example.com

Expected Result: ✓ Payment Approved
```

**Batch Processing Test**:
```
Processing batch file: Data\sample_payments.csv
Processing record 1...
✓ Success: [TransactionId] - Payment approved successfully
Processing record 2...
✗ Failed: [TransactionId] - Payment declined by issuer
...
=== Batch Processing Summary ===
Total Records: 10
Successful: 8
Failed: 2
Success Rate: 80.0%
```

## 2. Testing the MVC Web Application

### Setup and Launch

1. **Using Visual Studio**:
   - Set `PaymentProcessing.Web` as startup project
   - Press F5 to run
   - Browser should open to the home page

2. **Using IIS Express** (if available):
   - Configure IIS Express to point to PaymentProcessing.Web folder
   - Access via http://localhost:[port]

### Web Application Test Cases

#### Test Case 1: Home Page
- **URL**: `http://localhost:[port]/`
- **Expected**: Welcome page with navigation links
- **Verify**: "Process Payment" link works

#### Test Case 2: Payment Form Validation
- **URL**: `http://localhost:[port]/Payment`
- **Test**: Submit empty form
- **Expected**: Validation errors displayed

#### Test Case 3: Successful Payment
- **URL**: `http://localhost:[port]/Payment`
- **Test Data**:
  ```
  Amount: 100.00
  Currency: USD
  Card Number: 4111111111111111
  Card Holder: John Doe
  Expiry Month: 12
  Expiry Year: 2025
  CVV: 123
  Email: john.doe@example.com
  ```
- **Expected**: Success page with transaction details

#### Test Case 4: Declined Payment
- **Test Data**: Same as above but use card number `4111111111111119`
- **Expected**: Declined status with error message

#### Test Case 5: Processing Error
- **Test Data**: Same as above but use card number `4111111111111110`
- **Expected**: Error status with error details

#### Test Case 6: Transaction Status Check
- **URL**: `http://localhost:[port]/Payment/Status?transactionId=[ID]`
- **Use**: Transaction ID from previous successful payment
- **Expected**: Transaction details displayed

## 3. Testing the Web API

### Setup and Launch

1. **Using Visual Studio**:
   - Set `PaymentProcessing.WebAPI` as startup project
   - Press F5 to run
   - Note the port number (usually http://localhost:[port])

### API Testing Methods

#### Method 1: Using Browser (GET requests only)

```
# Health Check
http://localhost:[port]/Services/PaymentApiService.svc/health

# Version Info
http://localhost:[port]/Services/PaymentApiService.svc/version

# API Documentation
http://localhost:[port]/Default.aspx
```

#### Method 2: Using cURL (Command Line)

```bash
# Health Check
curl -X GET "http://localhost:[port]/Services/PaymentApiService.svc/health"

# Process Payment
curl -X POST "http://localhost:[port]/Services/PaymentApiService.svc/payments" \
  -H "Content-Type: application/json" \
  -d '{
    "Amount": 100.00,
    "Currency": "USD",
    "CardNumber": "4111111111111111",
    "CardHolderName": "John Doe",
    "ExpiryMonth": 12,
    "ExpiryYear": 2025,
    "CVV": "123",
    "MerchantId": "MERCHANT_001",
    "CustomerEmail": "john.doe@example.com"
  }'

# Check Transaction Status (replace {transactionId} with actual ID)
curl -X GET "http://localhost:[port]/Services/PaymentApiService.svc/payments/{transactionId}/status"

# Process Refund (replace {transactionId} with actual ID)
curl -X POST "http://localhost:[port]/Services/PaymentApiService.svc/payments/{transactionId}/refund" \
  -H "Content-Type: application/json" \
  -d '{
    "Amount": 50.00,
    "Reason": "Customer request"
  }'
```

#### Method 3: Using Postman

1. **Import Collection**: Create requests for each endpoint
2. **Set Base URL**: `http://localhost:[port]/Services/PaymentApiService.svc`
3. **Test Endpoints**:

**POST /payments**:
```json
{
  "Amount": 100.00,
  "Currency": "USD",
  "CardNumber": "4111111111111111",
  "CardHolderName": "John Doe",
  "ExpiryMonth": 12,
  "ExpiryYear": 2025,
  "CVV": "123",
  "MerchantId": "MERCHANT_001",
  "CustomerEmail": "john.doe@example.com"
}
```

**GET /payments/{transactionId}/status**:
- Replace `{transactionId}` with actual transaction ID

**POST /payments/{transactionId}/refund**:
```json
{
  "Amount": 50.00,
  "Reason": "Customer request"
}
```

### Expected API Responses

#### Successful Payment Response:
```json
{
  "TransactionId": "12345678-1234-1234-1234-123456789012",
  "Status": "Approved",
  "AuthorizationCode": "AUTH12345678",
  "ResponseMessage": "Payment approved successfully",
  "ProcessedAmount": 100.00,
  "ProcessedDateTime": "2025-06-23T06:00:00Z",
  "ProcessorTransactionId": "TXN_abc123def456",
  "IsSuccessful": true,
  "ErrorCode": null,
  "ErrorMessage": null
}
```

#### Declined Payment Response:
```json
{
  "TransactionId": "12345678-1234-1234-1234-123456789012",
  "Status": "Declined",
  "AuthorizationCode": null,
  "ResponseMessage": "Payment declined by issuer",
  "ProcessedAmount": 100.00,
  "ProcessedDateTime": "2025-06-23T06:00:00Z",
  "ProcessorTransactionId": null,
  "IsSuccessful": false,
  "ErrorCode": "DECLINED",
  "ErrorMessage": "Insufficient funds"
}
```

## 4. Integration Testing

### End-to-End Test Scenarios

#### Scenario 1: Complete Payment Flow
1. **Web App**: Process payment via web interface
2. **API**: Check transaction status via API
3. **Console**: Generate report including the transaction

#### Scenario 2: Batch Processing Flow
1. **Console**: Process batch file
2. **API**: Check status of batch transactions
3. **Web App**: Process refunds for successful transactions

#### Scenario 3: Error Handling
1. Test invalid card numbers
2. Test expired cards
3. Test network timeouts (simulated)
4. Test invalid API requests

## 5. Performance Testing

### Load Testing (Basic)

```bash
# Test multiple concurrent payments (requires additional tools)
# Use Apache Bench (ab) if available:
ab -n 100 -c 10 -p payment_data.json -T application/json http://localhost:[port]/Services/PaymentApiService.svc/payments
```

### Batch Processing Performance
```bash
# Create large CSV file and test processing time
PaymentProcessing.Console.exe /batch large_payments.csv
```

## 6. Test Data

### Valid Test Card Numbers (Luhn Algorithm Compliant)
- **4111111111111111** - Always approved
- **4111111111111119** - Always declined
- **4111111111111110** - Always error
- **4000000000000002** - Approved (alternative)
- **5555555555554444** - Approved (MasterCard format)

### Invalid Test Data
- **1234567890123456** - Invalid Luhn
- **411111111111111** - Too short
- **41111111111111111** - Too long

### Sample CSV Data
The system includes `sample_payments.csv` with 10 test records covering various scenarios.

## 7. Troubleshooting Common Issues

### Build Issues
```
Error: Could not load file or assembly
Solution: Ensure .NET Framework 3.5 is installed
```

### Web Application Issues
```
Error: HTTP 404 - Page not found
Solution: Check IIS Express is running and port is correct
```

### API Issues
```
Error: WCF Service not found
Solution: Ensure WCF HTTP Activation is enabled
```

### Console Application Issues
```
Error: File not found
Solution: Ensure you're in the correct directory with sample_payments.csv
```

## 8. Automated Testing Script

Create a batch file for automated testing:

```batch
@echo off
echo Running Payment Processing System Tests
echo =====================================

echo.
echo 1. Testing Console Application...
cd PaymentProcessing.Console\bin\Debug
PaymentProcessing.Console.exe /batch Data\sample_payments.csv
PaymentProcessing.Console.exe /report automated_test_report.txt

echo.
echo 2. Console tests completed. Check output above.
echo.
echo 3. Manual testing required for Web App and API:
echo    - Set PaymentProcessing.Web as startup project and run
echo    - Set PaymentProcessing.WebAPI as startup project and run
echo    - Use provided test data and endpoints
echo.
pause
```

## 9. Test Results Documentation

### Expected Success Rates
- **Console Application**: 80% success rate with sample data
- **Web Application**: 100% functionality for valid inputs
- **Web API**: All endpoints should respond correctly
- **Batch Processing**: Should handle 10 records in under 5 seconds

### Performance Benchmarks
- **Single Payment**: < 100ms response time
- **Batch Processing**: < 1 second per 10 transactions
- **API Response**: < 200ms for payment processing
- **Report Generation**: < 5 seconds for standard report

Remember to test with both valid and invalid data to ensure proper error handling throughout the system!
