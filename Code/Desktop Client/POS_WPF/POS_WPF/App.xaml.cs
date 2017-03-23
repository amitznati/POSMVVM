using POS_WPF.Views;
using System.Windows;

namespace POS_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App() { 
         System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("he");
         //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en"); 
      }
    
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow window = new MainWindow();
            window.Show();
            //Bootstrapper bootstrapper = new Bootstrapper();
            //bootstrapper.Run();
        }
    }
}
