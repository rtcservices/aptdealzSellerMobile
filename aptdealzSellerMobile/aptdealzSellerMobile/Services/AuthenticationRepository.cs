using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Services
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public async Task<bool> RefreshToken()
        {
            bool result = false;

            AuthenticationAPI authenticationAPI = new AuthenticationAPI();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await authenticationAPI.RefreshToken(Settings.RefreshToken);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mSeller = jObject.ToObject<Model.Reponse.Seller>();
                        if (mSeller != null)
                        {
                            Settings.UserId = mSeller.Id;
                            Settings.UserToken = mSeller.JwToken;
                            Common.Token = mSeller.JwToken;
                            Settings.RefreshToken = mSeller.RefreshToken;
                            Settings.LoginTrackingKey = mSeller.LoginTrackingKey == "00000000-0000-0000-0000-000000000000" ? Settings.LoginTrackingKey : mSeller.LoginTrackingKey;

                            result = true;
                        }
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
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("AuthenticationRepository/RefreshToken: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return result;
        }

        public async Task DoLogout()
        {
            try
            {
                var isClose = await App.Current.MainPage.DisplayAlert(Constraints.Logout, Constraints.AreYouSureWantLogout, Constraints.Yes, Constraints.No);
                if (isClose)
                {
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mResponse = await authenticationAPI.Logout(Settings.RefreshToken, Settings.LoginTrackingKey);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        //Common.DisplaySuccessMessage(mResponse.Message);
                    }
                    else
                    {
                        if (mResponse != null && !mResponse.Message.Contains("TrackingKey"))
                            Common.DisplayErrorMessage(mResponse.Message);
                    }

                    MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                    Common.ClearAllData();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/DoLogout: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task<Response> APIResponse(HttpResponseMessage httpResponseMessage)
        {
            Response mResponse = new Response();
            var responseJson = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage != null)
            {
                if (!Common.EmptyFiels(responseJson))
                {
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                    }
                    else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                        if (errorString == Constraints.Session_Expired)
                        {
                            mResponse.Message = Constraints.Session_Expired;
                            MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                            Common.ClearAllData();
                        }
                    }
                    else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        mResponse.Message = Constraints.ServiceUnavailable;
                        MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                        Common.ClearAllData();
                    }
                    else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        if (responseJson.Contains(Constraints.Str_Duplicate))
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else
                        {
                            mResponse.Message = Constraints.Something_Wrong_Server;
                            MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                            Common.ClearAllData();
                        }
                    }
                    else if (responseJson.Contains(Constraints.Str_AccountDeactivated) && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        if (Common.mSellerDetails != null && !Common.EmptyFiels(Common.mSellerDetails.FullName))
                            mResponse.Message = "Hey " + Common.mSellerDetails.FullName + ", your account is deactivated.Please contact customer support.";
                        else
                            mResponse.Message = "Hey, your account is deactivated.Please contact customer support.";

                        MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                        Common.ClearAllData();
                    }
                    else
                    {
                        if (responseJson.Contains(Constraints.Str_TokenExpired) || httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            var isRefresh = await DependencyService.Get<IAuthenticationRepository>().RefreshToken();
                            if (!isRefresh)
                            {
                                mResponse.Message = Constraints.Session_Expired;
                                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                                Common.ClearAllData();
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
                    mResponse.Succeeded = false;
                    mResponse.Message = Constraints.Something_Wrong;
                }
            }
            else
            {
                mResponse.Succeeded = false;
                mResponse.Message = Constraints.Something_Wrong;
            }
            return mResponse;
        }
    }
}
