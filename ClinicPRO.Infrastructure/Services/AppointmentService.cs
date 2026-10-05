using ClinicPRO.Application.DTOs;
using ClinicPRO.Application.Interfaces;
using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DoctorListDto>> GetAllDoctorsAsync()
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync();

            return doctors.Select(d => new DoctorListDto
            {
                DoctorId = d.DoctorId,
                FullName = d.Account.FullName,
                SpecializationName = d.Specialization.Name,
                Bio = d.Bio
            }).ToList();
        }

        public async Task<List<DoctorListDto>> GetDoctorsBySpecializationAsync(int specializationId)
        {
            var doctors = await _unitOfWork.Doctors.GetBySpecializationIdAsync(specializationId);

            return doctors.Select(d => new DoctorListDto
            {
                DoctorId = d.DoctorId,
                FullName = d.Account.FullName,
                SpecializationName = d.Specialization.Name,
                Bio = d.Bio
            }).ToList();
        }

        public async Task<List<AvailableSlotDto>> GetAvailableSlotsAsync(int doctorId)
        {
            var slots = await _unitOfWork.AvailableSlots.GetAvailableByDoctorIdAsync(doctorId);

            return slots
                .OrderBy(s => s.SlotDate)
                .ThenBy(s => s.StartTime)
                .Select(s => new AvailableSlotDto
                {
                    SlotId = s.SlotId,
                    SlotDate = s.SlotDate,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                }).ToList();
        }

        public async Task<(bool Success, string Message)> BookAppointmentAsync(BookAppointmentDto dto)
        {
            // 1. تأكد السلوت موجود وما زال متاح
            var slot = await _unitOfWork.AvailableSlots.GetByIdAsync(dto.SlotId);

            if (slot == null)
                return (false, "الموعد غير موجود");

            if (slot.IsBooked)
                return (false, "عذرا، هذا الموعد تم حجزه للتو من مريض آخر");

            // 2. أنشئ الموعد
            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                SlotId = dto.SlotId,
                Status = Domain.Enums.AppointmentStatus.Pending,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Appointments.AddAsync(appointment);

            // 3. علّم السلوت إنه محجوز
            slot.IsBooked = true;
            _unitOfWork.AvailableSlots.Update(slot);

            // 4. احفظ كل شي مع بعض
            await _unitOfWork.SaveChangesAsync();

            return (true, "تم حجز الموعد بنجاح");
        }
        public async Task<List<PatientAppointmentDto>> GetMyAppointmentsAsync(int patientId)
        {
            var appointments = await _unitOfWork.Appointments.GetByPatientIdAsync(patientId);

            return appointments
                .OrderByDescending(a => a.Slot != null ? a.Slot.SlotDate : DateTime.MinValue)
                .Select(a => new PatientAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorName = a.Doctor.Account.FullName,
                    SpecializationName = a.Doctor.Specialization.Name,
                    SlotDate = a.Slot?.SlotDate,
                    StartTime = a.Slot?.StartTime,
                    Status = a.Status
                }).ToList();
        }
        public async Task<List<DoctorAppointmentDto>> GetDoctorAppointmentsAsync(int doctorId)
        {
            var appointments = await _unitOfWork.Appointments.GetByDoctorIdAsync(doctorId);

            return appointments
                .OrderBy(a => a.Slot != null ? a.Slot.SlotDate : DateTime.MaxValue)
                .Select(a => new DoctorAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.Patient.Account.FullName,
                    SlotDate = a.Slot?.SlotDate,
                    StartTime = a.Slot?.StartTime,
                    Status = a.Status
                }).ToList();
        }
    }

}