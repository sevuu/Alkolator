using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
                    await sw.WriteLineAsync(s.Amount.ToString());
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

        public static string ToBase64<T>(IList<T> data)
        {
            string json = JsonSerializer.Serialize(data);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            using var ms = new MemoryStream();
            using (var ds = new DeflateStream(ms, CompressionLevel.Optimal, true))
            {
                ds.Write(jsonBytes, 0, jsonBytes.Length);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static T? FromBase64<T>(string base64)
        {
            byte[] compressedBytes = Convert.FromBase64String(base64);

            using var ms = new MemoryStream(compressedBytes);
            using var ds = new DeflateStream(ms, CompressionMode.Decompress);
            using var sr = new StreamReader(ds, Encoding.UTF8);
            string json = sr.ReadToEnd();

            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
