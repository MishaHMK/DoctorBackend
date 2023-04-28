using System.Security.Claims;

namespace Doctor.BLL.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        //public static string GetUsername(this ClaimsPrincipal user)
        //{
        //    return user.FindFirst(ClaimTypes.Name)?.Value;
        //}

        public static string? GetNameId(this ClaimsPrincipal user)
        {
            return user.FindFirst("NameIdentifier")?.Value;
        }

        public static string? GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst("UserName")?.Value;
        }
    }
}
