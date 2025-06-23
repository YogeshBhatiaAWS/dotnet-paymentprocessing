# Deployment Guide

## Prerequisites

### System Requirements
- Windows Server 2008 or later
- .NET Framework 3.5 SP1
- IIS 7.0 or later
- SQL Server 2008 or later (optional, for database storage)

### Development Environment
- Visual Studio 2008 SP1 or later
- .NET Framework 3.5 SDK

## Building the Solution

### Using Visual Studio
1. Open `PaymentProcessing.sln` in Visual Studio
2. Select Build → Build Solution
3. Ensure all projects build successfully

### Using Command Line
```batch
# Run the build script
build.bat

# Or use MSBuild directly
%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe PaymentProcessing.sln /p:Configuration=Release
```

## Deployment Steps

### 1. MVC Web Application (PaymentProcessing.Web)

#### IIS Deployment
1. Create a new website in IIS Manager
2. Set the physical path to the `PaymentProcessing.Web` folder
3. Configure the application pool:
   - .NET Framework version: v2.0 (for .NET 3.5)
   - Managed pipeline mode: Integrated
4. Ensure the following modules are installed:
   - ASP.NET MVC 1.0
   - URL Rewrite Module (optional)

#### Configuration
1. Update `web.config` connection strings
2. Set appropriate authentication mode
3. Configure custom errors for production

### 2. Web API (PaymentProcessing.WebAPI)

#### IIS Deployment
1. Create a new website or virtual directory in IIS
2. Set the physical path to the `PaymentProcessing.WebAPI` folder
3. Configure application pool (same as MVC app)
4. Enable WCF HTTP Activation:
   - Windows Features → .NET Framework 3.5.1 Features → WCF Activation

#### Testing API Endpoints
```http
GET http://yourserver/PaymentProcessing.WebAPI/Services/PaymentApiService.svc/health
GET http://yourserver/PaymentProcessing.WebAPI/Services/PaymentApiService.svc/version
```

### 3. Console Application (PaymentProcessing.Console)

#### Deployment
1. Copy the entire `bin\Release` folder to the target server
2. Ensure all dependencies are included:
   - PaymentProcessing.Core.dll
   - PaymentProcessing.Console.exe
   - PaymentProcessing.Console.exe.config

#### Running the Console App
```batch
# Interactive mode
PaymentProcessing.Console.exe

# Batch processing
PaymentProcessing.Console.exe /batch payments.csv

# Generate reports
PaymentProcessing.Console.exe /report monthly_report.txt
```

## Configuration

### Database Setup (Optional)
If using SQL Server for transaction storage:

```sql
CREATE DATABASE PaymentProcessing;

CREATE TABLE Transactions (
    TransactionId NVARCHAR(50) PRIMARY KEY,
    Amount DECIMAL(18,2) NOT NULL,
    Currency NVARCHAR(3) NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    ProcessedDateTime DATETIME NOT NULL,
    CustomerEmail NVARCHAR(255),
    MerchantId NVARCHAR(50)
);
```

### Connection Strings
Update connection strings in all config files:

```xml
<connectionStrings>
  <add name="PaymentDatabase" 
       connectionString="Server=YourServer;Database=PaymentProcessing;Integrated Security=true" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Application Settings
Configure merchant settings:

```xml
<appSettings>
  <add key="PaymentProcessor.MerchantId" value="YOUR_MERCHANT_ID" />
  <add key="PaymentProcessor.Environment" value="Production" />
</appSettings>
```

## Security Configuration

### SSL/HTTPS Setup
1. Install SSL certificate on IIS
2. Configure HTTPS binding
3. Update web.config to require HTTPS:

```xml
<system.web>
  <httpCookies requireSSL="true" />
</system.web>
```

### Authentication
For production environments, implement proper authentication:

```xml
<system.web>
  <authentication mode="Windows" />
  <authorization>
    <deny users="?" />
  </authorization>
</system.web>
```

## Monitoring and Logging

### Event Log Configuration
The application logs to Windows Event Log. Ensure proper permissions:

```batch
# Grant log permissions (run as administrator)
wevtutil sl Application /ca:O:BAG:SYD:(A;;0xf0007;;;SY)(A;;0x7;;;BA)(A;;0x7;;;SO)(A;;0x3;;;IU)(A;;0x3;;;SU)(A;;0x3;;;S-1-5-3)(A;;0x3;;;S-1-5-33)(A;;0x1;;;S-1-5-32-573)
```

### Performance Counters
Monitor application performance:
- Request processing time
- Transaction success rate
- Error frequency

## Troubleshooting

### Common Issues

#### "Could not load file or assembly" errors
- Ensure all referenced assemblies are in the bin folder
- Check .NET Framework 3.5 is properly installed
- Verify assembly versions match

#### WCF Service not accessible
- Check IIS application pool is running
- Verify WCF HTTP Activation is enabled
- Check Windows Firewall settings

#### MVC Routes not working
- Ensure URL Routing module is installed
- Check web.config has proper route configuration
- Verify application pool is in Integrated mode

### Log Files
Check these locations for error information:
- Windows Event Log (Application)
- IIS logs (`%SystemDrive%\inetpub\logs\LogFiles`)
- Application-specific logs (if configured)

## Performance Optimization

### IIS Optimization
1. Enable output caching for static content
2. Configure compression
3. Set appropriate connection limits
4. Monitor application pool recycling

### Application Optimization
1. Implement connection pooling
2. Use async processing where possible
3. Cache frequently accessed data
4. Optimize database queries

## Backup and Recovery

### Files to Backup
- Application files and binaries
- Configuration files (web.config, app.config)
- Database (if used)
- SSL certificates
- IIS configuration

### Recovery Procedures
1. Restore application files
2. Recreate IIS sites and application pools
3. Restore database
4. Update configuration files
5. Test all functionality

## Support

For deployment issues:
1. Check Windows Event Log
2. Review IIS logs
3. Verify .NET Framework 3.5 installation
4. Contact system administrator
