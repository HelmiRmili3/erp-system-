
namespace Backend.Application.Common.Settings;
public class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiryInMinutes { get; set; }
    //public int RefreshTokenExpirationInDays { get; set; }
    //public bool ValidateIssuerSigningKey { get; set; }
    //public bool ValidateIssuer { get; set; }
    //public bool ValidateAudience { get; set; }
}
