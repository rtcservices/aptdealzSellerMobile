using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPopup : PopupPage
    {
        #region Objects 
        ViewRequirementLister mRequirements;
        public event EventHandler isRefresh;
        #endregion

        #region Constructor
        public SearchPopup()
        {
            InitializeComponent();
            mRequirements = new ViewRequirementLister();
        }
        #endregion

        #region Method
        private void BindList()
        {
            if (Common.EmptyFiels(entrSearch.Text))
                mRequirements.SearchCriteria.RequirementNo = entrSearch.Text;

            isRefresh?.Invoke(mRequirements, null);
            PopupNavigation.Instance.PopAsync();
        }
        #endregion

        #region Events
        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            entrSearch.Text = string.Empty;
            PopupNavigation.Instance.PopAsync();
        }

        private void entrSearch_Unfocused(object sender, FocusEventArgs e)
        {
            BindList();
        }
        #endregion

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}