using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.BLL.Interface
{
    public interface IAppointmentService
    {
        public List<DataAcsess.Entities.Doctor> GetDoctorList();
        public List<Patient> GetPatientList();
        public Task<Appointment> Add(AppointmentDTO model);
        public Task<Appointment> ApproveAppointmentById(int id, bool status);
        public List<AppointmentDTO> DoctorEventsById(string doctorId);
        public List<AppointmentDTO> PatientsEventsById(string patientId, string doctorId);
        public List<AppointmentDate> GetApointmentDateList();
        public Task DeleteAppointmentById(int id);
        public Task<AppointmentDTO> GetDetailsById(int Id);
        Task<PagedList<AppointmentDTOPage>> GetUserAppoints(AppointParams appointParams, string Id);
        public Task<Appointment> EditAppointmentById(int id, AppointmentDTO model);

        public Task<List<ReportAppointment>> GetAppointmentsReview(string AdminId, DateTime startDate, DateTime endDate);
    }
}
