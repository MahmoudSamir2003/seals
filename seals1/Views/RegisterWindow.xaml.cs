using System;
using System.Windows;

namespace SalesSuite.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly Services.AuthService auth = new();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private async void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserBox.Text) ||
                string.IsNullOrWhiteSpace(Pass1.Password) ||
                string.IsNullOrWhiteSpace(Pass2.Password))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (Pass1.Password != Pass2.Password)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                await auth.RegisterAsync(UserBox.Text.Trim(), Pass1.Password, "User");
                MessageBox.Show("Registration successful. You can login now.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}
