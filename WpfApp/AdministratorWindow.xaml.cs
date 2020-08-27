using AuthorizationDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        public AdministratorWindow()
        {
            InitializeComponent();
            this._context = new AuthorizationContext();
            this.Loaded += this.AdministratorWindow_Loaded;
            this.UsersDG.AutoGeneratingColumn += this.UsersDG_AutoGeneratingColumn;
            this.Closing += this.AdministratorWindow_Closing;
        }

        ~AdministratorWindow()
        {
            this._context.Dispose();
        }

        private readonly AuthorizationContext _context;

        private void AdministratorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Users.Load();
            UsersDG.ItemsSource = _context.Users.Local.ToObservableCollection();
        }

        private void UsersDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                e.Column.IsReadOnly = true;
            }
        }

        private void AdministratorWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dr = MessageBox.Show("Сохранить изменения?", "Подтверджение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (dr)
            {
                case MessageBoxResult.Yes:
                    _context.SaveChanges();
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }
    }
}
