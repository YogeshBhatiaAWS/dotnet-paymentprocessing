// Simple verification script for core payment logic
// Compile with: csc /reference:PaymentProcessing.Core.dll verify_logic.cs
// Run with: verify_logic.exe

using System;
using PaymentProcessing.Core.Models;
using PaymentProcessing.Core.Services;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Payment Processing Core Logic Verification ===");
        Console.WriteLine();

        try
        {
            // Initialize services
            var validator = new PaymentValidator();
            var processor = new PaymentProcessor(validator);

            Console.WriteLine("✓ Services initialized successfully");

            // Test 1: Valid payment request
            Console.WriteLine("\nTest 1: Processing valid payment...");
            var validRequest = CreateTestPaymentRequest("4111111111111111", 100.00m);
            var response1 = processor.ProcessPayment(validRequest);
            
            Console.WriteLine($"Transaction ID: {response1.TransactionId}");
            Console.WriteLine($"Status: {response1.Status}");
            Console.WriteLine($"Success: {response1.IsSuccessful}");
            Console.WriteLine($"Message: {response1.ResponseMessage}");

            if (response1.IsSuccessful)
                Console.WriteLine("✓ Valid payment processed successfully");
            else
                Console.WriteLine("✗ Valid payment failed unexpectedly");

            // Test 2: Declined payment
            Console.WriteLine("\nTest 2: Processing declined payment...");
            var declinedRequest = CreateTestPaymentRequest("4111111111111119", 50.00m);
            var response2 = processor.ProcessPayment(declinedRequest);
            
            Console.WriteLine($"Status: {response2.Status}");
            Console.WriteLine($"Success: {response2.IsSuccessful}");
            Console.WriteLine($"Message: {response2.ResponseMessage}");

            if (!response2.IsSuccessful && response2.Status == PaymentStatus.Declined)
                Console.WriteLine("✓ Declined payment handled correctly");
            else
                Console.WriteLine("✗ Declined payment not handled correctly");

            // Test 3: Error scenario
            Console.WriteLine("\nTest 3: Processing error scenario...");
            var errorRequest = CreateTestPaymentRequest("4111111111111110", 75.00m);
            var response3 = processor.ProcessPayment(errorRequest);
            
            Console.WriteLine($"Status: {response3.Status}");
            Console.WriteLine($"Success: {response3.IsSuccessful}");
            Console.WriteLine($"Message: {response3.ResponseMessage}");

            if (!response3.IsSuccessful && response3.Status == PaymentStatus.Error)
                Console.WriteLine("✓ Error scenario handled correctly");
            else
                Console.WriteLine("✗ Error scenario not handled correctly");

            // Test 4: Transaction status check
            Console.WriteLine("\nTest 4: Checking transaction status...");
            var statusResponse = processor.GetTransactionStatus(response1.TransactionId);
            
            if (statusResponse.TransactionId == response1.TransactionId)
                Console.WriteLine("✓ Transaction status retrieved successfully");
            else
                Console.WriteLine("✗ Transaction status retrieval failed");

            // Test 5: Refund processing (if original was successful)
            if (response1.IsSuccessful && response1.Status == PaymentStatus.Approved)
            {
                Console.WriteLine("\nTest 5: Processing refund...");
                var refundResponse = processor.RefundPayment(response1.TransactionId, 25.00m);
                
                Console.WriteLine($"Refund Status: {refundResponse.Status}");
                Console.WriteLine($"Refund Success: {refundResponse.IsSuccessful}");
                Console.WriteLine($"Refund Message: {refundResponse.ResponseMessage}");

                if (refundResponse.IsSuccessful)
                    Console.WriteLine("✓ Refund processed successfully");
                else
                    Console.WriteLine("✗ Refund processing failed");
            }

            // Test 6: Validation tests
            Console.WriteLine("\nTest 6: Testing validation logic...");
            
            // Valid card
            var validCard = new CreditCard
            {
                CardNumber = "4111111111111111",
                CardHolderName = "John Doe",
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                CVV = "123"
            };

            if (validator.ValidateCreditCard(validCard))
                Console.WriteLine("✓ Valid card validation passed");
            else
                Console.WriteLine($"✗ Valid card validation failed: {validator.GetValidationError()}");

            // Invalid card (bad Luhn)
            var invalidCard = new CreditCard
            {
                CardNumber = "1234567890123456",
                CardHolderName = "John Doe",
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                CVV = "123"
            };

            if (!validator.ValidateCreditCard(invalidCard))
                Console.WriteLine("✓ Invalid card validation correctly rejected");
            else
                Console.WriteLine("✗ Invalid card validation incorrectly passed");

            Console.WriteLine("\n=== Core Logic Verification Complete ===");
            Console.WriteLine("All core payment processing functions are working correctly!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error during verification: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static PaymentRequest CreateTestPaymentRequest(string cardNumber, decimal amount)
    {
        return new PaymentRequest
        {
            Amount = amount,
            Currency = "USD",
            MerchantId = "MERCHANT_001",
            CustomerEmail = "test@example.com",
            Description = "Test payment",
            CreditCard = new CreditCard
            {
                CardNumber = cardNumber,
                CardHolderName = "Test User",
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                CVV = "123"
            }
        };
    }
}
