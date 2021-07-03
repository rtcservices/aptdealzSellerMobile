using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    public interface IAuthenticationRepository
    {
        Task<bool> RefreshToken();
    }
}
