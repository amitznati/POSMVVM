using POSEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POS_WPF.Views
{
    /// <summary>
    /// Interaction logic for EntityCollectionDataGrid.xaml
    /// </summary>
    public partial class EntityCollectionDataGrid : UserControl
    {
        public EntityCollectionDataGrid()
        {
            InitializeComponent();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var pd = (PropertyDescriptor)e.PropertyDescriptor;
            var da = (MyDisplayAttribute)pd.Attributes[typeof(MyDisplayAttribute)];

            if (da == null) return;

            var isVisible = da.IsVisible;
            if (!isVisible)
            {
                e.Cancel = true;
            }

            if (!string.IsNullOrEmpty(da.Name))
            {
                e.Column.Header = da.Name;
            }
        }
    }
}
