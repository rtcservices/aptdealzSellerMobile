﻿using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.SplashScreen;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aptdealzSellerMobile.API
{
    public class AuthenticationAPI
    {
        #region [POST]
        public async Task<Response> SellerAuth(Authenticate mRequestLogin)
        {
            Response mResponseLogin = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mRequestLogin);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.SellerAuth, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mResponseLogin = JsonConvert.DeserializeObject<Response>(responseJson);
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
                            mResponseLogin = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await SellerAuth(mRequestLogin);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponseLogin.Succeeded = false;
                mResponseLogin.Errors = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/SellerAuth: " + ex.Message);
            }
            return mResponseLogin;
        }

        public async Task<Response> ActivateUser(string userId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"userId\":\"" + userId + "\"}";
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.ActivateUser);
                        var response = await hcf.PostAsync(url, requestJson);
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
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                        await ActivateUser(userId);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/ActivateUser: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> SendOtpByEmail(string email)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"email\":\"" + email + "\"}";
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.SendOtpByEmail);
                        var response = await hcf.PostAsync(url, requestJson);
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
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await SendOtpByEmail(email);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/SendOtpByEmail: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> ValidateOtpForResetPassword(AuthenticateEmail mAuthenticateEmail)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = JsonConvert.SerializeObject(mAuthenticateEmail);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.ValidateOtpForResetPassword);
                        var response = await hcf.PostAsync(url, requestJson);
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
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await ValidateOtpForResetPassword(mAuthenticateEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/ValidateOtpForResetPassword: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> ResetPassword(ResetPassword mResetPassword)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = JsonConvert.SerializeObject(mResetPassword);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.ResetPassword);
                        var response = await hcf.PostAsync(url, requestJson);
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
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await ResetPassword(mResetPassword);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/ResetPassword: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> RefreshToken(string refreshToken)
        {
            Response mResponseToken = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"refreshToken\":\"" + refreshToken + "\"}";

                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.RefreshToken);
                        var response = await hcf.PostAsync(url, requestJson);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mResponseToken = JsonConvert.DeserializeObject<Response>(responseJson);
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
                            mResponseToken = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await RefreshToken(refreshToken);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponseToken.Succeeded = false;
                mResponseToken.Errors = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/RefreshToken: " + ex.Message);
            }
            return mResponseToken;
        }

        public async Task<Response> Logout(string refreshToken, string loginTrackingKey)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"refreshToken\":\"" + refreshToken + "\",\"loginTrackingKey\":\"" + loginTrackingKey + "\"}";
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.Logout);
                        var response = await hcf.PostAsync(url, requestJson);
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
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await Logout(refreshToken, loginTrackingKey);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/Logout: " + ex.Message);
            }
            return mResponse;
        }
        #endregion
    }
}