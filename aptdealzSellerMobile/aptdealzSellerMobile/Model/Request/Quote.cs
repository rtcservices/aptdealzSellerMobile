using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Model.Request
{
    public class Quote : INotifyPropertyChanged
    {
        [JsonProperty("quoteId")]
        public string QuoteId { get; set; }

        [JsonProperty("quoteNo")]
        public string QuoteNo { get; set; }

        [JsonProperty("requirementId")]
        public string RequirementId { get; set; }

        [JsonProperty("requirementNo")]
        public string RequirementNo { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("handlingCharges")]
        public decimal HandlingCharges { get; set; }

        [JsonProperty("shippingCharges")]
        public decimal ShippingCharges { get; set; }

        [JsonProperty("insuranceCharges")]
        public decimal InsuranceCharges { get; set; }

        [JsonProperty("shippingPinCode")]
        public string ShippingPinCode { get; set; }

        [JsonProperty("validityDate")]
        public DateTime ValidityDate { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("paymentStatus")]
        public string PaymentStatus { get; set; }

        [JsonProperty("sellerId")]
        public string SellerId { get; set; }

        [JsonProperty("buyerId")]
        public string BuyerId { get; set; }

        [JsonProperty("requestedQuantity")]
        public decimal RequestedQuantity { get; set; }

        [JsonProperty("netAmount")]
        public decimal NetAmount { get; set; }

        [JsonProperty("totalQuoteAmount")]
        public decimal TotalQuoteAmount { get; set; }

        [JsonProperty("requirementTitle")]
        public string RequirementTitle { get; set; }

        [JsonProperty("category")]
        public object Category { get; set; }

        [JsonProperty("subCategories")]
        public List<object> SubCategories { get; set; }

        [JsonProperty("preferredSource")]
        public string PreferredSource { get; set; }

        [JsonProperty("totalPriceEstimation")]
        public decimal TotalPriceEstimation { get; set; }

        [JsonProperty("expectedDeliveryDate")]
        public DateTime ExpectedDeliveryDate { get; set; }

        [JsonProperty("productImage")]
        public string ProductImage { get; set; }

        [JsonProperty("needInsuranceCoverage")]
        public bool NeedInsuranceCoverage { get; set; }

        [JsonProperty("isBuyerContactRevealed")]
        public bool IsBuyerContactRevealed { get; set; }

        [JsonProperty("isSellerContactRevealed")]
        public bool IsSellerContactRevealed { get; set; }

        [JsonProperty("sellerContact")]
        public object SellerContact { get; set; }

        [JsonProperty("buyerContact")]
        public BuyerContact BuyerContact { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        #region Extra
        private Color _GridBg { get; set; } = Color.Transparent;
        public Color GridBg
        {
            get { return _GridBg; }
            set { _GridBg = value; PropertyChangedEventArgs("GridBg"); }
        }

        private bool _MoreDetail { get; set; } = false;
        public bool MoreDetail
        {
            get { return _MoreDetail; }
            set { _MoreDetail = value; PropertyChangedEventArgs("MoreDetail"); }
        }

        private bool _OldDetail { get; set; } = true;
        public bool OldDetail
        {
            get { return _OldDetail; }
            set { _OldDetail = value; PropertyChangedEventArgs("OldDetail"); }
        }

        private string _ArrowImage { get; set; } = Constraints.Arrow_Right;
        public string ArrowImage
        {
            get { return _ArrowImage; }
            set { _ArrowImage = value; PropertyChangedEventArgs("ArrowImage"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedEventArgs(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
