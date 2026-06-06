using System.Text.Json;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Config
{
    public static class ConfigLoader
    {
        public static T Load<T>(string filePath)
        {
            string fullPath = Path.IsPathRooted(filePath)
                ? filePath
                : Path.GetFullPath(
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        filePath));

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(
                    $"Config file tidak ditemukan: {fullPath}");
            }

            string json = File.ReadAllText(fullPath);

            T? result = JsonSerializer.Deserialize<T>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result == null)
            {
                throw new Exception(
                    $"Gagal membaca konfigurasi: {fullPath}");
            }

            return result;
        }
    }
}