using Microsoft.AspNetCore.Mvc;
using ClinicPRO.Application.DTOs;
using ClinicPRO.Application.Interfaces;
using ClinicPRO.Application.Interfaces.Repositories;

namespace Clinic_PRO.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ISpecializationRepository _specializationRepository;

        public AccountController(IAccountService accountService, ISpecializationRepository specializationRepository)
        {
            _accountService = accountService;
            _specializationRepository = specializationRepository;
        }

        // ---------- Register ----------

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Specializations = await _specializationRepository.GetAllAsync();
            return View(new RegisterDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Specializations = await _specializationRepository.GetAllAsync();
                return View(dto);
            }

            var result = await _accountService.RegisterAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                ViewBag.Specializations = await _specializationRepository.GetAllAsync();
                return View(dto);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Login");
        }

        // ---------- Login ----------

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _accountService.LoginAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(dto);
            }

            // خزّن بيانات المستخدم بالـ Session
            HttpContext.Session.SetInt32("AccountId", result.AccountId);
            HttpContext.Session.SetString("Role", result.Role);

            return RedirectToAction("Index", "Home");
        }

        // ---------- Logout ----------

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}