using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    public interface IProfileRepository
    {
        Task<List<Country>> GetCountry();

        Task<List<Category>> GetCategory();

        Task<List<SubCategory>> GetSubCategory(string CategortyId);

        Task<List<Category>> CreateCategory(string OtherCategory);

        Task<List<SubCategory>> CreateSubCategory(string OtherSubCategory, string CategoryId);

        Task CreateSubCategoryByCategoryId(string OtherSubCategory, string categoryId);

        Task<bool> ValidPincode(string pinCode);

        Task<SellerDetails> GetMyProfileData();

        Task DeactivateAccount();
    }
}
