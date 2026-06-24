using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using WORKFLOW_TUBES_KPL_ERGOLAB.Core;

namespace ERGOLAB_KPL
{
    public class RegexRule : ValidationRule<string>
    {
        private readonly Regex _regex;
        private readonly string _defaultErrorMessage;

        public RegexRule(Regex regex, string defaultErrorMessage)
        {
            _regex = regex ?? throw new ArgumentNullException(nameof(regex));
            _defaultErrorMessage = defaultErrorMessage;
        }

        public override bool Validate(string input, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(input) || !_regex.IsMatch(input.Trim()))
            {
                errorMessage = _defaultErrorMessage;
                return false;
            }
            errorMessage = "";
            return true;
        }
    }

    public class InputValidatorAndFormatter
    {
        private static readonly Regex PhoneRegex = new Regex(@"^(08|\+628)[0-9]{8,11}$", RegexOptions.Compiled);
        private static readonly Regex NikRegex = new Regex(@"^[0-9]{16}$", RegexOptions.Compiled);

        public bool ValidateNoTelp(string noTelp)
        {
            var rules = new List<ValidationRule<string>>
            {
                new RequiredStringRule(),
                new RegexRule(PhoneRegex, "Format nomor telepon tidak valid.")
            };

            var validator = new RequiredStringRule();
            return validator.ValidateAll(noTelp, rules, out _);
        }

        public bool ValidateNIK(string nik)
        {
            var rules = new List<ValidationRule<string>>
            {
                new RequiredStringRule(),
                new RegexRule(NikRegex, "Format NIK tidak valid.")
            };

            var validator = new RequiredStringRule();
            return validator.ValidateAll(nik, rules, out _);
        }

        public string SamarkanNamaPelapor(string namaOriginal)
        {
            if (string.IsNullOrWhiteSpace(namaOriginal))
            {
                throw new ArgumentException("Nama pelapor tidak boleh kosong.", nameof(namaOriginal));
            }

            byte[] inputBytes = Encoding.UTF8.GetBytes(namaOriginal.Trim());
            byte[] hashBytes = SHA256.HashData(inputBytes);
            string hexHash = Convert.ToHexString(hashBytes);

            return $"WARGA-{hexHash[..8]}";
        }
    }
}