using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Music.Application.DataTransferObjects;
using Music.Application.Interface.Entity;
using Music.Domain.Entities;
using Music.Presentation.Controllers;

namespace Music.Tests.Controllers;

public class CategoryControllerTests
{
    private Fixture fixture;
    private Mock<IMapper> mapperMock;
    private Mock<ICategoryServices> categoryServicMock;
    private CategoryController categoryController;
    public CategoryControllerTests()
    {
        fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        mapperMock = new Mock<IMapper>();
        categoryServicMock = new Mock<ICategoryServices>();
        categoryController = new CategoryController(categoryServicMock.Object, mapperMock.Object);
    }
    [Fact]
    public void GetListCategory_Success()
    {
        // Arrange
        var categores = fixture.CreateMany<Category>(5).ToList();
        var categoresDto = fixture.CreateMany<CategoryDTO>(5).ToList();
        categoryServicMock.Setup(service => service.GetAllAsync()).ReturnsAsync(categores);
        mapperMock.Setup(mock => mock.Map<List<CategoryDTO>>(It.IsAny<List<Category>>())).Returns(categoresDto);
        //Act
        var result = categoryController.GetListCategory().Result;
        //Assert
        var resultOKObject = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(resultOKObject);
        var value = Assert.IsType<List<CategoryDTO>>(resultOKObject.Value);
        Assert.NotNull(value);
        Assert.Equal(5, value.Count);
        Assert.Equal(value[0].Id, categoresDto[0].Id);
        Assert.Equal(value[0].Title, categoresDto[0].Title);
        Assert.Equal(value[0].Music, categoresDto[0].Music);
        Assert.Equal(value[0], categoresDto[0]);
    }
}
