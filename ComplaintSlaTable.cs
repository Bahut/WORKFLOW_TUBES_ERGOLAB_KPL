using System;
using System.Collections.Generic;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Core
{
    public class RuleTable<T>
    {
        private readonly Dictionary<(string, string), T> table =
            new Dictionary<(string, string), T>();

        public void Add(string category, string severity, T value)
        {
            table[(category.ToLower(), severity.ToLower())] = value;
        }

        public T Get(string category, string severity, T defaultValue)
        {
            return table.TryGetValue(
                (category.ToLower(), severity.ToLower()),
                out T value)
                ? value
                : defaultValue;
        }
    }

    public class ComplaintSlaTable
    {
        private readonly RuleTable<string> assignmentMatrix =
            new RuleTable<string>();

        private readonly RuleTable<int> slaMatrix =
            new RuleTable<int>();

        public ComplaintSlaTable()
        {
            assignmentMatrix.Add(
                "infrastruktur",
                "berat",
                "Unit Infrastruktur");

            assignmentMatrix.Add(
                "keamanan",
                "sedang",
                "Unit Keamanan");

            slaMatrix.Add(
                "keamanan",
                "sedang",
                1);

            slaMatrix.Add(
                "infrastruktur",
                "berat",
                3);
        }

        public string GetUnit(string category, string severity)
        {
            return assignmentMatrix.Get(
                category,
                severity,
                "Unit Umum");
        }

        public int GetSLADays(string category, string severity)
        {
            return slaMatrix.Get(
                category,
                severity,
                7);
        }

        public string CheckEscalation(
            ComplaintStatus status,
            int daysOverdue)
        {
            if (status == ComplaintStatus.Diverifikasi &&
                daysOverdue >= 3)
            {
                return "Eskalasi ke Lurah";
            }

            return "Tim Operasional";
        }

        public string GetNotificationTemplate(
            ComplaintStatus status,
            string recipient)
        {
            if (status == ComplaintStatus.Diajukan &&
                recipient.Equals(
                    "Warga",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Halo Warga, laporan Anda telah diajukan.";
            }

            return string.Empty;
        }
    }
}