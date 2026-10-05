using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> GetByIdAsync(int appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.Account)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Account)
                .Include(a => a.Slot)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Account)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Specialization)
                .Include(a => a.Slot)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.Account)
                .Include(a => a.Slot)
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetUpcomingByPatientIdAsync(int patientId, DateTime fromDate)
        {
            return await _context.Appointments
                .Include(a => a.Slot)
                .Where(a => a.PatientId == patientId && a.Slot != null && a.Slot.SlotDate >= fromDate)
                .ToListAsync();
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        public void Update(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }

        public void Delete(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
        }
    }
}