using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AlkoLib;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Alkolator
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Beverage> Beverages { get; set; }
        private string DataFile = Path.Combine(FileSystem.AppDataDirectory, "database.json");

        private string _bestBeverageName;
        public string BestBeverageName
        {
            get => _bestBeverageName;
            set
            {
                _bestBeverageName = value;
                OnPropertyChanged(nameof(BestBeverageName));
            }
        }

        public MainPage()
        {
            InitializeComponent();
            Beverages = new ObservableCollection<Beverage>();

            LoadData();
            dataGrid.ItemsSource = Beverages;

            MessagingCenter.Subscribe<AddBeveragePage, Beverage>(this, "AddBeverage", (sender, beverage) =>
            {
                Beverages.Add(beverage);
                SaveData();
            });

            BindingContext = this;
            bestBeverage();
            SortPicker.SelectedIndex = 1;
        }

        private Beverage _selectedBeverage;
        public Beverage SelectedBeverage
        {
            get => _selectedBeverage;
            set
            {
                _selectedBeverage = value;
                OnPropertyChanged(nameof(SelectedBeverage));
            }
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Beverage selected)
            {
                SelectedBeverage = selected;
                EditBtn.IsEnabled = true;
            }
        }

        private async void AddBtn_Click(object sender, EventArgs e)
        {
            var addPage = new AddBeveragePage();
            await Navigation.PushAsync(addPage);

            // Czekamy aż użytkownik zamknie AddBeveragePage
            // i wtedy wykonujemy metodę
            // UWAGA: to wykona się PO powrocie do MainPage
            await addPage.PageClosedTask;

            bestBeverage();
            SortBeverages(SortPicker.SelectedIndex);

        }

        private async void EditBtn_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedItem is Beverage selected)
            {
                var editPage = new EditPage(selected);
                await Navigation.PushAsync(editPage);
                await editPage.PageClosedTask;

                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = Beverages;

                bestBeverage();
                SortBeverages(SortPicker.SelectedIndex);
                await SaveData();

            }
        }

        private async void DelBtn_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedItem is Beverage selected)
            {
                Beverages.Remove(selected);
                await SaveData();
            }
            bestBeverage();
            SortBeverages(SortPicker.SelectedIndex);

        }

        private void bestBeverage()
        {
            var najlepszy = Beverages.OrderByDescending(b => b.Spejson).FirstOrDefault();
            BestBeverageName = "Najopłacalniejszy: " + najlepszy?.Name;
        }

        private void SortBeverages(int selectedIndex)
        {
            if (Beverages == null || Beverages.Count == 0) return;

            IEnumerable<Beverage> sortedList = Beverages;

            switch (selectedIndex)
            {
                case 0: // Nazwa A-Z
                    sortedList = Beverages.OrderBy(b => b.Name);
                    break;
                case 1: // Spejson ↓
                    sortedList = Beverages.OrderByDescending(b => b.Spejson);
                    break;
                case 2: // Cena ↑
                    sortedList = Beverages.OrderBy(b => b.Price);
                    break;
                case 3: // Etanol ↓
                    sortedList = Beverages.OrderByDescending(b => b.Ethanol);
                    break;
                case 4: // Procent ↓
                    sortedList = Beverages.OrderByDescending(b => b.ABV);
                    break;
                default:
                    return;
            }

            Beverages = new ObservableCollection<Beverage>(sortedList);
            dataGrid.ItemsSource = Beverages;
        }

        private void SortPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = SortPicker.SelectedIndex;
            SortBeverages(selectedIndex);
        }


        private async void SaveBtn_Click(object sender, EventArgs e) { }
        private async void LoadBtn_Click(object sender, EventArgs e) { }



        private async Task SaveData()
        {
            try
            {
                var json = JsonSerializer.Serialize(Beverages);
                await File.WriteAllTextAsync(DataFile, json);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Błąd zapisu danych: {ex.Message}", "OK");
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(DataFile))
                {
                    var json = File.ReadAllText(DataFile);
                    var loadedBeverages = JsonSerializer.Deserialize<ObservableCollection<Beverage>>(json);

                    if (loadedBeverages != null)
                    {
                        Beverages.Clear();
                        foreach (var beverage in loadedBeverages)
                        {
                            Beverages.Add(beverage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Błąd", $"Błąd ładowania danych: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string base64 = Save.ToBase64(Beverages);
            string result = await DisplayPromptAsync(
                "Dane do przesłania",
                "Base64 (kliknij 'Zapisz do schowka', aby skopiować):",
                initialValue: base64,
                accept: "Zapisz do schowka",
                cancel: "Anuluj"
            );

            // If the user clicks "Zapisz do schowka" (Save to Clipboard)
            if (!string.IsNullOrWhiteSpace(result) && result == base64)
            {
                await Clipboard.SetTextAsync(base64);
                await DisplayAlert("Sukces", "Zakodowane dane zostały skopiowane do schowka!", "OK");
            }
        }


        private async void OnLoadClicked(object sender, EventArgs e)
        {
            string base64 = await DisplayPromptAsync("Wklej dane", "Wklej dane w formacie Base64+zlib:");

            if (!string.IsNullOrWhiteSpace(base64))
            {
                try
                {
                    // Decode and deserialize the Base64 string
                    var loaded = Save.FromBase64<ObservableCollection<Beverage>>(base64);

                    if (loaded != null)
                    {
                        // Append the loaded beverages to the existing collection
                        foreach (var beverage in loaded)
                        {
                            Beverages.Add(beverage);
                        }

                        // Notify the UI of changes
                        OnPropertyChanged(nameof(Beverages));

                        await DisplayAlert("Sukces", "Dane zostały załadowane i dodane do istniejącej kolekcji!", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Błąd", "Nie udało się wczytać danych: Pusta kolekcja.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Błąd", $"Nie udało się wczytać danych: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Błąd", "Nie podano danych do wczytania.", "OK");
            }

            bestBeverage();
            SortBeverages(SortPicker.SelectedIndex);
            await SaveData();
        }


    }

}
