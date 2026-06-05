using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace ERGOLAB_KPL
{
    public class InputValidatorAndFormatter
    {
        private static readonly Regex PhoneRegex = new Regex(@"^(08|\+628)[0-9]{8,11}$", RegexOptions.Compiled);
        private static readonly Regex NikRegex = new Regex(@"^[0-9]{16}$", RegexOptions.Compiled);

        public bool ValidateNoTelp(string noTelp)
        {
            if (string.IsNullOrWhiteSpace(noTelp)) return false;
            return PhoneRegex.IsMatch(noTelp.Trim());
        }

        public bool ValidateNIK(string nik)
        {
            if (string.IsNullOrWhiteSpace(nik)) return false;
            return NikRegex.IsMatch(nik.Trim());
        }

        public string SamarkanNamaPelapor(string namaOriginal)
        {
            if (string.IsNullOrWhiteSpace(namaOriginal))
            {
                throw new ArgumentException("Nama pelapor tidak boleh kosong.", nameof(namaOriginal));
            }

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(namaOriginal.Trim());
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                string namaSamaran = "WARGA-" + sb.ToString().Substring(0, 8);

                if (string.IsNullOrEmpty(namaSamaran))
                {
                    throw new InvalidOperationException("Gagal enkripsi nama.");
                }

                return namaSamaran;
            }
        }
    }
}