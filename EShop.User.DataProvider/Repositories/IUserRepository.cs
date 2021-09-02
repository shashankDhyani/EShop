using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using System.Threading.Tasks;

namespace EShop.User.DataProvider
{
    public interface IUserRepository
    {
        Task<UserCreated> AddUser(CreateUser user);
        Task<UserCreated> GetUser(CreateUser user);

        Task<UserCreated> GetUserByUsername(string name);
    }
}
