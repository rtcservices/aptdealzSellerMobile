﻿using aptdealzSellerMobile.Utility;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Model.Reponse
{
    public class Order : INotifyPropertyChanged
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("requirementId")]
        public string RequirementId { get; set; }

        [JsonProperty("quoteId")]
        public string QuoteId { get; set; }

        [JsonProperty("orderNo")]
        public string OrderNo { get; set; }

        [JsonProperty("quoteNo")]
        public string QuoteNo { get; set; }

        [JsonProperty("requirementNo")]
        public string RequirementNo { get; set; }

        [JsonProperty("buyer")]
        public string Buyer { get; set; }

        [JsonProperty("seller")]
        public string Seller { get; set; }

        [JsonProperty("shippingPincode")]
        public string ShippingPincode { get; set; }

        [JsonProperty("requestedQuantity")]
        public int RequestedQuantity { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("netAmount")]
        public decimal NetAmount { get; set; }

        [JsonProperty("handlingCharges")]
        public decimal HandlingCharges { get; set; }

        [JsonProperty("shippingCharges")]
        public decimal ShippingCharges { get; set; }

        [JsonProperty("insuranceCharges")]
        public decimal InsuranceCharges { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonProperty("expectedDelivery")]
        public DateTime ExpectedDelivery { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("paymentStatus")]
        public int PaymentStatus { get; set; }

        [JsonProperty("paidAmount")]
        public decimal PaidAmount { get; set; }

        [JsonProperty("trackingLink")]
        public string TrackingLink { get; set; }

        [JsonProperty("pickupProductDirectly")]
        public bool PickupProductDirectly { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("shipperNumber")]
        public string ShipperNumber { get; set; }

        [JsonProperty("lrNumber")]
        public string LrNumber { get; set; }

        [JsonProperty("billNumber")]
        public string BillNumber { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("productDescription")]
        public string ProductDescription { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("quoteComments")]
        public object QuoteComments { get; set; }

        [JsonProperty("orderStatus")]
        public int OrderStatus { get; set; }

        [JsonProperty("orderStatusDescr")]
        public string OrderStatusDescr { get; set; }

        [JsonProperty("paymentStatusDescr")]
        public string PaymentStatusDescr { get; set; }

        [JsonProperty("isOrderCancelAllowed")]
        public bool IsOrderCancelAllowed { get; set; }

        [JsonProperty("sellerContact")]
        public SellerContact SellerContact { get; set; }

        [JsonProperty("buyerContact")]
        public BuyerContact BuyerContact { get; set; }

        [JsonProperty("buyerAddressDetails")]
        public BuyerAddressDetails BuyerAddressDetails { get; set; }

        [JsonProperty("sellerAddressDetails")]
        public SellerAddressDetails SellerAddressDetails { get; set; }

        [JsonProperty("platFormCharges")]
        public int PlatFormCharges { get; set; }

        [JsonProperty("sellerEarnings")]
        public int SellerEarnings { get; set; }

        [JsonProperty("sellerRating")]
        public int SellerRating { get; set; }

        [JsonProperty("productRating")]
        public int ProductRating { get; set; }

        [JsonProperty("shippingAddressDetails")]
        public ShippingAddressDetails ShippingAddressDetails { get; set; }

        [JsonProperty("isCancellationPeriodOver")]
        public bool IsCancellationPeriodOver { get; set; }
        [JsonProperty("createdDate")]
        public string CreatedDate { get; set; }


        #region [ Extra Properties ]
        [JsonIgnore]
        public bool isSelectGrievance { get; set; } = false;

        [JsonIgnore]
        public bool ScanQRCode { get; set; }

        //[JsonIgnore]
        //public string ExpectedDeliveryDate
        //{
        //    get
        //    {
        //        if (ExpectedDelivery != null && ExpectedDelivery != DateTime.MinValue)
        //        {
        //            return ExpectedDelivery.Date.ToString(Constraints.Str_DateFormate);
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //}
        [JsonIgnore]
        public string OrderPaymentStatus
        {
            get
            {
                if (PaymentStatus == (int)Utility.PaymentStatus.Pending)
                {
                    return Utility.OrderStatus.Pending.ToString();
                }
                else if (PaymentStatus == (int)Utility.PaymentStatus.Success)
                {
                    return Utility.PaymentStatus.Success.ToString();
                }
                else if (PaymentStatus == (int)Utility.PaymentStatus.Failed)
                {
                    return Utility.PaymentStatus.Failed.ToString();
                }
                else
                {
                    return PaymentStatus.ToString();
                }
            }
        }

        [JsonIgnore]
        private string _ArrowImage { get; set; } = Constraints.Arrow_Right;

        [JsonIgnore]
        public string ArrowImage
        {
            get { return _ArrowImage; }
            set { _ArrowImage = value; PropertyChangedEventArgs("ArrowImage"); }
        }

        [JsonIgnore]
        private bool _MoreDetail { get; set; } = false;
        [JsonIgnore]
        public bool MoreDetail
        {
            get { return _MoreDetail; }
            set { _MoreDetail = value; PropertyChangedEventArgs("MoreDetail"); }
        }

        [JsonIgnore]
        private bool _OldDetail { get; set; } = true;
        [JsonIgnore]
        public bool OldDetail
        {
            get { return _OldDetail; }
            set { _OldDetail = value; PropertyChangedEventArgs("OldDetail"); }
        }

        [JsonIgnore]
        private Color _GridBg { get; set; } = Color.Transparent;
        [JsonIgnore]
        public Color GridBg
        {
            get { return _GridBg; }
            set { _GridBg = value; PropertyChangedEventArgs("GridBg"); }
        }

        [JsonIgnore]
        public Color StatusColor
        {
            get
            {
                if (Status == (int)Utility.OrderStatus.Completed)
                {
                    return (Color)App.Current.Resources["appColor1"];
                }
                else if (Status == (int)Utility.OrderStatus.Pending)
                {
                    return (Color)App.Current.Resources["appColor2"];
                }
                else
                {
                    return (Application.Current.UserAppTheme == OSAppTheme.Light) ? (Color)App.Current.Resources["appColor4"] : (Color)App.Current.Resources["appColor6"];
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void PropertyChangedEventArgs(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
