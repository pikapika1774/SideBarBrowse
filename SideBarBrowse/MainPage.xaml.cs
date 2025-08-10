using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace SideBarBrowse
{
    public partial class MainPage : ContentPage
    {
        private const string PrefDockSide = "DockSide"; // "Left" or "Right"

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

#if WINDOWS
            try
            {
                var hwnd = HwndHelper.GetMainWindowHandle();
                AppBarService.RegisterAppBar(hwnd);

                // 既に選択済みなら即ドック、未選択ならオーバーレイで必ず選ばせる
                var saved = Preferences.Get(PrefDockSide, string.Empty);
                if (saved == "Left")
                {
                    AppBarService.Dock(AppBarEdge.Left, 0.25);
                }
                else if (saved == "Right")
                {
                    AppBarService.Dock(AppBarEdge.Right, 0.25);
                }
                else
                {
                    SideChooserOverlay.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("エラー", ex.Message, "OK");
            }
#endif

            // 初期ページをGoogleに
            NavigateTo("https://www.google.com");
        }

        private void NavigateTo(string input)
        {
            var text = (input ?? "").Trim();
            if (string.IsNullOrEmpty(text)) return;

            // http(s) で始まるならそのまま遷移
            if (text.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                text.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                AddressEntry.Text = text;
                Browser.Source = text;
                return;
            }

            // それ以外は Google 検索
            var q = Uri.EscapeDataString(text);
            var searchUrl = $"https://www.google.com/search?q={q}";
            AddressEntry.Text = searchUrl;
            Browser.Source = searchUrl;
        }
        private void BackButton_Clicked(object sender, EventArgs e)
        {
            if (Browser.CanGoBack) Browser.GoBack();
        }

        private void ForwardButton_Clicked(object sender, EventArgs e)
        {
            if (Browser.CanGoForward) Browser.GoForward();
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            if (Browser.Source is UrlWebViewSource u)
                Browser.Source = new UrlWebViewSource { Url = u.Url };
        }

        private void GoButton_Clicked(object sender, EventArgs e)
        {
            NavigateTo(AddressEntry.Text?.Trim() ?? "");
        }

        private void AddressEntry_Completed(object sender, EventArgs e)
        {
            NavigateTo(AddressEntry.Text?.Trim() ?? "");
        }

        private void Browser_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url != null) AddressEntry.Text = e.Url;
        }

        // 左右選択（初回オーバーレイ/ツールバー両対応）
        private void DockLeft_Clicked(object sender, EventArgs e)
        {
#if WINDOWS
            AppBarService.Dock(AppBarEdge.Left, 0.25);
            Preferences.Set(PrefDockSide, "Left");
#endif
            SideChooserOverlay.IsVisible = false;
        }

        private void DockRight_Clicked(object sender, EventArgs e)
        {
#if WINDOWS
            AppBarService.Dock(AppBarEdge.Right, 0.25);
            Preferences.Set(PrefDockSide, "Right");
#endif
            SideChooserOverlay.IsVisible = false;
        }
    }
}
