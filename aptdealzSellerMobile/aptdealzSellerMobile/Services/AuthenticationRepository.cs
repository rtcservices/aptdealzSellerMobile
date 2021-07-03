using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
    }
}
