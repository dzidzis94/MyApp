using MyApp.Web.Models;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace MyApp.Web.Services
{
    public class DataService
    {
        private readonly string _filePath;
        public ApplicationData Data { get; private set; }

        public DataService(IWebHostEnvironment webHostEnvironment)
        {
            _filePath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "database.json");
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Data = JsonSerializer.Deserialize<ApplicationData>(json, options) ?? new ApplicationData();
            }
            else
            {
                Data = new ApplicationData();
            }
        }

        public void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(Data, options);
            var directory = Path.GetDirectoryName(_filePath);
            if(directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(_filePath, json);
        }
    }
}
