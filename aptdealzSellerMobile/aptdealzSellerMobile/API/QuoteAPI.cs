using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
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
    public class QuoteAPI
    {
        int saveQuote = 0;
        public async Task<Response> SaveQuote(Quote mQuote)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mQuote);
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.CreateQuote, (int)App.Current.Resources["Version"]);
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
                                if (!isRefresh && saveQuote == 3)
                                {
                                    Common.DisplayErrorMessage(Constraints.Session_Expired);
                                    App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                                }
                                else
                                {
                                    await SaveQuote(mQuote);
                                }
                                saveQuote++;
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
                        await SaveQuote(mQuote);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("QuoteAPI/SaveQuote: " + ex.Message);
            }
            return mResponse;
        }
    }
}
