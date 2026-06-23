using System;
using System.Collections.Generic;
using System.Linq;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Core
{
    public class ComplaintSlaTable
    {
        private readonly NotificationConfig _notificationConfig;

        private readonly Dictionary<(string, string), string> assignmentMatrix =
            new()
            {
                { ("infrastruktur", "berat"), "Unit Infrastruktur" },
                { ("keamanan", "sedang"), "Unit Keamanan" }
            };

        private readonly Dictionary<(string, string), int> slaMatrix =
            new();

        // FIX: Constructor utama — dipakai Program.cs via DI
        public ComplaintSlaTable(
            SlaConfig slaConfig,
            NotificationConfig notificationConfig)
        {
            _notificationConfig = notificationConfig;

            foreach (var rule in slaConfig.Rules)
            {
                slaMatrix[
                    (
                        rule.Category.ToLower(),
                        rule.Impact.ToLower()
                    )
                ] = rule.MaxDays;
            }
        }

        // FIX: Constructor kosong untuk unit test — pakai data hardcoded default
        public ComplaintSlaTable()
        {
            // Default SLA matrix (sama dengan isi sla_rules.json)
            slaMatrix[("kebersihan", "ringan")] = 3;
            slaMatrix[("keamanan", "sedang")] = 1;
            slaMatrix[("infrastruktur", "berat")] = 1;
            slaMatrix[("administrasi", "ringan")] = 3;
            slaMatrix[("umum", "sedang")] = 2;

            // Default notification config (sama dengan notification_templates.json)
            _notificationConfig = new NotificationConfig
            {
                Templates = new List<NotificationTemplate>
                {
                    new() { Status = "Diajukan",    Recipient = "Warga",   Message = "Laporan '{title}' telah diterima. Menunggu verifikasi." },
                    new() { Status = "Diverifikasi",Recipient = "Petugas", Message = "Laporan '{title}' perlu ditindaklanjuti segera." },
                    new() { Status = "Diproses",    Recipient = "Warga",   Message = "Laporan '{title}' sedang ditangani oleh {unit}." },
                    new() { Status = "Selesai",     Recipient = "Warga",   Message = "Laporan '{title}' telah selesai. Terima kasih." },
                    new() { Status = "Ditolak",     Recipient = "Warga",   Message = "Laporan '{title}' ditolak. Alasan: tidak sesuai domain." }
                }
            };
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
            var template = _notificationConfig.Templates.FirstOrDefault(t =>
                t.Status.Equals(status.ToString(), StringComparison.OrdinalIgnoreCase)
                &&
                t.Recipient.Equals(recipient, StringComparison.OrdinalIgnoreCase));

            return template?.Message ?? string.Empty;
        }

        public string GetUnit(
            string category,
            string severity)
        {
            var key =
            (
                category?.ToLower() ?? "",
                severity?.ToLower() ?? ""
            );

            return assignmentMatrix.TryGetValue(
                key,
                out string? unit)
                ? unit
                : "Unit Umum";
        }

        public int GetSLADays(
            string category,
            string severity)
        {
            var key =
            (
                category?.ToLower() ?? "",
                severity?.ToLower() ?? ""
            );

            return slaMatrix.TryGetValue(
                key,
                out int days)
                ? days
                : 7;
        }
    }
}