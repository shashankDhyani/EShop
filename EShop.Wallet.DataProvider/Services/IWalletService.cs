using EShop.Infrastructure.Command.Wallet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Wallet.DataProvider.Services
{
    public interface IWalletService
    {
        Task<bool> AddFunds(AddFunds funds);
        Task<bool> DeductFunds(DeductFunds funds);
    }
}
