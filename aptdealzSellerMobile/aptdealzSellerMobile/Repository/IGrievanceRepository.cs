using aptdealzSellerMobile.Model.Request;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    interface IGrievanceRepository
    {
        Task<Grievance> GetGrievancesDetails(string GrievanceId);
    }
}
