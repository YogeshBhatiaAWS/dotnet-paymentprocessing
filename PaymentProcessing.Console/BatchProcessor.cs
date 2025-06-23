using System;
using System.IO;
using PaymentProcessing.Core.Models;
using PaymentProcessing.Core.Services;

namespace PaymentProcessing.Console
{
    /// <summary>
    /// Handles batch processing of payment files
    /// </summary>
    public class BatchProcessor
    {
        private readonly IPaymentProcessor _paymentProcessor;

        public BatchProcessor(IPaymentProcessor paymentProcessor)
        {
            _paymentProcessor = paymentProcessor ?? throw new ArgumentNullException("paymentProcessor");
        }

        /// <summary>
        /// Process payments from a CSV file
        /// </summary>
        public void ProcessFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    System.Console.WriteLine($"File not found: {filePath}");
                    return;
                }

                System.Console.WriteLine($"Processing batch file: {filePath}");
                System.Console.WriteLine();

                string[] lines = File.ReadAllLines(filePath);
                int totalRecords = 0;
                int successfulRecords = 0;
                int failedRecords = 0;

                // Skip header row
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line))
                        continue;

                    totalRecords++;
                    System.Console.WriteLine($"Processing record {totalRecords}...");

                    try
                    {
                        var paymentRequest = ParseCsvLine(line);
                        var response = _paymentProcessor.ProcessPayment(paymentRequest);

                        if (response.IsSuccessful)
                        {
                            successfulRecords++;
                            System.Console.WriteLine($"✓ Success: {response.TransactionId} - {response.ResponseMessage}");
                        }
                        else
                        {
                            failedRecords++;
                            System.Console.WriteLine($"✗ Failed: {response.TransactionId} - {response.ErrorMessage}");
                        }
                    }
                    catch (Exception ex)
                    {
                        failedRecords++;
                        System.Console.WriteLine($"✗ Error processing record {totalRecords}: {ex.Message}");
                    }

                    System.Console.WriteLine();
                }

                // Display summary
                System.Console.WriteLine("=== Batch Processing Summary ===");
                System.Console.WriteLine($"Total Records: {totalRecords}");
                System.Console.WriteLine($"Successful: {successfulRecords}");
                System.Console.WriteLine($"Failed: {failedRecords}");
                System.Console.WriteLine($"Success Rate: {(totalRecords > 0 ? (successfulRecords * 100.0 / totalRecords):0):F1}%");

                // Generate batch report
                GenerateBatchReport(filePath, totalRecords, successfulRecords, failedRecords);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error processing batch file: {ex.Message}");
            }
        }

        /// <summary>
        /// Parse a CSV line into a PaymentRequest object
        /// Expected format: Amount,Currency,CardNumber,CardHolderName,ExpiryMonth,ExpiryYear,CVV,CustomerEmail,Description
        /// </summary>
        private PaymentRequest ParseCsvLine(string csvLine)
        {
            string[] fields = csvLine.Split(',');
            
            if (fields.Length < 8)
            {
                throw new ArgumentException("Invalid CSV format. Expected at least 8 fields.");
            }

            var request = new PaymentRequest
            {
                Amount = decimal.Parse(fields[0].Trim()),
                Currency = fields[1].Trim(),
                CustomerEmail = fields[7].Trim(),
                Description = fields.Length > 8 ? fields[8].Trim() : "",
                MerchantId = "MERCHANT_001",
                CreditCard = new CreditCard
                {
                    CardNumber = fields[2].Trim(),
                    CardHolderName = fields[3].Trim(),
                    ExpiryMonth = int.Parse(fields[4].Trim()),
                    ExpiryYear = int.Parse(fields[5].Trim()),
                    CVV = fields[6].Trim()
                }
            };

            return request;
        }

        /// <summary>
        /// Generate a batch processing report
        /// </summary>
        private void GenerateBatchReport(string inputFile, int total, int successful, int failed)
        {
            try
            {
                string reportFile = Path.ChangeExtension(inputFile, ".report.txt");
                
                using (StreamWriter writer = new StreamWriter(reportFile))
                {
                    writer.WriteLine("=== Batch Processing Report ===");
                    writer.WriteLine($"Generated: {DateTime.Now}");
                    writer.WriteLine($"Input File: {inputFile}");
                    writer.WriteLine();
                    writer.WriteLine($"Total Records Processed: {total}");
                    writer.WriteLine($"Successful Transactions: {successful}");
                    writer.WriteLine($"Failed Transactions: {failed}");
                    writer.WriteLine($"Success Rate: {(total > 0 ? (successful * 100.0 / total) : 0):F1}%");
                    writer.WriteLine();
                    writer.WriteLine("Report generated by Payment Processing Console Application");
                }

                System.Console.WriteLine($"Batch report saved to: {reportFile}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error generating batch report: {ex.Message}");
            }
        }
    }
}
