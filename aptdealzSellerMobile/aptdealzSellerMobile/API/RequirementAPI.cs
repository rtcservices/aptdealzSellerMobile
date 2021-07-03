using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.SplashScreen;
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
        int getAllActiveRequirements = 0;
        public async Task<Response> GetAllActiveRequirements(string SortBy = "", bool? IsAscending = null, int PageNumber = 1, int PageSize = 10)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = "";
                        if (!Common.EmptyFiels(SortBy) && IsAscending != null)
                            url = string.Format(EndPointURL.GetAllActiveRequirements + "?SortBy={1}&IsAscending={2}&PageNumber={3}&PageSize={4}", (int)App.Current.Resources["Version"], SortBy, IsAscending, PageNumber, PageSize);
                        else if (!Common.EmptyFiels(SortBy) && IsAscending == null)
                            url = string.Format(EndPointURL.GetAllActiveRequirements + "?SortBy={1}&PageNumber={2}&PageSize={3}", (int)App.Current.Resources["Version"], SortBy, PageNumber, PageSize);
                        else if (Common.EmptyFiels(SortBy) && IsAscending != null)
                            url = string.Format(EndPointURL.GetAllActiveRequirements + "?IsAscending={1}&PageNumber={2}&PageSize={3}", (int)App.Current.Resources["Version"], IsAscending, PageNumber, PageSize);
                        else
                            url = string.Format(EndPointURL.GetAllActiveRequirements + "?PageNumber={1}&PageSize={2}", (int)App.Current.Resources["Version"], PageNumber, PageSize);

                        var response = await hcf.GetAsync(url);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                var isRefresh = await DependencyService.Get<IAuthenticationRepository>().RefreshToken();
                                if (!isRefresh && getAllActiveRequirements == 3)
                                {
                                    Common.DisplayErrorMessage(Constraints.Session_Expired);
                                    App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                                }
                                else
                                {
                                    await GetAllActiveRequirements(RequirementSortBy.ID.ToString(), null, PageNumber, PageSize);
                                }
                                getAllActiveRequirements++;
                            }
                            else
                            {
                                mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                            }
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetAllActiveRequirements(RequirementSortBy.ID.ToString(), null, PageNumber, PageSize);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementAPI/GetAllActiveRequirements: " + ex.Message);
            }
            return mResponse;
        }

        int getRequirementById = 0;
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
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                var isRefresh = await DependencyService.Get<IAuthenticationRepository>().RefreshToken();
                                if (!isRefresh && getRequirementById == 3)
                                {
                                    Common.DisplayErrorMessage(Constraints.Session_Expired);
                                    App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                                }
                                else
                                {
                                    await GetRequirementById(RequirmentId);
                                }
                                getRequirementById++;
                            }
                            else
                            {
                                mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                            }
                        }
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
                Common.DisplayErrorMessage("RequirementAPI/GetRequirementById: " + ex.Message);
            }
            return mResponse;
        }
        #endregion

        #region [ POST ]
        int revealBuyerContact = 0;
        public async Task<Response> RevealBuyerContact(string quoteId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"quoteId\":\"" + quoteId + "\"}";
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.RevealBuyerContact, (int)App.Current.Resources["Version"], quoteId);
                        var mResponseMessage = await hcf.PostAsync(url, requestJson);
                        var responseJson = await mResponseMessage.Content.ReadAsStringAsync();
                        if (mResponseMessage.IsSuccessStatusCode)
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else if (mResponseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                var isRefresh = await DependencyService.Get<IAuthenticationRepository>().RefreshToken();
                                if (!isRefresh && revealBuyerContact == 3)
                                {
                                    Common.DisplayErrorMessage(Constraints.Session_Expired);
                                    App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                                }
                                else
                                {
                                    await RevealBuyerContact(quoteId);
                                }
                                revealBuyerContact++;
                            }
                            else
                            {
                                mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                            }
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await RevealBuyerContact(quoteId);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/RevealBuyerContact: " + ex.Message);
            }
            return mResponse;
        }
        #endregion
    }
}
