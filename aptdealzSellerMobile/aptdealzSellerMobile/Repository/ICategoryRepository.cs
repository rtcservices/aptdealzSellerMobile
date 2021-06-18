using aptdealzSellerMobile.Model.Reponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategory();
        Task<List<SubCategory>> GetSubCategory(string CategortyId);
        Task<List<Category>> CreateCategory(string OtherCategory);
        Task<List<SubCategory>> CreateSubCategory(string OtherSubCategory, string CategoryId);
    }
}
