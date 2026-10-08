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

            // افصل أي موعد ملغى قديم ما زال مربوط بنفس الوقت
            var oldLinked = await _unitOfWork.Appointments.GetBySlotIdAsync(dto.SlotId);
            if (oldLinked != null)
            {
                if (oldLinked.Status != Domain.Enums.AppointmentStatus.Cancelled)
                    return (false, "عذرا، هذا الوقت غير متاح");

                oldLinked.SlotId = null;
                _unitOfWork.Appointments.Update(oldLinked);
                await _unitOfWork.SaveChangesAsync();
            }

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
        public async Task<(bool Success, string Message)> AddSlotAsync(AddSlotDto dto)
        {
            if (dto.StartTime >= dto.EndTime)
                return (false, "وقت البداية يجب أن يكون قبل وقت النهاية");

            var slot = new AvailableSlot
            {
                DoctorId = dto.DoctorId,
                SlotDate = dto.SlotDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsBooked = false
            };

            await _unitOfWork.AvailableSlots.AddAsync(slot);
            await _unitOfWork.SaveChangesAsync();

            return (true, "تم إضافة الموعد بنجاح");
        }

        public async Task<List<DoctorSlotDto>> GetDoctorSlotsAsync(int doctorId)
        {
            var slots = await _unitOfWork.AvailableSlots.GetAllByDoctorIdAsync(doctorId);

            return slots.Select(s => new DoctorSlotDto
            {
                SlotId = s.SlotId,
                SlotDate = s.SlotDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsBooked = s.IsBooked
            }).ToList();
        }
        public async Task<(bool Success, string Message)> CancelAppointmentAsync(int appointmentId, int patientId)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);

            if (appointment == null)
                return (false, "الموعد غير موجود");

            if (appointment.PatientId != patientId)
                return (false, "لا يمكنك إلغاء موعد لا يخصك");

            if (appointment.Status == Domain.Enums.AppointmentStatus.Completed)
                return (false, "لا يمكن إلغاء موعد مكتمل بالفعل");

            if (appointment.Status == Domain.Enums.AppointmentStatus.Cancelled)
                return (false, "هذا الموعد ملغى بالفعل");

            // حرر الوقت حتى يصير متاح لمريض آخر
            if (appointment.Slot != null)
            {
                appointment.Slot.IsBooked = false;
                _unitOfWork.AvailableSlots.Update(appointment.Slot);
            }

            // افصل الموعد الملغى عن الوقت
            appointment.Status = Domain.Enums.AppointmentStatus.Cancelled;
            appointment.SlotId = null;
            appointment.Slot = null;
            _unitOfWork.Appointments.Update(appointment);

            await _unitOfWork.SaveChangesAsync();

            return (true, "تم إلغاء الموعد بنجاح");
        }

        public async Task<(bool Success, string Message)> DeleteSlotAsync(int slotId, int doctorId)
        {
            var slot = await _unitOfWork.AvailableSlots.GetByIdAsync(slotId);

            if (slot == null)
                return (false, "الموعد غير موجود");

            if (slot.DoctorId != doctorId)
                return (false, "لا يمكنك حذف موعد لا يخصك");

            if (slot.IsBooked)
                return (false, "لا يمكن حذف موعد محجوز، يجب إلغاء الحجز أولا");

            // نظّف أي موعد ملغى قديم ما زال مربوط بهاد الوقت
            var linked = await _unitOfWork.Appointments.GetBySlotIdAsync(slotId);
            if (linked != null)
            {
                if (linked.Status != Domain.Enums.AppointmentStatus.Cancelled)
                    return (false, "هذا الوقت مرتبط بموعد فعّال");

                linked.SlotId = null;
                _unitOfWork.Appointments.Update(linked);
                await _unitOfWork.SaveChangesAsync();
            }

            _unitOfWork.AvailableSlots.Delete(slot);
            await _unitOfWork.SaveChangesAsync();

            return (true, "تم حذف الموعد بنجاح");
        }
    }

}