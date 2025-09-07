using System.Windows;
using SalesSuite.Services;
using QuestPDF.Infrastructure;   // ✅ استدعاء مكتبة QuestPDF

namespace SalesSuite
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            QuestPDF.Settings.License = LicenseType.Community;

            var auth = new AuthService();
            await auth.CreateDefaultAdminAsync();

            var login = new Views.LoginWindow();
            login.Show();
        }
    }
}
