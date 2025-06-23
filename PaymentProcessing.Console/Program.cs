using System;
using System.Configuration;
using PaymentProcessing.Core.Services;

namespace PaymentProcessing.Console
{
    /// <summary>
    /// Main console application for payment processing
    /// </summary>
    class Program
    {
        private static IPaymentProcessor _paymentProcessor;
        private static IPaymentValidator _paymentValidator;

        static void Main(string[] args)
        {
            try
            {
                System.Console.WriteLine("=== Payment Processing Console Application ===");
                System.Console.WriteLine("Version: 1.0.0 (.NET Framework 3.5)");
                System.Console.WriteLine("Copyright © 2025 Payment Solutions Inc.");
                System.Console.WriteLine();

                // Initialize services
                InitializeServices();

                // Parse command line arguments
                if (args.Length == 0)
                {
                    ShowMenu();
                }
                else
                {
                    ProcessCommandLineArgs(args);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Fatal Error: " + ex.Message);
                System.Console.WriteLine("Press any key to exit...");
                System.Console.ReadKey();
                Environment.Exit(1);
            }
        }

        private static void InitializeServices()
        {
            _paymentValidator = new PaymentValidator();
            _paymentProcessor = new PaymentProcessor(_paymentValidator);
            System.Console.WriteLine("Payment processing services initialized successfully.");
            System.Console.WriteLine();
        }

        private static void ShowMenu()
        {
            while (true)
            {
                System.Console.WriteLine("=== Main Menu ===");
                System.Console.WriteLine("1. Process Single Payment");
                System.Console.WriteLine("2. Batch Process Payments from CSV");
                System.Console.WriteLine("3. Check Transaction Status");
                System.Console.WriteLine("4. Process Refund");
                System.Console.WriteLine("5. Generate Payment Report");
                System.Console.WriteLine("6. Test Payment Scenarios");
                System.Console.WriteLine("0. Exit");
                System.Console.WriteLine();
                System.Console.Write("Select an option (0-6): ");

                string choice = System.Console.ReadLine();
                System.Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ProcessSinglePayment();
                        break;
                    case "2":
                        ProcessBatchPayments();
                        break;
                    case "3":
                        CheckTransactionStatus();
                        break;
                    case "4":
                        ProcessRefund();
                        break;
                    case "5":
                        GenerateReport();
                        break;
                    case "6":
                        TestPaymentScenarios();
                        break;
                    case "0":
                        System.Console.WriteLine("Goodbye!");
                        return;
                    default:
                        System.Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                System.Console.WriteLine();
                System.Console.WriteLine("Press any key to continue...");
                System.Console.ReadKey();
                System.Console.Clear();
            }
        }

        private static void ProcessCommandLineArgs(string[] args)
        {
            string command = args[0].ToLower();

            switch (command)
            {
                case "/batch":
                case "-batch":
                    if (args.Length > 1)
                    {
                        var batchProcessor = new BatchProcessor(_paymentProcessor);
                        batchProcessor.ProcessFile(args[1]);
                    }
                    else
                    {
                        System.Console.WriteLine("Usage: PaymentProcessing.Console.exe /batch <filename>");
                    }
                    break;

                case "/status":
                case "-status":
                    if (args.Length > 1)
                    {
                        CheckTransactionStatus(args[1]);
                    }
                    else
                    {
                        System.Console.WriteLine("Usage: PaymentProcessing.Console.exe /status <transactionId>");
                    }
                    break;

                case "/report":
                case "-report":
                    var reportGenerator = new ReportGenerator();
                    string outputFile = args.Length > 1 ? args[1] : "payment_report.txt";
                    reportGenerator.GenerateReport(outputFile);
                    break;

                case "/help":
                case "-help":
                case "/?":
                    ShowHelp();
                    break;

                default:
                    System.Console.WriteLine("Unknown command: " + command);
                    ShowHelp();
                    break;
            }
        }

        private static void ProcessSinglePayment()
        {
            System.Console.WriteLine("=== Process Single Payment ===");
            
            try
            {
                var request = new Core.Models.PaymentRequest();

                System.Console.Write("Enter amount: $");
                if (decimal.TryParse(System.Console.ReadLine(), out decimal amount))
                    request.Amount = amount;
                else
                {
                    System.Console.WriteLine("Invalid amount.");
                    return;
                }

                System.Console.Write("Enter currency (USD): ");
                string currency = System.Console.ReadLine();
                request.Currency = string.IsNullOrEmpty(currency) ? "USD" : currency;

                System.Console.Write("Enter card number: ");
                request.CreditCard = new Core.Models.CreditCard
                {
                    CardNumber = System.Console.ReadLine()
                };

                System.Console.Write("Enter card holder name: ");
                request.CreditCard.CardHolderName = System.Console.ReadLine();

                System.Console.Write("Enter expiry month (1-12): ");
                if (int.TryParse(System.Console.ReadLine(), out int month))
                    request.CreditCard.ExpiryMonth = month;

                System.Console.Write("Enter expiry year: ");
                if (int.TryParse(System.Console.ReadLine(), out int year))
                    request.CreditCard.ExpiryYear = year;

                System.Console.Write("Enter CVV: ");
                request.CreditCard.CVV = System.Console.ReadLine();

                System.Console.Write("Enter customer email: ");
                request.CustomerEmail = System.Console.ReadLine();

                request.MerchantId = ConfigurationManager.AppSettings["MerchantId"] ?? "MERCHANT_001";

                System.Console.WriteLine("\nProcessing payment...");
                var response = _paymentProcessor.ProcessPayment(request);

                DisplayPaymentResult(response);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error processing payment: " + ex.Message);
            }
        }

        private static void ProcessBatchPayments()
        {
            System.Console.WriteLine("=== Batch Process Payments ===");
            System.Console.Write("Enter CSV file path (or press Enter for sample file): ");
            string filePath = System.Console.ReadLine();
            
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = "Data\\sample_payments.csv";
            }

            var batchProcessor = new BatchProcessor(_paymentProcessor);
            batchProcessor.ProcessFile(filePath);
        }

