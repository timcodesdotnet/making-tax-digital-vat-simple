namespace TimCodes.Mtd.Vat.Core.Authorisation
{
    public interface ITokenCache
    {
        Task ClearAsync();
        Task StoreTokenAsync(AccessToken token);
        Task<bool> TryGetAsync(out AccessToken? token);
    }
}
