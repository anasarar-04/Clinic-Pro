using System.ComponentModel.DataAnnotations;

namespace ClinicPRO.Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "الرقم الوطني مطلوب")]
        [Display(Name = "الرقم الوطني")]
        public string NationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة السر مطلوبة")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; } = string.Empty;
    }
}