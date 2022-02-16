using Acr.UserDialogs;
using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : ContentPage
    {
        #region [ Objects ]
        private string selectedView;
        private bool isNavigate = false;
        #endregion

        #region [ Constuctor ]
        public MainTabbedPage(string OpenView, bool isNavigate = false)
        {
            InitializeComponent();
            this.isNavigate = isNavigate;
            selectedView = OpenView;
            BindViews(selectedView);
            GetProfile();
        }
        #endregion

        #region [ Methods ]  
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

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                if (!Common.EmptyFiels(selectedView))
                {
                    if (selectedView == "Requirements" || selectedView == "Quotes"
                        || selectedView == "Orders" || selectedView == "Account"
                        || selectedView == "QrCodeScan" || selectedView == "AptDealz"
                        || selectedView == "Policies" || selectedView == "RaiseGrievances"
                        )
                    {
                        isNavigate = true;
                        selectedView = "Home";
                        BindViews("Home");
                    }
                }

                if (!isNavigate)
                {
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await DisplayAlert(Constraints.Alert, Constraints.DoYouWantToExit, Constraints.Yes, Constraints.No);
                            if (result)
                            {
                                Xamarin.Forms.DependencyService.Get<ICloseAppOnBackButton>().CloseApp("MainTabbedPage");
                            }
                        });
                    }
                }

                isNavigate = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        async void GetProfile()
        {
            try
            {
                await DependencyService.Get<IProfileRepository>().GetMyProfileData();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/GetProfile: " + ex.Message);
            }
        }

        private Color BindTextColor()
        {
            return (Application.Current.UserAppTheme == OSAppTheme.Light) ? (Color)App.Current.Resources["appColor4"] : (Color)App.Current.Resources["appColor6"];
        }

        private void BindViews(string view)
        {
            try
            {
                UnselectTab();
                if (view == "Home")
                {
                    imgHome.Source = Constraints.ImgHomeActive;
                    lblHome.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new HomeView());
                }
                else if (view == "Requirements")
                {
                    imgHome.Source = Constraints.ImgHomeActive;
                    lblHome.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new RequirementsView());
                }
                else if (view == "Quotes")
                {
                    imgQuotes.Source = Constraints.ImgQuoteActive;
                    lblQuotes.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new QuoteView());
                }
                else if (view == "Orders")
                {
                    imgOrders.Source = Constraints.ImgOrderActive;
                    lblOrders.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new OrderSupplyingView());
                }
                else if (view == "RaiseGrievances")
                {
                    grdMain.Children.Add(new OrderSupplyingView(true));
                }
                else if (view == "Account")
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    imgAccount.Source = Constraints.ImgAccountActive;
                    lblAccount.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new AccountView());
                    UserDialogs.Instance.HideLoading();
                }
                else if (view == "AptDealz")
                {
                    grdMain.Children.Add(new AboutAptDealzView());
                }
                else if (view == "FAQHelp")
                {
                    grdMain.Children.Add(new FaqHelpView());
                }
                else if (view == "Policies")
                {
                    grdMain.Children.Add(new TermsAndPoliciesView());
                }
                else
                {
                    imgHome.Source = Constraints.ImgHomeActive;
                    lblHome.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new HomeView());
                }
                selectedView = view;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/BindViews" + ex.Message);
            }
        }

        private void UnselectTab()
        {
            grdMain.Children.Clear();

            imgHome.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.ImgHome : Constraints.ImgHomeDark;
            imgQuotes.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.ImgQuote : Constraints.ImgQuoteDark;
            imgOrders.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.ImgOrder : Constraints.ImgOrderDark;
            imgAccount.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.ImgAccount : Constraints.ImgAccountDark;

            lblHome.TextColor = lblQuotes.TextColor = lblOrders.TextColor = lblAccount.TextColor = BindTextColor();
        }
        #endregion

        #region [ Events ]
        private void Tab_Tapped(object sender, EventArgs e)
        {
            var grid = (Grid)sender;
            try
            {
                if (!Common.EmptyFiels(grid.ClassId))
                {
                    if (grid.ClassId == "Home")
                    {
                        if (selectedView != "Home")
                        {
                            BindViews("Home");
                        }
                    }
                    else if (grid.ClassId == "Quotes")
                    {
                        if (selectedView != "Quotes")
                        {
                            this.isNavigate = true;
                            BindViews("Quotes");
                        }
                    }
                    else if (grid.ClassId == "Orders")
                    {
                        if (selectedView != "Orders")
                        {
                            this.isNavigate = true;
                            BindViews("Orders");
                        }
                    }
                    else if (grid.ClassId == "Account")
                    {
                        if (selectedView != "Account")
                        {
                            this.isNavigate = true;
                            BindViews("Account");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/Tab_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}