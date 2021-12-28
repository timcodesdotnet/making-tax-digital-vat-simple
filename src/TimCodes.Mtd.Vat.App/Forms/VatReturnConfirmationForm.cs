using TimCodes.Mtd.Vat.Core.Models.Requests;
using TimCodes.Mtd.Vat.Core.Models.Responses;

namespace TimCodes.Mtd.Vat.App.Forms
{
    public partial class VatReturnConfirmationForm : Form
    {
        public VatReturnRequest? DataToSubmit { get; set; }
        public Obligation? Obligation { get; internal set; }

        public VatReturnConfirmationForm()
        {
            InitializeComponent();
        }

        public void FillBoxes()
        {
            if (DataToSubmit == null || Obligation == null) return;

            LblPeriod.Text = $"{Obligation.PeriodKey} {Obligation.Start:MMM yyyy}-{Obligation.End:MMM yyyy}";
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

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (DataToSubmit == null || Obligation == null) return;

            DataToSubmit.Finalised = true;
            //TODO: Submit the return
        }
    }
}
