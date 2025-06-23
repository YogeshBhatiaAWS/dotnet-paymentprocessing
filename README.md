# Payment Processing System (.NET Framework 3.5)

A comprehensive legacy .NET Framework 3.5 payment processing application demonstrating MVC web application, Web API services, and console application components.

## Overview

This solution provides a complete payment processing system built with legacy .NET Framework 3.5 technologies, including:

- **Core Library**: Payment processing business logic and models
- **MVC Web Application**: User interface for payment processing
- **Web API**: RESTful services using WCF and ASMX
- **Console Application**: Batch processing and administrative tools

## Architecture

### Projects Structure

```
PaymentProcessing.sln
├── PaymentProcessing.Core/          # Core business logic library
├── PaymentProcessing.Web/           # MVC Web Application
├── PaymentProcessing.WebAPI/        # Web API Services
└── PaymentProcessing.Console/       # Console Application
```

### Key Features

- **Payment Processing**: Credit card transaction processing with validation
- **Multiple Payment Scenarios**: Approved, declined, and error simulations
- **Batch Processing**: CSV file processing for bulk transactions
- **Transaction Management**: Status checking and refund processing
- **Reporting**: Comprehensive payment reports and analytics
- **Web Interface**: User-friendly MVC application
- **API Services**: Both WCF REST and ASMX web services
- **Console Tools**: Command-line interface for administrative tasks

## Technology Stack

- **.NET Framework 3.5**
- **ASP.NET MVC 1.0**
- **WCF (Windows Communication Foundation)**
- **ASMX Web Services**
- **C# 3.0**
- **LINQ to Objects**

## Getting Started

### Prerequisites

- Visual Studio 2008 or later
- .NET Framework 3.5
- IIS (for web applications)

### Building the Solution

1. Open `PaymentProcessing.sln` in Visual Studio
2. Build the entire solution (Build → Build Solution)
3. Set startup projects as needed

### Running the Applications

#### MVC Web Application
1. Set `PaymentProcessing.Web` as startup project
2. Press F5 to run
3. Navigate to the payment processing interface

#### Web API
1. Set `PaymentProcessing.WebAPI` as startup project
2. Press F5 to run
3. Access API endpoints at `/Services/PaymentApiService.svc/`

#### Console Application
1. Set `PaymentProcessing.Console` as startup project
2. Run with or without command-line arguments
3. Use interactive menu or batch processing commands

## Usage Examples

### Test Card Numbers

Use these test card numbers to simulate different payment scenarios:

- **4111111111111111** - Approved transaction
- **4111111111111119** - Declined transaction  
- **4111111111111110** - Processing error

### Web API Endpoints

#### Process Payment
```http
POST /Services/PaymentApiService.svc/payments
Content-Type: application/json

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
```

#### Check Transaction Status
```http
GET /Services/PaymentApiService.svc/payments/{transactionId}/status
```

#### Process Refund
```http
POST /Services/PaymentApiService.svc/payments/{transactionId}/refund
Content-Type: application/json

{
  "Amount": 50.00,
  "Reason": "Customer request"
}
```

### Console Application Commands

```bash
# Interactive mode
PaymentProcessing.Console.exe

# Batch processing
PaymentProcessing.Console.exe /batch sample_payments.csv

# Check transaction status
PaymentProcessing.Console.exe /status {transactionId}

# Generate report
PaymentProcessing.Console.exe /report payment_report.txt
```

### Batch Processing CSV Format

```csv
Amount,Currency,CardNumber,CardHolderName,ExpiryMonth,ExpiryYear,CVV,CustomerEmail,Description
100.00,USD,4111111111111111,John Doe,12,2025,123,john.doe@example.com,Test Payment
```

## Configuration

### Web Applications (web.config)
- Connection strings for database access
- Application settings for merchant configuration
- WCF service configuration

### Console Application (app.config)
- Merchant ID and environment settings
- Database connection strings
- Logging and batch processing settings

## Security Considerations

- Credit card numbers are validated using Luhn algorithm
- Sensitive data should be encrypted in production
- Implement proper authentication and authorization
- Use HTTPS for all web communications
- Follow PCI DSS compliance requirements

## Development Notes

This is a legacy .NET Framework 3.5 application designed to demonstrate:

- Classic ASP.NET MVC patterns
- WCF REST services
- ASMX web services compatibility
- Console application architecture
- File processing and reporting
- Legacy .NET development practices

## License

Copyright © 2025 Payment Solutions Inc. All rights reserved.

## Support

For questions or issues, please refer to the documentation or contact the development team.