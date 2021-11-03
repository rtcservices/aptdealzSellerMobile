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
        public MasterDataPage(bool isNotification = false)
        {
            try
            {
                InitializeComponent();
                BindNavigation(isNotification);

                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += delegate
                {
                    if (App.stoppableTimer == null)
                    {
                        App.stoppableTimer = new StoppableTimer(TimeSpan.FromSeconds(3), () =>
                        {
                            GetNotificationCount();
                        });
                    }
                    App.stoppableTimer.Start();
                };
                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MasterDataPage/Constructor: " + ex.Message);
            }
        }

        void BindNavigation(bool isNotification = false)
        {
            try
            {
                Common.MasterData = this;
                Common.MasterData.Master = new MenuPage();
                if (isNotification == false)
                    Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
                else
                    Common.MasterData.Detail = new NavigationPage(new Views.Dashboard.NotificationPage());

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
                Common.DisplayErrorMessage("MasterDataPage/GetNotificationCount: " + ex.Message);
            }
        }
    }
}