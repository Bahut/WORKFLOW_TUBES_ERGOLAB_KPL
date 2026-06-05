using System;
using System.Collections.Generic;
using System.Diagnostics;
using TUBES_KPL.Core;
using TUBES_KPL.Models;

namespace TUBES_KPL.Testing
{
    public class GenericsPerformanceTest
    {
        public static void Run()
        {
            Stopwatch sw = new Stopwatch();

            sw.Restart();
            ComplaintRepository repo = new ComplaintRepository();
            for (int i = 0; i < 1000; i++)
            {
                Complaint c = new Complaint($"Pengaduan {i}", "Kebersihan", "Ringan", $"Deskripsi {i}", "Bandung", "Warga");
                repo.Add(c);
            }
            sw.Stop();
            Console.WriteLine($"[PERF] Add 1000 items ke Repository: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            List<Complaint> items = repo.GetAll();
            PagedList<Complaint> paged = new PagedList<Complaint>(items, 1, 1000, items.Count);
            sw.Stop();
            Console.WriteLine($"[PERF] PagedList 1000 items: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            RequiredStringRule required = new RequiredStringRule();
            MaxLengthRule maxLength = new MaxLengthRule(100);
            List<ValidationRule<string>> rules = new List<ValidationRule<string>> { required, maxLength };
            for (int i = 0; i < 1000; i++)
                required.ValidateAll($"Input ke-{i}", rules, out _);
            sw.Stop();
            Console.WriteLine($"[PERF] Batch validation 1000x: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            for (int i = 0; i < 1000; i++)
            {
                Complaint c = new Complaint($"Judul {i}", "Keamanan", "Berat", $"Desc {i}", "Medan", "User");
                Result<Complaint> result = Result<Complaint>.Ok(c, "OK");
            }
            sw.Stop();
            Console.WriteLine($"[PERF] Result<T> creation 1000x: {sw.ElapsedMilliseconds} ms");
        }
    }
}