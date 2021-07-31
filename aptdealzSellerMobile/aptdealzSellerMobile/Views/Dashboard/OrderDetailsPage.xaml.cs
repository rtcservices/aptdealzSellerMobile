using Acr.UserDialogs;
using aptdealzSellerMobile.Extention;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.OtherPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        #region Objects
        private Order mOrder;
        private List<string> mOrderStatusList;
        private string OrderId;
        private bool isShipped = false;
        #endregion

        #region Constructor
        public OrderDetailsPage(string orderId)
        {
            InitializeComponent();
            mOrder = new Order();
            mOrderStatusList = new List<string>();
            OrderId = orderId;

            MessagingCenter.Subscribe<string>(this, "NotificationCount", (count) =>
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
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            OrderStatusList();
            GetOrderDetails();
        }

        private void OrderStatusList()
        {
            try
            {
                mOrderStatusList.Add(OrderStatus.Pending.ToString());
                mOrderStatusList.Add(OrderStatus.Accepted.ToString());
                mOrderStatusList.Add(OrderStatus.ReadyForPickup.ToString().ToCamelCase());
                mOrderStatusList.Add(OrderStatus.Shipped.ToString());
                mOrderStatusList.Add(OrderStatus.Delivered.ToString());
                mOrderStatusList.Add(OrderStatus.Completed.ToString());
                mOrderStatusList.Add(OrderStatus.CancelledFromBuyer.ToString().ToCamelCase());

                pckOrderStatus.ItemsSource = mOrderStatusList.ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/OrderStatusList: " + ex.Message);
            }
        }

        private async Task GetOrderDetails()
        {
            try
            {
                mOrder = await DependencyService.Get<IOrderRepository>().GetOrderDetails(OrderId);
                if (mOrder != null)
                {
                    if (mOrder.PickupProductDirectly)
                        lblPinCodeTitle.Text = "Product Pickup PIN Code";
                    else
                        lblPinCodeTitle.Text = "Shipping PIN Code";

                    #region [ Details ]
                    lblOrderId.Text = mOrder.OrderNo;
                    lblOrderRequirementId.Text = mOrder.RequirementNo;
                    lblOrderReferenceNo.Text = mOrder.QuoteNo;
                    lblOrderBuyerName.Text = mOrder.BuyerContact.Name;
                    lblOrderSellerName.Text = mOrder.SellerContact.Name;
                    lblOrderPinCode.Text = mOrder.ShippingPincode;
                    lblOrderQuntity.Text = "" + mOrder.RequestedQuantity + " " + mOrder.Unit;
                    lblOrderUnitPrice.Text = "Rs " + mOrder.UnitPrice;
                    lblOrderNetAmount.Text = "Rs " + mOrder.NetAmount;
                    lblOrderHandlingCharge.Text = "Rs " + mOrder.HandlingCharges;
                    lblOrderShippingCharge.Text = "Rs " + mOrder.ShippingCharges;
                    lblOrderInsuranceCharge.Text = "Rs " + mOrder.InsuranceCharges;
                    lblOrderCountry.Text = mOrder.Country;
                    lblInvoiceNo.Text = mOrder.OrderNo;
                    lblTotalAmount.Text = "Rs " + mOrder.TotalAmount;
                    lblExpectedDate.Text = mOrder.ExpectedDelivery.ToString("dd/MM/yyyy");
                    lblBuyerContact.Text = mOrder.SellerContact.PhoneNumber;
                    lblOrderStatus.Text = mOrder.OrderStatusDescr;
                    lblPaymentStatus.Text = mOrder.PaymentStatusDescr;
                    pckOrderStatus.SelectedIndex = Common.GetOrderIndex(mOrder.OrderStatus);
                    #endregion

                    #region [ Address ]
                    lblBuyerAddress.Text = mOrder.BuyerAddressDetails.Building + "\n"
                                                         + mOrder.BuyerAddressDetails.Street + "\n"
                                                         + mOrder.BuyerAddressDetails.City + ", " + mOrder.BuyerAddressDetails.PinCode + "\n"
                                                         + mOrder.BuyerAddressDetails.Landmark + ", " + mOrder.BuyerAddressDetails.Country;
                    #endregion

                    #region [ Status Buttons ]
                    if (mOrder.PickupProductDirectly)  //Pickup from seller
                    {
                        BtnScanQRCode.IsVisible = true;
                        BtnUpdate.IsVisible = false;
                        StkUpdateStatus.IsVisible = false;
                        StkNoUpdation.IsVisible = false;
                    }

                    if (mOrder.OrderStatus == (int)OrderStatus.ReadyForPickup)
                    {
                        BtnScanQRCode.IsVisible = true;
                        BtnUpdate.IsVisible = false;
                        StkUpdateStatus.IsVisible = false;
                        StkNoUpdation.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Delivered || mOrder.OrderStatus == (int)OrderStatus.Completed)
                    {
                        StkNoUpdation.IsVisible = true;
                        GrdChargesAndEarnings.IsVisible = true;
                        BtnUpdate.IsVisible = false;
                        StkUpdateStatus.IsVisible = false;
                        BtnScanQRCode.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.CancelledFromBuyer)
                    {
                        BtnScanQRCode.IsVisible = false;
                        BtnUpdate.IsVisible = false;
                        StkUpdateStatus.IsVisible = false;
                        StkNoUpdation.IsVisible = false;
                    }
                    else //Update Status
                    {
                        BtnScanQRCode.IsVisible = false;
                        BtnUpdate.IsVisible = true;
                        StkUpdateStatus.IsVisible = true;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/GetOrderDetails: " + ex.Message);
            }
        }

        private bool UpdateOrderFieldsValidations()
        {
            bool isValid = false;
            try
            {
                if (pckOrderStatus.SelectedIndex == -1)
                {
                    Common.DisplayErrorMessage(Constraints.Required_OrderStatus);
                    BoxOrderStatus.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
                else if (isShipped)
                {
                    if (Common.EmptyFiels(txtShippingNumber.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.Required_ShippingNumber);
                        BoxShippingNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                    }
                    else if (Common.EmptyFiels(txtLRNumber.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.Required_LRNumber);
                        BoxLRNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                    }
                    else if (Common.EmptyFiels(txtEWayBillNumber.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.Required_EWayBillNumber);
                        BoxEWayBillNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/UpdateOrderFieldsValidations: " + ex.Message);
            }
            return isValid;
        }

        private void UnfocussedFields(Entry entry = null)
        {
            try
            {
                if (entry != null)
                {
                    if (entry.ClassId == "ShippingNumber")
                    {
                        BoxShippingNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "LRNumber")
                    {
                        BoxLRNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "EwayNumber")
                    {
                        BoxEWayBillNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/UnfocussedFields: " + ex.Message);
            }
        }


        private async void UpdateOrder()
        {
            try
            {
                if (UpdateOrderFieldsValidations())
                {
                    OrderUpdate mOrderUpdate = new OrderUpdate();
                    mOrderUpdate.OrderId = mOrder.OrderId;
                    mOrderUpdate.Status = Common.GetOrderStatus(pckOrderStatus.SelectedItem.ToString());

                    if (!Common.EmptyFiels(txtTrackingLink.Text))
                    {
                        mOrderUpdate.TrackingLink = txtTrackingLink.Text;
                    }

                    if (!Common.EmptyFiels(txtShippingNumber.Text))
                    {
                        mOrderUpdate.ShipperNumber = txtShippingNumber.Text;
                    }

                    if (!Common.EmptyFiels(txtLRNumber.Text))
                    {
                        mOrderUpdate.LrNumber = txtLRNumber.Text;
                    }

                    if (!Common.EmptyFiels(txtEWayBillNumber.Text))
                    {
                        mOrderUpdate.BillNumber = txtEWayBillNumber.Text;
                    }

                    await DependencyService.Get<IOrderRepository>().UpdateOrder(mOrderUpdate);
                    txtShippingNumber.Text = string.Empty;
                    txtLRNumber.Text = string.Empty;
                    txtEWayBillNumber.Text = string.Empty;
                    txtTrackingLink.Text = string.Empty;
                    BoxShippingNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    BoxLRNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    BoxEWayBillNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    await GetOrderDetails();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/UpdateOrder: " + ex.Message);
            }
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotificationPage());
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private void ImgOrderStatusPck_Clicked(object sender, EventArgs e)
        {
            pckOrderStatus.Focus();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void BtnScanQRCode_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnScanQRCode);
            Navigation.PushAsync(new QrCodeScanPage());
        }

        private void BtnUpdate_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnUpdate);
            UpdateOrder();
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            rfView.IsRefreshing = true;
            OrderStatusList();
            await GetOrderDetails();
            rfView.IsRefreshing = false;
        }

        private void pckOrderStatus_Unfocused(object sender, FocusEventArgs e)
        {
            if (pckOrderStatus.SelectedIndex != -1)
            {
                BoxOrderStatus.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (ExtEntry)sender;
            if (!Common.EmptyFiels(entry.Text))
            {
                UnfocussedFields(entry: entry);
            }
        }
        #endregion

        private void CopyString_Tapped(object sender, EventArgs e)
        {
            try
            {
                var stackLayout = (StackLayout)sender;
                if (!Common.EmptyFiels(stackLayout.ClassId))
                {
                    if (stackLayout.ClassId == "OrderId")
                    {
                        string message = Constraints.CopiedOrderId;
                        Common.CopyText(lblOrderId, message);
                    }
                    else if (stackLayout.ClassId == "RequirementId")
                    {
                        string message = Constraints.CopiedRequirementId;
                        Common.CopyText(lblOrderRequirementId, message);
                    }
                    else if (stackLayout.ClassId == "QuoteRefeNo")
                    {
                        string message = Constraints.CopiedQuoteRefNo;
                        Common.CopyText(lblOrderReferenceNo, message);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/CopyString_Tapped: " + ex.Message);
            }
        }

        private void pckOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Common.GetOrderStatus(pckOrderStatus.SelectedItem.ToString()) == (int)OrderStatus.Shipped)
            {
                isShipped = true;
                lblShippingNumber.IsVisible = true;
                lblLRNumber.IsVisible = true;
                lblEWayBillNumber.IsVisible = true;
            }
            else
            {
                isShipped = false;
                lblShippingNumber.IsVisible = false;
                lblLRNumber.IsVisible = false;
                lblEWayBillNumber.IsVisible = false;
            }
        }
    }
}