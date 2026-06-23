using System;
using WORKFLOW_TUBES_KPL_ERGOLAB.Core;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Reporter { get; set; }

        public ComplaintStatus Status { get; set; }

        public Complaint(
            string title,
            string category,
            string description,
            string location,
            string reporter)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Judul tidak boleh kosong");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Deskripsi tidak boleh kosong");

            Title = title;
            Category = category;
            Description = description;
            Location = location;
            Reporter = reporter;

            Status = ComplaintStatus.Diajukan;
        }

        public Complaint() { }
    }
}