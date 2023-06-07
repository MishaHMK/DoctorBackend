using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using Doctor.DataAcsess.Interfaces;
using DoctorWebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Doctor.DataAcsess.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext db) : base(db)
        {

        }
        public async Task AddAppointment(Appointment appointment)
        {
            await dbSet.AddAsync(appointment);
        }

        public List<AppointmentDTO> DoctorEventsById(string doctorId)
        {
            var events = _db.Appointments.Where(x => x.DoctorId == doctorId).ToList()
                                   .Select(c => new AppointmentDTO()
                                   {
                                       Id = c.Id,
                                       Title = c.Title,
                                       Description = c.Description,
                                       StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                       EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                       Duration = c.Duration,
                                       IsApproved = c.IsApproved
                                   }).ToList();

            return events;
        }

        public List<AppointmentDTO> PatientEventsById(string patientId, string doctorId)
        {
            var events = _db.Appointments.Where(x => x.PatientId == patientId && x.DoctorId == doctorId).ToList()
                                  .Select(c => new AppointmentDTO()
                                  {
                                      Id = c.Id,
                                      Title = c.Title,
                                      Description = c.Description,
                                      StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                      EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                      Duration = c.Duration,
                                      IsApproved = c.IsApproved
                                  }).ToList();

            return events;
        }

        public AppointmentDTO GetDetailsById(int Id)
        {
            var details = _db.Appointments.Where(x => x.Id == Id)
                                  .Select(c => new AppointmentDTO()
                                  {
                                      Id = c.Id,
                                      Title = c.Title,
                                      Description = c.Description,
                                      StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                      EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                      Duration = c.Duration,
                                      IsApproved = c.IsApproved,
                                      PatientId = c.PatientId,
                                      DoctorId = c.DoctorId,
                                      PatientName = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                                      DoctorName = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault()
                                  }).SingleOrDefault();

            return details;
        }


        public async Task<PagedList<AppointmentDTOPage>> GetPagedAppointsForDoctor(AppointParams appointParams, string Id)

        {
             var query = _db.Appointments.Where(x => x.DoctorId == Id)
                                  .Select(c => new AppointmentDTOPage()
                                  {
                                      Id = c.Id,
                                      Title = c.Title,
                                      Description = c.Description,
                                      StartDate = c.StartDate,
                                      EndDate = c.EndDate,
                                      Duration = c.Duration,
                                      IsApproved = c.IsApproved,
                                      PatientId = c.PatientId,
                                      DoctorId = c.DoctorId,
                                      PatientSurname = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Surname).FirstOrDefault(),
                                      PatientName = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                                      PatientFathername = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.FatherName).FirstOrDefault(),
                                      DoctorSurname = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Surname).FirstOrDefault(),
                                      DoctorName = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault(),
                                      DoctorFathername = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.FatherName).FirstOrDefault(),
                                      OfficeNumber = _db.DoctorUsers.Where(x => x.UserId == c.DoctorId).Select(x => x.OfficeNumber).FirstOrDefault()
                                  });

      
            if (appointParams.Approved != null)
            {
                query = query.Where(a => a.IsApproved == appointParams.Approved);
            }

            if (appointParams.Sort != null && appointParams.Sort != "")
            {
                query = appointParams.Sort switch
                {
                    "date" => appointParams.OrderBy switch
                    {
                        "ascend" => query.OrderBy(a => a.StartDate),
                        "descend" => query.OrderByDescending(a => a.StartDate),
                    },
                };
            }
            return await PagedList<AppointmentDTOPage>.CreateAsync(query.OrderByDescending(a => a.StartDate),
                                                           appointParams.PageNumber, appointParams.PageSize, query.Count());
        }



        public async Task<PagedList<AppointmentDTOPage>> GetPagedAppointsForPatient(AppointParams appointParams, string Id)

        {
            var query = _db.Appointments.Where(x => x.PatientId == Id)
                                  .Select(c => new AppointmentDTOPage()
                                  {
                                      Id = c.Id,
                                      Title = c.Title,
                                      Description = c.Description,
                                      StartDate = c.StartDate,
                                      EndDate = c.EndDate,
                                      Duration = c.Duration,
                                      IsApproved = c.IsApproved,
                                      PatientId = c.PatientId,
                                      DoctorId = c.DoctorId,
                                      PatientSurname = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Surname).FirstOrDefault(),
                                      PatientName = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                                      PatientFathername = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.FatherName).FirstOrDefault(),
                                      DoctorSurname = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Surname).FirstOrDefault(),
                                      DoctorName = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault(),
                                      DoctorFathername = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.FatherName).FirstOrDefault(),
                                      OfficeNumber = _db.DoctorUsers.Where(x => x.UserId == c.DoctorId).Select(x => x.OfficeNumber).FirstOrDefault()
                                  });


            if (appointParams.Approved != null)
            {
                query = query.Where(a => a.IsApproved == appointParams.Approved);
            }

            if (appointParams.Sort != null && appointParams.Sort != "")
            {
                query = appointParams.Sort switch
                {
                    "date" => appointParams.OrderBy switch
                    {
                        "ascend" => query.OrderBy(a => a.StartDate),
                        "descend" => query.OrderByDescending(a => a.StartDate),
                    },
                };
            }
            return await PagedList<AppointmentDTOPage>.CreateAsync(query.OrderByDescending(a => a.StartDate), 
                                                               appointParams.PageNumber, appointParams.PageSize, query.Count());
        }


        public async Task<List<ReportAppointment>> GetReportList(string AdminId, DateTime startDate, DateTime endDate)
        {
            var reportAppoints = _db.Appointments.Where(x => x.StartDate >= startDate && x.EndDate <= endDate)
                                 .Select(c => new ReportAppointment()
                                 {
                                     Id = c.Id,
                                     PatientName = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Surname).FirstOrDefault() + " " +
                                                   _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault() + " " +
                                                   _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.FatherName).FirstOrDefault(),

                                     DoctorName = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Surname).FirstOrDefault() + " " +
                                                 _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault() + " " +
                                                 _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.FatherName).FirstOrDefault(),
                                     Title = c.Title,
                                     Description = c.Description,
                                     StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm"),
                                     EndDate = c.StartDate.ToString("yyyy-MM-dd HH:mm"),
                                     IsApproved = c.IsApproved
                                 }).ToList();

            return reportAppoints;
        }



        public List<Doctor.DataAcsess.Entities.Doctor> GetDoctorList()
        {
            var doctors = (from user in _db.Users
            join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name == Roles.Doctor) on userRoles.RoleId equals roles.Id
                           select new Doctor.DataAcsess.Entities.Doctor
                           {
                               Id = user.Id,
                               Name = user.Name,
                               Fathername = user.FatherName,
                               Surname = user.Surname
                           }

                           )
                           .OrderBy( x => x.Surname)    
                           .ToList();

            return doctors;
        }


        public List<AppointmentDate> GetApointmentDateList()
        {
            var appointmentDates = _db.Appointments
                                  .Select(c => new AppointmentDate()
                                  {
                                      StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm"),     
                                  }).ToList();

            return appointmentDates;
        }


        public List<Patient> GetPatientList()
        {
            var patinents = (from user in _db.Users
                             join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                             join roles in _db.Roles.Where(x => x.Name == Roles.Patient) on userRoles.RoleId equals roles.Id
                             select new Patient
                             {
                                 Id = user.Id,
                                 Name = user.Name,
                                 Fathername = user.FatherName,
                                 Surname = user.Surname
                             }

                           )
                            .OrderBy(x => x.Surname)
                            .ToList();

            return patinents;
        }

        public Appointment GetAppointment(int? id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            return appointment;
        }

        public async Task DeleteAppointments(Appointment appointmentToDelete)
        {
            _db.Appointments.Remove(appointmentToDelete);
        }


        public User? GetUser(string id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return user;
            }
            return null;
        }



        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
