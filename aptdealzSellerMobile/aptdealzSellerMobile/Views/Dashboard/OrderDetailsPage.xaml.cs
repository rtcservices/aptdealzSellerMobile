using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.OtherPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        #region [ Objects ]
        private Order mOrder;
        private List<string> mOrderStatusList;
        private string OrderId;
        #endregion

        #region [ Constructor ]
        public OrderDetailsPage(string orderId)
        {
            try
            {
                InitializeComponent();
                mOrder = new Order();
                mOrderStatusList = new List<string>();
                OrderId = orderId;

                MessagingCenter.Unsubscribe<string>(this, "NotificationCount");
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/Ctor: " + ex.Message);
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
            OrderStatusList();
            GetOrderDetails();
        }

        private void OrderStatusList()
        {
            try
            {
                mOrderStatusList.Clear();
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
                    #region [ Details ]
                    if (mOrder.PickupProductDirectly)
                        lblPinCodeTitle.Text = "Product Pickup PIN Code";
                    else
                        lblPinCodeTitle.Text = "Shipping PIN Code";

                    lblOrderId.Text = mOrder.OrderNo;
                    lblOrderRequirementId.Text = mOrder.RequirementNo;
                    lblRequirementTitle.Text = mOrder.Title;
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


                    if (mOrder.ShippingAddressDetails != null &&
                        (!Common.EmptyFiels(mOrder.ShippingAddressDetails.Building) ||
                        !Common.EmptyFiels(mOrder.ShippingAddressDetails.Street) ||
                        !Common.EmptyFiels(mOrder.ShippingAddressDetails.City) ||
                        !Common.EmptyFiels(mOrder.ShippingAddressDetails.PinCode) ||
                        !Common.EmptyFiels(mOrder.ShippingAddressDetails.Landmark) ||
                        !Common.EmptyFiels(mOrder.ShippingAddressDetails.State) ||
                        mOrder.ShippingAddressDetails.Country > 0))
                    {
                        lblShippingAddress.Text = mOrder.ShippingAddressDetails.Building + "\n"
                                                          + mOrder.ShippingAddressDetails.Street + ", " + mOrder.ShippingAddressDetails.City + "\n"
                                                          + mOrder.ShippingAddressDetails.State + " " + mOrder.ShippingAddressDetails.PinCode + "\n"
                                                          + mOrder.ShippingAddressDetails.Landmark + ", " + mOrder.ShippingAddressDetails.Country;
                    }
                    else
                    {
                        lblShippingAddress.Text = "No shipping address found";
                    }
                    #endregion

                    #region [ Shipping Details ]
                    if (!Common.EmptyFiels(mOrder.ShipperNumber))
                    {
                        txtShippingNumber.Text = mOrder.ShipperNumber;
                    }
                    if (!Common.EmptyFiels(mOrder.LrNumber))
                    {
                        txtLRNumber.Text = mOrder.LrNumber;
                    }
                    if (!Common.EmptyFiels(mOrder.BillNumber))
                    {
                        txtEWayBillNumber.Text = mOrder.BillNumber;
                    }
                    if (!Common.EmptyFiels(mOrder.TrackingLink))
                    {
                        txtTrackingLink.Text = mOrder.TrackingLink;
                    }
                    #endregion

                    #region [ Status Buttons ]
                    if (mOrder.PickupProductDirectly)  //Pickup from seller
                    {
                        BtnScanQRCode.IsVisible = true;
                        BtnRaiseGrievance.IsVisible = true;
                        BtnUpdate.IsVisible = false;
                        GrdUpdateStatus.IsVisible = false;
                        StkNoUpdation.IsVisible = false;
                    }

                    if (mOrder.OrderStatus == (int)OrderStatus.ReadyForPickup)
                    {
                        BtnScanQRCode.IsVisible = true;
                        BtnRaiseGrievance.IsVisible = true;
                        BtnUpdate.IsVisible = false;
                        GrdUpdateStatus.IsVisible = false;
                        StkNoUpdation.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Delivered)
                    {
                        GrdUpdateStatus.IsVisible = true;
                        txtShippingNumber.IsReadOnly = true;
                        txtLRNumber.IsReadOnly = true;
                        txtEWayBillNumber.IsReadOnly = true;
                        txtTrackingLink.IsReadOnly = true;
                        GrdChargesAndEarnings.IsVisible = true;

                        BtnRaiseGrievance.IsVisible = false;
                        StkNoUpdation.IsVisible = false;
                        BtnUpdate.IsVisible = true;
                        BtnScanQRCode.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Completed)
                    {
                        StkNoUpdation.IsVisible = true;
                        GrdChargesAndEarnings.IsVisible = true;

                        BtnRaiseGrievance.IsVisible = false;
                        BtnUpdate.IsVisible = false;
                        GrdUpdateStatus.IsVisible = false;
                        BtnScanQRCode.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.CancelledFromBuyer)
                    {
                        BtnScanQRCode.IsVisible = false;
                        BtnUpdate.IsVisible = false;
                        GrdUpdateStatus.IsVisible = false;
                        StkNoUpdation.IsVisible = false;
                        BtnRaiseGrievance.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Shipped)
                    {
                        BtnRaiseGrievance.IsVisible = true;
                        BtnScanQRCode.IsVisible = false;
                        BtnUpdate.IsVisible = true;
                        GrdUpdateStatus.IsVisible = true;
                    }
                    else //Update Status
                    {
                        BtnScanQRCode.IsVisible = false; BtnRaiseGrievance.IsVisible = false;
                        BtnUpdate.IsVisible = true;
                        GrdUpdateStatus.IsVisible = true;
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

        private async Task UpdateOrder()
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
                    BtnUpdate.IsEnabled = false;
                    txtShippingNumber.Text = string.Empty;
                    txtLRNumber.Text = string.Empty;
                    txtEWayBillNumber.Text = string.Empty;
                    txtTrackingLink.Text = string.Empty;
                    await GetOrderDetails();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/UpdateOrder: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            var Tab = (Grid)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    await Navigation.PushAsync(new NotificationPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/ImgNotification_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
        }

        private void ImgOrderStatusPck_Clicked(object sender, EventArgs e)
        {
            pckOrderStatus.Focus();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private async void BtnScanQRCode_Clicked(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnScanQRCode);
                    await Navigation.PushAsync(new QrCodeScanPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnScanQRCode_Clicked: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void BtnUpdate_Tapped(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnUpdate);
                    await UpdateOrder();
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnUpdate_Tapped: " + ex.Message);
                }
                //finally
                //{
                //    Tab.IsEnabled = true;
                //}
            }
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            try
            {
                rfView.IsRefreshing = true;
                await GetOrderDetails();
                rfView.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/RefreshView_Refreshing: " + ex.Message);
            }
        }

        private void pckOrderStatus_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                if (pckOrderStatus.SelectedIndex != -1)
                {
                    BoxOrderStatus.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/pckOrderStatus_Unfocused: " + ex.Message);
            }
        }

        private async void CopyString_Tapped(object sender, EventArgs e)
        {
            var stackLayoutTab = (StackLayout)sender;
            if (stackLayoutTab.IsEnabled)
            {
                try
                {
                    stackLayoutTab.IsEnabled = false;
                    if (!Common.EmptyFiels(stackLayoutTab.ClassId))
                    {
                        if (stackLayoutTab.ClassId == "OrderId")
                        {
                            string message = Constraints.CopiedOrderId;
                            Common.CopyText(lblOrderId, message);
                        }
                        else if (stackLayoutTab.ClassId == "RequirementId")
                        {
                            if (!Common.EmptyFiels(mOrder.RequirementId))
                            {
                                string message = Constraints.CopiedRequirementId;
                                Common.CopyText(lblOrderRequirementId, message);
                                await Navigation.PushAsync(new RequirementDetailPage(mOrder.RequirementId));
                            }
                        }
                        else if (stackLayoutTab.ClassId == "QuoteRefeNo")
                        {
                            if (!Common.EmptyFiels(mOrder.QuoteId))
                            {
                                string message = Constraints.CopiedQuoteRefNo;
                                Common.CopyText(lblOrderReferenceNo, message);
                                await Navigation.PushAsync(new QuoteDetailsPage(mOrder.QuoteId));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/CopyString_Tapped: " + ex.Message);
                }
                finally
                {
                    stackLayoutTab.IsEnabled = true;
                }
            }
        }

        private void pckOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pckOrderStatus.SelectedIndex == -1)
                    return;

                if (Common.GetOrderStatus(pckOrderStatus.SelectedItem.ToString()) == (int)OrderStatus.Shipped)
                {
                    GrdShippingDetails.IsVisible = true;
                }
                else
                {
                    GrdShippingDetails.IsVisible = false;
                }

                if (pckOrderStatus.SelectedIndex != Common.GetOrderIndex(mOrder.OrderStatus))
                {
                    BtnUpdate.IsEnabled = true;
                }
                else
                {
                    BtnUpdate.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/pckOrderStatus_SelectedIndexChanged: " + ex.Message);
            }
        }

        private async void BtnRaiseGrievance_Clicked(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnRaiseGrievance);
                    await Navigation.PushAsync(new RaiseGrievancePage(OrderId));
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnRaiseGrievance_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }
        #endregion
    }
}