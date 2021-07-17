using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Popup;
using Newtonsoft.Json.Linq;
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
        private List<Quote> mQuote;
        private string filterBy = "";
        private string title = string.Empty;
        private int? statusBy = null;
        private bool? sortBy = null;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Constructor
        public QuoteView()
        {
            InitializeComponent();
            mQuote = new List<Quote>();
            pageNo = 1;
            GetSubmittedQuotes(statusBy, title, filterBy, sortBy, true);
        }
        #endregion

        #region Method        
        public async void GetSubmittedQuotes(int? StatusBy = null, string Title = "", string FilterBy = "", bool? SortBy = null, bool isLoader = false)
        {
            try
            {
                QuoteAPI quoteAPI = new QuoteAPI();

                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }

                var mResponse = await quoteAPI.GetSubmittedQuotesByMe(StatusBy, Title, FilterBy, SortBy, pageNo, pageSize);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    var mQuotes = result.ToObject<List<Quote>>();
                    if (pageNo == 1)
                    {
                        mQuote.Clear();
                    }

                    foreach (var quote in mQuotes)
                    {
                        if (mQuote.Where(x => x.QuoteId == quote.QuoteId).Count() == 0)
                            mQuote.Add(quote);
                    }
                    BindList(mQuote);
                }
                else
                {
                    lstQuotesDetails.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    if (mResponse.Message != null)
                    {
                        lblNoRecord.Text = mResponse.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/GetRequirements: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void BindList(List<Quote> mQuotes)
        {
            try
            {
                if (mQuotes != null && mQuotes.Count > 0)
                {
                    lstQuotesDetails.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstQuotesDetails.ItemsSource = mQuotes.ToList();
                }
                else
                {
                    lstQuotesDetails.ItemsSource = null;
                    lstQuotesDetails.IsVisible = false;
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

        private void FrmSortBy_Tapped(object sender, EventArgs e)
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
                GetSubmittedQuotes(statusBy, title, filterBy, sortBy, true);
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

                var response = (Quote)selectGrid.BindingContext;
                if (response != null)
                {
                    foreach (var selectedImage in mQuote)
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
                var sortby = new FilterPopup(filterBy, "Quote");
                sortby.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        filterBy = result;
                        lblFilterBy.Text = filterBy;

                        pageNo = 1;
                        GetSubmittedQuotes(statusBy, title, filterBy, sortBy, true);
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
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetSubmittedQuotes(statusBy, entrSearch.Text, filterBy, sortBy, false);
                }
                else
                {
                    GetSubmittedQuotes(statusBy, title, filterBy, sortBy, true);
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
                BindList(mQuote);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/BtnClose_Clicked: " + ex.Message);
            }
        }

        private async void FrmStatusBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                StatusPopup statusPopup = new StatusPopup(statusBy);
                statusPopup.isRefresh += (s1, e1) =>
                {
                    int result = (int)s1;
                    //if (result != null)
                    //{
                    pageNo = 1;
                    statusBy = result;
                    GetSubmittedQuotes(statusBy, title, filterBy, sortBy, true);
                    //}
                };
                await PopupNavigation.Instance.PushAsync(statusPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/FrmStatusBy_Tapped: " + ex.Message);
            }
        }

        private void lstQuotesDetails_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstQuotesDetails.IsRefreshing = true;
                pageNo = 1;
                mQuote.Clear();
                GetSubmittedQuotes(statusBy, title, filterBy, sortBy, true);
                lstQuotesDetails.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/lstRequirements_Refreshing: " + ex.Message);
            }
        }

        private void lstQuotesDetails_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                if (this.mQuote.Count < 10)
                    return;
                if (this.mQuote.Count == 0)
                    return;

                var lastrequirement = this.mQuote[this.mQuote.Count - 1];
                var lastAppearing = (Quote)e.Item;
                if (lastAppearing != null)
                {
                    if (lastrequirement == lastAppearing)
                    {
                        var totalAspectedRow = pageSize * pageNo;
                        pageNo += 1;

                        if (this.mQuote.Count() >= totalAspectedRow)
                        {
                            GetSubmittedQuotes(statusBy, title, filterBy, sortBy, true);
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void GrdList_Tapped(object sender, EventArgs e)
        {
            var GridExp = (Grid)sender;
            var mQuote = GridExp.BindingContext as Quote;
            Navigation.PushAsync(new Dashboard.QuoteDetailsPage(mQuote.RequirementId, mQuote.QuoteId));
        }
        #endregion
    }
}