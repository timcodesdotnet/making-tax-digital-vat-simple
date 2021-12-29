using Microsoft.Extensions.Options;
using OtpNet;
using System.Security.Cryptography;
using System.Text;
using TimCodes.Mtd.Vat.Core.Authorisation;
using TimCodes.Mtd.Vat.Core.Configuration;

namespace TimCodes.Mtd.Vat.App.Forms
{
    public partial class MfaForm : Form
    {
        private readonly MtdOptions _options;

        public MfaForm(IOptions<MtdOptions> options)
        {
            InitializeComponent();
            _options = options.Value;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            var key = Base32Encoding.ToBytes(_options.MfaKey);
            var totp = new Totp(key);

            if (totp.VerifyTotp(DateTime.UtcNow, TxtMfa.Text, out var _))
            {
                MfaTracker.LastChecked = DateTime.UtcNow;
                var message = Encoding.UTF8.GetBytes(_options.MfaKey.Substring(0, 5));
                using (var alg = SHA1.Create())
                {
                    string hex = string.Empty;

                    var hashValue = alg.ComputeHash(message);
                    foreach (byte x in hashValue)
                    {
                        hex += string.Format("{0:x2}", x);
                    }
                    MfaTracker.Identifier = hex;
                }
                Close();
            }
            else
            {
                MessageBox.Show("Incorrect code");
            }
        }

        private void TxtMfa_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnConfirm_Click(sender, e);
            }
        }
    }
}
