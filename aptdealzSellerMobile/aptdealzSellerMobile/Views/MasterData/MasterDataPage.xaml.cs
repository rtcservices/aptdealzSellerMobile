using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MasterData
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDataPage : MasterDetailPage
    {
        public MasterDataPage()
        {
            try
            {
                InitializeComponent();
                BindNavigation();

                GetNotificationCount();
                MessagingCenter.Unsubscribe<string>(this, Constraints.NotificationReceived);
                MessagingCenter.Subscribe<string>(this, Constraints.NotificationReceived, (response) =>
                {
                    //GetNotificationCount();
                    if (!string.IsNullOrWhiteSpace(Common.NotificationCount))
                    {
                        try
                        {
                            var cnt = Convert.ToInt32(Common.NotificationCount) + 1;
                            Common.NotificationCount = cnt.ToString();
                        }
                        catch
                        {

                        }
                    }
                    MessagingCenter.Send<string>(Common.NotificationCount, Constraints.Str_NotificationCount);
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MasterDataPage/Constructor: " + ex.Message);
            }
        }

        void BindNavigation()
        {
            try
            {
                Common.MasterData = this;
                Common.MasterData.Master = new MenuPage();
                
                if (!Settings.IsNotification )
                {
                    Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
                }
                else
                {
                    if (Common.mSellerDetails != null &&Common.mSellerDetails.SellerId != null && !Common.EmptyFiels(Common.Token))
                    {
                        Common.MasterData.Detail = new NavigationPage(new Dashboard.NotificationPage());
                    }
                    else
                    {
                        Common.MasterData.Detail = new Views.SplashScreen.SplashScreen();
                    }
                    Settings.IsNotification = false;
                }

                MasterBehavior = MasterBehavior.Popover;
                Common.MasterData.IsGestureEnabled = false;
                Common.MasterData.IsPresented = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MasterDataPage/BindNavigation: " + ex.Message);
            }
        }

        private async void GetNotificationCount()
        {
            try
            {
                var notificationCount = await DependencyService.Get<INotificationRepository>().GetNotificationCount();
                if (!Common.EmptyFiels(notificationCount))
                {
                    Common.NotificationCount = notificationCount;
                    MessagingCenter.Send<string>(Common.NotificationCount, Constraints.Str_NotificationCount);
                }
            }
            catch (Exception ex)
            {
                //Common.DisplayErrorMessage("MasterDataPage/GetNotificationCount: " + ex.Message);
            }
        }
    }
}