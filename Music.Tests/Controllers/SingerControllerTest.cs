using Moq;
using AutoMapper;
using AutoFixture;
using Music.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Music.Presentation.Controllers;
using Music.Application.Interface.Entity;
using Music.Application.DataTransferObjects;

namespace Music.Tests.Controllers;

public class SingerControllerTest 
{
    Fixture fixture;
    SingerController singerController;
    Mock<IMapper> mockMapper;   
    Mock<ISingerServices> singerServices;

    public SingerControllerTest()
    {
        fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        mockMapper = new Mock<IMapper>();
        singerServices = new Mock<ISingerServices>();
        singerController = new SingerController(mockMapper.Object, singerServices.Object);
    }
    [Fact]
    public void GetListSinger_Success()
    {
        // Arrange
        var singers = fixture.CreateMany<Singer>(5).ToList();
        var singersDTO = fixture.CreateMany<SingerDTO>(5).ToList();
        singerServices.Setup(mock => mock.GetAllAsync()).ReturnsAsync(singers);
        mockMapper.Setup(x => x.Map<List<SingerDTO>>(It.IsAny<List<Singer>>())).Returns(singersDTO);
        // Act
        var result = singerController.GetListSinger().Result;
        var resultType = result as OkObjectResult;
        var reslutValue = resultType.Value as List<SingerDTO>;
        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(resultType);
        Assert.NotNull(reslutValue);
        Assert.Equal(reslutValue[1].Id, singersDTO[1].Id);
        Assert.Equal(reslutValue[1].Name, singersDTO[1].Name);
        Assert.Equal(reslutValue[1].Musics, singersDTO[1].Musics);
    }
    [Fact]
    public void GetListSinger_NotFound()
    {
        // Arrange
        //Act
        var result = singerController.GetListSinger().Result;
        var resultType = result as NotFoundResult;
        // Assert
        Assert.IsType<NotFoundResult>(result);
        Assert.NotNull(resultType);
    }

    [Fact]
    public void GetDetailSinger_Success()
    {
        //Arrange
        string userId = "1";
        var singer = fixture.Create<Singer>();
        var singerDTO = fixture.Create<SingerDTO>();
        singerServices.Setup(mock => mock.FindByIdAsync(userId)).ReturnsAsync(singer);
        mockMapper.Setup(mock => mock.Map<SingerDTO>(It.IsAny<Singer>())).Returns(singerDTO);
        //Act
        var result = singerController.GetDetailSinger(userId).Result;
        var resultType = result as OkObjectResult;
        var reslutValue = resultType.Value as SingerDTO;
        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(reslutValue.Id, singerDTO.Id);
        Assert.Equal(reslutValue.Name, singerDTO.Name);
        Assert.Equal(reslutValue.Musics, singerDTO.Musics);
    }
    [Fact]
    public void GetDetailSinger_NotFound()
    {
        //Arrange
        string userId = "1";
        //Act
        var result = singerController.GetDetailSinger(userId).Result;
        //Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }

}
