using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TimCodes.Mtd.Vat.App.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddAllScoped<TParent>(this ServiceCollection services)
        {
            var allTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(q =>
                    q.IsClass &&
                    !q.IsAbstract &&
                    typeof(TParent).IsAssignableFrom(q));

            foreach (var type in allTypes)
            {
                services.AddTransient(type);
            }
        }
    }
}
