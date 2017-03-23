using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataGridFilterTest
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            if (MyObjectDataProvider != null) MyObjectDataProvider.ObjectInstance = TestData.TestDataGenerator.Instance;

            this.DataContext = MyObjectDataProvider;

            //this.DataContext = TestData.TestDataGenerator.Instance;

            TestData.TestDataGenerator.Instance.GenerateTestData(null);
        }

        private ObjectDataProvider MyObjectDataProvider
        {
            get
            {
               return this.TryFindResource("EmployeeData") as ObjectDataProvider;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TestData.TestDataGenerator.Instance.GenerateTestData(null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            double newSize;

            if (Double.TryParse(this.txtFontSize.Text, out newSize))
            {
                this.FontSize = newSize;
            }
            
        }


        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        private void Button_Click_Insert_New_Position(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button.Content.ToString() == "Start inserting Employees with new position")
            {
                timer.Interval = new TimeSpan(0, 0, 1);

                timer.Tick += delegate(object senderTimer, EventArgs timerTickEvent)
                {
                    TestData.TestDataGenerator.Instance.InsertNewEmployeeWithNewPosition();
                };

                button.Content = "Stop inserting Employees";
                button.Background = Brushes.Red;

                timer.Start();
            }
            else
            {
                timer.Stop();
                button.Content = "Start inserting Employees with new position";
                button.Background = Brushes.Transparent;
            }
        }
    }
}
