using System;

namespace PaymentProcessing.Core.Models
{
    /// <summary>
    /// Enumeration representing the status of a payment transaction
    /// </summary>
    public enum PaymentStatus
    {
        Pending = 0,
        Approved = 1,
        Declined = 2,
        Error = 3,
        Cancelled = 4,
        Refunded = 5
    }
}
