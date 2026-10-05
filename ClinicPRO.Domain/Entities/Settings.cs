using System.ComponentModel.DataAnnotations;

namespace ClinicPRO.Domain.Entities
{
    public class Settings
    {
        [Key]
        public int SettingId { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public bool NotificationsEnabled { get; set; } = true;

        [MaxLength(10)]
        public string Language { get; set; } = "ar";
    }
}