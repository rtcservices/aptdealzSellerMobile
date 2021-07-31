using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace aptdealzSellerMobile.Views.OtherPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodeScanPage : ContentPage
    {
        #region Objects
        private ZXingScannerView zxing;
        private ZXingDefaultOverlay overlay;
        //public EventHandler isRefresh;
        #endregion

        public QrCodeScanPage()
        {
            InitializeComponent();
            StartScanning();

        }

        #region Method
        private void StartScanning()
        {
            try
            {
                zxing = new ZXingScannerView
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                zxing.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(async () =>
                {
                    zxing.IsAnalyzing = false;
                    //isRefresh?.Invoke(result.Text, EventArgs.Empty);
                    if (result != null && !Common.EmptyFiels(result.Text))
                    {
                        await DependencyService.Get<IOrderRepository>().ScanQRCodeAndUpdateOrder(result.Text);
                    }
                    await Navigation.PopAsync();
                });

                overlay = new ZXingDefaultOverlay
                {
                    BottomText = "Scanning will happen automatically",
                    ShowFlashButton = zxing.HasTorch,
                };
                overlay.FlashButtonClicked += (sender, e) =>
                {
                    zxing.IsTorchOn = !zxing.IsTorchOn;
                };
                var grid = new Grid
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                grid.Children.Add(zxing);
                grid.Children.Add(overlay);

                Label lblTopText = new Label()
                {
                    Margin = new Thickness(10, 70, 10, 0),
                    Text = "Scan the buyer's QR code only after providing them the products.",
                    LineBreakMode = LineBreakMode.WordWrap,
                    TextColor = Color.White,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                grid.Children.Add(lblTopText);

                var image = new ImageButton
                {
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    HeightRequest = 50,
                    WidthRequest = 50,
                    BackgroundColor = Color.Transparent,
                    Padding = new Thickness(0, 10),
                    Margin = new Thickness(10, 10, 0, 0),
                    Source = Constraints.Arrow_Back_Left,
                };
                image.Clicked += delegate
                {
                    Navigation.PopAsync();
                };

                grid.Children.Add(image);

                Content = grid;

                zxing.IsScanning = true;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QrCodeScanView/StartScanning: " + ex.Message);

            }
        }
        #endregion   
    }
}