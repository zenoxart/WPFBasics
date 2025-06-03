using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Zenox.Wpf.Core.Common.Storage
{
    public static class SecureStorage
    {
        private static readonly string StorageFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FluentApp", "securestorage.dat");

        static SecureStorage()
        {
            var dir = Path.GetDirectoryName(StorageFilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public static void SetDouble(string key, double value)
        {
            SetString(key, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        public static double? GetDouble(string key)
        {
            var str = GetString(key);
            if (double.TryParse(str, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var result))
                return result;
            return null;
        }

        public static void SetString(string key, string value)
        {
            var all = LoadAll();
            all[key] = value;
            SaveAll(all);
        }

        public static string GetString(string key)
        {
            var all = LoadAll();
            return all.TryGetValue(key, out var value) ? value : null;
        }

        private static void SaveAll(System.Collections.Generic.Dictionary<string, string> data)
        {
            var sb = new StringBuilder();
            foreach (var kv in data)
            {
                sb.AppendLine($"{kv.Key}={kv.Value}");
            }
            var plain = Encoding.UTF8.GetBytes(sb.ToString());
            var encrypted = ProtectedData.Protect(plain, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(StorageFilePath, encrypted);
        }

        private static System.Collections.Generic.Dictionary<string, string> LoadAll()
        {
            var dict = new System.Collections.Generic.Dictionary<string, string>();
            if (!File.Exists(StorageFilePath))
                return dict;

            try
            {
                var encrypted = File.ReadAllBytes(StorageFilePath);
                var plain = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                var content = Encoding.UTF8.GetString(plain);
                foreach (var line in content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var idx = line.IndexOf('=');
                    if (idx > 0)
                    {
                        var key = line.Substring(0, idx);
                        var value = line.Substring(idx + 1);
                        dict[key] = value;
                    }
                }
            }
            catch
            {
                // Ignorieren bei Fehlern (z.B. Korruption)
            }
            return dict;
        }
    }
}
