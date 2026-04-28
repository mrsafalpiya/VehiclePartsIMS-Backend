using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentResponseDto> BookAppointmentAsync(int customerId, CreateAppointmentDto dto);
        Task<List<AppointmentResponseDto>> GetMyAppointmentsAsync(int customerId);
        Task<List<AppointmentResponseDto>> GetAllAppointmentsAsync();
    }
}