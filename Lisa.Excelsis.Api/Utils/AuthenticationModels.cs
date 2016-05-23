using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;

namespace Lisa.Excelsis.Api
{
    public class TokenAuthOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }

    public class AuthRequest
    {
        [Required]
        public string userName { get; set; }
    }

    public class TokenResponse
    {
        public string user { get; set; }
        public string token { get; set; }
        public DateTime? tokenExpires { get; set; }
    }
}
