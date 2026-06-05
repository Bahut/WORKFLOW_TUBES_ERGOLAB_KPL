using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WORKFLOW_TUBES_KPL_ERGOLAB.Core;
using WORKFLOW_TUBES_KPL_ERGOLAB.Models;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Testing
{
    [TestClass]
    public class ComplaintWorkflowTests
    {
        [TestMethod]
        public void ChangeStatus_VerifyAction_ShouldUpdateStatusToDiverifikasi()
        {
            ComplaintWorkflow workflow = new ComplaintWorkflow();
            Complaint complaint = new Complaint("Lampu Jalan Mati", "Listrik", "Lampu mati", "Jl. Asia Afrika", "Budi");

            workflow.ChangeStatus(complaint, "verify");

            Assert.AreEqual(ComplaintStatus.Diverifikasi, complaint.Status);
        }

        [TestMethod]
        public void ChangeStatus_InvalidTransition_ShouldThrowException()
        {
            ComplaintWorkflow workflow = new ComplaintWorkflow();
            Complaint complaint = new Complaint("Judul", "Kategori", "Deskripsi", "Lokasi", "Reporter");

            try
            {
                workflow.ChangeStatus(complaint, "finish");
                Assert.Fail("Harusnya melempar InvalidOperationException");
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}