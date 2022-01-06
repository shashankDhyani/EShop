using EShop.Infrastructure.Command.Wallet;
using EShop.Wallet.DataProvider.Services;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Wallet.Api.Handlers
{
    public class DeductFundsConsumer : IConsumer<DeductFunds>
    {
        private IWalletService _walletService;
        public DeductFundsConsumer(IWalletService walletService)
        {
            _walletService = walletService;
        }
        public async Task Consume(ConsumeContext<DeductFunds> context)
        {
            var isAdded = await _walletService.DeductFunds(context.Message);

            if (isAdded)
                await Task.CompletedTask;
            else
                throw new Exception("Funds are not deducted. Try after sometime.");
        }
    }
}
