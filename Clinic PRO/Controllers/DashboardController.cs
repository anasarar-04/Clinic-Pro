using Clinic_PRO.Models;
using ClinicPRO.Application.Interfaces;
using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_PRO.Controllers
{
    // no-store: يمنع المتصفح يعرض اللوحة من الكاش لما تعمل رجوع بعد تسجيل الخروج
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class DashboardController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly INotificationService _notificationService;

        public DashboardController(
            IAccountRepository accountRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IAppointmentService appointmentService,
            INotificationService notificationService)
        {
            _accountRepository = accountRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _appointmentService = appointmentService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var account = await _accountRepository.GetByIdAsync(accountId.Value);
            if (account == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            if (HttpContext.Session.GetString("Role") == "Doctor")
                return await BuildDoctorDashboard(account.AccountId, account.FullName);

            return await BuildPatientDashboard(account.AccountId, account.FullName);
        }

        private async Task<IActionResult> BuildPatientDashboard(int accountId, string fullName)
        {
            var patient = await _patientRepository.GetByAccountIdAsync(accountId);
            if (patient == null)
                return RedirectToAction("Login", "Account");

            var today = DateTime.Today;
            var appointments = await _appointmentService.GetMyAppointmentsAsync(patient.PatientId);
            var notifications = await _notificationService.GetMyNotificationsAsync(accountId);

            var upcoming = appointments
                .Where(a => a.Status != AppointmentStatus.Cancelled
                         && a.Status != AppointmentStatus.Completed
                         && a.SlotDate.HasValue
                         && a.SlotDate.Value.Date >= today)
                .OrderBy(a => a.SlotDate)
                .ThenBy(a => a.StartTime)
                .ToList();

            var vm = new PatientDashboardViewModel
            {
                FullName = fullName,
                UpcomingCount = upcoming.Count,
                CompletedCount = appointments.Count(a => a.Status == AppointmentStatus.Completed),
                NotificationsCount = notifications.Count,
                NextAppointment = upcoming.FirstOrDefault(),
                RecentAppointments = appointments.Take(5).ToList(),
                LatestNotifications = notifications.OrderByDescending(n => n.ScheduledAt).Take(4).ToList()
            };

            return View("PatientDashboard", vm);
        }

        private async Task<IActionResult> BuildDoctorDashboard(int accountId, string fullName)
        {
            var doctor = await _doctorRepository.GetByAccountIdAsync(accountId);
            if (doctor == null)
                return RedirectToAction("Login", "Account");

            var today = DateTime.Today;
            var appointments = await _appointmentService.GetDoctorAppointmentsAsync(doctor.DoctorId);
            var slots = await _appointmentService.GetDoctorSlotsAsync(doctor.DoctorId);

            var active = appointments.Where(a => a.Status != AppointmentStatus.Cancelled).ToList();

            var vm = new DoctorDashboardViewModel
            {
                FullName = fullName,
                TodayAppointments = active
                    .Where(a => a.SlotDate.HasValue && a.SlotDate.Value.Date == today)
                    .OrderBy(a => a.StartTime)
                    .ToList(),
                UpcomingAppointments = active
                    .Where(a => a.Status != AppointmentStatus.Completed
                             && a.SlotDate.HasValue
                             && a.SlotDate.Value.Date > today)
                    .OrderBy(a => a.SlotDate)
                    .ThenBy(a => a.StartTime)
                    .Take(5)
                    .ToList(),
                PendingCount = active.Count(a => a.Status != AppointmentStatus.Completed),
                CompletedCount = active.Count(a => a.Status == AppointmentStatus.Completed),
                FreeSlotsCount = slots.Count(s => !s.IsBooked && s.SlotDate.Date >= today)
            };
            vm.TodayCount = vm.TodayAppointments.Count;

            return View("DoctorDashboard", vm);
        }
    }
}