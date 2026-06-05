using WORKFLOW_TUBES_KPL_ERGOLAB.Core;
using WORKFLOW_TUBES_KPL_ERGOLAB.Models;
using WORKFLOW_TUBES_KPL_ERGOLAB.Testing;
using WORKFLOW_TUBES_KPL_ERGOLAB.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WORKFLOW_TUBES_KPL_ERGOLAB v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("=== LurahOnline (ErgoLab Version) ===");
Console.WriteLine("Pengelolaan Status Komplain\n");

Complaint complaint = new Complaint("Jalan Rusak", "Infrastruktur", "Jalan berlubang", "Jl. Merdeka No.10", "warga123");
ComplaintWorkflow workflow = new ComplaintWorkflow();

Console.WriteLine($"Status Awal     : {complaint.Status}");
workflow.ChangeStatus(complaint, "verify");
Console.WriteLine($"Status Sekarang : {complaint.Status}");

Console.WriteLine("\n=== Menjalankan Performance Test ===");
PerformanceTest.Run();
Console.WriteLine("\nSimulasi program selesai.\n");

app.Run();