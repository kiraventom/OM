using MathModel;
using System;
using Microsoft.EntityFrameworkCore;
using SamplesDB;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using System.CodeDom;

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
            _samplesContext = new SamplesContext();
            this._samplesContext.Samples.Load();
            this.CalculateBt.Click += this.CalculateBt_Click;
            this.Loaded += this.UserWindow_Loaded;
        }

        ~UserWindow()
        {
            _samplesContext.Dispose();
        }

        private readonly SamplesContext _samplesContext;

        private static class GridBuilder
        {
            public static Grid Build(double G, double A, int N)
            {
                var paramsNames = new string[] { nameof(G), nameof(A), nameof(N) };
                var paramsValues = new object[] { G, A, N };

                Grid grid = new Grid();

                for (int i = 0; i < paramsNames.Length; ++i)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

                    var label = new Label()
                    {
                        Margin = new Thickness(0),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Content = paramsNames[i]
                    };
                    grid.Children.Add(label);
                    Grid.SetColumn(label, i * 2);

                    var textBox = new TextBlock()
                    {
                        Background = Brushes.Gray,
                        Foreground = Brushes.White,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Padding = new Thickness(3),
                        Text = paramsValues[i].ToString(),
                        Tag = paramsNames[i]
                    };
                    grid.Children.Add(textBox);
                    Grid.SetColumn(textBox, i * 2 + 1);
                }

                return grid;
            }
        }

        private void UserWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var grids = this._samplesContext.Samples.Local
                .AsEnumerable()
                .Select(s => GridBuilder.Build(s.ReactionMassConsumption,s.ReactorPressure, s.HeatExchangersAmount));
            this.SamplesLB.ItemsSource = grids;
        }

        private void CalculateBt_Click(object sender, RoutedEventArgs e)
        {
            if (SamplesLB.SelectedItem is null)
            {
                return;
            }
            else
            {
                double G = 0, A = 0;
                int N = 0;
                var grid = (Grid)SamplesLB.SelectedItem;
                var textBoxes = grid.Children.OfType<TextBlock>();

                // TODO: Fix
                G = double.Parse(textBoxes.Single(tb => tb.Tag.ToString() == nameof(G)).Text);
                A = double.Parse(textBoxes.Single(tb => tb.Tag.ToString() == nameof(A)).Text);
                N = int.Parse(textBoxes.Single(tb => tb.Tag.ToString() == nameof(N)).Text);

                var result = Solver.Solve(G, A, N);

                Temperature1TB.Text = result.Temperature1.ToString();
                Temperature2TB.Text = result.Temperature2.ToString();
                ProfitTB.Text = result.Profit.ToString();
            }
        }
    }
}
