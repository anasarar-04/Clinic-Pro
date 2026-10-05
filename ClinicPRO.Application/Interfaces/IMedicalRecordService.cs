using ClinicPRO.Application.DTOs;

namespace ClinicPRO.Application.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<List<MedicineSearchResultDto>> SearchMedicinesAsync(string term);
        Task<PrescriptionViewDto?> GetPrescriptionDetailsAsync(int appointmentId);
        Task<(bool Success, string Message)> WriteMedicalRecordAsync(WriteMedicalRecordDto dto);
    }
}