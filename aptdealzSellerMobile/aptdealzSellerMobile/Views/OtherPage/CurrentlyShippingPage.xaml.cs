using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using aptdealzSellerMobile.Views.Popup;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.OtherPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentlyShippingPage : ContentPage
    {
        #region [ Objects ]         
        public List<Order> mOrders;
        private string filterBy = SortByField.Date.ToString();
        private string title = string.Empty;
        private bool? isAssending = false;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region [ Constructor ]
        public CurrentlyShippingPage()
        {
            try
            {
                InitializeComponent();
                mOrders = new List<Order>();
                pageNo = 1;
                GetShippedOrders(title, filterBy, isAssending);

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
                Common.DisplayErrorMessage("CurrentlyShippingPage/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Method ]
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

        private async void GetShippedOrders(string Title = "", string FilterBy = "", bool? SortBy = null, bool isLoader = true)
        {
            try
            {
                OrderAPI orderAPI = new OrderAPI();
                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }
                var mResponse = await orderAPI.GetShippedOrdersForSeller(Title, FilterBy, SortBy, pageNo, pageSize);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    var orders = result.ToObject<List<Order>>();
                    if (pageNo == 1)
                    {
                        mOrders.Clear();
                    }

                    foreach (var mOrder in orders)
                    {
                        if (mOrders.Where(x => x.OrderId == mOrder.OrderId).Count() == 0)
                            mOrders.Add(mOrder);
                    }
                    BindList(mOrders);
                }
                else
                {
                    lstCurrentlyShipping.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    //lblNoRecord.Text = mResponse.Message;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/GetOrders: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BindList(List<Order> mOrderList)
        {
            try
            {
                if (mOrderList != null && mOrderList.Count > 0)
                {
                    lstCurrentlyShipping.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstCurrentlyShipping.ItemsSource = mOrderList.ToList();
                }
                else
                {
                    lstCurrentlyShipping.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/BindList: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        #region [ Header Navigation ]
        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgMenu);
                await Navigation.PushAsync(new OtherPage.SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/ImgMenu_Tapped: " + ex.Message);
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
                Common.DisplayErrorMessage("CurrentlyShippingPage/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion

        #region [ Filtering ]
        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ImgASC = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Sort_ASC : Constraints.Sort_ASC_Dark;
                var ImgDSC = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Sort_DSC : Constraints.Sort_DSC_Dark;

                if (ImgSort.Source.ToString().Replace("File: ", "") == ImgASC)
                {
                    ImgSort.Source = ImgDSC;
                    isAssending = false;
                }
                else
                {
                    ImgSort.Source = ImgASC;
                    isAssending = true;
                }

                pageNo = 1;
                mOrders.Clear();
                GetShippedOrders(title, filterBy, isAssending, true);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private async void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var sortby = new FilterPopup(filterBy, "Order");
                sortby.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        filterBy = result;
                        if (filterBy == SortByField.ID.ToString())
                        {
                            lblFilterBy.Text = filterBy;
                        }
                        else
                        {
                            lblFilterBy.Text = filterBy.ToCamelCase();
                        }
                        pageNo = 1;
                        mOrders.Clear();
                        GetShippedOrders(title, filterBy, isAssending);
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShipping/FrmFilterBy_Tapped: " + ex.Message);
            }
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            try
            {
                entrSearch.Text = string.Empty;
                BindList(mOrders);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/BtnClose_Clicked: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetShippedOrders(entrSearch.Text, filterBy, isAssending, false);
                }
                else
                {
                    pageNo = 1;
                    mOrders.Clear();
                    GetShippedOrders(title, filterBy, isAssending);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/entrSearch_TextChanged: " + ex.Message);
            }
        }
        #endregion

        #region [ Listing ]
        private void ImgExpand_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectGrid = (ImageButton)sender;
                var setHight = (ViewCell)selectGrid.Parent.Parent.Parent;
                if (setHight != null)
                {
                    setHight.ForceUpdateSize();
                }

                var mOrder = (Order)selectGrid.BindingContext;
                if (mOrder != null && mOrder.ArrowImage == Constraints.Arrow_Right)
                {
                    mOrder.ArrowImage = Constraints.Arrow_Down;
                    mOrder.GridBg = (Application.Current.UserAppTheme == OSAppTheme.Light) ? (Color)App.Current.Resources["appColor8"] : Color.Transparent;
                    mOrder.MoreDetail = true;
                    mOrder.OldDetail = false;
                }
                else
                {
                    mOrder.ArrowImage = Constraints.Arrow_Right;
                    mOrder.GridBg = Color.Transparent;
                    mOrder.MoreDetail = false;
                    mOrder.OldDetail = true;
                }

                //var response = (Order)selectGrid.BindingContext;
                //if (response != null)
                //{
                //    foreach (var selectedImage in mOrders)
                //    {
                //        if (selectedImage.ArrowImage == Constraints.Arrow_Right)
                //        {
                //            selectedImage.ArrowImage = Constraints.Arrow_Right;
                //            selectedImage.GridBg = Color.Transparent;
                //            selectedImage.MoreDetail = false;
                //            selectedImage.OldDetail = true;
                //        }
                //        else
                //        {
                //            selectedImage.ArrowImage = Constraints.Arrow_Down;
                //            selectedImage.GridBg = (Color)App.Current.Resources["appColor8"];
                //            selectedImage.MoreDetail = true;
                //            selectedImage.OldDetail = false;
                //        }
                //    }
                //    if (response.ArrowImage == Constraints.Arrow_Right)
                //    {
                //        response.ArrowImage = Constraints.Arrow_Down;
                //        response.GridBg = (Color)App.Current.Resources["appColor8"];
                //        response.MoreDetail = true;
                //        response.OldDetail = false;
                //    }
                //    else
                //    {
                //        response.ArrowImage = Constraints.Arrow_Right;
                //        response.GridBg = Color.Transparent;
                //        response.MoreDetail = false;
                //        response.OldDetail = true;
                //    }

                //}
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/ImgExpand_Tapped: " + ex.Message);
            }
        }

        private void BtnTrack_Clicked(object sender, EventArgs e)
        {
            try
            {
                var ButtonExp = (Button)sender;
                var mOrder = ButtonExp.BindingContext as Order;
                if (mOrder != null && !Common.EmptyFiels(mOrder.TrackingLink))
                {
                    if (mOrder.TrackingLink.IsValidURL())
                    {
                        var trackinglink = "http://" + mOrder.TrackingLink.Replace("http://", "").Replace("https://", "");
                        Xamarin.Essentials.Launcher.OpenAsync(new Uri(trackinglink));
                    }
                    else
                        Common.DisplayErrorMessage(Constraints.Invalid_URL);
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.URL_Not_Available);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/BtnTrack_Tapped: " + ex.Message);
            }
        }

        private void lstCurrentlyShipping_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstCurrentlyShipping.IsRefreshing = true;
                pageNo = 1;
                mOrders.Clear();
                GetShippedOrders(title, filterBy, isAssending);
                lstCurrentlyShipping.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/Refreshing: " + ex.Message);
            }
        }

        private void lstCurrentlyShipping_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {

            try
            {
                if (this.mOrders.Count < 10)
                    return;
                if (this.mOrders.Count == 0)
                    return;

                var lastrequirement = this.mOrders[this.mOrders.Count - 1];
                var lastAppearing = (Order)e.Item;
                if (lastAppearing != null)
                {
                    if (lastrequirement == lastAppearing)
                    {
                        var totalAspectedRow = pageSize * pageNo;
                        pageNo += 1;

                        if (this.mOrders.Count() >= totalAspectedRow)
                        {
                            GetShippedOrders(title, filterBy, isAssending, false);
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void GrdList_Tapped(object sender, EventArgs e)
        {
            var Tab = (Grid)sender;
            try
            {
                var mOrder = Tab.BindingContext as Order;
                await Navigation.PushAsync(new OrderDetailsPage(mOrder.OrderId));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/GrdList_Tapped: " + ex.Message);
            }
        }
        #endregion
        #endregion

    }
}