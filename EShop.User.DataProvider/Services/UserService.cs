using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using EShop.Infrastructure.Security;
using System;
using System.Threading.Tasks;

namespace EShop.User.DataProvider
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        private IEncrypter _encrypter;
        public UserService(IUserRepository userRepository, IEncrypter encrypter)
        {
            _repository = userRepository;
            _encrypter = encrypter;
        }
        public async Task<UserCreated> AddUser(CreateUser user)
        {
            var usr = await _repository.GetUser(user);
            
            if(usr.UserId == null)
            {
                user.SetPassword(_encrypter);
            }
            else
            {
                //throw new Exception("Username already exists.");
            }

            return await _repository.AddUser(user);
        }

        public async Task<UserCreated> GetUser(CreateUser user)
        {
            return await _repository.GetUser(user);
        }

        public async Task<UserCreated> GetUserByusername(string name)
        {
            return await _repository.GetUserByUsername(name);
        }
    }
}
