using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Utility;
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
                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount); MessagingCenter.Subscribe<string>(this, Constraints.Str_NotificationCount, (count) =>
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

                GetPrivacyPolicyTermsAndConditions();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("TermsAndPoliciesView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        private async void GetPrivacyPolicyTermsAndConditions()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                AppSettingsAPI appSettingsAPI = new AppSettingsAPI();
                var mResponse = await appSettingsAPI.GetPrivacyPolicyTermsAndConditions();
                UserDialogs.Instance.HideLoading();

                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mTermsAndPolicy = jObject.ToObject<TermsAndPolicy>();
                        if (mTermsAndPolicy != null)
                        {
                            lblTerms.Text = mTermsAndPolicy.tandC;
                            lblPolicy.Text = mTermsAndPolicy.privacyPolicy;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("TermsAndPoliciesView/GetPrivacyPolicyTermsAndConditions: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgMenu);
                await Navigation.PushAsync(new OtherPage.SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("TermsAndPoliciesView/ImgMenu_Tapped: " + ex.Message);
            }
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NotificationPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("TermsAndPoliciesView/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPage("Home"));
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion
    }
}