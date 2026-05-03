using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IReportService
    {
        Task<FinancialReportDto> GetDailyReportAsync(DateOnly date);
        Task<FinancialReportDto> GetMonthlyReportAsync(int month, int year);
        Task<FinancialReportDto> GetYearlyReportAsync(int year);
    }
}