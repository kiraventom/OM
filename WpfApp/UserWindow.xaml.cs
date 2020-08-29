using MathModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
        }

        private void CalculateBt_Click(object sender, RoutedEventArgs e)
        {
            var result = Solver.Solve();
            Temperature1TB.Text = result.Item1.ToString();
            Temperature2TB.Text = result.Item2.ToString();
        }
    }
}
