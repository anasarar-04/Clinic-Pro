using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using ClinicPRO.Domain.Enums;

namespace ClinicPRO.Domain.Entities
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(20)]
        public string NationalId { get; set; }   // Unique - يستخدم لتسجيل الدخول

        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties (علاقة is-a)
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public Settings? Settings { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}