using BookApi.Models;
using Library.Interfaces;

namespace BookAPI.Repositories
{
    public interface ISubCategoryRepository: IManagable<SubCategory>
    {
        Task<List<SubCategory>> GetAllAsync();       
    }
}
