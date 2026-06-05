using Microsoft.VisualStudio.TestTools.UnitTesting;
using WORKFLOW_TUBES_KPL_ERGOLAB.Core;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Testing
{
	[TestClass]
	public class TableDrivenUnitTest
	{
		[TestMethod]
		public void TestAssignmentMatrix()
		{
			ComplaintSlaTable rules = new ComplaintSlaTable();

			string unit = rules.GetUnit("Infrastruktur", "Berat");

			Assert.AreEqual("Unit Infrastruktur", unit);
		}

		[TestMethod]
		public void TestSLAMatrix()
		{
			ComplaintSlaTable rules = new ComplaintSlaTable();

			int sla = rules.GetSLADays("Keamanan", "Sedang");

			Assert.AreEqual(1, sla);
		}

		[TestMethod]
		public void TestEscalationMatrix()
		{
			ComplaintSlaTable rules = new ComplaintSlaTable();

			string escalation = rules.CheckEscalation(ComplaintStatus.Diverifikasi, 3);

			Assert.IsTrue(escalation.Contains("Lurah"));
		}

		[TestMethod]
		public void TestNotificationMatrix()
		{
			ComplaintSlaTable rules = new ComplaintSlaTable();

			string notification = rules.GetNotificationTemplate(ComplaintStatus.Diajukan, "Warga");

			Assert.IsFalse(string.IsNullOrWhiteSpace(notification));
		}
	}
}