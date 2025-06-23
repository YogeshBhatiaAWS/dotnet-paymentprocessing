@echo off
echo Payment Processing System - Core Functionality Test
echo ==================================================

REM Set the path to the console application
set CONSOLE_PATH=PaymentProcessing.Console\bin\Debug\PaymentProcessing.Console.exe

echo.
echo Testing Core Payment Processing Logic...
echo.

REM Test 1: Batch processing with sample data
echo Test 1: Batch Processing Sample Data
echo -------------------------------------
if exist "%CONSOLE_PATH%" (
    "%CONSOLE_PATH%" /batch PaymentProcessing.Console\Data\sample_payments.csv
) else (
    echo Console application not found. Please build the solution first.
    echo Run: build.bat
    goto end
)

echo.
echo Test 2: Generate System Report
echo ------------------------------
if exist "%CONSOLE_PATH%" (
    "%CONSOLE_PATH%" /report test_system_report.txt
    if exist test_system_report.txt (
        echo Report generated successfully: test_system_report.txt
        echo First few lines of the report:
        echo.
        type test_system_report.txt | more /E +10
    )
) else (
    echo Console application not found.
)

echo.
echo Test 3: Show Help Information
echo -----------------------------
if exist "%CONSOLE_PATH%" (
    "%CONSOLE_PATH%" /help
)

echo.
echo Core functionality tests completed!
echo.
echo Next Steps:
echo 1. For Web Application testing: Open Visual Studio, set PaymentProcessing.Web as startup project, and press F5
echo 2. For Web API testing: Set PaymentProcessing.WebAPI as startup project and press F5
echo 3. Use the test card numbers provided in TESTING.md
echo.

:end
pause
