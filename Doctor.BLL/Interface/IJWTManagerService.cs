using Doctor.BLL.Helper;
using Doctor.DataAcsess.Entities;


namespace Doctor.BLL.Interface
{
    public interface IJWTService
    {
        Token Authenticate(string id, string name, IEnumerable<string> roles);
    }
}
