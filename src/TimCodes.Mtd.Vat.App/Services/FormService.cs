namespace TimCodes.Mtd.Vat.App.Services
{
    public class FormService
    {
        private readonly IServiceProvider _serviceProvider;

        public FormService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T? GetForm<T>()
            where T : Form
        {
            return _serviceProvider.GetService(typeof(T)) as T;
        }
    }
}
