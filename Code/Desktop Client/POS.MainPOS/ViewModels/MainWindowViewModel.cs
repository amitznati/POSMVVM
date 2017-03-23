using MahApps.Metro;
using POS.Data.Repository;
using POS.Windows;
using POSEntities.Entities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace POS.MainPOS.ViewModels
{
    public class AccentColorMenuData
    {
        public string Name { get; set; }
        public Brush BorderColorBrush { get; set; }
        public Brush ColorBrush { get; set; }

        private DelegateCommand changeAccentCommand;

        public DelegateCommand ChangeAccentCommand
        {
            get { return this.changeAccentCommand ?? (changeAccentCommand = new DelegateCommand(DoChangeTheme)); }
        }

        protected virtual void DoChangeTheme()
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
        }
    }
    public class MainWindowViewModel:BindableViewModelBase
    {
        public MainWindowViewModel()
        {
            MainPOSViewModel = new MainPOSViewModel();
            this.AccentColors = ThemeManager.Accents
                                            .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                                            .ToList();
        }
        public MainPOSViewModel MainPOSViewModel { get; set; }
        public List<AccentColorMenuData> AccentColors { get; set; }
    }
}
