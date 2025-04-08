using System;
using System.Xml.Linq;
using AlkoLib;
using Microsoft.Maui.Controls;

namespace Alkolator
{
    public partial class EditPage : ContentPage
    {
        public Beverage Beverage { get; private set; }
        public Task PageClosedTask => _pageClosedTcs.Task;
        private TaskCompletionSource<bool> _pageClosedTcs = new();

        public EditPage(Beverage beverage)
        {
            InitializeComponent();
            Beverage = beverage;

            // Wype³niamy pola danymi istniej¹cego napoju
            tbName.Text = Beverage.Name;
            tbPrice.Text = Beverage.Price.ToString();
            tbMl.Text = Beverage.Volume.ToString();
            tbVolt.Text = Beverage.ABV.ToString();
        }

        private async void EditBtn_Click(object sender, EventArgs e)
        {
            // Aktualizujemy dane obiektu
            Beverage.setName(tbName.Text);

            if (!double.TryParse(tbPrice.Text, out double cena))
            {
                await DisplayAlert("B³¹d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.setPrice(cena);

            if (!double.TryParse(tbMl.Text, out double ml))
            {
                await DisplayAlert("B³¹d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.setVolume((int)ml);

            if (!double.TryParse(tbVolt.Text, out double v))
            {
                await DisplayAlert("B³¹d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.setABV(v);

            Beverage.calculateCostEfficiency();
            Beverage.calculateEthanol();

            // Mo¿esz wys³aæ komunikat jeœli chcesz np. do MainPage o aktualizacji
            MessagingCenter.Send(this, "EdytujBeverage", Beverage);

            await Navigation.PopAsync();
            _pageClosedTcs.TrySetResult(true);
        }
    }
}
