using Application.Commands.AssetManagement.CategoryCommands.Create;
using Application.Commands.AssetManagement.CategoryCommands.Delete;
using Application.Commands.AssetManagement.CategoryCommands.Update;
using Application.Queries.AssetManagement.CategoryQueries.GetCategories;
using Application.Queries.AssetManagement.CategoryQueries.GetSubList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Controllers.AssetManagement;
[Route("api/[controller]")]
[ApiController]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CategoryC command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync( [FromRoute] long id,[FromBody] CategoryU command)
    {
        command.Id = id;
        return Ok(await mediator.Send(command));
    }
    
    [HttpGet]
    public async Task<IActionResult> FindAsync([FromQuery] GetCategoriesQ query)
    {
        return Ok(await mediator.Send(query));
    }
    
    [HttpGet("Parent")]
    public async Task<IActionResult> FindParentAsync([FromQuery] CategorySubListQ query)
    {
        return Ok(await mediator.Send(query));
    }
    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id)
    {
        return Ok(await mediator.Send(new CategoryD(id)));
    }
}