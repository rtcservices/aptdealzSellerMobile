using aptdealzSellerMobile.Model.Request;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    public interface IRequirementRepository
    {
        Task<Requirement> GetRequirementById(string RequirmentId);

        Task<string> RevealContact(RevealBuyerContact mRevealBuyerContact);
    }
}
