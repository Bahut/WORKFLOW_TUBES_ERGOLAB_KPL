using System;
using System.Diagnostics;
using WORKFLOW_TUBES_KPL_ERGOLAB.Core;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Testing
{
    public class TableDrivenPerformanceTest
    {
        public static void Run()
        {
            Console.WriteLine("--- Memulai Performance Test: Table-Driven ---");

            ComplaintSlaTable rules = new ComplaintSlaTable();
            int iterations = 100000; 

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < iterations; i++)
            {
                rules.GetSLADays("Infrastruktur", "Berat");
                rules.GetUnit("Keamanan", "Sedang");
            }

            stopwatch.Stop();
            Console.WriteLine($"Eksekusi Table-Driven {iterations} kali selesai.");
            Console.WriteLine($"Waktu yang dibutuhkan: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("-----------------------------------------------\n");
        }
    }
}