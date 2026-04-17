using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repository, 
            ISpecification<T> spec, int pageIndex, int pageSize) where T : BaseEntity
    {
        var count = await repository.CountAsync(spec);
        var data = await repository.ListAsync(spec);
        var pagination = new Pagination<T>(pageIndex, pageSize, count, data);
        return Ok(pagination);
    }
}
