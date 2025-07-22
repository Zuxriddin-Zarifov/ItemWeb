using Anjir.Zuhriddin.Items.DataAccess;
using Anjir.Zuhriddin.Items.DataAccess.Models;
using Anjir.Zuhriddin.Items.Services.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Anjir.Zuhriddin.Items.Services;

public class ItemService : IItemService
{
    private readonly MainContext _context;
    public ItemService(MainContext context)
    {
        _context = context;
    }
    public async Task<ItemResultModel> CreateAsync(CreateItemViewModel model)
    {
        Item item = new Item()
        {
            Date = model.Date,
            Name = model.Name,
            Type = model.Type
        };
        var res = await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();
        item = res.Entity;
        ItemResultModel result = new ItemResultModel()
        {
            Date = item.Date,
            Name = item.Name,
            Type = item.Type,
            ItemId = item.ItemId,
        };
        return result;
    }
    public async Task<ItemResultModel> DeleteAsync(int id)
    {
        var item = await _context.Items.FirstOrDefaultAsync(x => x.ItemId == id);
        if (item == null)
            throw new Exception("item not found");
        var res = _context.Remove(item);
         await _context.SaveChangesAsync();
        item = res.Entity;
        ItemResultModel result = new ItemResultModel()
        {
            Date = item.Date,
            Name = item.Name,
            Type = item.Type,
            ItemId = item.ItemId,
        };
        return result;
    }
    public async Task<ItemGetAllViewModel> GetAllAsync(int page, int pageSize, SortField orderBy, bool isAscending)
    {
        ItemGetAllViewModel result = new ItemGetAllViewModel();
        var items = await _context.Items
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(item => new ItemResultModel
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Type = item.Type,
                Date = item.Date
            })
            .ToListAsync();
        items = await Sort(items,orderBy, isAscending);

        result.Total = await _context.Items.CountAsync();
        result.CurrentPage = page;
        result.PageSize = pageSize;
        result.TotalPages = (result.Total + pageSize + 1) / pageSize;
        result.Items = items;

        return result;
    }
    private async Task<List<ItemResultModel>> Sort(List<ItemResultModel> items, SortField orderBy, bool asv)
    {
        items = (orderBy, asv) switch
        {
            (SortField.Name, true) => items.OrderBy(i => i.Name).ToList(),
            (SortField.Name, false) => items.OrderByDescending(i => i.Name).ToList(),

            (SortField.Type, true) => items.OrderBy(i => i.Type).ToList(),
            (SortField.Type, false) => items.OrderByDescending(i => i.Type).ToList(),

            (SortField.Date, true) => items.OrderBy(i => i.Date).ToList(),
            (SortField.Date, false) => items.OrderByDescending(i => i.Date).ToList(),

            (_, true) => items.OrderBy(i => i.ItemId).ToList(),
            (_, false) => items.OrderByDescending(i => i.ItemId).ToList(),
        };
        return items;
    }
    public async Task<ItemResultModel> UpdateAsync(UpdateItemViewModel model)
    {
        var item = await _context.Items.FirstOrDefaultAsync(x => x.ItemId == model.Id);
        if (item == null)
            throw new Exception("Item not found");

        item.Name = model.Name;
        item.Type = model.Type;
        item.Date = model.Date;

        _context.Items.Update(item);
        await _context.SaveChangesAsync();

        var result = new ItemResultModel
        {
            ItemId = item.ItemId,
            Name = item.Name,
            Type = item.Type,
            Date = item.Date
        };

        return result;
    }
}
