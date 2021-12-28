namespace TimCodes.Mtd.Vat.Core.Services
{
    public class FileUserIdService : IUserIdService
    {
        private readonly string _path;

        public FileUserIdService()
        {
            _path = Path.Combine(Directory.GetCurrentDirectory(), "userid.txt");
        }

        public string GetDeviceId()
        {
            if (!File.Exists(_path))
            {
                File.WriteAllText(_path, Guid.NewGuid().ToString());
            }
            return File.ReadAllText(_path);
        }
    }
}
