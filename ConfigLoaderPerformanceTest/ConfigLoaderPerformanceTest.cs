using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using WORKFLOW_TUBES_KPL_ERGOLAB;
using WORKFLOW_TUBES_KPL_ERGOLAB.Core;

namespace TUBES_KPL.Testing
{
    [TestClass]
    public class ConfigLoaderPerformanceTest
    {
        private static readonly string JsonDir = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", ".."
        );

        private string JsonPath(string fileName) =>
            Path.GetFullPath(Path.Combine(JsonDir, fileName));

        private const int MaxSingleLoadMs = 200;
        private const int MaxRepeatedLoadMs = 500;

        [TestMethod]
        public void Performance_LoadCategories_SingleLoad_FastEnough()
        {
            var sw = Stopwatch.StartNew();
            var value = ConfigLoader.Load<CategoryConfig>(JsonPath("categories.json"));
            sw.Stop();

            Assert.IsNotNull(value);
            Assert.IsTrue(sw.ElapsedMilliseconds < MaxSingleLoadMs);
        }

        [TestMethod]
        public void Performance_LoadRoles_SingleLoad_FastEnough()
        {
            var sw = Stopwatch.StartNew();
            var value = ConfigLoader.Load<RoleConfig>(JsonPath("role_permission.json"));
            sw.Stop();

            Assert.IsNotNull(value);
            Assert.IsTrue(sw.ElapsedMilliseconds < MaxSingleLoadMs);
        }

        [TestMethod]
        public void Performance_LoadSlaRules_SingleLoad_FastEnough()
        {
            var sw = Stopwatch.StartNew();
            var value = ConfigLoader.Load<SlaConfig>(JsonPath("sla_rules.json"));
            sw.Stop();

            Assert.IsNotNull(value);
            Assert.IsTrue(sw.ElapsedMilliseconds < MaxSingleLoadMs);
        }

        [TestMethod]
        public void Performance_LoadCategories_100Times_FastEnough()
        {
            ConfigLoader.Load<CategoryConfig>(JsonPath("categories.json"));

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 100; i++)
            {
                ConfigLoader.Load<CategoryConfig>(JsonPath("categories.json"));
            }

            sw.Stop();

            Assert.IsTrue(sw.ElapsedMilliseconds < MaxRepeatedLoadMs);
        }

        [TestMethod]
        public void Performance_LoadAllConfigs_Sequential_FastEnough()
        {
            var sw = Stopwatch.StartNew();

            ConfigLoader.Load<CategoryConfig>(JsonPath("categories.json"));
            ConfigLoader.Load<RoleConfig>(JsonPath("role_permission.json"));
            ConfigLoader.Load<SlaConfig>(JsonPath("sla_rules.json"));
            ConfigLoader.Load<NotificationConfig>(JsonPath("notification_templates.json"));

            sw.Stop();

            Assert.IsTrue(sw.ElapsedMilliseconds < MaxSingleLoadMs * 4);
        }

        [TestMethod]
        public void Performance_FileNotFound_FailsFast()
        {
            var sw = Stopwatch.StartNew();

            object value = Assert.ThrowsException<FileNotFoundException>(() =>
            {
                ConfigLoader.Load<CategoryConfig>("tidak_ada.json");
            });

            sw.Stop();

            Assert.IsTrue(sw.ElapsedMilliseconds < 50);
        }
    }
}