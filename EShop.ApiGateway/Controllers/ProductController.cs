using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;
using EShop.Infrastructure.Query.Product;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Wrap;
using System;
using System.Threading.Tasks;

namespace EShop.ApiGateway.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IBusControl _bus;
        private IRequestClient<GetProductById> _requestclient;
        private static readonly int MAX_RETRY_COUNT = 1;

        #region Policies

        private readonly AsyncFallbackPolicy<IActionResult> _fallbackPolicy;
        
        private static AsyncCircuitBreakerPolicy<IActionResult> _circuitBreaker = Policy<IActionResult>.Handle<Exception>().
                                                                                    AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(30),
                                                                                                                    2,TimeSpan.FromMinutes(1));

        private static AsyncRetryPolicy<IActionResult> _retryPolicy  = Policy<IActionResult>.Handle<Exception>().WaitAndRetryAsync(MAX_RETRY_COUNT,
                                retryCount => TimeSpan.FromSeconds(Math.Pow(3, retryCount)/3)); // Jittering strategy...

        private static AsyncPolicyWrap<IActionResult> _wrapPolicy = Policy.WrapAsync(_circuitBreaker, _retryPolicy);

        private static AsyncBulkheadPolicy _bulkhead = Policy.BulkheadAsync(1, 2, (ctx) =>
        {
            throw new Exception("All slots are filled.");
        });
        
        #endregion

        public ProductController(IBusControl bus, IRequestClient<GetProductById> request)
        {
            _bus = bus;
            _requestclient = request;
            _fallbackPolicy = Policy<IActionResult>.Handle<Exception>().FallbackAsync(Content("We are experincing issue. Please try after sometime."));
            
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(string productId)
        {
            var emptyExecutionSlots = _bulkhead.BulkheadAvailableCount;
            var emptyQueueSlots = _bulkhead.QueueAvailableCount;

            return await _bulkhead.ExecuteAsync(async () =>
            {
                var prdct = new GetProductById() { ProductId = productId };
                var product = await _requestclient.GetResponse<ProductCreated>(prdct);

                return Accepted(product);
            });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Add([FromForm] CreateProduct product)
        {
            var uri = new Uri("rabbitmq://localhost/create_product");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(product);
            
            return Accepted("Product Created");
        }
    }
}