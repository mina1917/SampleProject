using Microsoft.AspNetCore.Identity;

namespace SampleProject.WebApi.Helpers
{
    public static class IdentityResultHelper
    {
        public static string? CreateMessage(IdentityResult result)
        {
            if (!result.Succeeded)
                return string.Join('\n', result.Errors.Select(t => t.Description));
            return null;
        }
    }
}
