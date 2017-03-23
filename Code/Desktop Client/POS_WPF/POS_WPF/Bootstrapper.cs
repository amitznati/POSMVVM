using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Unity;
using Microsoft.Practices.Unity;
using System.Windows;
using POS_WPF;
using POS_WPF.Views;


namespace PrismTest
{
    class Bootstrapper : UnityBootstrapper
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

            //Container.ReqisterTypeForNavigation<MainPOSView>("MainPOSView");
            
        }
    }

    public static class UnityExtensions
    {
        public static void ReqisterTypeForNavigation<T>(this IUnityContainer container, string name)
        {
            container.RegisterType(typeof(object), typeof(T), name);
        }
    }
}
