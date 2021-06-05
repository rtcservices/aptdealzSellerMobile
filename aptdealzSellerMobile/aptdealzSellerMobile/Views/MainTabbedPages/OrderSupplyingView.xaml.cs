using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Views.Dashboard;
using aptdealzSellerMobile.Views.Popup;
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
        private List<OrderSupplying> mOrderSupplyings = new List<OrderSupplying>();
        #endregion

        #region Cunstructor
        public OrderSupplyingView()
        {
            InitializeComponent();
            BindOrderSupliyData();
        }
        #endregion

        #region Method
        private void BindOrderSupliyData()
        {
            lstOrderSupplyings.ItemsSource = null;
            mOrderSupplyings = new List<OrderSupplying>()
            {
                new OrderSupplying
                {
                  InvoiceId="INV#123",
                  DeliveryStatus="Shipped",
                  InvAmount=3500,
                  DeliveryDate="12-01-2021",
                  ExpDeliveryDate ="20-10-2021",
                  IsVisible=false
                },
                new OrderSupplying
                {
                  InvoiceId="INV#123",
                  DeliveryStatus="Accepted",
                  InvAmount=3500,
                  DeliveryDate="03-01-2021",
                  ExpDeliveryDate ="20-10-2021",
                   IsVisible=false,
                },
                new OrderSupplying
                {
                  InvoiceId="INV#123",
                  DeliveryStatus="Shipped",
                  InvAmount=3500,
                  DeliveryDate="12-01-2021",
                  ExpDeliveryDate ="03-10-2021",
                   IsVisible=false
                },
                new OrderSupplying
                {
                  InvoiceId="INV#123",
                  DeliveryStatus="Commplited",
                  InvAmount=3500,
                  DeliveryDate="12-01-2021",
                  ExpDeliveryDate ="03-10-2021",
                   IsVisible=false
                },
                new OrderSupplying
                {
                  InvoiceId="INV#123",
                  DeliveryStatus="Read For Pickup",
                  InvAmount=3500,
                  DeliveryDate="12-01-2021",
                  ExpDeliveryDate ="03-10-2021",
                   IsVisible=true
                },
            };
            lstOrderSupplyings.ItemsSource = mOrderSupplyings.ToList();
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
            isRefresh?.Invoke(true, EventArgs.Empty);
        }

        private async void ImgSearch_Tapped(object sender, EventArgs e)
        {
            try
            {
                SearchPopup searchPopup = new SearchPopup();
                searchPopup.isRefresh += (s1, e1) =>
                {
                    lstOrderSupplyings.ItemsSource = mOrderSupplyings.ToList();

                };
                await PopupNavigation.Instance.PushAsync(searchPopup);
            }
            catch (Exception ex)
            {

            }
        }

        private async void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                SortByPopup sortByPopup = new SortByPopup();
                sortByPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!string.IsNullOrEmpty(result))
                    {
                        //Bind list as per result
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortByPopup);
            }
            catch (Exception ex)
            {
            }
        }

        private async void FrmStatus_Tapped(object sender, EventArgs e)
        {
            try
            {
                StatusPopup statusPopup = new StatusPopup();
                statusPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!string.IsNullOrEmpty(result))
                    {
                        //Bind list as per result
                    }
                };
                await PopupNavigation.Instance.PushAsync(statusPopup);
            }
            catch (Exception ex)
            {
            }
        }

        private void ImgExpand_Tapped(object sender, EventArgs e)
        {

        }

        private async void frmOrderSupplyingScanQRCode_Tapped(object sender, EventArgs e)
        {
            try
            {
                var frm = (Frame)sender;
                if (frm != null)
                {
                    await frm.ScaleTo(0.9, 100, Easing.Linear);
                    await frm.ScaleTo(1.0, 100, Easing.Linear);

                    var frmScanQrCode = (OrderSupplying)frm.BindingContext;
                    if (frmScanQrCode != null && frmScanQrCode.IsVisible == true)
                    {
                        if (isRefreshScanQR != null)
                            isRefreshScanQR(true, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void lstOrderDetail_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstOrderSupplyings.SelectedItem = null;
            Navigation.PushAsync(new UpdateOrderDetailPage());
        }
        #endregion
    }
}