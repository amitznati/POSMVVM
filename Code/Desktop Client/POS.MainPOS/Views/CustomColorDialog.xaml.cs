using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POS.MainPOS.Views
{
    /// <summary>
    /// Interaction logic for CustomColorDialog.xaml
    /// </summary>
    public partial class CustomColorDialog : Window
    {
        public CustomColorDialog()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        public string Color
        {
            get { return this.colorCanvas.HexadecimalString; }
        }
    }
}
