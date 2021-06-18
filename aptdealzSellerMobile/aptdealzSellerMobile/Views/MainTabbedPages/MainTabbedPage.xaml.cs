using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using aptdealzSellerMobile.Views.OtherPage;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : ContentPage
    {
        #region Objects
        private string selectedView;
        #endregion

        #region Constuctor
        public MainTabbedPage(string OpenView)
        {
            InitializeComponent();
            selectedView = OpenView;
            BindViews(selectedView);
        }
        #endregion

        #region Methods  
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await DisplayAlert(Constraints.Alert, Constraints.DoYouWantToExit, Constraints.Yes, Constraints.No);
                        if (result)
                        {
                            Xamarin.Forms.DependencyService.Get<ICloseAppOnBackButton>().CloseApp();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        public void BindViews(string view)
        {
            try
            {
                if (view == "Home")
                {
                    UnselectTab();
                    imgHome.Source = Constraints.Img_Home_Activ;
                    lblHome.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new HomeView());
                }
                else if (view == "Requirements")
                {
                    UnselectTab();
                    imgHome.Source = Constraints.Img_Home_Activ;
                    lblHome.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new RequirementsView());
                }
                else if (view == "Submitted")
                {
                    UnselectTab();
                    imgRequirements.Source = Constraints.Img_Quote_Active;
                    lblRequirements.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new QuoteView());
                }
                else if (view == "Supplying")
                {
                    UnselectTab();
                    imgOrders.Source = Constraints.Img_Order_Active;
                    lblOrders.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new OrderSupplyingView());
                }
                else if (view == "AccountProfile")
                {
                    UnselectTab();
                    imgAccount.Source = Constraints.Img_Account_Active;
                    lblAccount.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new AccountView());
                }
                else if (view == "QrCodeScan")
                {
                    UnselectTab();
                    imgOrders.Source = Constraints.Img_Order_Active;
                    lblOrders.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new QrCodeScanView());
                }
                else if (view == "AptDealz")
                {
                    UnselectTab();
                    grdMain.Children.Add(new AboutAptDealzView());
                }
                else if (view == "Policies")
                {
                    UnselectTab();
                    grdMain.Children.Add(new TermsAndPoliciesView());
                }
                else
                {
                    UnselectTab();
                    imgHome.Source = Constraints.Img_Home_Activ;
                    lblHome.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new HomeView());
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/BindViews" + ex.Message);
            }
        }

        private void UnselectTab()
        {
            grdMain.Children.Clear();
            imgHome.Source = Constraints.Img_Home;
            imgRequirements.Source = Constraints.Img_Quote;
            imgOrders.Source = Constraints.Img_Order;
            imgAccount.Source = Constraints.Img_Account;
            lblHome.TextColor = (Color)App.Current.Resources["Black"];
            lblRequirements.TextColor = (Color)App.Current.Resources["Black"];
            lblOrders.TextColor = (Color)App.Current.Resources["Black"];
            lblAccount.TextColor = (Color)App.Current.Resources["Black"];
        }
        #endregion

        #region Events
        private void Home_Tapped(object sender, EventArgs e)
        {
            BindViews("Home");
        }

        private void Quotes_Tapped(object sender, EventArgs e)
        {
            BindViews("Submitted");
        }

        private void Orders_Tapped(object sender, EventArgs e)
        {
            BindViews("Supplying");
        }

        private void Account_Tapped(object sender, EventArgs e)
        {
            BindViews("AccountProfile");
        }
        #endregion
    }
}