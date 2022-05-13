namespace API.Models
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string SiteUrl { get; set; } = string.Empty;
        public string JwtExpireDay { get; set; } = string.Empty;
    }
}