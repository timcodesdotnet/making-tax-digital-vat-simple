using TimCodes.Mtd.Vat.Core.Models.Requests;
using TimCodes.Mtd.Vat.Core.Models.Responses;
using TimCodes.Mtd.Vat.Core.Services;

namespace TimCodes.Mtd.Vat.App.Forms
{
    public partial class VatReturnConfirmationForm : Form
    {
        private readonly IVatService _vatService;

        public VatReturnRequest? DataToSubmit { get; set; }
        public Obligation? Obligation { get; internal set; }

        public VatReturnConfirmationForm(IVatService vatService)
        {
            InitializeComponent();
            _vatService = vatService;
        }

        public void FillBoxes()
        {
            if (DataToSubmit == null || Obligation == null) return;

            LblPeriod.Text = $"{Obligation.Start:MMM yyyy}-{Obligation.End:MMM yyyy}";
            LblBox1.Text = DataToSubmit.VatDueSales.ToString("C2");
            LblBox2.Text = DataToSubmit.VatDueAcquisitions.ToString("C2");
            LblBox3.Text = DataToSubmit.TotalVatDue.ToString("C2");
            LblBox4.Text = DataToSubmit.VatReclaimedCurrPeriod.ToString("C2");
            LblBox5.Text = DataToSubmit.NetVatDue.ToString("C2");
            LblBox6.Text = DataToSubmit.TotalValueSalesExVAT.ToString("C2");
            LblBox7.Text = DataToSubmit.TotalValuePurchasesExVAT.ToString("C2");
            LblBox8.Text = DataToSubmit.TotalValueGoodsSuppliedExVAT.ToString("C2");
            LblBox9.Text = DataToSubmit.TotalAcquisitionsExVAT.ToString("C2");
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (DataToSubmit == null || Obligation == null) return;

            DataToSubmit.PeriodKey = Obligation.PeriodKey;
            DataToSubmit.Finalised = true;

            var response = await _vatService.SubmitVatReturnAsync(DataToSubmit);
            if (response != null)
            {
                if (response.WasSuccessful)
                {
                    MessageBox.Show("VAT return submitted successfully");
                    Close();
                }
                else
                {
                    MessageBox.Show($"VAT return failed to submit {response.Message}");
                }
            }
            else
            {
                MessageBox.Show("No response received");
            }
        }
    }
}
