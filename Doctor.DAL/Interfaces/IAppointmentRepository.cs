using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.DataAcsess.Interfaces
{
    public interface IAppointmentRepository
    {

        Task AddAppointment(Appointment appointment);
        List<AppointmentDTO> DoctorEventsById(string doctorId);
        List<AppointmentDTO> PatientEventsById(string patientId, string doctorId);
        AppointmentDTO GetDetailsById(int Id);

        User? GetUser(string id);
        Appointment GetAppointment(int? id);
        List<Doctor.DataAcsess.Entities.Doctor> GetDoctorList();
        List<Patient> GetPatientList();
        Task SaveAsync();

        Task<PagedList<AppointmentDTOPage>> GetPagedAppointsForDoctor(AppointParams appointParams, string Id);
        Task<PagedList<AppointmentDTOPage>> GetPagedAppointsForPatient(AppointParams appointParams, string Id);
        List<AppointmentDate> GetApointmentDateList();
        Task DeleteAppointments(Appointment appointmentToDelete);

        Task<List<ReportAppointment>> GetReportList(string AdminId, DateTime startDate, DateTime endDate);
    }
}
