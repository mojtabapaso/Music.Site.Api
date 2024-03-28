using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Music.Domain.Entities;
using Music.WebAPI.Controllers;

namespace Music.Tests.Presentation.Controllers;

[TestClass]
public class AuthControllerTests
{
	private Fixture fixture;
	private AuthController authController;
	private Mock<IConfiguration> configuration;
	private Mock<UserManager<ApplicationUser>> userManagerMock;
	private Mock<IPasswordHasher<ApplicationUser>> passwordHasher;

	public AuthControllerTests()
	{
		this.fixture = new Fixture();
		fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
		fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		this.configuration = new Mock<IConfiguration>();
		var userStore = new Mock<IUserStore<ApplicationUser>>();
		this.userManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
		this.passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

	}
	//[TestMethod]
	//public 
}
