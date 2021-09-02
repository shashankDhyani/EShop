using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.User.DataProvider
{
    public class UserRepository : IUserRepository
    {
        private IMongoDatabase _database;
        private IMongoCollection<CreateUser> _collection => _database.GetCollection<CreateUser>("user"); 
        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }
        public async Task<UserCreated> AddUser(CreateUser user)
        {
            await _collection.InsertOneAsync(user);
            return new UserCreated() {ContactNo = user.ContactNo,
                                        EmailId = user.EmailId,
            Password = user.Password,
            Username = user.Username};
        }

        public async Task<UserCreated> GetUser(CreateUser user)
        {
           var userResult = await _collection.AsQueryable().FirstOrDefaultAsync(usr => usr.Username == user.Username);

            if (userResult == null)
                return new UserCreated();

                return new UserCreated() {
               Username = userResult.Username,
               ContactNo = userResult.ContactNo,
               EmailId = userResult.EmailId,
               Password = userResult.Password,
               UserId = userResult.UserId
           };
        }

        public async Task<UserCreated> GetUserByUsername(string name)
        {
            var userResult = await _collection.AsQueryable().FirstOrDefaultAsync(usr => usr.Username == name);

            return new UserCreated()
            {
                Username = userResult.Username,
                ContactNo = userResult.ContactNo,
                EmailId = userResult.EmailId,
                Password = userResult.Password,
                UserId = userResult.UserId
            };
        }
    }
}
