﻿using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsAndPoliciesView : ContentView
    {
        #region [ Constructor ]
        public TermsAndPoliciesView()
        {
            InitializeComponent();

            try
            {
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
                Common.DisplayErrorMessage("TermsAndPoliciesView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

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
                    Common.DisplayErrorMessage("TermsAndPoliciesView/ImgNotification_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPage("Home"));
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion
    }
}