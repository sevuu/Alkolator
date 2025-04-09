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

        private async void SpejsonTapped(object sender, EventArgs e)
        {
            string spejson = "Użytkownik serwisu wykop.pl CzlowiekMagnetowid podaje przykład:\r\n" +
                "500 ml 40 % żubrówki białej kosztuje 19,99 zł. Jej wskaźnik opłacalności to: 40/19,99=" +
                " 2.00100050025\r\n\r\nGdy kupujemy alkohol o innej objętości, musimy obliczyć proporcjonalnie" +
                " ile alkoholu byłoby w 500 ml za tą cenę.\r\n\r\n100 ml spirytusu rektyfikowanego POLMOS 95% kosztuje 13,95 zł." +
                " Cieczy jest 5x mniej w stosunku do 500 ml, więc do równania musimy podzielić % przez 5= 19. Wtedy otrzymujemy 500 ml" +
                " 19% cieczy za 13,95 ergo 19/13,95= 1.36200716846\r\n\r\nOznacza to, że kupno 100 ml spirytusu POLMOS jest znacznie mniej" +
                " opłacalne od kupna 0,5 l żubrówki białej. \r\n\r\nW skrócie: im większy współczynnik tym lepiej";

            await DisplayAlert("Spejson", spejson, "Mhm, czaję");
        }
    }
}
