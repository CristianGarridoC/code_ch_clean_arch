using Application.Product.Commands.Create;
using Application.Product.Commands.Delete;
using Application.Product.Commands.Edit;
using Application.Product.Queries.GetAll;
using Application.Product.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers;

[Route("api/v1/[controller]")]
[EnableRateLimiting("fixed")]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        GetAllRequest request = new();
        var result = await _sender.Send(request);
        return new JsonResult(result)
        {
            StatusCode = 200
        };
    }
    
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid productId)
    {
        GetByIdRequest request = new(productId);
        var result = await _sender.Send(request);
        return new JsonResult(result)
        {
            StatusCode = 200
        };
    }
    
    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid productId)
    {
        DeleteRequest request = new(productId);
        await _sender.Send(request);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateRequest request)
    {
        var result = await _sender.Send(request);
        return new JsonResult(result)
        {
            StatusCode = 201
        };
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] EditRequest request)
    {
        await _sender.Send(request);
        return NoContent();
    }
}