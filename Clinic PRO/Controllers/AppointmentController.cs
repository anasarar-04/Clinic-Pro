using ClinicPRO.Application.DTOs;
using ClinicPRO.Application.Interfaces;
using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Infrastructure.Repositories;
using ClinicPRO.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;


namespace Clinic_PRO.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMedicalRecordService _medicalRecordService;


        public AppointmentController(
    IAppointmentService appointmentService,
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository,
    IMedicalRecordService medicalRecordService,
    IAppointmentRepository appointmentRepository)
        {
            _appointmentService = appointmentService;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _medicalRecordService = medicalRecordService;
            _appointmentRepository = appointmentRepository;
        }

        // ---------- عرض قائمة الدكاترة ----------
        public async Task<IActionResult> ChooseDoctor()
        {
            // تأكد المستخدم مسجل دخول
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var doctors = await _appointmentService.GetAllDoctorsAsync();
            return View(doctors);
        }

        // ---------- عرض الأوقات المتاحة لدكتور معين ----------
        public async Task<IActionResult> ChooseSlot(int doctorId)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var slots = await _appointmentService.GetAvailableSlotsAsync(doctorId);
            ViewBag.DoctorId = doctorId;
            return View(slots);
        }

        // ---------- تنفيذ الحجز ----------
        [HttpPost]
        public async Task<IActionResult> Book(int doctorId, int slotId)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var patient = await _patientRepository.GetByAccountIdAsync(accountId.Value);
            if (patient == null)
                return RedirectToAction("Login", "Account");

            var dto = new BookAppointmentDto
            {
                PatientId = patient.PatientId,
                DoctorId = doctorId,
                SlotId = slotId
            };

            var result = await _appointmentService.BookAppointmentAsync(dto);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            if (!result.Success)
                return RedirectToAction("ChooseSlot", new { doctorId });

            return RedirectToAction("MyAppointments");
        }
        public async Task<IActionResult> MyAppointments()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var patient = await _patientRepository.GetByAccountIdAsync(accountId.Value);
            if (patient == null)
                return RedirectToAction("Login", "Account");

            var appointments = await _appointmentService.GetMyAppointmentsAsync(patient.PatientId);
            return View(appointments);
        }
        public async Task<IActionResult> DoctorAppointments()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var doctor = await _doctorRepository.GetByAccountIdAsync(accountId.Value);
            if (doctor == null)
                return RedirectToAction("Login", "Account");

            var appointments = await _appointmentService.GetDoctorAppointmentsAsync(doctor.DoctorId);
            return View(appointments);
        }
        // ---------- عرض نموذج كتابة السجل الطبي ----------
        [HttpGet]
        public async Task<IActionResult> WriteRecord(int appointmentId)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                return RedirectToAction("DoctorAppointments");

            ViewBag.AppointmentId = appointment.AppointmentId;
            ViewBag.PatientId = appointment.PatientId;
            ViewBag.DoctorId = appointment.DoctorId;
            ViewBag.PatientName = appointment.Patient.Account.FullName;

            return View();
        }
        

        // ---------- حفظ السجل الطبي والوصفة ----------
        [HttpPost]
        public async Task<IActionResult> WriteRecord(WriteMedicalRecordDto dto)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var result = await _medicalRecordService.WriteMedicalRecordAsync(dto);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction("DoctorAppointments");
        }

        // ---------- API داخلي للـ autocomplete (يرجع JSON) ----------
        [HttpGet]
        public async Task<IActionResult> SearchMedicines(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
                return Json(new List<object>());

            var results = await _medicalRecordService.SearchMedicinesAsync(term);
            return Json(results);
        }
        [HttpGet]
        public async Task<IActionResult> ViewPrescription(int appointmentId)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var details = await _medicalRecordService.GetPrescriptionDetailsAsync(appointmentId);

            if (details == null)
            {
                TempData["ErrorMessage"] = "لا يوجد سجل طبي لهذا الموعد بعد";
                return RedirectToAction("MyAppointments");
            }

            return View(details);
        }
        [HttpGet]
        public async Task<IActionResult> MySlots()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var doctor = await _doctorRepository.GetByAccountIdAsync(accountId.Value);
            if (doctor == null)
                return RedirectToAction("Login", "Account");

            ViewBag.DoctorId = doctor.DoctorId;
            var slots = await _appointmentService.GetDoctorSlotsAsync(doctor.DoctorId);
            return View(slots);
        }

        [HttpPost]
        public async Task<IActionResult> AddSlot(AddSlotDto dto)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var result = await _appointmentService.AddSlotAsync(dto);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction("MySlots");
        }
        [HttpPost]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var patient = await _patientRepository.GetByAccountIdAsync(accountId.Value);
            if (patient == null)
                return RedirectToAction("Login", "Account");

            var result = await _appointmentService.CancelAppointmentAsync(appointmentId, patient.PatientId);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction("MyAppointments");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSlot(int slotId)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            var doctor = await _doctorRepository.GetByAccountIdAsync(accountId.Value);
            if (doctor == null)
                return RedirectToAction("Login", "Account");

            var result = await _appointmentService.DeleteSlotAsync(slotId, doctor.DoctorId);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction("MySlots");
        }

    }
}