using EShop.Infrastructure.Authentication;
using EShop.Infrastructure.Command.User;
using EShop.User.DataProvider;
using MassTransit;
using System.Threading.Tasks;

namespace EShop.User.Query.Api.Handlers
{
    using EShop.Infrastructure.Security;
    public class LoginUserHandler : IConsumer<LoginUser> 
    {
        private IUserService _userService;
        private IEncrypter _encrypter;
        private IAuthenticationHandler _authHandler;

        public LoginUserHandler(IUserService userService,IEncrypter encrypter, IAuthenticationHandler authHandler)
        {
            _userService = userService;
            _encrypter = encrypter;
            _authHandler = authHandler;
        }
        
        public async Task Consume(ConsumeContext<LoginUser> context)
        {
            var user = await _userService.GetUserByusername(context.Message.Username);
            JwtAuthToken token = new JwtAuthToken();
            
            if (user != null)
            {
                var isAllowed = user.ValidatePassword(context.Message, _encrypter);

                if (isAllowed)
                    token = _authHandler.Create(user.UserId);
            }

            await context.RespondAsync<JwtAuthToken>(token);
        }
    }
}
