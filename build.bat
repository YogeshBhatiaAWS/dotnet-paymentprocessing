@echo off
echo Building Payment Processing System (.NET Framework 3.5)
echo =====================================================

REM Set MSBuild path for .NET Framework 3.5
set MSBUILD_PATH=%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe

if not exist "%MSBUILD_PATH%" (
    echo MSBuild for .NET Framework 3.5 not found!
    echo Please ensure .NET Framework 3.5 is installed.
    pause
    exit /b 1
)

echo Using MSBuild: %MSBUILD_PATH%
echo.

REM Clean solution
echo Cleaning solution...
"%MSBUILD_PATH%" PaymentProcessing.sln /t:Clean /p:Configuration=Debug
if errorlevel 1 goto error

echo.
echo Building solution...
"%MSBUILD_PATH%" PaymentProcessing.sln /t:Build /p:Configuration=Debug
if errorlevel 1 goto error

echo.
echo Build completed successfully!
echo.
echo Available executables:
echo - PaymentProcessing.Web (Web Application)
echo - PaymentProcessing.WebAPI (Web API)
echo - PaymentProcessing.Console\bin\Debug\PaymentProcessing.Console.exe
echo.
pause
exit /b 0

:error
echo.
echo Build failed!
pause
exit /b 1
