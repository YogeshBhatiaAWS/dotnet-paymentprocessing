# Payment Processing System - Test Checklist

## 🚀 Quick Start Testing

### Step 1: Build the Solution
```batch
# Windows Command Prompt
cd /path/to/dotnet-paymentprocessing
build.bat
```

### Step 2: Quick Core Test
```batch
# Test core functionality
test_core.bat
```

### Step 3: Manual Testing Checklist

## ✅ Console Application Testing

### Basic Functionality
- [ ] Application starts without errors
- [ ] Main menu displays correctly
- [ ] Help command works (`/help`)

### Payment Processing
- [ ] **Single Payment Test**
  - Amount: $100.00
  - Card: 4111111111111111
  - Expected: ✅ Approved
  
- [ ] **Declined Payment Test**
  - Amount: $50.00
  - Card: 4111111111111119
  - Expected: ❌ Declined

- [ ] **Error Payment Test**
  - Amount: $75.00
  - Card: 4111111111111110
  - Expected: ⚠️ Error

### Batch Processing
- [ ] **Sample CSV Processing**
  ```batch
  PaymentProcessing.Console.exe /batch Data\sample_payments.csv
  ```
  - Expected: ~80% success rate (8/10 transactions)
  - Expected: Batch report generated

### Transaction Management
- [ ] **Status Check**
  ```batch
  PaymentProcessing.Console.exe /status [TransactionId]
  ```
  - Use transaction ID from successful payment
  - Expected: Transaction details displayed

### Reporting
- [ ] **Report Generation**
  ```batch
  PaymentProcessing.Console.exe /report test_report.txt
  ```
  - Expected: Report file created
  - Expected: Contains system metrics

## ✅ MVC Web Application Testing

### Setup
- [ ] Set `PaymentProcessing.Web` as startup project
- [ ] Press F5 to run
- [ ] Browser opens to home page

### Navigation
- [ ] **Home Page** (`/`)
  - Welcome message displays
  - Navigation links work
  - "Process Payment" button works

- [ ] **Payment Page** (`/Payment`)
  - Form displays correctly
  - All fields present
  - Validation messages work

### Payment Form Testing
- [ ] **Valid Payment Submission**
  ```
  Amount: 100.00
  Currency: USD
  Card Number: 4111111111111111
  Card Holder: John Doe
  Expiry: 12/2025
  CVV: 123
  Email: john.doe@example.com
  ```
  - Expected: Success page with transaction details
  - Expected: Authorization code displayed

- [ ] **Form Validation**
  - Submit empty form → Validation errors
  - Invalid email → Email validation error
  - Invalid amount → Amount validation error

- [ ] **Declined Payment**
  - Same data but card: 4111111111111119
  - Expected: Declined status page

- [ ] **Error Scenario**
  - Same data but card: 4111111111111110
  - Expected: Error status page

### Transaction Management
- [ ] **Status Check**
  - Use transaction ID from successful payment
  - URL: `/Payment/Status?transactionId=[ID]`
  - Expected: Transaction details displayed

- [ ] **Refund Processing**
  - From successful payment result page
  - Enter refund amount (less than original)
  - Expected: Refund processed successfully

## ✅ Web API Testing

### Setup
- [ ] Set `PaymentProcessing.WebAPI` as startup project
- [ ] Press F5 to run
- [ ] Note the port number

### Basic Endpoints (Browser Testing)
- [ ] **API Documentation**
  - URL: `http://localhost:[port]/Default.aspx`
  - Expected: API documentation page

- [ ] **Health Check**
  - URL: `http://localhost:[port]/Services/PaymentApiService.svc/health`
  - Expected: JSON health status

- [ ] **Version Info**
  - URL: `http://localhost:[port]/Services/PaymentApiService.svc/version`
  - Expected: JSON version information

### API Endpoint Testing (Use Postman/cURL)

#### Process Payment
- [ ] **POST** `/Services/PaymentApiService.svc/payments`
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
  - Expected: 200 OK with transaction details
  - Save transaction ID for next tests

#### Transaction Status
- [ ] **GET** `/Services/PaymentApiService.svc/payments/{transactionId}/status`
  - Use transaction ID from previous test
  - Expected: Transaction status details

