@echo off
echo Creating Deployment Package for Payment Processing System
echo ========================================================

set PACKAGE_DIR=DeploymentPackage
set DATE_STAMP=%date:~-4,4%%date:~-10,2%%date:~-7,2%

echo.
echo Creating deployment directory structure...

REM Create main deployment directory
if exist %PACKAGE_DIR% rmdir /s /q %PACKAGE_DIR%
mkdir %PACKAGE_DIR%
mkdir %PACKAGE_DIR%\WebApp
mkdir %PACKAGE_DIR%\WebAPI
mkdir %PACKAGE_DIR%\Console
mkdir %PACKAGE_DIR%\Documentation
mkdir %PACKAGE_DIR%\Scripts

echo.
echo Building solution...
%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe PaymentProcessing.sln /p:Configuration=Release
if errorlevel 1 (
    echo Build failed! Cannot create deployment package.
    pause
    exit /b 1
)

echo.
echo Copying Web Application files...
xcopy PaymentProcessing.Web %PACKAGE_DIR%\WebApp\ /E /I /Y
del %PACKAGE_DIR%\WebApp\*.cs /S /Q
del %PACKAGE_DIR%\WebApp\*.csproj /Q
del %PACKAGE_DIR%\WebApp\obj /S /Q 2>nul

echo.
echo Copying Web API files...
xcopy PaymentProcessing.WebAPI %PACKAGE_DIR%\WebAPI\ /E /I /Y
del %PACKAGE_DIR%\WebAPI\*.cs /S /Q
del %PACKAGE_DIR%\WebAPI\*.csproj /Q
del %PACKAGE_DIR%\WebAPI\obj /S /Q 2>nul

echo.
echo Copying Console Application...
xcopy PaymentProcessing.Console\bin\Release %PACKAGE_DIR%\Console\ /E /I /Y
xcopy PaymentProcessing.Console\Data %PACKAGE_DIR%\Console\Data\ /E /I /Y

echo.
echo Copying Core Library...
copy PaymentProcessing.Core\bin\Release\PaymentProcessing.Core.dll %PACKAGE_DIR%\Console\ /Y
copy PaymentProcessing.Core\bin\Release\PaymentProcessing.Core.dll %PACKAGE_DIR%\WebApp\bin\ /Y
copy PaymentProcessing.Core\bin\Release\PaymentProcessing.Core.dll %PACKAGE_DIR%\WebAPI\bin\ /Y

echo.
echo Copying Documentation...
copy README.md %PACKAGE_DIR%\Documentation\ /Y
copy DEPLOYMENT.md %PACKAGE_DIR%\Documentation\ /Y
copy TESTING.md %PACKAGE_DIR%\Documentation\ /Y
copy TEST_CHECKLIST.md %PACKAGE_DIR%\Documentation\ /Y
copy DEPLOYMENT_INSTRUCTIONS.md %PACKAGE_DIR%\Documentation\ /Y

echo.
echo Creating deployment scripts...

REM Create IIS deployment script
echo @echo off > %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo echo Deploying Payment Processing System to IIS >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo echo ============================================= >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo. >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo REM Create IIS directories >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo if not exist C:\inetpub\wwwroot\PaymentWeb mkdir C:\inetpub\wwwroot\PaymentWeb >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo if not exist C:\inetpub\wwwroot\PaymentAPI mkdir C:\inetpub\wwwroot\PaymentAPI >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo. >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo REM Copy files >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo xcopy WebApp C:\inetpub\wwwroot\PaymentWeb\ /E /I /Y >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo xcopy WebAPI C:\inetpub\wwwroot\PaymentAPI\ /E /I /Y >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo. >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo echo Deployment completed! >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo echo Configure IIS sites manually or use appcmd commands >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat
echo pause >> %PACKAGE_DIR%\Scripts\deploy_to_iis.bat

