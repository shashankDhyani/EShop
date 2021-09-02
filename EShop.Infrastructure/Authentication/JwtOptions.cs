using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Authentication
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int ExpiryMinutes { get; set; }
        public string Issuer { get; set; }  
    }
}
