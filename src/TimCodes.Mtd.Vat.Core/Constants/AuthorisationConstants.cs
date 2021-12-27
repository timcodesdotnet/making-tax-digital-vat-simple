namespace TimCodes.Mtd.Vat.Core.Constants
{
    public class AuthorisationConstants
    {

        /// <summary>
        /// The authorization_code is rendered in the title of a HTML page where you can parse the DOM to retrieve the code. 
        /// You can then programmatically close the window before the user sees the rendered web page.
        /// </summary>
        public const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
    }
}
