using Authorization;
using Authorization.Other;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow(Authorizer authorizer)
        {
            InitializeComponent();
            this._authorizer = authorizer;
            this.LoginBt.Click += LoginBt_Click;
        }

        private readonly Authorizer _authorizer;

        private void LoginBt_Click(object sender, RoutedEventArgs e)
        {
            var creds = new Credentials(LoginTB.Text.ToLower().Trim(), PasswordTB.Password.Trim());
            var u = _authorizer.Authorize(creds);
            if (u != null)
            {
                this.LoginTB.Clear();
                this.PasswordTB.Clear();
                this.Close();
            }
            else
            {
                MessageBox.Show(Properties.Resources.WrongLoginOrPassword);
            }
        }
    }
}
