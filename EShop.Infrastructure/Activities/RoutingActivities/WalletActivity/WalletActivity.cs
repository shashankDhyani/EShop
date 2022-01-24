using EShop.Infrastructure.Command.Wallet;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Infrastructure.Activities.RoutingActivities.WalletActivity
{
    public class WalletActivity : IActivity<TransactMoney, TransactMoneyLog>
    {
        public async Task<CompensationResult> Compensate(CompensateContext<TransactMoneyLog> context)
        {
            try
            {
                var addFunds = new AddFunds
                { UserId = context.Log.UserId, CreditAmount = context.Log.Amount };
                var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/add_funds"));
                await endpoint.Send(addFunds);

                return context.Compensated();
            }
            catch (Exception)
            {
                return context.Failed();
            }
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<TransactMoney> context)
        {
            try
            {
            var deductFunds = new DeductFunds() { UserId = context.Arguments.UserId, DebitAmount=context.Arguments.Amount };
            var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/deduct_funds"));
            await endpoint.Send(deductFunds);
            return context.CompletedWithVariables<TransactMoneyLog>(new TransactMoneyLog {
                UserId = context.Arguments.UserId,
                Amount = context.Arguments.Amount
            }, new { });
            }
            catch(Exception ex)
            {
                return context.Faulted();
            }
        }
    }

    public class TransactMoney
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransactMoneyLog
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
    }
}
