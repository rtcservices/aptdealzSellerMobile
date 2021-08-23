using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Interfaces;
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
                    if (selectedView == "Requirements" || selectedView == "Submitted"
                        || selectedView == "Supplying" || selectedView == "AccountProfile"
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
                                Xamarin.Forms.DependencyService.Get<ICloseAppOnBackButton>().CloseApp();
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
                ProfileAPI profileAPI = new ProfileAPI();
                var mResponse = await profileAPI.GetMyProfileData();
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        Common.mSellerDetails = jObject.ToObject<Model.Request.SellerDetails>();
                    }
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/GetProfile: " + ex.Message);
            }
        }

        private void BindViews(string view)
        {
            try
            {
                UnselectTab();
                if (view == "Home")
                {
                    imgHome.Source = Constraints.Img_Home_Activ;
                    lblHome.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new HomeView());
                }
                else if (view == "Requirements")
                {
                    imgHome.Source = Constraints.Img_Home_Activ;
                    lblHome.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new RequirementsView());
                }
                else if (view == "Submitted")
                {
                    imgQuotes.Source = Constraints.Img_Quote_Active;
                    lblQuotes.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new QuoteView());
                }
                else if (view == "Supplying")
                {
                    imgOrders.Source = Constraints.Img_Order_Active;
                    lblOrders.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new OrderSupplyingView());
                }
                else if (view == "RaiseGrievances")
                {
                    grdMain.Children.Add(new OrderSupplyingView(true));
                }
                else if (view == "AccountProfile")
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    imgAccount.Source = Constraints.Img_Account_Active;
                    lblAccount.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new AccountView());
                    UserDialogs.Instance.HideLoading();
                }
                else if (view == "AptDealz")
                {
                    grdMain.Children.Add(new AboutAptDealzView());
                }
                else if (view == "Policies")
                {
                    grdMain.Children.Add(new TermsAndPoliciesView());
                }
                else
                {
                    imgHome.Source = Constraints.Img_Home_Activ;
                    lblHome.TextColor = (Color)App.Current.Resources["Orange"];
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

            imgHome.Source = Constraints.Img_Home;
            imgQuotes.Source = Constraints.Img_Quote;
            imgOrders.Source = Constraints.Img_Order;
            imgAccount.Source = Constraints.Img_Account;

            lblHome.TextColor = (Color)App.Current.Resources["Black"];
            lblQuotes.TextColor = (Color)App.Current.Resources["Black"];
            lblOrders.TextColor = (Color)App.Current.Resources["Black"];
            lblAccount.TextColor = (Color)App.Current.Resources["Black"];
        }
        #endregion

        #region [ Events ]
        private void Tab_Tapped(object sender, EventArgs e)
        {
            var grid = (Grid)sender;
            if (grid.IsEnabled)
            {
                try
                {
                    grid.IsEnabled = false;
                    if (!Common.EmptyFiels(grid.ClassId))
                    {
                        if (grid.ClassId == "Home")
                        {
                            BindViews("Home");
                        }
                        else if (grid.ClassId == "Quotes")
                        {
                            this.isNavigate = true;
                            BindViews("Submitted");
                        }
                        else if (grid.ClassId == "Orders")
                        {
                            this.isNavigate = true;
                            BindViews("Supplying");
                        }
                        else if (grid.ClassId == "Account")
                        {
                            this.isNavigate = true;
                            BindViews("AccountProfile");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("MainTabbedPage/Tab_Tapped: " + ex.Message);
                }
                finally
                {
                    grid.IsEnabled = true;
                }
            }
        }
        #endregion
    }
}