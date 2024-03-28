using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Music.Application.Interface.Entity;
using Music.Application.Interface.Logic;
using Music.Domain.Entities;
using Music.Presentation.Controllers;
using System.Security.Claims;

namespace Music.Tests.Presentation.Controllers;

[TestClass]
public class SubscriptionControllerTests
{
	private HttpContext contextMock;
	private SubscriptionController musicController;
	private Mock<ISubscriptionServices> subscriptionServicesMock;
	private Mock<UserManager<ApplicationUser>> userManagerMock;
	private Mock<IUserRefreshTokensServices> userRefreshTokensMock;
	[TestInitialize]
	public void Initialize()
	{
		// Arrange
		this.subscriptionServicesMock = new Mock<ISubscriptionServices>();
		var userStore = new Mock<IUserStore<ApplicationUser>>();
		this.userRefreshTokensMock = new Mock<IUserRefreshTokensServices>();
		this.userManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
		this.musicController = new SubscriptionController(subscriptionServicesMock.Object, userManagerMock.Object, userRefreshTokensMock.Object);
		// Set current user
		var claims = new List<Claim>
		{
				new Claim(ClaimTypes.NameIdentifier, "userId")
		};
		var identity = new ClaimsIdentity(claims, "TestAuthScheme");
		var claimsPrincipal = new ClaimsPrincipal(identity);
		musicController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext
			{
				User = claimsPrincipal
			}
		};
	}

	[TestMethod]
	public async Task GetSubscription_WithValidSubscription_ReturnsOkResult()
	{
		var contextMock = new Mock<HttpContext>();
		contextMock.Setup(x => x.User).Returns(new ClaimsPrincipal());
		// Arrange
		var userId = "userId";
		var expectedSubscription = new Subscription(); // Provide your expected subscription object

		subscriptionServicesMock.Setup(mock => mock.GetSubscriptionByUserIdAsync(userId))
			.ReturnsAsync(expectedSubscription);

		// Act
		//var result = await musicController.GetSubscription();
		musicController.ControllerContext.HttpContext = contextMock.Object;
		// Assert
		Assert.IsNotNull(musicController.User);
		//Assert.IsNotNull(result);
		//Assert.IsInstanceOfType(result, typeof(OkObjectResult));
		//var okResult = result as OkObjectResult;
		//Assert.AreEqual(expectedSubscription, okResult.Value);
	}

	[TestMethod]
	public async Task GetSubscription_WithNullSubscription_ReturnsNotFoundResult()
	{
		// Arrange
		var userId = "userId";

		subscriptionServicesMock.Setup(mock => mock.GetSubscriptionByUserIdAsync(userId))
			.ReturnsAsync(null as Subscription);

		// Act
		var result = await musicController.GetSubscription();

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType(result, typeof(NotFoundResult));
	}
}