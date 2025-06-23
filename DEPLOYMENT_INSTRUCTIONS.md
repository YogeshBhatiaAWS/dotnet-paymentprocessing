# Complete Deployment Instructions

## 🚀 How to Run and Deploy the Payment Processing System

### Prerequisites
- Windows machine with .NET Framework 3.5
- Visual Studio 2008+ or Visual Studio Community
- IIS (for web applications)

## Step 1: Build the Solution

### Option A: Using Visual Studio (Recommended)
1. **Open the solution**:
   ```
   Double-click: PaymentProcessing.sln
   ```

2. **Build the solution**:
   ```
   Menu: Build → Build Solution
   Or press: Ctrl + Shift + B
   ```

3. **Verify build success**:
   - Check Output window for "Build succeeded"
   - No errors in Error List

### Option B: Using Command Line
```batch
# Navigate to project directory
cd C:\path\to\dotnet-paymentprocessing

# Run build script
build.bat

# Or use MSBuild directly
%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe PaymentProcessing.sln /p:Configuration=Release
```

## Step 2: Run Applications Locally

### Console Application (Easiest to test first)
```batch
# Navigate to console app
cd PaymentProcessing.Console\bin\Debug

# Run interactive mode
PaymentProcessing.Console.exe

# Test batch processing
PaymentProcessing.Console.exe /batch Data\sample_payments.csv

# Generate report
PaymentProcessing.Console.exe /report test_report.txt
```

**Expected Output:**
```
=== Payment Processing Console Application ===
Version: 1.0.0 (.NET Framework 3.5)
Copyright © 2025 Payment Solutions Inc.

Payment processing services initialized successfully.

=== Main Menu ===
1. Process Single Payment
2. Batch Process Payments from CSV
3. Check Transaction Status
4. Process Refund
5. Generate Payment Report
6. Test Payment Scenarios
0. Exit

Select an option (0-6):
```

### MVC Web Application
1. **In Visual Studio**:
   - Right-click `PaymentProcessing.Web` project
   - Select "Set as StartUp Project"
   - Press F5 or click Start

2. **Browser should open to**:
   ```
   http://localhost:[port]/
   ```

3. **Test the web interface**:
   - Click "Process Payment"
   - Fill form with test data:
     ```
     Amount: 100.00
     Card Number: 4111111111111111
     Card Holder: John Doe
     Expiry: 12/2025
     CVV: 123
     Email: test@example.com
     ```

### Web API
1. **In Visual Studio**:
   - Right-click `PaymentProcessing.WebAPI` project
   - Select "Set as StartUp Project"
   - Press F5

2. **Test API endpoints**:
   ```
   Health Check: http://localhost:[port]/Services/PaymentApiService.svc/health
   API Docs: http://localhost:[port]/Default.aspx
   ```

## Step 3: Test with cURL Commands

### Health Check
```bash
curl -X GET "http://localhost:8081/Services/PaymentApiService.svc/health"
```

### Process Payment
```bash
curl -X POST "http://localhost:8081/Services/PaymentApiService.svc/payments" \
  -H "Content-Type: application/json" \
  -d "{
    \"Amount\": 100.00,
    \"Currency\": \"USD\",
    \"CardNumber\": \"4111111111111111\",
    \"CardHolderName\": \"John Doe\",
    \"ExpiryMonth\": 12,
    \"ExpiryYear\": 2025,
    \"CVV\": \"123\",
    \"MerchantId\": \"MERCHANT_001\",
    \"CustomerEmail\": \"test@example.com\"
  }"
```

## Step 4: Production Deployment

### IIS Deployment for Web Applications

#### Deploy MVC Web Application
1. **Publish the application**:
   ```
   Right-click PaymentProcessing.Web → Publish
   Choose: File System
   Target Location: C:\inetpub\wwwroot\PaymentWeb
   ```

2. **Create IIS Site**:
   ```
   Open IIS Manager
   Right-click Sites → Add Website
   Site name: PaymentProcessingWeb
   Physical path: C:\inetpub\wwwroot\PaymentWeb
   Port: 80 (or your choice)
   ```

3. **Configure Application Pool**:
   ```
   .NET Framework version: v2.0 (for .NET 3.5)
   Managed pipeline mode: Integrated
   ```

#### Deploy Web API
1. **Publish the API**:
   ```
   Right-click PaymentProcessing.WebAPI → Publish
   Target Location: C:\inetpub\wwwroot\PaymentAPI
   ```

2. **Create IIS Site**:
   ```
   Site name: PaymentProcessingAPI
   Physical path: C:\inetpub\wwwroot\PaymentAPI
   Port: 8081
   ```

### Console Application Deployment
1. **Copy files**:
   ```batch
   xcopy PaymentProcessing.Console\bin\Release C:\PaymentProcessing\ /E /I
   ```

2. **Create Windows Service** (Optional):
   ```batch
   # Install as Windows Service for scheduled processing
   sc create PaymentProcessor binPath= "C:\PaymentProcessing\PaymentProcessing.Console.exe"
   ```

## Step 5: Configuration for Production

### Database Configuration (if needed)
```xml
<!-- Update connection strings in web.config and app.config -->
<connectionStrings>
  <add name="PaymentDatabase" 
       connectionString="Server=YourServer;Database=PaymentProcessing;Integrated Security=true" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Security Configuration
```xml
<!-- Enable HTTPS in web.config -->
<system.web>
  <httpCookies requireSSL="true" />
</system.web>
```

## Step 6: Monitoring and Maintenance

### Log Files Locations
- **IIS Logs**: `%SystemDrive%\inetpub\logs\LogFiles`
- **Event Logs**: Windows Event Viewer → Application
- **Application Logs**: Check application directories

### Performance Monitoring
```batch
# Monitor application performance
perfmon.exe

# Check application pools
%windir%\system32\inetsrv\appcmd.exe list apppool
```

## 🎯 Quick Deployment Checklist

### Pre-Deployment
- [ ] .NET Framework 3.5 installed on target server
- [ ] IIS installed and configured
- [ ] SQL Server installed (if using database)
- [ ] SSL certificates ready (for production)

### Build and Test
- [ ] Solution builds without errors
- [ ] Console application runs successfully
- [ ] Web application loads in browser
- [ ] API endpoints respond correctly
- [ ] All test scenarios pass

### Deploy
- [ ] Publish web applications to IIS
- [ ] Copy console application to server
- [ ] Update configuration files
- [ ] Test deployed applications
- [ ] Configure monitoring and logging

### Post-Deployment
- [ ] Verify all URLs are accessible
- [ ] Test payment processing end-to-end
- [ ] Check log files for errors
- [ ] Set up backup procedures
- [ ] Document deployment for team

## 🔧 Troubleshooting Common Issues

### Build Issues
```
Error: "Could not load file or assembly"
Solution: Install .NET Framework 3.5 SDK
```

### IIS Issues
```
Error: "HTTP Error 500.19"
Solution: Check web.config syntax and IIS configuration
```

### API Issues
```
Error: "Service Unavailable"
Solution: Enable WCF HTTP Activation in Windows Features
```

### Database Issues
```
Error: "Cannot open database"
Solution: Check connection string and SQL Server service
```

## 📞 Support

If you encounter issues:
1. Check Windows Event Logs
2. Review IIS logs
3. Verify .NET Framework installation
4. Test with provided sample data
5. Use debugging tools in Visual Studio

The application is designed to work with legacy systems and should deploy successfully on any Windows server with .NET Framework 3.5.
