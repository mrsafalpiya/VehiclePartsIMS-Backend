namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class FinancialReportDto
    {
        public string ReportType { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public int TotalRevenue { get; set; }
        public int TotalExpenditure { get; set; }
        public int NetProfit { get; set; }
        public List<TransactionItemDto> Transactions { get; set; } = [];
    }

    public class TransactionItemDto
    {
        public string Type { get; set; } = string.Empty; // "Sale" or "Purchase"
        public string InvoiceNumber { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Amount { get; set; }
    }
}