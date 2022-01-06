using EShop.Infrastructure.Command.Wallet;
using EShop.Wallet.DataProvider.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Wallet.DataProvider.Services
{
    public class WalletService : IWalletService
    {
        private IWalletRepository _walletRepository;
        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }
        public async Task<bool> AddFunds(AddFunds funds)
        {
            return await _walletRepository.AddFunds(funds);
        }

        public async Task<bool> DeductFunds(DeductFunds funds)
        {
            return await _walletRepository.DeductFunds(funds);
        }
    }
}
