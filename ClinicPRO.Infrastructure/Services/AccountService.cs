using ClinicPRO.Application.DTOs;
using ClinicPRO.Application.Interfaces;
using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto)
        {
            // 1. تحقق كلمة السر والتأكيد متطابقين
            if (dto.Password != dto.ConfirmPassword)
                return (false, "كلمة السر وتأكيد كلمة السر غير متطابقين");

            // 2. تحقق الرقم الوطني مش مستخدم من قبل
            var existing = await _unitOfWork.Accounts.GetByNationalIdAsync(dto.NationalId);
            if (existing != null)
                return (false, "الرقم الوطني مسجل مسبقا");

            // 3. أنشئ الحساب
            var account = new Account
            {
                FullName = dto.FullName,
                NationalId = dto.NationalId,
                PhoneNumber = dto.PhoneNumber,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();   // نحفظ هون حتى ياخذ account.AccountId قيمته الفعلية

            // 4. أنشئ Patient أو Doctor حسب الدور
            if (dto.Role == Domain.Enums.UserRole.Patient)
            {
                var patient = new Patient { AccountId = account.AccountId };
                await _unitOfWork.Patients.AddAsync(patient);
            }
            else if (dto.Role == Domain.Enums.UserRole.Doctor)
            {
                if (dto.SpecializationId == null)
                    return (false, "يجب اختيار التخصص للدكتور");

                var doctor = new Doctor
                {
                    AccountId = account.AccountId,
                    SpecializationId = dto.SpecializationId.Value,
                    Bio = dto.Bio
                };
                await _unitOfWork.Doctors.AddAsync(doctor);
            }

            // 5. أنشئ إعدادات افتراضية للحساب
            var settings = new Settings
            {
                AccountId = account.AccountId,
                NotificationsEnabled = true,
                Language = "ar"
            };
            await _unitOfWork.Settings.AddAsync(settings);

            await _unitOfWork.SaveChangesAsync();

            return (true, "تم إنشاء الحساب بنجاح");
        }

        public async Task<(bool Success, string Message, int AccountId, string Role)> LoginAsync(LoginDto dto)
        {
            var account = await _unitOfWork.Accounts.GetByNationalIdAsync(dto.NationalId);

            if (account == null)
                return (false, "الرقم الوطني غير مسجل", 0, string.Empty);

            bool passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, account.PasswordHash);

            if (!passwordValid)
                return (false, "كلمة السر غير صحيحة", 0, string.Empty);

            return (true, "تم تسجيل الدخول بنجاح", account.AccountId, account.Role.ToString());
        }
    }
}