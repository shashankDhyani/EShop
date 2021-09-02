using EShop.Infrastructure.Authentication;
using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EShop.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IBusControl _bus;
        private IRequestClient<LoginUser> _loginRequestClient;
        public UserController(IBusControl bus, IRequestClient<LoginUser> loginRequestClient)
        {
            _bus = bus;
            _loginRequestClient = loginRequestClient;
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateUser user)
        {

            var uri = new Uri("rabbitmq://localhost/add_user");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(user);

            return Accepted("User Created");
        }

        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> Login([FromForm] LoginUser loginUser)
        {
            var userResponse = await _loginRequestClient.GetResponse<JwtAuthToken>(loginUser);
            return Accepted(userResponse.Message);
        }
    }
}