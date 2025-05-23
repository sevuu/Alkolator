using System;
using AlkoLib;
using Microsoft.Maui.Controls;

namespace Alkolator
{
    public partial class AddBeveragePage : ContentPage
    {
        public Beverage Beverage { get; private set; }
        public Task PageClosedTask => _pageClosedTcs.Task;
        private TaskCompletionSource<bool> _pageClosedTcs = new();

        public AddBeveragePage()
        {
            InitializeComponent();
            Beverage = new Beverage();
        }

        private async void AddBtn_Click(object sender, EventArgs e)
        {
            // Ustawiamy w�a�ciwo�ci napoju
            Beverage.Name = tbName.Text;

            // Sprawdzamy poprawno�� danych
            if (!double.TryParse(tbPrice.Text, out double cena))
            {
                await DisplayAlert("B��d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.Price = cena;

            if (!double.TryParse(tbMl.Text, out double ml))
            {
                await DisplayAlert("B��d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.Volume = (int)ml;

            if (!double.TryParse(tbAbv.Text, out double v))
            {
                await DisplayAlert("B��d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.ABV = v;

            if (!int.TryParse(tbAmount.Text, out int amount))
            {
                await DisplayAlert("B��d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.Amount = amount;

            Beverage.calculateCostEfficiency();
            Beverage.calculateEthanol();
            
            // Wysy�amy wiadomo�� do MainPage z napojem
            MessagingCenter.Send(this, "AddBeverage", Beverage);

            // Wracamy na poprzedni� stron�
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
