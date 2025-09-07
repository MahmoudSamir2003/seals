using System.Windows;

namespace SalesSuite.Views
{
    public partial class LoginWindow : Window
    {
        private readonly Services.AuthService auth = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var user = await auth.LoginAsync(UsernameBox.Text.Trim(), PasswordBox.Password);
            if (user == null)
            {
                MessageBox.Show("Invalid username or password.");
                return;
            }

            new MainWindow(user).Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().ShowDialog();
        }
    }
}
