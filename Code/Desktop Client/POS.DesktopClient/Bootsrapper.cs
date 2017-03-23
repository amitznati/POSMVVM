using POS.DesktopClient.Views;
using Prism.Unity;
using System.Windows;
using Microsoft.Practices.Unity;

namespace POS.DesktopClient
{
    public class Bootsrapper:UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }
    }
    public static class UnityExtensions
    {
        public static void RegisterTypeForNavigation<T>(this IUnityContainer container,string name)
        {
            container.RegisterType(typeof(object), typeof(T), name);
        }
    }
    
}
