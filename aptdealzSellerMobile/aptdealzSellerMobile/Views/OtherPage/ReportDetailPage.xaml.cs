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
    public partial class ReportDetailPage : ContentPage
    {
        #region [ Objects ]
        public List<Order> mOrders;
        private string filterBy = SortByField.Date.ToString();
        private int? statusBy = null;
        private string title = string.Empty;
        private bool? isAssending = false;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region [ Constructor ]
        public ReportDetailPage()
        {
            try
            {
                InitializeComponent();
                pageNo = 1;
                mOrders = new List<Order>();

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

                BindShippingData(statusBy, title, filterBy, isAssending);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/Ctor: " + ex.Message);
            }
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        private async void BindShippingData(int? StatusBy = null, string Title = "", string FilterBy = "", bool? SortBy = null, bool isLoader = true)
        {
            try
            {
                try
                {
                    OrderAPI orderAPI = new OrderAPI();
                    if (isLoader)
                    {
                        UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    }
                    var mResponse = await orderAPI.GetCompletedOrdersAgainstSeller(StatusBy, Title, FilterBy, SortBy, pageNo, pageSize);
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
                            mOrder.isSelectGrievance = false;
                            if (mOrder.OrderStatus == (int)Utility.OrderStatus.Shipped)
                                mOrder.ScanQRCode = true;
                            else if (mOrder.PickupProductDirectly && mOrder.OrderStatus == (int)Utility.OrderStatus.Shipped)
                                mOrder.ScanQRCode = true;
                            else
                                mOrder.ScanQRCode = false;

                            if (mOrders.Where(x => x.OrderId == mOrder.OrderId).Count() == 0)
                                mOrders.Add(mOrder);
                        }
                        BindList(mOrders);
                    }
                    else
                    {
                        lstReportDetails.IsVisible = false;
                        lblNoRecord.IsVisible = true;
                    }
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("ReportDetailPage/GetOrders: " + ex.Message);
                }
                finally
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/BindShippingData: " + ex.Message);
            }
        }

        private void BindList(List<Order> mOrderList)
        {
            try
            {
                if (mOrderList != null && mOrderList.Count > 0)
                {
                    lstReportDetails.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstReportDetails.ItemsSource = mOrderList.ToList();
                }
                else
                {
                    lstReportDetails.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/BindList: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgMenu);
                await Navigation.PushAsync(new OtherPage.SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/ImgMenu_Tapped: " + ex.Message);
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
                Common.DisplayErrorMessage("ReportDetailPage/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
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
                Common.DisplayErrorMessage("ReportDetailPage/BtnClose_Clicked: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    BindShippingData(statusBy, entrSearch.Text, filterBy, isAssending, false);
                }
                else
                {
                    pageNo = 1;
                    mOrders.Clear();
                    BindShippingData(statusBy, title, filterBy, isAssending);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/entrSearch_TextChanged: " + ex.Message);
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
                BindShippingData(statusBy, title, filterBy, isAssending);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private async void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                FilterPopup sortByPopup = new FilterPopup(filterBy, "Order");
                sortByPopup.isRefresh += (s1, e1) =>
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
                        BindShippingData(statusBy, title, filterBy, isAssending);
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortByPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/FrmFilterBy_Tapped: " + ex.Message);
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
                        BindShippingData(statusBy, title, filterBy, isAssending);
                    }
                };
                await PopupNavigation.Instance.PushAsync(statusPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/FrmStatusBy_Tapped: " + ex.Message);
            }
        }

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
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/ImgExpand_Tapped: " + ex.Message);
            }
        }

        private void lstReportDetails_ItemAppearing(object sender, ItemVisibilityEventArgs e)
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
                            BindShippingData(statusBy, title, filterBy, isAssending, false);
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
                Common.DisplayErrorMessage("ReportDetailPage/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private void lstReportDetails_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstReportDetails.IsRefreshing = true;
                pageNo = 1;
                mOrders.Clear();
                BindShippingData(statusBy, title, filterBy, isAssending);
                lstReportDetails.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/Refreshing: " + ex.Message);
            }
        }
        #endregion
    }
}