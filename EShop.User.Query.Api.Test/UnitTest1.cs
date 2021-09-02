using EShop.Infrastructure.Authentication;
using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using EShop.Infrastructure.Security;
using EShop.User.DataProvider;
using EShop.User.Query.Api.Handlers;
using MassTransit.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EShop.User.Query.Api.Test
{
    public class LoginUserHandlerTest
    {

        [Test(TestOf = typeof(LoginUserHandler))]
        public async Task LoginUserReturnsToken()
        {
            LoginUser loginUserMock = new LoginUser()
            {
                Username = "john_doe",
                Password ="john_sus"
            };

            UserCreated userCreatedMock = new UserCreated()
            {
                Username = "john_doe",
                Password = "john_sus",
                ContactNo = "+124-7858966",
                UserId = "john_doe",
                EmailId = "john@cv.com"
            };

            JwtAuthToken token = new JwtAuthToken()
            {
                Token = "4wydyui758dhsdhkajsd?==",
                Expires = default(long)
            };

            var harness = new InMemoryTestHarness();

            try
            {
                //Arrange
                var service = new Mock<IUserService>();
                service.Setup(x => x.GetUserByusername(It.IsAny<string>()))
                    .ReturnsAsync(userCreatedMock);

                var authHandler = new Mock<IAuthenticationHandler>();
                authHandler.Setup(x => x.Create(It.IsAny<string>())).
                    Returns(token);

                var encrypter = new Mock<IEncrypter>();
                encrypter.Setup(e => e.GetHash(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(userCreatedMock.Password);


                var consumerHarness = harness.Consumer(() => new LoginUserHandler(service.Object, encrypter.Object,
                                                            authHandler.Object));

                //Act
                await harness.Start();

                var requestClient = harness.CreateRequestClient<LoginUser>();
                var user = await requestClient.GetResponse<JwtAuthToken>(loginUserMock);

                //Assert
                Assert.That(user.Message.Token == token.Token);

            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}