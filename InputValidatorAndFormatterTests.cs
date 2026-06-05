using System;

namespace ERGOLAB_KPL
{
	public class InputValidatorAndFormatterTest
	{
		private readonly InputValidatorAndFormatter _helper = new InputValidatorAndFormatter();

		public void JalankanSemuaTest()
		{
			Console.WriteLine("=== Menjalankan Simulasi Validasi ===");

			bool testTelp = _helper.ValidateNoTelp("081234567890");
			Console.WriteLine($"Test No Telp (081234567890): {(testTelp ? "BERHASIL" : "GAGAL")}");

			bool testNik = _helper.ValidateNIK("3273012345678901");
			Console.WriteLine($"Test NIK (3273012345678901): {(testNik ? "BERHASIL" : "GAGAL")}");

			try
			{
				_helper.SamarkanNamaPelapor("");
				Console.WriteLine("Test Enkripsi Nama Kosong: GAGAL (Tidak melempar eror)");
			}
			catch (ArgumentException)
			{
				Console.WriteLine("Test Enkripsi Nama Kosong: BERHASIL (Eror tertangkap)");
			}

			Console.WriteLine("=====================================");
		}
	}
}