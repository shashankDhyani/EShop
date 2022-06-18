using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Infrastructure.Common
{
    public interface IIdempotent
    {
        Task<bool> IsNewMessage(Guid? MessageId);
        Task AddMessage(string ConsumerName, Guid? MessageId);
    }
}
