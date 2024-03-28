using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Music.Application.Interface.Entity;
using Music.Controllers;
using Music.Domain.Entities;
using NSubstitute;
using System.Net;
using Xunit;

namespace Music.Tests.Presentation.Controllers;

[TestClass]
public class MusicControllerTests
{
	private Fixture fixture;
	private Mock<IMapper> imapperMock;
	private Mock<IMusicServices> musicServicesMock;
	private Mock<UserManager<ApplicationUser>> userManagerMock;
	private MusicController musicController;
	public MusicControllerTests()
	{
		fixture = new Fixture();
		var userStore = new Mock<IUserStore<ApplicationUser>>();
		imapperMock = new Mock<IMapper>();
		fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
		fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		musicServicesMock = new Mock<IMusicServices>();
		userManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
		musicController = new MusicController(musicServicesMock.Object, userManagerMock.Object, imapperMock.Object);
	}
	[TestMethod]
	public async Task GetListMusicTest_ReturnsOkObjectResult()
	{
		// Arrange
		var musics = fixture.CreateMany<MusicEntity>().ToList();
		musicServicesMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(musics);
		// Act
		var result = await musicController.GetListMusicPremiumOnlyUser(2, 24);
		
		var obj = result as ObjectResult;
		// Assert
		Assert.AreEqual(200, obj.StatusCode);
		Assert.IsInstanceOfType(result, typeof(OkObjectResult));
	}
	[TestMethod]
	public async Task GetListMusicTest_ReturnsListOfMusicEntitiesAsync()
	{
		// Arrange
		var musics = fixture.CreateMany<MusicEntity>().ToList();
		musicServicesMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(musics);
		// Act
		var result = musicController.GetListMusicPremiumOnlyUser(1, 3).Result;
		var resultds = result as OkObjectResult;
		var vaule = resultds.Value;

		var musicList = resultds.Value as List<MusicEntity>;
		// Assert
		Assert.IsNotNull(result);
		Assert.IsNotNull(musicList);
		CollectionAssert.AreEqual(musics, musicList);
	}
	[TestMethod]
	public async Task GetMusic_Test_ReturnsOkObjectResult()
	{
		//Arrange
		var musicId = "1";
		var music = fixture.Create<MusicEntity>();
		musicServicesMock.Setup(mock => mock.FindByIdAsync(musicId)).ReturnsAsync(music);

		await using var factory = new WebApplicationFactory<Program>();
		using var client = factory.CreateClient();
		// Act
		var fer = await musicController.GetMusic(musicId);

		var response = await client.GetAsync("api/Music/1/");
		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

	}

	//[TestMethod]
	//public void GetMusicTest_ReturnsMusicEntity()
	//{
	//	// Arrange
	//	var musicId = "1";
	//	var music = fixture.Create<MusicEntity>();
	//	musicServicesMock.Setup(mock => mock.FindByIdAsync(musicId)).ReturnsAsync(music);
	//	// Act
	//	var result = musicController.GetMusic(musicId).Result as OkObjectResult;
	//	var musicResult = result.Value as MusicDTO;
	//	// Assert
	//	Assert.IsNotNull(result);
	//	Assert.IsNotNull(musicResult);
	//	Assert.AreEqual(music.Subtitle, musicResult.Subtitle);
	//}
	[TestMethod]
	public void PaginationMusicTest_ReturnsCorrectResult()
	{
		// Arrange
		var filter = "";
		var page = 1;
		var pageSize = 10;
		var musicEntities = fixture.CreateMany<MusicEntity>().ToList();

		int totalCount = 20;
		int totalPages = 10;
		var queryResult = (musicEntities, totalCount, totalPages);
		var expectedMusicEntities = fixture.CreateMany<MusicEntity>(pageSize).ToList();

		musicServicesMock.Setup(mock => mock.PaginationMusicAsync(filter, page, pageSize)).ReturnsAsync((expectedMusicEntities, totalCount, totalPages));
		// Act
		var result = musicController.Get(page, pageSize, filter).Result as ObjectResult;

		var resultObject = result.Value as Tuple<string, List<MusicEntity>, string, string, string>;

		Assert.IsNotNull(result);
		Assert.IsNotNull(resultObject);
		Assert.AreEqual(queryResult.musicEntities, result.Value.Received(1));
	}
	[TestMethod]
	public void GetMusicListBySingerTest_ReturnsCorrectResult()
	{
		// Arrange
		var musicEntities = fixture.CreateMany<MusicEntity>().ToList();
		string categoryName = "Test_Category";
		var categoryMusic = fixture.Create<Category>();
		musicServicesMock.Setup(mock => mock.GetMusicListByCategoryAsync(categoryMusic.Title)).ReturnsAsync(musicEntities);
		//Act
		var result = musicController.GetMusicListByCategory(categoryName).Result;
		//Assert
		Assert.IsInstanceOfType(result, typeof(OkObjectResult));
	}
	[TestMethod]
	public void GetMusicListBySinger_ReturnsCorrectResult()
	{
		//Arrange 
		var music = fixture.CreateMany<MusicEntity>().ToList();
		string singerId = "1";
		var singer = fixture.Create<Singer>();
		musicServicesMock.Setup(mock => mock.GetMusicListBySingerAsync(singerId)).ReturnsAsync(music);
		//Act
		var result = musicController.GetMusicListBySinger(singerId).Result;
		//Assert
		Assert.IsInstanceOfType(result, typeof(OkObjectResult));
	}
	[TestMethod]
	public void GetListMusic_ReturnsCorrentResult()
	{

	}
	//Assert.AreEqual(queryResult.totalCount, resultObjectTuple.Item2);
	//Assert.AreEqual(queryResult.totalPages, resultObjectTuple.Item3);
	//Assert.AreEqual(page, resultObject.CurrentPage);
	//Assert.AreEqual(pageSize, resultObject.PageSize);
	//Assert.AreEqual(queryResult.musicEntities, resultObject.Musices);
	//====
	//Assert.AreEqual(totalCount, result.Value.totalCount);
	//Assert.AreEqual(totalPages, result.totalPages);
	//CollectionAssert.AreEqual(expectedMusicEntities, result.musicEntities);

	//[TestMethod]
	//public void PaginationMusicTest_ReturnsEmptyList_WhenOutOfRangePageIsRequested()
	//{
	//	// Arrange
	//	var filter = "";
	//	var page = 3;
	//	var pageSize = 10;

	//	var expectedMusicEntities = new List<MusicEntity>();
	//	var totalCount = 20;
	//	var totalPages = 2;

	//	// Act
	//	//var result = musicServicesMock.PaginationMusi(filter, page, pageSize);

	//	// Assert
	//	Assert.IsNotNull(result);
	//	Assert.AreEqual(totalCount, result.totalCount);
	//	Assert.AreEqual(totalPages, result.totalPages);
	//	CollectionAssert.AreEqual(expectedMusicEntities, result.musicEntities);
	//}
}