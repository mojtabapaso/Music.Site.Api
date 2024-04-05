using Moq;
using AutoMapper;
using AutoFixture;
using Music.Controllers;
using Music.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Music.Application.Interface.Entity;
using Music.Application.DataTransferObjects;
using Music.Infrastructure.Services;

namespace Music.Tests.Controllers;

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
    [Fact]
    public void GetListMusicPremiumOnlyUser_Success()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 5;
        var musics = fixture.CreateMany<MusicEntity>(5).ToList();
        var musicsDTO = fixture.CreateMany<MusicDTO>(5).ToList();
        musicServicesMock.Setup(x => x.GetSubscriptionRequiredMusicAsync(pageNumber, pageSize)).ReturnsAsync(musics);
        imapperMock.Setup(map => map.Map<List<MusicDTO>>(It.IsAny<List<MusicEntity>>())).Returns(musicsDTO);
        // Act
        var result = musicController.GetListMusicPremiumOnlyUser(pageNumber, pageSize).Result;
        var resultType = result as OkObjectResult;
        var reslutValue = resultType.Value as List<MusicDTO>;
        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(resultType);
        Assert.NotNull(reslutValue);
        Assert.Equal(reslutValue[1].Id, musicsDTO[1].Id);
        Assert.Equal(reslutValue[1].Description, musicsDTO[1].Description);
        Assert.Equal(reslutValue[1].UrlDownload, musicsDTO[1].UrlDownload);
    }
    [Fact]
    public void GetListMusic_Success()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 5;
        var musics = fixture.CreateMany<MusicEntity>(5).ToList();
        var musicsDto = fixture.CreateMany<MusicDTO>(5).ToList();
        musicServicesMock.Setup(x => x.GetSubscriptionNotRequiredMusicAsync(pageNumber, pageSize)).ReturnsAsync(musics);
        imapperMock.Setup(x => x.Map<List<MusicDTO>>(It.IsAny<List<MusicEntity>>())).Returns(musicsDto);
        //Act
        var result = musicController.GetListMusic(pageNumber,pageSize).Result;
        var resultType = result as OkObjectResult;
        var reslutValue = resultType.Value as List<MusicDTO>;
        //Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(resultType);
        Assert.NotNull(reslutValue);

    }

    [Fact]
    public void GetPremiumMusic_Success()
    {
        // Arrange
        string id = "1";
        var musics = fixture.Create<MusicEntity>();
        var musicsDTO = fixture.Create<MusicDTO>();
        imapperMock.Setup(map => map.Map<MusicDTO>(It.IsAny<MusicEntity>())).Returns(musicsDTO);
        musicServicesMock.Setup(m => m.FindSubscriptionRequiredMusicAsync(id)).ReturnsAsync(musics);
        // Act
        IActionResult result = musicController.GetPremiumMusic(id).Result;
        var resultType = result as OkObjectResult;
        var reslutValue = resultType.Value as MusicDTO;
        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(resultType);
        Assert.NotNull(reslutValue);
        Assert.Equal(reslutValue.Id, musicsDTO.Id);
        Assert.Equal(reslutValue.TypeMusicId, musicsDTO.TypeMusicId);
        Assert.Equal(reslutValue.Subtitle, musicsDTO.Subtitle);
        Assert.Equal(reslutValue.MusicCategories, musicsDTO.MusicCategories);
    }
    [Fact]
    public void GetMusic_Success()
    {
        //Arrange
        string id = "1";
        MusicEntity music = fixture.Create<MusicEntity>();
        MusicDTO musicDto = fixture.Create<MusicDTO>();
        musicServicesMock.Setup(m => m.FindSubscriptionNotRequiredMusicAsync(id)).ReturnsAsync(music);
        imapperMock.Setup(x=>x.Map<MusicDTO>(It.IsAny<MusicEntity>())).Returns(musicDto); ;
        //Act
        IActionResult result = musicController.GetMusic(id).Result;
        var resultType = result as OkObjectResult;
        var reslutValue = resultType.Value as MusicDTO;
        //Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(resultType);
        Assert.NotNull(reslutValue);
        Assert.Equal(reslutValue.Id, musicDto.Id);
        Assert.Equal(reslutValue.TypeMusicId, musicDto.TypeMusicId);
        Assert.Equal(reslutValue.Subtitle, musicDto.Subtitle);
        Assert.Equal(reslutValue.MusicCategories, musicDto.MusicCategories);
    }
    //[Fact]
    //public void Search_Success()
    //{
    //    // Arrange
    //    int page = 1;
    //    int pageSize = 10;
    //    string filter = "test text";

    //    int totalCount = 120;
    //    int totalPages = 124;
    //    List<MusicEntity> musicList = fixture.CreateMany<MusicEntity>(5).ToList();
    //    var searchResult = (musicList, totalCount, totalPages);
    //    musicServicesMock.Setup(m => m.PaginationMusicAsync(filter, page, pageSize)).ReturnsAsync((searchResult));
    //    //Act
    //    var result = musicController.Search(page, pageSize, filter).Result;
    //    var resultType = result as OkObjectResult;
    //    //var reslutValue = resultType.Value as Tuple<List<MusicEntity>, int, int,int ,int>;
    //    var resultValue = resultType.Value as dynamic;
    //    var musicEntities = resultValue.Musices ;
    //    var totalCount2 = resultValue.TotalCount;
    //    var totalPages2 = resultValue.TotalPages ;
    //    var currentPage = resultValue.CurrentPage ;
    //    var pageSize2 = resultValue.PageSize;
    //    //Assert
    //    Assert.NotNull(resultType);
    //    //Assert.NotNull(reslutValue);
    // TODO need to change
    //}

    [Fact]
    public void GetMusicListBySinger_Suuccess()
    {
        //Arrange
        string singerId = "1";
        var musics = fixture.CreateMany<MusicEntity>(5).ToList();
        var musicsDto = fixture.CreateMany<MusicDTO>(5).ToList();
        musicServicesMock.Setup(x=>x.GetMusicListBySingerAsync(singerId)).ReturnsAsync(musics);
        imapperMock.Setup(x => x.Map<List<MusicDTO>>(It.IsAny<List<MusicEntity>>())).Returns(musicsDto);
        //Act
        IActionResult result = musicController.GetMusicListBySinger(singerId).Result;
        var resultType = result as OkObjectResult;
        var reslutValue = resultType.Value as List<MusicDTO>;
        //Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(resultType);
        Assert.NotNull(reslutValue);
    }
    [Fact]
    public void GetMusicListByCategory_Suuccess()
    {
        //Arrange
        string categoryName = "category name test";
        var musics = fixture.CreateMany<MusicEntity>(5).ToList();
        var musicsDto = fixture.CreateMany<MusicDTO>(5).ToList();
        musicServicesMock.Setup(x => x.GetMusicListByCategoryAsync(categoryName)).ReturnsAsync(musics);
        imapperMock.Setup(x => x.Map<List<MusicDTO>>(It.IsAny<List<MusicEntity>>())).Returns(musicsDto);
        //Act
        IActionResult result = musicController.GetMusicListBySinger(categoryName).Result;
        var resultType = result as OkObjectResult;
        var reslutValue = resultType.Value as List<MusicDTO>;
        //Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(resultType);
        Assert.NotNull(reslutValue);
    }
}