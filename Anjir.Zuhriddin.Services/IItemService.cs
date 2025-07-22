using Anjir.Zuhriddin.Items.DataAccess.Models;
using Anjir.Zuhriddin.Items.Services.ViewModels;

namespace Anjir.Zuhriddin.Items.Services
{
    public interface IItemService
    {
        Task<ItemGetAllViewModel> GetAllAsync(int page, int pageSize,SortField orderBy,bool isAscending);
        Task<ItemResultModel> CreateAsync(CreateItemViewModel model);
        Task<ItemResultModel> UpdateAsync(UpdateItemViewModel model);
        Task<ItemResultModel> DeleteAsync(int id);
    }
}
