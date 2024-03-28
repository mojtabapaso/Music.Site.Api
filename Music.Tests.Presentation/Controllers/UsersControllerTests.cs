using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Music.Domain.Entities;
using Music.Presentation.Controllers;

namespace Music.Tests.Presentation.Controllers;
[TestClass]
public class UsersControllerTests
{
	private UsersController userController;
	private Fixture fixture;
	private Mock<IMapper> imapperMock;

	private Mock<UserManager<ApplicationUser>> userManagerMock;
	public UsersControllerTests()
	{
		var userStore = new Mock<IUserStore<ApplicationUser>>();
		userManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
		imapperMock = new Mock<IMapper>();

		fixture = new Fixture();
		fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
		fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		userController = new UsersController(userManagerMock.Object, imapperMock.Object);

	}
	[TestMethod]
	//[DataRow("{}")]
	public void DetailUserTest()
	{
		// Arrange
		string id = "1";
		var user = fixture.Create<ApplicationUser>();
		userManagerMock.Setup(s => s.FindByIdAsync(id)).ReturnsAsync(user);
		// Act
		var result = userController.DetailUser(id).Result;
		var obj = result as ObjectResult;

		// Assert
		Assert.IsNotNull(obj);
		Assert.AreEqual(200, obj.StatusCode);
		CollectionAssert.Equals(obj.Value, user);
	}
}
