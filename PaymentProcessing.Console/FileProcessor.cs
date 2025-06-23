using System;
using System.IO;
using System.Text;

namespace PaymentProcessing.Console
{
    /// <summary>
    /// Utility class for file processing operations
    /// </summary>
    public static class FileProcessor
    {
        /// <summary>
        /// Create a sample CSV file for testing batch processing
        /// </summary>
        public static void CreateSampleCsvFile(string filePath)
        {
            try
            {
                var csvContent = new StringBuilder();
                csvContent.AppendLine("Amount,Currency,CardNumber,CardHolderName,ExpiryMonth,ExpiryYear,CVV,CustomerEmail,Description");
                csvContent.AppendLine("100.00,USD,4111111111111111,John Doe,12,2025,123,john.doe@example.com,Test Payment 1");
                csvContent.AppendLine("250.50,USD,4111111111111119,Jane Smith,11,2025,456,jane.smith@example.com,Test Payment 2");
                csvContent.AppendLine("75.25,USD,4111111111111110,Bob Johnson,10,2025,789,bob.johnson@example.com,Test Payment 3");
                csvContent.AppendLine("500.00,EUR,4111111111111111,Alice Brown,09,2025,321,alice.brown@example.com,Test Payment 4");
                csvContent.AppendLine("150.75,USD,4111111111111119,Charlie Wilson,08,2025,654,charlie.wilson@example.com,Test Payment 5");

                File.WriteAllText(filePath, csvContent.ToString());
                System.Console.WriteLine($"Sample CSV file created: {filePath}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error creating sample CSV file: {ex.Message}");
            }
        }

        /// <summary>
        /// Validate CSV file format
        /// </summary>
        public static bool ValidateCsvFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    System.Console.WriteLine($"File not found: {filePath}");
                    return false;
                }

                string[] lines = File.ReadAllLines(filePath);
                
                if (lines.Length < 2)
                {
                    System.Console.WriteLine("CSV file must contain at least a header row and one data row.");
                    return false;
                }

                // Check header
                string header = lines[0];
                string[] expectedHeaders = { "Amount", "Currency", "CardNumber", "CardHolderName", "ExpiryMonth", "ExpiryYear", "CVV", "CustomerEmail" };
                
                foreach (string expectedHeader in expectedHeaders)
                {
                    if (!header.Contains(expectedHeader))
                    {
                        System.Console.WriteLine($"Missing required header: {expectedHeader}");
                        return false;
                    }
                }

                // Validate data rows
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line))
                        continue;

                    string[] fields = line.Split(',');
                    if (fields.Length < 8)
                    {
                        System.Console.WriteLine($"Invalid data format on line {i + 1}: Expected at least 8 fields, found {fields.Length}");
                        return false;
                    }

                    // Validate amount
                    if (!decimal.TryParse(fields[0].Trim(), out decimal amount) || amount <= 0)
                    {
                        System.Console.WriteLine($"Invalid amount on line {i + 1}: {fields[0]}");
                        return false;
                    }

                    // Validate expiry month
                    if (!int.TryParse(fields[4].Trim(), out int month) || month < 1 || month > 12)
                    {
                        System.Console.WriteLine($"Invalid expiry month on line {i + 1}: {fields[4]}");
                        return false;
                    }

                    // Validate expiry year
                    if (!int.TryParse(fields[5].Trim(), out int year) || year < DateTime.Now.Year)
                    {
                        System.Console.WriteLine($"Invalid expiry year on line {i + 1}: {fields[5]}");
                        return false;
                    }
                }

                System.Console.WriteLine("CSV file validation passed.");
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error validating CSV file: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Read file content safely
        /// </summary>
        public static string ReadFileContent(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Write content to file safely
        /// </summary>
        public static bool WriteFileContent(string filePath, string content)
        {
            try
            {
                // Create directory if it doesn't exist
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, content);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error writing file {filePath}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get file size in a human-readable format
        /// </summary>
        public static string GetFileSize(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return "File not found";

                long bytes = new FileInfo(filePath).Length;
                
                if (bytes < 1024)
                    return $"{bytes} bytes";
                else if (bytes < 1024 * 1024)
                    return $"{bytes / 1024.0:F1} KB";
                else
                    return $"{bytes / (1024.0 * 1024.0):F1} MB";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
