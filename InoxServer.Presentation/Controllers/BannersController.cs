using InoxServer.Application.Features.Banners.Commands.CreateBanner;
using InoxServer.Application.Features.Banners.Commands.DeleteBanner;
using InoxServer.Application.Features.Banners.Commands.UpdateBanner;
using InoxServer.Application.Features.Banners.Queries.GetActiveBanners;
using InoxServer.Application.Features.Banners.Queries.GetAllBanners;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InoxServer.Presentation.Controllers;

[ApiController]
[Route("api/v1/banners")]
public class BannersController : ControllerBase
{
    private readonly IMediator _mediator;

    public BannersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách banner đang hoạt động (Public)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetActiveBanners()
    {
        var banners = await _mediator.Send(new GetActiveBannersQuery());
        return Ok(banners);
    }

    /// <summary>
    /// Lấy tất cả banner (Admin)
    /// </summary>
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllBanners()
    {
        var banners = await _mediator.Send(new GetAllBannersQuery());
        return Ok(banners);
    }

    /// <summary>
    /// Tạo banner mới (Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBanner([FromBody] CreateBannerCommand command)
    {
        var id = await _mediator.Send(command);
        return Created($"/api/v1/banners/{id}", new { Id = id, Message = "Tạo banner thành công." });
    }

    /// <summary>
    /// Cập nhật banner (Admin)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBanner(Guid id, [FromBody] UpdateBannerCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(new { Success = result, Message = "Cập nhật banner thành công." });
    }

    /// <summary>
    /// Xóa banner (Admin)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBanner(Guid id)
    {
        var result = await _mediator.Send(new DeleteBannerCommand { Id = id });
        return Ok(new { Success = result, Message = "Xóa banner thành công." });
    }
}
