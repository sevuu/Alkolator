using AlkoLib;
using Microsoft.Maui.Controls;

namespace Alkolator
{
    public partial class DetailsPage : ContentPage
    {
        private Beverage _beverage;
        public Task PageClosedTask => _pageClosedTcs.Task;
        private TaskCompletionSource<bool> _pageClosedTcs = new();

        public DetailsPage(Beverage beverage)
        {
            InitializeComponent();
            _beverage = beverage;

            lblName.Text = _beverage.Name;
            lblMl.Text = $"{_beverage.Volume} ml";
            lblAbv.Text = $"{_beverage.ABV}%";
            lblPrice.Text = $"{_beverage.Price:C}";
            lblAmount.Text = _beverage.Amount.ToString();
            lblEthanol.Text = $"{_beverage.Ethanol} g";
            lblSpejson.Text = $"{_beverage.Spejson}";
            lblDrunk.Text = $"{Math.Round(150/(beverage.Ethanol/beverage.Amount),1)}";    // 150ML JEST WPISANE NA SZTYWNO!!! TODO: dorobić obliczanie tego

            ratingSlider.Value = _beverage.Rating;
            lblRating.Text = $"Ocena: {_beverage.Rating}";
        }



        private void OnRatingChanged(object sender, ValueChangedEventArgs e)
        {
            int rating = (int)e.NewValue;
            lblRating.Text = $"Ocena: {rating}";
        }

        private async void OnSaveRatingClicked(object sender, EventArgs e)
        {
            _beverage.Rating = (int)ratingSlider.Value;

            var mainPage = Application.Current.MainPage as MainPage;
            if (mainPage != null)
            {
                await mainPage.SaveData();
            }


            await Navigation.PopAsync();
            _pageClosedTcs.TrySetResult(true);
        }
    }
}
