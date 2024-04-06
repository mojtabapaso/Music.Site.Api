using AutoMapper;
using Asp.Versioning;
using Api.Controllers;
using Music.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Music.Application.DataTransferObjects;
using Music.Application.Interface.Entity;

namespace Music.Controllers;


[ApiVersion("1", Deprecated = false)]
public class MusicController : BaseController
{
    private readonly IMusicServices musicServices;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IMapper mapper;

        public MusicController(IMusicServices musicServices, UserManager<ApplicationUser> userManager, IMapper mapper
        )
    {
        this.musicServices = musicServices;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    [HttpGet("GetAll/Premium")]
    [Authorize("PremiumUser", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetListMusicPremiumOnlyUser(int pageNumber, int pageSize)
    {
        var musics = await musicServices.GetSubscriptionRequiredMusicAsync(pageNumber, pageSize);
        var musicsDto = mapper.Map<List<MusicDTO>>(musics);
        return Ok(musicsDto);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetListMusic(int pageNumber, int pageSize)
    {
        var musics = await musicServices.GetSubscriptionNotRequiredMusicAsync(pageNumber, pageSize);
        var musicsDto = mapper.Map<List<MusicDTO>>(musics);
        return Ok(musicsDto);
    }
    [HttpGet("Premium/{id}")]
    [ResponseCache(Duration = 60)]
    [Authorize("PremiumUser", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetPremiumMusic(string id)
    {
        var music = await musicServices.FindSubscriptionRequiredMusicAsync(id);
        var musicDto = mapper.Map<MusicDTO>(music);
        return Ok(musicDto);
    }
    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetMusic(string id)
    {
        var music = await musicServices.FindSubscriptionNotRequiredMusicAsync(id);
        var musicDto = mapper.Map<MusicDTO>(music);
        return Ok(musicDto);
    }
    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string filter = "")
    {
        var query = await musicServices.PaginationMusicAsync(filter, page, pageSize);
        var result = new
        {
            Musices = query.musicEntities,
            TotalCount = query.totalCount,
            TotalPages = query.totalPages,
            CurrentPage = page,
            PageSize = pageSize
        };
        return Ok(result);
    }
    [HttpGet("Singer/{id}")]
    public async Task<IActionResult> GetMusicListBySinger(string singerId)
    {
        var musics = await musicServices.GetMusicListBySingerAsync(singerId);
        var musicsDto = mapper.Map<List<MusicDTO>>(musics);
        return Ok(musicsDto);
    }
    [HttpGet("Category/{categoryName}")]
    public async Task<IActionResult> GetMusicListByCategory(string categoryName)
    {
        var musics = await musicServices.GetMusicListByCategoryAsync(categoryName);
        var musicsDto = mapper.Map<List<MusicDTO>>(musics);
        return Ok(musicsDto);
    }

}