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
    public partial class CurrentlyShippingPage : ContentPage
    {
        #region Objects
        private List<CurrentlyShipping> mCurrentlyShippings = new List<CurrentlyShipping>();
        #endregion

        #region Constructor
        public CurrentlyShippingPage()
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
            lstCurrentlyShipping.ItemsSource = null;

            mCurrentlyShippings = new List<CurrentlyShipping>()
            {
                new CurrentlyShipping
                {
                    InvoceId="INV#123",
                    ShippingDate="12-01-2021",
                    Description="Shipped with DTDC",
                    Amount=1800
                },
                new CurrentlyShipping
                {
                    InvoceId="INV#123",
                    ShippingDate="12-01-2021",
                    Description="Shipped with DTDC",
                    Amount=1800
                },
                new CurrentlyShipping
                {
                    InvoceId="INV#123",
                    ShippingDate="12-01-2021",
                    Description="Shipped with DTDC",
                    Amount=1800
                },
                new CurrentlyShipping
                {
                    InvoceId="INV#123",
                    ShippingDate="12-01-2021",
                    Description="Shipped with DTDC",
                    Amount=1800
                },
            };

            lstCurrentlyShipping.ItemsSource = mCurrentlyShippings.ToList();
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

                var response = (CurrentlyShipping)selectGrid.BindingContext;
                if (response != null)
                {
                    foreach (var selectedImage in mCurrentlyShippings)
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

        private async void ImgSearch_Tapped(object sender, EventArgs e)
        {
            try
            {
                SearchPopup searchPopup = new SearchPopup();
                searchPopup.isRefresh += (s1, e1) =>
                {
                    lstCurrentlyShipping.ItemsSource = mCurrentlyShippings.ToList();

                };
                await PopupNavigation.Instance.PushAsync(searchPopup);
            }
            catch (Exception ex)
            {

            }
        }

        private async void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                SortByPopup sortByPopup = new SortByPopup();
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

        private async void FrmStatus_Tapped(object sender, EventArgs e)
        {
            try
            {
                StatusPopup statusPopup = new StatusPopup();
                statusPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
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

        private void lsCurrentShipingDetails_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private async void frmTrack_Tapped(object sender, EventArgs e)
        {
            try
            {
                var frm = (Frame)sender;
                if (frm != null)
                {
                    await frm.ScaleTo(0.9, 100, Easing.Linear);
                    await frm.ScaleTo(1.0, 100, Easing.Linear);

                    var frmScanQrCode = (CurrentlyShippingPage)frm.BindingContext;
                    if (frmScanQrCode != null && frmScanQrCode.IsVisible == true)
                    {
                        //Navigation
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void frmTrackUp_Tapped(object sender, EventArgs e)
        {
            try
            {
                var frmTrackUp = (Frame)sender;
                if (frmTrackUp != null)
                {
                    await frmTrackUp.ScaleTo(0.9, 100, Easing.Linear);
                    await frmTrackUp.ScaleTo(1.0, 100, Easing.Linear);

                    var frmScanQrCode = (CurrentlyShippingPage)frmTrackUp.BindingContext;
                    if (frmScanQrCode != null && frmScanQrCode.IsVisible == true)
                    {
                        //Navigation
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}