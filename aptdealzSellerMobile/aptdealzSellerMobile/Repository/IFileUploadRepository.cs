using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    public interface IFileUploadRepository
    {
        Task<string> UploadFile(int fileUploadCategory);
    }
}
