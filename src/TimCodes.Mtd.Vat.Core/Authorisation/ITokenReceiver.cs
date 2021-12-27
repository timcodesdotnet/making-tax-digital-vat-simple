namespace TimCodes.Mtd.Vat.Core.Authorisation
{
    public interface ITokenReceiver
    {
        Task<AccessToken?> GetAccessTokenAsync(string code);
        Task<AccessToken?> RefreshAccessTokenAsync(string refreshToken);
    }
}
