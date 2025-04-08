using System;
using Microsoft.Maui.Controls;

namespace Alkolator
{
    public partial class CreditsPage : ContentPage
    {
        public CreditsPage()
        {
            InitializeComponent();
        }

        private async void OnGitHubTapped(object sender, EventArgs e)
        {
            await Launcher.OpenAsync("https://github.com/sevuu");
        }

        private async void OnPayPalTapped(object sender, EventArgs e)
        {
            await Launcher.OpenAsync("https://ko-fi.com/sevuu");
        }
    }
}
