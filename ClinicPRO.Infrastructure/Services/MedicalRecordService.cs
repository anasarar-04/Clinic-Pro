using ClinicPRO.Application.DTOs;
using ClinicPRO.Application.Interfaces;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Domain.Enums;

namespace ClinicPRO.Infrastructure.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicalRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MedicineSearchResultDto>> SearchMedicinesAsync(string term)
        {
            var medicines = await _unitOfWork.Medicines.SearchByNameAsync(term);

            return medicines.Select(m => new MedicineSearchResultDto
            {
                MedicineId = m.MedicineId,
                Name = m.Name
            }).ToList();
        }

        public async Task<(bool Success, string Message)> WriteMedicalRecordAsync(WriteMedicalRecordDto dto)
        {
            // 1. أنشئ السجل الطبي
            var record = new MedicalRecord
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentId = dto.AppointmentId,
                VisitDate = DateTime.Now,
                Diagnosis = dto.Diagnosis,
                Report = dto.Report
            };

            await _unitOfWork.MedicalRecords.AddAsync(record);
            await _unitOfWork.SaveChangesAsync();   // نحفظ حتى ياخذ record.RecordId قيمته

            // 2. لو في أدوية، أنشئ الوصفة والعناصر
            if (dto.Items.Any())
            {
                var prescription = new Prescription
                {
                    RecordId = record.RecordId,
                    IssueDate = DateTime.Now
                };

                await _unitOfWork.Prescriptions.AddAsync(prescription);
                await _unitOfWork.SaveChangesAsync();   // نحفظ حتى ياخذ prescription.PrescriptionId قيمته

                foreach (var itemDto in dto.Items)
                {
                    var item = new PrescriptionItem
                    {
                        PrescriptionId = prescription.PrescriptionId,
                        MedicineId = itemDto.MedicineId,
                        Dosage = itemDto.Dosage,
                        DurationDays = itemDto.DurationDays,
                        Instructions = itemDto.Instructions
                    };

                    await _unitOfWork.PrescriptionItems.AddAsync(item);
                    await _unitOfWork.SaveChangesAsync();   // نحفظ حتى ياخذ item.ItemId قيمته

                    // أضف أوقات التذكير المرتبطة بهاد الدواء
                    foreach (var timeStr in itemDto.ReminderTimes)
                    {
                        if (TimeSpan.TryParse(timeStr, out var parsedTime))
                        {
                            var medicationTime = new MedicationTime
                            {
                                ItemId = item.ItemId,
                                ReminderTime = parsedTime
                            };
                            await _unitOfWork.MedicationTimes.AddAsync(medicationTime);
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync();
            }

            // 3. حدّث حالة الموعد إلى "مكتمل"
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(dto.AppointmentId);
            if (appointment != null)
            {
                appointment.Status = AppointmentStatus.Completed;
                _unitOfWork.Appointments.Update(appointment);
                await _unitOfWork.SaveChangesAsync();
            }

            return (true, "تم حفظ السجل الطبي والوصفة بنجاح");
        }
        public async Task<PrescriptionViewDto?> GetPrescriptionDetailsAsync(int appointmentId)
        {
            var record = await _unitOfWork.MedicalRecords.GetByAppointmentIdAsync(appointmentId);

            if (record == null)
                return null;

            var dto = new PrescriptionViewDto
            {
                DoctorName = record.Doctor.Account.FullName,
                SpecializationName = record.Doctor.Specialization.Name,
                VisitDate = record.VisitDate,
                Diagnosis = record.Diagnosis,
                Report = record.Report
            };

            if (record.Prescription != null)
            {
                dto.Items = record.Prescription.Items.Select(i => new PrescriptionItemViewDto
                {
                    MedicineName = i.Medicine.Name,
                    Dosage = i.Dosage,
                    DurationDays = i.DurationDays,
                    Instructions = i.Instructions,
                    ReminderTimes = i.MedicationTimes.Select(mt => mt.ReminderTime.ToString(@"hh\:mm")).ToList()
                }).ToList();
            }

            return dto;
        }
    }
}