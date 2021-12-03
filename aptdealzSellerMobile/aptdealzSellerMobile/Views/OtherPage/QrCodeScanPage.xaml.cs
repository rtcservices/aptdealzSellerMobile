using aptdealzSellerMobile.Model.Reponse;
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
        #region [ Objects ]
        private ZXingScannerView zxing;
        private ZXingDefaultOverlay overlay;
        Order mOrder = new Order();
        #endregion

        #region [ Ctor ]
        public QrCodeScanPage(Order morder)
        {
            InitializeComponent();
            this.mOrder = morder;
            StartScanning();
        }
        #endregion

        #region [ Method ]
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
                    try
                    {
                        zxing.IsAnalyzing = false;
                        //isRefresh?.Invoke(result.Text, EventArgs.Empty);
                        if (result != null && !Common.EmptyFiels(result.Text) && result.Text == mOrder.OrderId)
                        {
                            await DependencyService.Get<IOrderRepository>().ScanQRCodeAndUpdateOrder(result.Text);
                        }
                        else
                        {
                            Common.DisplayErrorMessage("Invalid QRCode");
                        }
                        await Navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        Common.DisplayErrorMessage("StartScanning-BeginInvokeOnMainThread : " + ex.Message);
                    }
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
                    Source = "iconBackArrow.png",
                };
                image.Clicked += async delegate
                {
                    await Navigation.PopAsync();
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