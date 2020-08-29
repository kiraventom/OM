using AuthorizationDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        private List<User> DbAtLaunch;


        private void AdministratorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Users.Load();
            DbAtLaunch = _context.Users.Local.ToList(); // Save state of DB at launch to compare at closing
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
            bool dbChanged = !DbAtLaunch.SequenceEqual(UsersDG.ItemsSource.OfType<User>());
            if (!dbChanged)
            {
                return;
            }

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

        private void GenerateHashBt_Click(object sender, RoutedEventArgs e)
        {
            var hash =
                string.IsNullOrWhiteSpace(this.PasswordToHashPB.Password)
                ? string.Empty
                : Authorization.Other.Hasher.GetHash(this.PasswordToHashPB.Password);
            var cb = new TextCopy.Clipboard();
            cb.SetText(hash);
            this.HashCopiedPU.IsOpen = true;
        }
    }
}
