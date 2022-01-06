using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Command.Wallet
{
    public class AddFunds
    {
        public string UserId { get; set; }
        public decimal CreditAmount { get; set; }
    }
}
