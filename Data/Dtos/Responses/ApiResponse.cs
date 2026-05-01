namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        // Helper methods
        public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
            => new() { Success = true, Message = message, Data = data };
        public static ApiResponse<T> FailureResponse(List<string> errors, string message = "Failure")
            => new() { Success = false, Message = message, Errors = errors };
    }
}
