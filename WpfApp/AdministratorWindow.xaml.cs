using AuthorizationDB;
using Microsoft.EntityFrameworkCore;
using SamplesDB;
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
            this._usersContext = new AuthorizationContext();
            this._samplesContext = new SamplesContext();
            this.Loaded += this.AdministratorWindow_Loaded;
            this.UsersDG.AutoGeneratingColumn += this.DataGrid_AutoGeneratingColumn;
            this.SamplesDG.AutoGeneratingColumn += this.DataGrid_AutoGeneratingColumn;
            this.Closing += this.AdministratorWindow_Closing;
        }

        ~AdministratorWindow()
        {
            this._usersContext.Dispose();
            this._samplesContext.Dispose();
        }

        private readonly AuthorizationContext _usersContext;
        private readonly SamplesContext _samplesContext;
        private List<User> UsersDbAtLaunch;
        private List<Sample> SamplesDbAtLaunch;


        private void AdministratorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _usersContext.Users.Load();
            UsersDbAtLaunch = _usersContext.Users.Local.ToList(); // Save state of DB at launch to compare at closing
            UsersDG.ItemsSource = _usersContext.Users.Local.ToObservableCollection();

            _samplesContext.Samples.Load();
            SamplesDbAtLaunch = _samplesContext.Samples.Local.ToList(); // Save state of DB at launch to compare at closing
            SamplesDG.ItemsSource = _samplesContext.Samples.Local.ToObservableCollection();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                e.Column.IsReadOnly = true;
            }
        }

        private void AdministratorWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool dbChanged = 
                !UsersDbAtLaunch.SequenceEqual(UsersDG.ItemsSource.OfType<User>()) ||
                !SamplesDbAtLaunch.SequenceEqual(SamplesDG.ItemsSource.OfType<Sample>());
            if (!dbChanged)
            {
                return;
            }

            var dr = MessageBox.Show(Properties.Resources.SaveChangesQuestion, Properties.Resources.Confirmation, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (dr)
            {
                case MessageBoxResult.Yes:
                    _usersContext.SaveChanges();
                    _samplesContext.SaveChanges();
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
