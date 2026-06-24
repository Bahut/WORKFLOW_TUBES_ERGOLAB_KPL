using System.Text.Json;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Config
{
    public static class ConfigLoader
    {
        private static readonly JsonSerializerOptions Options =
            new()
            {
                PropertyNameCaseInsensitive = true
            };

        public static T Load<T>(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(
                    "Path konfigurasi tidak boleh kosong");

            string fullPath = Path.GetFullPath(filePath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException(
                    $"File konfigurasi tidak ditemukan: {fullPath}");

            try
            {
                using FileStream stream =
                    File.OpenRead(fullPath);

                T? result =
                    JsonSerializer.Deserialize<T>(
                        stream,
                        Options);

                if (result == null)
                    throw new InvalidOperationException(
                        $"Isi konfigurasi {filePath} tidak valid");

                ValidateConfig(result);

                return result;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"Format JSON tidak valid pada {filePath}",
                    ex);
            }
        }

        private static void ValidateConfig<T>(T config)
        {
            switch (config)
            {
                case CategoryConfig categoryConfig:
                    ConfigValidator.Validate(categoryConfig);
                    break;

                case SlaConfig slaConfig:
                    ConfigValidator.Validate(slaConfig);
                    break;
            }
        }
    }
}