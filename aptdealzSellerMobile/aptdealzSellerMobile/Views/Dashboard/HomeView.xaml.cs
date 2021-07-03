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
        #region Objects

        #endregion

        #region Constructor
        public HomeView()
        {
            InitializeComponent();
            BindMenus();
        }
        #endregion

        #region Methods
        private void BindMenus()
        {
            List<HomeMenu> HomeMenus;
            HomeMenus = new List<HomeMenu>()
            {
                new HomeMenu() { MenuImage = "iconViewReq.png",UiName = "View \nRequirements",MenuName = "Requirements"},
                new HomeMenu() { MenuImage = "iconSubmitedQuotes.png",UiName = "Quotes \nSubmitted",MenuName = "Submitted"},
                new HomeMenu() { MenuImage = "iconSupplying.png",UiName = "Orders For \nSupplying",MenuName = "Supplying"},
                new HomeMenu() { MenuImage = "imgProfile.png",UiName = "Account / \nProfile",MenuName = "AccountProfile"},
                new HomeMenu() { MenuImage = "imgNotifications.png", UiName = "Notifications",MenuName = "Notifications"},
                new HomeMenu() { MenuImage = "imgGrievances.png", UiName = "Grievances",MenuName = "Grievances"},
                new HomeMenu() { MenuImage = "imgAboutAptDealz.png", UiName = "About \nAptDealz",MenuName = "AptDealz"},
                new HomeMenu() { MenuImage = "iconTandP.png", UiName = "Terms & Policies",MenuName = "Policies"},
                new HomeMenu() { MenuImage = "imgContactSupport.png", UiName = "Contact \nSupport",MenuName = "Support"},
                new HomeMenu() { MenuImage = "iconCurrentlyShip.png", UiName = "Currently \nShipping",MenuName = "Shipping"},
                new HomeMenu() { MenuImage = "iconReports.png", UiName = "Reports",MenuName = "Reports"},
                new HomeMenu() { MenuImage = "imgWeSupport.png", UiName = "We Support",MenuName = "WeSupport"},
            };

            flvMenus.FlowItemsSource = HomeMenus.ToList();
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotificationPage());
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void BtnMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                var frm = (Frame)sender;
                var menuName = frm.BindingContext as HomeMenu;

                if (menuName != null && menuName.MenuName == "Notifications")
                {
                    Navigation.PushAsync(new NotificationPage());
                }
                else if (menuName != null && menuName.MenuName == "Grievances")
                {
                    Navigation.PushAsync(new GrievancesPage());
                }
                else if (menuName != null && menuName.MenuName == "Support")
                {
                    Navigation.PushAsync(new ContactSupportPage());
                }
                else if (menuName != null && menuName.MenuName == "Shipping")
                {
                    Navigation.PushAsync(new CurrentlyShippingPage());
                }
                else if (menuName != null && menuName.MenuName == "Reports")
                {
                    Navigation.PushAsync(new ReportDetailPage());
                }
                else if (menuName != null && menuName.MenuName == "WeSupport")
                {
                    Navigation.PushAsync(new WeSupportPage());
                }
                else if (menuName != null && menuName.MenuName != null)
                {
                    Navigation.PushAsync(new MainTabbedPage(menuName.MenuName));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("HomeView/BtnMenu_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}