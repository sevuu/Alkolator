using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AlkoLib
{
    public class Save
    {
        public static async Task Txt(IList<Beverage> beverages, string path)
        {
            try
            {
                using FileStream fs = new FileStream(path, FileMode.Create);
                using StreamWriter sw = new StreamWriter(fs);

                foreach (var s in beverages)
                {
                    await sw.WriteLineAsync(s.Name);
                    await sw.WriteLineAsync(s.Volume.ToString());
                    await sw.WriteLineAsync(s.ABV.ToString());
                    await sw.WriteLineAsync(s.Price.ToString());
                    await sw.WriteLineAsync(s.Spejson.ToString());
                    await sw.WriteLineAsync(s.Ethanol.ToString());
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "OK");
            }
        }

        public static async Task JSON(IList<Beverage> beverages, string path)
        {
            try
            {
                string json = JsonSerializer.Serialize(beverages, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(path, json);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "OK");
            }
        }
    }
}