REM Create console app installer
echo @echo off > %PACKAGE_DIR%\Scripts\install_console_app.bat
echo echo Installing Payment Processing Console Application >> %PACKAGE_DIR%\Scripts\install_console_app.bat
echo echo ================================================= >> %PACKAGE_DIR%\Scripts\install_console_app.bat
echo. >> %PACKAGE_DIR%\Scripts\install_console_app.bat
echo if not exist C:\PaymentProcessing mkdir C:\PaymentProcessing >> %PACKAGE_DIR%\Scripts\install_console_app.bat
echo xcopy Console C:\PaymentProcessing\ /E /I /Y >> %PACKAGE_DIR%\Scripts\install_console_app.bat
echo. >> %PACKAGE_DIR%\Scripts\install_console_app.bat
echo echo Console application installed to C:\PaymentProcessing\ >> %PACKAGE_DIR%\Scripts\install_console_app.bat
echo echo Run: C:\PaymentProcessing\PaymentProcessing.Console.exe >> %PACKAGE_DIR%\Scripts\install_console_app.bat
echo pause >> %PACKAGE_DIR%\Scripts\install_console_app.bat

REM Create test script
echo @echo off > %PACKAGE_DIR%\Scripts\run_tests.bat
echo echo Running Payment Processing System Tests >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo echo ======================================== >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo. >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo cd Console >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo echo Testing batch processing... >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo PaymentProcessing.Console.exe /batch Data\sample_payments.csv >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo. >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo echo Generating test report... >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo PaymentProcessing.Console.exe /report deployment_test_report.txt >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo. >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo echo Tests completed! Check output above. >> %PACKAGE_DIR%\Scripts\run_tests.bat
echo pause >> %PACKAGE_DIR%\Scripts\run_tests.bat

echo.
echo Creating README for deployment package...
echo Payment Processing System - Deployment Package > %PACKAGE_DIR%\README.txt
echo =============================================== >> %PACKAGE_DIR%\README.txt
echo. >> %PACKAGE_DIR%\README.txt
echo This package contains all files needed to deploy the Payment Processing System. >> %PACKAGE_DIR%\README.txt
echo. >> %PACKAGE_DIR%\README.txt
echo Directory Structure: >> %PACKAGE_DIR%\README.txt
echo - WebApp\     : MVC Web Application files >> %PACKAGE_DIR%\README.txt
echo - WebAPI\     : Web API service files >> %PACKAGE_DIR%\README.txt
echo - Console\    : Console application and dependencies >> %PACKAGE_DIR%\README.txt
echo - Documentation\ : All documentation files >> %PACKAGE_DIR%\README.txt
echo - Scripts\    : Deployment and installation scripts >> %PACKAGE_DIR%\README.txt
echo. >> %PACKAGE_DIR%\README.txt
echo Quick Start: >> %PACKAGE_DIR%\README.txt
echo 1. Run Scripts\deploy_to_iis.bat (as Administrator) >> %PACKAGE_DIR%\README.txt
echo 2. Run Scripts\install_console_app.bat >> %PACKAGE_DIR%\README.txt
echo 3. Run Scripts\run_tests.bat to verify installation >> %PACKAGE_DIR%\README.txt
echo. >> %PACKAGE_DIR%\README.txt
echo For detailed instructions, see Documentation\DEPLOYMENT_INSTRUCTIONS.md >> %PACKAGE_DIR%\README.txt
echo. >> %PACKAGE_DIR%\README.txt
echo Generated: %date% %time% >> %PACKAGE_DIR%\README.txt

echo.
echo Creating ZIP package...
if exist PaymentProcessing_Deployment_%DATE_STAMP%.zip del PaymentProcessing_Deployment_%DATE_STAMP%.zip
powershell -command "Compress-Archive -Path '%PACKAGE_DIR%\*' -DestinationPath 'PaymentProcessing_Deployment_%DATE_STAMP%.zip'"

echo.
echo ========================================================
echo Deployment Package Created Successfully!
echo ========================================================
echo.
echo Package Location: %CD%\%PACKAGE_DIR%\
echo ZIP File: PaymentProcessing_Deployment_%DATE_STAMP%.zip
echo.
echo Contents:
echo - Web Application (ready for IIS)
echo - Web API (ready for IIS)
echo - Console Application (ready to run)
echo - Complete Documentation
echo - Deployment Scripts
echo.
echo Next Steps:
echo 1. Copy package to target server
echo 2. Run deployment scripts as Administrator
echo 3. Configure IIS sites
echo 4. Test the applications
echo.
echo See Documentation\DEPLOYMENT_INSTRUCTIONS.md for detailed steps.
echo.
pause
