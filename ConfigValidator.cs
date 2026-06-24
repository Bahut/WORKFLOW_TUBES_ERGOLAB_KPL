using System;
using System.Linq;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Config
{
    public static class ConfigValidator
    {
        public static void Validate(CategoryConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (config.Categories == null || !config.Categories.Any())
                throw new InvalidOperationException(
                    "Daftar kategori tidak boleh kosong");

            var duplicateIds = config.Categories
                .GroupBy(x => x.Id)
                .Where(g => g.Count() > 1);

            if (duplicateIds.Any())
                throw new InvalidOperationException(
                    "Terdapat ID kategori duplikat");
        }

        public static void Validate(SlaConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (config.Rules == null || !config.Rules.Any())
                throw new InvalidOperationException(
                    "Daftar SLA tidak boleh kosong");

            foreach (var rule in config.Rules)
            {
                if (string.IsNullOrWhiteSpace(rule.Category))
                    throw new InvalidOperationException(
                        "Category tidak boleh kosong");

                if (rule.MaxDays <= 0)
                    throw new InvalidOperationException(
                        "MaxDays harus lebih besar dari 0");
            }
        }
    }
}