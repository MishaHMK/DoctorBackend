using AutoMapper;
using Doctor.BLL.Interface;
using Doctor.DataAcsess;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using DoctorWebApi.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace DoctorWebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }

        public async Task<PagedList<UserRateDTO>> GetUsersAsync(UserParams userParams)
        {

            var query = (from user in _db.Users
                         join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                         join roles in _db.Roles.Where(x => x.Name == Roles.Doctor) on userRoles.RoleId equals roles.Id
                         select new UserRateDTO
                         {
                             Id = user.Id,
                             UserName = user.UserName,
                             Email = user.Email,
                             EmailConfirmed = user.EmailConfirmed,
                             PasswordHash = user.PasswordHash,
                             SecurityStamp = user.SecurityStamp,
                             PhoneNumber = user.PhoneNumber,
                             Name = user.Name,
                             Surname = user.Surname,
                             Fathername = user.FatherName,
                             Introduction = user.Introduction,
                             LastActive = user.LastActive,
                             RegisteredOn = user.RegisteredOn,
                             Speciality = user.Speciality,
                             AverageRate = _db.Reviews.Where(x => x.DoctorId == user.Id).Select(x => x.Score).Average()
                         }
                         );


            if (userParams.SearchName != null && userParams.SearchName != "")
            {
                query = query.Where(u => u.Name.Contains(userParams.SearchName));
            }

            if (userParams.Speciality != null && (userParams.Speciality != "" && userParams.Speciality != "Any"))
            {
                query = query.Where(u => u.Speciality == userParams.Speciality);
            }

            if (userParams.Sort != null && userParams.Sort != "")
            {
                query = userParams.Sort switch
                {
                    "name" => userParams.OrderBy switch
                    {
                        "ascend" => query.OrderBy(u => u.Name),
                        "descend" => query.OrderByDescending(u => u.Name),
                      //  _ => throw new NotImplementedException()
                    },
                   // _ => throw new NotImplementedException()
                };
            }


            return await PagedList<UserRateDTO>.CreateAsync(query, userParams.PageNumber, userParams.PageSize, query.Count());
        }

        public async Task<User> GetUserAsync(string name)
        {
            var result = await _db.Users.Where(u => u.Name== name).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<string>> GetRoles()
        {
            var roles = new[]
            {
                Roles.Admin,
                Roles.Doctor,
                Roles.Patient
            };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var idRole = new IdentityRole(role);
                    await _roleManager.CreateAsync(idRole);
                }
            }

            var identityRoles = await _roleManager.Roles.ToListAsync();

            var roleList = new List<string>();

            foreach (var role in identityRoles)
            {
                roleList.Add(role.Name);
            }

            return roleList;
        }


        public async Task<List<string>> GetTimes()
        {
            var times = Timestamps.GetTimesForDropDown();
            var timeList = new List<string>();

            foreach (var time in times)
            {
                timeList.Add(time.Text);
            }

            return timeList;
        }

        public async Task<List<string>> GetSpecialities()
        {
            var specs = Specialities.GetSpecialitiesForDropDown();
            var list = new List<string>();

            foreach (var s in specs)
            {
                list.Add(s.Text);
            }

            return list;
        }

        public async Task<UserDTO> GetUserDTOById(string id)
        {
            var user = _db.Users.Where(x => x.Id == id).Select(c => new UserDTO()
            {
                Id = c.Id,
                Name = c.Name,
                Fathername = c.FatherName,
                Surname = c.Surname,
                Email = c.Email,
                Introduction = c.Introduction,
                Speciality = c.Speciality
            }).SingleOrDefault();

            return user;
        }


        public async Task UpdateEditedUserMessages(string id, string currentName, string newName)
        {
            if (currentName != newName)
            {
                var SendedMessages = _db.Messages.Where(x => x.SenderId == id);
                var RecievedMessages = _db.Messages.Where(x => x.RecipientId == id);    

                foreach (var msg in SendedMessages) 
                {
                    msg.SenderUserName = newName;
                }

                foreach (var msg in RecievedMessages)
                {
                    msg.RecipientUserName = newName;
                }
            }
        }


        public async Task<List<User>> GetAllUsers()
        {
            var userList = await _db.Users.ToListAsync();
            return userList;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<string> GetUsername(string name)
        {
            var userName = await _db.Users.Where(u => u.Name == name).Select(x => x.Name).FirstOrDefaultAsync();
            return userName;
        }

        public async Task SaveAllAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
