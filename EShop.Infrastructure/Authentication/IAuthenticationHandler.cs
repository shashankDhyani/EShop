using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using EShop.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Authentication
{
    public interface IAuthenticationHandler
    {
        JwtAuthToken Create(string userId);
    }
}
