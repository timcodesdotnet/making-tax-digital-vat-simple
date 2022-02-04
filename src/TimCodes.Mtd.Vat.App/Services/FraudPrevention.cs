using Microsoft.Win32;
using System.Text;
using TimCodes.Mtd.Vat.Core.Models.Requests;

namespace TimCodes.Mtd.Vat.App.Services;

public static class FraudPrevention
{
    public static FraudPreventionData GetFraudPrevention(Form form) => 
        new()
        {
            Window = GetWindow(form),
            Screens = GetScreens()
        };

    public static string GetWindow(Form form) => $"width={form.Size.Width}&height={form.Size.Height}";

    public static string GetScreens()
    {
        var sb = new StringBuilder();

        try
        {
            double scale = 1.0;
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop\\WindowMetrics"))
            {
                if (key != null)
                {
                    var o = key?.GetValue("AppliedDPI");
                    if (o != null)
                    {
                        var value = (int)o;
                        scale = value / 96.0;
                    }
                }
            }

            foreach (Screen? screen in Screen.AllScreens)
            {
                sb.Append($"width={screen.WorkingArea.Size.Width}&");
                sb.Append($"height={screen.WorkingArea.Size.Height}&");
                sb.Append($"scaling-factor={scale}&");
                sb.Append($"colour-depth={Screen.PrimaryScreen.BitsPerPixel}");
                sb.Append(',');
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
        catch (Exception ex) 
        {
            return "width=1920&height=1080&scaling-factor=1&colour-depth=32";
        }
    }
}
