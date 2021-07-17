using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        private Order mOrder;
        private string OrderId;
        private bool isOrderCancelled;

        #region Constructor
        public OrderDetailsPage(string orderId, bool isCancelled = false)
        {
            InitializeComponent();
            mOrder = new Order();
            OrderId = orderId;
            isOrderCancelled = isCancelled;
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetOrderDetails();
        }

        public async void GetOrderDetails()
        {
            try
            {
                if (!isOrderCancelled)
                {
                    lblCancelledOrder.IsVisible = false;
                    AllDetails.IsVisible = true;
                    mOrder = await DependencyService.Get<IOrderRepository>().GetOrderDetails(OrderId);
                    if (mOrder != null)
                    {
                        if (mOrder.PickupProductDirectly)
                            lblPinCodeTitle.Text = "Product Pickup PIN Code";
                        else
                            lblPinCodeTitle.Text = "Shipping PIN Code";

                        lblOrderRequirementId.Text = mOrder.RequirementNo;
                        lblOrderReferenceNo.Text = mOrder.QuoteNo;
                        lblOrderBuyerName.Text = mOrder.Buyer;
                        lblOrderSellerName.Text = mOrder.Seller;
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
                        lblPaymentStatus.Text = mOrder.OrderPaymentStatus;

                        //if (mOrder.Status != 1)
                        //{
                        //    //lblOrderStatus.Text = "Delivered";
                        //    StkAccept.IsVisible = false;
                        //    BtnUpdate.IsVisible = false;
                        //    StkDelivered.IsVisible = true;
                        //}
                        //else
                        //{
                        //    //lblOrderStatus.Text = "Accepted";
                        //    StkAccept.IsVisible = true;
                        //    BtnUpdate.IsVisible = true;
                        //    StkDelivered.IsVisible = false;
                        //}

                        if (mOrder.PickupProductDirectly)
                        {
                            BtnScanQRCode.IsVisible = true;
                            BtnUpdate.IsVisible = false;
                            StkAccept.IsVisible = false;
                        }
                        else
                        {
                            if (mOrder.Status != 1)
                            {
                                StkAccept.IsVisible = false;
                                BtnUpdate.IsVisible = false;
                                StkDelivered.IsVisible = true;
                                BtnScanQRCode.IsVisible = false;
                            }
                            else
                            {
                                BtnScanQRCode.IsVisible = false;
                                BtnUpdate.IsVisible = true;
                                StkAccept.IsVisible = true;
                            }
                        }
                    }
                }
                else
                {
                    AllDetails.IsVisible = false;
                    lblCancelledOrder.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/GetOrderDetails: " + ex.Message);
            }
        }

        public bool UpdateOrderFieldsVelidations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtShippingNumber.Text) || Common.EmptyFiels(txtLRNumber.Text)
                    || Common.EmptyFiels(txtEWayBillNumber.Text) || Common.EmptyFiels(txtTrackingLink.Text)
                    || pckOrderStatus.SelectedIndex == -1)
                {
                    RequiredFields();
                    isValid = false;
                }

                if (pckOrderStatus.SelectedIndex == -1)
                {
                    Common.DisplayErrorMessage(Constraints.Required_OrderStatus);
                }
                else if (Common.EmptyFiels(txtShippingNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_ShippingNumber);
                }
                else if (Common.EmptyFiels(txtLRNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_LRNumber);
                }
                else if (Common.EmptyFiels(txtEWayBillNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_EWayBillNumber);
                }
                else if (Common.EmptyFiels(txtTrackingLink.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_TrackingLink);
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/UpdateOrderFieldsVelidations: " + ex.Message);
            }
            return isValid;
        }

        void RequiredFields()
        {
            try
            {
                if (pckOrderStatus.SelectedIndex == -1)
                {
                    BoxOrderStatus.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
                if (Common.EmptyFiels(txtShippingNumber.Text))
                {
                    BoxShippingNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
                if (Common.EmptyFiels(txtLRNumber.Text))
                {
                    BoxLRNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
                if (Common.EmptyFiels(txtEWayBillNumber.Text))
                {
                    BoxEWayBillNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
                if (Common.EmptyFiels(txtTrackingLink.Text))
                {
                    BoxTrackingLink.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/RequiredFields: " + ex.Message);
            }
        }

        async void UpdateOrder()
        {
            try
            {
                if (UpdateOrderFieldsVelidations())
                {
                    mOrder.Status = pckOrderStatus.SelectedIndex;
                    mOrder.ShipperNumber = txtShippingNumber.Text;
                    mOrder.LrNumber = txtLRNumber.Text;
                    mOrder.BillNumber = txtEWayBillNumber.Text;
                    mOrder.TrackingLink = txtTrackingLink.Text;

                    await DependencyService.Get<IOrderRepository>().UpdateOrder(mOrder);
                    GetOrderDetails();
                    //await Navigation.PushAsync(new Views.MainTabbedPages.MainTabbedPage("Supplying"));
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

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
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
        }

        private void BtnUpdate_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnUpdate);
            UpdateOrder();
        }
        #endregion
    }
}