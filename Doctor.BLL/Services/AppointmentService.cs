using Doctor.BLL.Interface;
using Doctor.DataAcsess;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using Doctor.DataAcsess.Interfaces;
using Doctor.DataAcsess.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
#pragma warning disable CS8603 

namespace Doctor.BLL.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IEmailSender _emailSender;
        public AppointmentService(IAppointmentRepository appointmentRepository, IEmailSender emailSender)
        {
            _appointmentRepository = appointmentRepository;
            _emailSender = emailSender;
            
        }

        public async Task<Appointment> Add(AppointmentDTO model)
        {
            var startDate = Convert.ToDateTime(model.StartDate);
            var endDate = Convert.ToDateTime(model.StartDate).AddMinutes(Convert.ToDouble(60));
            var patient = _appointmentRepository.GetUser(model.PatientId); 
            var doctor = _appointmentRepository.GetUser(model.DoctorId);

            Appointment appointment = new Appointment()
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = startDate,
                EndDate = endDate,
                Duration = 20,
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                IsApproved = false,
                AdminId = model.AdminId
            };

            await _emailSender.SendEmailAsync(doctor.Email, "Appointment Created",
                                              $"Your appointment with {patient.Name} has been created and in pending status");
            await _emailSender.SendEmailAsync(patient.Email, "Appointment Created",
                                              $"Your appointment with {doctor.Name} has been created and in pending status");

            await _appointmentRepository.AddAppointment(appointment);
            await _appointmentRepository.SaveAsync();

            return appointment;
        }

        public List<AppointmentDTO> DoctorEventsById(string doctorId)
        {
            return _appointmentRepository.DoctorEventsById(doctorId);
        }


        public List<AppointmentDate> GetApointmentDateList()
        {
            return _appointmentRepository.GetApointmentDateList();
        }

        public List<AppointmentDTO> PatientsEventsById(string patientId, string doctorId)
        {
            return _appointmentRepository.PatientEventsById(patientId, doctorId);
        }
        
        public async Task<List<ReportAppointment>> GetAppointmentsReview(string AdminId, DateTime startDate, DateTime endDate)
        {
            return await _appointmentRepository.GetReportList(AdminId, startDate, endDate);
        }


        public async Task<AppointmentDTO> GetDetailsById(int Id)
        {
            return _appointmentRepository.GetDetailsById(Id);
        }

        public List<Doctor.DataAcsess.Entities.Doctor> GetDoctorList()
        {
            var doctors = _appointmentRepository.GetDoctorList();

            return doctors;
        }
        public List<Patient> GetPatientList()
        {
            var patinents = _appointmentRepository.GetPatientList();

            return patinents;
        }

        public async Task<PagedList<AppointmentDTOPage>> GetUserAppoints(AppointParams appointParams, string Id)
        {
            if (appointParams.CurrentRole == "Doctor")
            {
                var appointments = await _appointmentRepository.GetPagedAppointsForDoctor(appointParams, Id);
                return appointments;
            }
            else if (appointParams.CurrentRole == "Patient")
            {
                var appointments = await _appointmentRepository.GetPagedAppointsForPatient(appointParams, Id);
                return appointments;
            }
            else return null;
        }

        public async Task<Appointment> EditAppointmentById(int id, AppointmentDTO model)
        {
            var appointmentToUpdate = _appointmentRepository.GetAppointment(id);
            var startDate = Convert.ToDateTime(model.StartDate);
            var endDate = Convert.ToDateTime(model.StartDate).AddMinutes(Convert.ToDouble(60));

            if (appointmentToUpdate == null)
            {
                return appointmentToUpdate;
            }

            appointmentToUpdate.Title = model.Title;
            appointmentToUpdate.Description = model.Description;
            appointmentToUpdate.StartDate = startDate;
            appointmentToUpdate.EndDate = endDate;
            appointmentToUpdate.Duration = 20;
            appointmentToUpdate.DoctorId = model.DoctorId;
            appointmentToUpdate.PatientId = model.PatientId;
            appointmentToUpdate.IsApproved = false;
            appointmentToUpdate.AdminId = model.AdminId;

            await _appointmentRepository.SaveAsync();

            return appointmentToUpdate;
        }

        public async Task<Appointment> ApproveAppointmentById(int id, bool status)
        {
            var appointmentToUpdate = _appointmentRepository.GetAppointment(id);

            if (appointmentToUpdate == null)
            {
                return appointmentToUpdate;
            }

            appointmentToUpdate.IsApproved = status;
            var patient = _appointmentRepository.GetUser(appointmentToUpdate.PatientId);
            var doctor = _appointmentRepository.GetUser(appointmentToUpdate.DoctorId);


            if (status == true)
            {
                await _emailSender.SendEmailAsync(doctor.Email, "Appointment Approved",
                                           $"You have approved appointment #{appointmentToUpdate.Id} with " +
                                           $"{patient.Name}");
                await _emailSender.SendEmailAsync(patient.Email, "Appointment Approved",
                                                  $"Your appointment #{appointmentToUpdate.Id} with " +
                                                  $"{doctor.Name} has been approved");
            }
            else if (status == false)
            {
                await _emailSender.SendEmailAsync(doctor.Email, "Appointment Cancelled",
                                                            $"You have cancelled appointment #{appointmentToUpdate.Id} with " +
                                                            $"{patient.Name}");
                await _emailSender.SendEmailAsync(patient.Email, "Appointment Cancelled",
                                                  $"Your appointment #{appointmentToUpdate.Id} with " +
                                                  $"{doctor.Name} has been cancelled");
            }

            await _appointmentRepository.SaveAsync();

            return appointmentToUpdate;
        }

        public async Task DeleteAppointmentById(int id)
        {
            var appointmentToDelete = _appointmentRepository.GetAppointment(id);
            await _appointmentRepository.DeleteAppointments(appointmentToDelete);

            var patient = _appointmentRepository.GetUser(appointmentToDelete.PatientId);
            var doctor = _appointmentRepository.GetUser(appointmentToDelete.DoctorId);

            await _emailSender.SendEmailAsync(doctor.Email, "Appointment Deleted",
                                             $"Your appointment #{appointmentToDelete.Id} with " +
                                             $"{patient.Name} has been deleted");
            await _emailSender.SendEmailAsync(patient.Email, "Appointment Deleted",
                                              $"Your appointment #{appointmentToDelete.Id} with " +
                                              $"{doctor.Name} has been deleted");

            await _appointmentRepository.SaveAsync();
        }
    }
}
