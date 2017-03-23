
using MahApps.Metro.Controls;
using POS.Data.Repository;
using POS.DesktopClient.ViewModels;
using System;
using System.Windows;

namespace POS.DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       
    }
}
