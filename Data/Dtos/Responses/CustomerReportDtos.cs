namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class RegularCustomerReportItemDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalPurchases { get; set; }
    }

    public class HighSpenderReportItemDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalAmountSpent { get; set; }
    }

    public class PendingCreditReportItemDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalOutstandingBalance { get; set; }
    }
}
