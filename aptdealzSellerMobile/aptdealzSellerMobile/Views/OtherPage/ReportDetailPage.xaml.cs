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

        void BindList()
        {
            try
            {
                if (mReportDetails != null && mReportDetails.Count > 0)
                {
                    lstReportDetails.IsVisible = true;
                    FrmSortBy.IsVisible = true;
                   // FrmStatusBy.IsVisible = true;
                    FrmSearchBy.IsVisible = true;
                    FrmFilterBy.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstReportDetails.ItemsSource = mReportDetails.ToList();
                }
                else
                {
                    lstReportDetails.IsVisible = false;
                    //FrmStatusBy.IsVisible = false;
                    FrmSearchBy.IsVisible = false;
                    FrmSortBy.IsVisible = false;
                    FrmFilterBy.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Report/BindList: " + ex.Message);
            }
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
            if (shippingModel != null && shippingModel.ArrowImage == Constraints.Arrow_Right)
            {
                shippingModel.ArrowImage = Constraints.Arrow_Down;
                shippingModel.Layout = LayoutOptions.StartAndExpand;
                shippingModel.ShowDelete = false;
                shippingModel.ShowCategory = true;
            }
            else
            {
                shippingModel.ArrowImage = Constraints.Arrow_Right;
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

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
           
        }

        //private async void FrmStatusBy_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        StatusPopup statusPopup = new StatusPopup("isRequest");
        //        statusPopup.isRefresh += (s1, e1) =>
        //        {
        //            string result = s1.ToString();
        //            if (!Common.EmptyFiels(result))
        //            {
        //                //Bind list as per result
        //            }
        //        };
        //        await PopupNavigation.Instance.PushAsync(statusPopup);
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}

        private async void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                SortByPopup sortByPopup = new SortByPopup("", "");
                sortByPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
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

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            try
            {
                entrSearch.Text = string.Empty;
                BindList();
            }
            catch (Exception ex)
            {

            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    var ReportSearch = mReportDetails.Where(x =>
                                                        x.InvoiceId.ToLower().Contains(entrSearch.Text.ToLower())).ToList();
                    if (ReportSearch != null && ReportSearch.Count > 0)
                    {
                        lstReportDetails.IsVisible = true;
                        FrmSortBy.IsVisible = true;
                        //FrmStatusBy.IsVisible = true;
                        FrmFilterBy.IsVisible = true;
                        lblNoRecord.IsVisible = false;
                        lstReportDetails.ItemsSource = ReportSearch.ToList();
                    }
                    else
                    {
                        lstReportDetails.IsVisible = false;
                        //FrmStatusBy.IsVisible = false;
                        FrmSortBy.IsVisible = false;
                        FrmFilterBy.IsVisible = false;
                        lblNoRecord.IsVisible = true;
                    }
                }
                else
                {
                    BindList();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Report/entrSearch_TextChanged: " + ex.Message);
            }
        }

        #endregion
    }
}