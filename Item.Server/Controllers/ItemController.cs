using Anjir.Zuhriddin.Items.DataAccess.Models;
using Anjir.Zuhriddin.Items.Services;
using Anjir.Zuhriddin.Items.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anjir.Zuhriddin.Items.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    private IItemService _itemService;
    public ItemController( IItemService itemService)
    {
        _itemService = itemService;
    }
    [HttpGet]
    public async Task<ItemGetAllViewModel> GetAll(int page = 1, int pageSize = 10,SortField orderBy = SortField.ItemId,bool isAscending = true)
    {
        return await _itemService.GetAllAsync(page, pageSize,orderBy, isAscending);        
    }

    [HttpDelete]
    public async Task<ItemResultModel> Delete(int id)
    {
        return await _itemService.DeleteAsync(id);
    }

    [HttpPost]
    public async Task<ItemResultModel> Add([FromBody]CreateItemViewModel item)
    {
        return await _itemService.CreateAsync(item);

    }
    [HttpPut]
    public async Task<ItemResultModel> Update([FromBody]UpdateItemViewModel item)
    {
        return await _itemService.UpdateAsync(item);
    }
}
