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

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderSupplyingView : ContentView
    {
        #region Objects
        public event EventHandler isRefresh;
        public event EventHandler isRefreshScanQR;
        public List<Order> mOrders = new List<Order>();
        private string filterBy = "";
        private string title = string.Empty;
        private int? statusBy = null;
        private bool? sortBy = null;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Cunstructor
        public OrderSupplyingView()
        {
            InitializeComponent();
            pageNo = 1;
            GetOrders(statusBy, filterBy, sortBy);
        }
        #endregion

        #region Method
        public async void GetOrders(int? StatusBy = null, string FilterBy = "", bool? SortBy = null)
        {
            try
            {
                OrderAPI orderAPI = new OrderAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await orderAPI.GetOrdersForSeller(StatusBy, FilterBy, SortBy, pageNo, pageSize);
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
                    lstOrderSupplyings.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    lblNoRecord.Text = mResponse.Message;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/GetOrders: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void BindList(List<Order> mOrderList)
        {
            try
            {
                if (mOrderList != null && mOrderList.Count > 0)
                {
                    lstOrderSupplyings.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstOrderSupplyings.ItemsSource = mOrderList.ToList();
                }
                else
                {
                    lstOrderSupplyings.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/BindList: " + ex.Message);
            }
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            App.Current.MainPage = new MasterData.MasterDataPage();
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (ImgSort.Source.ToString().Replace("File: ", "") == Constraints.Sort_ASC)
                {
                    ImgSort.Source = Constraints.Sort_DSC;
                    sortBy = false;
                }
                else
                {
                    ImgSort.Source = Constraints.Sort_ASC;
                    sortBy = true;
                }

                pageNo = 1;
                GetOrders(statusBy, filterBy, sortBy);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private async void FrmStatusBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                StatusPopup statusPopup = new StatusPopup(statusBy);
                statusPopup.isRefresh += (s1, e1) =>
                {
                    int result = (int)s1;
                };
                await PopupNavigation.Instance.PushAsync(statusPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/FrmStatusBy_Tapped: " + ex.Message);
            }
        }

        private void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var sortby = new FilterPopup(filterBy, "Active");
                sortby.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        filterBy = result;
                        lblFilterBy.Text = filterBy;
                        pageNo = 1;
                        BindList(mOrders);
                    }
                };
                PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/CustomEntry_Unfocused: " + ex.Message);
            }
        }

        private void ImgExpand_Tapped(object sender, EventArgs e)
        {

        }

        private void lstOrderDetail_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstOrderSupplyings.SelectedItem = null;

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
                Common.DisplayErrorMessage("OrderSupplyingView/BtnClose_Clicked: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    var OrderSearch = mOrders.Where(x => x.OrderNo.ToLower().Contains(entrSearch.Text.ToLower())).ToList();
                    BindList(OrderSearch);
                }
                else
                {
                    BindList(mOrders);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingview/entrSearch_TextChanged: " + ex.Message);
            }
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void GrdList_Tapped(object sender, EventArgs e)
        {
            var GridExp = (Grid)sender;
            var mOrder = GridExp.BindingContext as Order;
            if (mOrder.OrderStatusDescr.Contains("Cancelled"))
            {
                Navigation.PushAsync(new OrderDetailsPage(mOrder.OrderId, true));
            }
            else
            {
                Navigation.PushAsync(new OrderDetailsPage(mOrder.OrderId));
            }
        }

        private void lstOrderSupplyings_ItemAppearing(object sender, ItemVisibilityEventArgs e)
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
                            GetOrders(statusBy, filterBy, sortBy);
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
                Common.DisplayErrorMessage("OrderSupplyingView/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private void lstOrderSupplyings_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstOrderSupplyings.IsRefreshing = true;
                pageNo = 1;
                mOrders.Clear();
                GetOrders(statusBy, filterBy, sortBy);
                lstOrderSupplyings.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/Refreshing: " + ex.Message);
            }
        }

        private void BtnQRScan_Clicked(object sender, EventArgs e)
        {
            try
            {
                var mResponse = (Button)sender;
                if (mResponse != null)
                {
                    Common.BindAnimation(button: mResponse);
                    var isScanQrCode = (Order)mResponse.BindingContext;
                    if (isScanQrCode != null && isScanQrCode.ScanQRCode)
                    {
                        isRefreshScanQR?.Invoke(true, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/BindList: " + ex.Message);
            }
        }
        #endregion
    }
}