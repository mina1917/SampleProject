namespace SampleProject.Framework
{
    public class SiteSettings
    {
        public string Domain { get; set; }
        public JwtBearer JwtBearer { get; set; }
        public IdentitySettings IdentitySettings { get; set; }
        public SMSSettings SMSSettings { get; set; }
    }

    public class IdentitySettings
    {
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonAlphanumeric { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool RequireUniqueEmail { get; set; }
    }
    public class JwtBearer
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
        public bool RequireHttpsMetadata { get; set; }
    }

    public class SMSSettings
    {
        public string BaseURL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