#### Refund Processing
- [ ] **POST** `/Services/PaymentApiService.svc/payments/{transactionId}/refund`
  ```json
  {
    "Amount": 50.00,
    "Reason": "Customer request"
  }
  ```
  - Expected: Refund confirmation

### Error Handling
- [ ] **Invalid JSON**
  - Send malformed JSON
  - Expected: Error response

- [ ] **Missing Fields**
  - Send incomplete payment request
  - Expected: Validation error

- [ ] **Invalid Transaction ID**
  - Check status of non-existent transaction
  - Expected: Not found error

## ✅ Integration Testing

### Cross-Component Testing
- [ ] **Web → API → Console Flow**
  1. Process payment via Web App
  2. Check status via API
  3. Generate report via Console
  4. Verify transaction appears in report

- [ ] **Batch → API → Web Flow**
  1. Process batch via Console
  2. Check individual transaction status via API
  3. Process refund via Web App

### Data Consistency
- [ ] **Transaction IDs**
  - Same transaction ID across all components
  - Status consistency between components

- [ ] **Amount Handling**
  - Decimal precision maintained
  - Currency handling consistent

## ✅ Error Handling Testing

### Invalid Data Testing
- [ ] **Invalid Card Numbers**
  - Test: 1234567890123456 (bad Luhn)
  - Expected: Validation error

- [ ] **Expired Cards**
  - Test: Expiry year < current year
  - Expected: Expired card error

- [ ] **Missing Required Fields**
  - Test: Empty card holder name
  - Expected: Required field error

### System Error Testing
- [ ] **File Not Found**
  - Console: Process non-existent CSV file
  - Expected: File not found error

- [ ] **Invalid CSV Format**
  - Console: Process malformed CSV
  - Expected: Format error with line number

## ✅ Performance Testing

### Response Time Testing
- [ ] **Single Payment Processing**
  - Expected: < 1 second response time
  - Test with multiple card numbers

- [ ] **Batch Processing**
  - Test with sample_payments.csv (10 records)
  - Expected: < 5 seconds total processing time

- [ ] **API Response Times**
  - Test all API endpoints
  - Expected: < 500ms response time

### Load Testing (Basic)
- [ ] **Multiple Concurrent Payments**
  - Process 10 payments simultaneously
  - Expected: All process successfully

- [ ] **Large Batch File**
  - Create CSV with 100+ records
  - Expected: Processes without memory issues

## 🔧 Troubleshooting Common Issues

### Build Issues
```
Issue: "Could not load file or assembly"
Solution: Ensure .NET Framework 3.5 is installed
Command: build.bat
```

### Runtime Issues
```
Issue: "File not found" in Console app
Solution: Ensure you're in the correct directory
Check: PaymentProcessing.Console\bin\Debug\
```

### Web Application Issues
```
Issue: "HTTP 404 - Not Found"
Solution: Check IIS Express is running
Check: Project is set as startup project
```

### API Issues
```
Issue: "Service not available"
Solution: Check WCF services are properly configured
Check: Web.config has correct service configuration
```

## 📊 Expected Test Results

### Success Rates
- **Console Single Payments**: 100% for valid data
- **Batch Processing**: ~80% (based on sample data)
- **Web Application**: 100% for valid inputs
- **API Endpoints**: 100% for valid requests

### Performance Benchmarks
- **Payment Processing**: < 100ms
- **Batch Processing**: < 500ms per transaction
- **API Response**: < 200ms
- **Report Generation**: < 2 seconds

## 📝 Test Documentation

After completing tests, document:
- [ ] Test execution date/time
- [ ] Environment details (OS, .NET version)
- [ ] Any failures and resolutions
- [ ] Performance measurements
- [ ] Recommendations for improvements

---

## 🎯 Quick Test Commands Summary

```batch
# Build solution
build.bat

# Test console core functionality
test_core.bat

# Manual console testing
cd PaymentProcessing.Console\bin\Debug
PaymentProcessing.Console.exe

# Batch processing test
PaymentProcessing.Console.exe /batch Data\sample_payments.csv

# Generate report
PaymentProcessing.Console.exe /report test_report.txt

# For Web App and API: Use Visual Studio F5 with respective startup projects
```

Remember: This is a legacy .NET Framework 3.5 application, so ensure you have the appropriate development environment set up!
