using System;
using System.Collections.Generic;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Core
{
    public class ComplaintSlaTable
    {
        // Hapus StringComparer dari constructor Dictionary Tuple
        private readonly Dictionary<(string, string), string> assignmentMatrix = new Dictionary<(string, string), string>()
        {
            { ("infrastruktur", "berat"), "Unit Infrastruktur" },
            { ("keamanan", "sedang"), "Unit Keamanan" }
        };

        private readonly Dictionary<(string, string), int> slaMatrix = new Dictionary<(string, string), int>()
        {
            { ("keamanan", "sedang"), 1 },
            { ("infrastruktur", "berat"), 3 }
        };

        public string CheckEscalation(ComplaintStatus status, int daysOverdue)
        {
            if (status == ComplaintStatus.Diverifikasi && daysOverdue >= 3)
            {
                return "Eskalasi ke Lurah";
            }
            return "Tim Operasional";
        }

        public string GetNotificationTemplate(ComplaintStatus status, string recipient)
        {
            if (status == ComplaintStatus.Diajukan && recipient.Equals("Warga", StringComparison.OrdinalIgnoreCase))
            {
                return "Halo Warga, laporan Anda telah diajukan.";
            }
            return string.Empty;
        }

        public string GetUnit(string category, string severity)
        {
            var key = (category?.ToLower() ?? "", severity?.ToLower() ?? "");
            return assignmentMatrix.TryGetValue(key, out string? unit) ? unit : "Unit Umum";
        }

        public int GetSLADays(string category, string severity)
        {
            var key = (category?.ToLower() ?? "", severity?.ToLower() ?? "");
            return slaMatrix.TryGetValue(key, out int days) ? days : 7;
        }
    }
}