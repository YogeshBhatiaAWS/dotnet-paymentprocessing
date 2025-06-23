@echo off
echo Verifying Payment Processing System Structure
echo ============================================

echo.
echo Checking solution file...
if exist PaymentProcessing.sln (
    echo ✓ PaymentProcessing.sln found
) else (
    echo ✗ PaymentProcessing.sln missing
)

echo.
echo Checking project directories...
if exist PaymentProcessing.Core (
    echo ✓ PaymentProcessing.Core project found
) else (
    echo ✗ PaymentProcessing.Core project missing
)

if exist PaymentProcessing.Web (
    echo ✓ PaymentProcessing.Web project found
) else (
    echo ✗ PaymentProcessing.Web project missing
)

if exist PaymentProcessing.WebAPI (
    echo ✓ PaymentProcessing.WebAPI project found
) else (
    echo ✗ PaymentProcessing.WebAPI project missing
)

if exist PaymentProcessing.Console (
    echo ✓ PaymentProcessing.Console project found
) else (
    echo ✗ PaymentProcessing.Console project missing
)

echo.
echo Checking key files...
if exist PaymentProcessing.Core\Services\PaymentProcessor.cs (
    echo ✓ Core payment processor found
) else (
    echo ✗ Core payment processor missing
)

if exist PaymentProcessing.Web\Controllers\PaymentController.cs (
    echo ✓ Web payment controller found
) else (
    echo ✗ Web payment controller missing
)

if exist PaymentProcessing.WebAPI\Services\PaymentApiService.svc (
    echo ✓ Web API service found
) else (
    echo ✗ Web API service missing
)

if exist PaymentProcessing.Console\Program.cs (
    echo ✓ Console application main program found
) else (
    echo ✗ Console application main program missing
)

if exist PaymentProcessing.Console\Data\sample_payments.csv (
    echo ✓ Sample CSV data found
) else (
    echo ✗ Sample CSV data missing
)

echo.
echo Checking documentation...
if exist README.md (
    echo ✓ README.md found
) else (
    echo ✗ README.md missing
)

if exist TESTING.md (
    echo ✓ TESTING.md found
) else (
    echo ✗ TESTING.md missing
)

if exist DEPLOYMENT_INSTRUCTIONS.md (
    echo ✓ DEPLOYMENT_INSTRUCTIONS.md found
) else (
    echo ✗ DEPLOYMENT_INSTRUCTIONS.md missing
)

echo.
echo Structure verification completed!
echo.
echo To build and run the application:
echo 1. Open PaymentProcessing.sln in Visual Studio
echo 2. Build the solution (Ctrl+Shift+B)
echo 3. Set startup project and run (F5)
echo.
echo Or use the build script:
echo build.bat
echo.
pause
