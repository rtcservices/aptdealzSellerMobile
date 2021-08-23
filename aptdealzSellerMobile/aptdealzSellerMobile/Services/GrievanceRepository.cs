using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Services
{
    public class GrievanceRepository : IGrievanceRepository
    {
        GrievanceAPI grievanceAPI = new GrievanceAPI();
        public async Task<Grievance> GetGrievancesDetails(string GrievanceId)
        {
            Grievance mGrievance = new Grievance();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await grievanceAPI.GetGrievancesDetailsForSeller(GrievanceId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mGrievance = jObject.ToObject<Grievance>();
                    }
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceRepository/GetGrievancesDetails: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return mGrievance;
        }

        public async Task SubmitGrievanceResponse(string GrievanceId, string Message)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await grievanceAPI.SubmitGrievanceResponseFromSeller(GrievanceId, Message);
                if (mResponse != null && mResponse.Succeeded)
                {
                    if (!(bool)mResponse.Data)
                        Common.DisplayErrorMessage(mResponse.Message);
                }
                else
                {
                    if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceRepository/SubmitGrievanceResponse: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }
    }
}
