using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.SplashScreen;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aptdealzSellerMobile.API
{
    class ProfileAPI
    {
        #region [ GET ]
        public async Task<List<Country>> GetCountry()
        {
            List<Country> mCountries = new List<Country>();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.Country, (int)App.Current.Resources["Version"]);
                        var response = await hcf.GetAsync(url);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mCountries = JsonConvert.DeserializeObject<List<Country>>(responseJson);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            mCountries = null;
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetCountry();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileAPI/GetCountry: " + ex.Message);
            }
            return mCountries;
        }

        int getMyProfileData = 0;
        public async Task<Response> GetMyProfileData()
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetMyProfileData, (int)App.Current.Resources["Version"]);
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
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                var isRefresh = await DependencyService.Get<IAuthenticationRepository>().RefreshToken();
                                if (!isRefresh && getMyProfileData == 3)
                                {
                                    Common.DisplayErrorMessage(Constraints.Session_Expired);
                                    App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                                }
                                else
                                {
                                    await GetMyProfileData();
                                }
                                getMyProfileData++;
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
                        await GetMyProfileData();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileAPI/GetMyProfileData: " + ex.Message);
            }
            return mResponse;
        }

        int getSellerDataById = 0;
        public async Task<Response> GetSellerDataById(string UserId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetSellerDataById, (int)App.Current.Resources["Version"], UserId);
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
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                var isRefresh = await DependencyService.Get<IAuthenticationRepository>().RefreshToken();
                                if (!isRefresh && getSellerDataById == 3)
                                {
                                    Common.DisplayErrorMessage(Constraints.Session_Expired);
                                    App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                                }
                                else
                                {
                                    await GetSellerDataById(UserId);
                                }
                                getSellerDataById++;
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
                        await GetSellerDataById(UserId);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileAPI/GetMyProfileData: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<bool> HasValidPincode(int pinCode)
        {
            bool result = false;
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(baseUrl: "https://api.postalpincode.in/"))
                    {
                        string url = string.Format(EndPointURL.ValidatePincode, pinCode);
                        var response = await hcf.GetAsync(url);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            var mResponsePincodes = JsonConvert.DeserializeObject<List<ResponsePincode>>(responseJson);
                            if (mResponsePincodes != null && mResponsePincodes.Count > 0)
                            {
                                var mResponsePincode = mResponsePincodes.FirstOrDefault();
                                if (mResponsePincode != null && mResponsePincode.Status == "Success")
                                    result = true;
                            }
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await HasValidPincode(pinCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileAPI/GetCountry: " + ex.Message);
            }
            return result;
        }
        #endregion

        #region [ POST ]
        public async Task<Response> FileUpload(FileUpload mFileUpload)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mFileUpload);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = EndPointURL.FileUpload;
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
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await FileUpload(mFileUpload);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("ProfileAPI/FileUpload: " + ex.Message);
            }
            return mResponse;
        }
        #endregion

        #region [ PUT ]
        int saveProfile = 0;
        public async Task<Response> SaveProfile(SellerDetails mSellerDetails)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mSellerDetails);
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.SaveProfile, (int)App.Current.Resources["Version"]);
                        var mResponseMessage = await hcf.PutAsync(url, requestJson);
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
                                if (!isRefresh && saveProfile == 3)
                                {
                                    Common.DisplayErrorMessage(Constraints.Session_Expired);
                                    App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                                }
                                else
                                {
                                    await SaveProfile(mSellerDetails);
                                }
                                saveProfile++;
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
                        await SaveProfile(mSellerDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("ProfileAPI/SaveProfile: " + ex.Message);
            }
            return mResponse;
        }
        #endregion
    }
}
