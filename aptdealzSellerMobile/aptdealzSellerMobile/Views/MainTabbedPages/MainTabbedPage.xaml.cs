using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : ContentPage
    {
        #region Objects
        Views.Dashboard.HomeView homeView;
        bool isQoute = false;
        #endregion

        #region Constuctor
        public MainTabbedPage(bool isQoute)
        {
            InitializeComponent();
            this.isQoute = isQoute;
            InitializeObject();
        }
        #endregion

        #region Methods
        private void InitializeObject()
        {
            grdMain.Children.Clear();
            imgHome.Source = Constraints.Home_Active_Img;
            lblHome.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
            ReirectHomeView();
            if (this.isQoute)
            {
                UnselectTab();
                grdMain.Children.Clear();
                imgRequirements.Source = Constraints.Quote_Active_Img;
                lblRequirements.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
                grdMain.Children.Add(new QuoteView());
            }
        }

        private void ReirectHomeView()
        {
            try
            {
                grdMain.Children.Clear();
                homeView = new HomeView();
                homeView.isRefresh += (s, e1) =>
                {
                    var viewToOpen = (string)s;
                    if (!string.IsNullOrEmpty(viewToOpen))
                    {
                        grdMain.Children.Clear();
                        if (viewToOpen == "Requirements")
                        {
                            OpenViewRequirements();
                        }

                        else if (viewToOpen == "Submitted")
                        {
                            OpenQuoteSubmit();
                        }

                        else if (viewToOpen == "Supplying")
                        {
                            OpenOrderSupplying();
                        }

                        else if (viewToOpen == "AccountProfile")
                        {
                            OpenAccountProfile();

                        }

                        else if (viewToOpen == "AptDealz")
                        {
                            NavigatetoAboutaptzDealz();
                        }

                        else if (viewToOpen == "Policies")
                        {
                            NavigatePolicieView();
                        }
                    }
                };
                grdMain.Children.Add(homeView);
            }
            catch (Exception ex)
            {
                //DisplayInfo.ErrorMessage(ex.Message);
            }
        }

        private void OpenViewRequirements()
        {
            UnselectTab();
            imgHome.Source = Constraints.Home_Active_Img;
            lblHome.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
            RequirementsView requirmentview = new RequirementsView();
            requirmentview.isRefresh += (s1, e2) =>
            {
                var isBack = (bool)s1;
                if (isBack)
                {
                    ReirectHomeView();
                }
            };
            grdMain.Children.Add(requirmentview);
        }

        private void OpenQuoteSubmit()
        {
            UnselectTab();
            imgRequirements.Source = Constraints.Quote_Active_Img;
            lblRequirements.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
            QuoteView quoteView = new QuoteView();
            quoteView.isRefresh += (s1, e2) =>
            {
                var isBack = (bool)s1;
                if (isBack)
                {
                    UnselectTab();
                    imgHome.Source = Constraints.Quote_Active_Img;
                    lblHome.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
                    ReirectHomeView();
                }
            };
            grdMain.Children.Add(quoteView);
        }

        private void OpenOrderSupplying()
        {
            UnselectTab();
            imgOrders.Source = Constraints.Order_Active_Img;
            lblOrders.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
            OrderSupplyingView ordersuppliyView = new OrderSupplyingView();
            ordersuppliyView.isRefresh += (s1, e2) =>
            {
                var isBack = (bool)s1;
                if (isBack)
                {
                    UnselectTab();
                    imgHome.Source = Constraints.Home_Active_Img;
                    lblHome.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
                    ReirectHomeView();
                }
            };
            ordersuppliyView.isRefreshScanQR += (s2, e4) =>
            {
                var isQRShow = (bool)s2;
                if (isQRShow)
                {
                    UnselectTab();
                    imgOrders.Source = Constraints.Order_Active_Img;
                    lblOrders.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
                    QrCodeScanView qrCodeScanView = new QrCodeScanView();
                    qrCodeScanView.isRefresh += (sq, eq) =>
                    {
                        grdMain.Children.Clear();
                        grdMain.Children.Add(ordersuppliyView);
                    };
                    grdMain.Children.Add(qrCodeScanView);
                }
            };
            grdMain.Children.Add(ordersuppliyView);
        }

        private void OpenAccountProfile()
        {
            UnselectTab();
            imgAccount.Source = Constraints.Account_Active_Img;
            lblAccount.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
            AccountView accountView = new AccountView();
            accountView.isRefresh += (s1, e2) =>
            {
                var isBack = (bool)s1;
                if (isBack)
                {
                    UnselectTab();
                    imgHome.Source = Constraints.Home_Active_Img;
                    lblHome.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
                    ReirectHomeView();
                }
            };
            grdMain.Children.Add(accountView);
        }

        private void NavigatetoAboutaptzDealz()
        {
            AboutAptDealzView aptdealzView = new AboutAptDealzView();
            aptdealzView.isRefresh += (s1, e2) =>
            {
                var isBack = (bool)s1;
                if (isBack)
                {
                    ReirectHomeView();
                }
            };
            grdMain.Children.Add(aptdealzView);
        }

        private void NavigatePolicieView()
        {
            TermsAndPoliciesView policieView = new TermsAndPoliciesView();
            policieView.isRefresh += (s1, e2) =>
            {
                var isBack = (bool)s1;
                if (isBack)
                {
                    ReirectHomeView();
                }
            };
            grdMain.Children.Add(policieView);
        }

        private void UnselectTab()
        {
            imgHome.Source = Constraints.Home_Img;
            imgRequirements.Source = Constraints.Quote_Img;
            imgOrders.Source = Constraints.Order_Img;
            imgAccount.Source = Constraints.Account_Img;
            lblHome.TextColor = (Color)App.Current.Resources["DarkBlackColor"]; 
            lblRequirements.TextColor = (Color)App.Current.Resources["DarkBlackColor"]; 
            lblOrders.TextColor = (Color)App.Current.Resources["DarkBlackColor"]; 
            lblAccount.TextColor = (Color)App.Current.Resources["DarkBlackColor"]; 
        }
        #endregion

        #region Events
        private void Home_Tapped(object sender, EventArgs e)
        {
            UnselectTab();
            grdMain.Children.Clear();
            imgHome.Source = Constraints.Home_Active_Img;
            lblHome.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
            grdMain.Children.Add(new HomeView());
            ReirectHomeView();
        }

        private void Quotes_Tapped(object sender, EventArgs e)
        {
            UnselectTab();
            grdMain.Children.Clear();
            imgRequirements.Source = Constraints.Quote_Active_Img;
            lblRequirements.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
            QuoteView quoteView = new QuoteView();
            quoteView.isRefresh += (s1, e2) =>
            {
                var isBack = (bool)s1;
                if (isBack)
                {
                    UnselectTab();
                    imgHome.Source = Constraints.Home_Active_Img;
                    lblHome.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
                    ReirectHomeView();
                }
            };
            grdMain.Children.Add(quoteView);
        }

        private void Orders_Tapped(object sender, EventArgs e)
        {
            UnselectTab();
            grdMain.Children.Clear();
            imgOrders.Source = Constraints.Order_Active_Img;
            lblOrders.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
            OrderSupplyingView ordersuppliyView = new OrderSupplyingView();
            ordersuppliyView.isRefreshScanQR += (s1, e2) =>
            {
                var isQRShow = (bool)s1;
                if (isQRShow)
                {
                    UnselectTab();
                    imgOrders.Source = Constraints.Order_Active_Img;
                    lblOrders.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
                    QrCodeScanView qrCodeScanView = new QrCodeScanView();
                    qrCodeScanView.isRefresh += (sq, eq) =>
                    {
                        grdMain.Children.Clear();
                        grdMain.Children.Add(ordersuppliyView);
                    };
                    grdMain.Children.Add(qrCodeScanView);

                }
            };
            ordersuppliyView.isRefresh += (s1, e2) =>
            {
                var isBack = (bool)s1;
                if (isBack)
                {
                    UnselectTab();
                    imgHome.Source = Constraints.Home_Active_Img;
                    lblHome.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
                    ReirectHomeView();
                }
            };
            grdMain.Children.Add(ordersuppliyView);
        }

        private void Account_Tapped(object sender, EventArgs e)
        {
            UnselectTab();
            grdMain.Children.Clear();
            imgAccount.Source = Constraints.Account_Active_Img;
            lblAccount.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
            AccountView accountView = new AccountView();
            accountView.isRefresh += (s1, e2) =>
            {
                var isBack = (bool)s1;
                if (isBack)
                {
                    UnselectTab();
                    imgHome.Source = Constraints.Home_Active_Img;
                    lblHome.TextColor = (Color)App.Current.Resources["DarkOrangeColor"];
                    ReirectHomeView();
                }
            };
            grdMain.Children.Add(accountView);
        }

        #endregion
    }
}