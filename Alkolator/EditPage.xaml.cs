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

            // Wype�niamy pola danymi istniej�cego napoju
            tbName.Text = Beverage.Name;
            tbPrice.Text = Beverage.Price.ToString();
            tbMl.Text = Beverage.Volume.ToString();
            tbAbv.Text = Beverage.ABV.ToString();
            tbAmount.Text = Beverage.Amount.ToString();
        }

        private async void EditBtn_Click(object sender, EventArgs e)
        {
            // Aktualizujemy dane obiektu
            Beverage.setName(tbName.Text);

            if (!double.TryParse(tbPrice.Text, out double cena))
            {
                await DisplayAlert("B��d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.setPrice(cena);

            if (!double.TryParse(tbMl.Text, out double ml))
            {
                await DisplayAlert("B��d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.setVolume((int)ml);

            if (!double.TryParse(tbAbv.Text, out double v))
            {
                await DisplayAlert("B��d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.setABV(v);

            if (!int.TryParse(tbAmount.Text, out int amount))
            {
                await DisplayAlert("B��d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.Amount = amount;

            Beverage.calculateCostEfficiency();
            Beverage.calculateEthanol();

            // Mo�esz wys�a� komunikat je�li chcesz np. do MainPage o aktualizacji
            MessagingCenter.Send(this, "EdytujBeverage", Beverage);

            await Navigation.PopAsync();
            _pageClosedTcs.TrySetResult(true);
        }
        private void Increment_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(tbAmount.Text, out int value))
            {
                value++;
                tbAmount.Text = value.ToString();
            }
        }

        private void Decrement_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(tbAmount.Text, out int value) && value > 1)
            {
                value--;
                tbAmount.Text = value.ToString();
            }
        }
    }
}
