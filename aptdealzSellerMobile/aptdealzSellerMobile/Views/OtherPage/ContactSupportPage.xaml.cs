﻿using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.OtherPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactSupportPage : ContentPage
    {
        #region [ Objects ]
        SupportChatAPI supportChatAPI;
        private List<ChatSupport> mMessageList;
        #endregion

        #region [ Constructor ]
        public ContactSupportPage()
        {
            try
            {
                InitializeComponent();
                supportChatAPI = new SupportChatAPI();
                mMessageList = new List<ChatSupport>();
                MessagingCenter.Unsubscribe<string>(this, "NotificationCount"); MessagingCenter.Subscribe<string>(this, "NotificationCount", (count) =>
                {
                    if (!Common.EmptyFiels(Common.NotificationCount))
                    {
                        lblNotificationCount.Text = count;
                        frmNotification.IsVisible = true;
                    }
                    else
                    {
                        frmNotification.IsVisible = false;
                        lblNotificationCount.Text = string.Empty;
                    }
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ContactSupportPage/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Method ]
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Dispose();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetMessages();
        }

        private async Task GetMessages(bool isGetChatList = true)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = new Response();
                if (isGetChatList)
                {
                    mResponse = await supportChatAPI.GetAllMyChat();
                }
                else
                {
                    if (!Common.EmptyFiels(txtMessage.Text))
                    {
                        mResponse = await supportChatAPI.SendChatSupportMessage(txtMessage.Text);
                    }
                    else
                    {
                        return;
                    }
                }

                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    if (result != null)
                    {
                        txtMessage.Text = string.Empty;
                        mMessageList = result.ToObject<List<ChatSupport>>();
                        if (mMessageList != null && mMessageList.Count > 0)
                        {
                            foreach (var message in mMessageList)
                            {
                                if (!message.IsMessageFromSupportTeam)
                                {
                                    //User Data
                                    message.IsContact = message.IsMessageFromSupportTeam;
                                    message.IsUser = true;
                                }
                                if (message.ChatMessageFromUserProfileImage == "")
                                {
                                    if (message.IsMessageFromSupportTeam)
                                    {
                                        message.ChatMessageFromUserProfileImage = "imgContact.jpg";
                                    }
                                    else
                                    {
                                        message.ChatMessageFromUserProfileImage = "iconUserAccount.png";
                                    }
                                }
                            }
                            lstChar.IsVisible = true;
                            lblNoRecord.IsVisible = false;
                            lstChar.ItemsSource = mMessageList.ToList();
                        }
                        else
                        {
                            lstChar.IsVisible = false;
                            lblNoRecord.IsVisible = true;
                        }
                    }
                }
                else
                {
                    lstChar.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                        lblNoRecord.Text = mResponse.Message;
                    else
                        lblNoRecord.Text = Constraints.Something_Wrong;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ContactSupportPage/GetMessages: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }
        #endregion

        #region [ Events ]
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            var Tab = (Grid)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    await Navigation.PushAsync(new NotificationPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("ContactSupportPage/ImgNotification_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void BtnSend_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: BtnSend);
            await GetMessages(false);
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void lstChar_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstChar.SelectedItem = null;
        }

        private async void lstChar_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstChar.IsRefreshing = true;
                mMessageList.Clear();
                await GetMessages();
                lstChar.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ContactSupportPage/lstChar_Refreshing: " + ex.Message);
            }
        }
        #endregion
    }
}