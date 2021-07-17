using aptdealzSellerMobile.Model.Reponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    public interface IProfileRepository
    {
        Task<List<Country>> GetCountry();
        Task<bool> ValidPincode(string pinCode);
    }
}
