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
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
                };
                Data = JsonSerializer.Deserialize<ApplicationData>(json, options) ?? new ApplicationData();
            }
            else
            {
                Data = new ApplicationData();
            }
        }

        // Since we are using a JSON file as a database, we need to manually handle ID generation.
        // These methods find the current maximum ID in a collection and return the next available ID.
        public int GetNextUserId() => Data.Users.Any() ? Data.Users.Max(u => u.Id) + 1 : 1;
        public int GetNextProjectId() => Data.Projects.Any() ? Data.Projects.Max(p => p.Id) + 1 : 1;
        public int GetNextSubSectionId(Project project) => project.SubSections.Any() ? project.SubSections.Max(s => s.Id) + 1 : 1;

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
