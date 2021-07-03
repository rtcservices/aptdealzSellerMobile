﻿using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Utility;
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
    public partial class QuoteView : ContentView
    {
        #region Objects
        private string filterBy = Utility.RequirementSortBy.ID.ToString();
        private bool sortBy = true;
        private readonly int pageSize = 10;
        private int pageNo;
        private List<QuoteDetail> mQuoteDetails = new List<QuoteDetail>();
        #endregion

        #region Constructor
        public QuoteView()
        {
            InitializeComponent();
            BindRequirements();
        }
        #endregion

        #region Method
        private void BindRequirements()
        {
            lstQuotesDetails.ItemsSource = null;
            mQuoteDetails = new List<QuoteDetail>()
            {
                new QuoteDetail
                {
                   QuoteId="QUO#123",
                   ReqId ="REQ#110",
                   Description="Need 5 Canon A210 All In one Printer.",
                   QuoteCode="B852",
                   QuoteDate="12-01-2021",
                   QuoteAmount=1000
                },
                new QuoteDetail
                {

                   QuoteId="QUO#123",
                   ReqId ="REQ#110",
                   Description="Need 5 Canon A210 All In one Printer.",
                   QuoteDate="12-01-2021",
                   QuoteCode="B852",
                   QuoteAmount=1800
                },
                new QuoteDetail
                {

                   QuoteId="QUO#123",
                   ReqId ="REQ#110",
                   Description="Need 5 Canon A210 All In one Printer.",
                   QuoteDate="12-01-2021",
                   QuoteCode="B852",
                   QuoteAmount=5000
                },
                new QuoteDetail
                {
                   QuoteId="QUO#123",
                   ReqId ="REQ#110",
                   Description="Need 5 Canon A210 All In one Printer.",
                   QuoteDate="12-01-2021",
                   QuoteCode="B852",
                   QuoteAmount=4500
                },

            };

            lstQuotesDetails.ItemsSource = mQuoteDetails.ToList();
        }

        void BindList()
        {
            try
            {
                if (mQuoteDetails != null && mQuoteDetails.Count > 0)
                {
                    lstQuotesDetails.IsVisible = true;
                    FrmSortBy.IsVisible = true;
                    FrmSearchBy.IsVisible = true;
                    FrmFilterBy.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstQuotesDetails.ItemsSource = mQuoteDetails.ToList();
                }
                else
                {
                    lstQuotesDetails.IsVisible = false;
                    FrmSearchBy.IsVisible = false;
                    FrmSortBy.IsVisible = false;
                    FrmFilterBy.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/BindList: " + ex.Message);
            }
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            App.Current.MainPage = new MasterData.MasterDataPage();
        }

        //private async void ImgSearch_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SearchPopup searchPopup = new SearchPopup();
        //        searchPopup.isRefresh += (s1, e1) =>
        //        {
        //            lstQuotesDetails.ItemsSource = mQuoteDetails.ToList();
        //        };
        //        await PopupNavigation.Instance.PushAsync(searchPopup);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private async void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (ImgSort.Source.ToString().Replace("File: ", "") == Constraints.Sort_ASC)
                {
                    ImgSort.Source = Constraints.Sort_DSC;
                    sortBy = false;
                }
                else
                {
                    ImgSort.Source = Constraints.Sort_ASC;
                    sortBy = true;
                }

                pageNo = 1;
                //GetActiveRequirements(filterBy, sortBy);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void BtnQuotes_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectGrid = (ImageButton)sender;
                var setHight = (ViewCell)selectGrid.Parent.Parent.Parent;
                if (setHight != null)
                {
                    setHight.ForceUpdateSize();
                }

                var response = (QuoteDetail)selectGrid.BindingContext;
                if (response != null)
                {
                    foreach (var selectedImage in mQuoteDetails)
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

            }
        }

        private void lstQuotesDetails_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstQuotesDetails.SelectedItem = null;
        }

        private void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var sortby = new SortByPopup(filterBy, "Active");
                sortby.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!string.IsNullOrEmpty(result))
                    {
                        pageNo = 1;
                        filterBy = result;
                        //GetActiveRequirements(filterBy, sortBy);
                    }
                };
                PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/CustomEntry_Unfocused: " + ex.Message);
            }
        }
      
        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    var QueSearch = mQuoteDetails.Where(x =>
                                                        x.QuoteId.ToLower().Contains(entrSearch.Text.ToLower())).ToList();
                    if (QueSearch != null && QueSearch.Count > 0)
                    {
                        lstQuotesDetails.IsVisible = true;
                        FrmSortBy.IsVisible = true;
                        FrmFilterBy.IsVisible = true;
                        lblNoRecord.IsVisible = false;
                        lstQuotesDetails.ItemsSource = QueSearch.ToList();
                    }
                    else
                    {
                        lstQuotesDetails.IsVisible = false;
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
                Common.DisplayErrorMessage("QuoteView/entrSearch_TextChanged: " + ex.Message);
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

        #endregion

    }
}