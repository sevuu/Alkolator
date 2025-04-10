using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AlkoLib
{
    public class Load
    {
        public static async Task<List<Beverage>> JSON(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd", "Plik nie istnieje.", "OK");
                    return new List<Beverage>();
                }

                string json = await File.ReadAllTextAsync(path);
                var beverages = JsonSerializer.Deserialize<List<Beverage>>(json) ?? new List<Beverage>();

                return beverages;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd JSON", ex.Message, "OK");
                return new List<Beverage>();
            }
        }

        
    }
}
