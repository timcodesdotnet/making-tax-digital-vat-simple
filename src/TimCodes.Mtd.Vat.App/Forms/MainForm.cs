using TimCodes.Mtd.Vat.App.Forms;
using TimCodes.Mtd.Vat.App.Services;
using TimCodes.Mtd.Vat.Core.Authorisation;
using TimCodes.Mtd.Vat.Core.Models.Responses;
using TimCodes.Mtd.Vat.Core.Services;

namespace TimCodes.Mtd.Vat.App
{
    public partial class MainForm : Form
    {
        private readonly AuthorisationProvider _authorisationProvider;
        private readonly FormService _formService;
        private readonly IVatService _vatService;

        private bool _isSignedIn = false;

        public MainForm(AuthorisationProvider authorisationProvider, FormService formService, IVatService vatService)
        {
            InitializeComponent();
            _authorisationProvider = authorisationProvider;
            _formService = formService;
            _vatService = vatService;
        }

        private async Task PopulateObligationsAsync()
        {
            var response = await _vatService.GetObligationsAsync(DateTime.Today.AddDays(-180), DateTime.Today);
            if (response?.WasSuccessful != true)
            {
                MessageBox.Show("Error getting obligations");
                return;
            }

            DataGridObligations.DataSource = response.Obligations?.OrderBy(q => q.Due).ToArray();
        }

        private async Task<bool> CheckSignInStatusAsync()
        {
            var token = await _authorisationProvider.GetAccessTokenAsync();

            if (token != null)
            {
                _isSignedIn = true;
                BtnToggleSignin.Text = "Sign Out";

                var name = await _vatService.GetBusinessName();
                LblSignedInAs.Visible = true;
                LblSignedInAs.Text = $"Signed in as {name}";

                await PopulateObligationsAsync();
                return true;
            } 
            else
            {
                _isSignedIn = false;
                BtnToggleSignin.Text = "Sign In";
                return false;
            }
        }

        private async Task SignInAsync()
        {
            using var authForm = _formService.GetForm<AuthorisationForm>();
            if (authForm is null) throw new InvalidOperationException($"{nameof(authForm)} could not be found");

            authForm.ShowDialog(this);

            await CheckSignInStatusAsync();
        }

        private async Task SignOutAsync()
        {
            await _authorisationProvider.ClearCacheAsync();
            _isSignedIn = false;
            LblSignedInAs.Visible = false;
            BtnToggleSignin.Text = "Sign In";
            DataGridObligations.DataSource = null;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            if (!await CheckSignInStatusAsync())
            {
                await SignInAsync();
            }
        }

        private async void BtnToggleSignin_Click(object sender, EventArgs e)
        {
            if (_isSignedIn)
            {
                await SignOutAsync();
            }
            else
            {
                await SignInAsync();
            }
        }

        private void DataGridObligations_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var item = (Obligation)DataGridObligations.Rows[e.RowIndex].DataBoundItem;
            if (item.Received.HasValue)
            {
                MessageBox.Show("This return has already been submitted");
            }
            else
            {
                var form = _formService.GetForm<VatReturnSelectionForm>();
                if (form == null)
                {
                    MessageBox.Show("Form not initialised");
                    return;
                }
                form.Obligation = item;
                form.ShowDialog(this);
            }
        }
    }
}