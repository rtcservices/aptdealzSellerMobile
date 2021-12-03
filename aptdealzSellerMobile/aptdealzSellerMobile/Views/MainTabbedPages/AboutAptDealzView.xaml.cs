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
    public partial class AboutAptDealzView : ContentView
    {
        #region [ Constructor ]
        public AboutAptDealzView()
        {
            InitializeComponent();
            BindAboutApzdealz();

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
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AboutAptDealzView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        async void BindAboutApzdealz()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                AppSettingsAPI appSettingsAPI = new AppSettingsAPI();
                var mResponse = await appSettingsAPI.AboutAptdealzSellerApp();
                UserDialogs.Instance.HideLoading();

                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mAboutAptDealz = jObject.ToObject<AboutAptDealz>();
                        if (mAboutAptDealz != null)
                        {
                            lblAbout.Text = mAboutAptDealz.About;
                            lblAddress1.Text = mAboutAptDealz.ContactAddressLine1;
                            lblAddress2.Text = mAboutAptDealz.ContactAddressLine2;
                            lblPincode.Text = "PIN - " + mAboutAptDealz.ContactAddressPincode;
                            lblEmail.Text = "Email : " + mAboutAptDealz.ContactAddressEmail;
                            lblPhoneNo.Text = "Phone : " + mAboutAptDealz.ContactAddressPhone;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AboutAptDealzView/BindAboutApzdealz: " + ex.Message);
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
                Common.DisplayErrorMessage("AboutAptDealzView/ImgMenu_Tapped: " + ex.Message);
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
                Common.DisplayErrorMessage("AboutAptDealzView/ImgNotification_Tapped: " + ex.Message);
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
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPage("Home"));
        }
        #endregion
    }
}