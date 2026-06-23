using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly NotificationConfig _notificationConfig;

        private readonly RuleTable<string> assignmentMatrix =
            new RuleTable<string>();

        private readonly RuleTable<int> slaMatrix =
            new RuleTable<int>();

        // Constructor untuk DI
        public ComplaintSlaTable(
            SlaConfig slaConfig,
            NotificationConfig notificationConfig)
        {
            _notificationConfig = notificationConfig;

            assignmentMatrix.Add(
                "infrastruktur",
                "berat",
                "Unit Infrastruktur");

            assignmentMatrix.Add(
                "keamanan",
                "sedang",
                "Unit Keamanan");

            foreach (var rule in slaConfig.Rules)
            {
                slaMatrix.Add(
                    rule.Category,
                    rule.Impact,
                    rule.MaxDays);
            }
        }

        // Constructor untuk unit test
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
                "kebersihan",
                "ringan",
                3);

            slaMatrix.Add(
                "keamanan",
                "sedang",
                1);

            slaMatrix.Add(
                "infrastruktur",
                "berat",
                1);

            slaMatrix.Add(
                "administrasi",
                "ringan",
                3);

            slaMatrix.Add(
                "umum",
                "sedang",
                2);

            _notificationConfig = new NotificationConfig
            {
                Templates = new List<NotificationTemplate>
                {
                    new()
                    {
                        Status = "Diajukan",
                        Recipient = "Warga",
                        Message = "Laporan '{title}' telah diterima. Menunggu verifikasi."
                    },
                    new()
                    {
                        Status = "Diverifikasi",
                        Recipient = "Petugas",
                        Message = "Laporan '{title}' perlu ditindaklanjuti segera."
                    },
                    new()
                    {
                        Status = "Diproses",
                        Recipient = "Warga",
                        Message = "Laporan '{title}' sedang ditangani oleh {unit}."
                    },
                    new()
                    {
                        Status = "Selesai",
                        Recipient = "Warga",
                        Message = "Laporan '{title}' telah selesai. Terima kasih."
                    },
                    new()
                    {
                        Status = "Ditolak",
                        Recipient = "Warga",
                        Message = "Laporan '{title}' ditolak. Alasan: tidak sesuai domain."
                    }
                }
            };
        }

        public string GetUnit(
            string category,
            string severity)
        {
            return assignmentMatrix.Get(
                category,
                severity,
                "Unit Umum");
        }

        public int GetSLADays(
            string category,
            string severity)
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
            var template =
                _notificationConfig.Templates.FirstOrDefault(
                    t =>
                        t.Status.Equals(
                            status.ToString(),
                            StringComparison.OrdinalIgnoreCase)
                        &&
                        t.Recipient.Equals(
                            recipient,
                            StringComparison.OrdinalIgnoreCase));

            return template?.Message ?? string.Empty;
        }
    }
}