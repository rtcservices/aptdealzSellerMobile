using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.OtherPage;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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
        #region [ Objects ]
        private Order mOrder;
        private List<string> mOrderStatusList;
        private string OrderId;
        bool isFirstLoad = true;
        #endregion

        #region [ Constructor ]
        public OrderDetailsPage(string orderId)
        {
            try
            {
                InitializeComponent();
                mOrder = new Order();
                mOrderStatusList = new List<string>();

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    txtEWayBillNumber.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                    txtLRNumber.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                    txtShippingNumber.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                }

                OrderId = orderId;

                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                MessagingCenter.Subscribe<string>(this, Constraints.Str_NotificationCount, (count) =>
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

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetOrderDetails();
        }

        //private void OrderStatusList(bool pickupProductDirectly)
        //{
        //    try
        //    {
        //        mOrderStatusList.Clear();
        //        mOrderStatusList.Add(OrderStatus.Pending.ToString());
        //        mOrderStatusList.Add(OrderStatus.Accepted.ToString());

        //        if (pickupProductDirectly)
        //        {
        //            mOrderStatusList.Add(OrderStatus.ReadyForPickup.ToString().ToCamelCase());
        //        }
        //        else
        //        {
        //            mOrderStatusList.Add(OrderStatus.Shipped.ToString());
        //            mOrderStatusList.Add(OrderStatus.Delivered.ToString());
        //        }

        //        mOrderStatusList.Add(OrderStatus.Completed.ToString());

        //        pckOrderStatus.ItemsSource = mOrderStatusList.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.DisplayErrorMessage("OrderDetailsPage/OrderStatusList: " + ex.Message);
        //    }
        //}

        private void OrderStatusList(bool pickupProductDirectly, int OrderStatusData)
        {
            try
            {
                var orderStatusList = new List<int>();
                foreach (int item in Enum.GetValues(typeof(OrderStatus)))
                {
                    orderStatusList.Add(item);
                }
                if (!pickupProductDirectly)
                {
                    orderStatusList = orderStatusList.ToList();
                    orderStatusList.Remove((int)OrderStatus.ReadyForPickup);
                    orderStatusList = orderStatusList.Where(u => u >= OrderStatusData && u <= OrderStatusData + 2).ToList();
                }
                else
                {
                    orderStatusList = orderStatusList.Where(u => u >= OrderStatusData && u <= OrderStatusData + 1).ToList();
                }
                mOrderStatusList.Clear();

                foreach (int item in orderStatusList)
                {
                    mOrderStatusList.Add(Common.GetOrderStatusString(item));
                }

                //mOrderStatusList.Add(OrderStatus.Pending.ToString());
                //mOrderStatusList.Add(OrderStatus.Accepted.ToString());

                //if (pickupProductDirectly)
                //{
                //    mOrderStatusList.Add(OrderStatus.ReadyForPickup.ToString().ToCamelCase());
                //}
                //else
                //{
                //    mOrderStatusList.Add(OrderStatus.Shipped.ToString());
                //    mOrderStatusList.Add(OrderStatus.Delivered.ToString());
                //}
                //mOrderStatusList.Add(OrderStatus.Completed.ToString());
                pckOrderStatus.ItemsSource = mOrderStatusList.ToList();

                pckOrderStatus.SelectedIndex = orderStatusList.IndexOf(OrderStatusData);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/OrderStatusList: " + ex.Message);
            }
        }

        private void BindSellerAddress(BuyerAddressDetails mBuyerAddress)
        {
            try
            {
                if (mBuyerAddress != null &&
                       (!Common.EmptyFiels(mBuyerAddress.Building) || !Common.EmptyFiels(mBuyerAddress.Street) ||
                        !Common.EmptyFiels(mBuyerAddress.City) || !Common.EmptyFiels(mBuyerAddress.State) ||
                        !Common.EmptyFiels(mBuyerAddress.PinCode) || !Common.EmptyFiels(mBuyerAddress.Landmark) ||
                        !Common.EmptyFiels(mBuyerAddress.Country)))
                {
                    FrmBuyerAddress.IsVisible = true;
                    List<string> addresses = new List<string>();

                    if (!Common.EmptyFiels(mBuyerAddress.Building))
                    {
                        addresses.Add(mBuyerAddress.Building);
                    }

                    if (!Common.EmptyFiels(mBuyerAddress.Street) && !Common.EmptyFiels(mBuyerAddress.City))
                    {
                        addresses.Add(mBuyerAddress.Street + ", " + mBuyerAddress.City);
                    }
                    else
                    {
                        if (!Common.EmptyFiels(mBuyerAddress.Street))
                        {
                            addresses.Add(mBuyerAddress.Street);
                        }
                        if (!Common.EmptyFiels(mBuyerAddress.City))
                        {
                            addresses.Add(mBuyerAddress.City);
                        }
                    }

                    if (!Common.EmptyFiels(mBuyerAddress.State) && !Common.EmptyFiels(mBuyerAddress.PinCode))
                    {
                        addresses.Add(mBuyerAddress.State + " " + mBuyerAddress.PinCode);
                    }
                    else
                    {
                        if (!Common.EmptyFiels(mBuyerAddress.State))
                        {
                            addresses.Add(mBuyerAddress.State);
                        }
                        if (!Common.EmptyFiels(mBuyerAddress.PinCode))
                        {
                            addresses.Add(mBuyerAddress.PinCode);
                        }
                    }

                    if (!Common.EmptyFiels(mBuyerAddress.Landmark))
                    {
                        addresses.Add(mBuyerAddress.Landmark + ", " + mBuyerAddress.Country);
                    }
                    else
                    {
                        if (!Common.EmptyFiels(mBuyerAddress.Landmark))
                        {
                            addresses.Add(mBuyerAddress.Landmark);
                        }
                        if (!Common.EmptyFiels(mBuyerAddress.Country))
                        {
                            addresses.Add(mBuyerAddress.Country);
                        }
                    }

                    if (addresses != null && addresses.Count > 0)
                    {
                        lblBuyerAddress.Text = string.Join(Environment.NewLine, addresses);
                    }
                }
                else
                {
                    //HideAddress
                    FrmBuyerAddress.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/BindSellerAddress: " + ex.Message);
            }
        }

        private void BindShippingAddress(ShippingAddressDetails mShippingAddress)
        {
            try
            {
                if (mShippingAddress != null && (mShippingAddress.Country > 0 ||
                      !Common.EmptyFiels(mShippingAddress.Building) || !Common.EmptyFiels(mShippingAddress.Street) ||
                      !Common.EmptyFiels(mShippingAddress.City) || !Common.EmptyFiels(mShippingAddress.PinCode) ||
                      !Common.EmptyFiels(mShippingAddress.Landmark) || !Common.EmptyFiels(mShippingAddress.State)))
                {
                    FrmShippingAddress.IsVisible = true;
                    List<string> addresses = new List<string>();

                    if (!Common.EmptyFiels(mShippingAddress.Building))
                    {
                        addresses.Add(mShippingAddress.Building);
                    }

                    if (!Common.EmptyFiels(mShippingAddress.Street) && !Common.EmptyFiels(mShippingAddress.City))
                    {
                        addresses.Add(mShippingAddress.Street + ", " + mShippingAddress.City);
                    }
                    else
                    {
                        if (!Common.EmptyFiels(mShippingAddress.Street))
                        {
                            addresses.Add(mShippingAddress.Street);
                        }
                        if (!Common.EmptyFiels(mShippingAddress.City))
                        {
                            addresses.Add(mShippingAddress.City);
                        }
                    }

                    if (!Common.EmptyFiels(mShippingAddress.State) && !Common.EmptyFiels(mShippingAddress.PinCode))
                    {
                        addresses.Add(mShippingAddress.State + " " + mShippingAddress.PinCode);
                    }
                    else
                    {
                        if (!Common.EmptyFiels(mShippingAddress.State))
                        {
                            addresses.Add(mShippingAddress.State);
                        }
                        if (!Common.EmptyFiels(mShippingAddress.PinCode))
                        {
                            addresses.Add(mShippingAddress.PinCode);
                        }
                    }

                    if (!Common.EmptyFiels(mShippingAddress.Landmark))
                    {
                        addresses.Add(mShippingAddress.Landmark);
                    }

                    if (mShippingAddress.Country.HasValue)
                    {
                        addresses.Add(mShippingAddress.Country.Value.ToString());
                    }

                    if (addresses != null && addresses.Count > 0)
                    {
                        lblShippingAddress.Text = string.Join(Environment.NewLine, addresses);
                    }
                }
                else
                {
                    //HideAddress
                    FrmShippingAddress.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/BindShippingAddress: " + ex.Message);
            }
        }

        private void BindOrderStatus(int Status, bool PickupProductDirectly, bool CancellationPeriod)
        {
            try
            {
                if (PickupProductDirectly && Status == (int)OrderStatus.ReadyForPickup)  //Pickup from seller
                {
                    BtnScanQRCode.IsVisible = true;
                    //BtnRaiseGrievance.IsVisible = true;
                    BtnUpdate.IsVisible = false;
                    GrdUpdateStatus.IsVisible = false;
                    StkNoUpdation.IsVisible = false;
                }

                if (CancellationPeriod)
                {
                    if (PickupProductDirectly && Status == (int)OrderStatus.ReadyForPickup)
                    {
                        BtnScanQRCode.IsVisible = true;
                        //BtnRaiseGrievance.IsVisible = true;
                        BtnUpdate.IsVisible = false;
                        GrdUpdateStatus.IsVisible = false;
                        StkNoUpdation.IsVisible = false;
                    }
                    else if (Status == (int)OrderStatus.Delivered)
                    {
                        GrdUpdateStatus.IsVisible = true;
                        txtShippingNumber.IsReadOnly = true;
                        txtLRNumber.IsReadOnly = true;
                        txtEWayBillNumber.IsReadOnly = true;
                        txtTrackingLink.IsReadOnly = true;
                        GrdChargesAndEarnings.IsVisible = true;

                        //BtnRaiseGrievance.IsVisible = false;
                        StkNoUpdation.IsVisible = false;
                        BtnUpdate.IsVisible = true;
                        BtnScanQRCode.IsVisible = false;
                    }
                    else if (Status == (int)OrderStatus.Completed)
                    {
                        StkNoUpdation.IsVisible = true;
                        GrdChargesAndEarnings.IsVisible = true;

                        //BtnRaiseGrievance.IsVisible = false;
                        BtnUpdate.IsVisible = false;
                        GrdUpdateStatus.IsVisible = false;
                        BtnScanQRCode.IsVisible = false;
                    }
                    else if (Status == (int)OrderStatus.CancelledFromBuyer)
                    {
                        BtnScanQRCode.IsVisible = false;
                        BtnUpdate.IsVisible = false;
                        GrdUpdateStatus.IsVisible = false;
                        StkNoUpdation.IsVisible = false;
                        //BtnRaiseGrievance.IsVisible = false;
                    }
                    else if (Status == (int)OrderStatus.Shipped)
                    {
                        //BtnRaiseGrievance.IsVisible = true;
                        BtnScanQRCode.IsVisible = false;
                        BtnUpdate.IsVisible = true;
                        GrdUpdateStatus.IsVisible = true;
                    }
                    else //Update Status
                    {
                        BtnScanQRCode.IsVisible = false;
                        //BtnRaiseGrievance.IsVisible = false;
                        BtnUpdate.IsVisible = true;
                        GrdUpdateStatus.IsVisible = true;
                    }
                }
                else
                {
                    BtnScanQRCode.IsVisible = false;
                    //BtnRaiseGrievance.IsVisible = false;
                    BtnUpdate.IsVisible = false;
                    GrdUpdateStatus.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/BindOrderStatus: " + ex.Message);
            }
        }

        private async Task GetOrderDetails()
        {
            try
            {
                isFirstLoad = true;

                mOrder = await DependencyService.Get<IOrderRepository>().GetOrderDetails(OrderId);
                if (mOrder != null)
                {
                    #region [ Details ]
                    if (mOrder.PickupProductDirectly)
                    {
                        lblPinCodeTitle.Text = Constraints.Str_PickupPINCode;
                        lblExpected.Text = Constraints.Str_ExpectedPickupDate;
                    }
                    else
                    {
                        lblPinCodeTitle.Text = Constraints.Str_ShippingPINCode;
                        lblExpected.Text = Constraints.Str_ExpectedDeliveryDate;
                    }

                    lblOrderId.Text = mOrder.OrderNo;
                    lblOrderRequirementId.Text = mOrder.RequirementNo;
                    lblRequirementTitle.Text = mOrder.Title;
                    lblOrderReferenceNo.Text = mOrder.QuoteNo;
                    lblOrderSellerName.Text = mOrder.SellerContact.Name;

                    lblOrderQuntity.Text = "" + mOrder.RequestedQuantity + " " + mOrder.Unit;
                    lblOrderUnitPrice.Text = "Rs " + mOrder.UnitPrice;
                    lblOrderNetAmount.Text = "Rs " + mOrder.NetAmount;
                    lblOrderHandlingCharge.Text = "Rs " + mOrder.HandlingCharges;
                    lblOrderShippingCharge.Text = "Rs " + mOrder.ShippingCharges;
                    lblOrderInsuranceCharge.Text = "Rs " + mOrder.InsuranceCharges;
                    lblOrderCountry.Text = mOrder.Country;
                    lblInvoiceNo.Text = mOrder.OrderNo;
                    lblTotalAmount.Text = "Rs " + mOrder.TotalAmount;

                    if (mOrder.ExpectedDelivery == null || mOrder.ExpectedDelivery.Date == DateTime.MinValue.Date)
                    {
                        lblExpectedDate.IsVisible = false;
                        lblExpected.IsVisible = false;
                    }
                    else
                    {
                        lblExpectedDate.Text = mOrder.ExpectedDelivery.ToString(Constraints.Str_DateFormate);
                        lblExpectedDate.IsVisible = true;
                        lblExpected.IsVisible = true;
                    }

                    lblBuyerContact.Text = mOrder.BuyerContact.PhoneNumber;

                    lblOrderStatus.Text = mOrder.OrderStatusDescr;
                    lblPaymentStatus.Text = mOrder.PaymentStatusDescr;
                    lblPlatformCharges.Text = "Rs " + mOrder.PlatFormCharges.ToString();
                    lblSellerEarnings.Text = "Rs " + mOrder.SellerEarnings.ToString();
                    lblIsReseller.Text = mOrder.IsReseller ? Constraints.Str_Right : Constraints.Str_Wrong;
                    if (!Common.EmptyFiels(mOrder.Gstin))
                    {
                        StkGSTNumber.IsVisible = true;
                        lblGstNumber.Text = mOrder.Gstin;
                    }
                    else
                    {
                        StkGSTNumber.IsVisible = false;
                    }

                    OrderStatusList(mOrder.PickupProductDirectly, mOrder.OrderStatus);

                    //if (mOrder.PickupProductDirectly)
                    //    pckOrderStatus.SelectedIndex = Common.GetOrderIndex(mOrder.OrderStatus);
                    //else
                    //    pckOrderStatus.SelectedIndex = Common.GetOrderIndexWithoutRP(mOrder.OrderStatus);

                    if (mOrder.BuyerContact != null && !Common.EmptyFiels(mOrder.BuyerContact.BuyerId) && !Common.EmptyFiels(mOrder.BuyerContact.UserId))
                    {
                        StkBuyerName.IsVisible = true;
                        StkBuyerPhoneNo.IsVisible = true;
                        StkBuyerEmail.IsVisible = true;
                        lblBuyerContactLabel.IsVisible = true;
                        lblBuyerContact.IsVisible = true;
                        lblOrderBuyerName.Text = mOrder.BuyerContact.Name;
                        lblBuyerPNumber.Text = mOrder.BuyerContact.PhoneNumber;
                        lblBuyerEmail.Text = mOrder.BuyerContact.Email;
                        lblBuyerContact.Text = mOrder.BuyerContact.PhoneNumber;
                    }
                    else
                    {
                        StkBuyerName.IsVisible = false;
                        StkBuyerPhoneNo.IsVisible = false;
                        StkBuyerEmail.IsVisible = false;
                        lblBuyerContactLabel.IsVisible = false;
                        lblBuyerContact.IsVisible = false;
                        lblOrderBuyerName.Text = string.Empty;
                        lblBuyerPNumber.Text = string.Empty;
                        lblBuyerEmail.Text = string.Empty;
                        lblBuyerContact.Text = string.Empty;
                    }

                    if (!Common.EmptyFiels(mOrder.ShippingPincode))
                    {
                        StkShippingPINCode.IsVisible = true;
                        lblOrderPinCode.Text = mOrder.ShippingPincode;
                    }
                    else
                    {
                        StkShippingPINCode.IsVisible = false;
                        lblOrderPinCode.Text = string.Empty;
                    }
                    #endregion

                    #region [ Address ]
                    BindSellerAddress(mOrder.BuyerAddressDetails);
                    BindShippingAddress(mOrder.ShippingAddressDetails);
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

                    #region [ Status ]
                    BindOrderStatus(mOrder.OrderStatus, mOrder.PickupProductDirectly, mOrder.IsCancellationPeriodOver);
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/GetOrderDetails: " + ex.Message);
            }
            finally
            {
                isFirstLoad = false;
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
                    BoxOrderStatus.BackgroundColor = (Color)App.Current.Resources["appColor3"];
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
                        if (txtTrackingLink.Text.IsValidURL())
                        {
                            mOrderUpdate.TrackingLink = txtTrackingLink.Text;
                        }
                        else
                        {
                            Common.DisplayErrorMessage(Constraints.Invalid_Tracking_link);
                            BtnUpdate.IsEnabled = true;
                            return;
                        }
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
                    //txtShippingNumber.Text = string.Empty;
                    //txtLRNumber.Text = string.Empty;
                    //txtEWayBillNumber.Text = string.Empty;
                    //txtTrackingLink.Text = string.Empty;
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
        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgMenu);
                await Navigation.PushAsync(new OtherPage.SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/ImgMenu_Tapped: " + ex.Message);
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
                Common.DisplayErrorMessage("OrderDetailsPage/ImgNotification_Tapped: " + ex.Message);
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
            try
            {
                await Common.BindAnimation(button: BtnScanQRCode);

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
                Common.DisplayErrorMessage("OrderDetailsPage/BtnScanQRCode_Clicked: " + ex.Message);
            }
        }

        private async void BtnUpdate_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: BtnUpdate);
                await UpdateOrder();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/BtnUpdate_Tapped: " + ex.Message);
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
                    BoxOrderStatus.BackgroundColor = (Color)App.Current.Resources["appColor8"];
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
            try
            {
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

                int idx = -1;

                if (mOrder.PickupProductDirectly)
                    idx = Common.GetOrderIndex(mOrder.OrderStatus);
                else
                    idx = Common.GetOrderIndexWithoutRP(mOrder.OrderStatus);

                //if (pckOrderStatus.SelectedIndex != idx)
                if (pckOrderStatus.SelectedIndex != 0)
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

        //private async void BtnRaiseGrievance_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        await Common.BindAnimation(button: BtnRaiseGrievance);
        //        await Navigation.PushAsync(new RaiseGrievancePage(OrderId));
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.DisplayErrorMessage("OrderDetailsPage/BtnRaiseGrievance_Tapped: " + ex.Message);
        //    }
        //}
        #endregion

        private void txtShippingNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isFirstLoad)
                return;

            if (e.OldTextValue != txtShippingNumber.Text)
                BtnUpdate.IsEnabled = true;
        }

        private void txtLRNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isFirstLoad)
                return;

            if (e.OldTextValue != txtLRNumber.Text)
                BtnUpdate.IsEnabled = true;
        }

        private void txtEWayBillNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isFirstLoad)
                return;

            if (e.OldTextValue != txtEWayBillNumber.Text)
                BtnUpdate.IsEnabled = true;
        }

        private void txtTrackingLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isFirstLoad)
                return;

            if (e.OldTextValue != txtTrackingLink.Text)
                BtnUpdate.IsEnabled = true;
        }
    }
}