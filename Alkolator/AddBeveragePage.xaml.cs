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

        // Konstruktor
        public AddBeveragePage()
        {
            InitializeComponent();
            Beverage = new Beverage();
        }

        private async void AddBtn_Click(object sender, EventArgs e)
        {
            // Ustawiamy w³aœciwoœci napoju
            Beverage.Name = tbName.Text;

            // Sprawdzamy poprawnoœæ danych
            if (!double.TryParse(tbPrice.Text, out double cena))
            {
                await DisplayAlert("B³¹d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.Price = cena;

            if (!double.TryParse(tbMl.Text, out double ml))
            {
                await DisplayAlert("B³¹d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.Volume = (int)ml;

            if (!double.TryParse(tbVolt.Text, out double v))
            {
                await DisplayAlert("B³¹d", "Nie jest to poprawna liczba", "OK");
                return;
            }
            Beverage.ABV = v;

            Beverage.calculateCostEfficiency();
            Beverage.calculateEthanol();
            
            // Wysy³amy wiadomoœæ do MainPage z napojem
            MessagingCenter.Send(this, "AddBeverage", Beverage);

            // Wracamy na poprzedni¹ stronê
            await Navigation.PopAsync();
            _pageClosedTcs.TrySetResult(true);
        }
    }
}
