using Microsoft.AspNetCore.Mvc;
using ClinicPRO.Application.Interfaces;

namespace Clinic_PRO.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var notifications = await _notificationService.GetMyNotificationsAsync(accountId.Value);
            return View(notifications);
        }
    }
}