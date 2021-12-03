using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using aptdealzSellerMobile.Views.OtherPage;
using aptdealzSellerMobile.Views.Popup;
using Newtonsoft.Json.Linq;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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
        #region [ Objects ]
        public event EventHandler isRefreshScanQR;
        public List<Order> mOrders;
        private string filterBy = SortByField.Date.ToString();
        private string title = string.Empty;
        private int? statusBy = null;
        private bool? isAssending = false;
        private bool isGrievance = false;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region [ Cunstructor ]
        public OrderSupplyingView(bool isGrievance = false)
        {
            try
            {
                InitializeComponent();
                pageNo = 1;
                mOrders = new List<Order>();
                this.isGrievance = isGrievance;
                GetOrders(statusBy, title, filterBy, isAssending);

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
                Common.DisplayErrorMessage("OrderSupplyingView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Method ]
        private async void GetOrders(int? StatusBy = null, string Title = "", string FilterBy = "", bool? SortBy = null, bool isLoader = true)
        {
            try
            {
                OrderAPI orderAPI = new OrderAPI();
                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }
                var mResponse = await orderAPI.GetOrdersForSeller(StatusBy, Title, FilterBy, SortBy, pageNo, pageSize);
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
                        if (isGrievance)
                        {
                            lblHeader.Text = "Raise Grievances";
                            mOrder.ScanQRCode = false;
                            mOrder.isSelectGrievance = true;
                        }
                        else
                        {
                            lblHeader.Text = "Order for Supplying";
                            mOrder.isSelectGrievance = false;
                            //if (mOrder.OrderStatus == (int)Utility.OrderStatus.ReadyForPickup)
                            //    mOrder.ScanQRCode = true;
                            if (mOrder.PickupProductDirectly && mOrder.OrderStatus == (int)Utility.OrderStatus.ReadyForPickup)
                                mOrder.ScanQRCode = true;
                            else
                                mOrder.ScanQRCode = false;
                        }

                        if (mOrders.Where(x => x.OrderId == mOrder.OrderId).Count() == 0)
                            mOrders.Add(mOrder);
                    }
                    BindList(mOrders);
                }
                else
                {
                    lstOrderSupplyings.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    // lblNoRecord.Text = mResponse.Message;
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

        private void BindList(List<Order> mOrderList)
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

        #region [ Events ]
        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgMenu);
                await Navigation.PushAsync(new OtherPage.SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/ImgMenu_Tapped: " + ex.Message);
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NotificationPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(imageButton: ImgBack);
                if (isGrievance)
                    await Navigation.PopAsync();
                else
                    Common.MasterData.Detail = new NavigationPage(new MainTabbedPage("Home"));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/ImgNotification_Tapped: " + ex.Message);
            }
        }

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
                GetOrders(statusBy, title, filterBy, isAssending);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private async void FrmStatusBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var statusPopup = new OrderStatusPopup(statusBy);
                statusPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        lblStatus.Text = result.ToCamelCase();
                        statusBy = Common.GetOrderStatus(result);
                        pageNo = 1;
                        mOrders.Clear();
                        GetOrders(statusBy, title, filterBy, isAssending);
                    }
                };
                await PopupNavigation.Instance.PushAsync(statusPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/FrmStatusBy_Tapped: " + ex.Message);
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
                        GetOrders(statusBy, title, filterBy, isAssending);
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/FrmFilterBy_Tapped: " + ex.Message);
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
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetOrders(statusBy, entrSearch.Text, filterBy, isAssending, false);
                }
                else
                {
                    pageNo = 1;
                    mOrders.Clear();
                    GetOrders(statusBy, title, filterBy, isAssending);
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
                Common.DisplayErrorMessage("OrderSupplyingview/GrdList_Tapped: " + ex.Message);
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
                            GetOrders(statusBy, title, filterBy, isAssending, false);
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
                GetOrders(statusBy, title, filterBy, isAssending);
                lstOrderSupplyings.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/Refreshing: " + ex.Message);
            }
        }

        private async void BtnQRScan_Clicked(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            var mOrder = (Order)Tab.BindingContext;

            try
            {
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (cameraStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
                    cameraStatus = results[Permission.Camera];
                    DependencyService.Get<ICameraPermission>().CameraPermission();
                }

                if (cameraStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    await Navigation.PushAsync(new QrCodeScanPage(mOrder));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingView/BindList: " + ex.Message);
            }
        }

        private async void BtnSelect_Tapped(object sender, EventArgs e)
        {
            var ButtonExp = (Button)sender;
            try
            {
                var mOrder = ButtonExp.BindingContext as Order;
                await Navigation.PushAsync(new RaiseGrievancePage(mOrder.OrderId));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/BtnSelect_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}