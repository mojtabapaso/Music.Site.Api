using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Music.Application.DataTransferObjects;
using Music.Application.Interface.Entity;
using Music.Application.Interface.Logic;
using Music.Domain.Entities;
using Music.Presentation.Controllers;
using System.Net;
using System.Security.Claims;

namespace Music.Tests.Controllers;

public class SubscriptionControllerTests
{
    private Fixture fixture;
    private SubscriptionController subscriptionController;
    private IConfiguration configuration;
    private Mock<IMapper> mapperMock;
    private Mock<IPaymentServisec> paymentServicesMock;
    private Mock<IWalletServices> wallettServicesMock;
    private Mock<ISubscriptionServices> subscriptionServicesMock;
    private Mock<UserManager<ApplicationUser>> userManagerMock;
    private Mock<IUserRefreshTokensServices> userRefreshTokensMock;

    public SubscriptionControllerTests()
    {
        fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        this.mapperMock = new Mock<IMapper>();
        this.subscriptionServicesMock = new Mock<ISubscriptionServices>();
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        this.userRefreshTokensMock = new Mock<IUserRefreshTokensServices>();
        var inMemorySettings = new Dictionary<string, string> {
            {"TopLevelKey", "TopLevelValue"},
            {"SubscriptionPrice:Mounth_3", "100"},
        };

        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

        this.paymentServicesMock = new Mock<IPaymentServisec>();
        this.wallettServicesMock = new Mock<IWalletServices>();
        this.userManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
        this.subscriptionController = new SubscriptionController(subscriptionServicesMock.Object, userManagerMock.Object, userRefreshTokensMock.Object, paymentServicesMock.Object, wallettServicesMock.Object, configuration, mapperMock.Object);
    }
    [Fact]
    public void GetSubscription_Success()
    {
        // Arrange
        string userId = "1";
        var subscription = fixture.Create<Subscription>();
        var subscriptionDto = fixture.Create<SubscriptionDTO>();
        userManagerMock.Setup(userManager => userManager.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
        subscriptionServicesMock.Setup(services => services.GetSubscriptionByUserIdAsync(userId)).ReturnsAsync(subscription);
        mapperMock.Setup(map => map.Map<SubscriptionDTO>(It.IsAny<Subscription>())).Returns(subscriptionDto);
        //Act
        var result = subscriptionController.GetSubscription().Result;
        //Assert
        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var returnedSubscription = Assert.IsAssignableFrom<SubscriptionDTO>(okObjectResult.Value);
        Assert.Equal(subscriptionDto.Id, returnedSubscription.Id);
        Assert.Equal(subscriptionDto.User, returnedSubscription.User);
        Assert.Equal(subscriptionDto.IsActive, returnedSubscription.IsActive);
        Assert.Equal(subscriptionDto.Create, returnedSubscription.Create);
        Assert.Equal(subscriptionDto.Expired, returnedSubscription.Expired);
    }
    [Fact]
    public void GetSubscription_Fail()
    {
        //Act
        var result = subscriptionController.GetSubscription().Result;
        //Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }
    [Fact]
    public async Task BuySubscription_ReturnsOkResult_WhenPaymentSucceeds()
    {
        // Arrange
        int month = 3;
        var user = fixture.Create<ApplicationUser>();
        userManagerMock.Setup(userManager => userManager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        //paymentServicesMock.Setup(service => service.PaymentAsync("100", "/", "", "")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK }); // Payment succeeds
        var statusPayment = await paymentServicesMock.Object.PaymentAsync("100", "/");
        subscriptionServicesMock.Setup(service => service.GetSubscriptionByUserIdAsync(user.Id)).ReturnsAsync((Subscription)null); // No existing subscription

        var newSubscription = fixture.Create<Subscription>();
        subscriptionServicesMock.Setup(service => service.AddAsync(It.IsAny<Subscription>())).Returns(Task.FromResult(newSubscription));
        subscriptionController.ControllerContext.HttpContext = new DefaultHttpContext(); // Set HttpContext for the controller

        // Act
        var result = await  subscriptionController.BuySubscription(month);

        // Assert
        Assert.IsType<OkResult>(result);
    }
}
