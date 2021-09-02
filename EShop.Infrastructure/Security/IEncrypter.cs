using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Security
{
    public interface IEncrypter
    {
        string GetSalt();
        string GetHash(string value, string salt);
    }
}
