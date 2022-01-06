using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Command.Wallet
{
    public class DeductFunds
    {
        public string UserId { get; set; }
        public decimal DebitAmount { get; set; }
    }
}
