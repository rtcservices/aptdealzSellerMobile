using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aptdealzSellerMobile.API
{
    public class RequirementAPI
    {
        #region [ GET ]
        public async Task<Response> GetAllActiveRequirements(string SortBy = "", string Title = "", bool? IsAscending = null, int PageNumber = 1, int PageSize = 10)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetAllActiveRequirements + "?PageNumber={1}&PageSize={2}", (int)App.Current.Resources["Version"], PageNumber, PageSize);
                        if (!Common.EmptyFiels(Title))
                            url += "&Title=" + Title;
                        if (!Common.EmptyFiels(SortBy))
                            url += "&SortBy=" + SortBy;
                        if (IsAscending.HasValue)
                            url += "&IsAscending=" + IsAscending.Value;

                        var response = await hcf.GetAsync(url);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetAllActiveRequirements(SortBy, Title, IsAscending, PageNumber, PageSize);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/GetAllActiveRequirements: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetRequirementById(string RequirmentId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetRequirementById, (int)App.Current.Resources["Version"], RequirmentId);
                        var response = await hcf.GetAsync(url);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetRequirementById(RequirmentId);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/GetRequirementById: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetRequirementDetailsWithoutQuoteDetails(string RequirmentId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetRequirementDetailsWithoutQuoteDetails, (int)App.Current.Resources["Version"], RequirmentId);
                        var response = await hcf.GetAsync(url);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetRequirementDetailsWithoutQuoteDetails(RequirmentId);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/GetRequirementDetailsWithoutQuoteDetails: " + ex.Message);
            }
            return mResponse;
        }
        #endregion

        #region [ POST ]
        public async Task<Response> RevealBuyerContact(RevealBuyerContact mRevealBuyerContact)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = JsonConvert.SerializeObject(mRevealBuyerContact);
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.RevealBuyerContact, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await RevealBuyerContact(mRevealBuyerContact);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/RevealBuyerContact: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetAmountToBePaidToRevealSellerContact(string quoteId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"quoteId\":\"" + quoteId + "\"}";
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        //string url = string.Format(EndPointURL.GetAmountToBePaidToRevealBuyerContact, (int)App.Current.Resources["Version"]);

                        var BaseURL = (string)App.Current.Resources["BaseURL"];
                        string url = BaseURL + "api/v1/Quote/GetAmountToBePaidToRevealSellerContact";
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetAmountToBePaidToRevealSellerContact(quoteId);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/GetAmountToBePaidToRevealSellerContact: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetAmountToBePaidToRevealBuyerContact(string requirementId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"requirementId\":\"" + requirementId + "\"}";
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        //string url = string.Format(EndPointURL.GetAmountToBePaidToRevealBuyerContact, (int)App.Current.Resources["Version"]);
                        var BaseURL = (string)App.Current.Resources["BaseURL"];
                        string url = BaseURL + "api/v1/Requirement/GetAmountToBePaidToRevealBuyerContact";
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetAmountToBePaidToRevealBuyerContact(requirementId);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/CancelRequirement: " + ex.Message);
            }
            return mResponse;
        }


        #endregion
    }
}
