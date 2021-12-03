using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.MainTabbedPages;
using aptdealzSellerMobile.Views.OtherPage;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentView
    {
        #region [ Constructor ]
        public HomeView()
        {
            try
            {
                InitializeComponent();
                BindMenus();

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
                Common.DisplayErrorMessage("HomeView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        private void BindMenus()
        {
            try
            {
                List<HomeMenu> HomeMenus;
                HomeMenus = new List<HomeMenu>()
                {
                    new HomeMenu() { MenuImage = "iconViewReq.png",UiName = "View\nRequirements", MenuName = "Requirements"},
                    new HomeMenu() { MenuImage = "iconSubmitedQuotes.png",UiName = "Quotes\nSubmitted", MenuName = "Quotes"},
                    new HomeMenu() { MenuImage = "iconSupplying.png",UiName = "Orders For\nSupplying", MenuName = "Orders"},
                    new HomeMenu() { MenuImage = "imgProfile.png",UiName = "Account /\nProfile", MenuName = "Account"},
                    new HomeMenu() { MenuImage = "imgNotifications.png", UiName = "Notifications", MenuName = "Notifications"},
                    new HomeMenu() { MenuImage = "imgGrievances.png", UiName = "Grievances", MenuName = "Grievances"},
                    new HomeMenu() { MenuImage = "imgAboutAptDealz.png", UiName = "About\nAptDealz", MenuName = "AptDealz"},
                    new HomeMenu() { MenuImage = "iconTandP.png", UiName = "Terms & Policies", MenuName = "Policies"},
                    new HomeMenu() { MenuImage = "imgContactSupport.png", UiName = "Contact\nSupport", MenuName = "Support"},
                    new HomeMenu() { MenuImage = "iconCurrentlyShip.png", UiName = "Currently\nShipping", MenuName = "Shipping"},
                    new HomeMenu() { MenuImage = "iconReports.png", UiName = "Reports", MenuName = "Reports"},
                    new HomeMenu() { MenuImage = "imgWeSupport.png", UiName = "We Support", MenuName = "WeSupport"},
                };

                flvMenus.FlowItemsSource = HomeMenus.ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("HomeView/BindMenus: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgMenu);
                Navigation.PushAsync(new SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("HomeView/ImgMenu_Tapped: " + ex.Message);
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
                Common.DisplayErrorMessage("HomeView/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private async void DashboardMenu_Tapped(object sender, EventArgs e)
        {
            var MenuTab = (Frame)sender;
            try
            {
                var mHomeMenu = MenuTab.BindingContext as HomeMenu;
                if (mHomeMenu != null && mHomeMenu.MenuName == "Notifications")
                {
                    await Navigation.PushAsync(new NotificationPage());
                }
                else if (mHomeMenu != null && mHomeMenu.MenuName == "Grievances")
                {
                    await Navigation.PushAsync(new GrievancesPage());
                }
                else if (mHomeMenu != null && mHomeMenu.MenuName == "Support")
                {
                    await Navigation.PushAsync(new ContactSupportPage());
                }
                else if (mHomeMenu != null && mHomeMenu.MenuName == "Shipping")
                {
                    await Navigation.PushAsync(new CurrentlyShippingPage());
                }
                else if (mHomeMenu != null && mHomeMenu.MenuName == "Reports")
                {
                    await Navigation.PushAsync(new ReportDetailPage());
                }
                else if (mHomeMenu != null && mHomeMenu.MenuName == "WeSupport")
                {
                    await Navigation.PushAsync(new WeSupportPage());
                }
                else if (mHomeMenu != null && mHomeMenu.MenuName != null)
                {
                    Common.MasterData.Detail = new NavigationPage(new MainTabbedPage(mHomeMenu.MenuName, true));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("HomeView/DashboardMenu_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}