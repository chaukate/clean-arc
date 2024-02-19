namespace App.Infrastructure.Common.Options
{
    public class JwtOptions
    {
        public const string SECTION_NAME = "JWT";
        public string Key { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int RefreshInMinutes { get; set; }
        public int ExpireInMinutes { get; set; }
    }
}
