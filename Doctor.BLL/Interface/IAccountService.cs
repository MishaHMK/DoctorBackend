using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using DoctorWebApi.Helpers;

namespace Doctor.BLL.Interface
{
    public interface IAccountService
    {
        public Task<PagedList<UserRateDTO>> GetUsersAsync(UserParams userParams);
        public Task<User> GetUserAsync(string name);
        public Task<List<string>> GetRoles();
        public Task<List<string>> GetTimes();
        public Task<List<string>> GetSpecialities();
        public Task<UserDTO> GetUserDTOById(string id);
        public Task<User> GetUserById(string id);
        public Task<List<User>> GetAllUsers();
        public Task<string> GetUsername(string name);
        public Task UpdateEditedUserMessages(string id, string currentName, string newName);
        public Task SaveAllAsync();
        public Task AddDoc(DoctorUser newDoc);
        public Task<DoctorUser> GetDoctorById(string id);
    }
}
