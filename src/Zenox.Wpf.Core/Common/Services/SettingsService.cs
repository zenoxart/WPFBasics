using System.IO;
using System.Text.Json;
using Zenox.Wpf.Core.Common;

namespace Zenox.Wpf.Core.Common.Services
{
    /// <summary>
    /// Dienst zum Laden und Speichern von Anwendungseinstellungen als JSON-Datei.
    /// </summary>
    /// <typeparam name="T">Typ der Einstellungen, muss eine Klasse mit parameterlosem Konstruktor sein.</typeparam>
    public class SettingsService<T> where T : class, new()
    {
        private readonly string _filePath;
        private T _settings;

        /// <summary>
        /// Initialisiert eine neue Instanz des <see cref="SettingsService{T}"/> mit dem angegebenen Dateinamen.
        /// </summary>
        /// <param name="fileName">Name der Einstellungsdatei (Standard: "settings.json").</param>
        public SettingsService(string fileName = "settings.json")
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            try
            {
                _settings = Load().Result;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "SettingsService.ctor");
                _settings = new T();
            }
        }

        /// <summary>
        /// Gibt die aktuell geladenen Einstellungen zurück.
        /// </summary>
        public T Value => _settings;

        /// <summary>
        /// Speichert die aktuellen Einstellungen asynchron in die Datei.
        /// </summary>
        public async Task SaveAsync()
        {
            try
            {
                string json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "SettingsService.SaveAsync");
            }
        }

        /// <summary>
        /// Lädt die Einstellungen asynchron aus der Datei.
        /// </summary>
        /// <returns>Die geladenen Einstellungen oder eine neue Instanz, falls die Datei nicht existiert oder ein Fehler auftritt.</returns>
        public async Task<T> Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new T();

                string json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<T>(json) ?? new T();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "SettingsService.Load");
                return new T();
            }
        }

        /// <summary>
        /// Aktualisiert die Einstellungen mit der angegebenen Aktion.
        /// </summary>
        /// <param name="updateAction">Eine Aktion, die auf die Einstellungen angewendet wird.</param>
        public void Update(Action<T> updateAction)
        {
            updateAction(_settings);
        }
    }
}
