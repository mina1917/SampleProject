using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SampleProject.Framework.Utilities;
using System.Security.Claims;

namespace SampleProject.Framework
{
    public static class Claims
    {
        public static string Sub = "sub";
        public static string Role = "role";
        public static string RoleCode = "roleCode";
    }

    public interface IClaimHelper
    {
        string GetToken();
        string? GetUserId();
        List<Guid> GetAllRoleIds();
    }

    public class ClaimHelper : IClaimHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal? GetUserInfo()
        {
            return _httpContextAccessor.HttpContext?.User;
        }

        public string? GetUserId()
        {
            if (_httpContextAccessor?.HttpContext != null)
            {
                //todo
                var userId = _httpContextAccessor.HttpContext.User.Identity?.FindFirstValue(ClaimTypes.NameIdentifier);
                return userId;
            }
            return null;
        }

        private List<Claim> GetUserClaims()
        {
            return _httpContextAccessor.HttpContext.User.Claims.ToList();
        }

        public string GetToken()
        {
            return _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];
        }

        public List<Guid> GetAllRoleIds()
        {
            var claims = GetUserClaims();
            var roleIds = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => new Guid(x.Value)).ToList();
            return roleIds;
        }
    }
}