using Asp.Versioning; using CRN.ProductApi.Application.DTOs; using CRN.ProductApi.Application.Interfaces; using FluentValidation; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc;
namespace CRN.ProductApi.Controllers;
[ApiController][Authorize][ApiVersion("1.0")][Route("api/v{version:apiVersion}/products")]
public class ProductsController(IProductService service,IValidator<CreateProductDto> createValidator,IValidator<UpdateProductDto> updateValidator) : ControllerBase
{
 [HttpGet] public async Task<ActionResult<PagedResult<ProductDto>>> Get([FromQuery]int? pageNumber=null,[FromQuery]int? page=null,[FromQuery]int pageSize=10,CancellationToken ct=default){int actualPage=pageNumber??page??1;return Ok(await service.GetPagedAsync(actualPage,pageSize,ct));}
 [HttpGet("{id:int}")] public async Task<ActionResult<ProductDto>> GetById(int id,CancellationToken ct){return Ok(await service.GetByIdAsync(id,ct));}
 [HttpGet("{id:int}/items")] public async Task<ActionResult<IReadOnlyCollection<ItemDto>>> GetItems(int id,CancellationToken ct){return Ok(await service.GetItemsByProductIdAsync(id,ct));}
 [HttpPost][Authorize(Roles="Admin")][ProducesResponseType(typeof(ProductDto),201)] public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto,CancellationToken ct){await createValidator.ValidateAndThrowAsync(dto,ct);var user=User.Identity?.Name??"system";var result=await service.CreateAsync(dto,user,ct);return CreatedAtAction(nameof(GetById),new{id=result.Id,version="1.0"},result);}
 [HttpPut("{id:int}")][Authorize(Roles="Admin")] public async Task<ActionResult<ProductDto>> Update(int id,UpdateProductDto dto,CancellationToken ct){await updateValidator.ValidateAndThrowAsync(dto,ct);var user=User.Identity?.Name??"system";return Ok(await service.UpdateAsync(id,dto,user,ct));}
 [HttpDelete("{id:int}")][Authorize(Roles="Admin")][ProducesResponseType(204)] public async Task<IActionResult> Delete(int id,CancellationToken ct){await service.DeleteAsync(id,ct);return NoContent();}
}
