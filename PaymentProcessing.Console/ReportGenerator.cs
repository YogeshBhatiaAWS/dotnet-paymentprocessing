using System;
using System.IO;
using System.Text;

namespace PaymentProcessing.Console
{
    /// <summary>
    /// Generates various reports for payment processing
    /// </summary>
    public class ReportGenerator
    {
        /// <summary>
        /// Generate a comprehensive payment processing report
        /// </summary>
        public void GenerateReport(string outputFile)
        {
            try
            {
                System.Console.WriteLine("Generating payment processing report...");

                var report = new StringBuilder();
                
                // Report header
                report.AppendLine("=".PadRight(60, '='));
                report.AppendLine("PAYMENT PROCESSING SYSTEM REPORT");
                report.AppendLine("=".PadRight(60, '='));
                report.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine($"System: .NET Framework 3.5");
                report.AppendLine($"Version: 1.0.0");
                report.AppendLine();

                // System Information
                GenerateSystemInfo(report);

                // Transaction Summary (simulated data)
                GenerateTransactionSummary(report);

                // Performance Metrics
                GeneratePerformanceMetrics(report);

                // Error Analysis
                GenerateErrorAnalysis(report);

                // Recommendations
                GenerateRecommendations(report);

                // Footer
                report.AppendLine();
                report.AppendLine("=".PadRight(60, '='));
                report.AppendLine("END OF REPORT");
                report.AppendLine("=".PadRight(60, '='));

                // Write report to file
                File.WriteAllText(outputFile, report.ToString());
                
                System.Console.WriteLine($"Report generated successfully: {outputFile}");
                System.Console.WriteLine($"Report size: {FileProcessor.GetFileSize(outputFile)}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error generating report: {ex.Message}");
            }
        }

        private void GenerateSystemInfo(StringBuilder report)
        {
            report.AppendLine("SYSTEM INFORMATION");
            report.AppendLine("-".PadRight(40, '-'));
            report.AppendLine($"Operating System: {Environment.OSVersion}");
            report.AppendLine($"Machine Name: {Environment.MachineName}");
            report.AppendLine($"User Name: {Environment.UserName}");
            report.AppendLine($"CLR Version: {Environment.Version}");
            report.AppendLine($"Working Directory: {Environment.CurrentDirectory}");
            report.AppendLine($"System Uptime: {TimeSpan.FromMilliseconds(Environment.TickCount)}");
            report.AppendLine();
        }

        private void GenerateTransactionSummary(StringBuilder report)
        {
            report.AppendLine("TRANSACTION SUMMARY (Last 30 Days)");
            report.AppendLine("-".PadRight(40, '-'));
            
            // Simulated data for demonstration
            var random = new Random();
            int totalTransactions = random.Next(1000, 5000);
            int approvedTransactions = (int)(totalTransactions * 0.85);
            int declinedTransactions = (int)(totalTransactions * 0.12);
            int errorTransactions = totalTransactions - approvedTransactions - declinedTransactions;
            decimal totalAmount = (decimal)(random.NextDouble() * 1000000);

            report.AppendLine($"Total Transactions: {totalTransactions:N0}");
            report.AppendLine($"Approved: {approvedTransactions:N0} ({(approvedTransactions * 100.0 / totalTransactions):F1}%)");
            report.AppendLine($"Declined: {declinedTransactions:N0} ({(declinedTransactions * 100.0 / totalTransactions):F1}%)");
            report.AppendLine($"Errors: {errorTransactions:N0} ({(errorTransactions * 100.0 / totalTransactions):F1}%)");
            report.AppendLine($"Total Amount Processed: ${totalAmount:N2}");
            report.AppendLine($"Average Transaction: ${totalAmount / totalTransactions:F2}");
            report.AppendLine();
        }

        private void GeneratePerformanceMetrics(StringBuilder report)
        {
            report.AppendLine("PERFORMANCE METRICS");
            report.AppendLine("-".PadRight(40, '-'));
            
            var random = new Random();
            double avgResponseTime = random.NextDouble() * 2000 + 500; // 500-2500ms
            double maxResponseTime = avgResponseTime * 2;
            double minResponseTime = avgResponseTime * 0.3;

            report.AppendLine($"Average Response Time: {avgResponseTime:F0} ms");
            report.AppendLine($"Maximum Response Time: {maxResponseTime:F0} ms");
            report.AppendLine($"Minimum Response Time: {minResponseTime:F0} ms");
            report.AppendLine($"Throughput: {random.Next(50, 200)} transactions/minute");
            report.AppendLine($"System Availability: {99.5 + random.NextDouble() * 0.4:F2}%");
            report.AppendLine();
        }

        private void GenerateErrorAnalysis(StringBuilder report)
        {
            report.AppendLine("ERROR ANALYSIS");
            report.AppendLine("-".PadRight(40, '-'));
            
            var random = new Random();
            string[] errorTypes = {
                "Invalid Card Number",
                "Expired Card",
                "Insufficient Funds",
                "Network Timeout",
                "Invalid CVV",
                "Card Blocked"
            };

            report.AppendLine("Top Error Types:");
            for (int i = 0; i < errorTypes.Length; i++)
            {
                int count = random.Next(10, 100);
                report.AppendLine($"  {i + 1}. {errorTypes[i]}: {count} occurrences");
            }
            report.AppendLine();
        }

        private void GenerateRecommendations(StringBuilder report)
        {
            report.AppendLine("RECOMMENDATIONS");
            report.AppendLine("-".PadRight(40, '-'));
            report.AppendLine("1. Monitor transaction decline rates and investigate high decline patterns");
            report.AppendLine("2. Implement retry logic for network timeout errors");
            report.AppendLine("3. Consider implementing fraud detection for suspicious patterns");
            report.AppendLine("4. Regular backup of transaction logs and data");
            report.AppendLine("5. Monitor system performance and scale resources as needed");
            report.AppendLine("6. Implement comprehensive logging for audit trails");
            report.AppendLine("7. Regular security updates and vulnerability assessments");
            report.AppendLine();
        }

        /// <summary>
        /// Generate a daily transaction report
        /// </summary>
        public void GenerateDailyReport(DateTime date, string outputFile)
        {
            try
            {
                var report = new StringBuilder();
                
                report.AppendLine($"DAILY TRANSACTION REPORT - {date:yyyy-MM-dd}");
                report.AppendLine("=".PadRight(50, '='));
                report.AppendLine();

                // Simulated daily data
                var random = new Random();
                int dailyTransactions = random.Next(50, 200);
                decimal dailyAmount = (decimal)(random.NextDouble() * 50000);

                report.AppendLine($"Date: {date:yyyy-MM-dd}");
                report.AppendLine($"Total Transactions: {dailyTransactions}");
                report.AppendLine($"Total Amount: ${dailyAmount:N2}");
                report.AppendLine($"Average Transaction: ${dailyAmount / dailyTransactions:F2}");
                report.AppendLine();

                // Hourly breakdown
                report.AppendLine("HOURLY BREAKDOWN");
                report.AppendLine("-".PadRight(30, '-'));
                for (int hour = 0; hour < 24; hour++)
                {
                    int hourlyCount = random.Next(0, 20);
                    decimal hourlyAmount = (decimal)(random.NextDouble() * 5000);
                    report.AppendLine($"{hour:D2}:00 - {hourlyCount:D2} transactions, ${hourlyAmount:N2}");
                }

                File.WriteAllText(outputFile, report.ToString());
                System.Console.WriteLine($"Daily report generated: {outputFile}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error generating daily report: {ex.Message}");
            }
        }
    }
}
