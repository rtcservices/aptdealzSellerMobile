﻿using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
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
    public partial class RequirementsView : ContentView
    {
        #region Objects      
        private List<Requirement> mRequirements;
        private string filterBy = Utility.RequirementSortBy.ID.ToString();
        private bool sortBy = true;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion       

        #region Constructor
        public RequirementsView()
        {
            InitializeComponent();
            mRequirements = new List<Requirement>();
            pageNo = 1;
            GetActiveRequirements(filterBy, sortBy);
        }
        #endregion

        #region Methods
        public async void GetActiveRequirements(string FilterBy, bool SortBy)
        {
            try
            {
                RequirementAPI requirementAPI = new RequirementAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await requirementAPI.GetAllActiveRequirements(FilterBy, SortBy, pageNo, pageSize);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    var requirements = result.ToObject<List<Requirement>>();
                    if (pageNo == 1)
                    {
                        mRequirements.Clear();
                    }

                    foreach (var mRequirement in requirements)
                    {
                        if (mRequirements.Where(x => x.RequirementId == mRequirement.RequirementId).Count() == 0)
                            mRequirements.Add(mRequirement);
                    }
                    BindList();
                }
                else
                {
                    lstRequirements.IsVisible = false;
                    FrmSortBy.IsVisible = false;
                    //FrmStatusBy.IsVisible = false;
                    FrmSearchBy.IsVisible = false;
                    FrmFilterBy.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    if (mResponse.Message != null)
                    {
                        lblNoRecord.Text = mResponse.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/GetRequirements: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void BindList()
        {
            try
            {
                if (mRequirements != null && mRequirements.Count > 0)
                {
                    lstRequirements.IsVisible = true;
                    FrmSortBy.IsVisible = true;
                    // FrmStatusBy.IsVisible = true;
                    FrmSearchBy.IsVisible = true;
                    FrmFilterBy.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstRequirements.ItemsSource = mRequirements.ToList();
                }
                else
                {
                    lstRequirements.IsVisible = false;
                    // FrmStatusBy.IsVisible = false;
                    FrmSearchBy.IsVisible = false;
                    FrmSortBy.IsVisible = false;
                    FrmFilterBy.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/BindList: " + ex.Message);
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
            Common.BindAnimation(image: ImgBack);
            //App.Current.MainPage = new MasterData.MasterDataPage();
            Navigation.PopAsync();
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
                GetActiveRequirements(filterBy, sortBy);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void BtnRequerments_Tapped(object sender, EventArgs e)
        {
            try
            {
                var imgExp = (ImageButton)sender;
                var viewCell = (ViewCell)imgExp.Parent.Parent.Parent;
                if (viewCell != null)
                {
                    viewCell.ForceUpdateSize();
                }

                var mRequirement = imgExp.BindingContext as Requirement;
                if (mRequirement != null && mRequirement.ArrowImage == Constraints.Arrow_Right)
                {
                    mRequirement.ArrowImage = Constraints.Arrow_Down;
                    mRequirement.GridBg = (Color)App.Current.Resources["LightGray"];
                    mRequirement.MoreDetail = true;
                    mRequirement.HideDetail = false;
                    mRequirement.NameFont = 15;
                }
                else
                {
                    mRequirement.ArrowImage = Constraints.Arrow_Right;
                    mRequirement.GridBg = Color.Transparent;
                    mRequirement.MoreDetail = false;
                    mRequirement.HideDetail = true;
                    mRequirement.NameFont = 13;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/BtnRequerments_Tapped: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    var ReqSearch = mRequirements.Where(x =>
                                                        x.RequirementNo.ToLower().Contains(entrSearch.Text.ToLower()) ||
                                                        x.Title.ToLower().Contains(entrSearch.Text.ToLower())).ToList();
                    if (ReqSearch != null && ReqSearch.Count > 0)
                    {
                        lstRequirements.IsVisible = true;
                        FrmSortBy.IsVisible = true;
                        //FrmStatusBy.IsVisible = true;
                        FrmFilterBy.IsVisible = true;
                        lblNoRecord.IsVisible = false;
                        lstRequirements.ItemsSource = ReqSearch.ToList();
                    }
                    else
                    {
                        lstRequirements.IsVisible = false;
                        // FrmStatusBy.IsVisible = false;
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
                Common.DisplayErrorMessage("RequirementsView/entrSearch_TextChanged: " + ex.Message);
            }
        }

        private void lstRequirements_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                if (this.mRequirements.Count < 10)
                    return;
                if (this.mRequirements.Count == 0)
                    return;

                var lastrequirement = this.mRequirements[this.mRequirements.Count - 1];
                var lastAppearing = (Requirement)e.Item;
                if (lastAppearing != null)
                {
                    if (lastrequirement == lastAppearing)
                    {
                        var totalAspectedRow = pageSize * pageNo;
                        pageNo += 1;

                        if (this.mRequirements.Count() >= totalAspectedRow)
                        {
                            GetActiveRequirements(filterBy, sortBy);
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
                Common.DisplayErrorMessage("RequirementsView/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private void lstRequirements_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstRequirements.IsRefreshing = true;
                pageNo = 1;
                mRequirements.Clear();
                GetActiveRequirements(filterBy, sortBy);
                lstRequirements.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/lstRequirements_Refreshing: " + ex.Message);
            }
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            entrSearch.Text = string.Empty;
            BindList();
        }

        //private async void FrmStatusBy_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        StatusPopup statusPopup = new StatusPopup();
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
                        GetActiveRequirements(filterBy, sortBy);
                    }
                };
                PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/CustomEntry_Unfocused: " + ex.Message);
            }
        }
        #endregion

        private void GrdRequirements_Tapped(object sender, EventArgs e)
        {
            try
            {
                var GridExp = (Grid)sender;
                var mRequirement = GridExp.BindingContext as Requirement;
                Navigation.PushAsync(new RequirementDetailPage(mRequirement.RequirementId));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/GrdRequirements_Tapped: " + ex.Message);
            }
        }
    }
}