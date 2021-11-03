using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aptdealzSellerMobile.API
{
    public class CategoryAPI
    {
        #region [ GET ]
        public async Task<List<Category>> GetCategory()
        {
            List<Category> mCategory = new List<Category>();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.Category, (int)App.Current.Resources["Version"]);
                        var response = await hcf.GetAsync(url);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mCategory = JsonConvert.DeserializeObject<List<Category>>(responseJson);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                mCategory = null;
                                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                                Common.ClearAllData();
                            }
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable
                                || response.StatusCode == System.Net.HttpStatusCode.InternalServerError
                                || responseJson.Contains(Constraints.Str_AccountDeactivated) && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            mCategory = null;
                            MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                            Common.ClearAllData();
                        }
                        else
                        {
                            mCategory = null;
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetCategory();
                    }
                }
            }
            catch (Exception ex)
            {
                mCategory = null;
                Common.DisplayErrorMessage("ProfileAPI/GetCategory: " + ex.Message);
            }
            return mCategory;
        }

        public async Task<List<SubCategory>> GetSubCategory(string categoryId)
        {
            List<SubCategory> mSubCategory = new List<SubCategory>();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.SubCategory, (int)App.Current.Resources["Version"], categoryId);
                        var response = await hcf.GetAsync(url);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mSubCategory = JsonConvert.DeserializeObject<List<SubCategory>>(responseJson);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                mSubCategory = null;
                                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                                Common.ClearAllData();
                            }
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable
                                || response.StatusCode == System.Net.HttpStatusCode.InternalServerError
                                || responseJson.Contains(Constraints.Str_AccountDeactivated) && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            mSubCategory = null;
                            MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                            Common.ClearAllData();
                        }
                        else
                        {
                            mSubCategory = null;
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetSubCategory(categoryId);
                    }
                }
            }
            catch (Exception ex)
            {
                mSubCategory = null;
                Common.DisplayErrorMessage("ProfileAPI/GetSubCategory: " + ex.Message);
            }
            return mSubCategory;
        }
        #endregion

        #region [ PUT ]
        public async Task<Response> CreateCategory(string name)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"name\":\"" + name + "\"}";

                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.CreateCategory, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await CreateCategory(name);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("CategoryAPI/CreateCategory: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> CreateSubCategory(string name, string categoryId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"name\":\"" + name + "\",\"categoryId\":\"" + categoryId + "\"}";

                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.CreateSubCategory, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await CreateSubCategory(name, categoryId);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("CategoryAPI/CreateSubCategory: " + ex.Message);
            }
            return mResponse;
        }
        #endregion
    }
}
