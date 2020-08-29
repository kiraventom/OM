using Authorization;
using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var authorizer = new Authorizer();
            var auth = new AuthorizationWindow(authorizer);
            auth.ShowDialog();
            var user = authorizer.AuthorizedUser;
            if (user is null)
            {
                this.Close();
                return;
            }

            Window window = user.Level switch
            {
                AuthorizationDB.User.AccessLevel.User => new UserWindow(),
                AuthorizationDB.User.AccessLevel.Admin => new AdministratorWindow(),
                _ => throw new System.ArgumentOutOfRangeException(nameof(user.Level), user.Level, Properties.Resources.UserLevelDoesNotExist),
            };

            window.ShowDialog();

            this.Close();
        }
    }
}
