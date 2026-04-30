using Asp.Versioning;
using InoxServer.Application.Features.Products.Commands.AddProductImage;
using InoxServer.Application.Features.Products.Commands.CreateProduct;
using InoxServer.Application.Features.Products.Commands.DeleteProduct;
using InoxServer.Application.Features.Products.Commands.DeleteProductImage;
using InoxServer.Application.Features.Products.Commands.UpdateProduct;
using InoxServer.Application.Features.Products.Queries.GetProductById;
using InoxServer.Application.Features.Products.Queries.GetProducts;
using InoxServer.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InoxServer.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetProductsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));
        return result ? Ok() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/images")]
    public async Task<IActionResult> AddImage(Guid id, IFormFile file, [FromQuery] bool isPrimary = false)
    {
        if (file is null || file.Length == 0)
            throw new DomainException(Error.BadRequest(
                DomainErrorCodes.General.UploadFailed, "Vui lòng chọn file ảnh."));

        var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowed.Contains(file.ContentType.ToLower()))
            throw new DomainException(Error.BadRequest(
                DomainErrorCodes.General.UploadFailed, "Chỉ chấp nhận JPG, PNG hoặc WEBP."));

        if (file.Length > 5 * 1024 * 1024)
            throw new DomainException(Error.BadRequest(
                DomainErrorCodes.General.UploadFailed, "Kích thước ảnh không được vượt quá 5MB."));

        using var stream = file.OpenReadStream();
        var result = await _mediator.Send(new AddProductImageCommand
        {
            ProductId = id,
            FileStream = stream,
            FileName = file.FileName,
            IsPrimary = isPrimary
        });

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{productId:guid}/images/{imageId:guid}")]
    public async Task<IActionResult> DeleteImage(Guid productId, Guid imageId)
    {
        await _mediator.Send(new DeleteProductImageCommand(productId, imageId));
        return NoContent();
    }
}
