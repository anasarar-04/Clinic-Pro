using ClinicPRO.Application.DTOs;

namespace ClinicPRO.Application.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto);
        Task<(bool Success, string Message, int AccountId, string Role)> LoginAsync(LoginDto dto);
    }
}