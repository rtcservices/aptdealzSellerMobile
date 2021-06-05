using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Popup;
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
        #region Objects
        private List<ReportDetail> mReportDetails = new List<ReportDetail>();
        #endregion

        #region Constructor
        public ReportDetailPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindShippingData();
        }

        private void BindShippingData()
        {
            lstReportDetails.ItemsSource = null;

            mReportDetails = new List<ReportDetail>()
            {
                new ReportDetail
                {
                    InvoiceId="INV#123",
                    Status="Cleared",
                    Amount=3500,
                    EarningAmount=3000,
                    InvoiceDate="12-01-2021",

                },
                new ReportDetail
                {
                    InvoiceId="INV#123",
                    Status="Pending",
                    Amount=3500,
                    EarningAmount=3000,
                    InvoiceDate="12-01-2021",

                },
                new ReportDetail
                {
                    InvoiceId="INV#123",
                    Status="Cleared",
                    Amount=3500,
                    EarningAmount=3000,
                    InvoiceDate="12-01-2021",

                },
                new ReportDetail
                {
                    InvoiceId="INV#123",
                    Status="Cleared",
                    Amount=3500,
                    EarningAmount=3000,
                    InvoiceDate="12-01-2021",

                },
                  new ReportDetail
                {
                    InvoiceId="INV#123",
                    Status="Pending",
                    Amount=3500,
                    EarningAmount=3000,
                    InvoiceDate="12-01-2021",

                },
            };

            lstReportDetails.ItemsSource = mReportDetails.ToList();
        }
        #endregion

        #region Events
        private void ImgExpand_Tapped(object sender, EventArgs e)
        {
            var imgExp = (Image)sender;
            var viewCell = (ViewCell)imgExp.Parent.Parent.Parent;
            if (viewCell != null)
            {
                viewCell.ForceUpdateSize();
            }
            var shippingModel = imgExp.BindingContext as ReportDetail;
            if (shippingModel != null && shippingModel.ArrowImage == Constraints.Right_Arrow)
            {
                shippingModel.ArrowImage = Constraints.Down_Arrow;
                shippingModel.Layout = LayoutOptions.StartAndExpand;
                shippingModel.ShowDelete = false;
                shippingModel.ShowCategory = true;
            }
            else
            {
                shippingModel.ArrowImage = Constraints.Right_Arrow;
                shippingModel.Layout = LayoutOptions.CenterAndExpand;
                shippingModel.ShowDelete = true;
                shippingModel.ShowCategory = false;
            }
        }

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
                StatusPopup statusPopup = new StatusPopup("isRequest");
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
        #endregion
    }
}