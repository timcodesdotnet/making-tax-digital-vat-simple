using Microsoft.Extensions.Options;
using TimCodes.Mtd.Vat.Core.Authorisation;
using TimCodes.Mtd.Vat.Core.Configuration;
using TimCodes.Mtd.Vat.Core.Constants;

namespace TimCodes.Mtd.Vat.App
{
    public partial class AuthorisationForm : Form
    {
        private const string SuccessAuthCodePrefix = "Success code=";

        private readonly MtdOptions _options;
        private readonly AuthorisationProvider _authorisationProvider;

        public AuthorisationForm(IOptions<MtdOptions> options, AuthorisationProvider authorisationProvider)
        {
            _options = options.Value;

            InitializeComponent();

            var uriBuilder = new UriBuilder(_options.AuthUri);
            uriBuilder.Query = $"?response_type=code&scope={_options.Scope}&client_id={_options.ClientId}&redirect_uri={AuthorisationConstants.RedirectUri}";
            WebSignIn.Source = uriBuilder.Uri;
            _authorisationProvider = authorisationProvider;
        }

        private async void WebSignIn_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                if (WebSignIn.CoreWebView2.DocumentTitle.StartsWith(SuccessAuthCodePrefix))
                {
                    var authCode = WebSignIn.CoreWebView2.DocumentTitle.Replace(SuccessAuthCodePrefix, string.Empty);
                    await _authorisationProvider.GetAccessTokenAsync(authCode);
                    WebSignIn.Dispose();

                    Close();
                }
            }
        }
    }
}
