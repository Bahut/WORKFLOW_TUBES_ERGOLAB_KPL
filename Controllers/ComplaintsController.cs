using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WORKFLOW_TUBES_KPL_ERGOLAB.Models;
using WORKFLOW_TUBES_KPL_ERGOLAB.Core;
using WORKFLOW_TUBES_KPL_ERGOLAB.Data;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ComplaintsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Complaint>>> GetComplaints()
        {
            return await _context.Complaints.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Complaint>> GetComplaint(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);

            if (complaint == null)
                return NotFound(new { message = $"Data komplain ID {id} gak ketemu, pren!" });

            return complaint;
        }

        [HttpPost]
        public async Task<ActionResult<Complaint>> PostComplaint(Complaint complaint)
        {
            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();
                
            return Ok(new { message = "Data berhasil masuk Supabase, pren!", data = complaint });
        }

        [HttpPut("{id}/change-status")]
        public async Task<IActionResult> ChangeComplaintStatus(int id, [FromBody] ComplaintStatus newStatus)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null)
                return NotFound(new { message = "Komplain kagak ada!" });

            try
            {
                complaint.Status = newStatus;

                _context.Entry(complaint).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = $"Status komplain ID {id} berhasil diubah!", data = complaint });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null)
                return NotFound(new { message = "Barangnya emang kagak ada dari awal, pren!" });

            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Komplain ID {id} berhasil diapus dari awan Supabase!" });
        }
    }
}