﻿using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.OtherPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckOutPage : ContentPage
    {
        public event EventHandler PaidEvent;
        private bool isSuccess = false;
        private bool isSuccessPushed = false;

        public CheckOutPage(string url)
        {
            InitializeComponent();

            wbView.Source = new Uri(url);
            wbView.Navigating += WbView_Navigating;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (!isSuccess)
            {
                PaidEvent?.Invoke(null, EventArgs.Empty);
            }
        }

        private void WbView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            try
            {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                var data = e.Url;
                if (data.StartsWith("https://quotesouk.azurewebsites.net/login"))
                {
                    isSuccess = true;
                    var dataResponse = data.Replace("https://quotesouk.azurewebsites.net/login?", "");
                    if (!string.IsNullOrEmpty(dataResponse))
                    {
                        var datas = dataResponse.Split('&');
                        if (datas != null && datas.Count() > 0)
                        {
                            foreach (var item in datas)
                            {
                                var items = item.Split('=');
                                keyValuePairs.Add(items[0], items[1]);
                            }
                        }
                    }
                }

                if (isSuccess)
                {
                    if (isSuccessPushed) return;
                    isSuccessPushed = true;
                    Navigation.PopAsync();
                    PaidEvent?.Invoke(keyValuePairs, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }
    }
}