using TimCodes.Mtd.Vat.App.Services;
using TimCodes.Mtd.Vat.Core.Models;
using TimCodes.Mtd.Vat.Core.Models.Responses;
using TimCodes.Mtd.Vat.Core.OpenXml;

namespace TimCodes.Mtd.Vat.App.Forms
{
    public partial class VatReturnSelectionForm : Form
    {
        private readonly FormService _formService;

        public Obligation? Obligation { get; set; }

        public VatReturnSelectionForm(FormService formService)
        {
            InitializeComponent();

            if (FileDialog.ShowDialog() != DialogResult.OK)
            {
                Close();
                return;
            }

            LoadCells();
            _formService = formService;
        }

        private void LoadCells()
        {
            var rowCells = SpreadsheetMapper.MapRows(FileDialog.FileName);

            DataGridSpreadsheet.DataSource = rowCells;
        }

        private void DataGridSpreadsheet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var validatedRow = ValidatedVatRow.Validate((AccountingRow)DataGridSpreadsheet.Rows[e.RowIndex].DataBoundItem);
            if (validatedRow != null)
            {
                var form = _formService.GetForm<VatReturnConfirmationForm>();
                if (form == null)
                {
                    MessageBox.Show("Form not initialised");
                    return;
                }
                form.Obligation = Obligation;
                form.DataToSubmit = validatedRow;
                form.FillBoxes();
                form.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("The selected row is not valid");
            }
        }
    }
}
