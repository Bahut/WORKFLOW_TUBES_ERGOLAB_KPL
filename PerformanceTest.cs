using System;
using System.Diagnostics;
using WORKFLOW_TUBES_KPL_ERGOLAB.Core;
using WORKFLOW_TUBES_KPL_ERGOLAB.Models;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Testing
{
    public class PerformanceTest
    {
        public static void Run()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ComplaintWorkflow workflow = new ComplaintWorkflow();

            for (int i = 0; i < 10000; i++)
            {
                Complaint complaint = new Complaint("Test", "Infrastruktur", "Deskripsi", "Bandung", "User");
                workflow.ChangeStatus(complaint, "verify");
                workflow.ChangeStatus(complaint, "process");
                workflow.ChangeStatus(complaint, "finish");
            }

            stopwatch.Stop();
            Console.WriteLine($"Waktu eksekusi (10.000 iterasi): {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}