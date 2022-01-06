using EShop.Infrastructure.Command.Wallet;
using MongoDB.Driver;
using System;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace EShop.Wallet.DataProvider.Repository
{
    using EShop.Infrastructure.Wallet;
    public class WalletRepository : IWalletRepository
    {
        private IMongoDatabase _database;
        private IMongoCollection<Wallet> _Collection;
        public WalletRepository(IMongoDatabase database)
        {
            _database = database;
            _Collection = database.GetCollection<Wallet>("wallet", null);
        }
        public async Task<bool> AddFunds(AddFunds funds)
        {
            var userWallet = await GetWallet(funds.UserId);
            userWallet.Amount += funds.CreditAmount;

            var filter = Builders<Wallet>.Filter.Eq("userId", funds.UserId);
            var update = Builders<Wallet>.Update.Set("amount", userWallet.Amount);

            var options = new UpdateOptions() {
                IsUpsert = true
            };

            await _Collection.UpdateOneAsync(filter, update, options);
            return true;
        }

        public async Task<bool> DeductFunds(DeductFunds funds)
        {
            var userWallet = await GetWallet(funds.UserId);
            userWallet.Amount -= funds.DebitAmount;

            var filter = Builders<Wallet>.Filter.Eq("userId", funds.UserId);
            var update = Builders<Wallet>.Update.Set("amount", userWallet.Amount);

            var options = new UpdateOptions()
            {
                IsUpsert = false
            };

            await _Collection.UpdateOneAsync(filter, update, options);
            return true;
        }

        public async Task<Wallet> GetWallet(string UserId)
        {
            var userWallet = new Wallet();
            userWallet = await _Collection.AsQueryable().FirstOrDefaultAsync(x =>x.UserId == UserId);

            if (userWallet is null)
                userWallet = new Wallet();

            return userWallet;
        }
    }
}
