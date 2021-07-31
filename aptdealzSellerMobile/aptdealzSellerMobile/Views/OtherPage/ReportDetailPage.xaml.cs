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
        private string filterBy = "";
        private string title = string.Empty;
        private bool? sortBy = null;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Constructor
        public ReportDetailPage()
        {
            InitializeComponent();

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
            BindShippingData();
        }

        private void BindShippingData()
        {
            try
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/BindShippingData: " + ex.Message);
            }
        }

        private void BindList(List<ReportDetail> mReportDetailList)
        {
            try
            {
                if (mReportDetailList != null && mReportDetailList.Count > 0)
                {
                    lstReportDetails.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstReportDetails.ItemsSource = mReportDetailList.ToList();
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

        #region Events
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

                var response = (ReportDetail)selectGrid.BindingContext;
                if (response != null)
                {
                    foreach (var selectedImage in mReportDetails)
                    {
                        if (selectedImage.ArrowImage == Constraints.Arrow_Right)
                        {
                            selectedImage.ArrowImage = Constraints.Arrow_Right;
                            selectedImage.GridBg = Color.Transparent;
                            selectedImage.MoreDetail = false;
                            selectedImage.OldDetail = true;
                        }
                        else
                        {
                            selectedImage.ArrowImage = Constraints.Arrow_Down;
                            selectedImage.GridBg = (Color)App.Current.Resources["LightGray"];
                            selectedImage.MoreDetail = true;
                            selectedImage.OldDetail = false;
                        }
                    }
                    if (response.ArrowImage == Constraints.Arrow_Right)
                    {
                        response.ArrowImage = Constraints.Arrow_Down;
                        response.GridBg = (Color)App.Current.Resources["LightGray"];
                        response.MoreDetail = true;
                        response.OldDetail = false;
                    }
                    else
                    {
                        response.ArrowImage = Constraints.Arrow_Right;
                        response.GridBg = Color.Transparent;
                        response.MoreDetail = false;
                        response.OldDetail = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/ImgExpand_Tapped: " + ex.Message);
            }
        }

        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Dashboard.NotificationPage());
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                //if (ImgSort.Source.ToString().Replace("File: ", "") == Constraints.Sort_ASC)
                //{
                //    ImgSort.Source = Constraints.Sort_DSC;
                //    sortBy = false;
                //}
                //else
                //{
                //    ImgSort.Source = Constraints.Sort_ASC;
                //    sortBy = true;
                //}

                pageNo = 1;
                //GetOrders(statusBy, title, filterBy, sortBy);
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
                FilterPopup sortByPopup = new FilterPopup("", "");
                sortByPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        filterBy = result;
                        lblFilterBy.Text = filterBy;
                        pageNo = 1;
                        BindShippingData();
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortByPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/FrmFilterBy_Tapped: " + ex.Message);
            }
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            try
            {
                entrSearch.Text = string.Empty;
                BindList(mReportDetails);
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
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    var ReportSearch = mReportDetails.Where(x =>
                                                        x.InvoiceId.ToLower().Contains(entrSearch.Text.ToLower())).ToList();
                    BindList(ReportSearch);
                }
                else
                {
                    BindList(mReportDetails);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ReportDetailPage/entrSearch_TextChanged: " + ex.Message);
            }
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void lstReportDetails_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstReportDetails.SelectedItem = null;
        }
        #endregion
    }
}