        private static void CheckTransactionStatus()
        {
            System.Console.WriteLine("=== Check Transaction Status ===");
            System.Console.Write("Enter transaction ID: ");
            string transactionId = System.Console.ReadLine();
            
            CheckTransactionStatus(transactionId);
        }

        private static void CheckTransactionStatus(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                System.Console.WriteLine("Transaction ID is required.");
                return;
            }

            try
            {
                var response = _paymentProcessor.GetTransactionStatus(transactionId);
                DisplayPaymentResult(response);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error checking transaction status: " + ex.Message);
            }
        }

        private static void ProcessRefund()
        {
            System.Console.WriteLine("=== Process Refund ===");
            System.Console.Write("Enter transaction ID: ");
            string transactionId = System.Console.ReadLine();

            System.Console.Write("Enter refund amount: $");
            if (decimal.TryParse(System.Console.ReadLine(), out decimal amount))
            {
                try
                {
                    var response = _paymentProcessor.RefundPayment(transactionId, amount);
                    DisplayPaymentResult(response);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Error processing refund: " + ex.Message);
                }
            }
            else
            {
                System.Console.WriteLine("Invalid amount.");
            }
        }

        private static void GenerateReport()
        {
            System.Console.WriteLine("=== Generate Payment Report ===");
            System.Console.Write("Enter output file name (payment_report.txt): ");
            string fileName = System.Console.ReadLine();
            
            if (string.IsNullOrEmpty(fileName))
                fileName = "payment_report.txt";

            var reportGenerator = new ReportGenerator();
            reportGenerator.GenerateReport(fileName);
        }

        private static void TestPaymentScenarios()
        {
            System.Console.WriteLine("=== Test Payment Scenarios ===");
            
            var testCards = new[]
            {
                new { Number = "4111111111111111", Expected = "Approved" },
                new { Number = "4111111111111119", Expected = "Declined" },
                new { Number = "4111111111111110", Expected = "Error" }
            };

            foreach (var testCard in testCards)
            {
                System.Console.WriteLine($"Testing card {testCard.Number} (Expected: {testCard.Expected})");
                
                var request = new Core.Models.PaymentRequest
                {
                    Amount = 100.00m,
                    Currency = "USD",
                    MerchantId = "MERCHANT_001",
                    CustomerEmail = "test@example.com",
                    CreditCard = new Core.Models.CreditCard
                    {
                        CardNumber = testCard.Number,
                        CardHolderName = "Test User",
                        ExpiryMonth = 12,
                        ExpiryYear = 2025,
                        CVV = "123"
                    }
                };

                var response = _paymentProcessor.ProcessPayment(request);
                System.Console.WriteLine($"Result: {response.Status} - {response.ResponseMessage}");
                System.Console.WriteLine();
            }
        }

        private static void DisplayPaymentResult(Core.Models.PaymentResponse response)
        {
            System.Console.WriteLine("\n=== Payment Result ===");
            System.Console.WriteLine($"Transaction ID: {response.TransactionId}");
            System.Console.WriteLine($"Status: {response.Status}");
            System.Console.WriteLine($"Amount: ${response.ProcessedAmount:F2}");
            System.Console.WriteLine($"Processed: {response.ProcessedDateTime}");
            System.Console.WriteLine($"Success: {response.IsSuccessful}");
            
            if (!string.IsNullOrEmpty(response.AuthorizationCode))
                System.Console.WriteLine($"Auth Code: {response.AuthorizationCode}");
            
            System.Console.WriteLine($"Message: {response.ResponseMessage}");
            
            if (!string.IsNullOrEmpty(response.ErrorMessage))
                System.Console.WriteLine($"Error: {response.ErrorMessage}");
        }

        private static void ShowHelp()
        {
            System.Console.WriteLine("Payment Processing Console Application");
            System.Console.WriteLine("Usage:");
            System.Console.WriteLine("  PaymentProcessing.Console.exe                    - Interactive mode");
            System.Console.WriteLine("  PaymentProcessing.Console.exe /batch <file>      - Batch process CSV file");
            System.Console.WriteLine("  PaymentProcessing.Console.exe /status <txnId>    - Check transaction status");
            System.Console.WriteLine("  PaymentProcessing.Console.exe /report [file]     - Generate payment report");
            System.Console.WriteLine("  PaymentProcessing.Console.exe /help              - Show this help");
        }
    }
}
